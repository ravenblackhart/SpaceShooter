# SpaceShooter
Space Shooter using DOTS 0.51
Windows Build in Builds > Windows

Includes - 
- Simple Movements (Enemies, Asteroids, & Player) 
- Shooting 
- Waves of Enemies & Obstacles 
- Basic Masking Shader[^1]

### Components 
- Tags (Asteroid, Player, Enemy, Projectile) 
- General Transform Component (See Below) 
- Game Settings Component + Game Settings Authoring Script 
- Prefabs Collection Component

#### General Transform Component 
Since all entities spawning in the game will require a similar set of information for movements, the following properties where included in a General Transform Component (TransformComponent) : 
- Speed
- Rotation Speed 
- Direction (of travel) 
- Switch DirectionX & Switch DirectionY (Bools) 
- Health
In the current iteration, not all the properties are in use. 

#### Game Settings Component & Prefabs Collection Component
Both the Game Settings Component (with seperate authoring script) and the Prefabs Collection Component were created to allow fast editing and reference storage of required values and components. 

### Systems
A number of systems were created, each handling only 1 aspect of the behaviour of an entity type

### Task List 
Additional aspects to be explored - 
- [] Collision & Damage System 
- [] UI vs DOTS



[^1]: Shader created to ensure Entities spawning in game is not visible outside of white bounds box.
