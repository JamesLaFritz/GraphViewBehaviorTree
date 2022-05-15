// BehaviorTreeView.cs
// 05-15-2022
// James LaFritz

using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

        public BehaviorTreeView()
        {
            style.flexGrow = 1;
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
    }
}