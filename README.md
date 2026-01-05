# Dark Ring
<img width="616" height="335" alt="Image" src="https://github.com/user-attachments/assets/c3c4f0b4-f0d6-4312-ba14-ca07bca75de7" />

#  DarkRing
[3인 개발 프로젝트] 

[다크링 플레이 영상](https://youtu.be/UxYZ_Yr1PTE)

## 1. 프로젝트 개요

엘든링 레퍼런스로 만든 3D 액션 RPG입니다.

Unity를 활용하여 3D로 제작하였습니다

개발기간 : 2025.08.06 ~ 2525.08.25

## 2. 주요 기능
   
-----------------------------------------------------------
장우형 개발파트

### 2.1 플레이어

### [Player.cs](https://github.com/jangwh/3D-Team-Project/blob/main/Assets/Scripts/Player/Player.cs)

#### 💡 역할

* Player는 체력·사망·부활·사운드·UI 연동까지 책임지는 플레이어 생명주기 관리 클래스입니다.

* Character 추상 클래스를 상속받아 전투 및 상태 관리 로직을 구현하였습니다.

#### 📌 주요 메서드

* Init() : 플레이어 재시작(부활) 시 상태 초기화

### [PlayerLockOn](Assets/Scripts/Player/PlayerLockOn.cs)

#### 💡 역할

* PlayerLockOn은 전투 중 적 탐색, 카메라 시점 고정, 타겟 전환, UI 마커 연동을 담당하는 락온 시스템입니다.

#### 📌 주요 메서드

* TryLockOn() : 지정 반경(lockOnRange) 내 적 콜라이더 탐색, EnemyTarget 컴포넌트가 있는 객체만 필터링 후에 거리 기준으로 가장 가까운 적 우선 정렬하고 최초 타겟으로 락온 시도

* LockTo(EnemyTarget target) : 현재 락온 대상 설정 후 락온 상태 활성화 하고 Cinemachine Virtual Camera의 LookAt을 타겟 기준점으로 전환

### [WeaponSwapAndAttack](Assets/Scripts/Player/WeaponSwapAndAttack.cs)

#### 💡 역할

* WeaponSwapAndAttack은 무기 교체, 일반/강 콤보 공격, 가드 및 특수 공격을 입력 버퍼와 애니메이션 이벤트 기반으로 통합 관리하는 전투 시스템입니다.

#### 📌 주요 메서드

* HandleWeaponSwap() : Tab 키 입력 시 무기 순환 교체

* SetWeapon(int index) : 무기 데이터에 따른 Animator Controller 교체

* HandleAttackInput() : 공격 중 추가 입력은 입력 버퍼에 저장

* StartAmberCombo() : 콤보 시작 초기화, 첫 콤보 애니메이션 재생

* PlayAmberComboAnimation() : 일반 공격 콤보 애니메이션 실행

* OnComboAnimationEnd() : 입력 버퍼가 있을 경우 다음 콤보로 진행, 없으면 콤보 종료

* UpdateInputBuffer() : 입력 버퍼 타이머 감소, 제한 시간 초과 시 입력 무효화

* ResetCombo() : 입력 버퍼 초기화

### [GameManager](Assets/Scripts/GameManager.cs)

#### 💡 역할

GameManager는 다크링 인게임에서 플레이어 생명주기, 카메라, 월드 상태를 통합 관리하는 핵심 흐름 제어 매니저입니다.

#### 📌 주요 메서드

* Revive() : 플레이어 사망 후 부활 로직 처리

* SetPlayerReferences(Player playerObj) : 플레이어-카메라-전투 시스템 간 참조 재설정

### 2.2 게임시스템


### 2.3 상점


-----------------------------------------------------------

## 3. 플로우 차트 및 다이어그램

3.1 플로우차트

<img width="3564" height="1407" alt="Image" src="https://github.com/user-attachments/assets/9a14ff6e-ed61-493b-82e4-477dbd02be42" />

-----------------------------------------------------------

3.2 클래스 다이어그램

플레이어 - 다이어그램

<img width="714" height="473" alt="Image" src="https://github.com/user-attachments/assets/dbdf6b5c-1c1d-4b0b-8f89-864bcc688165" />

-----------------------------------------------------------

게임시스템 - 다이어그램

<img width="1129" height="367" alt="Image" src="https://github.com/user-attachments/assets/2c2abe7c-bfd4-492c-9345-15cf2b7fa049" />

-----------------------------------------------------------

상점 - 다이어그램

<img width="670" height="351" alt="Image" src="https://github.com/user-attachments/assets/6e384c2b-49fa-4a31-abb8-e769d191c212" />

-----------------------------------------------------------

## 4. 기술 스택
   
* C#
* Unity
* Fork + Github(형상 관리)

-----------------------------------------------------------
장우형 스택파트

* Queue 를 사용하지 않고 제작한 연속 공격
* Chinemachine 카메라를 이용하여 시점 조정
* Async를 활용하여 로딩 씬 전환
-----------------------------------------------------------
