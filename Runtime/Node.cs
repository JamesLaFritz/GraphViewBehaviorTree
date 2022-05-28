using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Base class for all nodes in the Behavior tree.
    /// </summary>
    [System.Serializable]
    public abstract class Node : ScriptableObject
    {
        /// <summary>
        /// The states a node can be in.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// The Node is Running
            /// </summary>
            Running,

            /// <summary>
            /// The Node has completed successfully.
            /// </summary>
            Success,

            /// <summary>
            /// The Node has completed unsuccessfully;
            /// </summary>
            Failure
        }

        [SerializeField] private State state = State.Running;

        [SerializeField] bool started;

        public string guid;

        /// <summary>
        /// Runs when the Node first starts running.
        /// Initialize the Node.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Runs when the Node Stops.
        /// Any Cleanup that the Node may need to do.
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// Runs every Update of the Node.
        /// </summary>
        /// <returns>The State the Node is in once it finishes Updating.</returns>
        protected abstract State OnUpdate();

        /// <summary>
        /// Update the Node.
        /// </summary>
        /// <returns>The state that the Node is in.</returns>
        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            // if the state is running the state is not failure or not success (in case I decide to add other states latter).
            if (state != State.Failure && state != State.Success) return state;
            OnStop();
            started = false;
            return state;
        }
    }
}
