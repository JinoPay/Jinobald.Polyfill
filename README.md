# Jinobald.Polyfill

.NET Framework 3.5부터 최신 .NET까지 다양한 버전에서 최신 .NET 기능을 사용할 수 있도록 하는 Polyfill 라이브러리입니다.

## 개요

Jinobald.Polyfill은 오래된 .NET Framework 버전에서 최신 .NET의 타입과 기능을 사용할 수 있게 해주는 라이브러리입니다. 레거시 프로젝트를 유지보수하면서도 현대적인 C# 문법과 API를 활용할 수 있습니다.

## 지원 타겟 프레임워크

- .NET Framework 3.5
- .NET Framework 4.0
- .NET Framework 4.5
- .NET Framework 4.5.1
- .NET Framework 4.5.2
- .NET Framework 4.6
- .NET Framework 4.6.1
- .NET Framework 4.6.2
- .NET Framework 4.7.2
- .NET Framework 4.8
- .NET 8.0
- .NET 9.0
- .NET 10.0

## 주요 기능

### 기본 타입
- **ValueTuple** - .NET 3.5에서 ValueTuple 지원
- **Tuple Extensions** - Tuple 관련 확장 메서드
- **HashCode** - 해시코드 생성 유틸리티
- **Index & Range** - 인덱스와 범위 연산자 지원

### 메모리 관련
- **Span<T>** - 스택 기반 메모리 관리
- **ReadOnlySpan<T>** - 읽기 전용 스팬

### 컬렉션
- **IStructuralEquatable** - 구조적 동등성 비교
- **IStructuralComparable** - 구조적 비교
- **ThreadSafe.CacheDict** - 스레드 안전 캐시 딕셔너리

### 문자열 확장
- **StringEx.Join** - 문자열 결합
- **StringEx.Concat** - 문자열 연결
- **StringEx.Implode** - 배열을 문자열로 변환
- **StringEx.IsNullOrWhiteSpace** - 공백 문자열 검사

### 스레딩 (.NET 3.5용)
- **Task** - 비동기 작업 표현
- **Task<TResult>** - 결과를 반환하는 비동기 작업
- **TaskFactory** - Task 생성 및 스케줄링 지원
- **CancellationToken** - 작업 취소 알림 전파
- **CancellationTokenSource** - CancellationToken 신호
- **TaskAwaiter** - async/await 지원 (NET35, NET40)
- **AsyncTaskMethodBuilder** - async 메서드 빌더 (NET35)
- **Volatile** - Volatile 읽기/쓰기 작업
- **InterlockedEx** - Interlocked 확장

### 컴파일러 서비스 속성 ✅
- **CallerMemberNameAttribute** - 호출자 멤버 이름 자동 전달 (.NET 2.0, 3.5, 4.0용)
- **CallerFilePathAttribute** - 호출자 파일 경로 자동 전달 (.NET 2.0, 3.5, 4.0용)
- **CallerLineNumberAttribute** - 호출자 라인 번호 자동 전달 (.NET 2.0, 3.5, 4.0용)
- **ExtensionAttribute** - 확장 메서드 지원 (.NET 2.0용)
- **IsExternalInit** - record 및 init-only 속성 지원 (.NET 4.6~4.8용)
- **RequiredMemberAttribute** - 필수 멤버 지정 (.NET 4.7~4.8용)
- **SetsRequiredMembersAttribute** - 생성자에서 필수 멤버 설정 (.NET 4.7~4.8용)

### HTTP 및 네트워킹 (계획 중)
- **HttpClient** - 비동기 HTTP 요청 (.NET 3.5~4.5용)
- **HttpRequestMessage / HttpResponseMessage** - HTTP 메시지
- **HttpContent** - StringContent, ByteArrayContent, FormUrlEncoded 등
- **HttpMethod** - HTTP 메서드 정의 (GET, POST, PUT, DELETE)
- **HttpHeaders** - HTTP 헤더 관리

### JSON 직렬화 (계획 중)
- **JsonSerializer** - System.Text.Json 스타일 직렬화 (.NET 2.0+용)
- **JsonSerializerOptions** - 직렬화 옵션 (PropertyNamingPolicy, WriteIndented 등)
- **JsonPropertyNameAttribute / JsonIgnoreAttribute** - JSON 속성 제어

### 최신 유틸리티 타입 (계획 중)
- **DateOnly / TimeOnly** - 날짜/시간 전용 타입 (.NET 4.5+용)
- **Half** - 16비트 부동소수점 (.NET 4.5+용)
- **UnreachableException** - 도달 불가 코드 표시 (.NET 4.7+용)
- **Nullable 분석 속성** - NotNullWhen, MaybeNull, MemberNotNull 등

### 최신 LINQ 메서드 (계획 중)
- **Chunk** - 컬렉션 청크 분할 (.NET 6+용)
- **DistinctBy / ExceptBy / IntersectBy / UnionBy** - 키 기반 집합 연산 (.NET 6+용)
- **MinBy / MaxBy** - 키 기반 최소/최대 (.NET 6+용)
- **Index / CountBy / AggregateBy** - 최신 LINQ 확장 (.NET 9+용)

### 기타
- **AggregateException** - 집계 예외 처리
- **WeakReference<T>** - 약한 참조
- **NumericHelper** - 숫자 관련 헬퍼 유틸리티

## 설치

```bash
# NuGet 패키지로 설치 (예정)
dotnet add package Jinobald.Polyfill
```

## 사용 예제

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// Task 사용 (.NET 3.5에서도 가능)
var task = Task.Run(() => {
    Console.WriteLine("Running in background");
    return 42;
});
Console.WriteLine($"Result: {task.Result}");

// Task.WhenAll 사용
var task1 = Task.Run(() => 1);
var task2 = Task.Run(() => 2);
var task3 = Task.Run(() => 3);
var results = Task.WhenAll(task1, task2, task3).Result;

// CancellationToken 사용
var cts = new CancellationTokenSource();
var cancelableTask = Task.Run(() => {
    while (!cts.Token.IsCancellationRequested) {
        Thread.Sleep(100);
    }
}, cts.Token);
cts.CancelAfter(1000);

// ValueTuple 사용 (.NET 3.5에서도 가능)
var tuple = (Name: "John", Age: 30);
Console.WriteLine($"{tuple.Name} is {tuple.Age} years old");

// StringEx 사용
var isEmpty = StringEx.IsNullOrWhiteSpace("   ");  // true
var joined = StringEx.Join(", ", new[] { "a", "b", "c" });  // "a, b, c"
```

## 빌드

```bash
# 솔루션 빌드
dotnet build Jinobald.Polifill.sln

# 테스트 실행
dotnet test
```

## 프로젝트 구조

```
Jinobald.Polyfill/
├── src/
│   └── Jinobald.Polyfill/           # 메인 라이브러리
│       ├── Properties/
│       │   └── AssemblyInfo.cs      # InternalsVisibleTo 설정
│       └── System/                  # System 네임스페이스 확장
│           ├── Collections/         # 컬렉션 관련
│           ├── Diagnostics/         # 진단 관련
│           ├── Numerics/            # 숫자 관련
│           ├── Runtime/             # 런타임 관련
│           │   └── CompilerServices/ # 컴파일러 속성
│           └── Threading/           # 스레딩 관련
├── tests/
│   └── Jinobald.Polyfill.Tests/     # 단위 테스트
└── docs/
    ├── IMPLEMENTATION_PLAN.md       # 구현 계획
    ├── TESTING_STRATEGY.md          # 테스트 전략
    └── POLYFILL_ANALYSIS_REPORT.md  # 분석 및 진행 상황 (2025-12-21)
```

## 테스트 전략

이 라이브러리는 Internal 타입 테스트를 위해 `InternalsVisibleTo` 속성을 사용합니다:

- **Public API**: 외부 테스트 프로젝트에서 직접 테스트
- **Internal 타입**: AssemblyInfo.cs에 InternalsVisibleTo 설정으로 테스트 프로젝트 접근 허용
- **컴파일러 전용 타입**: 리플렉션을 통한 존재 확인

자세한 내용은 [docs/TESTING_STRATEGY.md](docs/TESTING_STRATEGY.md)를 참조하세요.

## 기여

버그 리포트, 기능 제안, Pull Request를 환영합니다.

## 라이선스

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

## 저작권

Copyright (c) 2025 Jinho Park

## 최근 업데이트 (2025-12-21)

### 🔧 Critical 타입 충돌 수정 완료
다음 타입들의 조건부 컴파일을 수정하여 .NET 4.6+ 프레임워크에서의 타입 충돌을 해결했습니다:
- **FormattableString** - .NET 4.6+ 충돌 해결
- **HashCode** - .NET 4.7.1+ 충돌 해결
- **ITuple** - .NET 4.7+ 충돌 해결
- **FormattableStringFactory** - .NET 4.6+ 충돌 해결

### 📊 현재 진행 상황
- **구현 완료**: 약 50개 타입 (전체의 33%)
- **Phase 1 진행률**: 75% (델리게이트, Tuple, Compiler Attributes 대부분 완료)
- **테스트 커버리지**: 구현된 타입의 약 50%

### 📝 상세 분석 보고서
전체 분석 결과 및 권장사항은 [POLYFILL_ANALYSIS_REPORT.md](POLYFILL_ANALYSIS_REPORT.md)를 참조하세요.

### 다음 단계
1. Index & Range 구현 (C# 8.0 지원)
2. LINQ 기본 연산자 구현 시작
3. 누락된 테스트 추가 (Lazy, FormattableString 등)

## 참고

이 라이브러리의 일부 코드는 .NET Foundation의 코드를 기반으로 하며, MIT 라이선스 하에 배포됩니다.
