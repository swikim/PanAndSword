# Pan & Sword

3D 탑다운 액션 로그라이트 · Unity 2022.3 LTS (Built-in RP) · 1인 개발

프라이팬과 칼을 든 셰프가 던전의 몬스터를 처치하고 재료를 모아, 로비 주방에서 요리·강화한 뒤 다시 던전으로 향하는 로그라이트 게임입니다.

## 개요

| | |
|---|---|
| 장르 | 3D 탑다운 액션 로그라이트 |
| 개발 기간 | 2026.06 ~ 개발 중 (1인 개발) |
| 엔진 | Unity 2022.3 LTS, Built-in RP, C# |
| 타겟 플랫폼 | Android |
| 핵심 루프 | 전투(팬·칼) → 재료 수집 → 로비 요리·강화 → 재입장 |
| 레퍼런스 | Archero 2(전투), Overcooked(UI), Hades(스킬 선택 UI), Gunfire Reborn(3D 모바일 최적화) |

이전 프로젝트 **Dogbit**(2D 픽셀 아트 액션 RPG)에서 사용한 Strategy Pattern, Object Pooling 설계를 3D 환경으로 확장한 프로젝트입니다.

## 핵심 시스템

- **던전 구조**: Dungeon → Stage(4) → Room(4) 계층, Room 클리어 시 Door 개방 → Portal로 다음 Stage 이동 (`RoomManager`, `RoomController`, `Portal`)
- **전투**: 무기(팬/칼) 전환에 따라 판정 각도·공격 주기·데미지 배율 변경. `EnemyRegistry` 기반 최근접 적 자동 타겟팅, 근거리 원형 범위 + 전방 부채꼴 범위 이중 판정 (`EnemyDetector`, `WeaponSwitcher`, `Skill`)
- **적 / 보스**: `IAttackPattern` 인터페이스 기반 공격 패턴. 보스는 플레이어와의 거리에 따라 패턴을 선택하고, HP 50% 이하에서 2페이즈로 전환 (`Enemy`, `Boss`)
- **재료·요리·강화**: 던전에서 획득한 재료는 런 동안 별도로 보관하다가 던전 종료 시(클리어·사망) 영구 데이터에 반영하고 JSON으로 저장. 로비에서 요리로 소모해 스탯 강화 (`IngredientManager`, `RecipeManager`, `UpgradeManager`, `SaveManager`)
- **데이터 아키텍처**: `EnemyData` / `IngredientData` / `RecipeData` ScriptableObject 기반 — 기획 데이터와 로직 분리
- **수익화**: Unity Ads 전면 광고(광고 제거 구매 시 표시 생략, 표시 실패 시에도 후속 콜백 실행), Unity IAP v5(`StoreController` 이벤트 방식) 광고 제거 비소모성 상품 구매 및 실행 시 구매 내역 조회로 복원 (`AdManager`, `IAPManager`)

## 아키텍처 하이라이트

### Strategy Pattern — 공격 패턴

`IAttackPattern`(`IsRunning`, `Execute`) 인터페이스로 공격 방식을 분리했습니다.

- 일반 적: `EnemyData.attackType`에 따라 `NormalAttackPattern`(근접) 또는 `RangedAttackPattern`(투사체)을 사용
- 보스: `MeleeAttackPattern`(돌진)과 `AoeAttackPattern`(경고 표시 후 범위 판정) 두 패턴을 보유하고, `judgeRange` 기준으로 원거리면 AoE, 근거리면 돌진을 실행
- 페이즈 전환: HP 50% 이하가 되면 공격 쿨타임을 줄이고, 수치를 강화한 패턴 세트로 `attackPatterns` 리스트를 교체

```
Assets/Scripts/Enemy/IAttackPattern.cs
Assets/Scripts/Enemy/AttackPattern/
Assets/Scripts/Enemy/Enemy.cs
Assets/Scripts/Enemy/Boss.cs
```

### Observer Pattern — 상태 변화 전파

`OnHpChanged`, `OnHealthChanged`, `OnPlayerDied` 이벤트로 HUD, HealthBar, 페이즈 전환 로직이 느슨하게 결합됩니다. 상태 변경 시 반드시 함께 실행되어야 하는 로직(사망 체크 등)은 `TakeDamage()` 같은 정문 메서드로만 접근하도록 캡슐화했습니다.

```
Assets/Scripts/Enemy/Enemy.cs
Assets/Scripts/Player/PlayerController.cs
Assets/Scripts/HUD/HUDManager.cs
```

### Object Pooling

타격 이펙트(`HitEffectPool`)와 재료 아이템(`IngredientPool`)을 Queue 기반으로 풀링해 빈번한 Instantiate/Destroy를 방지했습니다.

```
Assets/Scripts/GameCore/HitEffectPool.cs
Assets/Scripts/Ingredient/IngredientPool.cs
```

### Animator 파라미터 해시 캐싱

`animator.SetTrigger(AnimParam.Attack.ToString())`처럼 매 호출마다 enum을 문자열로 변환하던 코드를 `AnimHash` 정적 클래스의 `Animator.StringToHash` 캐싱 값으로 교체했습니다. Unity Profiler(Deep Profile)로 `Enemy.FixedUpdate()`의 GC Alloc이 40B → 0B로 줄어든 것을 확인했습니다.

```
Assets/Scripts/GameCore/AnimParam.cs
```

## 프로젝트 구조

```
Assets/Scripts/
├── Enemy/           # 적 AI, 보스, 공격 패턴
│   ├── AttackPattern/
│   └── EnemyData/
├── Player/          # 플레이어 컨트롤, 스킬, 무기 전환, 자동 타겟팅
├── Room/            # 던전 구조, 포탈
├── Ingredient/      # 재료 수집·풀링
├── Recipe/          # 요리 시스템
├── Upgrade/         # 강화 시스템
├── UI/              # 결과창, 일시정지
├── HUD/
├── Loby/
├── ADs/             # AdManager, IAPManager
└── GameCore/        # SaveManager, SoundManager, 풀링, 공용 데이터
```

## 향후 계획

- 로비 콘텐츠 추가 (도감 등)
- 던전 추가 개발
- 밸런싱 및 Google Play 출시 준비