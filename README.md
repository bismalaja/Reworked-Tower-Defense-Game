# Simple Tower Defense

A compact desktop tower-defense game made with Unity 6. Its gameplay implementation was created as a
small, readable project using fourteen focused C# scripts, two custom scenes and eight custom prefabs.
It includes camera controls, scoring, JSON persistence, audio, particles and a complete UI flow.

The project uses a limited collection of licensed third-party model meshes and audio clips. Their sources
are described in the Assets section below and in the README retained with the DevAssets tower pack. They
do not include this project's gameplay code, scenes, prefab construction, UI, balancing, materials, save
system or documentation.

## Requirements

- Unity Hub
- Unity Editor `6000.3.22f1`
- Windows Build Support when producing the assessed executable
- Git LFS when cloning through Git

## Open and run

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

## Create the Windows executable

1. Open **File → Build Profiles**.
2. Select **Windows** and switch platform if required.
3. Confirm `MainMenu` and `Game` are enabled in that order.
4. Build to `Builds/Windows`.
5. Submit the entire generated folder, not only `SimpleTowerDefense.exe`.

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
