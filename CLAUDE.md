# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 언어 규칙

- 모든 응답은 한글로 작성
- 코드 주석도 한글로 작성
- XML 문서화 주석도 한글로 작성

## 핵심 원칙

**버전 간 동작 일관성 보장이 최우선 목표입니다.**

- NET35부터 최신 버전까지 모든 타겟 프레임워크에서 완벽하게 동일한 기능 동작을 보장해야 함
- 버전마다 다르게 동작하는 코드는 절대 허용되지 않음
- 새 기능 구현 시 모든 타겟 프레임워크에서 테스트 필수
- 조건부 컴파일(`#if`)은 구현 방식만 다르게 하고, 외부 동작은 반드시 동일해야 함
- **테스트 코드는 버전별로 분기하지 않음** - 동일한 테스트 코드가 모든 프레임워크에서 실행되어 동일한 결과를 보장해야 함
- **완전한 폴리필 구현** - 땜빵이나 우회 구현이 아닌 실제 타입을 완벽하게 구현해야 함. 예: Span이 없으면 Span을 구현하고, 그 과정에서 Memory가 필요하면 Memory도 구현함. 의존성 타입까지 모두 완전하게 구현하는 것이 원칙
- **누락 기능 캐치 및 구현** - 특정 프레임워크 버전에서 없거나 누락된 타입/기능을 발견하면 반드시 구현해야 함. 빌드나 테스트 중 누락된 것이 발견되면 즉시 폴리필로 추가

## Project Overview

Jinobald.Polyfill은 최신 .NET 기능(.NET 10.0)을 레거시 .NET Framework 버전(NET35~NET481)에서 사용할 수 있게 하는 .NET 폴리필 라이브러리입니다. 문서화는 한글로 작성됩니다.

## Build Commands

```bash
# 전체 18개 타겟 프레임워크 복원 및 빌드
dotnet restore Jinobald.Polyfill.sln
dotnet build Jinobald.Polyfill.sln -c Release

# 특정 프레임워크 빌드
dotnet build -f net8.0
```

## Test Commands

테스트 프레임워크: NUnit 3.14.0

```bash
# 전체 테스트 실행
dotnet test

# 특정 프레임워크 테스트
dotnet test --framework net8.0
dotnet test --framework net462

# 특정 클래스 테스트
dotnet test --filter "FullyQualifiedName~EnumerableBasicTests"

# 특정 메서드 테스트
dotnet test --filter "FullyQualifiedName~Where_Should_Filter_Elements"
```

## Architecture

**멀티 타겟팅 전략:**
- 메인 라이브러리: `src/Jinobald.Polyfill/Jinobald.Polyfill.csproj`
- 테스트: `tests/Jinobald.Polyfill.Tests/Jinobald.Polyfill.Tests.csproj`
- 공유 빌드 설정: `Directory.Build.props`

**타겟 프레임워크 (18개):**
- .NET Framework: net35, net40, net45, net451, net452, net46, net461, net462, net47, net471, net472, net48, net481
- Modern .NET: net6.0, net7.0, net8.0, net9.0, net10.0

**네임스페이스 구조:**
`src/Jinobald.Polyfill/System/` 아래 .NET 네임스페이스를 미러링:
- `Buffers/` - ArrayPool, MemoryPool, SpanAction, MemoryManager 등 (8개 파일)
- `Collections/Concurrent/` - ConcurrentQueue, ConcurrentStack, ConcurrentBag (3개 파일)
- `Collections/Generic/` - IReadOnlyCollection, IReadOnlyList, IReadOnlyDictionary (3개 파일)
- `Diagnostics/CodeAnalysis/` - Nullable 분석 어트리뷰트 (11개 파일)
- `Linq/` - LINQ 연산자 (16개 파일)
- `Net/Http/` - HttpClient 구현 (15개 파일)
- `Runtime/CompilerServices/` - 컴파일러 속성, async 지원 (15개 파일)
- `Runtime/InteropServices/` - MemoryMarshal (1개 파일)
- `Threading/` - CancellationToken, SpinWait 등 스레딩 유틸리티 (7개 파일)
- `Threading/Tasks/` - Task, Parallel, ValueTask 등 (12개 파일)

**현재 파일 수:**
- 소스 파일: 127개
- 테스트 파일: 53개

**조건부 컴파일 패턴:**
```csharp
#if NET35
namespace System
{
    public class Lazy<T> { ... }
}
#endif
```

**타겟 프레임워크 심볼:**
- 레거시: NET35, NET40, NET45, NET451, NET452, NET46, NET461, NET462, NET47, NET471, NET472, NET48, NET481
- 기능 플래그: NETFRAMEWORK, FeatureMemory, AllowUnsafeBlocks

## Coding Conventions

- 모든 public API에 한글 XML 문서화
- 테스트 메서드 이름은 한글 (예: `All_모든_요소가_조건_만족_True()`)
- `Properties/AssemblyInfo.cs`에 `Jinobald.Polyfill.Tests`용 InternalsVisibleTo 설정
- Nullable 참조 타입 활성화, 특정 경고를 오류로 처리 (CS8600-CS8604, CS8625)
- 코드 분석기: StyleCop, Roslynator, SonarAnalyzer

## Commit Message Format

시맨틱 버저닝 트리거:
- `feat:` 또는 `feature:` → 마이너 버전 증가
- `fix:` 또는 `patch:` → 패치 버전 증가
- `breaking:` 또는 `major:` → 메이저 버전 증가
- `docs:`, `chore:`, `style:`, `refactor:`, `test:`, `build:` → 버전 변경 없음

## Key Files

- `GitVersion.yml` - 브랜치 기반 버전 관리 설정
- `stylecop.json` - StyleCop 규칙 (company: Jinobald)
- `.editorconfig` - 코드 스타일 (4 spaces, CRLF, UTF-8)
