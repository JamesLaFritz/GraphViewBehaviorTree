// DecoratorNode.cs
// 05-05-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// A Node that has one childNode and is capable of augmenting the return state of it's childNode.
    /// The Interface, Abstract members will be implemented in the individual Decorator Nodes.
    /// </summary>
    [System.Serializable]
    public abstract class DecoratorNode : Node
    {
        [SerializeField, HideInInspector] protected Node child;

        #region Overrides of Node

        /// <inheritdoc />
        public override void AddChild(Node childNode)
        {
            child = childNode;
        }

        /// <inheritdoc />
        public override void RemoveChild(Node childNode)
        {
            if (child == childNode)
                child = null;
        }

        /// <inheritdoc />
        public override List<Node> GetChildren()
        {
            return new List<Node>() { child };
        }

        /// <inheritdoc />
        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();

            return node;
        }

        #endregion
    }
}