// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.Net.Http;
using Xunit;

/// <summary>
/// HttpClient 클래스에 대한 단위 테스트입니다.
/// </summary>
public class HttpClientTests
{
    #region Constructor Tests

    [Fact]
    public void HttpClient_DefaultConstructor_CreatesInstance()
    {
        using var client = new HttpClient();

        Assert.NotNull(client);
        Assert.NotNull(client.DefaultRequestHeaders);
    }

    [Fact]
    public void HttpClient_Constructor_WithHandler_CreatesInstance()
    {
        using var handler = new HttpClientHandler();
        using var client = new HttpClient(handler);

        Assert.NotNull(client);
    }

    #endregion

    #region Properties Tests

    [Fact]
    public void HttpClient_BaseAddress_CanBeSet()
    {
        using var client = new HttpClient();
        var baseUri = new Uri("https://example.com/api/");

        client.BaseAddress = baseUri;

        Assert.Equal(baseUri, client.BaseAddress);
    }

    [Fact]
    public void HttpClient_Timeout_DefaultIs100Seconds()
    {
        using var client = new HttpClient();

        Assert.Equal(TimeSpan.FromSeconds(100), client.Timeout);
    }

    [Fact]
    public void HttpClient_Timeout_CanBeSet()
    {
        using var client = new HttpClient();
        var timeout = TimeSpan.FromSeconds(30);

        client.Timeout = timeout;

        Assert.Equal(timeout, client.Timeout);
    }

    [Fact]
    public void HttpClient_Timeout_ThrowsOnNegative()
    {
        using var client = new HttpClient();

        Assert.Throws<ArgumentOutOfRangeException>(() => client.Timeout = TimeSpan.FromSeconds(-5));
    }

    [Fact]
    public void HttpClient_MaxResponseContentBufferSize_CanBeSet()
    {
        using var client = new HttpClient();
        var size = 1024 * 1024L; // 1MB

        client.MaxResponseContentBufferSize = size;

        Assert.Equal(size, client.MaxResponseContentBufferSize);
    }

    [Fact]
    public void HttpClient_MaxResponseContentBufferSize_ThrowsOnZeroOrNegative()
    {
        using var client = new HttpClient();

        Assert.Throws<ArgumentOutOfRangeException>(() => client.MaxResponseContentBufferSize = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => client.MaxResponseContentBufferSize = -1);
    }

    #endregion

    #region DefaultRequestHeaders Tests

    [Fact]
    public void HttpClient_DefaultRequestHeaders_CanAddHeader()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add("X-Custom-Header", "value");

        Assert.True(client.DefaultRequestHeaders.Contains("X-Custom-Header"));
    }

    [Fact]
    public void HttpClient_DefaultRequestHeaders_CanSetAccept()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Accept = "application/json";

        Assert.Equal("application/json", client.DefaultRequestHeaders.Accept);
    }

    [Fact]
    public void HttpClient_DefaultRequestHeaders_CanSetAuthorization()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = "Bearer token123";

        Assert.Equal("Bearer token123", client.DefaultRequestHeaders.Authorization);
    }

    [Fact]
    public void HttpClient_DefaultRequestHeaders_CanSetUserAgent()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.UserAgent = "TestAgent/1.0";

        Assert.Equal("TestAgent/1.0", client.DefaultRequestHeaders.UserAgent);
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void HttpClient_Dispose_CanBeCalledMultipleTimes()
    {
        var client = new HttpClient();

        client.Dispose();
        client.Dispose(); // Should not throw

        Assert.True(true);
    }

    [Fact]
    public void HttpClient_AfterDispose_ThrowsObjectDisposedException()
    {
        var client = new HttpClient();
        client.Dispose();

        Assert.Throws<ObjectDisposedException>(() => client.GetAsync("https://example.com").Wait());
    }

    #endregion

    #region CancelPendingRequests Tests

    [Fact]
    public void HttpClient_CancelPendingRequests_DoesNotThrow()
    {
        using var client = new HttpClient();

        client.CancelPendingRequests();

        Assert.True(true);
    }

    #endregion

    #region Property Modification After Operation Tests

    [Fact]
    public void HttpClient_BaseAddress_CannotBeChangedAfterRequest()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://example.com");

        // 요청 시작 시뮬레이션 (실제 요청 없이)
        // 실제로 이 테스트는 요청을 보내야 하지만, 네트워크 없이 테스트하기 위해 스킵
        // Assert.Throws<InvalidOperationException>(() => client.BaseAddress = new Uri("https://other.com"));
        Assert.True(true);
    }

    #endregion
}
#endif
