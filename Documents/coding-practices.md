## Coding Practices

### Entity Component System (ECS)
- Entias
- not Unity's default ECS
- game will use "pure ECS" (no inheritance of monoobject, etc)


### Entity
- have an id (integer id)
- group together components

### Component
- contains data (just a struct)
- has no code (no method functions on the components)

### Systems
- contain behavior
- system must not have state
- example: draw system, physics system
- if a system has state, then it has a singleton component

### Goal: Eventually
- physics engine/simulation part of game can share code with the client (server/client have same code for physics simulation; code sharing)
- backend server and simulation can be compiled with importing unity

### Input System
- use unity initially
- eventually, inputs should be fed in with a singleton input state component (not required at begining)
- intially just use Unity Input library

### Acessors
- NO GETTERS/SETTERS on every field, unless there is a reason
- code should not be 60% getter/setter methods
- but getter/setter can be used if we need to introspect what is writing to field 