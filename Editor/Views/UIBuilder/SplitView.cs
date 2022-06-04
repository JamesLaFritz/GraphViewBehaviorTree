// SplitView.cs
// 05-15-2022
// James LaFritz

using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor.Views
{
    /// <summary>
    /// <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/UIElements.TwoPaneSplitView.html" rel="external">UnityEngine.UIElements.TwoPaneSplitView</a>
    /// A SplitView that contains two resizable panes. One pane is fixed-size while the other pane has flex-grow style set to 1 to take all remaining space.
    /// The border between he panes is draggable to resize both panes. Both horizontal and vertical modes are supported.
    /// Requires _exactly_ two child elements to operate.
    /// 
    /// Allows you to add a split view in the <a href="https://docs.unity3d.com/Packages/com.unity.ui.builder@1.0/manual/index.html" rel="external">UI Builder</a>.
    /// </summary>
    public class SplitView : TwoPaneSplitView
    {
        /// <summary>
        /// Required in order to have <see cref="SplitView"/> show up in the UI Builder Library.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
    }
}