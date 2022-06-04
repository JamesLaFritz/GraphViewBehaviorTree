// BehaviorTreeView.cs
// 05-15-2022
// James LaFritz

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = GraphViewBehaviorTree.Nodes.Node;

namespace GraphViewBehaviorTree.Editor.Views
{
    /// <summary>
    /// > [!WARNING]
    /// > Experimental: this API is experimental and might be changed or removed in the future.
    /// 
    /// A View for the Behavior Tree, derived from <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.GraphView.html" rel="external">UnityEditor.Experimental.GraphView.GraphView</a>
    /// Can be used in the UI Builder.
    /// </summary>
    public class BehaviorTreeView : GraphView
    {
        /// <summary>
        /// Required in order to have <see cref="BehaviorTreeView"/> show up in the UI Builder Library.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

        /// <summary>
        /// <value>Notifies the Observers that a <see cref="Node"/> has been Selected and pass the <see cref="Node"/> that was selected.</value>
        /// </summary>
        public Action<Node> onNodeSelected;

        /// <summary>
        /// <value>The Tree associated with this view.</value>
        /// </summary>
        private BehaviorTree m_tree;

        /// <summary>
        /// <value>Dose the view have a tree</value>
        /// </summary>
        private bool m_hasTree;

        /// <summary>
        /// Creates a new <see cref="BehaviorTreeView"/>.
        /// Required in order to have this show up in the UI Builder Library.
        /// </summary>
        public BehaviorTreeView()
        {
            style.flexGrow = 1;
            Insert(0, new GridBackground() { name = "grid_background" });
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Undo-Undo.UndoRedoCallback.html" rel="external">UnityEditor.Undo.UndoRedoCallback</a> assigned to <a href="https://docs.unity3d.com/ScriptReference/Undo-undoRedoPerformed.html" rel="external">UnityEditor.Undo.undoRedoPerformed</a>
        /// </summary>
        private void UndoRedoPerformed()
        {
            PopulateView(m_tree);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Populate the View with the passed in tree
        /// </summary>
        /// <param name="tree">The <see cref="BehaviorTree"/> to populate the View from</param>
        public void PopulateView(BehaviorTree tree)
        {
            m_tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            m_hasTree = m_tree != null;
            if (!m_hasTree) return;
            m_tree.GetNodes().ForEach(CreateNodeView);

            foreach (Edge edge in from node in m_tree.GetNodes()
                                  let parentView = GetNodeByGuid(node.guid) as BehaviorTreeNodeView
                                  where parentView is { output: { } }
                                  from child in m_tree.GetChildren(node)
                                  where child != null
                                  let childView = GetNodeByGuid(child.guid) as BehaviorTreeNodeView
                                  where childView is { input: { } }
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
                            int count = (childView.input.connections ?? Array.Empty<Edge>()).Count();
                            childView.node.hasMultipleParents = count > 2;

                            break;
                        }
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null && m_hasTree)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    BehaviorTreeNodeView parentView = edge.output.node as BehaviorTreeNodeView;
                    BehaviorTreeNodeView childView = edge.input.node as BehaviorTreeNodeView;

                    m_tree.AddChild(parentView.node, childView.node);
                    parentView.SortChildren();

                    int count = (childView.input.connections ?? Array.Empty<Edge>()).Count();
                    childView.node.hasMultipleParents = count > 0;
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (BehaviorTreeNodeView parentNodeView
                         in from movedElement in graphViewChange.movedElements
                            let movedNode = movedElement as BehaviorTreeNodeView
                            where movedNode is { input: { connections: { } } }
                            from edge in movedNode.input.connections
                            where edge.output is { node: BehaviorTreeNodeView }
                            select edge.output?.node as BehaviorTreeNodeView)
                {
                    parentNodeView?.SortChildren();
                }
            }

            return graphViewChange;
        }

        /// <summary>
        /// Adds a <see cref="BehaviorTreeNodeView"/> from the passed in Node.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to create a view for.</param>
        private void CreateNodeView(Node node)
        {
            BehaviorTreeNodeView nodeView = new BehaviorTreeNodeView(node)
            {
                onNodeSelected = onNodeSelected
            };
            if (m_hasTree)
                nodeView.onSetRootNode = _ => m_tree.rootNode = node;
            AddElement(nodeView);
        }

        /// <summary>
        /// Create a new <see cref="Node"/> with a Node View.
        /// </summary>
        /// <param name="type">The Type of Node to create.</param>
        private void CreateNode(Type type)
        {
            if (!m_hasTree) return;

            Node node = m_tree.CreateNode(type);
            CreateNodeView(node);
            Undo.RecordObject(m_tree, "Behavior Tree (Create Node)");

            if (Application.isPlaying) return;

            AssetDatabase.AddObjectToAsset(node, m_tree);
            AssetDatabase.SaveAssets();

            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (Create Node)");
            EditorUtility.SetDirty(node);
        }

        /// <summary>
        /// Delete a <see cref="Node"/> from the tree.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to Delete.</param>
        private void DeleteNode(Node node)
        {
            if (!m_hasTree) return;

            m_tree.DeleteNode(node);
            Undo.RecordObject(m_tree, "Behavior Tree (Delete Node)");

            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(m_tree);
        }

        /// <summary>
        /// Used to Update the Node State of all nodes in this tree for when Unity is in Play Mode.
        /// </summary>
        public void UpdateNodeStates()
        {
            foreach (BehaviorTreeNodeView nodeView in nodes.OfType<BehaviorTreeNodeView>())
            {
                nodeView.UpdateState();
            }
        }

        #region Overrides of GraphView

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.GraphView.BuildContextualMenu.html" rel="external">UnityEditor.Experimental.GraphView.GraphView.BuildContextualMenu</a>
        /// Add menu items to the contextual menu.
        /// </summary>
        /// <param name="evt">The (<a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/UIElements.ContextualMenuPopulateEvent.html" rel="external">UnityEngine.UIElements.ContextualMenuPopulateEvent</a>) event holding the menu to populate.</param>
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

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Experimental.GraphView.GraphView.GetCompatiblePorts.html" rel="external">UnityEditor.Experimental.GraphView.GraphView.GetCompatiblePorts</a>
        /// Get all ports compatible with given port.
        /// </summary>
        /// <param name="startPort">
        /// <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Port.html" rel="external">UnityEditor.Experimental.GraphView.Port</a>
        /// Start port to validate against.
        /// </param>
        /// <param name="nodeAdapter">
        /// <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Port.html" rel="external">UnityEditor.Experimental.GraphView.Port</a>
        /// Node adapter.
        /// </param>
        /// <returns>List of <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.NodeAdapter.html" rel="external">UnityEditor.Experimental.GraphView.NodeAdapter</a> List of compatible ports.</returns>
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