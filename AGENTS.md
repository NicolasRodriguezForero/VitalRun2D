## Project Overview

VitalRun2D is a 2D Unity game (Unity 6+) built in C#. Players deliver humanitarian aid packages by collecting items, packing them at stations, and delivering completed boxes to color-coded destinations. The game is timed (2 minutes), uses the New Input System, and is primarily written in Spanish for in-game text/comments.

## Unity Development Commands

There is no CLI build system — development happens through the Unity Editor. Open the project by launching Unity Hub and selecting this folder, or via `open VitalRun2D.sln` for script-only editing in VS Code/Rider.

**Scenes** (in `Assets/Scenes/`): `MainMenu.unity`, `Tutorial.unity`, `Game.unity`

To run: press Play in the Unity Editor with `Game.unity` loaded.

## Architecture

### Singleton Managers

All core systems are singletons accessed via `Instance`:

- `GameManager` — game state (start, end, active orders display, final score)
- `OrderManager` — active orders list (max 3), completion tracking, points dispatch
- `ScoreManager` — point accumulation with combo multiplier (1.5× at 3, 2× at 5+ combos; combo resets after 15s inactivity)
- `GameTimer` — 2-minute countdown with blinking warning at low time
- `RoomBlocker` — randomly blocks/unblocks rooms every 25s for 10s durations
- `PowerUpInventory` — holds up to 3 powerup slots, activated via keys 1/2/3

### Data Model (ScriptableObjects)

- `ItemData` (`Assets/Items/`) — item name, sprite, category (Medical/Food/Water/Logistics)
- `OrderData` (`Assets/Orders/`) — destination, required items, box color, base points

### Gameplay Flow

```
ItemPickup → PlayerInventory → PackingTable (10s animation) → Box → DispatchBox → OrderManager → ScoreManager
```

1. Player picks up `ItemPickup` objects (8s respawn after pickup)
2. Deposits items at `PackingTable` (progress bar, 10s pack time)
3. Packed `Box` is carried to matching-color `DispatchBox`
4. `OrderManager` validates and completes the order, awarding points

### Player System

- `PlayerController` — movement (WASD/arrows), interaction (E/Space), drop (Q); handles Velocidad speed boost (1.5× for 5s)
- `PlayerInventory` — item capacity (default 1, expandable to 2 with Carrito powerup)
- Input bindings defined in `PlayerActions.cs` (New Input System asset)

### PowerUps

| Type | Effect |
|---|---|
| `Velocidad` | 1.5× speed for 5s |
| `Carrito` | Inventory capacity → 2 for 15s |
| `DoblePuntuacion` | 2× points on next delivery |

`PowerUpSpawner` spawns random powerups at fixed spawn points every 12s; each has a 5s lifetime.

### Room Blocking

`Room` objects have a collider-based blocker and dark overlay. `RoomBlocker` (singleton) picks a random room, activates its blocker (preventing entry), holds for 10s, then deactivates. `RoomOccupancyProbe` prevents blocking a room the player currently occupies.

### UI

- `PackingTable` — packing station UI with progress bar
- `ResultsUI` — end-screen score display and scene navigation
- PowerUp inventory slots rendered via `PowerUpInventory`

## Script File Locations

```
Assets/Scripts/
  Game/          GameManager, GameTimer, ScoreManager, ResultsUI
  Player/        PlayerController, PlayerInventory, PlayerActions
  Orders/        OrderManager, OrderData
  Items/         ItemData, ItemPickup, Box, DispatchBox
  UI/            PackingTable, Room, RoomBlocker, RoomOccupancyProbe
  PowerUps/      PowerUpType, PowerUpInventory, PowerUpPickup, PowerUpSpawner
```

## Conventions

- Class/method names: English. In-game text, comments, and some variable names: Spanish.
- Singletons follow the `public static T Instance` pattern with `Awake` self-assignment.
- ScriptableObjects are stored as assets in `Assets/Items/` and `Assets/Orders/`.
- Prefabs for items and powerups live in `Assets/Prefabs/`.
