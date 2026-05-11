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
