# Lab-ECS-collisions

A small C# project showing how to create a simple collision system with [friflo ECS](https://github.com/friflo/Friflo.Engine.ECS).

Created the example because of a question *"How to detect collisions in an ECS"* on [Discord](https://discord.com/invite/nFfrhgQkb8).  
In case of questions join the Discord server.

Setup a small [GitHub gist](https://gist.github.com/friflo/b81cff785ce846270afa2810dc4b0cd5) to define required component types.

**Goals**
- Super simple implementation using ECS queries to get the collision candidates.
- Rudimentary BBox collision logic from CoPilot. No optimizations.
- May add optimized collision detection using spatial hashing.
- Collect ideas.

## Key decisions

Precondition / assumptions  
- A solution for 2D collision detection.
- Typically a map created with [Tiled](https://www.mapeditor.org/).

On startup the **Tiled** Map is imported and all tiles are converted to entities.  
Tiles which are subject to collision will have a `Position2D` and a `ColliderBox` component.  
These tiles are also tagged with `Environment`.  
Depending of the layer and its properties additional tags can be added to the created entities.  
After importing the TileMap all created entities will have a `Position2D` component in world space.

Characters like players or monsters will also be entities with a `Position2D` and a `ColliderBox` component.  
These entities will be tagged with `Character`.



To enforce simplicity only queries like: `Query<Position, ColliderBox>()` are used.  
E.g. entities which are only used for rendering will not have a collider.  





Contributions welcome 😊

