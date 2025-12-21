// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System.Net.Http;
using Xunit;

/// <summary>
/// HttpMethod 클래스에 대한 단위 테스트입니다.
/// </summary>
public class HttpMethodTests
{
    [Fact]
    public void Get_ReturnsGetMethod()
    {
        var method = HttpMethod.Get;

        Assert.NotNull(method);
        Assert.Equal("GET", method.Method);
    }

    [Fact]
    public void Post_ReturnsPostMethod()
    {
        var method = HttpMethod.Post;

        Assert.NotNull(method);
        Assert.Equal("POST", method.Method);
    }

    [Fact]
    public void Put_ReturnsPutMethod()
    {
        var method = HttpMethod.Put;

        Assert.NotNull(method);
        Assert.Equal("PUT", method.Method);
    }

    [Fact]
    public void Delete_ReturnsDeleteMethod()
    {
        var method = HttpMethod.Delete;

        Assert.NotNull(method);
        Assert.Equal("DELETE", method.Method);
    }

    [Fact]
    public void Patch_ReturnsPatchMethod()
    {
        var method = HttpMethod.Patch;

        Assert.NotNull(method);
        Assert.Equal("PATCH", method.Method);
    }

    [Fact]
    public void Head_ReturnsHeadMethod()
    {
        var method = HttpMethod.Head;

        Assert.NotNull(method);
        Assert.Equal("HEAD", method.Method);
    }

    [Fact]
    public void Options_ReturnsOptionsMethod()
    {
        var method = HttpMethod.Options;

        Assert.NotNull(method);
        Assert.Equal("OPTIONS", method.Method);
    }

    [Fact]
    public void Trace_ReturnsTraceMethod()
    {
        var method = HttpMethod.Trace;

        Assert.NotNull(method);
        Assert.Equal("TRACE", method.Method);
    }

    [Fact]
    public void Constructor_WithCustomMethod_CreatesMethod()
    {
        var method = new HttpMethod("CUSTOM");

        Assert.Equal("CUSTOM", method.Method);
    }

    [Fact]
    public void Equals_SameMethod_ReturnsTrue()
    {
        var method1 = HttpMethod.Get;
        var method2 = HttpMethod.Get;

        Assert.Equal(method1, method2);
    }

    [Fact]
    public void Equals_DifferentMethod_ReturnsFalse()
    {
        var method1 = HttpMethod.Get;
        var method2 = HttpMethod.Post;

        Assert.NotEqual(method1, method2);
    }

    [Fact]
    public void Equals_CustomMethodWithSameName_ReturnsTrue()
    {
        var method1 = new HttpMethod("GET");
        var method2 = HttpMethod.Get;

        Assert.Equal(method1, method2);
    }

    [Fact]
    public void ToString_ReturnsMethodName()
    {
        var method = HttpMethod.Get;

        Assert.Equal("GET", method.ToString());
    }

    [Fact]
    public void GetHashCode_SameMethod_ReturnsSameHash()
    {
        var method1 = HttpMethod.Get;
        var method2 = new HttpMethod("GET");

        Assert.Equal(method1.GetHashCode(), method2.GetHashCode());
    }
}
