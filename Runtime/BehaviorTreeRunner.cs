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

            DebugLogNode log1 = ScriptableObject.CreateInstance<DebugLogNode>();
            log1.message = "Testing 1";
            DebugLogNode log2 = ScriptableObject.CreateInstance<DebugLogNode>();
            log2.message = "Testing 2";
            DebugLogNode log3 = ScriptableObject.CreateInstance<DebugLogNode>();
            log3.message = "Testing 3";

            ActionNode wait1 = ScriptableObject.CreateInstance<WaitNode>();
            ActionNode wait2 = ScriptableObject.CreateInstance<WaitNode>();
            ActionNode wait3 = ScriptableObject.CreateInstance<WaitNode>();

            CompositeNode sequence = ScriptableObject.CreateInstance<SequencerNode>();
            sequence.children.Add(log1);
            sequence.children.Add(wait1);
            sequence.children.Add(log2);
            sequence.children.Add(wait2);
            sequence.children.Add(log3);
            sequence.children.Add(wait3);

            DecoratorNode loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.child = sequence;

            m_tree.rootNode = loop;
        }

        private void Update()
        {
            m_tree.Update();
        }
    }
}