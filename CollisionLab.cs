using System.Numerics;
using Friflo.Engine.ECS;

namespace LabEcsCollisions;


struct Position2D : IComponent
{
    public Vector2     pos;
        
    public Position2D(float x, float y) {
        pos = new Vector2(x, y);
    }
}
    
struct ColliderBBox : IComponent
{
    public Vector2     size;
        
    public ColliderBBox(float width, float height) {
        size = new Vector2(width, height);
    }
}
    
struct Character   : ITag;  // tag character entities. E.g. Player, monsters, ...
struct Environment : ITag;  // tag environment entities. E.g. tiles, pickups, ...



public static class CollisionLab
{
    public static void Run()
    {
        var store = new EntityStore();
        Console.WriteLine("\nCollisions:");
        
        // --- create character entities
        store.CreateEntity(new EntityName("Player"),    new Position2D(4, 0),  new ColliderBBox(1,1), Tags.Get<Character>());
        store.CreateEntity(new EntityName("Monster 1"), new Position2D(8, 0),  new ColliderBBox(1,1), Tags.Get<Character>());
        store.CreateEntity(new EntityName("Monster 2"), new Position2D(10, 0), new ColliderBBox(1,1), Tags.Get<Character>());
        
        // --- create environment entities
        for (int n = 0; n < 10; n++) {
            store.CreateEntity(new EntityName($"Tile {n}"), new Position2D(n, 0), new ColliderBBox(1,1), Tags.Get<Environment>());    
        }
        
        // --- create / execute two nested queries to get collision between environment tiles and characters
        // Using tags enables to define two sets of collision candidates (tiles & characters)
        // So collisions of tiles with other tiles will not be in the collision result.
        var characterQuery  = store.Query<Position2D, ColliderBBox>().AnyTags(Tags.Get<Character>());
        var envQuery        = store.Query<Position2D, ColliderBBox>().AnyTags(Tags.Get<Environment>());

        envQuery.ForEachEntity((ref Position2D envPos, ref ColliderBBox envBBox, Entity env) =>
        {
            var pos  = envPos.pos;
            var size = envBBox.size;
            characterQuery.ForEachEntity((ref Position2D charPos, ref ColliderBBox charBBox, Entity character) =>
            {
                if (IsCollision(pos, size, charPos.pos, charBBox.size)) {
                    Console.WriteLine($"{character.Name} collided with environment entity {env.Id}");
                }
            });
        });
    }
    
    private static bool IsCollision(Vector2 pos1, Vector2 size1, Vector2 pos2, Vector2 size2)
    {
        return !(pos1.X + size1.X < pos2.X ||
                 pos1.X > pos2.X + size2.X ||
                 pos1.Y + size1.Y < pos2.Y ||
                 pos1.Y > pos2.Y + size2.Y);
    }

}