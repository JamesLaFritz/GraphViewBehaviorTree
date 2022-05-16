// BehaviorTreeNodeView.cs
// 05-15-2022
// James LaFritz

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {
        Node m_node;

        public BehaviorTreeNodeView(Node node)
        {
            m_node = node;
            base.title = node.name;

            SerializedObject so = new SerializedObject(m_node);

            VisualTreeAsset visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/GraphViewBehaviorTree/Editor/BehaviorTreeEditor.uxml");
            visualTree.CloneTree(this);
            this.Bind(so);
        }
    }
}