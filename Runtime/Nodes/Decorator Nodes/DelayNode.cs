// DelayNode.cs
// 06-01-2022
// James LaFritz

using UnityEngine;

namespace GraphViewBehaviorTree.Nodes
{
    /// <summary>
    /// Causes a delay in operations if the delay time is set.
    /// Time To Start is Delaying before the Node Updates. (Node is Running but not Started) OnUpdate is Called after Node is Started.
    /// Time To Stop is Delaying after the Node Finishes Running. (Node is not running, and is stopping) Causes delay in started to being set to false.
    /// </summary>
    public class DelayNode : DecoratorNode
    {
        /// <value>
        /// Amount of time to delay before Starting the node.
        /// </value>
        [SerializeField] private float timeToStart;

        /// <value>
        /// The time the node started
        /// </value>
        private float m_startTime;

        /// <value>
        /// Amount of time to delay before Stopping the node.
        /// </value>
        [SerializeField] private float timeToStop;

        /// <value>
        /// The time the node stopped running.
        /// </value>
        private float m_stopTime;

        #region Overrides of Node

        /// <inheritdoc />
        public override void NodeReset()
        {
            m_startTime = 0.01f;
            m_stopTime = 0.01f;
            base.NodeReset();
            m_startTime = 0;
            m_stopTime = 0;
        }

        /// <inheritdoc />
        protected override void OnStart()
        {
            if (m_startTime <= 0 && timeToStart != 0)
                m_startTime = timeToStart;

            m_startTime -= Time.deltaTime;

            if (m_startTime > 0)
                IsStarted = false;
        }

        /// <inheritdoc />
        protected override void OnStop()
        {
            if (m_stopTime <= 0 && timeToStop != 0)
                m_stopTime = timeToStop;

            m_stopTime -= Time.deltaTime;

            if (m_stopTime > 0)
                IsStarted = true;
        }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            return state = child.Update();
        }

        #endregion
    }
}