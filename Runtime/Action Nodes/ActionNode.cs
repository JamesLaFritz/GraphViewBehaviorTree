// ActionNode.cs
// 05-05-2022
// James LaFritz

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// A <see cref="Node"/> that has no children, is the Leaf of the tree, and is where all of the logic gets implemented.
    /// The Interface, Abstract members will be implemented in the individual Action Nodes.
    /// </summary>
    [System.Serializable]
    public abstract class ActionNode : Node { }
}