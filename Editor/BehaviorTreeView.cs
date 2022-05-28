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
            DeleteElements(graphElements);

            m_hasTree = tree != null;
            m_tree.nodes.ForEach(CreateNodeView);
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

        private void CreateActionNode(DropdownMenuAction obj) { }

        #endregion
    }
}