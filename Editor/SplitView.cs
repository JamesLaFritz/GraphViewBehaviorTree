// SplitView.cs
// 05-15-2022
// James LaFritz

using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
    }
}