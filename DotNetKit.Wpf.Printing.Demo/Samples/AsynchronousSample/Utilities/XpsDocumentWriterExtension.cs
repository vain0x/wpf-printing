using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using DotNetKit.Misc.Disposables;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample.Utilities
{
    public static class XpsDocumentWriterExtension
    {
        /// <summary>
        /// Converts the event-based WriteAsync to a task-based one.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static TaskCompletionSource<object>
            WriteAsyncAsTaskCore(
                XpsDocumentWriter @this,
                CancellationToken cancellationToken
            )
        {
            var tcs = new TaskCompletionSource<object>();
            var subscription = new SingleAssignmentDisposable();

            var onCompleted =
                new WritingCompletedEventHandler((sender, e) =>
                {
                    using (subscription)
                    {
                        if (e.Error != null)
                        {
                            tcs.SetException(e.Error);
                        }
                        else if (e.Cancelled)
                        {
                            tcs.SetCanceled();
                        }
                        else
                        {
                            tcs.SetResult(default(object));
                        }
                    }
                });
            var onCancelled =
                new WritingCancelledEventHandler((sender, e) =>
                {
                    using (subscription)
                    {
                        if (e.Error != null)
                        {
                            tcs.SetException(e.Error);
                        }
                        else
                        {
                            tcs.SetCanceled();
                        }
                    }
                });

            @this.WritingCompleted += onCompleted;
            @this.WritingCancelled += onCancelled;
            var registration = cancellationToken.Register(@this.CancelAsync);

            subscription.Content =
                new AnonymousDisposable(() =>
                {
                    @this.WritingCompleted -= onCompleted;
                    @this.WritingCancelled -= onCancelled;
                    registration.Dispose();
                });
            return tcs;
        }

        /// <summary>
        /// Starts a task to print a fixed document asynchronously.
        /// </summary>
        public static Task
            WriteAsyncAsTask(
                this XpsDocumentWriter @this,
                FixedDocument document,
                CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var tcs = WriteAsyncAsTaskCore(@this, cancellationToken);
            @this.WriteAsync(document);
            return tcs.Task;
        }
    }
}
