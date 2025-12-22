// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System;
using global::System.IO;
using global::System.Net.Http;
using global::System.Text;
using global::System.Threading.Tasks;
using NUnit.Framework;

/// <summary>
/// HttpContent 파생 클래스들에 대한 단위 테스트입니다.
/// </summary>
public class HttpContentTests
{
    #region StringContent Tests

    [Test]
    public void StringContent_Constructor_WithString_SetsContent()
    {
        var content = new StringContent("Hello, World!");

        var result = content.ReadAsStringAsync().Result;

        Assert.AreEqual("Hello, World!", result);
    }

    [Test]
    public void StringContent_Constructor_WithEncoding_SetsContentType()
    {
        var content = new StringContent("Hello", Encoding.UTF8, "text/plain");

        Assert.Contains("text/plain", content.Headers.ContentType ?? string.Empty);
    }

    [Test]
    public void StringContent_ReadAsByteArrayAsync_ReturnsBytes()
    {
        var text = "Hello, World!";
        var content = new StringContent(text);

        var bytes = content.ReadAsByteArrayAsync().Result;

        Assert.IsNotNull(bytes);
        Assert.IsTrue(bytes.Length > 0);
    }

    #endregion

    #region ByteArrayContent Tests

    [Test]
    public void ByteArrayContent_Constructor_WithByteArray_SetsContent()
    {
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        var content = new ByteArrayContent(data);

        var result = content.ReadAsByteArrayAsync().Result;

        Assert.AreEqual(data, result);
    }

    [Test]
    public void ByteArrayContent_Constructor_WithOffsetAndCount_SetsPartialContent()
    {
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        var content = new ByteArrayContent(data, 0, 5);

        var result = content.ReadAsByteArrayAsync().Result;

        Assert.AreEqual(5, result.Length);
        Assert.AreEqual("Hello", Encoding.UTF8.GetString(result));
    }

    [Test]
    public void ByteArrayContent_ReadAsStringAsync_ReturnsString()
    {
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        var content = new ByteArrayContent(data);

        var result = content.ReadAsStringAsync().Result;

        Assert.AreEqual("Hello, World!", result);
    }

    #endregion

    #region StreamContent Tests

    [Test]
    public void StreamContent_Constructor_WithStream_SetsContent()
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!"));
        var content = new StreamContent(stream);

        var result = content.ReadAsStringAsync().Result;

        Assert.AreEqual("Hello, World!", result);
    }

    [Test]
    public void StreamContent_ReadAsByteArrayAsync_ReturnsBytes()
    {
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        var stream = new MemoryStream(data);
        var content = new StreamContent(stream);

        var result = content.ReadAsByteArrayAsync().Result;

        Assert.AreEqual(data, result);
    }

    #endregion

    #region FormUrlEncodedContent Tests

    [Test]
    public void FormUrlEncodedContent_Constructor_WithKeyValuePairs_EncodesContent()
    {
        var pairs = new[]
        {
            new KeyValuePair<string, string>("key1", "value1"),
            new KeyValuePair<string, string>("key2", "value2"),
        };
        var content = new FormUrlEncodedContent(pairs);

        var result = content.ReadAsStringAsync().Result;

        Assert.Contains("key1=value1", result);
        Assert.Contains("key2=value2", result);
        Assert.Contains("&", result);
    }

    [Test]
    public void FormUrlEncodedContent_EncodesSpecialCharacters()
    {
        var pairs = new[]
        {
            new KeyValuePair<string, string>("key", "value with spaces"),
        };
        var content = new FormUrlEncodedContent(pairs);

        var result = content.ReadAsStringAsync().Result;

        Assert.DoesNotContain(" ", result);
        Assert.Contains("key=value", result);
    }

    [Test]
    public void FormUrlEncodedContent_ContentType_IsFormUrlEncoded()
    {
        var pairs = new[]
        {
            new KeyValuePair<string, string>("key", "value"),
        };
        var content = new FormUrlEncodedContent(pairs);

        Assert.Contains("application/x-www-form-urlencoded", content.Headers.ContentType ?? string.Empty);
    }

    #endregion

    #region Headers Tests

    [Test]
    public void HttpContent_Headers_CanSetContentType()
    {
        var content = new StringContent("Hello");
        content.Headers.ContentType = "application/json";

        Assert.AreEqual("application/json", content.Headers.ContentType);
    }

    [Test]
    public void HttpContent_Headers_CanSetContentLength()
    {
        var content = new StringContent("Hello");

        // ContentLength는 자동으로 계산됨
        Assert.IsTrue(content.Headers.ContentLength > 0);
    }

    #endregion

    #region CopyTo Tests

    [Test]
    public void HttpContent_CopyToAsync_CopiesToStream()
    {
        var content = new StringContent("Hello, World!");
        var targetStream = new MemoryStream();

        content.CopyToAsync(targetStream).Wait();
        targetStream.Position = 0;

        var result = new StreamReader(targetStream).ReadToEnd();
        Assert.AreEqual("Hello, World!", result);
    }

    #endregion

    #region Dispose Tests

    [Test]
    public void HttpContent_Dispose_DisposesContent()
    {
        var content = new StringContent("Hello");

        content.Dispose();

        // Dispose 후 읽기 시도하면 ObjectDisposedException 발생
        Assert.Throws<ObjectDisposedException>(() => content.ReadAsStringAsync().Result);
    }

    #endregion
}
#endif
