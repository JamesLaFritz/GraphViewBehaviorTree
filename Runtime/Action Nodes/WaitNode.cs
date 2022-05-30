// WaitNode.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// <see cref="ActionNode"/> that waits for a period of time before returning success.
    /// </summary>
    [System.Serializable]
    public class WaitNode : ActionNode
    {
        /// <summary>
        /// The Duration that the Node waits before returning success.
        /// </summary>
        [Range(0, 10)] [SerializeField] private float duration = 1f;

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