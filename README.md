# Jinobald.Polyfill

.NET Framework 3.5부터 최신 .NET까지 다양한 버전에서 최신 .NET 기능을 사용할 수 있도록 하는 Polyfill 라이브러리입니다.

## 개요

Jinobald.Polyfill은 오래된 .NET Framework 버전에서 최신 .NET의 타입과 기능을 사용할 수 있게 해주는 라이브러리입니다. 레거시 프로젝트를 유지보수하면서도 현대적인 C# 문법과 API를 활용할 수 있습니다.

## 지원 타겟 프레임워크

- .NET Framework 3.5
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

### 스레딩
- **Volatile** - Volatile 읽기/쓰기 작업
- **InterlockedEx** - Interlocked 확장

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

// ValueTuple 사용 (.NET 3.5에서도 가능)
var tuple = (Name: "John", Age: 30);
Console.WriteLine($"{tuple.Name} is {tuple.Age} years old");

// Index와 Range 사용
var array = new[] { 1, 2, 3, 4, 5 };
var lastItem = array[^1];  // 5
var range = array[1..3];   // { 2, 3 }

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
│   └── Jinobald.Polifill/          # 메인 라이브러리
│       └── System/                  # System 네임스페이스 확장
│           ├── Collections/         # 컬렉션 관련
│           ├── Diagnostics/         # 진단 관련
│           ├── Numerics/            # 숫자 관련
│           ├── Runtime/             # 런타임 관련
│           └── Threading/           # 스레딩 관련
└── tests/
    └── Jinobald.Polifill.Tests/    # 단위 테스트
```

## 기여

버그 리포트, 기능 제안, Pull Request를 환영합니다.

## 라이선스

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

## 저작권

Copyright (c) 2025 Jinho Park

## 참고

이 라이브러리의 일부 코드는 .NET Foundation의 코드를 기반으로 하며, MIT 라이선스 하에 배포됩니다.
