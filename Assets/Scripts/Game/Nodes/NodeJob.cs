using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeJob<T> : NodeAgentAction where T : Jobs.Job
    {
        protected T m_job;

        public override NodeState Evaluate()
        {
            if (!TryFindVar("targetJob", out int targetJob))
            {
                return NodeState.Success; 
            }

            if (Context.Jobs.At(targetJob) is not T job)
            {
                return NodeState.Success;
            }

            m_job = job;

            return NodeState.Failure;
        }
    }
}
