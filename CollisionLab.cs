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
        
        // --- create character entities
        store.CreateEntity(new EntityName("Player"),    new Position2D(4, 0),  new ColliderBBox(1,1), Tags.Get<Character>());
        store.CreateEntity(new EntityName("Monster 1"), new Position2D(8, 0),  new ColliderBBox(1,1), Tags.Get<Character>());
        store.CreateEntity(new EntityName("Monster 2"), new Position2D(10, 0), new ColliderBBox(1,1), Tags.Get<Character>());
        
        // --- create environment entities
        for (int n = 0; n < 10; n++) {
            store.CreateEntity(new EntityName($"Tile {n}"), new Position2D(n, 0), new ColliderBBox(1,1), Tags.Get<Environment>());    
        }
        
        var characterQuery  = store.Query<Position2D, ColliderBBox>().AnyTags(Tags.Get<Character>());
        var envQuery        = store.Query<Position2D, ColliderBBox>().AnyTags(Tags.Get<Environment>());
        
        envQuery.ForEachEntity((ref Position2D envPos, ref ColliderBBox envBBox, Entity _) =>
        {
            var pos  = envPos.pos;
            var size = envBBox.size;
            characterQuery.ForEachEntity((ref Position2D charPos, ref ColliderBBox charBBox, Entity _) =>
            {
                Collide(pos, size, charPos.pos, charBBox.size);
            });
        });
    }
    
    private static bool Collide(Vector2 pos1, Vector2 bbox1, Vector2 pos2, Vector2 bbox2) {
        return false;
    }
}