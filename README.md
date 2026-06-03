# Our Last Walk

> *Some walks lead us home. Some lead us to let go.*

**Our Last Walk** is a first-person, narrative-driven puzzle game about a ghost and his dog. Mr. Rich Thorne wakes up in his locked-up house to find his hands transparent and his body lying motionless on the floor. He is dead, but not gone. Bound to the world by unfinished business he can't quite name, and unwilling to abandon his loyal companion Angel, he must work together with her to make his way through the house, piece together the life he lived, and find a way to let go.

A cozy yet uneasy escape-room experience about companionship, mutual dependence, loss, and understanding what truly matters before the last walk is over.

🌐 **Website:** https://ourlastwalk.vercel.app/

🎮 **Download the game:** https://github.com/yushinliou/Our-Last-Walk-Langing/releases/download/v1.0.0/final-build.zip

---

## Table of Contents

- [Overview](#overview)
- [Story](#story)
- [Characters](#characters)
- [Gameplay](#gameplay)
- [Controls](#controls)
- [Levels (Demo)](#levels-demo)
- [Technical Details](#technical-details)
- [Project Structure](#project-structure)
- [Building From Source](#building-from-source)
- [The Team — Lucky 7](#the-team--lucky-7)
- [Credits & Third-Party Assets](#credits--third-party-assets)
- [License & Copyright](#license--copyright)

---

## Overview

| | |
|---|---|
| **Genre** | Slice of Life / Puzzle / Escape Room |
| **Perspective** | First-person, 3D |
| **Platform** | PC (Windows / macOS) |
| **Engine** | Unity (Universal Render Pipeline) |
| **Players** | Single-player |
| **Setting** | A remote New England house, June 2nd 2009 |


---

## Story

The year is 2009. On a stormy morning in the New England countryside, Rich Thorne — a 58-year-old hedge-fund founder, long divorced and estranged from his adult daughter — wakes earlier than usual to the feeling of being watched. It's his dog, Angel, sitting in the corner, looking unusually sad. Rich rises feeling strangely light, then turns to see his own body lying motionless on the bed.

He is dead, and trapped in a house he obsessively kept locked. He will remain here until he settles his unfinished business — though he isn't sure what that business is. With Angel at his side, Rich moves room to room, reliving the choices that shaped his life and uncovering details that make him question how he really died.

Two goals drive the journey: find Angel a new family so she isn't trapped here, and discover what Rich left unfinished so he can finally move on. Each completed room rewards the player with a *memento* and a flashback that deepens the story.

---

## Characters

### 🟦 Mr. Rich Thorne — The Ghost
Reserved, gloomy, and guarded, Rich shows his soft side only to the rare few he cares about. As a ghost, he can:
- See through walls
- Possess and control electronics (lights, computers, phones, TV, radio)
- Read notes and inspect objects
- *Cannot* touch or move physical things

### ⬜ Angel — The Dog
A 9-year-old border collie — black and white, greying around the face. Smart, loyal, and still full of wit. As the dog, she can:
- Pick up and carry physical objects in her mouth
- Squeeze into tight spaces, jump, and climb
- Follow **scent trails** with her super sense of smell
- *Cannot* read notes or operate electronics (and is colorblind — sometimes a hindrance, sometimes her greatest asset)

> Neither can escape alone. Every puzzle requires using at least one unique ability from **each** character.

---

## Gameplay

The player explores the house in first person, switching between Rich and Angel at any time. The two characters share the scene at all times — switching activates one and deactivates the other, leaving the inactive one waiting in place. The core of every puzzle is the divide in what each character can interact with: one touches what the other cannot.

Each room contains a **main puzzle** unlocked by solving several smaller ones, which together reveal the solution. Completing a room awards a memento and a cutscene, then leads to the next room. A **Game State Manager** tracks puzzle progress and enforces the intended order — objects only become interactive once their prerequisites are met.

Two signature visual mechanics support the design:
- **Scent line** — a wavering trail drawn between Angel and a relevant object, visible only as the dog, that guides her toward puzzle-relevant items.
- **Bond line** — a line connecting ghost and dog whenever Rich is active, a reminder of their connection and a way to locate Angel.

---

## Controls

| Action | Input |
|---|---|
| Move | `W` `A` `S` `D` |
| Look | Mouse |
| Switch character (Ghost ⇄ Dog) | `Tab` |
| Jump | `Space` |
| Interact with prop (open phone, safe, read note, etc.) | `E` |
| Grab / carry object (Dog) — hold to bite, release to drop | `Left Shift` |
| Reveal scent trail (Dog) | `F` |
| Exit a focused interaction | `E` / `Esc` |

*The game uses Unity's Input System, so actions can also be mapped to a gamepad.*

---

## Levels (Demo)

The full game is planned for **7 rooms**; **3** are complete in this demo.

### 1. Bedroom 🛏️
Rich realizes he's dead and must escape his locked bedroom. As the ghost, listen through five voice memos on the phone for hidden dates; cross-reference the calendar to deduce the safe code. Inside the safe is one of Mia's childhood drawings. As the dog, slip under the bed to find a box of more drawings that tell a sadder story — count the family members across them to crack the door code. **Memento:** a broken, glued-together handmade mug.

### 2. Office 🗄️
The storm has scattered the office. Unlock the password-protected computer to read emails revealing Rich's relationships with his employees, then sort three photos into the correctly colored frames using the room's color-wheel clue (each month maps to a color). **Memento:** a name plaque from Legacy Inc's founding.

### 3. Kitchen 🍳
Angel is hungry. Find the recipe on the fridge and gather the correct ingredients from around the kitchen — some are duplicates, distinguished only by each character's special abilities — and deliver them to the bowl. **Memento:** Angel's puppy collar.

> Planned future rooms include the Living Room, Bathroom, Attic, and Backyard, where the story reaches its conclusion.

---

## Technical Details

- **Engine:** Unity with the **Universal Render Pipeline (URP)**, using a global post-processing volume for the warm, slightly desaturated interior look.
- **Input:** Unity's new **Input System** package, with all actions defined in a central input action asset.
- **Character system:** Both playable characters live in the scene simultaneously; switching with `Tab` toggles components rather than spawning anything. Interactables are gated by character **tag** (`Dog` / `Ghost`).
- **Interaction:** Proximity triggers (collider volumes) surface prompts; confirm with `E`. UI interactions pause the game via `Time.timeScale = 0`.
- **State management:** A singleton **Game State Manager** persists puzzle flags (e.g. `hasPaperCode`, `hasPhonePassword`, `safeUnlocked`) to enforce puzzle dependencies.
- **3D assets:** Built in **Blender**; version control via **Git/GitHub**.
- **Art direction:** Stylized realism with deliberately retro, lived-in props and crude low-resolution in-world UI evoking early-2000s consumer electronics.

---

## Project Structure

```
Our-Last-Walk/
├── My project/        # Unity project (open this in the Unity Editor)
├── build1/            # PC build
├── build 2/           # PC build
├── buildMac.app/      # macOS build
├── final-build/       # Final packaged build
├── .gitattributes
├── .gitignore
└── README.md
```

Gameplay scripts live under `My project/Assets/Scripts/` and include the character controllers (`Dog_movement`, `Ghost_movement`, `Camera_movement`, `Player`), interaction systems (`*ProximityTrigger`, `*Controller`, `*UIManager`), the puzzle props (paper, phone, safe, turntable, door), and the visual mechanics (`Bond`, `SmellLine`).

---

## Building From Source

**Requirements:** Unity (with URP and the Input System package) and Blender for editing 3D assets.

1. Clone the repository:
   ```bash
   git clone https://github.com/rippyboii/Our-Last-Walk.git
   ```
2. Open the `My project` folder in the Unity Hub / Editor (matching the project's Unity version).
3. Let Unity import packages and resolve dependencies.
4. Open the starting scene and press **Play**, or build for your target platform.

Prefer to just play? Download the packaged build from the [website](https://ourlastwalk.vercel.app/) or the [direct download link](https://github.com/yushinliou/Our-Last-Walk-Langing/releases/download/v1.0.0/final-build.zip).

---

## The Team — Lucky 7

*Five people. One shared vision. Countless late nights.*

| Name | Role | Links |
|---|---|---|
| **Anna Likhanova** | Developer, Narrative Designer & 2D Artist | [LinkedIn](https://www.linkedin.com/in/anna-likhanova-ba9478222/) · anna.likhanova2002@gmail.com |
| **Lexi Yu Shin Liou** | Narrative Designer & Developer | [LinkedIn](https://www.linkedin.com/in/yu-shin-liou-is-lexi/) · vxh440@gmail.com |
| **Apeel Subedi** | Developer & 3D Artist | [Website](https://apeels.com.np) · apeelsubedi@gmail.com |
| **Abidin Baran** | 3D Environment Artist | [LinkedIn](https://www.linkedin.com/in/abidin-baran-948965315/) |
| **Alfred Malmström** | Contributor | [LinkedIn](https://www.linkedin.com/in/alfred-malmstr%C3%B6m-954126290/) |

---

## Credits & Third-Party Assets

All narrative writing, voice-memo and email scripts, Mia's drawings, character designs, and original art are the work of the Lucky 7 team.

The following third-party asset packs from the Unity Asset Store were used during development, with thanks to their creators:

| Pack | Publisher |
|---|---|
| Animals — Free Animated Low Poly 3D Models | ithappy |
| Low-Poly Office Set 1 (VNB) | VNB Productions |
| Interior House Assets (URP) | Unity Technologies |
| Cute Furniture — Free Low Poly 3D Models | ithappy |
| Furniture Mega Pack (Free) | dlgames |
| 3D Props — Adorable Foods | LAYERLAB |

All third-party assets remain the property of their respective owners and are used under the terms of the Unity Asset Store EULA.

---

## License & Copyright

**© 2026 Lucky 7** — Anna Likhanova, Lexi Yu Shin Liou, Apeel Subedi, Abidin Baran, and Alfred Malmström.

All original game code, narrative, art, and design assets are the property of the Lucky 7 team. Third-party assets are governed by their respective licenses (see above). Please contact the team before reusing or redistributing any original content.

---

*Made with care and late nights. 🐾*
