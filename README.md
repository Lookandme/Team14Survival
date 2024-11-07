#  프로젝트 명 : Team14Survival
스파르타 유니티_6기 14사단 전우조 팀 프로젝트 

## 📖 목차
1. 프로젝트 소개
2. 멤버 구성
3. InputSystem 설정
4. 주요기능
5. 트러블슈팅


## 👨‍💻프로젝트 소개 
- 개요 : 종말한 세상의 유일한 생존자가 되어 괴들을 물리치는 게임
- 장르 : 🏕서바이벌


## 👥멤버 구성
 - 🪖팀장 : 이호균
 - 🪖팀원 : 김경구, 박기찬, 김관영
<br>


## 🎮InputSystem 설정 
- 👣 이동 [wasd] 🏃 점프 [space]
- 💼 인벤토리 [tab] 🔎 상호작용 키 [E]
- 🪓 공격 [마우스 좌클릭] 🔧 아이템 제작탭 [I]
- 👷‍♂️ 건축 탭 [F]  👨‍🔧건축 항목 회전 [Z]
- 🔙 ESCAPE키 [ESC]
<br>

## 👤플레이어
- 직접 조종하는 캐릭터입니다. 플레이어는 체력 허기 스테미나 수분 상태를 가지고 있습니다.
- 허기와 수분은 매 프레임마다 줄어들고 허기와 수분이 바닥나면 체력과 스테미나가 줄어들게 됩니다.

## 👿악마 ( 몬스터 )
- 이 게임의 주적입니다. 소환되었을 때 주변을 탐색하는 행동을 보입니다. 
- 플레이어를 발견하면 추적하며 사거리에 들어오면 플레이어를 공격합니다.
- AI네비게이션 기능을 사용하여 갈 수 있는 곳이라면 어디든 플레이어를 쫒을겁니다.

## 🔦아이템
- 아이템은 무기와 음식 그리고 자원 크게 세 종류로 구성하였습니다.
- ⚔ 무기 아이템을 장착 시 적을 공격할 수 있고 일부 무기는 자원을 채취하는 것이 가능합니다.
- 🥖음식 아이템은 플레이어의 체력과 같은 상태 자원에 영향을 줍니다.
- 💎자원 아이템으로 아이템을 제작할 수 있으며 자원 아이템은 광맥을 캐거나 나무를 베는 등의 활동으로 수급이 가능합니다.

## ⚒아이템 제작
- I를 눌러 제작탭을 열게되면 좌측에 조합식을 볼 수 있습니다.
- 충분한 재료가 있다면 조합식의 결과항목을 눌러 아이템 제작이 가능합니다.

## 👷‍♂️건축 시스템
- 건축할 오브젝트를 선택하고 지면에 배치가 가능합니다. Z키를 눌러 건축할 대상을 회전시킬 수 있습니다.
- 건축이 가능한 곳이라면 건축 대상이 초록색으로 보이게 되고 불가능하다면 빨간색을 띄게 됩니다.

## 🕒자원 재생성
- 자원 아이템이 고갈되는 것을 막기 위해 주기적으로 자원을 생성하는 영역입니다.
- 플레이어가 영역안에 있으면 자원 생성이 되지 않으며 일정 시간마다 자원이 재생성되게 합니다.
- 생성할 자원 오브젝트와 생성되는 영역의 크기, 플레이어 감지범위, 재생성 시간 등의 설정이 가능합니다.
- 생성하는 자원은 미리 생성해두고 비활성화하였다가 필요할 때 활성화하는 오브젝트풀링 방식이 사용되었습니다.


## ❗트러블 슈팅
### ⚙AI네비게이션 문제
- AI네비게이션 기능을 사용하면서 몬스터의 움직임이 부자연스러워지는 문제점 발생했습니다.
- 원인 : isStopped는 물리적 영향을 안 받는게 아니라서 속도가 있을경우 미끄러짐 현상이 발생합니다. 
- 해결방안 : agent.stoppingDistance = attackDistance 로 변수 값을 초기화하여 isStopped의 역할을 대신 하였습니다.
- 공격상태가 업데이트 되는 부분에 agent.velocity를 0으로 설정했습니다. 
- 에이전트가 멈추는 거리 설정을 공격 사거리로 설정하여 자연스럽게 연계되게 하였고 추가적인 코드 작성도 줄일 수 있었습니다. 
