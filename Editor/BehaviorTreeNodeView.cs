// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private Node m_node;

        public BehaviorTreeNodeView(Node node)
        {
            m_node = node;
            base.title = node.name;
        }
    }
}