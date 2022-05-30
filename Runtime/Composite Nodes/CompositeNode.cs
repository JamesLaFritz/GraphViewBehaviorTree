// CompositeNode.cs
// 05-13-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Has a list of children and is the control flow of the behavior tree like switch statements and for loops.
    /// There are 2 types Composite Nodes the Selector and Sequence node.
    /// </summary>
    [System.Serializable]
    public abstract class CompositeNode : Node
    {
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