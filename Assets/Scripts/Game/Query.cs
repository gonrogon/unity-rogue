using Rogue.Core;
using Rogue.Coe;
using Rogue.Map;
using GG.Mathe;
using System;
using System.Collections.Generic;

namespace Rogue.Game
{
    public struct QueryResult<T>
    {
        public static QueryResult<T> Empty => default;

        /// <summary>
        /// Flag indicating whether the result is good or not.
        /// </summary>
        public readonly bool ok;

        /// <summary>
        /// Value.
        /// </summary>
        public readonly T value;

        public QueryResult(T value)
        {
            this.ok    = true;
            this.value = value;
        }

        public static implicit operator bool(QueryResult<T> result)
        {
            return result.ok;
        }
    }

    public static partial class Query
    {
        /// <summary>
        /// Defines an array with the distance to the neighbour of a coordinate in the map.
        /// </summary>
        private static readonly Vec2i[] Neighbour8 =
        {
            new Vec2i(-1, -1),
            new Vec2i(-1,  0),
            new Vec2i(-1,  1),
            new Vec2i( 0,  1),
            new Vec2i( 1,  1),
            new Vec2i( 1,  0),
            new Vec2i( 1, -1),
            new Vec2i( 0, -1)
        };

        // --------
        // ENTITIES
        // --------

        public static bool Is(Ident entity, TagType tags)
        {
            var cTag = Context.World.Find(entity)?.FindFirstComponent<Comp.Tag>();
            if (cTag != null)
            {
                return cTag.ContainsAll(tags);
            }

            return false;
        }

        public static bool IsOneOf(Ident entity, TagType tags)
        {
            var cTag = Context.World.Find(entity)?.FindFirstComponent<Comp.Tag>();
            if (cTag != null)
            {
                return cTag.ContainsOne(tags);
            }

            return false;
        }

        public static bool IsPlayer(Ident entity)
        {
            return Is(entity, TagType.Player);
        }

        public static bool IsEnemy(Ident entity)
        {
            return Is(entity, TagType.Enemy);
        }

        public static bool IsWeapon(Ident entity)
        {
            return IsOneOf(entity, TagType.MeleeWeapon | TagType.RangeWeapon);
        }

        public static bool IsMelee(Ident entity)
        {
            return Is(entity, TagType.MeleeWeapon);
        }

        public static bool IsRange(Ident entity)
        {
            return Is(entity, TagType.RangeWeapon);
        }

        public static bool IsDoor(Ident entity)
        {
            return Is(entity, TagType.Door);
        }

        public static bool IsPickable(Ident entity, Ident who)
        {
            return Context.World.Find(entity)?.FindFirstComponent<Comp.Pickable>() != null;
        }

        public static bool IsBuildable(Ident entity, Ident who)
        {
            GameEntity e = Context.World.Find(entity);
            if (e == null)
            {
                return false;
            }

            return e.FindFirstComponent<Comp.Terrain>() != null || e.FindFirstComponent<Comp.Scaffold>() != null;
        }

        public static bool IsCultivable(Ident entity, Ident who)
        {
            GameEntity e = Context.World.Find(entity);
            if (e == null)
            {
                return false;
            }

            return e.FindFirstComponent<Comp.Crop>() != null;
        }

        public static bool IsOpaque(Ident entity, Ident who)
        {
            return false;
        }

        public static bool IsPassable(Ident entity, Ident who)
        {
            var cBlock = Context.World.Find(entity)?.FindFirstComponent<Comp.Block>();
            var cTag   = Context.World.Find(who)?.FindFirstComponent<Comp.Tag>();

            if ((cBlock is null) || (cBlock.enabled == false) || (cTag is null && cBlock.tags != TagType.All))
            {
                return true;
            }

            if (cTag is null)
            {
                return false;
            }

            return !cBlock.Blocked(cTag.tags);
        }

        public static QueryResult<Ident> GetWieldMeleeWeapon(Ident entity)
        {
            return GetWieldItem(entity, IsMelee);
        }

        public static QueryResult<Ident> GetWieldRangeWeapon(Ident entity)
        {
            return GetWieldItem(entity, IsRange);
        }

        public static QueryResult<Ident> GetWieldItem(Ident entity, Func<Ident, bool> pred)
        {
            var cBody = Context.World.Find(entity)?.FindFirstComponent<Comp.Body>();

            if (cBody == null || cBody.bid.IsZero) 
            {
                return QueryResult<Ident>.Empty;
            }

            for (int i = 0; Context.Bodies.Get(cBody.bid).TryGetWield(i, out Ident wield); i++)
            {
                if (pred(wield))
                {
                    return new QueryResult<Ident>(wield);
                }
            }

            return QueryResult<Ident>.Empty;
        }

        public static QueryResult<string> GetName(Ident entity)
        {
            return new QueryResult<string>(Context.World.Send(entity, new Msg.Name()).name);
        }

        public static QueryResult<Vec2i> GetPosition(Ident entity)
        {
            var cLoc = Context.World.Find(entity)?.FindFirstComponent<Comp.Location>();
            if (cLoc is null)
            {
                return QueryResult<Vec2i>.Empty;
            }

            return new QueryResult<Vec2i>(cLoc.position);
        }

        // ---
        // MAP
        // ---

        public static bool MapTryGetRandomPassableSpot(Vec2i origin, Ident who, out Vec2i result)
        {
            int start = UnityEngine.Random.Range(0, 8);

            for (int i = 0; i < 8; i++)
            {
                Vec2i coord = GetNeighbour(origin, start + i);

                if (MapIsPassable(coord, who))
                {
                    result = coord;
                    return true;
                }
            }

            result = Vec2i.Zero;
            return false;
        }

        public static bool MapIsVisible(Vec2i origin, Vec2i target, Ident who)
        {
            Segment2i seg = new Segment2i(origin, target);

            foreach (Vec2i cell in seg)
            {
                if (Context.Map.IsSolid(cell))
                {
                    return false;
                }

                bool opaque = false;

                Context.Map.ForEach(cell, eid => 
                {
                    if (IsOpaque(eid, who))
                    { 
                        opaque = true;
                    }
                });

                if (opaque)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool MapIsPassable(Vec2i coord, Ident who)
        {
            if (Context.Map.IsSolid(coord))
            {
                return false;
            }

            return !Context.Map.TryFindFirst(coord, (Ident eid) => { return !IsPassable(eid, who); }, out _);
        }

        public static bool MapIsStuck(Vec2i coord, Ident who)
        {
            Rect2i rect = new Rect2i(coord - Vec2i.One, 3, 3);

            foreach (Vec2i p in rect)
            {
                if (p == coord)
                {
                    continue;
                }

                if (MapIsPassable(p, who))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool MapIsStuck(Vec2i coord, Ident who, Vec2i solid)
        {
            Rect2i rect = new Rect2i(coord - Vec2i.One, 3, 3);

            foreach (Vec2i p in rect)
            {
                if (p == coord || p == solid)
                {
                    continue;
                }

                if (MapIsPassable(p, who))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool MapIsEmpty(Vec2i coord)
        {
            return Context.Map.Empty(coord);
        }

        public static bool MapIsDropAllow(Vec2i coord, Ident who) => MapIsPassable(coord, who);

        public static bool MapGetRandomPassable(Vec2i coord, Ident who, out Vec2i result)
        {
            result = Vec2i.Zero;
            int si = UnityEngine.Random.Range(0, 8);

            for (int i = 0; i < Neighbour8.Length; i++)
            {
                Vec2i testCoord = coord + Neighbour8[(si + i) % 8];

                if (MapIsPassable(testCoord, who))
                {
                    result = testCoord;
                    return true;
                }
            }

            return false;
        }

        public static bool MapGetFirstPlayer(Vec2i coord, out Ident entity)
        {
            return Context.Map.TryFindFirst(coord, eid => IsPlayer(eid), out entity);
        }

        public static bool MapGetFirstPlayer(IEnumerable<Vec2i> coords, out Ident entity)
        {
            entity = Ident.Zero;

            foreach (Vec2i coord in coords)
            {
                if (Context.Map.TryFindFirst(coord, eid => IsPlayer(eid), out entity))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MapGetFirst(Vec2i coord, out Ident entity) => Context.Map.TryGetFirst(coord, out entity);

        public static bool MapGetFirst(Vec2i coord, TagType tags, out Ident entity)
        {
            return Context.Map.TryFindFirst(coord, ident => { return Is(ident, tags); }, out entity);
        }

        public static bool MapGetFirstEnemy(Vec2i coord, Ident entity, out Ident enemy)
        {
            return Context.Map.TryFindFirst(coord, ident => { return IsEnemy(ident); }, out enemy);
        }

        public static bool MapGetFirstPickable(Vec2i coord, Ident entity, out Ident what)
        {
            return Context.Map.TryFindFirst(coord, ident => { return IsPickable(ident, entity); }, out what);
        }

        public static bool MapGetFirstBuildable(Vec2i coord, Ident entity, out Ident what)
        {
            return Context.Map.TryFindFirst(coord, ident => { return IsBuildable(ident, entity); }, out what);
        }

        public static bool MapGetFirstCultivable(Vec2i coord, Ident entity, out Ident what)
        {
            return Context.Map.TryFindFirst(coord, ident => { return IsCultivable(ident, entity); }, out what);
        }

        // ----
        // BODY
        // ----

        public static bool BodyGetWeaponsInHands(Ident bid, out Ident left, out Ident right)
        {
            left  = Ident.Zero;
            right = Ident.Zero;
            // Try to get the body.
            var body = Context.Bodies.Get(bid);
            if (body == null)
            {
                return false;
            }

            BodyMember lHand = body.Find(BodyMember.Type.Hand, "L");
            BodyMember rHand = body.Find(BodyMember.Type.Hand, "R");

            left  = lHand != null ? lHand.wield : Ident.Zero;
            right = rHand != null ? rHand.wield : Ident.Zero;

            return !left.IsZero || !right.IsZero;
        }

        #region @@@ HELPERS @@@

        public static Vec2i GetNeighbour(Vec2i origin, int neighbour)
        {
            return origin + Neighbour8[neighbour % 8];
        }

        #endregion
    }
}
