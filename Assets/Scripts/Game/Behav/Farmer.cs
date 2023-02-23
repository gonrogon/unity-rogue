using Rogue.Coe;
using UnityEngine;

namespace Rogue.Game.Behav
{
    public class Farmer : GameBehaviour<Farmer>
    {
        public override bool OnSetup(GameWorld world, GameEntity entity)
        {
            if (!base.OnSetup(world, entity))
            {
                return false;
            }

            return true;
        }

        public override GameMessageState OnMessage(IGameMessage message) => message switch
        {
            Msg.ActionPlow    plow    => OnPlow(plow),
            Msg.ActionSeed    seed    => OnSeed(seed),
            Msg.ActionHarvest harvest => OnHarvest(harvest),
            _                         => GameMessageState.Continue,
        };

        public GameMessageState OnPlow(Msg.ActionPlow message)
        {
            World.Send(message.target, message);
            message.cost += 100;

            return GameMessageState.Consumed;
            /*
            var msg = new Msg.Plow(message.where);
            //msg.job = message.job;

            World.Send(message.target, msg);
            if (msg.done)
            {
                message.done = true;
            }

            //message.state = Msg.ActionState.Good;
            message.cost  = 100;

            return GameMessageState.Consumed;
            */
        }
        public GameMessageState OnSeed(Msg.ActionSeed message)
        {
            World.Send(message.target, message);
            message.cost += 100;

            return GameMessageState.Consumed;
            /*
            var msg = new Msg.Seed(message.where);
            msg.job = message.job;

            World.Send(message.target, msg);
            if (msg.done)
            {
                message.done = true;
            }

            message.state = Msg.ActionState.Good;
            message.cost  = 100;
            
            return GameMessageState.Consumed;
            */
        }

        public GameMessageState OnHarvest(Msg.ActionHarvest message)
        {
            World.Send(message.target, message);
            message.cost += 100;

            return GameMessageState.Consumed;
        }
    }
}
