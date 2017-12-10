using Roguelike.models;
using System.Collections.Generic;

namespace Roguelike
{
    public interface IDungeonService
    {
        int GetSafestDistanceInSteps(Dungeon dungeon, Room from, Room to);
        int GetShortestDistanceInSteps(Room from, Room to);
        List<Room> GetShortestPath(Room from, Room to);
        List<Room> GetSafestPath(Dungeon dungeon, Room from, Room to);
        List<Corridor> GetMinimalSpanningTree(Dungeon dungeon);
        Room GetRandomRoom(Dungeon dungeon);
    }
}
