using UnityEngine;
using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Behav
{
    public class Buildable : GameBehaviour<Attack>
    {
        /// <summary>
        /// Location.
        /// </summary>
        private Comp.Location mLocation = null;

        /// <summary>
        /// Construction component.
        /// </summary>
        private Comp.IConstruction mConstruction = null;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            mLocation     = entity.FindFirstComponent<Comp.Location>();
            mConstruction = entity.FindFirstComponent<Comp.Terrain>();

            if (mLocation == null)
            {
                return false;
            }

            if (mConstruction == null)
            {
                mConstruction = entity.FindFirstComponent<Comp.Scaffold>();

                if (mConstruction == null)
                {
                    return false;
                }
            }

            return true;
        }

        public override void OnQuit()
        {
            if (mConstruction.Job >= 0)
            {
                Context.Jobs.At(mConstruction.Job).Complete();
            }
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.Build msg: return OnBuild(msg);
            }

            return GameMessageState.Continue;
        }

        private GameMessageState OnBuild(Msg.Build message)
        {
            if (mConstruction != null)
            {
                if (mConstruction.Advance(message.work))
                {
                    mConstruction.Construct(mLocation.position);
                    // Mark the job as completed.
                    Entity.Kill();

                    message.done = true;
                }
            }

            return GameMessageState.Consumed;
        }
    }
}
