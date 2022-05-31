// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// A View for Behavior Tree <see cref="Node"/>, derived from <see cref="UnityEditor.Experimental.GraphView.Node"/>
    /// </summary>
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private Node m_node;

        /// <summary>
        /// Notifies the Observers that a <see cref="Node"/> has been Selected and pass the <see cref="Node"/> that was selected.
        /// </summary>
        public Action<Node> onNodeSelected;

        /// <summary>
        /// Notifies Observers that the set root node has been selected. Pass the <see cref="Node"/> that was selected to be set as the root node.
        /// </summary>
        public Action<Node> onSetRootNode;

        /// <summary>
        /// The <see cref="Node"/> that is associate with this view.
        /// </summary>
        public Node node => m_node;

        /// <summary>
        /// The Input <see cref="UnityEditor.Experimental.GraphView.Port"/>.
        /// </summary>
        public Port input;

        /// <summary>
        /// The Output <see cref="UnityEditor.Experimental.GraphView.Port"/>.
        /// </summary>
        public Port output;

        /// <summary>
        /// Create a New Node View.
        /// </summary>
        /// <param name="node"><see cref="Node"/> that is associated with this view.</param>
        public BehaviorTreeNodeView(Node node) : base("Assets/GraphViewBehaviorTree/Editor/Resources/BehaviorTreeNodeView.uxml")
        {
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

        public void SortChildren()
        {
            CompositeNode composite = m_node as CompositeNode;

            if (composite != null)
                composite.GetChildren().Sort(SortByHorizontalPosition);
        }

        private int SortByHorizontalPosition(Node node1, Node node2)
        {
            return node1.nodeGraphPosition.x < node2.nodeGraphPosition.x ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");
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

        /// <inheritdoc />
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(m_node, "Behavior Tree (Set Position)");
            m_node.nodeGraphPosition.x = newPos.xMin;
            m_node.nodeGraphPosition.y = newPos.yMin;
            EditorUtility.SetDirty(m_node);
        }

        /// <inheritdoc />
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction($"Set as Root Node", _ => onSetRootNode?.Invoke(m_node));
            base.BuildContextualMenu(evt);
        }

        #endregion

        #region Overrides of GraphElement

        /// <inheritdoc />
        public override void OnSelected()
        {
            onNodeSelected?.Invoke(m_node);
            base.OnSelected();
        }

        #endregion
    }
}