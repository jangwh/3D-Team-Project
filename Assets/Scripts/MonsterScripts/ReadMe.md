# 몬스터 스크립트 사용 설명서

이 문서는 'MonsterScripts' 폴더에 포함된 스크립트들의 사용법과 몬스터 설정 방법에 대해 설명합니다.

## 1. 스크립트 구조
Monster.cs : 몬스터가 가지는 메인 스크립트입니다!
MonsterHitboxController.cs : Monster.cs가 있는 오브젝트에 같이 붙입니다. 몬스터의 히트박스 정보를 가지고 있고 스킬 스크립터블 오브젝트에게 정보를 받아서 히트박스를 활성화하고 끕니다.
DamageOnTrigger.cs : 각 히트박스 콜라이더가 있는 오브젝트에 붙입니다. 데미지 정보를 받아서 상대에게 데미지를 줍니다.

AttackPatternSO : 스킬 스크립터블 오브젝트의 부모 스크립트 입니다! 스킬의 정보를 담고 있습니다!
MeleePatterSO : 히트박스를 이용한 공격은 이 스크립트로 스크립터블 오브젝트를 만들면 됩니다.

## 2. 몬스터 프리펩 설정
필요한 컴포넌트 : Animator, NavMeshAgent, Rigidbody, Monster.cs, MonsterHitboxController.cs, Collider(몬스터끼리의 밀어내는 용)
rigidbody는 freeze rotation 모두 체크해 주세요
자식에는 Istrigger 체크된 콜라이더로 피격판정을 담당합니다.
특정 부분에 있는 Istrigger 체크된 콜라이더는 공격판정을 담당하는 히트박스 콜라이더입니다. DamageOnTrigger.cs를 붙여주세요


## 3. 새로운 공격 스킬 추가 방법
애니메이션 트리거 이름을 똑같이 적어주세요
MonsterHitboxController.cs에 등록된 히트박스 이름들중 사용할 히트박스의 이름을 적어주세요