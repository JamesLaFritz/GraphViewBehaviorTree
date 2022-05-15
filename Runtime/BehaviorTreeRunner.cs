// BehaviorTreeRunner.cs
// 05-13-2022
// James LaFritz

using System.Collections.Generic;
using UnityEngine;

namespace GraphViewBehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [SerializeField] private BehaviorTree m_tree;

        private void Start()
        {
            m_tree = ScriptableObject.CreateInstance<BehaviorTree>();

            DebugLogNode log1 = new DebugLogNode
            {
                message = "Testing 1"
            };
            DebugLogNode log2 = new DebugLogNode
            {
                message = "Testing 1"
            };
            DebugLogNode log3 = new DebugLogNode
            {
                message = "Testing 1"
            };

            ActionNode wait1 = new WaitNode();
            ActionNode wait2 = new WaitNode();
            ActionNode wait3 = new WaitNode();

            CompositeNode sequence = new SequencerNode()
            {
                children = new List<Node>()
                {
                    log1, wait1, log2, wait2, log3, wait3
                }
            };

            DecoratorNode loop = new RepeatNode()
            {
                child = sequence
            };

            m_tree.rootNode = loop;
        }

        private void Update()
        {
            m_tree.Update();
        }
    }
}