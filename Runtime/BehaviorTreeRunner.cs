// BehaviorTreeRunner.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    /// <inheritdoc />
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [SerializeField] public BehaviorTree tree;

        private bool m_hasTree;

        private void Start()
        {
            m_hasTree = tree != null;
            if (!m_hasTree) return;

            tree = tree.Clone();
        }

        private void Update()
        {
            m_hasTree = tree != null;
            if (!m_hasTree) return;

            tree.Update();
        }
    }
}