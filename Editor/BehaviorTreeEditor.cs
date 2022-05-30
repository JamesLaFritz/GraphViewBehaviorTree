using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView m_treeView;
        private IMGUIContainer m_inspectorView;
        private UnityEditor.Editor m_editor;

        [MenuItem("Window/Behavior Tree/Editor")]
        public static void OpenTreeEditor()
        {
            GetWindow<BehaviorTreeEditor>("Behavior Tree Editor");
        }

        /// <summary>
        /// Use Unity Editor Call Back On Open Asset.
        /// </summary>
        /// <returns>True if this method handled the asset. Else return false.</returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is BehaviorTree)
            {
                OpenTreeEditor();
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            VisualTreeAsset visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/GraphViewBehaviorTree/Editor/BehaviorTreeEditor.uxml");
            visualTree.CloneTree(rootVisualElement);

            m_treeView = rootVisualElement.Q<BehaviorTreeView>();
            m_inspectorView = rootVisualElement.Q<IMGUIContainer>("InspectorView");
            m_treeView.onNodeSelected = OnNodeSelectionChange;

            OnSelectionChange();
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

        private void OnNodeSelectionChange(Node node)
        {
            m_inspectorView.Clear();
            DestroyImmediate(m_editor);
            m_editor = UnityEditor.Editor.CreateEditor(node);
            m_inspectorView.onGUIHandler = () => { m_editor.OnInspectorGUI(); };
        }
    }
}