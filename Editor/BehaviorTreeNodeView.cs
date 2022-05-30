// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// A View for Behavior Tree Nodes, based on the Graph View Node
    /// </summary>
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private Node m_node;

        public Action<Node> onNodeSelected;

        public Action<Node> onSetRootNode;

        public Node node => m_node;

        public Port input;
        public Port output;

        public BehaviorTreeNodeView(Node node)
        {
            m_node = node;
            if (m_node == null) return;
            base.title = m_node.GetType().Name;
            viewDataKey = m_node.guid;
            style.left = m_node.nodeGraphPosition.x;
            style.top = m_node.nodeGraphPosition.y;

            CreateInputPorts();
            CreateOutputPorts();
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
            outputContainer.Add(output);
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            m_node.nodeGraphPosition.x = newPos.xMin;
            m_node.nodeGraphPosition.y = newPos.yMin;
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