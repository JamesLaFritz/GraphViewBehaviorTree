// RepeatNode.cs
// 05-13-2022
// James LaFritz

namespace GraphViewBehaviorTree.Nodes
{
    /// <summary>
    /// <see cref="DecoratorNode"/> returns State Running Regardless of the Child Nodes State.
    /// </summary>
    [System.Serializable]
    public class RepeatNode : DecoratorNode
    {
        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart() { }

        /// <inheritdoc />
        protected override void OnStop()
        {
            if (!IsStarted)
                NodeReset();
        }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            state = child.Update();

            return state;
        }

        #endregion
    }
}