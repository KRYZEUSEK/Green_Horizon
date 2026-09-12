# Green Horizon

A 3D neighbourhood management sim made in Unity by a ten-person student team from SKN Game Development
(Poznań University of Economics and Business) for **Cyberiada — Poland's national student game development
championship**, 2024/2025.

You play the chair of a Polish housing estate. Decisions arrive as cards that you drag with the mouse.
Every choice raises some of the estate's indicators and lowers others, there is no option that is simply
good, and random events test how well you have kept things in balance. The estate reacts to how you run it:
neglect brings litter and gloomier visuals, good management brings greenery and busier streets.

## Running the game

There is no prebuilt release yet, so the game runs from the Unity editor.

1. Install [Unity Hub](https://unity.com/download) and the **Unity 2022.3.9f1** editor
   (Unity Hub → *Installs* → *Install Editor* → *Archive*, or the
   [Unity download archive](https://unity.com/releases/editor/archive)).
2. Clone the repository. It is large (about 360 MB), so this takes a moment:

   ```bash
   git clone https://github.com/KRYZEUSEK/Green_Horizon.git
   ```

3. In Unity Hub: *Projects* → *Add* → *Add project from disk* → select the cloned folder.
   The first import can take several minutes.
4. Open `Assets/Scenes/MainMenu.unity` and press **Play**.

Scenes included in the build, in order:

| Scene | Purpose |
|---|---|
| `MainMenu` | Main menu, starting point |
| `Alfa` | The main game |
| `FailScene` | Shown when the estate fails |
| `WinScene` | Shown after a successful term |

## Controls

- **Mouse** — drag decision cards to choose; interact with menus and minigames.

## Features

- Card-based decisions that trade one estate indicator against another.
- Random event cards that cannot be planned for.
- An estate that changes visually with your indicators — traffic, people, litter, fog.
- Short minigames: stamping papers, typing and catching falling items.
- Main menu with settings, plus win and fail endings.

## Project structure

```
Assets/Scenes/     Game scenes (MainMenu, Alfa, FailScene, WinScene) and team working scenes
Assets/Scripts/    Cards, camera, traffic and pedestrians, minigames, time and indicator bars, menus, sound
Assets/Prefabs/    Reusable game objects
Assets/Audio/      Music and sound effects
```

## Team

A ten-person team: four artists, one sound designer and five programmers.
