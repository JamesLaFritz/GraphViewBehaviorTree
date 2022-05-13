// BehaviorTree.cs
// 05-05-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Behavior tree is an execution tree and always starts with a Root Node.
    /// </summary>
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "Behavior Tree")]
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
                if (rootNode!.state == Node.State.Running)
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