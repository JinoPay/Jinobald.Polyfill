# 테스트 전략 가이드

## 테스트 프로젝트 구조

Jinobald.Polyfill은 외부 테스트 프로젝트를 사용하며, `InternalsVisibleTo`를 통해 internal 타입에도 접근할 수 있습니다.

### 프로젝트 위치

**테스트 프로젝트**: `tests/Jinobald.Polyfill.Tests/`

**테스트 파일**: 39개
**테스트 케이스**: 473개 이상

### 테스트 파일 구조

```
tests/Jinobald.Polyfill.Tests/
├── GlobalUsings.cs
└── System/
    ├── ActionTests.cs
    ├── FuncTests.cs
    ├── HashCodeTests.cs
    ├── MemoryTests.cs
    ├── PredicateTests.cs
    ├── ProgressTests.cs
    ├── ReadOnlyMemoryTests.cs
    ├── ReadOnlySpanTests.cs
    ├── SpanTests.cs
    ├── TupleTests.cs
    ├── ValueTupleTests.cs
    ├── StringExTests.cs
    ├── StringExTests.Contains.cs
    ├── StringExTests.Create.cs
    ├── StringExTests.EndsWith.cs
    ├── StringExTests.GetHashCode.cs
    ├── StringExTests.Replace.cs
    ├── StringExTests.Split.cs
    ├── StringExTests.StartsWith.cs
    ├── StringExTests.Trim.cs
    ├── Linq/
    │   ├── EnumerableAggregateTests.cs
    │   ├── EnumerableBasicTests.cs
    │   ├── EnumerableConversionTests.cs
    │   ├── EnumerableGroupingTests.cs
    │   ├── EnumerableJoinTests.cs
    │   ├── EnumerableSetTests.cs
    │   └── EnumerableSortingTests.cs
    ├── Net/Http/
    │   ├── HttpClientTests.cs
    │   ├── HttpContentTests.cs
    │   ├── HttpHeadersTests.cs
    │   ├── HttpMessageTests.cs
    │   ├── HttpMethodTests.cs
    │   └── MultipartContentTests.cs
    ├── Runtime/CompilerServices/
    │   ├── CallerInfoTests.cs
    │   └── CompilerAttributesTests.cs
    └── Threading/Tasks/
        ├── ParallelTests.cs
        ├── TaskTests.cs
        └── ThreadingUtilitiesTests.cs
```

---

## InternalsVisibleTo 설정

**설정 파일**: `src/Jinobald.Polyfill/Properties/AssemblyInfo.cs`

```csharp
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Jinobald.Polyfill.Tests")]
[assembly: InternalsVisibleTo("Jinobald.Polyfill.InternalTests")]
```

이 설정으로 테스트 프로젝트에서 internal 타입에 직접 접근할 수 있습니다.

---

## 테스트 접근 방식

### 1. Public API 테스트 (권장)

가능하면 public API를 통해 테스트하세요:

```csharp
// Good: Public API 테스트
[Fact]
public void CallerMemberName_Should_Be_Filled_By_Compiler()
{
    var result = GetCallerMemberName();
    Assert.Equal(nameof(CallerMemberName_Should_Be_Filled_By_Compiler), result);
}

private string GetCallerMemberName([CallerMemberName] string memberName = null)
{
    return memberName;
}
```

### 2. Internal 타입 테스트

Internal 타입은 InternalsVisibleTo를 통해 직접 접근하거나 리플렉션을 사용합니다:

```csharp
// Internal 타입 존재 확인 (리플렉션)
[Fact]
public void IsExternalInit_Should_Exist()
{
    var assembly = typeof(SomePublicType).Assembly;
    var type = assembly.GetType("System.Runtime.CompilerServices.IsExternalInit");
    Assert.NotNull(type);
}
```

### 3. 조건부 컴파일 활용

프레임워크별로 다른 테스트:

```csharp
#if NET35 || NET40
[Fact]
public void CallerInfo_Should_Work_In_Old_Framework()
{
    // .NET 3.5-4.0에서만 실행되는 테스트
}
#else
[Fact]
public void CallerInfo_Should_Use_BuiltIn_In_New_Framework()
{
    // .NET 4.5+에서는 빌트인 기능 사용
    Assert.True(true);
}
#endif
```

---

## 테스트 영역별 가이드

### LINQ 테스트 (7개 파일)

**테스트 파일**:
- `EnumerableBasicTests.cs` - 기본 연산자 (Where, Select, First 등)
- `EnumerableConversionTests.cs` - 변환 연산자 (ToArray, ToList 등)
- `EnumerableSortingTests.cs` - 정렬 연산자 (OrderBy, ThenBy)
- `EnumerableGroupingTests.cs` - 그룹화 연산자 (GroupBy, ToLookup)
- `EnumerableJoinTests.cs` - 조인 연산자 (Join, GroupJoin)
- `EnumerableSetTests.cs` - 집합 연산자 (Union, Intersect, Except)
- `EnumerableAggregateTests.cs` - 집계 연산자 (Sum, Average, Min, Max)

**테스트 패턴**:
```csharp
[Fact]
public void Where_Should_Filter_Elements()
{
    var source = new[] { 1, 2, 3, 4, 5 };
    var result = source.Where(x => x % 2 == 0).ToArray();
    Assert.Equal(new[] { 2, 4 }, result);
}

[Fact]
public void Where_Should_Throw_On_Null_Source()
{
    int[] source = null;
    Assert.Throws<ArgumentNullException>(() => source.Where(x => true).ToArray());
}
```

### HttpClient 테스트 (6개 파일)

**테스트 파일**:
- `HttpClientTests.cs` - HttpClient 기본 동작
- `HttpContentTests.cs` - StringContent, ByteArrayContent 등
- `HttpHeadersTests.cs` - HTTP 헤더 관리
- `HttpMessageTests.cs` - Request/Response 메시지
- `HttpMethodTests.cs` - HTTP 메서드
- `MultipartContentTests.cs` - 멀티파트 콘텐츠

**테스트 패턴**:
```csharp
[Fact]
public void HttpMethod_Get_Should_Be_GET()
{
    Assert.Equal("GET", HttpMethod.Get.Method);
}

[Fact]
public async Task StringContent_ReadAsStringAsync_Should_Return_Content()
{
    var content = new StringContent("test content");
    var result = await content.ReadAsStringAsync();
    Assert.Equal("test content", result);
}
```

### Threading 테스트 (3개 파일)

**테스트 파일**:
- `ParallelTests.cs` - Parallel.For, Parallel.ForEach
- `TaskTests.cs` - Task, Task.Run, Task.WhenAll
- `ThreadingUtilitiesTests.cs` - CancellationToken, SpinWait 등

**테스트 패턴**:
```csharp
[Fact]
public void ParallelFor_Should_Execute_All_Iterations()
{
    var count = 0;
    Parallel.For(0, 100, i => Interlocked.Increment(ref count));
    Assert.Equal(100, count);
}

[Fact]
public async Task TaskWhenAll_Should_Wait_For_All()
{
    var task1 = Task.Run(() => 1);
    var task2 = Task.Run(() => 2);
    var results = await Task.WhenAll(task1, task2);
    Assert.Equal(new[] { 1, 2 }, results);
}
```

---

## 테스트 실행

### 모든 테스트 실행

```bash
dotnet test
```

### 특정 프레임워크에서만 실행

```bash
dotnet test --framework net8.0
dotnet test --framework net48
dotnet test --framework net462
```

### 특정 테스트 클래스만 실행

```bash
dotnet test --filter "FullyQualifiedName~EnumerableBasicTests"
dotnet test --filter "FullyQualifiedName~HttpClientTests"
```

### 특정 테스트 메서드만 실행

```bash
dotnet test --filter "FullyQualifiedName~Where_Should_Filter_Elements"
```

### 코드 커버리지 수집

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## 테스트 작성 체크리스트

새로운 기능 추가 시:

- [ ] 정상 케이스 테스트 작성
- [ ] 경계값 테스트 작성 (빈 컬렉션, null, 최대/최소값)
- [ ] 예외 케이스 테스트 작성 (ArgumentNullException 등)
- [ ] 조건부 컴파일이 필요한 경우 `#if` 지시문 사용
- [ ] 모든 타겟 프레임워크에서 빌드 및 테스트 성공 확인
- [ ] XML 문서 주석 추가 (public API만)

---

## 프레임워크별 테스트 타겟

테스트 프로젝트는 다음 프레임워크를 지원합니다:

### .NET Framework
- NET462, NET47, NET471, NET472, NET48, NET481

### Modern .NET
- NET6.0, NET7.0, NET8.0, NET9.0, NET10.0

---

## 참고 자료

- [InternalsVisibleTo 공식 문서](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute)
- [.NET 테스트 베스트 프랙티스](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [xUnit 문서](https://xunit.net/)
- [조건부 컴파일](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives)

---

**마지막 업데이트**: 2025-12-22
**테스트 케이스**: 473개+
**테스트 파일**: 39개
