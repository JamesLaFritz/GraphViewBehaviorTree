using UnityEditor;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
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
        }
    }
}