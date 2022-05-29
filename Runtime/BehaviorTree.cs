// BehaviorTree.cs
// 05-05-2022
// James LaFritz

using System.Collections.Generic;
using UnityEditor;
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
        private List<Node> m_nodes = new List<Node>();

        public List<Node> GetNodes()
        {
            return m_nodes;
        }

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

        /// <summary>
        /// Create a new Node and add it to the nodes.
        /// </summary>
        /// <param name="type">The Type of Node to create.</param>
        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            m_nodes.Add(node);

            if (rootNode == null)
                rootNode = node;

            return node;
        }

        /// <summary>
        /// Delete a Node from the tree.
        /// </summary>
        /// <param name="node">The Node to Delete.</param>
        public void DeleteNode(Node node)
        {
            m_nodes.Remove(node);

            if (rootNode == node)
            {
                rootNode = null;

                if (m_nodes.Count > 0)
                {
                    rootNode = m_nodes[0];
                }
            }
        }
    }
}