// BehaviorTree.cs
// 05-05-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Behavior tree is an execution tree and always starts with a Root Node.
    /// </summary>
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "Behavior Tree")]
    [System.Serializable]
    public class BehaviorTree : ScriptableObject
    {
        /// <summary>
        /// The Node to start the Behavior tree.
        /// </summary>
        public Node rootNode;

        /// <summary>
        /// The State the tree is in.
        /// </summary>
        public Node.State treeState = Node.State.Running;

        /// <summary>
        /// The Nodes that the tree has.
        /// </summary>
        public List<Node> nodes = new List<Node>();

        private bool m_hasRootNode;

        /// <summary>
        /// Update the tree by updating the root node.
        /// </summary>
        /// <returns>The state of the tree.</returns>
        public Node.State Update()
        {
            if (!m_hasRootNode)
            {
                m_hasRootNode = rootNode != null;

                if (!m_hasRootNode)
                {
                    Debug.LogWarning($"{name} needs a root node in order to properly run. Please add one.", this);
                }
            }

            if (m_hasRootNode)
            {
                if (treeState == Node.State.Running)
                    treeState = rootNode.Update();
            }
            else
            {
                treeState = Node.State.Failure;
            }

            return treeState;
        }
    }
}