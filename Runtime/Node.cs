using System.Collections.Generic;
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

        /// <summary>
        /// The State the Node is in.
        /// </summary>
        [SerializeField, HideInInspector] public State state = State.Running;

        [SerializeField, HideInInspector] bool started;

        /// <summary>
        /// Has the node started.
        /// </summary>
        public bool IsStarted => started;

        /// <summary>
        /// The GUID of the node view, used to get the Node View that this Node is associated with.
        /// </summary>
        [HideInInspector] public string guid;

        /// <summary>
        /// The Position in the Behavior Tree View that this Node is at.
        /// </summary>
        [HideInInspector] public Vector2 nodeGraphPosition;

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
        /// Add the child node to this node.
        /// </summary>
        /// <param name="childNode">The Node to add as a Child.</param>
        public virtual void AddChild(Node childNode) { }

        /// <summary>
        /// Remove a Child from the Node.
        /// </summary>
        /// <param name="childNode">The Child to remove.</param>
        public virtual void RemoveChild(Node childNode) { }

        /// <summary>
        /// Get a list of children the node contains.
        /// </summary>
        /// <returns>A list of children Nodes.</returns>
        public virtual List<Node> GetChildren()
        {
            List<Node> children = new List<Node>();

            return children;
        }

        public void Reset()
        {
            foreach (Node child in GetChildren())
            {
                child.Reset();
            }

            if (started)
            {
                OnStop();
                started = false;
            }

            state = State.Running;
        }

        /// <summary>
        /// Update the Node.
        /// </summary>
        /// <returns>The state that the Node is in.</returns>
        public State Update()
        {
            if (!started && state == State.Running)
            {
                started = true;
                OnStart();
                return state;
            }

            // if the state is running the state is not failure or not success (in case I decide to add other states latter).
            if (state != State.Failure && state != State.Success)
            {
                state = OnUpdate();
                return State.Running;
            }

            if (!started) return state;

            started = false;
            OnStop();

            return State.Running;
        }

        /// <summary>
        /// Clone the Node.
        /// </summary>
        /// <returns>A Clone of the Node.</returns>
        public Node Clone()
        {
            return Instantiate(this);
        }
    }
}
