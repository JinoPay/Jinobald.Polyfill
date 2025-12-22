// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System.Net.Http;
using NUnit.Framework;

/// <summary>
/// HttpMethod 클래스에 대한 단위 테스트입니다.
/// </summary>
public class HttpMethodTests
{
    [Test]
    public void Get_ReturnsGetMethod()
    {
        var method = HttpMethod.Get;

        Assert.IsNotNull(method);
        Assert.AreEqual("GET", method.Method);
    }

    [Test]
    public void Post_ReturnsPostMethod()
    {
        var method = HttpMethod.Post;

        Assert.IsNotNull(method);
        Assert.AreEqual("POST", method.Method);
    }

    [Test]
    public void Put_ReturnsPutMethod()
    {
        var method = HttpMethod.Put;

        Assert.IsNotNull(method);
        Assert.AreEqual("PUT", method.Method);
    }

    [Test]
    public void Delete_ReturnsDeleteMethod()
    {
        var method = HttpMethod.Delete;

        Assert.IsNotNull(method);
        Assert.AreEqual("DELETE", method.Method);
    }

    [Test]
    public void Patch_ReturnsPatchMethod()
    {
        var method = HttpMethod.Patch;

        Assert.IsNotNull(method);
        Assert.AreEqual("PATCH", method.Method);
    }

    [Test]
    public void Head_ReturnsHeadMethod()
    {
        var method = HttpMethod.Head;

        Assert.IsNotNull(method);
        Assert.AreEqual("HEAD", method.Method);
    }

    [Test]
    public void Options_ReturnsOptionsMethod()
    {
        var method = HttpMethod.Options;

        Assert.IsNotNull(method);
        Assert.AreEqual("OPTIONS", method.Method);
    }

    [Test]
    public void Trace_ReturnsTraceMethod()
    {
        var method = HttpMethod.Trace;

        Assert.IsNotNull(method);
        Assert.AreEqual("TRACE", method.Method);
    }

    [Test]
    public void Constructor_WithCustomMethod_CreatesMethod()
    {
        var method = new HttpMethod("CUSTOM");

        Assert.AreEqual("CUSTOM", method.Method);
    }

    [Test]
    public void Equals_SameMethod_ReturnsTrue()
    {
        var method1 = HttpMethod.Get;
        var method2 = HttpMethod.Get;

        Assert.AreEqual(method1, method2);
    }

    [Test]
    public void Equals_DifferentMethod_ReturnsFalse()
    {
        var method1 = HttpMethod.Get;
        var method2 = HttpMethod.Post;

        Assert.AreNotEqual(method1, method2);
    }

    [Test]
    public void Equals_CustomMethodWithSameName_ReturnsTrue()
    {
        var method1 = new HttpMethod("GET");
        var method2 = HttpMethod.Get;

        Assert.AreEqual(method1, method2);
    }

    [Test]
    public void ToString_ReturnsMethodName()
    {
        var method = HttpMethod.Get;

        Assert.AreEqual("GET", method.ToString());
    }

    [Test]
    public void GetHashCode_SameMethod_ReturnsSameHash()
    {
        var method1 = HttpMethod.Get;
        var method2 = new HttpMethod("GET");

        Assert.AreEqual(method1.GetHashCode(), method2.GetHashCode());
    }
}
