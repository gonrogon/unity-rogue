using UnityEngine;

namespace Rogue.Core.BHT
{
    public class NodePrint : NodeAction
    {
        private string m_msg;

        public NodePrint() : base() {}

        public NodePrint(string msg) : base()
        {
            m_msg = msg;
        }

        public override NodeState Evaluate()
        {
            Debug.Log(m_msg);

            return NodeState.Success;
        }
    }
}
