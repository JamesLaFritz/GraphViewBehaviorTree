using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// Derive from <see cref="EditorWindow"/> class to create an editor window to Edit Behavior Tree Scriptable Objects.
    /// Requires file named "BehaviorTreeEditor.uxml" to be in an Editor Resources Folder
    /// Uses Visual Elements requires a <see cref="BehaviorTreeView"/> an an <see cref="IMGUIContainer"/> with a name of InspectorView.
    /// </summary>
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView m_treeView;
        private IMGUIContainer m_inspectorView;
        private UnityEditor.Editor m_editor;

        /// <summary>
        /// Adds a Entry to Window/Behavior Tree/Editor
        /// Will Open the Behavior Tree Editor to Edit Behavior Trees
        /// </summary>
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
            if (Selection.activeObject is not BehaviorTree) return false;
            OpenTreeEditor();
            return true;
        }

        /// <summary>
        /// CreateGUI is called when the EditorWindow's rootVisualElement is ready to be populated.
        ///
        /// Clones a Visual Tree Located in an Editor Resources Folder BehaviorTreeEditor.uxml";
        /// </summary>
        public void CreateGUI()
        {
            //VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("BehaviorTreeEditor.uxml");
            VisualTreeAsset vt = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GraphViewBehaviorTree/Editor/Resources/BehaviorTreeEditor.uxml");
            vt.CloneTree(rootVisualElement);

            m_treeView = rootVisualElement.Q<BehaviorTreeView>();
            m_inspectorView = rootVisualElement.Q<IMGUIContainer>("InspectorView");
            m_treeView.onNodeSelected = OnNodeSelectionChange;

            OnSelectionChange();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnplayModeStateChanged;
            EditorApplication.playModeStateChanged += OnplayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnplayModeStateChanged;
        }

        private void OnplayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }

        /// <summary>
        /// Called whenever the selection has changed.
        ///
        /// If the Selected Object is a Behavior Tree Binds the Tree SO to the root element and populates the tree view.
        /// </summary>
        private void OnSelectionChange()
        {
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (tree == null)
            {
                if (Selection.activeGameObject)
                {
                    BehaviorTreeRunner treeRunner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                    if (treeRunner)
                    {
                        tree = treeRunner.tree;
                    }
                }
            }

            if (tree != null)
            {
                if (Application.isPlaying || AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                {
                    SerializedObject so = new SerializedObject(tree);
                    rootVisualElement.Bind(so);
                    if (m_treeView != null)
                        m_treeView.PopulateView(tree);

                    return;
                }
            }

            rootVisualElement.Unbind();

            TextField textField = rootVisualElement.Q<TextField>("BehaviorTreeName");
            if (textField != null)
            {
                textField.value = string.Empty;
            }
        }

        /// <summary>
        /// Used to Observer the tree view for when a Node is Selected.
        /// Causes the Node to display in the Inspector View.
        /// </summary>
        /// <param name="node">The Selected Node</param>
        private void OnNodeSelectionChange(Node node)
        {
            m_inspectorView.Clear();
            DestroyImmediate(m_editor);
            m_editor = UnityEditor.Editor.CreateEditor(node);
            m_inspectorView.onGUIHandler = () =>
            {
                if (m_editor.target)
                    m_editor.OnInspectorGUI();
            };
        }

        public void OnInspectorUpdate()
        {
            m_treeView?.UpdateNodeStates();
        }
    }
}