using Rogue.Core;
using Rogue.Core.Betree;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public abstract class NodeJob<T> : NodeBase where T : Jobs.Job
    {
        private enum JobState
        {
            Testing, // Testing the job preconditions.
            Invalid, // The job does not pass the preconditions.
            Running, // The job is in progress.
            Success, // The job has been done successful.
            Failure  // There was a problem with the job.
        }

        private JobState m_jobState = JobState.Testing;

        protected T m_job;

        protected Node m_run;

        protected Node m_cancel;

        public NodeJob(Node run, Node cancel)
        {
            m_run    = run;
            m_cancel = cancel;
        }

        public virtual bool OnJobAccept(T job) => true;

        public virtual void OnJobStart() => ReserveJob();

        public virtual void OnJobSuccess() {}

        public virtual void OnJobFailure() => ReleaseJob();

        public virtual void OnJobAbort() {}

        public override void OnAttached(Core.Betree.Tree tree, Node parent)
        {
            base.OnAttached(tree, parent);

            m_run   ?.OnAttached(tree, this);
            m_cancel?.OnAttached(tree, this);
        }

        public override void OnInit()
        {
            m_jobState = JobState.Testing;
        }

        public override void OnQuit(NodeState state)
        {
            if (state != NodeState.Aborted)
            {
                return;
            }

            OnJobAbort();
        }

        public override NodeState OnUpdate() => Step();

        public NodeState Step()
        {
            //Debug.Log("Step of job: " + this.GetType().Name);

            switch (m_jobState)
            {
                // Test the job preconditions.
                case JobState.Testing:
                {
                    if (OnTesting())
                    {
                        m_jobState = JobState.Running;
                        OnJobStart();
                    }
                    else
                    {
                        m_jobState = JobState.Invalid;
                    }

                    return Step();
                    /*
                    NodeState test = OnTesting();

                    switch (test)
                    {
                        case NodeState.Success: m_jobState = JobState.Running; return Step();
                        case NodeState.Failure: m_jobState = JobState.Failure; return Step();

                        case NodeState.Running: return NodeState.Running;
                        default:                return test;
                    }
                    */
                }
                case JobState.Invalid:
                {
                    return NodeState.Failure;
                }
                // The job is done successful.
                case JobState.Success: 
                {
                    OnSuccess();
                    return NodeState.Success;
                }
                // There was a problem with the job so it is cancelled. In any case, the job is reported a failure.
                case JobState.Failure:
                {
                    OnFailure();
                    NodeState cancel = m_cancel != null ? m_cancel.Tick() : NodeState.Success;

                    switch (cancel)
                    {
                        case NodeState.Success: return NodeState.Failure;
                        case NodeState.Failure: return NodeState.Failure;

                        case NodeState.Running: return NodeState.Running;
                        default:                return cancel;
                    }
                }
                // Running, try to do the job.
                default:
                {
                    NodeState run = m_run.Tick();

                    switch (run)
                    {
                        case NodeState.Success: m_jobState = JobState.Success; return Step();
                        case NodeState.Failure: m_jobState = JobState.Failure; return Step();

                        case NodeState.Running: return NodeState.Running;
                        default:                return run;
                    }
                }
            }
        }

        public bool OnTesting()
        {
            if (!Blackboard.TryGet("job", out T job))
            {
                return false;
            }

            if (!OnJobAccept(job))
            {
                return false;
            }

            m_job = job;

            return true;
        }

        public void OnSuccess()
        {
            OnJobSuccess();
        }

        public void OnFailure()
        {
            Debug.Log("Job failure");
            OnJobFailure();
        }

        #region @@@ JOB HELPERS @@@

        protected void ReserveJob() => m_job.Reserve();

        protected void CompleteJob() => m_job.Complete();

        protected void ReleaseJob()  => m_job.Release();

        #endregion
    }
}
