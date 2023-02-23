using Rogue.Coe;
using UnityEngine;

namespace Rogue.Game.Behav
{
    public class Builder : GameBehaviour<Builder>
    {
        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message)
        {
            switch (message)
            {
                case Msg.ActionBuild msg: return OnBuild(msg);
            }

            return GameMessageState.Continue;
        }

        public GameMessageState OnBuild(Msg.ActionBuild message)
        {
            var msg = new Msg.Build(message.work);
            
            World.Send(message.target, msg);
            if (msg.done)
            {
                message.done = true;
            }

            message.state = Msg.ActionState.Good;
            message.cost  = 100;

            return GameMessageState.Consumed;
        }
    }
}
