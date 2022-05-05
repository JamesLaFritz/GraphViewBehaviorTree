// ActionNode.cs
// 05-05-2022
// James LaFritz

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// The Leaf of the tree, has no children, and is where all of the logic gets implemented.
    /// The Interface, Abstract members will be implemented in the individual Action Nodes.
    /// </summary>
    public abstract class ActionNode : Node { }
}