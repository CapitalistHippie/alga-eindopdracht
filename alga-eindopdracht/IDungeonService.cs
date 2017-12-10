using Roguelike.models;
using System.Collections.Generic;

namespace Roguelike
{
    public interface IDungeonService
    {
        int GetDistanceInSteps(Room from, Room to);
        List<Room> GetShortestPath(Room from, Room to);
        List<Room> GetSafestPath(Room from, Room to);
        List<Corridor> GetMinimalSpanningTree(Dungeon dungeon);
        Room GetRandomRoom(Dungeon dungeon);
    }
}
