# Simple Tower Defense

A compact desktop tower-defense game made with Unity 6. Its gameplay implementation was created as a
small, readable project using fourteen focused C# scripts, two custom scenes and eight custom prefabs.
It includes camera controls, scoring, JSON persistence, audio, particles and a complete UI flow.

The project uses a limited collection of licensed third-party model meshes and audio clips. Those files
are identified separately in [`ASSET_ATTRIBUTION.md`](ASSET_ATTRIBUTION.md); they do not include this
project's gameplay code, scenes, prefab construction, UI, balancing, materials, save system or
documentation.

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
- [Third-party asset attribution](ASSET_ATTRIBUTION.md)

## Assets and licences

The nine model mesh files in `ImportedArt`, two music tracks and eleven sound-effect files in `Audio`
originate from Unity Technologies' Tower Defense Template. Their Asset Store metadata and the supplied
`Third-Party Notices.txt` are retained. See `ASSET_ATTRIBUTION.md` for the complete file-level boundary.

All C# gameplay code, scenes, prefab composition and configuration, project materials, UI, balancing,
save system, particle setup and project documentation were created specifically for Simple Tower
Defense and are not attributed to Unity Technologies.
