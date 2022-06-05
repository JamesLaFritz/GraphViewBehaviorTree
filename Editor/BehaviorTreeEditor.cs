using GraphViewBehaviorTree.Editor.Views;
using GraphViewBehaviorTree.Nodes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphViewBehaviorTree.Editor
{
    /// <summary>
    /// Derive from <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/EditorWindow.html" rel="external">UnityEditor.EditorWindow</a> class to create an editor window to Edit Behavior Tree Scriptable Objects.
    /// Requires file named "BehaviorTreeEditor.uxml" to be in an Editor Resources Folder
    /// Uses Visual Elements requires a <see cref="BehaviorTreeView"/> an an <a href="https://docs.unity3d.com/ScriptReference/UIElements.IMGUIContainer.html" rel="external">UnityEngine.UIElements.IMGUIContainer</a> with a name of InspectorView.
    /// </summary>
    public class BehaviorTreeEditor : EditorWindow
    {
        /// <value> The <see cref="TreeView"/> associated with this view. </value>
        private BehaviorTreeView m_treeView;

        /// <value> The <a href="https://docs.unity3d.com/ScriptReference/UIElements.IMGUIContainer.html" rel="external">UnityEngine.UIElements.IMGUIContainer</a> associated with the view. </value>
        private IMGUIContainer m_inspectorView;

        /// <value> The <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Editor.html" rel="external">UnityEditor.Editor</a> associated with this view. </value>
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
        private void CreateGUI()
        {
            //VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("BehaviorTreeEditor.uxml");
            VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("BehaviorTreeEditor");
            vt.CloneTree(rootVisualElement);

            m_treeView = rootVisualElement.Q<BehaviorTreeView>();
            m_inspectorView = rootVisualElement.Q<IMGUIContainer>("InspectorView");
            m_treeView.onNodeSelected = OnNodeSelectionChange;

            OnSelectionChange();
        }

        /// <summary>
        /// This function is called when the object is loaded.
        /// </summary>
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnplayModeStateChanged;
            EditorApplication.playModeStateChanged += OnplayModeStateChanged;
        }

        /// <summary>
        /// This function is called when the scriptable object goes out of scope.
        /// </summary>
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnplayModeStateChanged;
        }

        /// <summary>
        /// OnInspectorUpdate is called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        private void OnInspectorUpdate()
        {
            m_treeView?.UpdateNodeStates();
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
        /// Method registered to <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/EditorApplication-playModeStateChanged.html" rel="external">UnityEditor.EditorApplication.playModeStateChanged</a>
        /// </summary>
        /// <param name="obj">The <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/PlayModeStateChange.html" rel="external">UnityEditor.PlayModeStateChange</a> object.</param>
        private void OnplayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                // Occurs during the next update of the Editor application if it is in edit mode and was previously in play mode.
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                // Occurs when exiting edit mode, before the Editor is in play mode.
                case PlayModeStateChange.ExitingEditMode:
                    break;
                // Occurs during the next update of the Editor application if it is in play mode and was previously in edit mode.
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                // Occurs when exiting play mode, before the Editor is in edit mode.
                case PlayModeStateChange.ExitingPlayMode:
                    break;
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
    }
}