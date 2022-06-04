// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GraphViewBehaviorTree.Nodes;
using UnityEditor.Experimental.GraphView;
using Node = GraphViewBehaviorTree.Nodes.Node;

namespace GraphViewBehaviorTree.Editor.Views
{
    /// <summary>
    /// > [!WARNING]
    /// > Experimental: this API is experimental and might be changed or removed in the future.
    /// 
    /// A View for Behavior Tree <see cref="Nodes.Node"/>, derived from <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Node.html" rel="external">UnityEditor.Experimental.GraphView.Node</a>
    /// </summary>
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        /// <value>
        /// The Node Associated with this view
        /// </value>
        private Node m_node;

        /// <value>
        /// Notifies the Observers that a <see cref="Node"/> has been Selected and pass the <see cref="Node"/> that was selected.
        /// </value>
        public Action<Node> onNodeSelected;

        /// <value>
        /// Notifies Observers that the set root node has been selected. Pass the <see cref="Node"/> that was selected to be set as the root node.
        /// </value>
        public Action<Node> onSetRootNode;

        /// <value>
        /// The <see cref="Node"/> that is associate with this view.
        /// </value>
        public Node node => m_node;

        /// <value>The Input <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Port.html" rel="external">UnityEditor.Experimental.GraphView.Port</a></value>
        public Port input;

        /// <value>
        /// The Output <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Port.html" rel="external">UnityEditor.Experimental.GraphView.Port</a>.
        /// </value>
        public Port output;

        private readonly Label m_description;

        /// <summary>
        /// Create a New Node View.
        /// </summary>
        /// <param name="node"><see cref="Node"/> that is associated with this view.</param>
        public BehaviorTreeNodeView(Node node) : base(
            AssetDatabase.GetAssetPath(Resources.Load<VisualTreeAsset>("BehaviorTreeNodeView")))
        {
            m_description = this.Q<Label>("description-label");
            m_node = node;
            if (m_node == null) return;
            base.title = m_node.GetType().Name;
            viewDataKey = m_node.guid;
            style.left = m_node.nodeGraphPosition.x;
            style.top = m_node.nodeGraphPosition.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();
        }

        /// <summary>
        /// Adds Classes to the Node View depending on the <see cref="Node.State"/> of <see cref="m_node"/>
        /// </summary>
        private void SetupClasses()
        {
            switch (m_node)
            {
                case ActionNode:
                    AddToClassList("action");
                    break;
                case CompositeNode:
                    AddToClassList("composite");
                    break;
                case DecoratorNode:
                    AddToClassList("decorator");
                    break;
            }
        }

        /// <summary>
        /// Create an Input port for all Node Types
        /// </summary>
        private void CreateInputPorts()
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input,
                                    Port.Capacity.Multi, typeof(Node));
            if (input == null) return;
            input.portName = "";
            input.name = "input-port";
            inputContainer.Add(input);
        }

        /// <summary>
        /// Create Output Port based on the Node Type.
        /// </summary>
        private void CreateOutputPorts()
        {
            switch (m_node)
            {
                case ActionNode:
                    break;
                case CompositeNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output,
                                             Port.Capacity.Multi, typeof(Node));
                    break;
                case DecoratorNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output,
                                             Port.Capacity.Single, typeof(Node));
                    break;
            }

            if (output == null) return;
            output.portName = "";
            output.name = "output-port";
            outputContainer.Add(output);
        }

        /// <summary>
        /// Sorts the Children of the <see cref="m_node"/> if it is a <see cref="CompositeNode"/>
        /// </summary>
        public void SortChildren()
        {
            CompositeNode composite = m_node as CompositeNode;

            if (composite != null)
                composite.GetChildren().Sort(SortByHorizontalPosition);
        }

        /// <summary>
        /// Sort the Nodes by their horizontal position.
        /// </summary>
        /// <param name="node1">First Node.</param>
        /// <param name="node2">Second Node.</param>
        /// <returns> if the node1 x position in the Graph is less then node1 x position in the Graph else 1</returns>
        private int SortByHorizontalPosition(Node node1, Node node2)
        {
            return node1.nodeGraphPosition.x < node2.nodeGraphPosition.x ? -1 : 1;
        }

        /// <summary>
        /// Update the Node View Visual State.
        /// Also Used to Visualize the <see cref="Node.State"/> of the node when Unity is in Play Mode.
        /// </summary>
        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");

            if (m_description != null)
            {
                List<Node> nodes = m_node.GetChildren();
                int count = m_node.GetChildren().Count;
                string text = $"{count} children:";

                text = nodes!.Aggregate(
                    text,
                    (current, child) =>
                        $"{current}\n{(child == null ? "" : child.name)}" +
                        $"{(child is DecoratorNode ? $": {(child.GetChildren()[0] != null ? child.GetChildren()[0].name : "")}" : "")}");

                m_description.text = text;
            }

            if (!Application.isPlaying) return;

            switch (m_node.state)
            {
                case Node.State.Running:
                    if (m_node.IsStarted)
                    {
                        AddToClassList("running");
                    }

                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
            }
        }

        #region Overrides of Node

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Node.SetPosition.html" rel="external">UnityEditor.Experimental.GraphView.Node.SetPosition</a>
        /// Set node position.
        /// </summary>
        /// <param name="newPos"><a href="https://docs.unity3d.com/ScriptReference/Rect.html" rel="external">UnityEngine.Rect</a> New position.</param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(m_node, "Behavior Tree (Set Position)");
            m_node.nodeGraphPosition.x = newPos.xMin;
            m_node.nodeGraphPosition.y = newPos.yMin;
            EditorUtility.SetDirty(m_node);
        }

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Node.BuildContextualMenu.html" rel="external">UnityEditor.Experimental.GraphView.Node.BuildContextualMenu</a>
        /// Add menu items to the node contextual menu.
        /// </summary>
        /// <param name="evt">The (<a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/UIElements.ContextualMenuPopulateEvent.html" rel="external">UnityEngine.UIElements.ContextualMenuPopulateEvent</a>) event holding the menu to populate.</param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction($"Set as Root Node", _ => onSetRootNode?.Invoke(m_node));
            base.BuildContextualMenu(evt);
        }

        #endregion

        #region Overrides of GraphElement

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.GraphElement.OnSelected.html" rel="external">UnityEditor.Experimental.GraphView.GraphElement.OnSelected</a>
        /// Called when the GraphElement is selected.
        /// </summary>
        public override void OnSelected()
        {
            onNodeSelected?.Invoke(m_node);
            base.OnSelected();
        }

        #endregion
    }
}