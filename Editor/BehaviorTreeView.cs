// BehaviorTreeView.cs
// 05-15-2022
// James LaFritz

using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

        private int m_currentNodeId = 0;

        private BehaviorTree m_tree;
        private bool m_hasTree;

        public BehaviorTreeView()
        {
            style.flexGrow = 1;
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public void PopulateView(BehaviorTree tree)
        {
            // Store the tree
            m_tree = tree;
            // clear out anything that may have been created previously;
            DeleteElements(graphElements);
            // loop through the nodes and create a new node.
            m_tree.nodes.ForEach(n => CreateNodeView(n));
        }

        private void CreateNodeView(Node node)
        {
            BehaviorTreeNodeView nodeView = new BehaviorTreeNodeView(node);
            AddElement(nodeView);
        }

        public Node CreateNode<T>() where T : Node, new()
        {
            if (!m_hasTree) return null;

            Node node = new T();
            node.name = node.GetType().Name;
            node.nodeID = m_currentNodeId++;

            m_tree.nodes.Add(node);

            return node;
        }

        public void DeleteNode(Node node)
        {
            if (!m_hasTree) return;

            m_tree.nodes.Remove(node);
        }
    }
}