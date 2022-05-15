// RepeatNode.cs
// 05-13-2022
// James LaFritz

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Decorator Node, Returns State Running Regardless of the Child Nodes State.
    /// </summary>
    [System.Serializable]
    public class RepeatNode : DecoratorNode
    {
        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart() { }

        /// <inheritdoc />
        protected override void OnStop() { }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            child.Update();
            return State.Running;
        }

        #endregion
    }
}