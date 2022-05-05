using UnityEngine;

namespace GraphViewBehaviorTree
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            /// <summary>
            /// The Node is Running
            /// </summary>
            Running,

            /// <summary>
            /// The Node has completed successfully.
            /// </summary>
            Success,

            /// <summary>
            /// The Node has completed unsuccessfully;
            /// </summary>
            Failure
        }

        [SerializeField] private State state = State.Running;

        [SerializeField] private bool started;

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            // if the state is running the state is not failure or not success (in case I decide to add other states latter).
            if (state != State.Failure && state != State.Success) return state;
            OnStop();
            started = false;
            return state;
        }
    }
}
