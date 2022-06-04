// DecoratorNode.cs
// 05-05-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree.Nodes
{
    /// <summary>
    /// <see cref="Node"/> that has one child Node and is capable of augmenting the return state of it's childNode.
    /// The Interface, Abstract members will be implemented in the individual Decorator Nodes.
    /// </summary>
    [System.Serializable]
    public abstract class DecoratorNode : Node
    {
        /// <value>
        /// The <see cref="Node"/> to Augment.
        /// </value>
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
            return new List<Node> { child };
        }

        #endregion
    }
}