using UnityEngine;
using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Life : GameBehaviour<Life>
    {
        private Comp.Life mLife;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            mLife = Entity.FindFirstComponent<Comp.Life>();

            if (mLife == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.Damage msg:
                {
                    Debug.Log($"Ouch! I receive {msg.amount} point of damage, my life is {mLife.current}");

                    if (mLife.Add(-msg.amount) <= 0)
                    {
                        Debug.Log("I'm dead");
                    }
                }
                break;
            }

            return GameMessageState.Continue;
        }
    }
}
