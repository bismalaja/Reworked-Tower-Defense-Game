# Simple Tower Defense

This folder contains the project-specific gameplay implementation for Simple Tower Defense. Its scripts,
scenes, prefab configurations, materials, UI and balancing form a compact beginner-friendly architecture.
A limited set of licensed third-party mesh and audio files is kept in clearly identified folders.

## How to play

1. Open `Scenes/MainMenu.unity` and press Play.
2. Click Play in the menu.
3. Move the camera with WASD/arrow keys and zoom with the mouse wheel.
4. Click a blue build pad and buy one of the three towers.
5. Press Start Wave.
6. Click a built tower to upgrade or sell it.
7. Protect the base through all five waves. Escape pauses the game.

## Game scope

- One desktop level with a fixed waypoint path
- Three towers: Machine Gun, Laser and Rocket
- One upgrade per tower and a sell option
- Three enemy types: Basic, Fast and Tank
- Five waves, credits, score, saved progression and base health
- Music, sound effects, particles and short scene fades
- Settings, pause, victory, defeat, restart and a main menu

## Runtime scripts

| Script | Responsibility |
| --- | --- |
| `GameManager` | Currency, base lives, pause and win/lose state |
| `WaveManager` | The five waves and enemy spawning |
| `Enemy` | Waypoint movement, health, rewards and base damage |
| `BuildSpot` | Buying, upgrading and selling on a fixed pad |
| `Tower` | Target search, aiming and all three tower styles |
| `Projectile` | Bullet movement and rocket splash damage |
| `GameUI` | HUD panels and button actions |
| `MainMenu` | Play and quit buttons |
| `CameraController` | Smooth movement, zoom and map boundaries |
| `SaveData` | The six values stored in JSON |
| `SaveSystem` | Load, save, validation and progress reset |
| `AudioManager` | Persistent music, SFX and volume settings |
| `EffectsManager` | Four short particle effects |
| `SceneFader` | Fade transitions around scene loading |

The classes use direct Inspector references and ordinary methods on purpose. There is no pooling,
generic event framework, singleton inheritance hierarchy, alignment system or launcher inheritance tree.

## Where to make changes

- Starting credits and base lives: `Game Manager` in `Game.unity`
- Enemy health, speed, reward and base damage: the three enemy prefabs
- Tower range, damage, fire rate and upgrade values: the three tower prefabs
- Enemy counts and spawn delays: `Wave Manager` in `Game.unity`
- Tower prices: the `Build Spot` objects in `Game.unity`
- Camera speed, zoom and limits: `Main Camera` in `Game.unity`
- Music and SFX volume: the Settings screen or `save.json`

All required scene references are already connected in the Inspector.

## Third-party asset attribution

The nine FBX mesh files under `ImportedArt` and the thirteen files under `Audio` are licensed content
from Unity Technologies' Tower Defense Template. Their original Asset Store metadata is retained. Keep
`Third-Party Notices.txt` and comply with the applicable Unity Asset Store licence while those files
remain in the project. See the root `ASSET_ATTRIBUTION.md` for the complete list.

The enemy/tower prefabs that reference those meshes were constructed for this project; they are not
prefabs copied from the source package. The gameplay code, scenes, UI, project materials, balance data,
save system, particles and documentation are also project-specific work.
