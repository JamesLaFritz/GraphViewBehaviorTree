using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
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
            if (tree != null)
            {
                SerializedObject so = new SerializedObject(tree);
                rootVisualElement.Bind(so);
                if (m_treeView != null)
                    m_treeView.PopulateView(tree);
            }
            else
            {
                rootVisualElement.Unbind();

                TextField textField = rootVisualElement.Q<TextField>("BehaviorTreeName");
                if (textField != null)
                {
                    textField.value = string.Empty;
                }
            }
        }
    }
}