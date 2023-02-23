using System.Collections.Generic;
using UnityEngine;
using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeSelectJobCheck : NodeAction
    {
        public NodeSelectJobCheck()
        {
        }

        public override NodeState Evaluate()
        {
            Debug.Log("Job not done");

            int? jid = (int?)FindVar("targetJob");
            if (jid == null)
            {
                Debug.Log("No JOB");
            }

            Jobs.Job job = Context.Jobs.At(jid.Value);

            if (!job.Done)
            {
                Debug.Log("Unfinished job");
            }

            return NodeState.Success;
        }
    }
}
