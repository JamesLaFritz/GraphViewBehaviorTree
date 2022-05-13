// SequencerNode.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Composite Node - Runs all of the children in Sequence.
    /// If one of the children fails the node will stop running and return failure.
    /// Returns Success if all of the children nodes Succeed.
    /// </summary>
    public class SequencerNode : CompositeNode
    {
        private int m_current;

        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart()
        {
            m_current = 0;
        }

        /// <inheritdoc />
        protected override void OnStop() { }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            if (children is { Count: >= 1 })
                return children[m_current]!.Update() switch
                {
                    State.Running => State.Running,
                    State.Failure => State.Failure,
                    State.Success => ++m_current == children.Count ? State.Success : State.Running,
                    _ => State.Failure
                };
            Debug.LogWarning("Sequencer Node has no children.");
            return State.Failure;
        }

        #endregion
    }
}