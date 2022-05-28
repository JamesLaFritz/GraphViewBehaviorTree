// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// A View for Behavior Tree Nodes, based on the Graph View Node
    /// </summary>
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private Node m_node;

        public Node node => m_node;

        public BehaviorTreeNodeView(Node node)
        {
            m_node = node;
            if (m_node == null) return;
            base.title = m_node.GetType().Name;
            viewDataKey = m_node.guid;
            style.left = m_node.nodeGraphPosition.x;
            style.top = m_node.nodeGraphPosition.y;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            m_node.nodeGraphPosition.x = newPos.xMin;
            m_node.nodeGraphPosition.y = newPos.yMin;
        }

        #endregion
    }
}