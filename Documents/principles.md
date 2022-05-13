principles.md

---

## Server should be able to be compiled and run game without importing Unity or Opengl

- There is no OpenGl for server, so should not need to import
- There is no Unity for server, so should not need to import

Example
- PlanetTileMap
-- is part of Unity Scene
-- is rendered with OpenGl

This mean
- create seperate folder/namespace for Unity Object, that imports the PlanetTileMap
- create/seperate out the Draw() function from the state/physics simulation for a planet map

Even if first thought is to put all functionality related to PlanetTileMap into one folder, its important to keep this seperation.

## Assets/Libraries should not contain anything that imports from Assets/src

- The library folder contains libraries which have few or no import dependencies
- The game may use these library functions
- But the library functions must not import anything internal to game

- A library is an external library used by the game
- A utility is a library that uses imports from Assets/src, that requires loading some parts of game engine

- keep a strict seperation

## If Assets are created in Unity Scenes at run-time, the performance of unity editor is better

If assets are generated programmatically, then unity editor does not load them or process them, during compilation or build/testing, until the objects are created at run time.

This keeps unity from becoming slow.

Otherwise Unity will try to resolve and deal with thousands of objects and references during compilation.

So sometimes, it makes sense to write a loader, that reads a text file from StreamableAssets and then creates the object at runtime, instead of importing/creating object in the unity gui.

##

