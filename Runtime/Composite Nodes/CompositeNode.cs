// CompositeNode.cs
// 05-13-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// <see cref="Node"/> that has a list of children.
    /// </summary>
    [System.Serializable]
    public abstract class CompositeNode : Node
    {
        /// <summary>
        /// The Children that this <see cref="Node"/> contains.
        /// </summary>
        [SerializeField, HideInInspector] protected List<Node> children = new List<Node>();

        #region Overrides of Node

        /// <inheritdoc />
        public override void AddChild(Node childNode)
        {
            children.Add(childNode);
        }

        /// <inheritdoc />
        public override void RemoveChild(Node childNode)
        {
            children.Remove(childNode);
        }

        /// <inheritdoc />
        public override List<Node> GetChildren()
        {
            return children;
        }

        #endregion
    }
}