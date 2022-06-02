// Fail.cs
// 06-01-2022
// James LaFritz

namespace GraphViewBehaviorTree
{
    public class Fail : ActionNode
    {
        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart() { }

        /// <inheritdoc />
        protected override void OnStop() { }

        /// <inheritdoc />
        protected override State OnUpdate() => state = State.Failure;

        #endregion
    }
}