// BehaviorTreeRunner.cs
// 05-13-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [SerializeField] private BehaviorTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviorTree>();

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
            sequence.children = new List<Node> { log1, wait1, log2, wait2, log3, wait3 };

            DecoratorNode loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.child = sequence;

            tree.rootNode = loop;
        }

        private void Update()
        {
            tree.Update();
        }
    }
}