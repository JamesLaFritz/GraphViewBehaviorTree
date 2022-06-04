// WaitNode.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree.Nodes
{
    /// <summary>
    /// <see cref="ActionNode"/> that waits for a period of time before returning success.
    /// </summary>
    [System.Serializable]
    public class WaitNode : ActionNode
    {
        /// <value>
        /// The Duration that the Node waits before returning success.
        /// </value>
        [Range(0, 10)] [SerializeField] private float duration = 1f;

        /// <value>
        /// The Time the Node Started
        /// </value>
        private float m_startTime;

        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart() => m_startTime = Time.time;

        /// <inheritdoc />
        protected override void OnStop() { }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            return Time.time - m_startTime > duration ? State.Success : State.Running;
        }

        #endregion
    }
}