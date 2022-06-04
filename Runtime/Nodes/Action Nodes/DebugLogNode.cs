// DebugLogNode.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree.Nodes
{
    /// <summary>
    /// <see cref="ActionNode"/> that logs a message.
    /// </summary>
    [System.Serializable]
    public class DebugLogNode : ActionNode
    {
        /// <value>
        /// The Message to Log On Start.
        /// Empty will not Log a message.
        /// </value>
        [SerializeField] private string onStartMessage;

        /// <value>
        /// The Message to Log On Stop.
        /// Empty will not Log a message.
        /// </value>
        [SerializeField] private string onStopMessage;

        /// <value>
        /// The Message to Log On Start.
        /// Empty will not Log a message.
        /// </value>
        [SerializeField] private string onUpdateMessage;

        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart()
        {
            if (!string.IsNullOrEmpty(onStopMessage)) Debug.Log(onStopMessage);
        }

        /// <inheritdoc />
        protected override void OnStop()
        {
            if (!string.IsNullOrEmpty(onStartMessage)) Debug.Log(onStartMessage);
        }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            if (!string.IsNullOrEmpty(onUpdateMessage)) Debug.Log(onUpdateMessage);
            return State.Success;
        }

        #endregion
    }
}