
namespace Rogue.Core.BHT
{
    /// <summary>
    /// Defines a condition that checks whether a boolean variable is defined and have the required value or not.
    /// </summary>
    public class NodeVarCheckBoolean : NodeCondition
    {
        /// <summary>
        /// Name of the variable to check.
        /// </summary>
        private readonly string m_var;

        /// <summary>
        /// Expected value.
        /// </summary>
        private readonly bool m_expected = true;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="var">Variable.</param>
        /// <param name="expected">Expected value.</param>
        public NodeVarCheckBoolean(string var, bool expected = true) : base()
        {
            m_var      = var;
            m_expected = expected;
        }

        protected override bool DoTest()
        {
            object obj = FindVar(m_var);

            if (obj == null || obj is not bool value)
            {
                return false;
            }

            if (value != m_expected)
            { 
                return false;
            }

            return true;
        }
    }
}
