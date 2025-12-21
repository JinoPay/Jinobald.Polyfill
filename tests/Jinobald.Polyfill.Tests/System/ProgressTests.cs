using System;
using System.Threading;
using Xunit;

namespace Jinobald.Polyfill.Tests.System
{
#if NET35 || NET40
    public class ProgressTests
    {
        [Fact]
        public void Progress_CreateInstance_SuccessfullyCreated()
        {
            var progress = new Progress<int>();

            Assert.NotNull(progress);
        }

        [Fact]
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

            Assert.True(eventRaised);
            Assert.Equal(42, reportedValue);
        }

        [Fact]
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

            Assert.Equal(3, callCount);
        }

        [Fact]
        public void Progress_WithoutSubscriber_NoException()
        {
            var progress = new Progress<int>();

            // Should not throw an exception
            progress.Report(42);
        }

        [Fact]
        public void Progress_MultipleSubscribers_AllReceiveNotification()
        {
            var progress = new Progress<int>();
            int subscriber1Count = 0;
            int subscriber2Count = 0;

            progress.ProgressChanged += (sender, args) => subscriber1Count++;
            progress.ProgressChanged += (sender, args) => subscriber2Count++;

            progress.Report(42);

            Assert.Equal(1, subscriber1Count);
            Assert.Equal(1, subscriber2Count);
        }

        [Fact]
        public void Progress_WithString_ReportsStringValue()
        {
            var progress = new Progress<string>();
            string reportedValue = null;

            progress.ProgressChanged += (sender, args) =>
            {
                reportedValue = args.Result;
            };

            progress.Report("test");

            Assert.Equal("test", reportedValue);
        }

        [Fact]
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

                Assert.Equal(42, reportedValue);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(originalContext);
            }
        }
    }
#endif
}
