using Roguelike.models;

namespace Roguelike
{
    public interface IDungeonGenerator
    {
        Dungeon Generate(int width, int height);
    }
}
