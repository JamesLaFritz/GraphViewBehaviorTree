using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree.Nodes
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

        /// <value>
        /// The State the Node is in.
        /// </value>
        [SerializeField, HideInInspector] public State state = State.Running;

        /// <value>
        /// Has the node started.
        /// </value>
        [SerializeField, HideInInspector] private bool started;

        /// <value>
        /// Has the node started.
        /// </value>
        public bool IsStarted
        {
            get => started;
            protected set => started = value;
        }

        /// <value>
        /// The GUID of the Node View, used to get the Node View that this Node is associated with.
        /// </value>
        [HideInInspector] public string guid;

        /// <value>
        /// The Position in the Behavior Tree View that this Node is at.
        /// </value>
        [HideInInspector] public Vector2 nodeGraphPosition;

        /// <value>
        /// Does this node have more then one parent.
        /// </value>
        [HideInInspector] public bool hasMultipleParents;

        #region Abstract Methods

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

        #endregion

        #region Virtual Methods

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

        /// <summary>
        /// Reset the Node.
        /// Calls all Nodes children to Reset.
        /// If the Node is Started calls OnStop() then sets started to false.
        /// Set the State to running.
        /// </summary>
        public virtual void NodeReset()
        {
            foreach (Node child in GetChildren())
            {
                child.NodeReset();
            }

            if (started)
            {
                OnStop();
                started = false;
            }

            state = State.Running;
        }

        #endregion

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

            if (!started)
            {
                if (!hasMultipleParents) return state;
                State stateToReturn = state;
                NodeReset();
                return stateToReturn;
            }

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
