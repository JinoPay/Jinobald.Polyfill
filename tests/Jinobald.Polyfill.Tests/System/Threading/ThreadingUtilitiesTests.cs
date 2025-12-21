using Xunit;
using System.Threading;

namespace Jinobald.Polyfill.Tests.System.Threading;

public class ThreadingUtilitiesTests
{
    [Fact]
    public void ManualResetEventSlim_InitialState_False()
    {
        using (var mre = new ManualResetEventSlim(false))
        {
            Assert.False(mre.IsSet);
        }
    }

    [Fact]
    public void ManualResetEventSlim_InitialState_True()
    {
        using (var mre = new ManualResetEventSlim(true))
        {
            Assert.True(mre.IsSet);
        }
    }

    [Fact]
    public void ManualResetEventSlim_Set_SignalsEvent()
    {
        using (var mre = new ManualResetEventSlim(false))
        {
            mre.Set();
            Assert.True(mre.IsSet);
        }
    }

    [Fact]
    public void ManualResetEventSlim_Reset_ResetsEvent()
    {
        using (var mre = new ManualResetEventSlim(true))
        {
            mre.Reset();
            Assert.False(mre.IsSet);
        }
    }

    [Fact]
    public void ManualResetEventSlim_Wait_ReturnsImmediatelyIfSignaled()
    {
        using (var mre = new ManualResetEventSlim(true))
        {
            Assert.True(mre.Wait(100));
        }
    }

    [Fact]
    public void ManualResetEventSlim_Wait_TimesOutIfNotSignaled()
    {
        using (var mre = new ManualResetEventSlim(false))
        {
            Assert.False(mre.Wait(50));
        }
    }

    [Fact]
    public void SemaphoreSlim_Constructor_ValidatesParameters()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemaphoreSlim(-1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemaphoreSlim(0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemaphoreSlim(2, 1));
    }

    [Fact]
    public void SemaphoreSlim_InitialCount_IsCorrect()
    {
        using (var sem = new SemaphoreSlim(3, 5))
        {
            Assert.Equal(3, sem.CurrentCount);
        }
    }

    [Fact]
    public void SemaphoreSlim_Wait_DecrementsCount()
    {
        using (var sem = new SemaphoreSlim(2, 5))
        {
            sem.Wait();
            Assert.Equal(1, sem.CurrentCount);
            sem.Wait();
            Assert.Equal(0, sem.CurrentCount);
        }
    }

    [Fact]
    public void SemaphoreSlim_Release_IncrementsCount()
    {
        using (var sem = new SemaphoreSlim(1, 5))
        {
            sem.Wait();
            Assert.Equal(0, sem.CurrentCount);

            int prevCount = sem.Release();
            Assert.Equal(0, prevCount);
            Assert.Equal(1, sem.CurrentCount);
        }
    }

    [Fact]
    public void SemaphoreSlim_Release_ThrowsWhenFull()
    {
        using (var sem = new SemaphoreSlim(3, 3))
        {
            Assert.Throws<SemaphoreFullException>(() => sem.Release());
        }
    }

    [Fact]
    public void SemaphoreSlim_WaitWithTimeout_ReturnsTrue()
    {
        using (var sem = new SemaphoreSlim(1, 1))
        {
            Assert.True(sem.Wait(100));
        }
    }

    [Fact]
    public void SemaphoreSlim_WaitWithTimeout_ReturnsFalse()
    {
        using (var sem = new SemaphoreSlim(0, 1))
        {
            Assert.False(sem.Wait(50));
        }
    }

    [Fact]
    public void CountdownEvent_Constructor_ValidatesCount()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CountdownEvent(-1));
    }

    [Fact]
    public void CountdownEvent_InitialCount_IsZero_IsSetIsTrue()
    {
        using (var ce = new CountdownEvent(0))
        {
            Assert.True(ce.IsSet);
            Assert.Equal(0, ce.CurrentCount);
        }
    }

    [Fact]
    public void CountdownEvent_InitialCount_IsPositive_IsSetIsFalse()
    {
        using (var ce = new CountdownEvent(3))
        {
            Assert.False(ce.IsSet);
            Assert.Equal(3, ce.CurrentCount);
        }
    }

    [Fact]
    public void CountdownEvent_Signal_DecrementsCount()
    {
        using (var ce = new CountdownEvent(3))
        {
            ce.Signal();
            Assert.Equal(2, ce.CurrentCount);
            ce.Signal();
            Assert.Equal(1, ce.CurrentCount);
        }
    }

    [Fact]
    public void CountdownEvent_Signal_SignalsWhenZero()
    {
        using (var ce = new CountdownEvent(2))
        {
            ce.Signal();
            Assert.False(ce.IsSet);
            ce.Signal();
            Assert.True(ce.IsSet);
        }
    }

    [Fact]
    public void CountdownEvent_SignalWithCount_DecrementsCorrectly()
    {
        using (var ce = new CountdownEvent(5))
        {
            ce.Signal(2);
            Assert.Equal(3, ce.CurrentCount);
            ce.Signal(3);
            Assert.True(ce.IsSet);
        }
    }

    [Fact]
    public void CountdownEvent_AddCount_IncrementsCount()
    {
        using (var ce = new CountdownEvent(2))
        {
            ce.AddCount();
            Assert.Equal(3, ce.CurrentCount);
            ce.AddCount(2);
            Assert.Equal(5, ce.CurrentCount);
        }
    }

    [Fact]
    public void CountdownEvent_AddCount_ThrowsWhenSignaled()
    {
        using (var ce = new CountdownEvent(0))
        {
            Assert.Throws<InvalidOperationException>(() => ce.AddCount());
        }
    }

    [Fact]
    public void CountdownEvent_Wait_ReturnsImmediatelyIfSignaled()
    {
        using (var ce = new CountdownEvent(0))
        {
            Assert.True(ce.Wait(100));
        }
    }

    [Fact]
    public void CountdownEvent_Wait_TimesOutIfNotSignaled()
    {
        using (var ce = new CountdownEvent(1))
        {
            Assert.False(ce.Wait(50));
        }
    }

    [Fact]
    public void SpinWait_Count_StartsAtZero()
    {
        var spinner = new SpinWait();
        Assert.Equal(0, spinner.Count);
    }

    [Fact]
    public void SpinWait_SpinOnce_IncrementsCount()
    {
        var spinner = new SpinWait();
        spinner.SpinOnce();
        Assert.Equal(1, spinner.Count);
        spinner.SpinOnce();
        Assert.Equal(2, spinner.Count);
    }

    [Fact]
    public void SpinWait_Reset_ResetsCount()
    {
        var spinner = new SpinWait();
        spinner.SpinOnce();
        spinner.SpinOnce();
        Assert.Equal(2, spinner.Count);

        spinner.Reset();
        Assert.Equal(0, spinner.Count);
    }

    [Fact]
    public void SpinWait_NextSpinWillYield_FalseInitially()
    {
        var spinner = new SpinWait();
        Assert.False(spinner.NextSpinWillYield);
    }

    [Fact]
    public void SpinWait_NextSpinWillYield_TrueAfterThreshold()
    {
        var spinner = new SpinWait();
        for (int i = 0; i < 10; i++)
        {
            spinner.SpinOnce();
        }
        Assert.True(spinner.NextSpinWillYield);
    }

    [Fact]
    public void SpinWait_SpinUntil_ReturnsTrue_WhenConditionMet()
    {
        int counter = 0;
        bool result = SpinWait.SpinUntil(() => ++counter > 5, 1000);
        Assert.True(result);
        Assert.True(counter > 5);
    }

    [Fact]
    public void SpinWait_SpinUntil_ReturnsFalse_WhenTimedOut()
    {
        bool result = SpinWait.SpinUntil(() => false, 50);
        Assert.False(result);
    }

    [Fact]
    public void ManualResetEventSlim_MultipleDismissDisposeCalls()
    {
        var mre = new ManualResetEventSlim(false);
        mre.Dispose();
        mre.Dispose(); // Calling dispose multiple times should not throw
    }

    [Fact]
    public void SemaphoreSlim_MultipleDismissDisposeCalls()
    {
        var sem = new SemaphoreSlim(1, 1);
        sem.Dispose();
        sem.Dispose(); // Calling dispose multiple times should not throw
    }

    [Fact]
    public void CountdownEvent_MultipleDismissDisposeCalls()
    {
        var ce = new CountdownEvent(1);
        ce.Dispose();
        ce.Dispose(); // Calling dispose multiple times should not throw
    }
}
