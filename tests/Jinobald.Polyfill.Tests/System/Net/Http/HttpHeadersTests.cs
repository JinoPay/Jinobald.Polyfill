// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.Linq;
using global::System.Net.Http.Headers;
using NUnit.Framework;

/// <summary>
/// HttpHeaders 클래스들에 대한 단위 테스트입니다.
/// </summary>
public class HttpHeadersTests
{
    #region HttpRequestHeaders Tests

    [Test]
    public void HttpRequestHeaders_Add_AddsHeader()
    {
        var headers = new HttpRequestHeaders();

        headers.Add("X-Custom-Header", "value");

        Assert.IsTrue(headers.Contains("X-Custom-Header"));
    }

    [Test]
    public void HttpRequestHeaders_GetValues_ReturnsValues()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("X-Custom-Header", "value1");
        headers.Add("X-Custom-Header", "value2");

        var values = headers.GetValues("X-Custom-Header").ToList();

        Assert.Contains("value1", values);
        Assert.Contains("value2", values);
    }

    [Test]
    public void HttpRequestHeaders_Remove_RemovesHeader()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("X-Custom-Header", "value");

        var result = headers.Remove("X-Custom-Header");

        Assert.IsTrue(result);
        Assert.IsFalse(headers.Contains("X-Custom-Header"));
    }

    [Test]
    public void HttpRequestHeaders_Accept_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.Accept = "application/json";

        Assert.AreEqual("application/json", headers.Accept);
    }

    [Test]
    public void HttpRequestHeaders_Authorization_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.Authorization = "Bearer token123";

        Assert.AreEqual("Bearer token123", headers.Authorization);
    }

    [Test]
    public void HttpRequestHeaders_UserAgent_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.UserAgent = "MyApp/1.0";

        Assert.AreEqual("MyApp/1.0", headers.UserAgent);
    }

    [Test]
    public void HttpRequestHeaders_Host_CanBeSet()
    {
        var headers = new HttpRequestHeaders();

        headers.Host = "example.com";

        Assert.AreEqual("example.com", headers.Host);
    }

    #endregion

    #region HttpResponseHeaders Tests

    [Test]
    public void HttpResponseHeaders_Add_AddsHeader()
    {
        var headers = new HttpResponseHeaders();

        headers.Add("X-Response-Header", "value");

        Assert.IsTrue(headers.Contains("X-Response-Header"));
    }

    [Test]
    public void HttpResponseHeaders_Server_CanBeSet()
    {
        var headers = new HttpResponseHeaders();

        headers.Server = "MyServer/1.0";

        Assert.AreEqual("MyServer/1.0", headers.Server);
    }

    [Test]
    public void HttpResponseHeaders_Location_CanBeSet()
    {
        var headers = new HttpResponseHeaders();
        var uri = new Uri("https://example.com/redirect");

        headers.Location = uri;

        Assert.AreEqual(uri, headers.Location);
    }

    #endregion

    #region HttpContentHeaders Tests

    [Test]
    public void HttpContentHeaders_ContentType_CanBeSet()
    {
        var headers = new HttpContentHeaders();

        headers.ContentType = "application/json";

        Assert.AreEqual("application/json", headers.ContentType);
    }

    [Test]
    public void HttpContentHeaders_ContentLength_CanBeSet()
    {
        var headers = new HttpContentHeaders();

        headers.ContentLength = 1024;

        Assert.AreEqual(1024, headers.ContentLength);
    }

    [Test]
    public void HttpContentHeaders_ContentEncoding_CanBeSet()
    {
        var headers = new HttpContentHeaders();

        headers.ContentEncoding = "gzip";

        Assert.AreEqual("gzip", headers.ContentEncoding);
    }

    #endregion

    #region Enumeration Tests

    [Test]
    public void HttpHeaders_Enumerable_ReturnsAllHeaders()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("Header1", "Value1");
        headers.Add("Header2", "Value2");

        var list = headers.ToList();

        Assert.AreEqual(2, list.Count);
    }

    [Test]
    public void HttpHeaders_Clear_RemovesAllHeaders()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("Header1", "Value1");
        headers.Add("Header2", "Value2");

        headers.Clear();

        Assert.IsEmpty(headers);
    }

    #endregion

    #region TryGetValues Tests

    [Test]
    public void HttpHeaders_TryGetValues_ExistingHeader_ReturnsTrue()
    {
        var headers = new HttpRequestHeaders();
        headers.Add("X-Custom-Header", "value");

        var result = headers.TryGetValues("X-Custom-Header", out var values);

        Assert.IsTrue(result);
        Assert.IsNotNull(values);
        Assert.Contains("value", values);
    }

    [Test]
    public void HttpHeaders_TryGetValues_NonExistingHeader_ReturnsFalse()
    {
        var headers = new HttpRequestHeaders();

        var result = headers.TryGetValues("Non-Existing", out var values);

        Assert.IsFalse(result);
        Assert.IsNull(values);
    }

    #endregion
}
#endif
