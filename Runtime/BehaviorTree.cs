// BehaviorTree.cs
// 05-05-2022
// James LaFritz

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Behavior tree is an execution tree requires that the Root Node be set, derived from <see cref="UnityEngine.ScriptableObject"/>
    /// </summary>
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "Behavior Tree")]
    [System.Serializable]
    public class BehaviorTree : ScriptableObject
    {
        /// <summary>
        /// The Node to start the Behavior tree.
        /// </summary>
        [HideInInspector] public Node rootNode;

        /// <summary>
        /// The State the tree is in.
        /// </summary>
        [HideInInspector] public Node.State treeState = Node.State.Running;

        /// <summary>
        /// The Nodes that the tree has.
        /// </summary>
        [SerializeField, HideInInspector] List<Node> m_nodes = new List<Node>();

        /// <summary>
        /// Get all of the Nodes in the Tree.
        /// </summary>
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

        /// <summary>
        /// Add a child node to the parent.
        /// </summary>
        /// <param name="parent">The parent Node.</param>
        /// <param name="child">The Node to add to the parent.</param>
        public void AddChild(Node parent, Node child)
        {
            if (!m_nodes.Contains(parent)) return;

            m_nodes[m_nodes.IndexOf(parent)].AddChild(child);

            if (m_nodes.Contains(child)) return;

            m_nodes.Add(child);
        }

        /// <summary>
        /// Remove a node from the parent.
        /// </summary>
        /// <param name="parent">The parent Node.</param>
        /// <param name="child">The Node to remove from the parent.</param>
        public void RemoveChild(Node parent, Node child)
        {
            if (!m_nodes.Contains(parent)) return;

            m_nodes[m_nodes.IndexOf(parent)].RemoveChild(child);
        }

        /// <summary>
        /// Get a list of children from the parent.
        /// </summary>
        /// <param name="parent">The node to get the children from</param>
        /// <returns>A list of children Nodes that the parent contains.</returns>
        public List<Node> GetChildren(Node parent)
        {
            return !m_nodes.Contains(parent)
                ? new List<Node>()
                : m_nodes[m_nodes.IndexOf(parent)].GetChildren();
        }

        /// <summary>
        /// Traverse the node and run the Action.
        /// </summary>
        public void Traverse(Node node, System.Action<Node> visitor)
        {
            if (!node) return;
            visitor?.Invoke(node);
            node.GetChildren()?.ForEach((n) => Traverse(n, visitor));
        }

        /// <summary>
        /// Clone the Tree.
        /// </summary>
        /// <returns>A Clone of the tree</returns>
        public BehaviorTree Clone()
        {
            BehaviorTree tree = Instantiate(this);
            tree.m_nodes = new List<Node>();
            foreach (Node node in m_nodes)
            {
                tree.m_nodes.Add(node.Clone());
            }

            tree.rootNode = tree.m_nodes[m_nodes.IndexOf(rootNode)];
            Traverse(rootNode, (n) =>
            {
                int nodeIndex = m_nodes.IndexOf(n);
                foreach (int childIndex in m_nodes[nodeIndex]?.GetChildren().Select(c => m_nodes.IndexOf(c)))
                {
                    tree.m_nodes[nodeIndex].RemoveChild(m_nodes[childIndex]);
                    tree.AddChild(tree.m_nodes[nodeIndex], tree.m_nodes[childIndex]);
                }
            });

            return tree;
        }
    }
}