// CompositeNode.cs
// 05-13-2022
// James LaFritz

using System.Collections.Generic;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// Has a list of children and is the control flow of the behavior tree like switch statements and for loops.
    /// There are 2 types Composite Nodes the Selector and Sequence node.
    /// </summary>
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
    }
}