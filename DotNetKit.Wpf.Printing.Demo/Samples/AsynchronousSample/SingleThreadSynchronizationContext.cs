using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample
{
    // NOTE: Don't reuse this because the implementation is too simple.
    public sealed class SingleThreadSynchronizationContext
        : SynchronizationContext
    {
        public Thread Thread { get; }

        readonly Queue<Action> jobs = new Queue<Action>();

        bool isStarted;
        bool isStopped;

        public void Run()
        {
            SetSynchronizationContext(this);

            var wait = new SpinWait();

            while (true)
            {
                wait.SpinOnce();

                var job = default(Action);
                lock (jobs)
                {
                    if (jobs.Count == 0)
                    {
                        if (isStopped) break;
                        continue;
                    }
                    else
                    {
                        job = jobs.Dequeue();
                    }
                }

                // NOTE: `job` must NOT throw exceptions.
                job();
            }
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotImplementedException();
        }

        public void Post(Action job)
        {
            lock (jobs)
            {
                jobs.Enqueue(job);
            }
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            Post(() => d(state));
        }

        public void Start()
        {
            if (isStarted) return;
            isStarted = true;
            Thread.Start();
        }

        public void Dispose()
        {
            if (isStopped) return;
            isStopped = true;

            if (isStarted)
            {
                Thread.Join();
            }
        }

        public SingleThreadSynchronizationContext()
        {
            Thread = new Thread(Run);
        }
    }
}
