using NUnit.Framework;
using System.Threading;

namespace Jinobald.Polyfill.Tests.System.Threading;

public class ThreadingUtilitiesTests
{
    [Test]
    public void ManualResetEventSlim_InitialState_False()
    {
        using (var mre = new ManualResetEventSlim(false))
        {
            Assert.IsFalse(mre.IsSet);
        }
    }

    [Test]
    public void ManualResetEventSlim_InitialState_True()
    {
        using (var mre = new ManualResetEventSlim(true))
        {
            Assert.IsTrue(mre.IsSet);
        }
    }

    [Test]
    public void ManualResetEventSlim_Set_SignalsEvent()
    {
        using (var mre = new ManualResetEventSlim(false))
        {
            mre.Set();
            Assert.IsTrue(mre.IsSet);
        }
    }

    [Test]
    public void ManualResetEventSlim_Reset_ResetsEvent()
    {
        using (var mre = new ManualResetEventSlim(true))
        {
            mre.Reset();
            Assert.IsFalse(mre.IsSet);
        }
    }

    [Test]
    public void ManualResetEventSlim_Wait_ReturnsImmediatelyIfSignaled()
    {
        using (var mre = new ManualResetEventSlim(true))
        {
            Assert.IsTrue(mre.Wait(100));
        }
    }

    [Test]
    public void ManualResetEventSlim_Wait_TimesOutIfNotSignaled()
    {
        using (var mre = new ManualResetEventSlim(false))
        {
            Assert.IsFalse(mre.Wait(50));
        }
    }

    [Test]
    public void SemaphoreSlim_Constructor_ValidatesParameters()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemaphoreSlim(-1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemaphoreSlim(0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemaphoreSlim(2, 1));
    }

    [Test]
    public void SemaphoreSlim_InitialCount_IsCorrect()
    {
        using (var sem = new SemaphoreSlim(3, 5))
        {
            Assert.AreEqual(3, sem.CurrentCount);
        }
    }

    [Test]
    public void SemaphoreSlim_Wait_DecrementsCount()
    {
        using (var sem = new SemaphoreSlim(2, 5))
        {
            sem.Wait();
            Assert.AreEqual(1, sem.CurrentCount);
            sem.Wait();
            Assert.AreEqual(0, sem.CurrentCount);
        }
    }

    [Test]
    public void SemaphoreSlim_Release_IncrementsCount()
    {
        using (var sem = new SemaphoreSlim(1, 5))
        {
            sem.Wait();
            Assert.AreEqual(0, sem.CurrentCount);

            int prevCount = sem.Release();
            Assert.AreEqual(0, prevCount);
            Assert.AreEqual(1, sem.CurrentCount);
        }
    }

    [Test]
    public void SemaphoreSlim_Release_ThrowsWhenFull()
    {
        using (var sem = new SemaphoreSlim(3, 3))
        {
            Assert.Throws<SemaphoreFullException>(() => sem.Release());
        }
    }

    [Test]
    public void SemaphoreSlim_WaitWithTimeout_ReturnsTrue()
    {
        using (var sem = new SemaphoreSlim(1, 1))
        {
            Assert.IsTrue(sem.Wait(100));
        }
    }

    [Test]
    public void SemaphoreSlim_WaitWithTimeout_ReturnsFalse()
    {
        using (var sem = new SemaphoreSlim(0, 1))
        {
            Assert.IsFalse(sem.Wait(50));
        }
    }

    [Test]
    public void CountdownEvent_Constructor_ValidatesCount()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CountdownEvent(-1));
    }

    [Test]
    public void CountdownEvent_InitialCount_IsZero_IsSetIsTrue()
    {
        using (var ce = new CountdownEvent(0))
        {
            Assert.IsTrue(ce.IsSet);
            Assert.AreEqual(0, ce.CurrentCount);
        }
    }

    [Test]
    public void CountdownEvent_InitialCount_IsPositive_IsSetIsFalse()
    {
        using (var ce = new CountdownEvent(3))
        {
            Assert.IsFalse(ce.IsSet);
            Assert.AreEqual(3, ce.CurrentCount);
        }
    }

    [Test]
    public void CountdownEvent_Signal_DecrementsCount()
    {
        using (var ce = new CountdownEvent(3))
        {
            ce.Signal();
            Assert.AreEqual(2, ce.CurrentCount);
            ce.Signal();
            Assert.AreEqual(1, ce.CurrentCount);
        }
    }

    [Test]
    public void CountdownEvent_Signal_SignalsWhenZero()
    {
        using (var ce = new CountdownEvent(2))
        {
            ce.Signal();
            Assert.IsFalse(ce.IsSet);
            ce.Signal();
            Assert.IsTrue(ce.IsSet);
        }
    }

    [Test]
    public void CountdownEvent_SignalWithCount_DecrementsCorrectly()
    {
        using (var ce = new CountdownEvent(5))
        {
            ce.Signal(2);
            Assert.AreEqual(3, ce.CurrentCount);
            ce.Signal(3);
            Assert.IsTrue(ce.IsSet);
        }
    }

    [Test]
    public void CountdownEvent_AddCount_IncrementsCount()
    {
        using (var ce = new CountdownEvent(2))
        {
            ce.AddCount();
            Assert.AreEqual(3, ce.CurrentCount);
            ce.AddCount(2);
            Assert.AreEqual(5, ce.CurrentCount);
        }
    }

    [Test]
    public void CountdownEvent_AddCount_ThrowsWhenSignaled()
    {
        using (var ce = new CountdownEvent(0))
        {
            Assert.Throws<InvalidOperationException>(() => ce.AddCount());
        }
    }

    [Test]
    public void CountdownEvent_Wait_ReturnsImmediatelyIfSignaled()
    {
        using (var ce = new CountdownEvent(0))
        {
            Assert.IsTrue(ce.Wait(100));
        }
    }

    [Test]
    public void CountdownEvent_Wait_TimesOutIfNotSignaled()
    {
        using (var ce = new CountdownEvent(1))
        {
            Assert.IsFalse(ce.Wait(50));
        }
    }

    [Test]
    public void SpinWait_Count_StartsAtZero()
    {
        SpinWait spinner = default;
        Assert.AreEqual(0, spinner.Count);
    }

    [Test]
    public void SpinWait_SpinOnce_IncrementsCount()
    {
        SpinWait spinner = default;
        spinner.SpinOnce();
        Assert.AreEqual(1, spinner.Count);
        spinner.SpinOnce();
        Assert.AreEqual(2, spinner.Count);
    }

    [Test]
    public void SpinWait_Reset_ResetsCount()
    {
        SpinWait spinner = default;
        spinner.SpinOnce();
        spinner.SpinOnce();
        Assert.AreEqual(2, spinner.Count);

        spinner.Reset();
        Assert.AreEqual(0, spinner.Count);
    }

    [Test]
    public void SpinWait_NextSpinWillYield_FalseInitially()
    {
        SpinWait spinner = default;
        Assert.IsFalse(spinner.NextSpinWillYield);
    }

    [Test]
    public void SpinWait_NextSpinWillYield_TrueAfterThreshold()
    {
        SpinWait spinner = default;
        // Native SpinWait uses Count > 10, polyfill uses Count >= 10
        // Use 11 spins to satisfy both implementations
        for (int i = 0; i < 11; i++)
        {
            spinner.SpinOnce();
        }
        Assert.IsTrue(spinner.NextSpinWillYield);
    }

    [Test]
    public void SpinWait_SpinUntil_ReturnsTrue_WhenConditionMet()
    {
        int counter = 0;
        bool result = SpinWait.SpinUntil(() => ++counter > 5, 1000);
        Assert.IsTrue(result);
        Assert.IsTrue(counter > 5);
    }

    [Test]
    public void SpinWait_SpinUntil_ReturnsFalse_WhenTimedOut()
    {
        bool result = SpinWait.SpinUntil(() => false, 50);
        Assert.IsFalse(result);
    }

    [Test]
    public void ManualResetEventSlim_MultipleDismissDisposeCalls()
    {
        var mre = new ManualResetEventSlim(false);
        mre.Dispose();
        mre.Dispose(); // Calling dispose multiple times should not throw
    }

    [Test]
    public void SemaphoreSlim_MultipleDismissDisposeCalls()
    {
        var sem = new SemaphoreSlim(1, 1);
        sem.Dispose();
        sem.Dispose(); // Calling dispose multiple times should not throw
    }

    [Test]
    public void CountdownEvent_MultipleDismissDisposeCalls()
    {
        var ce = new CountdownEvent(1);
        ce.Dispose();
        ce.Dispose(); // Calling dispose multiple times should not throw
    }
}
