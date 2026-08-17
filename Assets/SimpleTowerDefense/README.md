# Simple Tower Defense — Script and Balancing Guide

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

## Current gameplay balance

These values are the serialized settings currently used by the prefabs and `Scenes/Game.unity`. Values
shown here describe the actual game rather than the default values written in the scripts.

### Starting resources and game rules

| Setting | Current value | Location |
| --- | ---: | --- |
| Starting credits | 400 | `Game` scene → `Game Manager` → `Starting Currency` |
| Base health | 20 lives | `Game` scene → `Game Manager` → `Starting Lives` |
| Starting score | 0 | Initialized by `GameManager` |
| Build pads | 7 | Seven `Build Spot` objects in the `Game` scene |
| Waves | 5 | `Game` scene → `Wave Manager` → `Waves` |
| Waypoints | 8 | `Game` scene → `Wave Manager` → `Waypoints` |

Base health is stored by `GameManager`; the visible base does not have a separate health component. The
player loses when lives reach zero and wins after every enemy in wave five has either been destroyed or
reached the base.

There is no passive income. Defeating an enemy awards its `Reward` as credits and awards ten times that
amount as score. An enemy that reaches the base gives no credits or score.

### Tower statistics

Purchase prices are stored on every `BuildSpot`, while combat, upgrade and sale values are stored on the
tower prefabs.

| Tower | Build cost | Range | Damage/shot | Attacks/sec | Projectile speed | Splash radius | Upgrade cost | Sell value |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Machine Gun | 100 | 7.5 | 9 | 3.5 | 22 | 0 | 90 | 65 |
| Laser | 140 | 8.5 | 25 | 0.8 | Instant | 0 | 120 | 90 |
| Rocket | 180 | 9.5 | 30 | 0.65 | 14 | 2.3 | 150 | 115 |

The Machine Gun and Rocket launch homing projectiles. The Laser deals damage immediately. Rocket damage
is applied once to every enemy found inside the splash radius; multiple colliders on one enemy do not
cause duplicate damage.

Every tower has exactly one upgrade level. `Tower.Upgrade()` applies the same multipliers to all towers:

- Damage × 1.6
- Range × 1.15
- Attack speed × 1.2
- Rotating visual scale × 1.12

| Tower at level 2 | Damage/shot | Range | Attacks/sec | Approx. damage/sec |
| --- | ---: | ---: | ---: | ---: |
| Machine Gun | 14.4 stored, 14 applied | 8.63 | 4.2 | 58.8 |
| Laser | 40 | 9.78 | 0.96 | 38.4 |
| Rocket | 48 | 10.93 | 0.78 | 37.44 per enemy hit |

Enemy health uses integers, so each incoming hit is rounded to the nearest integer. Selling always gives
the prefab's fixed `Sell Value`; upgrading a tower does not increase its resale value.

### Enemy statistics

| Enemy | Health | Speed | Credit reward | Score reward | Base damage | Turn speed |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Basic | 45 | 3.2 | 22 | 220 | 1 | 8 |
| Fast | 30 | 4.7 | 25 | 250 | 1 | 8 |
| Tank | 120 | 2.1 | 45 | 450 | 2 | 8 |

All enemies follow the same eight-point path. `Speed` is measured in Unity units per second. `Base
Damage` is removed only if that enemy finishes the path.

### Wave composition

Groups spawn sequentially in the order shown. The delay is the number of seconds between enemies within
that group. A wave finishes only after spawning has ended and the live-enemy count reaches zero.

| Wave | Groups in spawn order | Total enemies | Available credits | Available score | Maximum leak damage |
| --- | --- | ---: | ---: | ---: | ---: |
| 1 — Getting Started | 6 Basic at 0.85 s | 6 | 132 | 1,320 | 6 |
| 2 — Faster | 8 Basic at 0.70 s; 2 Fast at 0.65 s | 10 | 226 | 2,260 | 10 |
| 3 — Heavy Units | 6 Fast at 0.55 s; 3 Tank at 1.00 s | 9 | 285 | 2,850 | 12 |
| 4 — Mixed Attack | 7 Basic at 0.50 s; 4 Fast at 0.45 s; 3 Tank at 0.85 s | 14 | 389 | 3,890 | 17 |
| 5 — Final Wave | 9 Basic at 0.40 s; 7 Fast at 0.40 s; 4 Tank at 0.75 s | 20 | 553 | 5,530 | 24 |

A perfect five-wave run defeats 59 enemies, earns 1,585 additional credits and produces a maximum score
of 15,850.

### Build-pad configuration

Each of the seven pads has:

- One `BuildSpot` component
- One `Tower Anchor` child that controls where the tower appears
- A three-item `Tower Prefabs` array
- A matching three-item `Tower Costs` array: `100`, `140`, `180`
- A Capsule Collider used for mouse selection
- A local scale of approximately `1.7 × 0.18 × 1.7`

Array order is important because `GameUI` buys towers by index:

| Array index | Tower | UI method |
| ---: | --- | --- |
| 0 | Machine Gun | `BuyMachineGun()` |
| 1 | Laser | `BuyLaser()` |
| 2 | Rocket | `BuyRocket()` |

## How to modify the balance

Always exit Play Mode before editing values. Changes made during Play Mode are normally discarded when
Play Mode ends.

### Change tower combat, upgrade cost or sell value

1. Open `Prefabs/MachineGunTower.prefab`, `LaserTower.prefab` or `RocketTower.prefab`.
2. Select the prefab root.
3. Edit the `Tower` component fields: Range, Damage, Attacks Per Second, Projectile Speed, Splash Radius,
   Upgrade Cost or Sell Value.
4. Save the prefab and test it against at least one early and one late wave.

Keep `Splash Radius` at zero for a single-target projectile. The Laser does not use Projectile Prefab,
Projectile Speed or Splash Radius. Avoid setting `Attacks Per Second` to zero because the attack timer
divides by this value.

The level-two multipliers are code values rather than Inspector values. Change them in
`Scripts/Tower.cs`, inside `Upgrade()`. This affects all three tower types.

### Change tower purchase prices

1. Open `Scenes/Game.unity`.
2. Find the seven objects named `Build Spot` and select all of them.
3. In `BuildSpot`, expand `Tower Costs` and change elements 0, 1 and 2.
4. Keep `Tower Costs` the same length and order as `Tower Prefabs`.
5. Update the three static purchase-button labels in the Build Panel, such as `Machine Gun (100)`.

The purchase labels do not read prices automatically. If only the array changes, gameplay will charge the
new amount while the UI continues displaying the old amount.

### Change enemy strength, speed or rewards

1. Open `Prefabs/BasicEnemy.prefab`, `FastEnemy.prefab` or `TankEnemy.prefab`.
2. Select the prefab root.
3. Edit the `Enemy` component: Max Health, Move Speed, Reward, Base Damage or Turn Speed.
4. Save and test that the model still follows corners cleanly and that towers can kill it reliably.

Changing `Reward` changes both income and score because `GameManager` calculates score as
`creditReward × 10`. To separate score from credits, add a separate score field to `Enemy` and pass it to
`RewardEnemyDefeat`.

### Change waves and spawn timing

1. Open `Scenes/Game.unity` and select `Wave Manager`.
2. Expand `Waves`, then expand the desired wave and its `Groups` array.
3. Assign an enemy prefab, set Count and set Time Between Spawns for each group.
4. Groups run sequentially. Reorder them if a different enemy type should appear first.
5. Keep at least one valid group in every wave and test the complete final wave after editing.

To add a sixth wave, increase the `Waves` array size and configure the new element. The HUD reads the
array length automatically, and victory occurs after the final array element is cleared.

### Change starting credits or base health

1. Open `Scenes/Game.unity` and select `Game Manager`.
2. Change `Starting Currency` or `Starting Lives`.
3. Enter Play Mode and confirm the HUD starts with the new values.

Starting lives are the base's entire health pool. There is no second health value on the base mesh.

### Change pads or tower placement

- Move a `Build Spot` object to relocate a pad.
- Move its `Tower Anchor` child to adjust the spawned tower position without moving the pad.
- Resize the pad's Capsule Collider if it is difficult to click.
- Duplicate a complete `Build Spot` object to add another usable pad, then verify all three prefab and
  cost entries were copied.

### Change projectile collision

Open `Prefabs/Bullet.prefab` or `Rocket.prefab` and edit `Projectile → Collision Radius`. Both currently
use `0.12`. Projectile lifetime is fixed at six seconds in `Scripts/Projectile.cs`. Projectile travel
speed and rocket splash size belong to the tower prefab rather than the projectile prefab.

### Change camera movement

Select `Main Camera` in `Scenes/Game.unity` and edit `CameraController`:

| Setting | Current value |
| --- | ---: |
| Move Speed | 12 |
| Move Smooth Time | 0.12 |
| Movement Limits | Enabled |
| Horizontal Limits | -7 to 7 |
| Depth Limits | -23 to -15 |
| Zoom Sensitivity | 0.05 |
| Zoom Smooth Time | 0.08 |
| Field-of-view limits | 38 to 70 |

### Balance-testing checklist

After changing balance values, verify:

- The player can afford at least one useful tower before wave one.
- All seven pads display and charge the intended prices.
- Each tower can acquire targets, rotate and attack without Console errors.
- Fast enemies remain targetable and Tank enemies remain visible.
- Rocket splash hits nearby enemies only once each.
- Credits and score increase by the expected reward.
- A leaking enemy removes the expected base health.
- Every wave ends when the final enemy disappears.
- Both victory and defeat remain achievable.
- The Windows executable is rebuilt; editing the Unity project does not update an existing build.

## Runtime scripts

| Script | Responsibility |
| --- | --- |
| `GameManager` | Currency, base lives, score, selection, pause and win/lose state |
| `WaveManager` | The five waves and enemy spawning |
| `Enemy` | Waypoint movement, health, rewards and base damage |
| `BuildSpot` | Buying, upgrading and selling on a fixed pad |
| `Tower` | Target search, aiming and all three tower styles |
| `Projectile` | Homing movement, collision checks and rocket splash damage |
| `GameUI` | HUD panels and button actions |
| `MainMenu` | Play, settings, saved progress, reset confirmation and quit |
| `CameraController` | Smooth movement, zoom and map boundaries |
| `SaveData` | The six values stored in JSON |
| `SaveSystem` | Load, save, validation and progress reset |
| `AudioManager` | Persistent music, SFX and volume settings |
| `EffectsManager` | Four short particle effects |
| `SceneFader` | Fade transitions around scene loading |

The classes use direct Inspector references and ordinary methods on purpose. There is no pooling,
generic event framework, singleton inheritance hierarchy, alignment system or launcher inheritance tree.

## Quick file reference

- Starting credits and base lives: `Game Manager` in `Game.unity`
- Enemy health, speed, reward and base damage: the three enemy prefabs
- Tower range, damage, fire rate and upgrade values: the three tower prefabs
- Enemy counts and spawn delays: `Wave Manager` in `Game.unity`
- Tower prices: the `Build Spot` objects in `Game.unity`
- Camera speed, zoom and limits: `Main Camera` in `Game.unity`
- Music and SFX volume: the Settings screen or `save.json`

All required scene references are already connected in the Inspector.
