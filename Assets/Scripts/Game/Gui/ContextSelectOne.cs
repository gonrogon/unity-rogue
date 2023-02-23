using UnityEngine;
using Rogue.Core;
using Rogue.Map;
using GG.Mathe;

namespace Rogue.Game.Gui
{
    public class ContextSelectOne : Rogue.Gui.Context
    {
        private enum State { Start, Select, Finish }

        private State mState = State.Start;

        private Vec2i mStart;

        private GameMap mMap;

        private MapView mMapView;

        private Input mInput;

        public Vec2i Selection => mStart;

        public Ident Entity { get; private set; } = Ident.Zero;

        public ContextSelectOne(Input input, MapView mapView, GameMap map)
        {
            mInput   = input;
            mMap     = map;
            mMapView = mapView;
        }

        protected override void OnStart()
        {
            mState = State.Start;
        }

        protected override void OnAction(string action)
        {
            switch (mState)
            {
                case State.Start:
                {
                    if (action == "click")
                    {
                        mStart = GetMapCoord();
                        Entity = GetEntity();
                        mState = State.Finish;
                    }
                }
                break;
            }
        }

        protected override bool OnUpdate(float time)
        {
            return mState == State.Finish;
        }

        private Ident GetEntity()
        {
            if (!mMap.HasCoord(mStart) || !mMap.TryGetFirst(mStart, out Ident eid))
            {
                return Ident.Zero;
            }

            return eid;
        }

        private Vec2i GetMapCoord() => GetMapCoord(mInput.Pointer);

        private Vec2i GetMapCoord(Vector2 cursor)
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(new Vector3(cursor.x, cursor.y, -10.0f));
            Vec2i   map   = mMapView.GetCursorCell(world);

            return map;
        }
    }
}
