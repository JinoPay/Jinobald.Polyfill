// Copyright (c) 2024 Jinobald. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK
namespace Jinobald.Polyfill.Tests.System.Net.Http;

using global::System.IO;
using global::System.Linq;
using global::System.Net.Http;
using global::System.Text;
using NUnit.Framework;

/// <summary>
/// MultipartContent 클래스들에 대한 단위 테스트입니다.
/// </summary>
public class MultipartContentTests
{
    #region MultipartContent Tests

    [Test]
    public void MultipartContent_DefaultConstructor_CreatesInstance()
    {
        var content = new MultipartContent();

        Assert.IsNotNull(content);
        Assert.Contains("multipart/mixed", content.Headers.ContentType ?? string.Empty);
    }

    [Test]
    public void MultipartContent_Constructor_WithSubtype_SetsContentType()
    {
        var content = new MultipartContent("form-data");

        Assert.Contains("multipart/form-data", content.Headers.ContentType ?? string.Empty);
    }

    [Test]
    public void MultipartContent_Constructor_WithBoundary_SetsBoundary()
    {
        var boundary = "----MyBoundary123";
        var content = new MultipartContent("form-data", boundary);

        Assert.Contains(boundary, content.Headers.ContentType ?? string.Empty);
    }

    [Test]
    public void MultipartContent_Add_AddsContent()
    {
        var content = new MultipartContent();
        var part = new StringContent("Hello");

        content.Add(part);

        Assert.AreEqual(1, content.Count());
    }

    [Test]
    public void MultipartContent_Add_MultipleContents()
    {
        var content = new MultipartContent();
        content.Add(new StringContent("Part 1"));
        content.Add(new StringContent("Part 2"));
        content.Add(new StringContent("Part 3"));

        Assert.AreEqual(3, content.Count());
    }

    [Test]
    public void MultipartContent_Enumerable_IteratesOverParts()
    {
        var content = new MultipartContent();
        content.Add(new StringContent("Part 1"));
        content.Add(new StringContent("Part 2"));

        var count = 0;
        foreach (var part in content)
        {
            count++;
            Assert.IsNotNull(part);
        }

        Assert.AreEqual(2, count);
    }

    #endregion

    #region MultipartFormDataContent Tests

    [Test]
    public void MultipartFormDataContent_DefaultConstructor_CreatesInstance()
    {
        var content = new MultipartFormDataContent();

        Assert.IsNotNull(content);
        Assert.Contains("multipart/form-data", content.Headers.ContentType ?? string.Empty);
    }

    [Test]
    public void MultipartFormDataContent_Constructor_WithBoundary_SetsBoundary()
    {
        var boundary = "----FormBoundary";
        var content = new MultipartFormDataContent(boundary);

        Assert.Contains(boundary, content.Headers.ContentType ?? string.Empty);
    }

    [Test]
    public void MultipartFormDataContent_Add_WithName_AddsContent()
    {
        var content = new MultipartFormDataContent();
        var stringContent = new StringContent("value");

        content.Add(stringContent, "fieldName");

        Assert.AreEqual(1, content.Count());
    }

    [Test]
    public void MultipartFormDataContent_Add_WithNameAndFileName_AddsFileContent()
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("file content"));

        content.Add(fileContent, "file", "document.txt");

        Assert.AreEqual(1, content.Count());
    }

    [Test]
    public void MultipartFormDataContent_ReadAsStringAsync_ContainsBoundary()
    {
        var boundary = "----TestBoundary";
        var content = new MultipartFormDataContent(boundary);
        content.Add(new StringContent("value"), "field");

        var result = content.ReadAsStringAsync().Result;

        Assert.Contains(boundary, result);
    }

    [Test]
    public void MultipartFormDataContent_ReadAsStringAsync_ContainsContentDisposition()
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("value"), "fieldName");

        var result = content.ReadAsStringAsync().Result;

        Assert.Contains("Content-Disposition", result);
        Assert.Contains("fieldName", result);
    }

    [Test]
    public void MultipartFormDataContent_ReadAsStringAsync_ContainsFileInfo()
    {
        var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(new byte[] { 1, 2, 3 }), "file", "test.bin");

        var result = content.ReadAsStringAsync().Result;

        Assert.Contains("filename", result);
        Assert.Contains("test.bin", result);
    }

    #endregion

    #region Serialization Tests

    [Test]
    public void MultipartContent_Serialization_ContainsBoundaryDelimiters()
    {
        var boundary = "----Boundary";
        var content = new MultipartContent("mixed", boundary);
        content.Add(new StringContent("Part 1"));
        content.Add(new StringContent("Part 2"));

        var stream = new MemoryStream();
        content.CopyToAsync(stream).Wait();
        stream.Position = 0;

        var result = new StreamReader(stream).ReadToEnd();

        // 시작/종료 경계 확인
        Assert.Contains("--" + boundary, result);
        Assert.Contains("--" + boundary + "--", result);
    }

    [Test]
    public void MultipartContent_Serialization_ContainsAllParts()
    {
        var content = new MultipartContent();
        content.Add(new StringContent("First Part"));
        content.Add(new StringContent("Second Part"));

        var result = content.ReadAsStringAsync().Result;

        Assert.Contains("First Part", result);
        Assert.Contains("Second Part", result);
    }

    #endregion
}
#endif
