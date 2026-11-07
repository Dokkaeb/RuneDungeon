# Rune Dungeon

> 마녀(Witch)가 던전 앞에서 주운 룬스톤으로 던전의 비밀을 밝혀 보물을 찾는 **2D 퍼즐 탈출 게임**

드래그 앤 드롭 아이템, 스프라이트 마스크 기믹, 암호 키패드, Behavior Tree 기반 적 AI까지 **1주 만에 1인으로 설계하고 구현**했습니다.

- 시연 영상 : https://youtu.be/8KDLkqQ1-s0
- 개발기 (블로그) : https://trew202.tistory.com/107

<br>

## 1. 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 장르 | 2D · 퍼즐 · 탈출 |
| 플랫폼 | PC Web / Windows |
| 개발 형태 | 개인 프로젝트 (1인 개발) |
| 개발 기간 | 2025.11.21 ~ 2025.11.27 (약 1주) |
| 엔진 / 언어 | Unity (URP) / C# |

<br>

## 2. 클래스 설계

| 분류 | 설계 |
|------|------|
| 플레이어 | 상태 패턴으로 각 상태 분리 |
| 매니저 | 싱글톤으로 GameManager · UIManager · SoundManager 설계 |
| 적(함정) | Behavior Tree로 적 AI 관리 |
| UI | UI와 게임 로직 분리, UIManager가 중계 |
| 카메라 | Cinemachine |

<br>

## 3. 주요 구현 내용

### 3.1 FSM 기반 플레이어 컨트롤러
- `IWitchState` 인터페이스를 직접 설계하고 `Idle / Move / Jump / Interact / Hit` 5개 상태로 행동을 분리
- `Witch.cs` 는 입력만 받고, `SetState()` 로 상태 전환을 중앙 관리하며 Enter / Exit / Update 생명주기 보장
- Unity Input System의 `OnMove`, `OnJump`, `OnInteraction` 콜백을 상태 로직과 분리해 다양한 기기 대응과 새 액션 추가가 쉬움

**물리 기반 이동 · 점프**
- `Rigidbody2D.linearVelocity` 를 직접 제어해 이동 · 점프 구현
- 점프할 때 `noFriction`, 착지할 때 `useFriction` 머티리얼로 자동 교체해 벽에 달라붙는 문제 방지
- `Physics2D.Raycast` 로 지형 판정(IsGrounded), 공중에서도 좌우 이동 허용

**피격 · HP**
- `HitState` : 넉백 방향 계산 → 일정 시간 조작 불가 → 자동 복귀
- 넉백 강도(`_knockbackForce`), 피격 시간(`_hitDuration`)을 Inspector에서 조정
- `Trap.cs` 로 함정과 피격 상태를 연동, `HpBarView.cs` 가 이벤트로 HP 바를 실시간 갱신

### 3.2 스프라이트 마스크 · 드래그 앤 드롭 아이템
- **스프라이트 마스크**를 주 기믹으로 사용해, 룬스톤 주변에서만 숨겨진 사물이 보이도록 설계
- `DraggableImage.cs` : `IBeginDragHandler`, `IDragHandler`, `IEndDragHandler` 로 UI 이미지 드래그 처리
- `DropZone.cs` : 드롭 영역을 판정한 뒤 `ItemData` ScriptableObject를 기준으로 월드에 프리팹 생성
- 상호작용 키를 누르면 소환한 아이템이 다시 인벤토리로 돌아옴
- `UIManager.cs` 가 HP UI · 인벤토리 슬롯 · 드래그 상태를 통합 관리해 UI와 게임 로직을 분리

### 3.3 상호작용 시스템 · 키패드 기믹
- `IInteractable` 인터페이스로 세이브 포인트 · 키패드 · 워프 · 가이드 · 다음 스테이지 · 클리어 등 **6종 오브젝트를 같은 방식으로 상호작용**
- Input System에 `Interaction` 액션(G키)을 추가, `InteractState` 에서는 이동을 막고 완료 후 `IdleState` 로 복귀
- **암호 키패드** : 입력값을 리스트에 저장하고, 길이가 비밀번호와 같아지면 비교. 틀리면 리스트를 비우고 버튼을 다시 활성화, 맞으면 문 개방. 언제든 닫을 수 있는 나가기 버튼 제공
- **워프** : 트리거에 들어가면 지정 위치로 즉시 이동, 타일맵 구역 간 이동에 활용

### 3.4 드래그 앤 런치
- 드래그로 당긴 거리에 비례해 발사 거리를 계산하고, 당긴 만큼 트레일로 표시
- 발사체가 날아갈 때 Cinemachine 카메라가 따라감

### 3.5 Behavior Tree 기반 적 AI
- Unity Behavior 패키지로 BT 그래프를 직접 만들어 적의 탐지 · 추격 · 공격을 노드로 구성
- **PatrolAction** : 웨이포인트를 순서대로 이동하는 순찰
- **ChaseAction** : 탐지 범위(`DetectRange`) 안에 플레이어가 들어오면 추격, 벗어나면 추격을 끝내고 비활성화
- Blackboard 변수(`Self`, `Player`, `MoveSpeed`, `DetectRange`)로 AI 수치를 밖에서 조정
- Cinemachine 연출로 추격의 긴장감을 높임

### 3.6 씬 구성 · 세이브 포인트
- Tilemap으로 Stage1 · Stage2 맵을 직접 디자인하고, House(허브) · Stage1 · Stage2 씬으로 분리
- `SceneLoader` 로 타이틀 → 인게임 → 다음 스테이지 전환 관리, 타이틀 · 런처 UI 제작
- **세이브 포인트** : 상호작용 키로 현재 위치와 체력을 저장하고, 사망하면 그 지점에서 부활. 저장하면 색이 바뀌어 저장 여부를 표시

### 3.7 사운드
- `BGMSliderController.cs` 로 배경음악 볼륨을 실시간 조절, UIManager와 연동해 인게임 UI에서 관리

<br>

## 4. 설계 패턴과 개선 효과

| 패턴 | 미적용 시 | 적용 후 | 개선 |
|------|------|------|------|
| State (`IWitchState`) | `Update()` 하나에 모든 if-else | 새 State 클래스 하나만 추가 | 수정 범위 약 80% 감소 |
| Interface (`IInteractable`) | 오브젝트 6종만큼 태그 분기 | 인터페이스 호출만 | 분기 100% 제거 |
| Singleton (Game · UI · Sound Manager) | 씬마다 재초기화, `FindObjectOfType` 탐색 | 정적 참조로 O(1) 접근, 씬 넘어 상태 보존 | 데이터 유실 위험 제거 |
| Event (`OnHpChanged`) | 60fps 폴링 · 분당 3,600회 확인 | HP가 바뀔 때만 호출, `OnDestroy` 에서 구독 해제 | 호출 99% 감소 |
| Behavior Tree | 단일 스크립트 AI, 조건문 전체 검토 | 노드 추가만으로 행동 확장, `DetectAction` 공유 | 확장 비용 약 70% 감소 |
| ScriptableObject (`ItemData`) | 아이템마다 코드 수정 | 에셋만 추가 | 코드 수정 위험 제거 |

<br>

## 5. 기술 스택

| 분류 | 기술 |
|------|------|
| 엔진 | Unity (URP) |
| 언어 | C# |
| 입력 | Unity Input System |
| 물리 | Rigidbody2D, PhysicsMaterial2D, Raycast |
| AI | Unity Behavior (Behavior Tree) |
| UI | Unity UI, TextMeshPro, 드래그 앤 드롭 |
| 데이터 | ScriptableObject (ItemData) |
| 맵 · 카메라 | 2D Tilemap, Sprite Mask, Cinemachine |
| 패턴 | State, Interface, Singleton, Event, Manager |

<br>

## 6. 회고

**아쉬운 점**
- 세이브 · 로드를 JSON으로 제대로 만들어 보고 싶었지만, 기간과 규모상 위치 · 체력 저장 방식으로 대신했습니다.
- ScriptableObject를 더 넓게 써 보고 싶었는데 적용한 곳이 많지 않았습니다.
- 이펙트를 늘려 오브젝트 풀링까지 적용하고 싶었지만 하지 못했습니다.

**배운 점**
- 디자인 패턴의 중요성과, 첫 설계를 견고하게 하는 것의 중요성
- 1주 정도의 짧은 프로젝트에서 규모를 어디까지 잡고 기능을 구현해야 하는지
