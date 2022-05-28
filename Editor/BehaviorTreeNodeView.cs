// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

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
        }
    }
}