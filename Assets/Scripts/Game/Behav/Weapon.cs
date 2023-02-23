using UnityEngine;
using Rogue.Coe;

namespace Rogue.Game.Behav
{
    public class Weapon : GameBehaviour<Weapon>
    {
        private Comp.Damage mDamage;

        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            mDamage = Entity.FindFirstComponent<Comp.Damage>();

            if (mDamage == null)
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.Hit msg:
                {
                    for (var c = mDamage; c != null; c = c.NextThis)
                    {
                        Debug.Log($"I'm a weapon and I do {c.current} points of {c.type} damage to {msg.target.Value}");

                        World.Send(msg.target, new Msg.Damage(c.type, c.current));
                    }  
                }
                break;
            }

            return GameMessageState.Continue;
        }
    }
}
