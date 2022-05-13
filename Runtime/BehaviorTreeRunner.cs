// BehaviorTreeRunner.cs
// 05-13-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [SerializeField] private BehaviorTree m_tree;

        private void Start()
        {
            m_tree = ScriptableObject.CreateInstance<BehaviorTree>();

            DebugLogNode log = ScriptableObject.CreateInstance<DebugLogNode>();
            log.message = "Testing 1,2,3.";

            RepeatNode loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.child = log;

            m_tree.rootNode = loop;
        }

        private void Update()
        {
            m_tree.Update();
        }
    }
}