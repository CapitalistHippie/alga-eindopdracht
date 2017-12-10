using System.Collections.Generic;
using System.Linq;
using Roguelike.models;

namespace Roguelike
{
    public class DungeonGenerator : IDungeonGenerator
    {
        private IRandomService _randomService;
        private IDungeonService _dungeonService;

        public DungeonGenerator(IRandomService randomService, IDungeonService dungeonService)
        {
            _randomService = randomService;
            _dungeonService = dungeonService;
        }

        public Dungeon Generate(int width, int height)
        {
            var dungeon = new Dungeon();
            var rng = _randomService.GetRandom();

            // Generate the rooms.
            for (int y = 0; y < height; y++)
            {
                dungeon.DungeonRows.Add(new List<Room>());

                for (int x = 0; x < width; x++)
                {
                    var room = new Room();
                    room.Visited = false;

                    var enemyChance = rng.Next(100);
                    if (enemyChance < 50)
                    {
                        room.Enemy = new Enemy();
                        room.Enemy.Level = rng.Next(10);
                    }

                    dungeon.DungeonRows[y].Add(room);
                }
            }

            // Now the corridors.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y != 0)
                    {
                        var northCorridor = new Corridor();
                        northCorridor.Collapsed = false;
                        northCorridor.Room1 = dungeon.DungeonRows[y][x];
                        northCorridor.Room2 = dungeon.DungeonRows[y - 1][x];
                        dungeon.DungeonRows[y][x].NorthCorridor = northCorridor;
                        dungeon.DungeonRows[y - 1][x].SouthCorridor = northCorridor;
                        dungeon.Corridors.Add(northCorridor);
                    }

                    if (x != 0)
                    {
                        var westCorridor = new Corridor();
                        westCorridor.Collapsed = false;
                        westCorridor.Room1 = dungeon.DungeonRows[y][x];
                        westCorridor.Room2 = dungeon.DungeonRows[y][x - 1];
                        dungeon.DungeonRows[y][x].WestCorridor = westCorridor;
                        dungeon.DungeonRows[y][x - 1].EastCorridor = westCorridor;
                        dungeon.Corridors.Add(westCorridor);
                    }
                }
            }

            // Set a random start room.
            var startRoom = _dungeonService.GetRandomRoom(dungeon);
            startRoom.Enemy = null;
            startRoom.Visited = true;
            dungeon.StartRoom = startRoom;

            // Set a random end room.
            Room endRoom = null;

            do
            {
                endRoom = _dungeonService.GetRandomRoom(dungeon);
                endRoom.Enemy = null;
                dungeon.EndRoom = endRoom;

            } while (startRoom == endRoom);

            // Destroy a couple of corridors.
            var mst = _dungeonService.GetMinimalSpanningTree(dungeon);

            var collapsableCorridors = dungeon.Corridors.Select(x => x).ToList();
            foreach (var corridor in mst)
            {
                collapsableCorridors.Remove(corridor);
            }

            for (int i = 0; i < rng.Next(0, collapsableCorridors.Count / 2); i++)
            {
                var randomCollapsableCorridor = collapsableCorridors[rng.Next(collapsableCorridors.Count)];

                randomCollapsableCorridor.Collapsed = true;
                collapsableCorridors.Remove(randomCollapsableCorridor);
            }

            return dungeon;
        }
    }
}
