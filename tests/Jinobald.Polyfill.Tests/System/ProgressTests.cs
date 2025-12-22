using System;
using System.Threading;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System
{
#if NET35 || NET40
    public class ProgressTests
    {
        [Test]
        public void Progress_CreateInstance_SuccessfullyCreated()
        {
            var progress = new Progress<int>();

            Assert.IsNotNull(progress);
        }

        [Test]
        public void Progress_Report_RaisesProgressChanged()
        {
            var progress = new Progress<int>();
            int reportedValue = 0;
            bool eventRaised = false;

            progress.ProgressChanged += (sender, args) =>
            {
                eventRaised = true;
                reportedValue = args.Result;
            };

            progress.Report(42);

            Assert.IsTrue(eventRaised);
            Assert.AreEqual(42, reportedValue);
        }

        [Test]
        public void Progress_MultipleReports_RaisesMultipleEvents()
        {
            var progress = new Progress<int>();
            int callCount = 0;

            progress.ProgressChanged += (sender, args) =>
            {
                callCount++;
            };

            progress.Report(1);
            progress.Report(2);
            progress.Report(3);

            Assert.AreEqual(3, callCount);
        }

        [Test]
        public void Progress_WithoutSubscriber_NoException()
        {
            var progress = new Progress<int>();

            // Should not throw an exception
            progress.Report(42);
        }

        [Test]
        public void Progress_MultipleSubscribers_AllReceiveNotification()
        {
            var progress = new Progress<int>();
            int subscriber1Count = 0;
            int subscriber2Count = 0;

            progress.ProgressChanged += (sender, args) => subscriber1Count++;
            progress.ProgressChanged += (sender, args) => subscriber2Count++;

            progress.Report(42);

            Assert.AreEqual(1, subscriber1Count);
            Assert.AreEqual(1, subscriber2Count);
        }

        [Test]
        public void Progress_WithString_ReportsStringValue()
        {
            var progress = new Progress<string>();
            string? reportedValue = null;

            progress.ProgressChanged += (sender, args) =>
            {
                reportedValue = args.Result;
            };

            progress.Report("test");

            Assert.AreEqual("test", reportedValue);
        }

        [Test]
        public void Progress_SynchronizationContext_CapturedOnCreation()
        {
            var originalContext = SynchronizationContext.Current;
            try
            {
                // Set a custom synchronization context
                var customContext = new SynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(customContext);

                var progress = new Progress<int>();
                int reportedValue = 0;

                progress.ProgressChanged += (sender, args) =>
                {
                    reportedValue = args.Result;
                };

                progress.Report(42);

                Assert.AreEqual(42, reportedValue);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(originalContext);
            }
        }
    }
#endif
}
