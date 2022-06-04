// BehaviorTreeRunner.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <summary>
    /// <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.html" rel="external">"UnityEngine.MonoBehaviour"</a>
    /// That allows you to run a <see cref="BehaviorTree"/> in Unity.
    /// </summary>
    public class BehaviorTreeRunner : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="BehaviorTree"/> to run.
        /// </summary>
        [SerializeField] public BehaviorTree tree;

        /// <summary>
        /// Is there a tree.
        /// </summary>
        private bool m_hasTree;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            m_hasTree = tree != null;
            if (!m_hasTree) return;

            tree = tree.Clone();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            m_hasTree = tree != null;
            if (!m_hasTree) return;

            tree.Update();
        }
    }
}