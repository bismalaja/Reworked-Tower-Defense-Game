# Simple Tower Defense — Technical Report

## Project information

- Student: `Biorni Ismalaja`
- Course: `Programim i Lojrave`
- Project: `Simple Tower Defense`
- Instructor: `Egers Braho`
- Academic year: `2025 - 2026`
- Unity version: `6000.3.22f1`
- Render pipeline: Built-in Render Pipeline
- Input backend: Unity Input System package `1.20.0`
- UI: Unity UI (`uGUI`)
- Primary namespace: `SimpleTowerDefense`

## Technical objective

Simple Tower Defense uses a project-specific runtime implementation designed for a small educational
game. The architecture uses direct Inspector references, serializable values and ordinary method calls.
It avoids pooling frameworks, inheritance-heavy launcher systems, service containers and generic event
layers so a new Unity developer can trace each gameplay action from input to result. A limited set of
licensed third-party mesh and audio assets is used as content. No third-party runtime framework or
gameplay scripts are part of this implementation.

## Runtime architecture

| Script | Responsibility |
| --- | --- |
| `GameManager` | Credits, base health, score, pause state, victory/defeat and scene actions |
| `WaveManager` | Five serialized waves, spawning and live-enemy counting |
| `Enemy` | Waypoint movement, health, rewards, base damage and destruction feedback |
| `BuildSpot` | Fixed-pad buying, upgrading and selling |
| `Tower` | Target search, rotation and three attack behaviours |
| `Projectile` | Homing movement, sphere-cast collision and rocket splash damage |
| `CameraController` | Smooth keyboard movement, mouse-wheel zoom and camera limits |
| `GameUI` | HUD, selection panels, pause panel and end panel |
| `MainMenu` | Menu navigation, settings sliders and reset confirmation |
| `SaveData` | Six serializable save values |
| `SaveSystem` | JSON loading, saving, validation and progress reset |
| `AudioManager` | Persistent music, saved volume and named sound-effect methods |
| `EffectsManager` | Bullet, rocket, enemy and building particle bursts |
| `SceneFader` | Unscaled-time UI fades around scene loading |

The final runtime layer is approximately 1,700 lines across fourteen scripts.

## Important runtime flow

1. `MainMenu` reads `SaveSystem.Data` and displays saved progress.
2. `SceneFader` fades out and loads the `Game` scene.
3. `GameManager` initializes credits, health and score and records a played game.
4. A mouse raycast in `GameManager.Update` selects a `BuildSpot` or its `Tower`.
5. `WaveManager` instantiates enemies and supplies the waypoint array.
6. Towers search `Enemy.ActiveEnemies` and attack the nearest target within range.
7. `Enemy.Finish` informs `WaveManager`; the next wave becomes available when spawning has stopped and
   the live count reaches zero.
8. Victory or defeat writes progress and opens the end panel.

## Input system

The project uses the new Input System directly:

- `Keyboard.current` reads WASD, arrow keys and Escape.
- `Mouse.current` reads left-click position and scroll-wheel input.
- `Physics.Raycast` converts a left click into a world selection.
- `EventSystem.current.IsPointerOverGameObject` prevents clicks on UI from selecting the world behind it.

Direct device access is sufficient for this small desktop-only project and is easier to follow than a
generated input wrapper. The scene EventSystems use `InputSystemUIInputModule` for buttons and sliders.

## Camera controller

`CameraController` stores a target position and target field of view. Keyboard input changes the target
position, and `Vector3.SmoothDamp` produces smooth motion. Mouse-wheel input changes the target field of
view through `Mathf.SmoothDamp`. Serialized X/Z limits keep the camera close to the level. Unscaled delta
time allows the camera and scene fade to remain responsive while gameplay is paused.

## Projectile and collision handling

Projectiles home toward an assigned `Enemy.TargetPoint`. Each frame they use `Physics.SphereCast` across
their travel distance, preventing a fast projectile from skipping through a collider between frames.
If the target is closer than the frame travel distance, a direct impact is also accepted.

- Bullets apply damage to the contacted enemy and request a bullet-impact particle burst.
- Rockets call `Physics.OverlapSphere` at the impact point. A `HashSet<Enemy>` prevents an enemy with
  multiple colliders from receiving duplicate splash damage.
- Lasers apply immediate damage and briefly enable a `LineRenderer`.
- World selection uses a separate camera raycast.

Enemy movement uses deterministic waypoint movement rather than NavMesh, which is appropriate for a
fixed tower-defense road.

## Save system

The save file is written to:

```text
Application.persistentDataPath/save.json
```

Example:

```json
{
  "highScore": 10320,
  "highestWaveReached": 4,
  "gamesWon": 0,
  "gamesPlayed": 8,
  "musicVolume": 0.8,
  "sfxVolume": 0.8
}
```

`SaveSystem` lazily loads the file with `JsonUtility`. Missing files create default values. Invalid or
unreadable JSON is caught and replaced with safe defaults instead of stopping gameplay. Numeric progress
cannot be negative and volumes are clamped from 0 to 1.

The system saves when a game starts, a new highest wave is reached, a game finishes, a high score changes,
volume changes, progress is reset, the player returns to the menu, or the application closes.

## Audio implementation

Each scene contains a configured `AudioManager`. The first instance uses `DontDestroyOnLoad`; duplicates
destroy themselves. One `AudioSource` loops music and one plays overlapping SFX with `PlayOneShot`.
Scene-load callbacks switch between menu and gameplay music. Slider values are immediately written to the
JSON save file.

The audio content uses two licensed music tracks and eleven licensed sound-effect files. Audio playback,
persistence, scene switching and volume control are implemented by this project's `AudioManager`; no
third-party audio framework or mixer is used.

## Particle implementation

`EffectsManager` uses one helper method to configure a short non-looping `ParticleSystem`. Size, color and
particle count produce four variations. `ParticleSystemStopAction.Destroy` removes completed effects, so
no manual timer script is necessary.

## Scenes and prefabs

### MainMenu scene

- Main camera and light
- Main menu canvas and Input System EventSystem
- Main, settings and reset-confirmation panels
- Persistent audio manager
- Full-screen fade overlay

### Game scene

- Perspective camera with `CameraController`
- Eight path waypoints and seven fixed build spots
- `GameManager`, `WaveManager`, `EffectsManager` and audio manager
- HUD, build/tower panels, pause panel and end panel
- Full-screen fade overlay

### Prefabs

- Enemies: `BasicEnemy`, `FastEnemy`, `TankEnemy`
- Towers: `MachineGunTower`, `LaserTower`, `RocketTower`
- Projectiles: `Bullet`, `Rocket`

Balance values are serialized in the enemy/tower prefabs and wave composition is serialized in the Game
scene, allowing Inspector tuning without editing code.

## Installation and configuration

1. Install Unity Hub.
2. Install Unity Editor `6000.3.22f1` with Windows Build Support.
3. Clone or extract the complete project folder.
4. If using Git, install Git LFS before checkout because FBX and audio assets use LFS.
5. Add the folder in Unity Hub and open it with the specified editor version.
6. Wait for asset and script import to finish.
7. Open `Assets/SimpleTowerDefense/Scenes/MainMenu.unity`.
8. Press Play.

## Windows build instructions

1. Open **File → Build Profiles**.
2. Select or add **Windows** and switch to it.
3. Confirm both scenes are enabled, with `MainMenu` first and `Game` second.
4. Use x86-64 architecture.
5. Choose **Build** and select `Builds/Windows`.
6. The deliverable must include the executable, `SimpleTowerDefense_Data`, `UnityPlayer.dll` and all other
   files Unity places beside them.

## Testing checklist

- Main menu Play, Settings, Back, Reset confirmation and Quit work.
- Saved high score and highest wave survive a complete application restart.
- Music and SFX sliders change volume and persist.
- WASD/arrows move the camera; mouse wheel zooms; boundaries hold.
- UI clicks do not select build pads behind the interface.
- All three towers build, attack, upgrade and sell.
- All three enemy models render; Tanks have visible spacing.
- Bullet collision and rocket splash damage work.
- Five waves complete and the live enemy counter returns to zero.
- Pause/resume works through Escape and the button.
- Victory, defeat, restart and return-to-menu work.
- Fade, music, SFX and particles work in both scenes.
- Console contains no errors.
- Windows build launches and completes the main loop.

## Known scope limitations

- One level and five hand-authored waves
- Keyboard and mouse desktop controls only
- One local JSON save slot
- No cloud save, multiplayer, controller remapping or localization
- Windows executable should be validated on Windows or Wine/Lutris after cross-building on Linux

These are deliberate project-scope decisions rather than incomplete core requirements.

## Asset documentation

The Standard Turret, Laser Beamer and Missile Launcher models originate from the DevAssets tower-defense
asset pack. Its README and usage-guideline link are retained under
`Assets/SimpleTowerDefense/ThirdParty/DevAssetsTowerPack`. Three enemy mesh files, two music tracks and
eleven sound-effect files originate from Unity Technologies' Tower Defense Template. These third-party
files remain governed by their applicable licences.

The fourteen gameplay scripts, two scenes, eight prefab compositions, project materials, UI structure,
game rules and balancing, save system, camera controller, particle configuration and all project
documentation were created specifically for Simple Tower Defense. The prefab configurations use the
licensed meshes but were constructed and configured for this project.
