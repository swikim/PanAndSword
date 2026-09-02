# Pan & Sword

3D 탑다운 액션 로그라이트 · Unity 2022.3 LTS (Built-in RP) · 1인 개발

프라이팬과 칼을 든 셰프가 던전의 몬스터를 처치하고 재료를 모아, 로비 주방에서 요리·강화한 뒤 다시 던전으로 향하는 로그라이트 게임입니다.

## 개요

| | |
|---|---|
| 장르 | 3D 탑다운 액션 로그라이트 |
| 개발 기간 | 2026.06 – 2026.10 (1인 개발) |
| 엔진 | Unity 2022.3 LTS, Built-in RP, C# |
| 타겟 플랫폼 | Android / iOS |
| 핵심 루프 | 전투(팬·칼) → 재료 수집 → 로비 요리·강화 → 재입장 |
| 레퍼런스 | Archero 2(전투), Overcooked(UI), Hades(스킬 선택 UI), Gunfire Reborn(3D 모바일 최적화) |

이전 프로젝트 **Dogbit**(2D 픽셀 아트 액션 RPG)의 FSM 전투, Strategy Pattern, Object Pooling 아키텍처를 계승·확장한 프로젝트입니다.

## 핵심 시스템

- **던전 구조**: Dungeon → Stage(4) → Room(4) 계층, Room 클리어 시 Door 개방 → Portal로 다음 Stage 이동 (`RoomManager`, `RoomController`, `Portal`)
- **전투**: 무기(팬/칼) 전환, 자동 타겟팅 및 근접/원거리 판정 (`EnemyDetector`, `WeaponSwitcher`, `Skill`)
- **보스 시스템**: Strategy Pattern 기반 공격 패턴, HP 50% 도달 시 페이즈 전환으로 패턴 세트 교체 (`Boss`, `IAttackPattern` 구현체들)
- **재료·요리·강화**: 런 단위 재료 수집 → 로그라이트 규칙(완주 시에만 확정)에 따른 영구 저장, 요리로 소모해 스탯 강화 (`IngredientManager`, `RecipeManager`, `UpgradeManager`)
- **데이터 아키텍처**: `EnemyData` / `IngredientData` / `RecipeData` ScriptableObject 기반 — 기획 데이터와 로직 분리
- **수익화**: Unity Ads(전면 광고), Unity IAP v5(광고 제거 구매·복원)

## 아키텍처 하이라이트

### Strategy Pattern — 보스 공격 패턴
`IAttackPattern` 인터페이스로 `MeleeAttackPattern`(대시), `RangedAttackPattern`, `AoeAttackPattern`, `NormalAttackPattern`을 분리. 보스는 페이즈 전환 시점에 `attackPatterns` 리스트 자체를 교체해 런타임에 행동 세트를 바꿉니다.

```
Assets/Scripts/Enemy/IAttackPattern.cs
Assets/Scripts/Enemy/AttackPattern/
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

## 프로젝트 구조

```
Assets/Scripts/
├── Enemy/           # 적 AI, 보스, 공격 패턴 (Strategy Pattern)
│   ├── AttackPattern/
│   └── EnemyData/
├── Player/          # 플레이어 컨트롤, 스킬, 무기 전환
├── Room/            # 던전 구조, 포탈
├── Ingredient/       # 재료 수집·풀링
├── Recipe/          # 요리 시스템
├── Upgrade/         # 강화 시스템
├── UI/               # 결과창, 일시정지
├── HUD/
├── Loby/
└── GameCore/         # GameManager, SaveManager, SoundManager, AdManager, IAPManager
```


## 향후 계획

- 로비 UI 구현
- 던전 추가 개발
- 밸런싱 및 Google Play 출시 준비 (2026.10 목표)
