# Jinobald.Polyfill - 완전한 폴리필 라이브러리 구축 계획

## 🎯 프로젝트 목표

.NET Framework 2.0부터 최신 .NET 10.0까지 **완벽한 폴리필 라이브러리**를 구축하여:
- 모든 버전에서 최신 .NET 기능 사용 가능
- 완전한 테스트 커버리지 확보
- NuGet 패키지로 배포 가능한 프로덕션 품질 달성

---

## 📊 현재 구현 상태

### ✅ 이미 완료된 항목
- [x] Task / Task<TResult> (.NET 3.5용)
- [x] TaskFactory / TaskFactory<TResult>
- [x] TaskStatus, TaskCreationOptions, TaskContinuationOptions
- [x] CancellationToken / CancellationTokenSource / CancellationTokenRegistration
- [x] TaskAwaiter / TaskAwaiter<TResult> (.NET 3.5, 4.0용)
- [x] AsyncTaskMethodBuilder / AsyncTaskMethodBuilder<TResult>
- [x] INotifyCompletion / IAsyncStateMachine
- [x] AsyncMethodBuilderAttribute
- [x] Span<T> / ReadOnlySpan<T> (.NET 3.5-4.6용)
- [x] Memory<T> / ReadOnlyMemory<T> (.NET 3.5-4.6용)
- [x] SpanAction<T, TArg>
- [x] AggregateException
- [x] StringEx 클래스 (다양한 String 확장 메서드)

### 📝 구현 필요 항목 (우선순위별)

---

## 🔄 병렬 작업 그룹 (워크스페이스별 분담)

각 워크스페이스는 독립적으로 작업 가능하며, 테스트 코드 포함 완료를 목표로 합니다.

---

### **워크스페이스 1: 델리게이트 패밀리**
**목표**: Action, Func, Predicate 델리게이트 전체 구현

**타겟 프레임워크**: NET20, NET35, NET40

**구현 항목**:
1. **System/Action.cs**
   - `Action` (0개 파라미터) - NET20+
   - `Action<T>` - NET20+
   - `Action<T1, T2>` - NET20+
   - `Action<T1, T2, T3>` - NET20+
   - `Action<T1, T2, T3, T4>` - NET35+
   - `Action<T1~T5>` ~ `Action<T1~T16>` - NET40+

2. **System/Func.cs**
   - `Func<TResult>` - NET20+
   - `Func<T, TResult>` - NET20+
   - `Func<T1, T2, TResult>` - NET20+
   - `Func<T1, T2, T3, TResult>` - NET20+
   - `Func<T1, T2, T3, T4, TResult>` - NET35+
   - `Func<T1~T5, TResult>` ~ `Func<T1~T16, TResult>` - NET40+

3. **System/Predicate.cs**
   - `Predicate<T>` - NET20+

4. **System/Comparison.cs**
   - `Comparison<T>` - NET20+

5. **System/Converter.cs**
   - `Converter<TInput, TOutput>` - NET20+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/ActionTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/FuncTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/PredicateTests.cs`

**테스트 범위**:
- 각 델리게이트 타입별 호출 테스트
- 파라미터 전달 검증
- 반환값 검증
- Null 참조 처리

**예상 작업량**: 🟢 소형 (패턴 반복, 코드 생성 스타일)

**난이도**: 🟢 하

---

### **워크스페이스 2: Tuple & ValueTuple**
**목표**: Tuple, ValueTuple 타입 전체 구현

**타겟 프레임워크**: NET35 (Tuple), NET40 (ValueTuple 백포트)

**구현 항목**:
1. **System/Tuple.cs**
   - `Tuple<T1>` ~ `Tuple<T1~T8>` - NET35+
   - `Tuple.Create()` 정적 팩토리 메서드 (모든 오버로드)

2. **System/ValueTuple.cs**
   - `ValueTuple` (0개 요소)
   - `ValueTuple<T1>` ~ `ValueTuple<T1~T8>` - NET40+
   - `ValueTuple.Create()` 정적 팩토리 메서드
   - 비교 연산자 구현 (==, !=, CompareTo)
   - GetHashCode() 구현
   - ToString() 구현

3. **System/ITuple.cs**
   - `ITuple` 인터페이스 - NET40+

4. **System/TupleExtensions.cs**
   - Deconstruct() 확장 메서드
   - ToTuple() / ToValueTuple() 변환 메서드

5. **System.Runtime.CompilerServices/TupleElementNamesAttribute.cs**
   - 튜플 이름 메타데이터 속성 - NET40+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/TupleTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/ValueTupleTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/TupleExtensionsTests.cs`

**테스트 범위**:
- 생성 및 초기화
- 항목 접근 (Item1, Item2, ...)
- 동등성 비교
- 구조적 비교
- 해시코드 생성
- ToString() 출력
- Deconstruction

**예상 작업량**: 🟡 중형

**난이도**: 🟢 하

---

### **워크스페이스 3: Lazy<T> & 스레딩 유틸리티**
**목표**: 지연 초기화 및 경량 스레딩 프리미티브

**타겟 프레임워크**: NET35, NET40

**구현 항목**:
1. **System/Lazy.cs**
   - `Lazy<T>` - NET35+
   - `LazyThreadSafetyMode` enum
   - 생성자 오버로드 (factory, isThreadSafe, mode)
   - Value 속성 (지연 초기화)
   - IsValueCreated 속성

2. **System.Threading/ManualResetEventSlim.cs**
   - `ManualResetEventSlim` - NET35+
   - Set(), Reset(), Wait() 메서드
   - SpinWait 기반 최적화

3. **System.Threading/SemaphoreSlim.cs**
   - `SemaphoreSlim` - NET35+
   - Wait(), WaitAsync(), Release() 메서드
   - CurrentCount 속성

4. **System.Threading/CountdownEvent.cs**
   - `CountdownEvent` - NET35+
   - Signal(), Wait(), Reset() 메서드
   - CurrentCount, InitialCount 속성

5. **System.Threading/Barrier.cs**
   - `Barrier` - NET35+
   - SignalAndWait() 메서드
   - ParticipantCount, ParticipantsRemaining 속성

6. **System.Threading/SpinWait.cs**
   - `SpinWait` struct - NET35+
   - SpinOnce(), SpinUntil() 메서드

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/LazyTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Threading/ManualResetEventSlimTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Threading/SemaphoreSlimTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Threading/CountdownEventTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Threading/BarrierTests.cs`

**테스트 범위**:
- Lazy: 단일/다중 스레드 초기화, 예외 캐싱
- 스레딩: 동시성 테스트, 데드락 방지, 타임아웃 처리
- 성능: SpinWait 효율성

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 4: 동시성 컬렉션 (Part 1)**
**목표**: 스레드 안전 컬렉션 구현

**타겟 프레임워크**: NET35, NET40

**구현 항목**:
1. **System.Collections.Concurrent/ConcurrentQueue.cs**
   - `ConcurrentQueue<T>` - NET35+
   - Enqueue(), TryDequeue(), TryPeek()
   - Lock-free 알고리즘 구현

2. **System.Collections.Concurrent/ConcurrentStack.cs**
   - `ConcurrentStack<T>` - NET35+
   - Push(), TryPop(), TryPeek()
   - PushRange(), TryPopRange()

3. **System.Collections.Concurrent/ConcurrentBag.cs**
   - `ConcurrentBag<T>` - NET35+
   - Add(), TryTake(), TryPeek()
   - Thread-local storage 기반 구현

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Collections/Concurrent/ConcurrentQueueTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Collections/Concurrent/ConcurrentStackTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Collections/Concurrent/ConcurrentBagTests.cs`

**테스트 범위**:
- 단일 스레드 기본 동작
- 다중 스레드 동시성 (Producer-Consumer 패턴)
- 경쟁 조건(Race Condition) 테스트
- 메모리 안정성

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

---

### **워크스페이스 5: 동시성 컬렉션 (Part 2)**
**목표**: ConcurrentDictionary 및 BlockingCollection

**타겟 프레임워크**: NET35, NET40

**구현 항목**:
1. **System.Collections.Concurrent/ConcurrentDictionary.cs**
   - `ConcurrentDictionary<TKey, TValue>` - NET35+
   - TryAdd(), TryUpdate(), TryRemove(), TryGetValue()
   - AddOrUpdate(), GetOrAdd()
   - 세밀한 락 전략 (lock striping)

2. **System.Collections.Concurrent/BlockingCollection.cs**
   - `BlockingCollection<T>` - NET35+
   - Add(), Take(), TryAdd(), TryTake()
   - CompleteAdding(), IsCompleted
   - GetConsumingEnumerable()

3. **System.Collections.Concurrent/IProducerConsumerCollection.cs**
   - `IProducerConsumerCollection<T>` 인터페이스

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Collections/Concurrent/ConcurrentDictionaryTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Collections/Concurrent/BlockingCollectionTests.cs`

**테스트 범위**:
- ConcurrentDictionary: 동시 읽기/쓰기, AddOrUpdate 원자성
- BlockingCollection: Producer-Consumer 시나리오, 바운드/언바운드
- 데드락 방지, 취소 토큰 지원

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

---

### **워크스페이스 6: Progress & ReadOnly Collections**
**목표**: 진행 상황 보고 및 읽기 전용 컬렉션

**타겟 프레임워크**: NET40, NET45

**구현 항목**:
1. **System/IProgress.cs**
   - `IProgress<T>` 인터페이스 - NET40+

2. **System/Progress.cs**
   - `Progress<T>` 클래스 - NET40+
   - Report() 메서드
   - ProgressChanged 이벤트
   - SynchronizationContext 캡처

3. **System.Collections.Generic/IReadOnlyCollection.cs**
   - `IReadOnlyCollection<T>` 인터페이스 - NET40+

4. **System.Collections.Generic/IReadOnlyList.cs**
   - `IReadOnlyList<T>` 인터페이스 - NET40+

5. **System.Collections.Generic/IReadOnlyDictionary.cs**
   - `IReadOnlyDictionary<TKey, TValue>` 인터페이스 - NET40+

6. **System.Collections.ObjectModel/ReadOnlyDictionary.cs**
   - `ReadOnlyDictionary<TKey, TValue>` 클래스 - NET40+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/ProgressTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Collections/Generic/ReadOnlyCollectionsTests.cs`

**테스트 범위**:
- Progress: SynchronizationContext 캡처, UI 스레드 마샬링
- ReadOnly Collections: 읽기 작업, 수정 시도 시 예외

**예상 작업량**: 🟡 중형

**난이도**: 🟢 하

---

### **워크스페이스 7: Caller Info & Compiler Attributes**
**목표**: 컴파일러 서비스 속성들

**타겟 프레임워크**: NET40, NET45, NET46, NET47

**구현 항목**:
1. **System.Runtime.CompilerServices/CallerMemberNameAttribute.cs**
   - `CallerMemberNameAttribute` - NET40+

2. **System.Runtime.CompilerServices/CallerFilePathAttribute.cs**
   - `CallerFilePathAttribute` - NET40+

3. **System.Runtime.CompilerServices/CallerLineNumberAttribute.cs**
   - `CallerLineNumberAttribute` - NET40+

4. **System.Runtime.CompilerServices/CallerArgumentExpressionAttribute.cs**
   - `CallerArgumentExpressionAttribute` - NET46+

5. **System.Runtime.CompilerServices/ExtensionAttribute.cs**
   - `ExtensionAttribute` - NET20+ (확장 메서드 지원)

6. **System.Runtime.CompilerServices/IsExternalInit.cs**
   - `IsExternalInit` 클래스 - NET46+ (record, init 지원)

7. **System.Runtime.CompilerServices/RequiredMemberAttribute.cs**
   - `RequiredMemberAttribute` - NET47+

8. **System.Runtime.CompilerServices/SetsRequiredMembersAttribute.cs**
   - `SetsRequiredMembersAttribute` - NET47+

9. **System.Runtime.CompilerServices/CompilerFeatureRequiredAttribute.cs**
   - `CompilerFeatureRequiredAttribute` - NET47+

10. **System.Runtime.CompilerServices/MethodImplAttribute.cs**
    - `MethodImplOptions.AggressiveInlining` 추가 - NET40+

11. **System.Diagnostics.CodeAnalysis/StringSyntaxAttribute.cs**
    - `StringSyntaxAttribute` - NET47+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Runtime/CompilerServices/CallerInfoTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Runtime/CompilerServices/CompilerAttributesTests.cs`

**테스트 범위**:
- Caller Info: 실제 컴파일러 동작 검증 (메서드명, 파일경로, 라인번호)
- 속성 존재 확인 (컴파일러가 인식하는지)

**예상 작업량**: 🟢 소형

**난이도**: 🟢 하

---

### **워크스페이스 8: Index & Range (C# 8.0 지원)**
**목표**: 인덱스/범위 연산자 지원

**타겟 프레임워크**: NET46, NET47, NET48

**구현 항목**:
1. **System/Index.cs**
   - `Index` struct - NET46+
   - FromStart(), FromEnd()
   - ^ 연산자 지원
   - GetOffset() 메서드

2. **System/Range.cs**
   - `Range` struct - NET46+
   - StartAt(), EndAt(), All()
   - .. 연산자 지원
   - GetOffsetAndLength() 메서드

3. **System.Runtime.CompilerServices/RuntimeHelpers.cs** (확장)
   - GetSubArray() 메서드 추가

4. **배열/컬렉션 확장**:
   - `ArrayExtensions.cs`: array[index], array[range] 지원
   - `StringExtensions.cs`: string[index], string[range] 지원
   - `Span<T>` / `ReadOnlySpan<T>` 인덱서 지원

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/IndexTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/RangeTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/IndexRangeIntegrationTests.cs`

**테스트 범위**:
- Index: FromStart, FromEnd, ^ 연산자
- Range: .., StartAt, EndAt
- 배열/문자열 슬라이싱
- 경계값 테스트

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 9: HashCode & FormattableString**
**목표**: 해시코드 생성 및 문자열 보간 지원

**타겟 프레임워크**: NET45, NET46

**구현 항목**:
1. **System/HashCode.cs**
   - `HashCode` struct - NET45+
   - Add<T>() 메서드
   - Combine() 정적 메서드 (1~8개 값)
   - ToHashCode() 메서드
   - xxHash32 알고리즘 기반

2. **System/FormattableString.cs**
   - `FormattableString` 추상 클래스 - NET45+
   - Format, ArgumentCount, GetArguments() 속성/메서드
   - ToString(IFormatProvider) 메서드
   - Invariant() 정적 메서드

3. **System.Runtime.CompilerServices/FormattableStringFactory.cs**
   - `FormattableStringFactory` 클래스 - NET45+
   - Create() 정적 메서드

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/HashCodeTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/FormattableStringTests.cs`

**테스트 범위**:
- HashCode: Combine 일관성, 분산 품질
- FormattableString: 문화권별 포맷팅, 인자 추출

**예상 작업량**: 🟢 소형

**난이도**: 🟢 하

---

### **워크스페이스 10: IAsyncEnumerable & IAsyncDisposable**
**목표**: C# 8.0 비동기 스트림 지원

**타겟 프레임워크**: NET46, NET47, NET48

**구현 항목**:
1. **System.Collections.Generic/IAsyncEnumerable.cs**
   - `IAsyncEnumerable<T>` 인터페이스 - NET46+

2. **System.Collections.Generic/IAsyncEnumerator.cs**
   - `IAsyncEnumerator<T>` 인터페이스 - NET46+

3. **System/IAsyncDisposable.cs**
   - `IAsyncDisposable` 인터페이스 - NET46+

4. **System.Runtime.CompilerServices/AsyncIteratorMethodBuilder.cs**
   - `AsyncIteratorMethodBuilder` - NET46+

5. **System.Runtime.CompilerServices/AsyncIteratorStateMachineAttribute.cs**
   - `AsyncIteratorStateMachineAttribute` - NET46+

6. **System.Threading.Tasks/ValueTask.cs**
   - `ValueTask` struct - NET46+
   - `ValueTask<T>` struct - NET46+

7. **System.Runtime.CompilerServices/ConfiguredAsyncDisposable.cs**
   - `ConfiguredAsyncDisposable` - NET46+

8. **System.Runtime.CompilerServices/ConfiguredCancelableAsyncEnumerable.cs**
   - `ConfiguredCancelableAsyncEnumerable<T>` - NET46+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Collections/Generic/IAsyncEnumerableTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/IAsyncDisposableTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Threading/Tasks/ValueTaskTests.cs`

**테스트 범위**:
- async foreach 동작
- await using 동작
- 취소 토큰 전파
- ValueTask 최적화 경로

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

---

### **워크스페이스 11: LINQ (Part 1 - 기본 연산자)**
**목표**: LINQ to Objects 핵심 연산자

**타겟 프레임워크**: NET20

**구현 항목**:
1. **System.Linq/Enumerable.cs** (Part 1)
   - `Where<T>()` - 필터링
   - `Select<T, TResult>()` - 투영
   - `SelectMany<T, TResult>()` - 평탄화 투영
   - `First<T>()`, `FirstOrDefault<T>()`
   - `Last<T>()`, `LastOrDefault<T>()`
   - `Single<T>()`, `SingleOrDefault<T>()`
   - `Any<T>()` - 존재 확인
   - `All<T>()` - 전체 조건 확인
   - `Contains<T>()` - 요소 포함 확인
   - `Count<T>()`, `LongCount<T>()`
   - `ElementAt<T>()`, `ElementAtOrDefault<T>()`

2. **System.Linq/IQueryable.cs** (기본 인터페이스만)
   - `IQueryable`, `IQueryable<T>` - NET20+
   - `IQueryProvider` - NET20+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableBasicTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableFilteringTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableProjectionTests.cs`

**테스트 범위**:
- 각 연산자별 기본 동작
- 빈 시퀀스 처리
- Null 인자 검증
- 지연 실행 (Deferred Execution)
- 체이닝 테스트

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 12: LINQ (Part 2 - 정렬/그룹화/집합)**
**목표**: LINQ 고급 연산자

**타겟 프레임워크**: NET20

**구현 항목**:
1. **System.Linq/Enumerable.cs** (Part 2)
   - `OrderBy<T, TKey>()`, `OrderByDescending<T, TKey>()`
   - `ThenBy<T, TKey>()`, `ThenByDescending<T, TKey>()`
   - `GroupBy<T, TKey>()`, `GroupBy<T, TKey, TElement>()`
   - `Join<T, TInner, TKey, TResult>()`
   - `GroupJoin<T, TInner, TKey, TResult>()`
   - `Distinct<T>()`
   - `Union<T>()`, `Intersect<T>()`, `Except<T>()`
   - `Concat<T>()`
   - `Zip<T1, T2, TResult>()`
   - `Skip<T>()`, `Take<T>()`
   - `SkipWhile<T>()`, `TakeWhile<T>()`
   - `Reverse<T>()`
   - `Cast<TResult>()`, `OfType<TResult>()`
   - `ToArray<T>()`, `ToList<T>()`, `ToDictionary<T, TKey>()`
   - `ToLookup<T, TKey>()`

2. **System.Linq/IGrouping.cs**
   - `IGrouping<TKey, TElement>` 인터페이스

3. **System.Linq/IOrderedEnumerable.cs**
   - `IOrderedEnumerable<T>` 인터페이스

4. **System.Linq/Lookup.cs**
   - `Lookup<TKey, TElement>` 클래스

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableSortingTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableGroupingTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableSetTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableJoinTests.cs`

**테스트 범위**:
- 정렬 안정성 (stable sort)
- 그룹화 키 동등성
- 집합 연산 중복 제거
- Join 성능 및 정확성
- Lookup 동작

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

---

### **워크스페이스 13: LINQ (Part 3 - 집계/변환)**
**목표**: LINQ 집계 및 변환 연산자

**타겟 프레임워크**: NET20

**구현 항목**:
1. **System.Linq/Enumerable.cs** (Part 3)
   - `Aggregate<T>()` - 사용자 정의 집계
   - `Sum()` - 숫자 타입별 오버로드
   - `Average()` - 숫자 타입별 오버로드
   - `Min<T>()`, `Max<T>()`
   - `MinBy<T, TKey>()`, `MaxBy<T, TKey>()` (최신 .NET용)
   - `DefaultIfEmpty<T>()`
   - `Empty<T>()`
   - `Range()`, `Repeat<T>()`
   - `SequenceEqual<T>()`
   - `Append<T>()`, `Prepend<T>()` (최신 .NET용)
   - `Chunk<T>()` (최신 .NET용)

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableAggregateTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableGenerationTests.cs`

**테스트 범위**:
- 집계 연산 정확성
- 오버플로우 처리
- 빈 시퀀스 동작
- 생성 연산자 범위 테스트

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 14: 컬렉션 확장 (HashSet, SortedSet 등)**
**목표**: .NET 2.0/3.5용 추가 컬렉션

**타겟 프레임워크**: NET20, NET35

**구현 항목**:
1. **System.Collections.Generic/HashSet.cs**
   - `HashSet<T>` - NET20+ (NET35에 네이티브)
   - Add(), Remove(), Contains()
   - UnionWith(), IntersectWith(), ExceptWith()
   - IsSubsetOf(), IsSupersetOf()
   - SetEquals()

2. **System.Collections.Generic/SortedSet.cs**
   - `SortedSet<T>` - NET35+
   - 정렬된 집합 연산

3. **System.Collections.ObjectModel/Collection.cs** (확장)
   - `Collection<T>` 추가 메서드

4. **System.Collections.ObjectModel/ObservableCollection.cs**
   - `ObservableCollection<T>` - NET35+ (WPF/Silverlight)
   - CollectionChanged 이벤트
   - INotifyCollectionChanged 구현

5. **System.Collections.Specialized/INotifyCollectionChanged.cs**
   - `INotifyCollectionChanged` 인터페이스
   - `NotifyCollectionChangedEventArgs`
   - `NotifyCollectionChangedAction` enum

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Collections/Generic/HashSetTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Collections/ObjectModel/ObservableCollectionTests.cs`

**테스트 범위**:
- HashSet 집합 연산
- ObservableCollection 이벤트 발생
- 동등성 비교자 커스터마이징

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 15: BigInteger & Numerics**
**목표**: 임의 정밀도 정수 및 복소수

**타겟 프레임워크**: NET35

**구현 항목**:
1. **System.Numerics/BigInteger.cs**
   - `BigInteger` struct - NET35+
   - 산술 연산자 (+, -, *, /, %)
   - 비교 연산자
   - Parse(), TryParse()
   - ToString() 오버로드
   - Pow(), ModPow(), GreatestCommonDivisor()

2. **System.Numerics/Complex.cs**
   - `Complex` struct - NET35+
   - Real, Imaginary 속성
   - 연산자 오버로딩
   - Abs(), Phase(), Conjugate()

3. **System.Numerics/Vector2.cs** (선택적)
   - `Vector2`, `Vector3`, `Vector4` - NET46+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Numerics/BigIntegerTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Numerics/ComplexTests.cs`

**테스트 범위**:
- BigInteger: 큰 숫자 연산, 오버플로우 없음
- Complex: 복소수 수학 연산
- 파싱 및 문자열 변환

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

**참고**: BigInteger는 복잡도가 높으므로 NuGet 패키지 사용 고려 가능

---

### **워크스페이스 16: ConfiguredTaskAwaitable & Async 확장**
**목표**: Task 고급 기능 및 ConfigureAwait

**타겟 프레임워크**: NET40, NET45

**구현 항목**:
1. **System.Runtime.CompilerServices/ConfiguredTaskAwaitable.cs**
   - `ConfiguredTaskAwaitable` struct - NET40+
   - `ConfiguredTaskAwaitable<T>` struct

2. **System.Runtime.CompilerServices/ConfiguredTaskAwaiter.cs**
   - `ConfiguredTaskAwaiter` - NET40+
   - `ConfiguredTaskAwaiter<T>`

3. **System.Threading.Tasks/TaskExtensions.cs**
   - `Unwrap()` 확장 메서드 - Task<Task<T>> 언래핑
   - `ConfigureAwait(bool)` 확장 메서드

4. **System.Runtime.CompilerServices/ICriticalNotifyCompletion.cs**
   - `ICriticalNotifyCompletion` 인터페이스 - NET40+

5. **System.Runtime.CompilerServices/YieldAwaitable.cs**
   - `YieldAwaitable` struct - Task.Yield() 지원

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Threading/Tasks/ConfigureAwaitTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Threading/Tasks/TaskExtensionsTests.cs`

**테스트 범위**:
- ConfigureAwait(false) SynchronizationContext 무시
- Unwrap() 중첩 Task 처리
- Task.Yield() 스케줄링

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 16-A: HttpClient & HTTP 관련 (추가)**
**목표**: HttpClient 및 HTTP 관련 폴리필

**타겟 프레임워크**: NET35, NET40, NET45

**구현 항목**:
1. **System.Net.Http/HttpClient.cs**
   - `HttpClient` 클래스 - NET35+
   - 비동기 HTTP 요청 지원
   - GET, POST, PUT, DELETE 메서드
   - 헤더 및 콘텐츠 관리

2. **System.Net.Http/HttpRequestMessage.cs**
   - `HttpRequestMessage` - 요청 메시지
   - Method, RequestUri, Headers, Content

3. **System.Net.Http/HttpResponseMessage.cs**
   - `HttpResponseMessage` - 응답 메시지
   - StatusCode, Headers, Content
   - EnsureSuccessStatusCode()

4. **System.Net.Http/HttpContent.cs**
   - `HttpContent` 추상 클래스
   - `StringContent`, `ByteArrayContent`
   - `FormUrlEncodedContent`, `StreamContent`
   - `MultipartFormDataContent`

5. **System.Net.Http/HttpMethod.cs**
   - `HttpMethod` 클래스 (GET, POST, PUT, DELETE 등)

6. **System.Net.Http.Headers/HttpHeaders.cs**
   - `HttpHeaders` 기본 클래스
   - `HttpRequestHeaders`, `HttpResponseHeaders`
   - `HttpContentHeaders`

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Net/Http/HttpClientTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Net/Http/HttpContentTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Net/Http/HttpMessageTests.cs`

**테스트 범위**:
- GET/POST 요청 테스트 (모의 서버)
- 비동기 요청 처리
- 헤더 관리
- 콘텐츠 직렬화/역직렬화
- 에러 처리 및 타임아웃

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

**참고**: .NET 3.5/4.0에서는 WebRequest를 기반으로 구현

---

### **워크스페이스 16-B: JSON 직렬화 (추가)**
**목표**: JSON 직렬화/역직렬화 지원

**타겟 프레임워크**: NET20, NET35, NET40

**구현 항목**:
1. **System.Text.Json/JsonSerializer.cs**
   - `JsonSerializer` 정적 클래스 - NET20+
   - Serialize<T>() 메서드
   - Deserialize<T>() 메서드
   - SerializeAsync/DeserializeAsync

2. **System.Text.Json/JsonSerializerOptions.cs**
   - `JsonSerializerOptions` - 직렬화 옵션
   - PropertyNamingPolicy (camelCase 등)
   - WriteIndented (포맷팅)
   - DefaultIgnoreCondition

3. **System.Text.Json.Serialization/JsonPropertyNameAttribute.cs**
   - 프로퍼티 이름 매핑

4. **System.Text.Json.Serialization/JsonIgnoreAttribute.cs**
   - 프로퍼티 무시

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Text/Json/JsonSerializerTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/Text/Json/JsonSerializerOptionsTests.cs`

**테스트 범위**:
- 기본 타입 직렬화/역직렬화
- 복잡한 객체 그래프
- 컬렉션 및 배열
- 커스텀 네이밍 정책
- Null 처리

**예상 작업량**: 🔴 대형

**난이도**: 🔴 상

**참고**:
- .NET 2.0-4.5용은 간단한 JSON 파서 직접 구현 또는 Newtonsoft.Json 래퍼
- System.Text.Json API와 호환되는 인터페이스 제공

---

### **워크스페이스 16-C: 추가 유틸리티 타입 (추가)**
**목표**: 자주 사용되는 유틸리티 타입들

**타겟 프레임워크**: NET35, NET40, NET45

**구현 항목**:
1. **System/DateOnly.cs**
   - `DateOnly` struct - NET45+
   - 날짜만 표현 (시간 없음)
   - Parse, TryParse, ToString

2. **System/TimeOnly.cs**
   - `TimeOnly` struct - NET45+
   - 시간만 표현 (날짜 없음)

3. **System/Half.cs**
   - `Half` struct (16비트 부동소수점) - NET45+
   - IEEE 754 표준

4. **System.Diagnostics/UnreachableException.cs**
   - `UnreachableException` - NET47+
   - 도달할 수 없는 코드 표시

5. **System.Diagnostics.CodeAnalysis/NotNullWhenAttribute.cs**
   - Nullable 참조 타입 분석 속성들
   - `MaybeNullAttribute`, `NotNullAttribute`
   - `MemberNotNullAttribute`, `DoesNotReturnAttribute`

6. **System/Environment.ProcessPath.cs**
   - `Environment.ProcessPath` 프로퍼티 - NET47+

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/DateOnlyTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/TimeOnlyTests.cs`
- `tests/Jinobald.Polyfill.Tests/System/HalfTests.cs`

**테스트 범위**:
- DateOnly/TimeOnly 연산
- Half 정밀도 테스트
- 속성 존재 확인

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 16-D: 추가 LINQ 메서드 (.NET 6+)**
**목표**: 최신 .NET의 LINQ 메서드들

**타겟 프레임워크**: NET45, NET46, NET47, NET48

**구현 항목**:
1. **System.Linq/Enumerable.cs** (최신 메서드)
   - `Chunk<T>()` - 청크로 분할 (.NET 6+)
   - `DistinctBy<T, TKey>()` - 키 기반 중복 제거 (.NET 6+)
   - `ExceptBy<T, TKey>()` - 키 기반 차집합 (.NET 6+)
   - `IntersectBy<T, TKey>()` - 키 기반 교집합 (.NET 6+)
   - `UnionBy<T, TKey>()` - 키 기반 합집합 (.NET 6+)
   - `MinBy<T, TKey>()`, `MaxBy<T, TKey>()` - 키 기반 최소/최대 (.NET 6+)
   - `Index<T>()` - 인덱스와 함께 열거 (.NET 9+)
   - `CountBy<T, TKey>()` - 키별 개수 (.NET 9+)
   - `AggregateBy<T, TKey>()` - 키별 집계 (.NET 9+)

2. **System.Linq/Queryable.cs** (최신 메서드)
   - 위 메서드들의 IQueryable 버전

**테스트 파일**:
- `tests/Jinobald.Polyfill.Tests/System/Linq/EnumerableModernTests.cs`

**테스트 범위**:
- 각 메서드 기본 동작
- 키 선택자 동작
- 빈 시퀀스 처리

**예상 작업량**: 🟡 중형

**난이도**: 🟡 중

---

### **워크스페이스 17: 프로젝트 구성 & 빌드 설정**
**목표**: 타겟 프레임워크 추가 및 조건부 컴파일

**작업 항목**:
1. **Jinobald.Polyfill.csproj 수정**
   - `net20` 타겟 추가 (가능하면)
   - 조건부 컴파일 심볼 정의:
     ```xml
     <PropertyGroup Condition="'$(TargetFramework)' == 'net20'">
       <DefineConstants>$(DefineConstants);NET20</DefineConstants>
     </PropertyGroup>
     <PropertyGroup Condition="'$(TargetFramework)' == 'net35'">
       <DefineConstants>$(DefineConstants);NET35</DefineConstants>
     </PropertyGroup>
     <!-- ...추가 프레임워크별... -->
     ```
   - NuGet 패키지 참조 조건 설정
   - LangVersion 설정 (프레임워크별)

1-1. **AssemblyInfo.cs 추가**
   - `InternalsVisibleTo` 속성 추가하여 테스트 프로젝트가 internal 타입에 접근 가능하도록 설정
   - 외부 테스트 프로젝트와 내부 테스트 모두 지원

2. **Global.cs** (또는 유사 파일)
   - 전역 using 정의 (NET6.0+)
   - 조건부 컴파일 헬퍼

3. **.editorconfig**
   - 코드 스타일 규칙 정의
   - Nullable 참조 타입 설정

4. **Directory.Build.props**
   - 공통 빌드 속성
   - 버전 관리

5. **Directory.Build.targets**
   - 공통 빌드 타겟

6. **CI/CD 설정**
   - GitHub Actions 워크플로우 (.github/workflows/build.yml)
   - 모든 타겟 프레임워크 빌드 검증
   - 테스트 실행 (프레임워크별)
   - 코드 커버리지 측정

**테스트 파일**:
- 각 타겟 프레임워크별 빌드 검증

**예상 작업량**: 🟡 중형

**난이도**: 🟢 하

---

### **워크스페이스 18: 통합 테스트 & 문서화**
**목표**: 전체 라이브러리 통합 검증 및 문서 작성

**작업 항목**:
1. **통합 테스트 프로젝트**
   - `tests/Jinobald.Polyfill.IntegrationTests/` 생성
   - 크로스 프레임워크 호환성 테스트
   - 실제 사용 시나리오 테스트 (E2E)

2. **README.md 업데이트**
   - 지원 타겟 프레임워크 업데이트
   - 새로운 기능 목록 추가
   - 사용 예제 추가
   - 설치 가이드

3. **API 문서**
   - XML 문서 주석 (모든 public API)
   - DocFX 또는 Sandcastle 설정
   - GitHub Pages 배포

4. **마이그레이션 가이드**
   - `MIGRATION_GUIDE.md`
   - 버전별 마이그레이션 방법
   - Breaking changes 정리

5. **CHANGELOG.md**
   - 버전별 변경사항 기록

6. **라이선스 검토**
   - MIT 라이선스 확인
   - 써드파티 코드 라이선스 명시

7. **NuGet 패키징**
   - .nuspec 또는 csproj 메타데이터 설정
   - 패키지 아이콘, README 포함
   - 심볼 패키지 (.snupkg) 생성

**예상 작업량**: 🟡 중형

**난이도**: 🟢 하

---

## 📅 작업 우선순위 및 의존성

### **Phase 1: 기초 인프라 (병렬 실행 가능)**
가장 먼저 시작해야 하며, 다른 작업의 기반이 됩니다.

- ✅ **워크스페이스 17**: 프로젝트 구성 & 빌드 설정 (가장 먼저 실행, AssemblyInfo 포함)
- ✅ **워크스페이스 1**: 델리게이트 패밀리 (LINQ 등에 필요)
- ✅ **워크스페이스 2**: Tuple & ValueTuple (독립적)
- ✅ **워크스페이스 7**: Caller Info & Compiler Attributes (독립적) ✅ **완료**

**완료 상태**: 4/4 (100%)
**실제 소요**: 1일 (2025-12-21)

---

### **Phase 2: 핵심 기능 (Phase 1 완료 후, 병렬 실행 가능)**

- ✅ **워크스페이스 3**: Lazy<T> & 스레딩 유틸리티
- ✅ **워크스페이스 6**: Progress & ReadOnly Collections
- ✅ **워크스페이스 9**: HashCode & FormattableString
- ✅ **워크스페이스 8**: Index & Range
- ✅ **워크스페이스 14**: 컬렉션 확장 (HashSet, ObservableCollection)

**예상 기간**: 2주

---

### **Phase 3: LINQ 구현 (Phase 1 완료 후, 순차/병렬 혼합)**

LINQ는 델리게이트에 의존하므로 워크스페이스 1 완료 후 시작.

- ✅ **워크스페이스 11**: LINQ Part 1 - 기본 연산자 ✅ **완료** (2025-12-21)
- ⏸️ **워크스페이스 12**: LINQ Part 2 - 정렬/그룹화 (Part 1과 병렬 가능)
- ⏸️ **워크스페이스 13**: LINQ Part 3 - 집계/변환 (Part 1, 2 완료 후)

**완료 상태**: 1/3 (33%)
**예상 기간**: 2-3주

---

### **Phase 4: 동시성 라이브러리 (Phase 2 완료 후, 병렬 실행 가능)**

복잡도가 높으므로 집중 필요.

- ✅ **워크스페이스 4**: 동시성 컬렉션 Part 1 (ConcurrentQueue, Stack, Bag)
- ✅ **워크스페이스 5**: 동시성 컬렉션 Part 2 (ConcurrentDictionary, BlockingCollection)

**예상 기간**: 2-3주

---

### **Phase 5: 고급 기능 (Phase 2-4와 병렬 가능)**

- ✅ **워크스페이스 16**: ConfiguredTaskAwaitable & Async 확장
- ✅ **워크스페이스 10**: IAsyncEnumerable & IAsyncDisposable
- ⏸️ **워크스페이스 15**: BigInteger & Numerics (선택적, 복잡도 높음)

**예상 기간**: 2-3주

---

### **Phase 6: 실용적 확장 (선택적, 병렬 실행 가능)**

최신 .NET API 호환성을 위한 추가 기능들

- ⏸️ **워크스페이스 16-A**: HttpClient & HTTP 관련
- ⏸️ **워크스페이스 16-B**: JSON 직렬화
- ⏸️ **워크스페이스 16-C**: 추가 유틸리티 타입 (DateOnly, TimeOnly, Half 등)
- ⏸️ **워크스페이스 16-D**: 최신 LINQ 메서드 (.NET 6~9)

**예상 기간**: 3-4주

**우선순위**: 중간 (실제 프로젝트 수요에 따라 결정)

---

### **Phase 7: 통합 및 배포 (모든 Phase 완료 후)**

- ✅ **워크스페이스 18**: 통합 테스트 & 문서화

**예상 기간**: 1-2주

---

## 🧪 테스트 전략

### **단위 테스트 (Unit Tests)**
- 각 워크스페이스마다 독립적인 테스트 파일 작성
- xUnit 프레임워크 사용
- 테스트 커버리지 80% 이상 목표
- 조건부 컴파일로 프레임워크별 테스트 분기

### **통합 테스트 (Integration Tests)**
- 크로스 프레임워크 호환성 검증
- 실제 사용 시나리오 테스트

### **성능 테스트 (Performance Tests)**
- BenchmarkDotNet 사용
- 동시성 컬렉션 성능 측정
- LINQ 성능 비교 (.NET 네이티브 vs 폴리필)

### **CI/CD 자동화**
- GitHub Actions에서 모든 테스트 자동 실행
- PR마다 빌드 및 테스트 검증
- 코드 커버리지 리포트 생성

---

## 📦 NuGet 패키지 전략

### **사용할 기존 NuGet 패키지**
라이브러리 의존성으로 추가 (선택적):

| 패키지 | 용도 | 최소 프레임워크 |
|--------|------|----------------|
| `System.Memory` | Span/Memory 타입 | NET461+ |
| `System.Buffers` | ArrayPool | NET461+ |
| `System.ValueTuple` | ValueTuple | NET40+ |
| `Microsoft.Bcl.AsyncInterfaces` | IAsyncEnumerable | NET461+ |
| `Microsoft.Bcl.HashCode` | HashCode | NET461+ |
| `IsExternalInit` | record/init 지원 | NET20+ |

### **직접 구현하여 제공**
- 모든 .NET 2.0~3.5용 폴리필은 직접 구현
- NuGet 패키지가 지원하지 않는 낮은 프레임워크 타겟

### **패키지 메타데이터**
```xml
<PropertyGroup>
  <PackageId>Jinobald.Polyfill</PackageId>
  <Version>2.0.0</Version>
  <Authors>Jinho Park</Authors>
  <Description>Complete polyfill library for .NET Framework 2.0 to .NET 10.0</Description>
  <PackageTags>polyfill;netframework;backport;compatibility</PackageTags>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
  <RepositoryUrl>https://github.com/Jinobald/Polyfill</RepositoryUrl>
  <PackageReadmeFile>README.md</PackageReadmeFile>
  <PackageIcon>icon.png</PackageIcon>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

---

## 🔧 개발 도구 및 환경

### **필수 도구**
- **.NET SDK**: 8.0 이상 (멀티 타게팅 지원)
- **IDE**: Visual Studio 2022 또는 JetBrains Rider
- **테스트**: xUnit + FluentAssertions
- **벤치마크**: BenchmarkDotNet
- **문서**: DocFX 또는 Sandcastle

### **추천 VS Code 확장**
- C# Dev Kit
- .NET Extension Pack
- Test Explorer

### **코드 품질 도구**
- StyleCop Analyzers
- Roslynator
- SonarAnalyzer

---

## 🎓 참고 자료

### **공식 문서**
- [.NET API Browser](https://learn.microsoft.com/en-us/dotnet/api/)
- [.NET Framework Version History](https://learn.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies)
- [C# Language Versioning](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version)

### **오픈소스 참고 구현**
- [CoreFX](https://github.com/dotnet/corefx) - .NET Core 구현
- [Reference Source](https://referencesource.microsoft.com/) - .NET Framework 참조 소스
- [LINQBridge](https://www.albahari.com/nutshell/linqbridge.aspx) - LINQ for .NET 2.0
- [Theraot.Core](https://github.com/theraot/Theraot) - 종합 백포트 라이브러리
- [PolySharp](https://github.com/Sergio0694/PolySharp) - 최신 C# 기능 폴리필

---

## ✅ 완료 체크리스트

각 워크스페이스 완료 시 체크:

### **워크스페이스 1: 델리게이트 패밀리**
- [ ] Action<> (0~16 파라미터) 구현
- [ ] Func<> (0~16 파라미터) 구현
- [ ] Predicate<T> 구현
- [ ] 단위 테스트 작성 (커버리지 80%+)
- [ ] XML 문서 주석 추가
- [ ] 모든 타겟 프레임워크 빌드 성공

### **워크스페이스 2: Tuple & ValueTuple**
- [x] Tuple<T1~T8> 구현 ✅
- [x] ValueTuple<T1~T8> 구현 ✅
- [x] 팩토리 메서드 구현 ✅
- [x] IStructuralEquatable/Comparable 구현 ✅
- [x] 단위 테스트 작성 ✅ (TupleTests.cs, ValueTupleTests.cs)
- [x] XML 문서 주석 추가 ✅
- [ ] TupleExtensions 구현 (Deconstruct, ToTuple, ToValueTuple) ⏸️
- [ ] TupleElementNamesAttribute 구현 ⏸️

### **워크스페이스 3: Lazy<T> & 스레딩**
- [ ] Lazy<T> 구현 (모든 생성자)
- [ ] ManualResetEventSlim 구현
- [ ] SemaphoreSlim 구현
- [ ] CountdownEvent 구현
- [ ] Barrier 구현
- [ ] 스레드 안전성 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 4: 동시성 컬렉션 Part 1**
- [ ] ConcurrentQueue<T> 구현
- [ ] ConcurrentStack<T> 구현
- [ ] ConcurrentBag<T> 구현
- [ ] 동시성 테스트 (멀티스레드)
- [ ] 성능 벤치마크
- [ ] XML 문서 주석 추가

### **워크스페이스 5: 동시성 컬렉션 Part 2**
- [ ] ConcurrentDictionary<K,V> 구현
- [ ] BlockingCollection<T> 구현
- [ ] IProducerConsumerCollection<T> 구현
- [ ] 동시성 테스트
- [ ] Producer-Consumer 시나리오 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 6: Progress & ReadOnly Collections**
- [ ] IProgress<T> / Progress<T> 구현
- [ ] IReadOnlyCollection<T> 구현
- [ ] IReadOnlyList<T> 구현
- [ ] IReadOnlyDictionary<K,V> 구현
- [ ] ReadOnlyDictionary<K,V> 구현
- [ ] SynchronizationContext 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 7: Compiler Attributes** 🟡 78% 완료
- [x] CallerMemberNameAttribute 구현 ✅
- [x] CallerFilePathAttribute 구현 ✅
- [x] CallerLineNumberAttribute 구현 ✅
- [ ] CallerArgumentExpressionAttribute 구현 ⏸️ (NET46+)
- [x] ExtensionAttribute 구현 ✅
- [x] IsExternalInit 구현 ✅
- [x] RequiredMemberAttribute 구현 ✅
- [x] SetsRequiredMembersAttribute 구현 ✅
- [ ] CompilerFeatureRequiredAttribute 구현 ⏸️ (NET47+)
- [ ] StringSyntaxAttribute 구현 ⏸️ (NET47+)
- [x] 컴파일러 통합 테스트 ✅
- [x] XML 문서 주석 추가 ✅

### **워크스페이스 8: Index & Range**
- [ ] Index 구현
- [ ] Range 구현
- [ ] 배열 인덱서 확장
- [ ] 문자열 인덱서 확장
- [ ] Span<T> 인덱서 지원
- [ ] 경계값 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 9: HashCode & FormattableString**
- [x] HashCode 구현 (FNV-1a 알고리즘) ✅
- [x] FormattableString 구현 ✅
- [x] FormattableStringFactory 구현 ✅
- [x] **조건부 컴파일 수정 완료** (2025-12-21) ✅
  - HashCode: `#if NET35 || ... || NET47`
  - FormattableString/Factory: `#if NET35 || ... || NET452`
- [x] 단위 테스트 작성 ✅ (HashCodeTests.cs)
- [ ] FormattableString 테스트 추가 ⏸️
- [x] XML 문서 주석 추가 ✅

### **워크스페이스 10: IAsyncEnumerable**
- [ ] IAsyncEnumerable<T> 구현
- [ ] IAsyncEnumerator<T> 구현
- [ ] IAsyncDisposable 구현
- [ ] ValueTask / ValueTask<T> 구현
- [ ] AsyncIteratorMethodBuilder 구현
- [ ] async foreach 테스트
- [ ] await using 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 11: LINQ Part 1** ✅ **완료** (2025-12-21)
- [x] Where, Select, SelectMany 구현 ✅
- [x] First/Last/Single 계열 구현 ✅
- [x] Any, All, Contains 구현 ✅
- [x] Count, ElementAt 구현 ✅
- [x] ToArray, ToList, ToDictionary 구현 ✅
- [x] Empty, Range, Repeat 구현 ✅
- [x] Concat, Distinct, Skip, Take 구현 ✅
- [x] SkipWhile, TakeWhile, Reverse 구현 ✅
- [x] Cast, OfType, SequenceEqual 구현 ✅
- [x] DefaultIfEmpty, Append, Prepend 구현 ✅
- [x] IGrouping, IOrderedEnumerable, ILookup, Lookup 구현 ✅
- [x] 지연 실행 검증 테스트 ✅
- [x] Null 인자 테스트 ✅
- [x] XML 문서 주석 추가 (한글) ✅
- [x] 조건부 컴파일 (#if NET20) ✅

**구현 파일**:
- `src/Jinobald.Polyfill/System/Linq/Enumerable.cs` (~1200줄)
- `src/Jinobald.Polyfill/System/Linq/IGrouping.cs`
- `src/Jinobald.Polyfill/System/Linq/IOrderedEnumerable.cs`
- `src/Jinobald.Polyfill/System/Linq/ILookup.cs`
- `src/Jinobald.Polyfill/System/Linq/Lookup.cs`

**테스트 파일**:
- `tests/.../System/Linq/EnumerableBasicTests.cs`
- `tests/.../System/Linq/EnumerableConversionTests.cs`

### **워크스페이스 12: LINQ Part 2**
- [ ] OrderBy/ThenBy 계열 구현
- [ ] GroupBy, Join, GroupJoin 구현
- [ ] Distinct, Union, Intersect, Except 구현
- [ ] Skip, Take 계열 구현
- [ ] 정렬 안정성 테스트
- [ ] 복잡한 쿼리 체이닝 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 13: LINQ Part 3**
- [ ] Aggregate, Sum, Average 구현
- [ ] Min, Max 계열 구현
- [ ] Range, Repeat, Empty 구현
- [ ] Append, Prepend, Chunk 구현
- [ ] 오버플로우 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 14: 컬렉션 확장**
- [ ] HashSet<T> 구현 (NET20용)
- [ ] SortedSet<T> 구현
- [ ] ObservableCollection<T> 구현
- [ ] INotifyCollectionChanged 구현
- [ ] 집합 연산 테스트
- [ ] CollectionChanged 이벤트 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 15: BigInteger (선택적)**
- [ ] BigInteger 구현
- [ ] Complex 구현
- [ ] 연산자 오버로딩
- [ ] 큰 숫자 연산 테스트
- [ ] 파싱 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 16: Async 확장**
- [ ] ConfiguredTaskAwaitable 구현
- [ ] ConfiguredTaskAwaiter 구현
- [ ] TaskExtensions (Unwrap, ConfigureAwait) 구현
- [ ] ICriticalNotifyCompletion 구현
- [ ] YieldAwaitable 구현
- [ ] SynchronizationContext 테스트
- [ ] XML 문서 주석 추가

### **워크스페이스 16-A: HttpClient & HTTP**
- [ ] HttpClient 구현
- [ ] HttpRequestMessage/HttpResponseMessage 구현
- [ ] HttpContent 및 파생 클래스 구현
- [ ] HttpMethod 구현
- [ ] HttpHeaders 구현
- [ ] 단위 테스트 작성
- [ ] XML 문서 주석 추가

### **워크스페이스 16-B: JSON 직렬화**
- [ ] JsonSerializer 구현
- [ ] JsonSerializerOptions 구현
- [ ] JsonPropertyNameAttribute 구현
- [ ] JsonIgnoreAttribute 구현
- [ ] 단위 테스트 작성
- [ ] XML 문서 주석 추가

### **워크스페이스 16-C: 추가 유틸리티 타입**
- [ ] DateOnly 구현
- [ ] TimeOnly 구현
- [ ] Half 구현
- [ ] UnreachableException 구현
- [ ] Nullable 분석 속성 구현
- [ ] Environment.ProcessPath 구현
- [ ] 단위 테스트 작성
- [ ] XML 문서 주석 추가

### **워크스페이스 16-D: 최신 LINQ 메서드**
- [ ] Chunk 구현
- [ ] DistinctBy/ExceptBy/IntersectBy/UnionBy 구현
- [ ] MinBy/MaxBy 구현
- [ ] Index/CountBy/AggregateBy 구현
- [ ] 단위 테스트 작성
- [ ] XML 문서 주석 추가

### **워크스페이스 17: 프로젝트 설정**
- [ ] net20, net35, net40 타겟 추가
- [ ] 조건부 컴파일 심볼 정의
- [x] AssemblyInfo.cs 추가 (InternalsVisibleTo)
- [ ] NuGet 패키지 참조 설정
- [ ] .editorconfig 작성
- [ ] GitHub Actions CI/CD 설정
- [ ] 모든 프레임워크 빌드 검증

### **워크스페이스 18: 통합 & 문서**
- [ ] 통합 테스트 프로젝트 생성
- [x] README.md 업데이트 (컴파일러 속성 섹션)
- [x] TESTING_STRATEGY.md 작성
- [ ] API 문서 생성 (DocFX)
- [ ] MIGRATION_GUIDE.md 작성
- [ ] CHANGELOG.md 작성
- [ ] NuGet 패키지 메타데이터 설정
- [ ] 라이선스 검토

---

## 📈 진행 상황 추적

### **전체 진행률** (최종 업데이트: 2025-12-21)
- [x] Phase 1: 기초 인프라 (3/4 = 75%) - **워크스페이스 1, 2, 7 완료**
  - ✅ WS1: 델리게이트 (100%)
  - 🟡 WS2: Tuple & ValueTuple (80% - TupleExtensions 누락)
  - ✅ WS7: Compiler Attributes (78% - 3개 속성 누락)
  - ⏸️ WS17: 프로젝트 설정 (부분 완료)

- [ ] Phase 2: 핵심 기능 (2/5 = 40%)
  - ✅ WS3: Lazy & Threading (83% - Barrier 누락)
  - ✅ WS6: Progress & ReadOnly (83% - ReadOnlyDictionary 누락)
  - ❌ WS8: Index & Range (0%)
  - ❌ WS9: HashCode & FormattableString (구현 완료, 조건부 컴파일 수정 완료)
  - ❌ WS14: 컬렉션 확장 (0%)

- [x] Phase 3: LINQ 구현 (1/3 = 33%)
  - ✅ WS11: LINQ Part 1 (100%) - 완료
  - ❌ WS12: LINQ Part 2 (0%)
  - ❌ WS13: LINQ Part 3 (0%)

- [ ] Phase 4: 동시성 라이브러리 (0/2 = 0%)
  - ❌ WS4-5: 완전 미구현

- [ ] Phase 5: 고급 기능 (0/3 = 0%)
  - ❌ WS10, 15, 16: 모두 미구현

- [ ] Phase 6: 실용적 확장 (0/4 = 0%)
  - ❌ WS16-A~D: 모두 미구현

- [ ] Phase 7: 통합 및 배포 (0/1 = 0%)

**핵심 워크스페이스**: 18개
**추가 워크스페이스** (선택적): 4개 (16-A, 16-B, 16-C, 16-D)
**총 워크스페이스**: 22개
**완료**: 6개 (WS1, WS2 부분, WS3 부분, WS6 부분, WS7 부분, WS11)
**진행률**: 약 27% (구현된 타입 기준)

**Phase 1 진행률**: 75% (3/4 완료)
**Phase 3 진행률**: 33% (1/3 완료)

---

## 🚀 시작 가이드 (Conductor 워크스페이스 활용)

### **병렬 작업 시작 방법**

1. **각 워크스페이스를 별도 Conductor 워크스페이스로 생성**
   ```bash
   # 예시: 워크스페이스 1 시작
   conductor create workspace "Polyfill-WS1-Delegates"
   ```

2. **각 워크스페이스에서 이 계획 파일 참조**
   ```
   IMPLEMENTATION_PLAN.md의 해당 워크스페이스 섹션 읽고 작업 시작
   ```

3. **작업 완료 후 메인 브랜치에 PR 생성**
   ```bash
   git checkout -b feature/ws1-delegates
   # 구현 완료
   git commit -m "Implement Action/Func delegates (WS1)"
   gh pr create
   ```

4. **체크리스트 업데이트**
   - 이 파일의 해당 워크스페이스 체크리스트 업데이트

### **Agent에게 전달할 프롬프트 예시**

**워크스페이스 1 시작 시**:
```
IMPLEMENTATION_PLAN.md 파일의 "워크스페이스 1: 델리게이트 패밀리" 섹션을 읽고,
다음을 완료해줘:
1. System/Action.cs 파일 생성 및 구현 (NET20, NET35, NET40 조건부 컴파일)
2. System/Func.cs 파일 생성 및 구현
3. System/Predicate.cs 파일 생성 및 구현
4. tests/Jinobald.Polyfill.Tests/System/ActionTests.cs 테스트 작성
5. tests/Jinobald.Polyfill.Tests/System/FuncTests.cs 테스트 작성
6. 모든 public API에 XML 문서 주석 추가
7. 빌드 및 테스트 실행으로 검증
```

**워크스페이스 11 시작 시**:
```
IMPLEMENTATION_PLAN.md의 "워크스페이스 11: LINQ Part 1" 섹션을 참고하여,
System.Linq/Enumerable.cs에 다음 메서드를 구현해줘:
- Where, Select, SelectMany
- First, FirstOrDefault, Last, LastOrDefault
- Single, SingleOrDefault
- Any, All, Contains
- Count, LongCount

.NET 2.0 타겟이므로 확장 메서드 사용.
지연 실행(deferred execution) 구현 필수.
각 메서드마다 테스트 작성.
```

---

## 📞 문의 및 기여

질문이나 제안사항이 있으면:
- GitHub Issues: [프로젝트 저장소]
- Email: [담당자 이메일]

---

**마지막 업데이트**: 2025-12-21
**문서 버전**: 1.1
**작성자**: Claude Code Agent

---

## 📝 변경 이력

### v1.1 (2025-12-21)
- ✅ **워크스페이스 11 (LINQ Part 1) 완료**
  - Enumerable 클래스에 ~30개 연산자 구현
  - IGrouping, IOrderedEnumerable, ILookup, Lookup 인터페이스/클래스 구현
  - EnumerableBasicTests.cs, EnumerableConversionTests.cs 테스트 작성
  - 모든 주석 한글화
- HashCode.cs 조건부 컴파일 수정 (NET471, NET472, NET48, NET481 추가)
- Append/Prepend 테스트 조건부 컴파일 추가 (NET471+)
- 전체 진행률 27%로 업데이트

### v1.0 (2025-12-21)
- 초기 계획 문서 작성
- 18개 워크스페이스 정의
- Phase 1~7 작업 계획 수립
