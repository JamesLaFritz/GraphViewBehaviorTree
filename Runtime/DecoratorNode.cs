// DecoratorNode.cs
// 05-05-2022
// James LaFritz

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// A Node that has one child and is capable of augmenting the return state of it's child.
    /// The Interface, Abstract members will be implemented in the individual Decorator Nodes.
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        public Node child;
    }
}