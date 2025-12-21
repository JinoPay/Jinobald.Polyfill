# 테스트 전략 가이드

## 테스트 프로젝트 구조

Jinobald.Polyfill은 두 가지 테스트 접근 방식을 지원합니다:

### 1. 외부 테스트 프로젝트 (현재 구조)

**위치**: `tests/Jinobald.Polyfill.Tests/`

**장점**:
- 프로덕션 코드와 테스트 코드의 명확한 분리
- 일반적인 .NET 프로젝트 구조 패턴
- NuGet 패키지 배포 시 테스트 코드가 포함되지 않음
- 여러 프레임워크에서 동일한 테스트 실행 가능

**사용 시기**:
- Public API 테스트
- 통합 테스트
- 일반적인 기능 테스트

**접근 가능한 타입**:
- Public 타입 및 멤버
- Internal 타입 (AssemblyInfo.cs의 InternalsVisibleTo 덕분)

```csharp
// tests/Jinobald.Polyfill.Tests/System/StringExTests.cs
namespace Jinobald.Polyfill.Tests.System
{
    public class StringExTests
    {
        [Fact]
        public void IsNullOrWhiteSpace_Should_Return_True_For_Null()
        {
            Assert.True(StringEx.IsNullOrWhiteSpace(null));
        }
    }
}
```

### 2. 내부 테스트 (InternalsVisibleTo 활용)

**설정**: `src/Jinobald.Polyfill/Properties/AssemblyInfo.cs`

```csharp
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Jinobald.Polyfill.Tests")]
[assembly: InternalsVisibleTo("Jinobald.Polyfill.InternalTests")]
```

**테스트 가능한 타입**:
- `internal class IsExternalInit` - 컴파일러 전용 타입
- `internal static class InternalHelpers` - 내부 헬퍼 메서드
- `internal sealed class MemoryManager<T>` - 내부 구현 세부사항

**예제**:

```csharp
// tests/Jinobald.Polyfill.Tests/System/Runtime/CompilerServices/CompilerAttributesTests.cs
namespace Jinobald.Polyfill.Tests.System.Runtime.CompilerServices
{
    public class CompilerAttributesTests
    {
        [Fact]
        public void IsExternalInit_Should_Exist_Via_Reflection()
        {
            // IsExternalInit은 internal이므로 리플렉션으로 접근
            var assembly = typeof(CompilerAttributesTests).Assembly;
            var polyfillAssembly = assembly.GetReferencedAssemblies()
                .Where(a => a.Name.Contains("Jinobald.Polyfill"))
                .FirstOrDefault();

            if (polyfillAssembly != null)
            {
                var asm = Assembly.Load(polyfillAssembly);
                var type = asm.GetType("System.Runtime.CompilerServices.IsExternalInit");
                Assert.NotNull(type);
            }
        }
    }
}
```

---

## Internal 타입 테스트 시 권장사항

### 1. Internal 타입이 필요한 경우

다음과 같은 타입들은 internal로 유지해야 합니다:

- **컴파일러 전용 타입**: `IsExternalInit`, `AsyncMethodBuilder` 등
- **내부 구현 세부사항**: 사용자가 직접 사용하지 않는 헬퍼 클래스
- **성능 최적화 타입**: 내부적으로만 사용되는 캐시나 풀

### 2. InternalsVisibleTo를 통한 테스트

**장점**:
- Internal 타입에 직접 접근 가능
- 외부 테스트 프로젝트 구조 유지
- 프로덕션 코드에서 불필요한 public API 노출 방지

**단점**:
- 리플렉션보다는 낫지만, 여전히 내부 구현에 의존
- InternalsVisibleTo 설정 필요

### 3. 리플렉션을 통한 테스트

Internal 타입을 리플렉션으로 테스트할 때:

```csharp
[Fact]
public void Internal_Type_Should_Exist()
{
    var assembly = typeof(SomePublicType).Assembly;
    var internalType = assembly.GetType("Namespace.InternalType");

    Assert.NotNull(internalType);
    Assert.False(internalType.IsPublic);
    Assert.True(internalType.IsNotPublic); // internal or private
}
```

**장점**:
- InternalsVisibleTo 불필요
- 타입의 존재와 접근성만 검증

**단점**:
- 타입 안정성 없음
- 메서드 호출이 복잡함
- 테스트 유지보수 어려움

---

## 테스트 작성 가이드라인

### 1. Public API 우선

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

### 2. Internal 타입은 필요한 경우에만

```csharp
// Acceptable: Internal 타입의 존재 확인
[Fact]
public void IsExternalInit_Should_Exist_Via_Reflection()
{
    // 리플렉션을 통해 존재만 확인
    var type = GetInternalType("System.Runtime.CompilerServices.IsExternalInit");
    Assert.NotNull(type);
}
```

### 3. 조건부 컴파일 활용

프레임워크별로 다른 테스트:

```csharp
#if NET40 || NET35 || NET20
[Fact]
public void CallerInfo_Should_Work_In_Old_Framework()
{
    // .NET 2.0-4.0에서만 실행되는 테스트
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

## 프로젝트별 테스트 권장사항

| 구현 타입 | 접근 수준 | 테스트 방법 | 이유 |
|----------|----------|-----------|-----|
| Caller Info Attributes | Public | 외부 테스트 | 사용자가 직접 사용 |
| IsExternalInit | Internal | 리플렉션 | 컴파일러 전용 |
| Task/TaskFactory | Public | 외부 테스트 | Public API |
| Internal Helpers | Internal | InternalsVisibleTo | 복잡한 로직 검증 필요 |
| Extension Methods | Public | 외부 테스트 | 사용자가 직접 사용 |

---

## 테스트 실행

### 모든 테스트 실행

```bash
dotnet test
```

### 특정 프레임워크에서만 실행

```bash
dotnet test --framework net48
dotnet test --framework net9.0
```

### 특정 테스트 클래스만 실행

```bash
dotnet test --filter "FullyQualifiedName~CompilerAttributesTests"
```

### Internal 테스트만 실행

```bash
dotnet test --filter "FullyQualifiedName~Internal"
```

---

## 체크리스트

새로운 기능 추가 시:

- [ ] Public API는 외부 테스트 프로젝트에 테스트 추가
- [ ] Internal 타입이 필수인 경우 AssemblyInfo.cs 확인
- [ ] 조건부 컴파일이 필요한 경우 #if 지시문 사용
- [ ] 모든 타겟 프레임워크에서 빌드 및 테스트 성공 확인
- [ ] XML 문서 주석 추가 (public API만)

---

## 참고 자료

- [InternalsVisibleTo 공식 문서](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute)
- [.NET 테스트 베스트 프랙티스](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [조건부 컴파일](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives)

**마지막 업데이트**: 2025-12-21
