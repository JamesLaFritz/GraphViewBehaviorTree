// BehaviorTreeView.cs
// 05-15-2022
// James LaFritz

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// A View for the Behavior Tree.
    /// Can be used in the UI Builder.
    /// </summary>
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

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
            m_tree.nodes.ForEach(CreateNodeView);
        }

        /// <summary>
        /// Hook into the Graph View Change to delete Nodes when the Node View Element is slated to be Removed.
        /// </summary>
        /// <param name="graphViewChange"><a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Experimental.GraphView.GraphViewChange.html">GraphViewChange</a></param>
        /// <returns></returns>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    BehaviorTreeNodeView nodeView = element as BehaviorTreeNodeView;
                    if (nodeView == null) continue;
                    DeleteNode(nodeView.node);
                }
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
            AddElement(nodeView);
        }

        /// <summary>
        /// Create a new Node with a Node View.
        /// </summary>
        /// <param name="type">The Type of Node to create.</param>
        private void CreateNode(Type type)
        {
            if (!m_hasTree) return;

            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            m_tree.nodes.Add(node);

            CreateNodeView(node);

            if (m_tree.rootNode == null)
                m_tree.rootNode = node;

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

            m_tree.nodes.Remove(node);

            if (m_tree.rootNode == node)
            {
                m_tree.rootNode = null;

                if (m_tree.nodes.Count > 0)
                {
                    m_tree.rootNode = m_tree.nodes[0];
                }
            }

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

        #endregion
    }
}