# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 언어 규칙

- 모든 응답은 한글로 작성
- 코드 주석도 한글로 작성
- XML 문서화 주석도 한글로 작성

## Project Overview

Jinobald.Polyfill is a .NET polyfill library that enables modern .NET features (.NET 10.0) in legacy .NET Framework versions (NET35 through NET481). Documentation is in Korean.

## Build Commands

```bash
# Restore and build all 19 target frameworks
dotnet restore Jinobald.Polyfill.sln
dotnet build Jinobald.Polyfill.sln -c Release

# Build specific framework
dotnet build -f net8.0
```

## Test Commands

Test framework: NUnit 3.14.0

```bash
# Run all tests
dotnet test

# Test specific framework
dotnet test --framework net8.0
dotnet test --framework net462

# Test specific class
dotnet test --filter "FullyQualifiedName~EnumerableBasicTests"

# Test specific method
dotnet test --filter "FullyQualifiedName~Where_Should_Filter_Elements"
```

## Architecture

**Multi-targeting Strategy:**
- Main library: `src/Jinobald.Polyfill/Jinobald.Polyfill.csproj`
- Tests: `tests/Jinobald.Polyfill.Tests/Jinobald.Polyfill.Tests.csproj`
- Shared build config: `Directory.Build.props`

**Namespace Organization:**
Files mirror .NET namespaces under `src/Jinobald.Polyfill/System/`:
- `Buffers/` - Memory operations (SpanAction)
- `Collections/Concurrent/` - ConcurrentQueue, ConcurrentStack, ConcurrentBag
- `Collections/Generic/` - IReadOnlyCollection, IReadOnlyList, IReadOnlyDictionary
- `Linq/` - LINQ operators (EnumerableEx, IGrouping, ILookup)
- `Net/Http/` - HttpClient implementation
- `Runtime/CompilerServices/` - Compiler attributes, async support
- `Threading/Tasks/` - Task, Parallel, TaskFactory

**Conditional Compilation Pattern:**
```csharp
#if NET35
namespace System
{
    public class Lazy<T> { ... }
}
#endif
```

**Target Framework Symbols:**
- Legacy: NET35, NET40, NET45, NET451, NET452, NET46, NET461, NET462, NET47, NET471, NET472, NET48, NET481
- Feature flags: NETFRAMEWORK, FeatureMemory, FeatureSpan, FeatureAsync, FeatureTPL

## Coding Conventions

- XML documentation in Korean for all public APIs
- Test method names in Korean (e.g., `All_모든_요소가_조건_만족_True()`)
- InternalsVisibleTo configured for `Jinobald.Polyfill.Tests` in `Properties/AssemblyInfo.cs`
- Nullable reference types enabled with specific warnings as errors (CS8600-CS8604, CS8625)
- Code analyzers: StyleCop, Roslynator, SonarAnalyzer

## Commit Message Format

Uses semantic versioning triggers:
- `feat:` or `feature:` → Minor version bump
- `fix:` or `patch:` → Patch version bump
- `breaking:` or `major:` → Major version bump
- `docs:`, `chore:`, `style:`, `refactor:`, `test:`, `build:` → No version bump

## Key Files

- `GitVersion.yml` - Branch-based versioning configuration
- `stylecop.json` - StyleCop rules (company: Jinobald)
- `.editorconfig` - Code style (4 spaces, CRLF, UTF-8)
