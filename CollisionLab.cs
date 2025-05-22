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
    
struct ColliderBox : IComponent
{
    public Vector2     size;
        
    public ColliderBox(float width, float height) {
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

        
        // --- create character entities
        store.CreateEntity(new EntityName("Player"),    new Position2D(4, 0),  new ColliderBox(1,1), Tags.Get<Character>());
        store.CreateEntity(new EntityName("Monster 1"), new Position2D(5, 0),  new ColliderBox(1,1), Tags.Get<Character>());
        store.CreateEntity(new EntityName("Monster 2"), new Position2D(10, 0), new ColliderBox(1,1), Tags.Get<Character>());
        
        // --- create environment entities
        for (int n = 0; n < 10; n++) {
            store.CreateEntity(new EntityName($"Tile {n}"), new Position2D(n, 0), new ColliderBox(1,1), Tags.Get<Environment>());    
        }
        
        // --- create / execute two nested queries to get collision between environment tiles and characters
        // Using tags enables to define two sets of collision candidates (tiles & characters)
        // So collisions of tiles with other tiles will not be in the collision result.
        var characterQuery  = store.Query<Position2D, ColliderBox>().AnyTags(Tags.Get<Character>());
        var envQuery        = store.Query<Position2D, ColliderBox>().AnyTags(Tags.Get<Environment>());

        Console.WriteLine();
        Console.WriteLine("--- Collisions: characters with tiles");
        envQuery.ForEachEntity((ref Position2D envPos, ref ColliderBox envBox, Entity env) =>
        {
            foreach (var (positions, colliders, characters) in characterQuery.Chunks) {
                var positionSpan = positions.Span;
                var colliderSpan = colliders.Span;
                for (int n = 0; n < characters.Length; n++) {
                    if (IsCollision(envPos.pos, envBox.size, positionSpan[n].pos, colliderSpan[n].size)) {
                        Console.WriteLine($"{characters.EntityAt(n).Name} collided with {env.Name}");
                    }
                }
            }
        });
        
        Console.WriteLine();
        Console.WriteLine("--- Collisions: characters with characters");
        characterQuery.ForEachEntity((ref Position2D otherPos, ref ColliderBox otherBox, Entity other) =>
        {
            foreach (var (positions, colliders, characters) in characterQuery.Chunks) {
                var positionSpan = positions.Span;
                var colliderSpan = colliders.Span;
                for (int n = 0; n < characters.Length; n++) {
                    if (other.Id == characters[n]) continue;
                    if (IsCollision(otherPos.pos, otherBox.size, positionSpan[n].pos, colliderSpan[n].size)) {
                        Console.WriteLine($"{characters.EntityAt(n).Name} collided with {other.Name}");
                    }
                }
            }
        });
    }
    
    // If wrong blame Copilot :)
    private static bool IsCollision(Vector2 pos1, Vector2 size1, Vector2 pos2, Vector2 size2)
    {
        return !(pos1.X + size1.X < pos2.X ||
                 pos1.X > pos2.X + size2.X ||
                 pos1.Y + size1.Y < pos2.Y ||
                 pos1.Y > pos2.Y + size2.Y);
    }

}