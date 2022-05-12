## Scenes

### Background

- Player Will be 2x1 tiles tall
- Each tile will be 32x32 pixels

### Scenes to Inplement
1. Game Introduction Scene
- just a series of comic book like .pngs
- with paning (zooming and movement)
- music
- [optional] narration (dont do)
- some text

2. Scene for testing combat
- character with gun
- test dummy
- hit-scan/collision detection for projectiles
- 2d or 3d bone animation
- rag doll
- animations/poses for enemies/player, attack animation, etc
- impact effects
- impact sound effects
  - sound for impacting enemy armour
  - sound for impacting unamoured (using more bass)
  - sound for miss/environmental hit
  - sound for critical hit
- smoke effects
- environmental damage effects (from explosions0
- player knockback
- pose freeze on hit
- [optional] dynamic lighting effects from projectile
- decals and damage to environment
- [optional] shell cases, or particle effects
- inverse-kinematics for arm position for player holding gun
- "gibs", arm flying off when critical
- [decide] use 3d models and pixel shader or use 2d bone animation?
- normal mapped lighting for players/weapon
  - ultra low poly 3d models, projected to 2d?

3. Movement Test Scene
- spunkly/metroid like dungeon
- things jump around on, jump puzzles, vertical cooridors, etc
- get from starting door, to exit door
  - jet pack
  - wall jump
  - running
  - jump
  - double jump
  - crouch

4. 2d side view (terraria like)
- port of github.com/skycoin/cx-game
- port tile loading and rendering system to C# and run in unity
- placing blocks/removing blocks, building houses, etc
- placement of furniture and machines
- [TODO] Define the drawing layers and write document

5. Ship falling through atomosphere
- gas giant
- planet warps around in the X axis
- like Outer Wildes, gas giant
- player falls through atomosphere
- player hits ground and dies within 40 to 60 seconds
- player has square ship (corvette)
- player can turn
- player can thrust forward
- player may have after burner (shift key)
- clouds / perlin noise
- rain?
- clouds, thunder? Damage player? Avoid?
  - lightning effect
  - thunder sound
  - damage to to player?
  - cloud assets, that player should avoid
  - low visibility if they are in the clouds
- floating crystals? floating rocks
- giant alien jelly fish floating through air? Six segments
- Space whale?
- water, at bottom of level
- player slows down, sinks, dies/is crushed when under the water
- player can go up vertically against gravity while after burner thrusting,but after burner is limited
- player blown around by wind
- [optional] tornado or updraft that throws player vertical
- [optional  tornados]
- atomosphere shaders whose density/intensity increases from blacka at space to liquid like as player approaches 0 m above planet

6. Top Down "Space Arena"
- see Gravity Well, Escape Velocity Nova, etc
- player has mining ship
  - forward to accelerate
  - backwards to deaccelerate
  - left/right to turn
  - Q/E to straff
- left click to fire? or space, turrets left click?
- there are astroids
- player receives items for mining certain areas of the astroid
- player can dock at space station at docking port
- player can deposit ore (iron ore, etc)
- player can build modules on space station on a grid, to upgrade storage space, smelting space etc
