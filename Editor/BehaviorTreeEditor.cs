using UnityEditor;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView m_treeView;

        [MenuItem("Window/Behavior Tree/Editor")]
        public static void OpenTreeEditor()
        {
            GetWindow<BehaviorTreeEditor>("Behavior Tree Editor");
        }

        public void CreateGUI()
        {
            VisualTreeAsset visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/GraphViewBehaviorTree/Editor/BehaviorTreeEditor.uxml");
            visualTree.CloneTree(rootVisualElement);

            m_treeView = rootVisualElement.Q<BehaviorTreeView>();
            rootVisualElement.Q<VisualElement>("InspectorView");
        }

        private void OnSelectionChange()
        {
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (!tree) return;

            m_treeView.PopulateView(tree);
        }
    }
}