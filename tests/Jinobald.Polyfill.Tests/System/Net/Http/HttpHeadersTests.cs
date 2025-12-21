// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.Linq;
using global::System.Net.Http.Headers;
using Xunit;

/// <summary>
/// HttpHeaders 클래스들에 대한 단위 테스트입니다.
/// </summary>
public class HttpHeadersTests
{
    #region HttpRequestHeaders Tests

    [Fact]
    public void HttpRequestHeaders_Add_AddsHeader()
    {
        var headers = new HttpRequestHeaders();

        headers.Add("X-Custom-Header", "value");

        Assert.True(headers.Contains("X-Custom-Header"));
    }

    [Fact]
    public void HttpRequestHeaders_GetValues_ReturnsValues()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("X-Custom-Header", "value1");
        headers.Add("X-Custom-Header", "value2");

        var values = headers.GetValues("X-Custom-Header").ToList();

        Assert.Contains("value1", values);
        Assert.Contains("value2", values);
    }

    [Fact]
    public void HttpRequestHeaders_Remove_RemovesHeader()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("X-Custom-Header", "value");

        var result = headers.Remove("X-Custom-Header");

        Assert.True(result);
        Assert.False(headers.Contains("X-Custom-Header"));
    }

    [Fact]
    public void HttpRequestHeaders_Accept_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.Accept = "application/json";

        Assert.Equal("application/json", headers.Accept);
    }

    [Fact]
    public void HttpRequestHeaders_Authorization_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.Authorization = "Bearer token123";

        Assert.Equal("Bearer token123", headers.Authorization);
    }

    [Fact]
    public void HttpRequestHeaders_UserAgent_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.UserAgent = "MyApp/1.0";

        Assert.Equal("MyApp/1.0", headers.UserAgent);
    }

    [Fact]
    public void HttpRequestHeaders_Host_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.Host = "example.com";

        Assert.Equal("example.com", headers.Host);
    }

    #endregion

    #region HttpResponseHeaders Tests

    [Fact]
    public void HttpResponseHeaders_Add_AddsHeader()
    {
        var headers = new HttpResponseHeaders();

        headers.Add("X-Response-Header", "value");

        Assert.True(headers.Contains("X-Response-Header"));
    }

    [Fact]
    public void HttpResponseHeaders_Server_CanBeSet()
    {
        var headers = new HttpResponseHeaders();

        headers.Server = "MyServer/1.0";

        Assert.Equal("MyServer/1.0", headers.Server);
    }

    [Fact]
    public void HttpResponseHeaders_Location_CanBeSet()
    {
        var headers = new HttpResponseHeaders();
        var uri = new Uri("https://example.com/redirect");

        headers.Location = uri;

        Assert.Equal(uri, headers.Location);
    }

    #endregion

    #region HttpContentHeaders Tests

    [Fact]
    public void HttpContentHeaders_ContentType_CanBeSet()
    {
        var headers = new HttpContentHeaders();

        headers.ContentType = "application/json";

        Assert.Equal("application/json", headers.ContentType);
    }

    [Fact]
    public void HttpContentHeaders_ContentLength_CanBeSet()
    {
        var headers = new HttpContentHeaders();

        headers.ContentLength = 1024;

        Assert.Equal(1024, headers.ContentLength);
    }

    [Fact]
    public void HttpContentHeaders_ContentEncoding_CanBeSet()
    {
        var headers = new HttpContentHeaders();

        headers.ContentEncoding = "gzip";

        Assert.Equal("gzip", headers.ContentEncoding);
    }

    #endregion

    #region Enumeration Tests

    [Fact]
    public void HttpHeaders_Enumerable_ReturnsAllHeaders()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("Header1", "Value1");
        headers.Add("Header2", "Value2");

        var list = headers.ToList();

        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void HttpHeaders_Clear_RemovesAllHeaders()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("Header1", "Value1");
        headers.Add("Header2", "Value2");

        headers.Clear();

        Assert.Empty(headers);
    }

    #endregion

    #region TryGetValues Tests

    [Fact]
    public void HttpHeaders_TryGetValues_ExistingHeader_ReturnsTrue()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("X-Custom-Header", "value");

        var result = headers.TryGetValues("X-Custom-Header", out var values);

        Assert.True(result);
        Assert.NotNull(values);
        Assert.Contains("value", values);
    }

    [Fact]
    public void HttpHeaders_TryGetValues_NonExistingHeader_ReturnsFalse()
    {
        var headers = new HttpRequestHeaders();

        var result = headers.TryGetValues("Non-Existing", out var values);

        Assert.False(result);
        Assert.Null(values);
    }

    #endregion
}
#endif
