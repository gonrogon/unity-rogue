using Rogue.Coe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define an enumerator with the type of result of the execution of an action.
    /// </summary>
    public enum ActionState
    {
        Fail, // The action is not performed.
        Miss, // The action is performed without success.
        Good, // The action is performed successfully.
    }

    /// <summary>
    /// Define a message to perform an action.
    /// </summary>
    public class Action<T> : GameMessage<T> where T : Action<T>
    {
        /// <summary>
        /// Result of the action.
        /// </summary>
        public ActionState state = ActionState.Fail;

        /// <summary>
        /// Cost of the action.
        /// </summary>
        public int cost = 0;

        public bool Success => state == ActionState.Good;

        public bool Failure => state == ActionState.Fail || state == ActionState.Miss;

        public void MarkFail() => state = ActionState.Fail;

        public void MarkMiss() => state = ActionState.Miss;

        public void MarkGood() => state = ActionState.Good;

        public void MarkFailOrGood(bool test)
        {
            if (test)
            {
                state = ActionState.Good;
            }
            else
            {
                state = ActionState.Fail;
            }
        }
    }
}