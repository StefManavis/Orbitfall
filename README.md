# Orbitfall

Orbitfall is a personal 2D action game project developed in **Unity** using **C#**.

The project is built around room-based combat, real-time player control, enemy spawning, projectile-based shooting, and a simple upgrade system. It is currently a work-in-progress prototype focused on gameplay systems and programming practice.

## Features

- Real-time 2D player movement using Unity physics
- Touch-based input support, with mouse input for editor testing
- Automatic player rotation based on movement direction
- Projectile shooting system with configurable fire rate, damage, volleys, and bullet count
- Enemy AI that follows the player and deals contact damage
- Player health system with temporary invulnerability after taking damage
- Room-based gameplay loop with random room loading
- Enemy spawning from room-specific spawn points
- Upgrade selection system after clearing enemies
- Stat upgrades such as fire rate, damage, movement speed, max HP, double bullets, and double shots
- Unity UI integration for upgrade choices

## Tech Stack

- **Unity**
- **C#**
- **Unity 2D Physics**
- **TextMesh Pro**
- **Universal Render Pipeline**
- **Git / GitHub**

## Project Structure

```md
```text
Orbitfall/
├── Assets/
│   ├── Art/              # Sprites, textures, and materials
│   ├── Models/           # Player, enemy, and projectile models
│   ├── Prefabs/          # Player, enemies, projectiles, and rooms
│   ├── Scenes/           # Unity scenes
│   ├── Scripts/
│   │   ├── Combat/       # Bullet and projectile behavior
│   │   ├── Enemies/      # Enemy movement, damage, and death logic
│   │   ├── Player/       # Player movement, health, and stats
│   │   ├── Systems/      # Room management, enemy spawning, and upgrades
│   │   └── UI/           # Upgrade selection UI
│   └── Settings/         # Render pipeline and project-related settings
├── Packages/             # Unity package manifest and lock file
├── ProjectSettings/      # Unity project settings
├── .gitignore
└── README.md

```md

Main Gameplay Systems
Player Controller

The player controller handles movement, rotation, screen clamping, and shooting.

The project supports touch input for mobile-style control and mouse input inside the Unity editor for testing. The player moves using a Rigidbody2D, rotates toward the movement direction, and is kept within the visible camera boundaries.

When the player is not moving, the controller searches for the nearest enemy and fires projectiles toward it.

Combat System

The combat system uses projectile prefabs for shooting.

Bullets move forward, damage enemies on collision, and are destroyed after a short lifetime. Player stats affect bullet damage, fire rate, number of bullets per volley, and number of volleys per shot.

Enemy System

Enemies locate the player and move toward them using 2D physics.

Each enemy has configurable movement speed, health, and contact damage. When an enemy is defeated, it notifies the enemy spawner so the room-clear logic can continue.

Room System

Orbitfall uses a room-based structure.

A RoomManager loads random room prefabs from a room pool. Each room has its own player spawn point and enemy spawn points. When a room starts, enemies are spawned into that room.

Upgrade System

After all enemies in a room are defeated, the upgrade system offers the player a set of upgrade choices.

Available upgrades currently include:

Fire rate increase
Damage increase
Movement speed increase
Max HP increase
Additional bullets per volley
Additional volleys per shot

The upgrade UI pauses the game while the player chooses an upgrade, then resumes gameplay.

How to Open the Project
Clone the repository:
git clone https://github.com/StefManavis/Orbitfall.git
Open Unity Hub.
Click Add project.
Select the cloned Orbitfall folder.
Open the project in Unity.
How to Play / Test
Open the main Unity scene from the Assets/Scenes folder.
Press Play in the Unity editor.
Use mouse input in the editor to test movement.
Clear enemies in each room to trigger upgrade selection and progress to the next room.
Current Status

This project is currently in development.

Implemented so far:

Player movement
Player health
Enemy movement and contact damage
Projectile shooting
Room loading
Enemy spawning
Upgrade choices
Basic stat progression

Planned improvements:

More enemy types
More room layouts
Improved upgrade balancing
Better UI and menus
Sound effects and music
More polished visuals and effects
Game over and restart flow
More complete progression system
What I Practiced

Through this project, I practiced:

C# scripting in Unity
Object-oriented programming
Component-based game architecture
Unity 2D physics
Player input handling
Projectile and collision logic
Enemy behavior
Room-based game flow
UI-driven upgrade selection
Git/GitHub version control for a Unity project
Author

Stefanos Manavis
GitHub: github.com/StefManavis
