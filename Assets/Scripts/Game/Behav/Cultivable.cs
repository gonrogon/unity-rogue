using Rogue.Coe;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Behav
{
    public class Cultivable : GameBehaviour<Cultivable>
    {
        private Comp.Crop m_crop;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            m_crop = entity.FindFirstComponent<Comp.Crop>();
            if (m_crop == null)
            {
                return false;
            }

            SetupCrop();

            return true;
        }

        private void SetupCrop()
        {
            Crops.Crop crop = Context.Crops.At(m_crop.cropId);
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.ActionPlow    plow:    return OnPlow(plow);
                case Msg.ActionSeed    seed:    return OnSeed(seed);
                case Msg.ActionHarvest harvest: return OnHarvest(harvest);
                
            }

            return GameMessageState.Continue;
        }

        private GameMessageState OnPlow(Msg.ActionPlow message)
        {
            message.MarkFailOrGood(GetCrop().Plow(message.where));
            /*
            Crops.Crop crop = GetCrop();

            message.done = crop.Plow(message.where);
            if (message.done)
            {
                if (message.job >= 0)
                {
                    Context.Jobs.At(message.job).Complete();
                }
            }
            */
            return GameMessageState.Consumed;
        }

        private GameMessageState OnSeed(Msg.ActionSeed message)
        {
            message.MarkFailOrGood(GetCrop().Seed(message.where));
            /*
            Crops.Crop crop = GetCrop();

            message.done = crop.Seed(message.where);
            if (message.done)
            {
                if (message.job >= 0)
                {
                    Context.Jobs.At(message.job).Complete();
                }
            }
            */
            return GameMessageState.Consumed;
        }

        private GameMessageState OnHarvest(Msg.ActionHarvest message)
        {
            message.MarkFailOrGood(GetCrop().Harvest(message.where));
            if (message.Success)
            {
                CreatePlantBunch(message.where);
            }

            return GameMessageState.Consumed;
        }

        private void CreatePlantBunch(Vec2i where)
        {
            GameEntity entity = World.Create("plant_bunch");

            entity.FindFirstComponent<Comp.Location>().position = where;
            Context.Map.Add(where, entity.Id);
            World.Start(entity);
            // GameEntity ent = new ();
            // ent.Populate(Context.Templates.Find("plant_bunch"));
            // ent.FindFirstComponent<Comp.Location>().position = where;
            // Ident eid = Context.World.Add(ent);
            // Context.Map.Add(where, eid);
        }

        #region @@@ UTILITIES @@@

        private Crops.Crop GetCrop()
        {
            return Context.Crops.At(m_crop.cropId);
        }

        #endregion
    }
}
