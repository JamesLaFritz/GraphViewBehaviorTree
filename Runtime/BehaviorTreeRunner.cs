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

            Node wait1 = ScriptableObject.CreateInstance<WaitNode>();
            Node wait2 = ScriptableObject.CreateInstance<WaitNode>();
            Node wait3 = ScriptableObject.CreateInstance<WaitNode>();

            Node sequence = ScriptableObject.CreateInstance<SequencerNode>();
            sequence.AddChild(log1);
            sequence.AddChild(wait1);
            sequence.AddChild(wait2);
            sequence.AddChild(log3);
            sequence.AddChild(wait3);

            Node loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.AddChild(sequence);

            tree.rootNode = loop;
        }

        private void Update()
        {
            tree.Update();
        }
    }
}