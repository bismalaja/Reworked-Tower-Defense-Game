# Simple Tower Defense

> [!IMPORTANT]
> **A ready-to-play Windows build is included in this repository.** You do not need Unity to play it.
> Download or clone the complete repository, open [`WindowsEXE_Playable`](WindowsEXE_Playable), and run
> **`Simple Tower Defense.exe`**. Keep the executable, `Simple Tower Defense_Data`, `UnityPlayer.dll`,
> and the other files together in that folder—the `.exe` will not work correctly by itself.

A compact desktop tower-defense game made with Unity 6. Its gameplay implementation was created as a
small, readable project using fourteen focused C# scripts, two custom scenes and eight custom prefabs.
It includes camera controls, scoring, JSON persistence, audio, particles and a complete UI flow.

The project uses a limited collection of licensed third-party model meshes and audio clips. Their sources
are described in the Assets section below and in the README retained with the DevAssets tower pack. They
do not include this project's gameplay code, scenes, prefab construction, UI, balancing, materials, save
system or documentation.

## Project information

- Student: **Biorni Ismalaja**
- Course: **Programim i Lojrave**
- Project: **Simple Tower Defense**
- Instructor: **Egers Braho**
- Academic year: **2025 - 2026**

## Play the Windows build

The playable build is already available under [`WindowsEXE_Playable/`](WindowsEXE_Playable):

```text
WindowsEXE_Playable/
├── Simple Tower Defense.exe       ← launch this file
├── Simple Tower Defense_Data/     ← required game data
├── UnityPlayer.dll                ← required Unity runtime
├── MonoBleedingEdge/              ← required runtime files
└── other supporting files
```

To play:

1. Download or clone the **entire repository**. Git users should have Git LFS installed before cloning.
2. Open the `WindowsEXE_Playable` folder.
3. Double-click `Simple Tower Defense.exe`.
4. Do not move or distribute the `.exe` without the rest of its folder.

The build targets 64-bit Windows. It can also be tested on Linux through Wine or Lutris. Because this is
a student-built executable and is not code-signed, Windows may display its standard unknown-publisher
warning on first launch.

## Requirements

These requirements are only necessary for opening or modifying the Unity project. They are **not** needed
when using the included Windows build.

- Unity Hub
- Unity Editor `6000.3.22f1`
- Windows Build Support when producing the assessed executable
- Git LFS when cloning through Git

## Open and run in Unity

1. Add this folder to Unity Hub.
2. Open it using Unity `6000.3.22f1`.
3. Wait for compilation and asset import.
4. Open `Assets/SimpleTowerDefense/Scenes/MainMenu.unity`.
5. Press Play.

Both required scenes are already present in Build Settings in the correct order.

## How to play

1. Select **Play** from the main menu.
2. Click a blue build pad.
3. Buy a Machine Gun, Laser or Rocket tower.
4. Press **Start Wave**.
5. Click a built tower to upgrade or sell it.
6. Protect the base through all five waves.

## Controls

| Input | Action |
| --- | --- |
| WASD / Arrow keys | Move camera |
| Mouse wheel | Zoom |
| Left mouse button | Select pads, towers and UI |
| Escape | Pause/resume |

## Features

- One level with five progressively harder waves
- Three towers and three enemy types
- Fixed build pads, one upgrade level and selling
- Credits, base health, score, wave progress and enemies remaining
- Smooth camera movement, zoom and boundaries
- Main menu, settings, pause, victory, defeat and restart flow
- JSON high score, highest wave, game statistics and saved volumes
- Menu/game music, tower/action SFX and four particle effects
- Short scene fades and responsive button colors

## Saved data

The game writes `save.json` under `Application.persistentDataPath`. It stores high score, highest wave,
games won, games played, music volume and SFX volume. Use **Settings → Reset Progress** to restore defaults.

## Rebuild the Windows executable

1. Open **File → Build Profiles**.
2. Select **Windows** and switch platform if required.
3. Confirm `MainMenu` and `Game` are enabled in that order.
4. Build to `Builds/Windows`.
5. Submit the entire generated folder, not only `SimpleTowerDefense.exe`.

These instructions are for producing a fresh build. Most players should use the ready-made build in
`WindowsEXE_Playable` described at the top of this README.

Expected contents include:

```text
Builds/Windows/
├── SimpleTowerDefense.exe
├── SimpleTowerDefense_Data/
├── UnityPlayer.dll
└── supporting runtime files
```

## Documentation

- [Game Design Document](GDD.md)
- [Technical Report](TECHNICAL_REPORT.md)
- [Script and balancing guide](Assets/SimpleTowerDefense/README.md)

## Assets

The three tower models are from the DevAssets tower-defense asset pack: Standard Turret, Laser Beamer
and Missile Launcher. The pack README and usage-guideline link are retained under
`Assets/SimpleTowerDefense/ThirdParty/DevAssetsTowerPack`.

The three enemy mesh files in `ImportedArt/Models/Units`, two music tracks and eleven sound-effect files
in `Audio` originate from Unity Technologies' Tower Defense Template. The third-party files are licensed
content only; the gameplay scripts, scenes, prefab construction, UI, balance and supporting systems are
project-specific work.
