# CI/CD 가이드

이 문서는 Jinobald.Polyfill 프로젝트의 CI/CD 파이프라인에 대한 완전한 가이드입니다.

## 📋 목차

1. [CI/CD 개요](#cicd-개요)
2. [워크플로우 설명](#워크플로우-설명)
3. [필수 시크릿 설정](#필수-시크릿-설정)
4. [브랜치 전략](#브랜치-전략)
5. [릴리스 프로세스](#릴리스-프로세스)
6. [문제 해결](#문제-해결)

---

## CI/CD 개요

### 자동화된 파이프라인

우리의 CI/CD 파이프라인은 다음을 자동화합니다:

- ✅ **모든 타겟 프레임워크 빌드** (NET35~NET481, NET6.0~NET10.0)
- ✅ **크로스 플랫폼 테스트** (Ubuntu, Windows, macOS)
- ✅ **코드 품질 분석** (Roslynator, SonarCloud)
- ✅ **코드 커버리지 측정** (80%+ 목표)
- ✅ **PR 검증** (자동 라벨링, 크기 체크, 문서 검증)
- ✅ **NuGet 패키지 자동 퍼블리시**
- ✅ **GitHub Release 생성**
- ✅ **의존성 자동 업데이트**

---

## 워크플로우 설명

### 1. Build and Test (`build-and-test.yml`)

**트리거**:
- Push: `main`, `develop`, `JinoPay/**` 브랜치
- Pull Request: `main`, `develop` 브랜치
- 수동 실행 (workflow_dispatch)

**주요 작업**:

#### Job 1: Build Matrix
```yaml
strategy:
  matrix:
    os: [ubuntu-latest, windows-latest, macos-latest]
```

- 3개 OS에서 동시 빌드
- Windows에서만 .NET Framework 타겟 빌드
- 빌드 아티팩트 업로드 (7일 보관)

#### Job 2: Test Matrix
```yaml
strategy:
  matrix:
    framework: [net6.0, net7.0, net8.0, net9.0]
    os: [ubuntu-latest, windows-latest, macos-latest]
```

- 각 프레임워크별 테스트 실행
- OpenCover 형식 코드 커버리지 수집
- 테스트 결과 TRX 파일 생성

#### Job 3: Coverage Report
- ReportGenerator로 통합 커버리지 리포트 생성
- Markdown 요약을 PR 코멘트로 추가
- Codecov에 업로드

#### Job 4: Package
- NuGet 패키지 생성 (main 브랜치만)
- GitVersion으로 자동 버전 결정
- Symbol 패키지 (.snupkg) 생성

#### Job 5: Code Quality
- Roslynator 분석기 실행
- SonarCloud 스캔 (PR 및 main)

**예상 실행 시간**: 15~25분

---

### 2. PR Validation (`pr-validation.yml`)

**트리거**: Pull Request (opened, synchronize, reopened)

**주요 검증**:

#### 1. PR 기본 체크
- **Semantic PR 제목 검증**:
  ```
  feat: Add Action/Func delegates
  fix: Resolve race condition in ConcurrentQueue
  docs: Update README with new features
  ```
- **Merge 충돌 감지**
- **소스 변경 시 테스트 필요성 경고**

#### 2. Quick Build
- Windows에서 빠른 빌드 검증
- 컴파일 오류 조기 발견

#### 3. 변경된 코드 테스트
- `tj-actions/changed-files`로 변경 파일 감지
- 변경된 코드에 대해서만 테스트 실행 (시간 절약)

#### 4. Code Style
- `dotnet format --verify-no-changes`로 포맷 검증
- Roslynator 분석기 실행

#### 5. Documentation 검증
- XML 문서 주석 누락 경고
- Markdown 링크 유효성 검사

#### 6. 자동 라벨링
- 파일 경로 기반 자동 라벨 추가:
  - `area: threading`, `area: linq`, `area: collections` 등
  - `type: feature`, `type: tests`, `type: docs` 등

#### 7. PR 크기 체크
- 변경 라인 수 및 파일 수 측정
- `size/S`, `size/M`, `size/L` 라벨 자동 추가
- 대형 PR에 대해 경고

**예상 실행 시간**: 5~10분

---

### 3. Release (`release.yml`)

**트리거**:
- Git 태그 push: `v*.*.*` (예: `v2.0.0`)
- 수동 실행 (버전 입력)

**릴리스 프로세스**:

#### Step 1: Release Build
```bash
git tag v2.0.0
git push origin v2.0.0
```

- 모든 프레임워크 빌드
- 전체 테스트 실행
- NuGet 패키지 생성 (버전 적용)

#### Step 2: Publish to NuGet.org
```bash
dotnet nuget push *.nupkg --api-key $NUGET_API_KEY
```

- `production` environment 승인 필요 (선택사항)
- Symbol 패키지도 함께 퍼블리시

#### Step 3: Publish to GitHub Packages
- GitHub 패키지 레지스트리에도 퍼블리시
- 내부 사용 또는 백업용

#### Step 4: Create GitHub Release
- CHANGELOG.md에서 릴리스 노트 추출
- NuGet 패키지 첨부
- 지원 프레임워크 목록 포함

#### Step 5: Notify
- 릴리스 성공 로그 출력
- (옵션) Slack/Discord 알림

**예상 실행 시간**: 20~30분

---

### 4. Dependency Update (`dependency-update.yml`)

**트리거**:
- 매주 월요일 00:00 UTC (KST 09:00)
- 수동 실행

**작업**:
- `dotnet-outdated-tool`로 오래된 패키지 감지
- 최신 버전으로 자동 업데이트
- PR 자동 생성 (`chore/dependency-updates` 브랜치)

**PR 내용**:
- 변경된 패키지 목록
- `dependencies`, `automated` 라벨 자동 추가

**리뷰 후 수동 머지 필요**

---

## 필수 시크릿 설정

GitHub Repository Settings → Secrets and variables → Actions에서 설정:

### 1. NUGET_API_KEY
- **목적**: NuGet.org에 패키지 퍼블리시
- **획득 방법**:
  1. https://www.nuget.org/ 로그인
  2. Account → API Keys
  3. "Create" 클릭
  4. Key Name: `Jinobald.Polyfill CI/CD`
  5. Glob Pattern: `Jinobald.Polyfill`
  6. Expire: 365 days
  7. 생성된 키 복사 후 GitHub Secrets에 추가

### 2. SONAR_TOKEN (선택사항)
- **목적**: SonarCloud 코드 품질 분석
- **획득 방법**:
  1. https://sonarcloud.io/ 로그인
  2. My Account → Security
  3. Generate Token
  4. Name: `Jinobald.Polyfill`
  5. 생성된 토큰을 GitHub Secrets에 추가

### 3. CODECOV_TOKEN (선택사항)
- **목적**: Codecov 코드 커버리지 리포트
- **획득 방법**:
  1. https://codecov.io/ 로그인
  2. Repository 추가
  3. Settings → General → Repository Upload Token 복사
  4. GitHub Secrets에 추가

### 4. SLACK_WEBHOOK (선택사항)
- **목적**: 릴리스 알림
- Slack Incoming Webhook URL

---

## 브랜치 전략

### GitFlow 기반 전략

```
main (프로덕션)
  ├─ develop (개발)
  │   ├─ JinoPay/* (기능 브랜치)
  │   ├─ feature/* (기능 브랜치)
  │   └─ hotfix/* (핫픽스)
  └─ release/* (릴리스 준비)
```

### 브랜치별 CI 동작

| 브랜치 | 빌드 | 테스트 | 패키지 | 퍼블리시 |
|--------|------|--------|--------|---------|
| `main` | ✅ | ✅ | ✅ | ❌ (태그 시에만) |
| `develop` | ✅ | ✅ | ✅ (alpha) | ❌ |
| `JinoPay/*` | ✅ | ✅ | ❌ | ❌ |
| `feature/*` | ✅ | ✅ | ❌ | ❌ |
| `release/*` | ✅ | ✅ | ✅ (rc) | ❌ |
| `hotfix/*` | ✅ | ✅ | ✅ (beta) | ❌ |

### GitVersion 설정

`GitVersion.yml` 파일로 버전 자동 결정:

```yaml
# main 브랜치: 1.0.0
# develop 브랜치: 1.1.0-alpha.1
# feature 브랜치: 1.1.0-feat-my-feature.1
# release 브랜치: 2.0.0-rc.1
```

**버전 증가 규칙**:
- `major:` 또는 `breaking:` 커밋 메시지 → Major 버전 증가
- `feat:` 또는 `feature:` → Minor 버전 증가
- `fix:` 또는 `patch:` → Patch 버전 증가
- `docs:`, `chore:`, `style:` → 버전 변경 없음

---

## 릴리스 프로세스

### 정식 릴리스 (Production)

1. **개발 완료 후 release 브랜치 생성**:
   ```bash
   git checkout develop
   git checkout -b release/2.0.0
   ```

2. **버전 업데이트 및 테스트**:
   - CHANGELOG.md 업데이트
   - 최종 테스트 실행
   - 버그 수정

3. **main 브랜치에 머지**:
   ```bash
   git checkout main
   git merge release/2.0.0
   ```

4. **태그 생성 및 푸시** (릴리스 자동 트리거):
   ```bash
   git tag v2.0.0
   git push origin main --tags
   ```

5. **CI/CD가 자동으로**:
   - 빌드 및 테스트
   - NuGet 패키지 생성
   - NuGet.org 및 GitHub Packages에 퍼블리시
   - GitHub Release 생성

6. **develop에 병합** (버전 동기화):
   ```bash
   git checkout develop
   git merge main
   git push origin develop
   ```

### 핫픽스 릴리스

1. **main에서 hotfix 브랜치 생성**:
   ```bash
   git checkout main
   git checkout -b hotfix/2.0.1
   ```

2. **버그 수정 및 커밋**:
   ```bash
   git commit -m "fix: critical bug in ConcurrentDictionary"
   ```

3. **main에 머지 후 태그**:
   ```bash
   git checkout main
   git merge hotfix/2.0.1
   git tag v2.0.1
   git push origin main --tags
   ```

4. **develop에도 병합**:
   ```bash
   git checkout develop
   git merge hotfix/2.0.1
   git push origin develop
   ```

---

## 문제 해결

### 빌드 실패

#### "Framework not found: net35"
- **원인**: 오래된 .NET SDK 설치 필요
- **해결**:
  ```yaml
  - uses: actions/setup-dotnet@v4
    with:
      dotnet-version: '3.5.x'
  ```

#### "MSBuild not found"
- **원인**: Windows에서만 .NET Framework 빌드 가능
- **해결**: 조건부 실행 추가
  ```yaml
  if: runner.os == 'Windows'
  ```

### 테스트 실패

#### "Test timeout"
- **원인**: 동시성 테스트가 교착 상태
- **해결**: 테스트에 타임아웃 추가
  ```csharp
  [Fact(Timeout = 5000)]
  ```

#### "Framework not supported"
- **원인**: 특정 프레임워크에서만 테스트 실행 필요
- **해결**: 조건부 컴파일 사용
  ```csharp
  #if NET35
  [Fact]
  public void Test_NET35_Specific() { }
  #endif
  ```

### 패키지 퍼블리시 실패

#### "Package already exists"
- **원인**: 같은 버전이 이미 NuGet.org에 존재
- **해결**: 버전 증가 후 재시도

#### "Invalid API key"
- **원인**: NUGET_API_KEY 시크릿 만료 또는 잘못됨
- **해결**: 새 API 키 생성 후 시크릿 업데이트

### 코드 커버리지 실패

#### "No coverage report found"
- **원인**: 테스트가 실행되지 않음
- **해결**: 테스트 실행 확인
  ```bash
  dotnet test --collect:"XPlat Code Coverage"
  ```

#### "Coverage below threshold"
- **원인**: 테스트 커버리지 80% 미만
- **해결**: 테스트 추가 작성

---

## 모범 사례

### 1. 커밋 메시지
```
feat(delegates): add Action<T1..T16> delegates for NET40

- Implement Action delegates with 5-16 parameters
- Add XML documentation
- Add unit tests with 95% coverage

Closes #123
```

### 2. PR 생성
- **제목**: Semantic 형식 준수
- **설명**: 변경 내용, 테스트 결과, 스크린샷 포함
- **라벨**: 자동 라벨 확인 후 필요시 수동 추가
- **리뷰어**: 최소 1명 지정

### 3. 릴리스 준비
- CHANGELOG.md 업데이트 필수
- 모든 테스트 통과 확인
- 문서 업데이트 (README, API docs)
- 마이그레이션 가이드 작성 (Breaking changes 있을 경우)

### 4. 코드 리뷰
- CI 통과 후 리뷰 시작
- 코드 커버리지 확인
- 성능 영향 검토
- 문서화 확인

---

## CI/CD 메트릭

### 목표

| 메트릭 | 목표 | 현재 |
|--------|------|------|
| 빌드 시간 | < 20분 | - |
| 테스트 성공률 | > 99% | - |
| 코드 커버리지 | > 80% | - |
| PR 검증 시간 | < 10분 | - |
| 릴리스 주기 | 월 1회 | - |

### 모니터링

- GitHub Actions 대시보드
- Codecov 대시보드
- SonarCloud 대시보드
- NuGet.org 다운로드 통계

---

## 추가 리소스

- [GitHub Actions 문서](https://docs.github.com/en/actions)
- [GitVersion 문서](https://gitversion.net/)
- [NuGet 패키징 가이드](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package)
- [SonarCloud 문서](https://docs.sonarcloud.io/)
- [Codecov 문서](https://docs.codecov.com/)

---

**마지막 업데이트**: 2025-12-22
**작성자**: Claude Code Agent

---

## 최근 변경 사항 (2025-12-22)

### 구현 완료된 기능
- **HttpClient & HTTP**: 완전한 HTTP 클라이언트 구현 (.NET 3.5+)
- **Parallel 클래스**: 병렬 처리 지원 (Parallel.For, Parallel.ForEach, Parallel.Invoke)
- **LINQ**: 모든 주요 연산자 구현 완료

### 테스트 통계
- **총 테스트 케이스**: 473개 이상
- **테스트 파일**: 39개
- **지원 프레임워크**: 17개 (NET35 ~ NET10.0)

### 다음 우선순위
1. Index & Range 구현
2. Concurrent Collections 구현
3. IAsyncEnumerable 지원
