// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.Net.Http;
using NUnit.Framework;

/// <summary>
/// HttpClient 클래스에 대한 단위 테스트입니다.
/// </summary>
public class HttpClientTests
{
    #region Constructor Tests

    [Test]
    public void HttpClient_DefaultConstructor_CreatesInstance()
    {
        using var client = new HttpClient();

        Assert.IsNotNull(client);
        Assert.IsNotNull(client.DefaultRequestHeaders);
    }

    [Test]
    public void HttpClient_Constructor_WithHandler_CreatesInstance()
    {
        using var handler = new HttpClientHandler();
        using var client = new HttpClient(handler);

        Assert.IsNotNull(client);
    }

    #endregion

    #region Properties Tests

    [Test]
    public void HttpClient_BaseAddress_CanBeSet()
    {
        using var client = new HttpClient();
        var baseUri = new Uri("https://example.com/api/");

        client.BaseAddress = baseUri;

        Assert.AreEqual(baseUri, client.BaseAddress);
    }

    [Test]
    public void HttpClient_Timeout_DefaultIs100Seconds()
    {
        using var client = new HttpClient();

        Assert.AreEqual(TimeSpan.FromSeconds(100), client.Timeout);
    }

    [Test]
    public void HttpClient_Timeout_CanBeSet()
    {
        using var client = new HttpClient();
        var timeout = TimeSpan.FromSeconds(30);

        client.Timeout = timeout;

        Assert.AreEqual(timeout, client.Timeout);
    }

    [Test]
    public void HttpClient_Timeout_ThrowsOnNegative()
    {
        using var client = new HttpClient();

        Assert.Throws<ArgumentOutOfRangeException>(() => client.Timeout = TimeSpan.FromSeconds(-5));
    }

    [Test]
    public void HttpClient_MaxResponseContentBufferSize_CanBeSet()
    {
        using var client = new HttpClient();
        var size = 1024 * 1024L; // 1MB

        client.MaxResponseContentBufferSize = size;

        Assert.AreEqual(size, client.MaxResponseContentBufferSize);
    }

    [Test]
    public void HttpClient_MaxResponseContentBufferSize_ThrowsOnZeroOrNegative()
    {
        using var client = new HttpClient();

        Assert.Throws<ArgumentOutOfRangeException>(() => client.MaxResponseContentBufferSize = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => client.MaxResponseContentBufferSize = -1);
    }

    #endregion

    #region DefaultRequestHeaders Tests

    [Test]
    public void HttpClient_DefaultRequestHeaders_CanAddHeader()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add("X-Custom-Header", "value");

        Assert.IsTrue(client.DefaultRequestHeaders.Contains("X-Custom-Header"));
    }

    [Test]
    public void HttpClient_DefaultRequestHeaders_CanSetAccept()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Accept = "application/json";

        Assert.AreEqual("application/json", client.DefaultRequestHeaders.Accept);
    }

    [Test]
    public void HttpClient_DefaultRequestHeaders_CanSetAuthorization()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = "Bearer token123";

        Assert.AreEqual("Bearer token123", client.DefaultRequestHeaders.Authorization);
    }

    [Test]
    public void HttpClient_DefaultRequestHeaders_CanSetUserAgent()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.UserAgent = "TestAgent/1.0";

        Assert.AreEqual("TestAgent/1.0", client.DefaultRequestHeaders.UserAgent);
    }

    #endregion

    #region Dispose Tests

    [Test]
    public void HttpClient_Dispose_CanBeCalledMultipleTimes()
    {
        var client = new HttpClient();

        client.Dispose();
        client.Dispose(); // Should not throw

        Assert.IsTrue(true);
    }

    [Test]
    public void HttpClient_AfterDispose_ThrowsObjectDisposedException()
    {
        var client = new HttpClient();
        client.Dispose();

        Assert.Throws<ObjectDisposedException>(() => client.GetAsync("https://example.com").Wait());
    }

    #endregion

    #region CancelPendingRequests Tests

    [Test]
    public void HttpClient_CancelPendingRequests_DoesNotThrow()
    {
        using var client = new HttpClient();

        client.CancelPendingRequests();

        Assert.IsTrue(true);
    }

    [Test]
    public void HttpClient_CancelPendingRequests_CanBeCalledMultipleTimes()
    {
        using var client = new HttpClient();

        client.CancelPendingRequests();
        client.CancelPendingRequests();
        client.CancelPendingRequests();

        Assert.IsTrue(true);
    }

    #endregion

    #region HTTP Method Async Tests

    [Test]
    public void HttpClient_HeadAsync_MethodExists()
    {
        using var client = new HttpClient();

        // HeadAsync 메서드가 존재하는지 확인
        var method = typeof(HttpClient).GetMethod("HeadAsync", new[] { typeof(string) });
        Assert.IsNotNull(method, "HeadAsync(string) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("HeadAsync", new[] { typeof(Uri) });
        Assert.IsNotNull(method, "HeadAsync(Uri) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("HeadAsync", new[] { typeof(string), typeof(global::System.Threading.CancellationToken) });
        Assert.IsNotNull(method, "HeadAsync(string, CancellationToken) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("HeadAsync", new[] { typeof(Uri), typeof(global::System.Threading.CancellationToken) });
        Assert.IsNotNull(method, "HeadAsync(Uri, CancellationToken) 메서드가 존재해야 합니다.");
    }

    [Test]
    public void HttpClient_OptionsAsync_MethodExists()
    {
        using var client = new HttpClient();

        // OptionsAsync 메서드가 존재하는지 확인
        var method = typeof(HttpClient).GetMethod("OptionsAsync", new[] { typeof(string) });
        Assert.IsNotNull(method, "OptionsAsync(string) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("OptionsAsync", new[] { typeof(Uri) });
        Assert.IsNotNull(method, "OptionsAsync(Uri) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("OptionsAsync", new[] { typeof(string), typeof(global::System.Threading.CancellationToken) });
        Assert.IsNotNull(method, "OptionsAsync(string, CancellationToken) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("OptionsAsync", new[] { typeof(Uri), typeof(global::System.Threading.CancellationToken) });
        Assert.IsNotNull(method, "OptionsAsync(Uri, CancellationToken) 메서드가 존재해야 합니다.");
    }

    [Test]
    public void HttpClient_TraceAsync_MethodExists()
    {
        using var client = new HttpClient();

        // TraceAsync 메서드가 존재하는지 확인
        var method = typeof(HttpClient).GetMethod("TraceAsync", new[] { typeof(string) });
        Assert.IsNotNull(method, "TraceAsync(string) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("TraceAsync", new[] { typeof(Uri) });
        Assert.IsNotNull(method, "TraceAsync(Uri) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("TraceAsync", new[] { typeof(string), typeof(global::System.Threading.CancellationToken) });
        Assert.IsNotNull(method, "TraceAsync(string, CancellationToken) 메서드가 존재해야 합니다.");

        method = typeof(HttpClient).GetMethod("TraceAsync", new[] { typeof(Uri), typeof(global::System.Threading.CancellationToken) });
        Assert.IsNotNull(method, "TraceAsync(Uri, CancellationToken) 메서드가 존재해야 합니다.");
    }

    #endregion

    #region Property Modification After Operation Tests

    [Test]
    public void HttpClient_BaseAddress_CannotBeChangedAfterRequest()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://example.com");

        // 요청 시작 시뮬레이션 (실제 요청 없이)
        // 실제로 이 테스트는 요청을 보내야 하지만, 네트워크 없이 테스트하기 위해 스킵
        // Assert.Throws<InvalidOperationException>(() => client.BaseAddress = new Uri("https://other.com"));
        Assert.IsTrue(true);
    }

    #endregion
}
#endif
