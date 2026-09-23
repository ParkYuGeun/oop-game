# BladeMan Survival

> **뱀파이어서바이버를 모티브로 한 로그라이크 검사 게임**  
> 2024년 개봉한 귀멸의칼날 극장판을 보고 총 대신 검을 휘두르는 로그라이크 게임을 기획·제작하고 싶어졌음

---

## 📸 Demo & Screenshots

| 메인 플레이 화면 |
| :---: | 
| ![스크린샷1](./screenshot/BS1.png) |
| ![스크린샷2](./screenshot/BS2.png) |
| ![스크린샷3](./screenshot/BS3.png) |

---

## ✨ Key Features

- **사용자 친화적인 UI**: 직관적인 HUD(체력, 경험치바, 진행 시간) 설계 및 레벨업 시 게임 일시정지와 함께 명확한 카드 선택 UI 제공, 플레이어 상태 변화 및 스탯 업데이트가 UI에 즉각적이고 부드럽게 반영되도록 구현
- **레벨시스템과 강화된 적과 보상**: 플레이 시간(Wave) 및 레벨에 따라 적의 스폰율, 체력, 공격력이 체계적으로 상승하는 동적 난이도 조절 시스템 구축
- **오브젝트 풀링 및 선택적 비활성화 기술을 활용한 최적화**: 수백 개 이상의 적 NPC, 탄막, 경험치 보석에 범용 오브젝트 풀링(Object Pooling)을 적용, 카메라 뷰포트 영역 밖의 개체 및 멀리 떨어진 적의 연산/렌더링을 선택적으로 비활성화(Culling)하여 메모리 재할당 및 GC(Garbage Collector) 발생을 최소화
- **자체제작 2d 애니메이션**: Aseprite를 활용해 캐릭터, 적 NPC, 무기 이펙트의 프레임 단위 픽셀 아트 및 스프라이트 시트(Sprite Sheet) 직접 제작

---
## 💻 Languages and Tools
[![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Aseprite](https://img.shields.io/badge/Aseprite-7D5260?style=for-the-badge&logo=aseprite&logoColor=white)](https://www.aseprite.org)
---

## 🛠 Tech Stack & Environment

| 구분 | 내용 |
| :--- | :--- |
| **Engine** | Unity 2021.3.45f1 |
| **Render Pipeline** | Universal Render Pipline(URP) |
| **Target Device** | Mobile(android) |
| **SDK / Framework** | Input System Package |
| **Language** | C# |
| **IDE** | Visual Studio|

---


## 📁 Project Structure

```text
Assets/
├── Core/               # 핵심 게임 매니저 및 시스템 스크립트
├── Prefabs/            # 총기, 적 AI, UI 프레합
├── Scenes/             # 메인 게임 및 테스트 씬
├── Scripts/            # C# 로직 스크립트
└── Shaders/            # Custom Shader Graph 파일

