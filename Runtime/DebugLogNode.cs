// DebugLogNode.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Action node that logs a message.
    /// </summary>
    public class DebugLogNode : ActionNode
    {
        /// <summary>
        /// The Message to Log.
        /// </summary>
        public string message;

        #region Overrides of Node

        /// <inheritdoc />
        protected override void OnStart() => Debug.Log($"OnStart: {message}");

        /// <inheritdoc />
        protected override void OnStop() => Debug.Log($"OnStop: {message}");

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate: {message}");
            return State.Success;
        }

        #endregion
    }
}