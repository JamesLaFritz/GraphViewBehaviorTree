// SequencerNode.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree.Nodes
{
    /// <summary>
    /// <see cref="CompositeNode"/> - Runs all of the children in Sequence, by the order in the list of children.
    /// If one of the children fails the node will stop running and return failure.
    /// Returns Success if all of the children nodes Succeed.
    /// </summary>
    public class SequencerNode : CompositeNode
    {
        /// <value>
        /// The current Node index.
        /// </value>
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
            if (children is { Count: >= 1 } && m_current < children.Count)
                return children[m_current]!.Update() switch
                {
                    State.Running => State.Running,
                    State.Failure => State.Failure,
                    State.Success => ++m_current == children.Count ? State.Success : State.Running,
                    _ => State.Failure
                };

            if (children is { Count: >= 1 })
            {
                Debug.LogWarning("Something went wrong! Sequencer Node has gone out of range");
                m_current = children.Count - 1;
                return State.Running;
            }

            Debug.LogWarning("Sequencer Node has no children.");
            return State.Failure;
        }

        #endregion
    }
}