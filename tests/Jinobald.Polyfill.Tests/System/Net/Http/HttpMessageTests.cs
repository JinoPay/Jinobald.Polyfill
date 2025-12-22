// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.Net;
using global::System.Net.Http;
using NUnit.Framework;

/// <summary>
/// HttpRequestMessage 및 HttpResponseMessage 클래스에 대한 단위 테스트입니다.
/// </summary>
public class HttpMessageTests
{
    #region HttpRequestMessage Tests

    [Test]
    public void HttpRequestMessage_DefaultConstructor_SetsDefaultValues()
    {
        var request = new HttpRequestMessage();

        Assert.AreEqual(HttpMethod.Get, request.Method);
        Assert.IsNull(request.RequestUri);
        Assert.IsNotNull(request.Headers);
    }

    [Test]
    public void HttpRequestMessage_Constructor_WithMethodAndUri_SetsValues()
    {
        var uri = new Uri("https://example.com/api");
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        Assert.AreEqual(HttpMethod.Post, request.Method);
        Assert.AreEqual(uri, request.RequestUri);
    }

    [Test]
    public void HttpRequestMessage_Constructor_WithMethodAndStringUri_SetsValues()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "https://example.com/api");

        Assert.AreEqual(HttpMethod.Put, request.Method);
        Assert.IsNotNull(request.RequestUri);
        Assert.AreEqual("https://example.com/api", request.RequestUri!.ToString());
    }

    [Test]
    public void HttpRequestMessage_Method_CanBeSet()
    {
        var request = new HttpRequestMessage();

        request.Method = HttpMethod.Delete;

        Assert.AreEqual(HttpMethod.Delete, request.Method);
    }

    [Test]
    public void HttpRequestMessage_RequestUri_CanBeSet()
    {
        var request = new HttpRequestMessage();
        var uri = new Uri("https://example.com");

        request.RequestUri = uri;

        Assert.AreEqual(uri, request.RequestUri);
    }

    [Test]
    public void HttpRequestMessage_Content_CanBeSet()
    {
        var request = new HttpRequestMessage();
        var content = new StringContent("Hello");

        request.Content = content;

        Assert.AreEqual(content, request.Content);
    }

    [Test]
    public void HttpRequestMessage_Version_DefaultIsHttp11()
    {
        var request = new HttpRequestMessage();

        Assert.AreEqual(new Version(1, 1), request.Version);
    }

    [Test]
    public void HttpRequestMessage_Version_CanBeSet()
    {
        var request = new HttpRequestMessage();

        request.Version = new Version(2, 0);

        Assert.AreEqual(new Version(2, 0), request.Version);
    }

    [Test]
    public void HttpRequestMessage_Dispose_DisposesContent()
    {
        var content = new StringContent("Hello");
        var request = new HttpRequestMessage { Content = content };

        request.Dispose();

        Assert.Throws<ObjectDisposedException>(() => { var _ = content.ReadAsStringAsync().Result; });
    }

    #endregion

    #region HttpResponseMessage Tests

    [Test]
    public void HttpResponseMessage_DefaultConstructor_SetsOkStatus()
    {
        var response = new HttpResponseMessage();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(response.Headers);
    }

    [Test]
    public void HttpResponseMessage_Constructor_WithStatusCode_SetsStatus()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public void HttpResponseMessage_StatusCode_CanBeSet()
    {
        var response = new HttpResponseMessage();

        response.StatusCode = HttpStatusCode.Created;

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    [Test]
    public void HttpResponseMessage_ReasonPhrase_CanBeSet()
    {
        var response = new HttpResponseMessage();

        response.ReasonPhrase = "Custom Reason";

        Assert.AreEqual("Custom Reason", response.ReasonPhrase);
    }

    [Test]
    public void HttpResponseMessage_Content_CanBeSet()
    {
        var response = new HttpResponseMessage();
        var content = new StringContent("Response Body");

        response.Content = content;

        Assert.AreEqual(content, response.Content);
    }

    [Test]
    public void HttpResponseMessage_RequestMessage_CanBeSet()
    {
        var response = new HttpResponseMessage();
        var request = new HttpRequestMessage();

        response.RequestMessage = request;

        Assert.AreEqual(request, response.RequestMessage);
    }

    [Test]
    public void HttpResponseMessage_IsSuccessStatusCode_TrueFor2xx()
    {
        Assert.IsTrue(new HttpResponseMessage(HttpStatusCode.OK).IsSuccessStatusCode);
        Assert.IsTrue(new HttpResponseMessage(HttpStatusCode.Created).IsSuccessStatusCode);
        Assert.IsTrue(new HttpResponseMessage(HttpStatusCode.NoContent).IsSuccessStatusCode);
    }

    [Test]
    public void HttpResponseMessage_IsSuccessStatusCode_FalseForNon2xx()
    {
        Assert.IsFalse(new HttpResponseMessage(HttpStatusCode.BadRequest).IsSuccessStatusCode);
        Assert.IsFalse(new HttpResponseMessage(HttpStatusCode.NotFound).IsSuccessStatusCode);
        Assert.IsFalse(new HttpResponseMessage(HttpStatusCode.InternalServerError).IsSuccessStatusCode);
    }

    [Test]
    public void HttpResponseMessage_EnsureSuccessStatusCode_ThrowsOnError()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
    }

    [Test]
    public void HttpResponseMessage_EnsureSuccessStatusCode_ReturnsResponseOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var result = response.EnsureSuccessStatusCode();

        Assert.AreSame(response, result);
    }

    [Test]
    public void HttpResponseMessage_Dispose_DisposesContent()
    {
        var content = new StringContent("Response");
        var response = new HttpResponseMessage { Content = content };

        response.Dispose();

        Assert.Throws<ObjectDisposedException>(() => { var _ = content.ReadAsStringAsync().Result; });
    }

    #endregion
}
#endif
