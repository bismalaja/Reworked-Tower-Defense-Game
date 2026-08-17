# Simple Tower Defense — Asset Attribution

This document defines the authorship boundary between Simple Tower Defense and its licensed third-party
content.

## Unity Technologies assets

The following files originate from Unity Technologies' **Tower Defense Template**, Asset Store package
version 2.0. Their Unity `AssetOrigin` metadata is intentionally retained, and the files remain governed
by the applicable Unity Asset Store licence.

### Model meshes

- `Assets/SimpleTowerDefense/ImportedArt/Models/Units/Buggy.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Units/Helicopter.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Units/HoverTank_Base.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Towers/MachineGun/Base_MachineGun_L01.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Towers/MachineGun/Turret_MachineGun_L01.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Towers/Laser/LaserTower_BASE_L01.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Towers/Laser/LaserTower_TURRET_L01.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Towers/Rocket/Base_RocketTower_L01.fbx`
- `Assets/SimpleTowerDefense/ImportedArt/Models/Towers/Rocket/Turret_RocketTower_L01.fbx`

### Audio content

- `Assets/SimpleTowerDefense/Audio/Music/Game Music.mp3`
- `Assets/SimpleTowerDefense/Audio/Music/Menu Music.mp3`
- `Assets/SimpleTowerDefense/Audio/SFX/Base Attack.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Build.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Button.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Defeat.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Enemy Death.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Laser Fire.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Machine Gun Fire.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Rocket Fire.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Sell.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Upgrade.wav`
- `Assets/SimpleTowerDefense/Audio/SFX/Victory.wav`

The third-party notice supplied with those assets is retained at
`Assets/SimpleTowerDefense/Third-Party Notices.txt`.

## Simple Tower Defense project work

The following parts were created specifically for this project and are not attributed to Unity
Technologies:

- All fourteen C# gameplay and support scripts under `Assets/SimpleTowerDefense/Scripts`
- The `MainMenu` and `Game` scenes
- All eight prefab compositions and their gameplay configuration
- Project materials, level layout, waypoints and build-pad arrangement
- UI layout, menu flow, HUD, pause screen, settings and end screens
- Camera controls, combat rules, tower/enemy statistics and five-wave balancing
- JSON save system, score system, audio-control code, particle setup and scene fades
- The README, Game Design Document and Technical Report

The prefabs reference the licensed mesh files listed above, but their hierarchy, components, values and
gameplay behaviour are part of the Simple Tower Defense project implementation.
