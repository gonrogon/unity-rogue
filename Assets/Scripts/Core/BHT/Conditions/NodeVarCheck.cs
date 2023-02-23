
namespace Rogue.Core.BHT
{
    /// <summary>
    /// Defines a condition that checks whether a variable is defined or not.
    /// </summary>
    public class NodeVarCheck : NodeCondition
    {
        /// <summary>
        /// Defines an enumeration with the types of checks.
        /// </summary>
        public enum Check
        {
            Defined,
            NotDefined,
        }

        /// <summary>
        /// Name of the variable to check.
        /// </summary>
        private readonly string m_var;

        /// <summary>
        /// Type of check to do.
        /// </summary>
        private readonly Check m_check = Check.Defined;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeVarCheck() : this(null) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="var">Name of the variable.</param>
        /// <param name="check">Type of check.</param>
        public NodeVarCheck(string var, Check check = Check.Defined) : base()
        {
            m_var   = var;
            m_check = check;
        }

        protected override bool DoTest() => m_check switch
        {
            Check.Defined => FindVar(m_var) != null,
            _             => FindVar(m_var) == null
        };
    }
}
