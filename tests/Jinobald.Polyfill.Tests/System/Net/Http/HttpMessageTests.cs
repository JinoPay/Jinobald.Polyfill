// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.Net;
using global::System.Net.Http;
using Xunit;

/// <summary>
/// HttpRequestMessage 및 HttpResponseMessage 클래스에 대한 단위 테스트입니다.
/// </summary>
public class HttpMessageTests
{
    #region HttpRequestMessage Tests

    [Fact]
    public void HttpRequestMessage_DefaultConstructor_SetsDefaultValues()
    {
        var request = new HttpRequestMessage();

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Null(request.RequestUri);
        Assert.NotNull(request.Headers);
    }

    [Fact]
    public void HttpRequestMessage_Constructor_WithMethodAndUri_SetsValues()
    {
        var uri = new Uri("https://example.com/api");
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(uri, request.RequestUri);
    }

    [Fact]
    public void HttpRequestMessage_Constructor_WithMethodAndStringUri_SetsValues()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "https://example.com/api");

        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.NotNull(request.RequestUri);
        Assert.Equal("https://example.com/api", request.RequestUri.ToString());
    }

    [Fact]
    public void HttpRequestMessage_Method_CanBeSet()
    {
        var request = new HttpRequestMessage();

        request.Method = HttpMethod.Delete;

        Assert.Equal(HttpMethod.Delete, request.Method);
    }

    [Fact]
    public void HttpRequestMessage_RequestUri_CanBeSet()
    {
        var request = new HttpRequestMessage();
        var uri = new Uri("https://example.com");

        request.RequestUri = uri;

        Assert.Equal(uri, request.RequestUri);
    }

    [Fact]
    public void HttpRequestMessage_Content_CanBeSet()
    {
        var request = new HttpRequestMessage();
        var content = new StringContent("Hello");

        request.Content = content;

        Assert.Equal(content, request.Content);
    }

    [Fact]
    public void HttpRequestMessage_Version_DefaultIsHttp11()
    {
        var request = new HttpRequestMessage();

        Assert.Equal(new Version(1, 1), request.Version);
    }

    [Fact]
    public void HttpRequestMessage_Version_CanBeSet()
    {
        var request = new HttpRequestMessage();

        request.Version = new Version(2, 0);

        Assert.Equal(new Version(2, 0), request.Version);
    }

    [Fact]
    public void HttpRequestMessage_Dispose_DisposesContent()
    {
        var content = new StringContent("Hello");
        var request = new HttpRequestMessage { Content = content };

        request.Dispose();

        Assert.Throws<ObjectDisposedException>(() => content.ReadAsStringAsync().Result);
    }

    #endregion

    #region HttpResponseMessage Tests

    [Fact]
    public void HttpResponseMessage_DefaultConstructor_SetsOkStatus()
    {
        var response = new HttpResponseMessage();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Headers);
    }

    [Fact]
    public void HttpResponseMessage_Constructor_WithStatusCode_SetsStatus()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public void HttpResponseMessage_StatusCode_CanBeSet()
    {
        var response = new HttpResponseMessage();

        response.StatusCode = HttpStatusCode.Created;

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public void HttpResponseMessage_ReasonPhrase_CanBeSet()
    {
        var response = new HttpResponseMessage();

        response.ReasonPhrase = "Custom Reason";

        Assert.Equal("Custom Reason", response.ReasonPhrase);
    }

    [Fact]
    public void HttpResponseMessage_Content_CanBeSet()
    {
        var response = new HttpResponseMessage();
        var content = new StringContent("Response Body");

        response.Content = content;

        Assert.Equal(content, response.Content);
    }

    [Fact]
    public void HttpResponseMessage_RequestMessage_CanBeSet()
    {
        var response = new HttpResponseMessage();
        var request = new HttpRequestMessage();

        response.RequestMessage = request;

        Assert.Equal(request, response.RequestMessage);
    }

    [Fact]
    public void HttpResponseMessage_IsSuccessStatusCode_TrueFor2xx()
    {
        Assert.True(new HttpResponseMessage(HttpStatusCode.OK).IsSuccessStatusCode);
        Assert.True(new HttpResponseMessage(HttpStatusCode.Created).IsSuccessStatusCode);
        Assert.True(new HttpResponseMessage(HttpStatusCode.NoContent).IsSuccessStatusCode);
    }

    [Fact]
    public void HttpResponseMessage_IsSuccessStatusCode_FalseForNon2xx()
    {
        Assert.False(new HttpResponseMessage(HttpStatusCode.BadRequest).IsSuccessStatusCode);
        Assert.False(new HttpResponseMessage(HttpStatusCode.NotFound).IsSuccessStatusCode);
        Assert.False(new HttpResponseMessage(HttpStatusCode.InternalServerError).IsSuccessStatusCode);
    }

    [Fact]
    public void HttpResponseMessage_EnsureSuccessStatusCode_ThrowsOnError()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
    }

    [Fact]
    public void HttpResponseMessage_EnsureSuccessStatusCode_ReturnsResponseOnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var result = response.EnsureSuccessStatusCode();

        Assert.Same(response, result);
    }

    [Fact]
    public void HttpResponseMessage_Dispose_DisposesContent()
    {
        var content = new StringContent("Response");
        var response = new HttpResponseMessage { Content = content };

        response.Dispose();

        Assert.Throws<ObjectDisposedException>(() => content.ReadAsStringAsync().Result);
    }

    #endregion
}
#endif
