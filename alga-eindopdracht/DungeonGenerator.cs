using System;
using System.Collections.Generic;
using Roguelike.models;

namespace Roguelike
{
    public class DungeonGenerator : IDungeonGenerator
    {
        public Dungeon Generate(int width, int height)
        {
            var dungeon = new Dungeon();
            var rnd = new Random();

            // Generate the rooms.
            for (int y = 0; y < height; y++)
            {
                dungeon.DungeonRows.Add(new List<Room>());

                for (int x = 0; x < width; x++)
                {
                    var room = new Room();
                    room.Visited = false;

                    var enemyChance = rnd.Next(100);
                    if (enemyChance < 20)
                    {
                        room.Enemy = new Enemy();
                        room.Enemy.Level = rnd.Next(10);
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
                    }

                    if (x != 0)
                    {
                        var westCorridor = new Corridor();
                        westCorridor.Collapsed = false;
                        westCorridor.Room1 = dungeon.DungeonRows[y][x];
                        westCorridor.Room2 = dungeon.DungeonRows[y][x - 1];
                        dungeon.DungeonRows[y][x].WestCorridor = westCorridor;
                        dungeon.DungeonRows[y][x - 1].EastCorridor = westCorridor;
                    }
                }
            }

            // Set a random start room.
            var row = dungeon.DungeonRows[rnd.Next(dungeon.DungeonRows.Count)];
            var startRoom = row[rnd.Next(row.Count)];
            startRoom.Enemy = null;
            startRoom.Visited = true;
            dungeon.StartRoom = startRoom;

            // Set a random end room.
            Room endRoom = null;

            do
            {
                row = dungeon.DungeonRows[rnd.Next(dungeon.DungeonRows.Count)];
                endRoom = row[rnd.Next(row.Count)];
                endRoom.Enemy = null;
                dungeon.EndRoom = endRoom;

            } while (startRoom == endRoom);

            return dungeon;
        }
    }
}
