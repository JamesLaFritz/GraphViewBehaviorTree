// BehaviorTreeView.cs
// 05-15-2022
// James LaFritz

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// A View for the Behavior Tree.
    /// Can be used in the UI Builder.
    /// </summary>
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

        public Action<Node> OnNodeSelected;

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

        /// <summary>
        /// Populate the View wih the passed in tree
        /// </summary>
        /// <param name="tree">The tree to populate the View from</param>
        public void PopulateView(BehaviorTree tree)
        {
            m_tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            m_hasTree = tree != null;
            if (!m_hasTree) return;
            m_tree.GetNodes().ForEach(CreateNodeView);

            foreach (Edge edge in from node in m_tree.GetNodes()
                                  let parentView = GetNodeByGuid(node.guid) as BehaviorTreeNodeView
                                  from child in m_tree.GetChildren(node)
                                  let childView = GetNodeByGuid(child.guid) as BehaviorTreeNodeView
                                  select parentView.output.ConnectTo(childView.input))
            {
                AddElement(edge);
            }
        }

        /// <summary>
        /// Hook into the Graph View Change to delete Nodes when the Node View Element is slated to be Removed.
        /// </summary>
        /// <param name="graphViewChange"><a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Experimental.GraphView.GraphViewChange.html">GraphViewChange</a></param>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    switch (element)
                    {
                        case BehaviorTreeNodeView nodeView:
                            DeleteNode(nodeView.node);
                            break;
                        case Edge edge:
                        {
                            BehaviorTreeNodeView parentView = edge.output.node as BehaviorTreeNodeView;
                            BehaviorTreeNodeView childView = edge.input.node as BehaviorTreeNodeView;
                            m_tree.RemoveChild(parentView.node, childView.node);
                            break;
                        }
                    }
                }
            }

            if (graphViewChange.edgesToCreate == null || !m_hasTree) return graphViewChange;

            foreach (Edge edge in graphViewChange.edgesToCreate)
            {
                BehaviorTreeNodeView parentView = edge.output.node as BehaviorTreeNodeView;
                BehaviorTreeNodeView childView = edge.input.node as BehaviorTreeNodeView;

                m_tree.AddChild(parentView.node, childView.node);
            }

            return graphViewChange;
        }

        /// <summary>
        /// Adds a Node View to the Tree View from the passed in Node.
        /// </summary>
        /// <param name="node">The Node to create a view for.</param>
        private void CreateNodeView(Node node)
        {
            BehaviorTreeNodeView nodeView = new BehaviorTreeNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        /// <summary>
        /// Create a new Node with a Node View.
        /// </summary>
        /// <param name="type">The Type of Node to create.</param>
        private void CreateNode(Type type)
        {
            if (!m_hasTree) return;

            Node node = m_tree.CreateNode(type);

            CreateNodeView(node);

            AssetDatabase.AddObjectToAsset(node, m_tree);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Delete a Node from the tree.
        /// </summary>
        /// <param name="node">The Node to Delete.</param>
        private void DeleteNode(Node node)
        {
            if (!m_hasTree) return;

            m_tree.DeleteNode(node);

            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        #region Overrides of GraphView

        /// <inheritdoc />
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;
                evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}",
                                      _ => CreateNode(type));
            }

            base.BuildContextualMenu(evt);
        }

        /// <inheritdoc />
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()!.Where(endPort =>
                                             endPort.direction != startPort.direction &&
                                             endPort.node != startPort.node &&
                                             endPort.portType == startPort.portType).ToList();
        }

        #endregion
    }
}