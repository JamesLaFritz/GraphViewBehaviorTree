using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Base class for all nodes in the Behavior tree.
    /// </summary>
    public abstract class Node : Object
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

        private State m_state = State.Running;

        private bool m_started;

        public int nodeID;

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
            if (!m_started)
            {
                OnStart();
                m_started = true;
            }

            m_state = OnUpdate();

            // if the state is running the state is not failure or not success (in case I decide to add other states latter).
            if (m_state != State.Failure && m_state != State.Success) return m_state;
            OnStop();
            m_started = false;
            return m_state;
        }
    }
}
