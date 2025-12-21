# Jinobald.Polyfill

.NET Framework 3.5부터 최신 .NET 10.0까지 다양한 버전에서 최신 .NET 기능을 사용할 수 있도록 하는 종합 Polyfill 라이브러리입니다.

## 개요

Jinobald.Polyfill은 오래된 .NET Framework 버전에서 최신 .NET의 타입과 기능을 사용할 수 있게 해주는 라이브러리입니다. 레거시 프로젝트를 유지보수하면서도 현대적인 C# 문법과 API를 활용할 수 있습니다.

## 지원 타겟 프레임워크

### .NET Framework
- .NET Framework 3.5
- .NET Framework 4.0
- .NET Framework 4.5 / 4.5.1 / 4.5.2
- .NET Framework 4.6 / 4.6.1 / 4.6.2
- .NET Framework 4.7 / 4.7.1 / 4.7.2
- .NET Framework 4.8 / 4.8.1

### Modern .NET
- .NET 6.0
- .NET 7.0
- .NET 8.0
- .NET 9.0
- .NET 10.0

## 주요 기능

### ✅ 델리게이트 (완전 구현)
- **Action** - 0~16개 파라미터 버전 (NET20, NET35, NET40+)
- **Func** - 0~16개 파라미터 버전 (NET20, NET35, NET40+)
- **Predicate\<T\>** - 조건 검사 델리게이트
- **Comparison\<T\>** - 비교 델리게이트
- **Converter\<TInput, TOutput\>** - 변환 델리게이트

### ✅ Tuple & ValueTuple (완전 구현)
- **Tuple\<T1~T8\>** - 참조 타입 튜플
- **ValueTuple\<T1~T8\>** - 값 타입 튜플 (C# 7.0 구문 지원)
- **ITuple** - 튜플 인터페이스
- **IStructuralEquatable / IStructuralComparable** - 구조적 비교

### ✅ LINQ (완전 구현)
**기본 연산자**:
- `Where`, `Select`, `SelectMany` - 필터링 및 투영
- `First`, `FirstOrDefault`, `Last`, `LastOrDefault` - 요소 선택
- `Single`, `SingleOrDefault` - 단일 요소 선택
- `Any`, `All`, `Contains` - 조건 검사
- `Count`, `LongCount` - 개수 세기
- `Skip`, `Take`, `SkipWhile`, `TakeWhile` - 페이징
- `Distinct`, `DistinctBy` - 중복 제거

**정렬**:
- `OrderBy`, `OrderByDescending` - 정렬
- `ThenBy`, `ThenByDescending` - 보조 정렬
- `Reverse` - 역순

**그룹화 및 조인**:
- `GroupBy` - 그룹화
- `Join`, `GroupJoin` - 조인
- `ToLookup` - 룩업 테이블

**집합 연산**:
- `Union`, `Intersect`, `Except` - 집합 연산
- `Concat`, `Zip` - 연결

**집계**:
- `Aggregate` - 사용자 정의 집계
- `Sum`, `Average` - 합계/평균 (int, long, float, double, decimal)
- `Min`, `Max` - 최소/최대
- `MinBy`, `MaxBy` - 키 기반 최소/최대

**변환**:
- `ToArray`, `ToList`, `ToDictionary`, `ToHashSet` - 컬렉션 변환
- `Cast`, `OfType` - 타입 변환
- `AsEnumerable` - 열거자 변환

**생성**:
- `Empty`, `Range`, `Repeat` - 시퀀스 생성
- `Append`, `Prepend` - 요소 추가
- `DefaultIfEmpty` - 기본값 처리

### ✅ Task Parallel Library (완전 구현)
- **Task / Task\<TResult\>** - 비동기 작업 표현
- **TaskFactory / TaskFactory\<TResult\>** - Task 생성 및 스케줄링
- **TaskStatus** - 작업 상태 열거형
- **TaskCreationOptions / TaskContinuationOptions** - 작업 옵션
- **Task.Run** - 백그라운드 작업 실행
- **Task.WhenAll / Task.WhenAny** - 복수 작업 대기
- **TaskEx** - Task 확장 메서드 (.NET 4.0 이하용)

### ✅ Parallel 클래스 (완전 구현)
- **Parallel.For** - 병렬 for 루프
- **Parallel.ForEach** - 병렬 foreach 루프
- **Parallel.Invoke** - 병렬 액션 실행
- **ParallelOptions** - 병렬 처리 옵션 (MaxDegreeOfParallelism, CancellationToken)
- **ParallelLoopState** - 루프 상태 제어 (Stop, Break)
- **ParallelLoopResult** - 루프 결과

### ✅ HttpClient (완전 구현)
- **HttpClient** - 비동기 HTTP 클라이언트
  - `GetAsync`, `PostAsync`, `PutAsync`, `DeleteAsync`
  - `GetStringAsync`, `GetByteArrayAsync`, `GetStreamAsync`
  - `SendAsync` - 사용자 정의 요청
- **HttpRequestMessage / HttpResponseMessage** - HTTP 메시지
- **HttpContent** 구현체:
  - `StringContent` - 문자열 콘텐츠
  - `ByteArrayContent` - 바이트 배열 콘텐츠
  - `StreamContent` - 스트림 콘텐츠
  - `FormUrlEncodedContent` - 폼 데이터
  - `MultipartContent` / `MultipartFormDataContent` - 멀티파트 콘텐츠
- **HttpMethod** - HTTP 메서드 (GET, POST, PUT, DELETE, PATCH 등)
- **HttpHeaders** - HTTP 헤더 관리
- **HttpClientHandler / HttpMessageHandler** - 메시지 핸들러
- **HttpRequestException** - HTTP 예외
- **SecurityProtocolType** - TLS 1.2/1.3 지원
- **ServicePointManagerEx** - 보안 프로토콜 설정 헬퍼

### ✅ Concurrent Collections (완전 구현)
- **ConcurrentQueue\<T\>** - 스레드 안전 FIFO 큐
  - `Enqueue`, `TryDequeue`, `TryPeek`
  - Lock-free 알고리즘, Segment-based 구조
- **ConcurrentStack\<T\>** - 스레드 안전 LIFO 스택
  - `Push`, `TryPop`, `TryPeek`
  - `PushRange`, `TryPopRange` - 배치 처리
- **ConcurrentBag\<T\>** - 스레드 안전 순서 없는 컬렉션
  - `Add`, `TryTake`, `TryPeek`
  - Thread-local storage, Work-stealing 메커니즘

### ✅ 스레딩 유틸리티 (완전 구현)
- **CancellationToken / CancellationTokenSource** - 작업 취소
- **CancellationTokenRegistration** - 취소 콜백
- **ManualResetEventSlim** - 경량 수동 리셋 이벤트
- **SemaphoreSlim** - 경량 세마포어
- **CountdownEvent** - 카운트다운 이벤트
- **SpinWait** - 스핀 대기

### ✅ async/await 지원 (완전 구현)
- **TaskAwaiter / TaskAwaiter\<TResult\>** - await 지원
- **AsyncTaskMethodBuilder** - async 메서드 빌더
- **IAsyncStateMachine** - 비동기 상태 머신
- **INotifyCompletion / ICriticalNotifyCompletion** - 완료 알림
- **AsyncMethodBuilderAttribute** - 비동기 메서드 빌더 속성

### ✅ 메모리 타입 (완전 구현)
- **Span\<T\>** - 스택 기반 메모리 슬라이스
- **ReadOnlySpan\<T\>** - 읽기 전용 스팬
- **Memory\<T\>** - 관리되는 메모리 슬라이스
- **ReadOnlyMemory\<T\>** - 읽기 전용 메모리
- **SpanAction\<T, TArg\>** - 스팬 액션 델리게이트

### ✅ 컴파일러 서비스 속성 (완전 구현)
- **CallerMemberNameAttribute** - 호출자 멤버 이름 (.NET 2.0, 3.5, 4.0용)
- **CallerFilePathAttribute** - 호출자 파일 경로 (.NET 2.0, 3.5, 4.0용)
- **CallerLineNumberAttribute** - 호출자 라인 번호 (.NET 2.0, 3.5, 4.0용)
- **ExtensionAttribute** - 확장 메서드 지원 (.NET 2.0용)
- **IsExternalInit** - record 및 init-only 속성 지원
- **RequiredMemberAttribute** - 필수 멤버 지정
- **SetsRequiredMembersAttribute** - 생성자에서 필수 멤버 설정
- **FormattableStringFactory** - 문자열 보간 지원

### ✅ 컬렉션 인터페이스 (완전 구현)
- **IReadOnlyCollection\<T\>** - 읽기 전용 컬렉션
- **IReadOnlyList\<T\>** - 읽기 전용 리스트
- **IReadOnlyDictionary\<TKey, TValue\>** - 읽기 전용 딕셔너리

### ✅ 문자열 확장 (완전 구현)
- **StringEx.IsNullOrWhiteSpace** - 공백 문자열 검사
- **StringEx.Join** - 문자열 결합 (다양한 오버로드)
- **StringEx.Concat** - 문자열 연결
- **StringEx.Contains** - 문자열 포함 검사 (StringComparison 지원)
- **StringEx.StartsWith / EndsWith** - 시작/끝 검사 (StringComparison 지원)
- **StringEx.Split** - 문자열 분할 (StringSplitOptions 지원)
- **StringEx.Replace** - 문자열 대체 (StringComparison 지원)
- **StringEx.GetHashCode** - 해시코드 (StringComparison 지원)
- **StringEx.Create** - 문자열 빌더 스타일 생성
- **StringEx.Trim / TrimStart / TrimEnd** - 문자열 트림

### ✅ 기타 유틸리티 타입 (완전 구현)
- **Lazy\<T\>** - 지연 초기화
- **Progress\<T\> / IProgress\<T\>** - 진행 상황 보고
- **AggregateException** - 집계 예외
- **HashCode** - 해시코드 생성 (FNV-1a 알고리즘)
- **FormattableString** - 문자열 보간

## 설치

```bash
# NuGet 패키지로 설치 (예정)
dotnet add package Jinobald.Polyfill
```

## 사용 예제

### Task 사용 (.NET 3.5에서도 가능)

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// Task.Run 사용
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
```

### Parallel 사용

```csharp
using System.Threading.Tasks;

// Parallel.For
Parallel.For(0, 100, i => {
    Console.WriteLine($"Processing {i}");
});

// Parallel.ForEach with options
var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };
Parallel.ForEach(items, options, item => {
    ProcessItem(item);
});

// Parallel.Invoke
Parallel.Invoke(
    () => Task1(),
    () => Task2(),
    () => Task3()
);
```

### HttpClient 사용 (.NET 3.5에서도 가능)

```csharp
using System.Net.Http;

// TLS 1.2 활성화 (레거시 프레임워크용)
ServicePointManagerEx.EnableModernTls();

using var client = new HttpClient();

// GET 요청
var response = await client.GetStringAsync("https://api.example.com/data");

// POST 요청
var content = new StringContent("{\"name\":\"test\"}", Encoding.UTF8, "application/json");
var result = await client.PostAsync("https://api.example.com/data", content);

// 멀티파트 폼 데이터
var form = new MultipartFormDataContent();
form.Add(new StringContent("value"), "field");
form.Add(new ByteArrayContent(fileBytes), "file", "filename.txt");
await client.PostAsync("https://api.example.com/upload", form);
```

### LINQ 사용 (.NET 3.5에서도 가능)

```csharp
using System.Linq;

var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// 필터링 및 투영
var evenSquares = numbers
    .Where(n => n % 2 == 0)
    .Select(n => n * n)
    .ToList();

// 그룹화
var grouped = numbers
    .GroupBy(n => n % 3)
    .ToDictionary(g => g.Key, g => g.ToList());

// 집계
var sum = numbers.Sum();
var average = numbers.Average();
var max = numbers.Max();

// 정렬
var sorted = numbers
    .OrderByDescending(n => n)
    .ThenBy(n => n % 2)
    .ToArray();
```

### ValueTuple 사용 (.NET 3.5에서도 가능)

```csharp
// 명명된 튜플
var person = (Name: "John", Age: 30);
Console.WriteLine($"{person.Name} is {person.Age} years old");

// 튜플 반환
(string name, int count) GetInfo() => ("Test", 42);
var info = GetInfo();
```

### Concurrent Collections 사용 (.NET 3.5에서도 가능)

```csharp
using System.Collections.Concurrent;

// ConcurrentQueue - FIFO 큐
var queue = new ConcurrentQueue<int>();
queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

if (queue.TryDequeue(out int item))
    Console.WriteLine($"Dequeued: {item}"); // 1

// ConcurrentStack - LIFO 스택
var stack = new ConcurrentStack<int>();
stack.Push(1);
stack.Push(2);
stack.PushRange(new[] { 3, 4, 5 });

if (stack.TryPop(out int value))
    Console.WriteLine($"Popped: {value}"); // 5

// ConcurrentBag - 순서 없는 컬렉션
var bag = new ConcurrentBag<int>();
Parallel.For(0, 100, i => bag.Add(i));

int count = bag.Count;
Console.WriteLine($"Bag contains {count} items");
```

### Caller Info 사용

```csharp
using System.Runtime.CompilerServices;

void Log(string message,
    [CallerMemberName] string memberName = "",
    [CallerFilePath] string filePath = "",
    [CallerLineNumber] int lineNumber = 0)
{
    Console.WriteLine($"[{memberName}] {message} ({filePath}:{lineNumber})");
}

Log("Hello"); // 자동으로 호출 위치 정보 포함
```

## 빌드

```bash
# 솔루션 빌드
dotnet build Jinobald.Polyfill.sln

# 테스트 실행
dotnet test

# 특정 프레임워크로 테스트
dotnet test --framework net8.0
dotnet test --framework net48
```

## 프로젝트 구조

```
Jinobald.Polyfill/
├── src/
│   └── Jinobald.Polyfill/           # 메인 라이브러리 (85개 소스 파일)
│       ├── Properties/
│       │   └── AssemblyInfo.cs      # InternalsVisibleTo 설정
│       └── System/                  # System 네임스페이스 확장
│           ├── Buffers/             # SpanAction 등
│           ├── Collections/         # 컬렉션 인터페이스
│           │   ├── Concurrent/      # Concurrent Collections (3개 파일)
│           │   └── Generic/         # IReadOnlyCollection 등
│           ├── Linq/                # LINQ 연산자 (10개 파일)
│           ├── Net/                 # 네트워킹
│           │   └── Http/            # HttpClient 관련 (14개 파일)
│           ├── Runtime/             # 런타임 관련
│           │   └── CompilerServices/ # 컴파일러 속성 (13개 파일)
│           └── Threading/           # 스레딩 관련
│               └── Tasks/           # Task, Parallel 등 (10개 파일)
├── tests/
│   └── Jinobald.Polyfill.Tests/     # 단위 테스트 (42개 파일)
│       └── System/
│           ├── Collections/Concurrent/ # Concurrent 테스트 (3개 파일)
│           ├── Linq/                # LINQ 테스트 (7개 파일)
│           ├── Net/Http/            # HttpClient 테스트 (6개 파일)
│           ├── Runtime/             # 컴파일러 속성 테스트
│           └── Threading/           # 스레딩 테스트
└── docs/
    └── TESTING_STRATEGY.md          # 테스트 전략 가이드
```

## 테스트 전략

이 라이브러리는 `InternalsVisibleTo` 속성을 사용하여 internal 타입 테스트를 지원합니다:

- **Public API**: 외부 테스트 프로젝트에서 직접 테스트
- **Internal 타입**: AssemblyInfo.cs에 InternalsVisibleTo 설정으로 테스트 프로젝트 접근 허용
- **컴파일러 전용 타입**: 리플렉션을 통한 존재 확인

자세한 내용은 [docs/TESTING_STRATEGY.md](docs/TESTING_STRATEGY.md)를 참조하세요.

## CI/CD

GitHub Actions를 통한 자동화된 빌드 및 테스트:

- **크로스 플랫폼 빌드**: Ubuntu, Windows, macOS
- **다중 프레임워크 테스트**: .NET 6.0 ~ .NET 10.0, .NET Framework 4.6.2 ~ 4.8.1
- **코드 커버리지**: Codecov 연동
- **NuGet 패키지 자동 배포**

자세한 내용은 [CI_CD_GUIDE.md](CI_CD_GUIDE.md)를 참조하세요.

## 기여

버그 리포트, 기능 제안, Pull Request를 환영합니다.

## 라이선스

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

## 저작권

Copyright (c) 2025 Jinho Park

## 최근 업데이트 (2025-12-21)

### 🚀 Concurrent Collections 구현 완료
.NET Framework 3.5 이상에서 스레드 안전 컬렉션을 사용할 수 있습니다:
- **ConcurrentQueue\<T\>**: Lock-free FIFO 큐, Segment-based 구조
- **ConcurrentStack\<T\>**: Lock-free LIFO 스택, PushRange/TryPopRange
- **ConcurrentBag\<T\>**: Thread-local storage + Work-stealing

### 🚀 HttpClient 구현 완료
.NET Framework 3.5 이상에서 현대적인 HttpClient API를 사용할 수 있습니다:
- 비동기 HTTP 요청 (GET, POST, PUT, DELETE)
- 다양한 콘텐츠 타입 지원 (String, ByteArray, Stream, Form, Multipart)
- TLS 1.2/1.3 지원 (SecurityProtocolType)

### 🚀 Parallel 클래스 구현 완료
.NET Framework 3.5에서 병렬 처리를 사용할 수 있습니다:
- Parallel.For / Parallel.ForEach
- ParallelOptions (MaxDegreeOfParallelism, CancellationToken)
- ParallelLoopState (Stop, Break)

### ✅ LINQ 완전 구현
모든 주요 LINQ 연산자 구현 완료:
- 기본 연산자 (Where, Select, First, Count 등)
- 정렬 (OrderBy, ThenBy)
- 그룹화 및 조인 (GroupBy, Join, GroupJoin)
- 집합 연산 (Union, Intersect, Except)
- 집계 (Sum, Average, Min, Max, Aggregate)

### 📊 현재 진행 상황
- **구현 완료**: 약 83개 타입 (전체의 약 60%)
- **Phase 1 (기초 인프라)**: 100% 완료
- **Phase 2 (핵심 기능)**: 100% 완료
- **Phase 3 (LINQ)**: 100% 완료
- **Phase 4 (동시성 컬렉션)**: 50% 완료
- **테스트 커버리지**: 525개 테스트 이상

### �� 상세 분석 보고서
전체 분석 결과 및 권장사항은 [POLYFILL_ANALYSIS_REPORT.md](POLYFILL_ANALYSIS_REPORT.md)를 참조하세요.

### 다음 단계
1. ConcurrentDictionary\<K,V\> 구현 (스레드 안전 딕셔너리)
2. BlockingCollection\<T\> 구현 (Producer-Consumer 패턴)
3. IAsyncEnumerable 지원 (async foreach)

## 참고

이 라이브러리의 일부 코드는 .NET Foundation의 코드를 기반으로 하며, MIT 라이선스 하에 배포됩니다.
