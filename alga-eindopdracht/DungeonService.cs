using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Roguelike.ExtensionMethods;
using Roguelike.models;

namespace Roguelike
{
    public class DungeonService : IDungeonService
    {
        private IRandomService _randomService;

        public DungeonService(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public int GetDistanceInSteps(Room from, Room to)
        {
            int depth = 0;

            if (from == to)
            {
                return depth;
            }

            var roomSources = new Dictionary<Room, Room>();
            var visitedRooms = new List<Room>();
            var roomQueue = new Queue<Room>();

            roomQueue.Enqueue(from);
            visitedRooms.Add(from);

            while (roomQueue.Any())
            {
                var room = roomQueue.Dequeue();

                if (room == to)
                {
                    break;
                }

                var neighbouringRooms = GetNeighbouringRooms(room);

                foreach (var neighbouringRoom in neighbouringRooms)
                {
                    if (!visitedRooms.Contains(neighbouringRoom))
                    {
                        roomQueue.Enqueue(neighbouringRoom);
                        visitedRooms.Add(neighbouringRoom);
                        roomSources[neighbouringRoom] = room;
                    }
                }
            }

            var roomSource = to;
            do
            {
                roomSource = roomSources[roomSource];
                depth++;
            } while (roomSource != from);

            return depth;
        }

        private int GetDungeonRoomCount(Dungeon dungeon)
        {
            return dungeon.DungeonRows.Count * dungeon.DungeonRows[0].Count;
        }

        public List<Corridor> GetMinimalSpanningTree(Dungeon dungeon)
        {
            var visitedRooms = new List<Room>();
            var mstCorridors = new List<Corridor>();

            var startingRoom = GetRandomRoom(dungeon);
            visitedRooms.Add(startingRoom);

            while (true)
            {
                Corridor lowestWeightCorridor = null;
                Room lowestWeightCorridorOppositeRoom = null;
                int lowestWeightCorridorWeight = int.MaxValue;
                foreach (var visitingRoom in visitedRooms)
                {
                    // Get all corridors with a room that hasn't been visited yet.
                    var corridors = GetConnectedCorridors(visitingRoom).Where(x => !visitedRooms.Contains(GetOtherRoom(x, visitingRoom)));

                    // We might have already visited all those places.
                    if (!corridors.Any())
                    {
                        continue;
                    }

                    // Filter the corridors on the lowest weight.
                    var corridor = corridors.MinBy(x => GetCorridorWeight(x, visitingRoom));

                    // Check if it's really the lowest weight.
                    var weight = GetCorridorWeight(corridor, visitingRoom);
                    if (weight < lowestWeightCorridorWeight)
                    {
                        lowestWeightCorridor = corridor;
                        lowestWeightCorridorWeight = weight;
                        lowestWeightCorridorOppositeRoom = GetOtherRoom(corridor, visitingRoom);
                    }
                }

                mstCorridors.Add(lowestWeightCorridor);
                visitedRooms.Add(lowestWeightCorridorOppositeRoom);

                // If we've visited all the rooms, then we're done.
                if (visitedRooms.Count == GetDungeonRoomCount(dungeon))
                {
                    break;
                }
            }

            return mstCorridors;
        }

        public Room GetRandomRoom(Dungeon dungeon)
        {
            var rnd = _randomService.GetRandom();

            var randomRow = dungeon.DungeonRows[rnd.Next(dungeon.DungeonRows.Count)];
            var randomRoom = randomRow[rnd.Next(randomRow.Count)];

            return randomRoom;
        }

        private int GetCorridorWeight(Corridor corridor, Room fromRoom)
        {
            var oppositeRoom = GetOtherRoom(corridor, fromRoom);
            return oppositeRoom.Enemy == null ? 0 : oppositeRoom.Enemy.Level;
        }

        public List<Room> GetSafestPath(Room from, Room to)
        {
            throw new NotImplementedException();
        }

        public List<Room> GetShortestPath(Room from, Room to)
        {
            if (from == to)
            {
                return new List<Room> { from };
            }

            var roomSources = new Dictionary<Room, Room>();
            var visitedRooms = new List<Room>();
            var roomQueue = new Queue<Room>();

            roomQueue.Enqueue(from);
            visitedRooms.Add(from);

            while (roomQueue.Any())
            {
                var room = roomQueue.Dequeue();

                if (room == to)
                {
                    break;
                }

                var neighbouringRooms = GetNeighbouringRooms(room);

                foreach (var neighbouringRoom in neighbouringRooms)
                {
                    if (!visitedRooms.Contains(neighbouringRoom))
                    {
                        roomQueue.Enqueue(neighbouringRoom);
                        visitedRooms.Add(neighbouringRoom);
                        roomSources[neighbouringRoom] = room;
                    }
                }
            }

            var path = new List<Room>();

            var roomSource = to;
            do
            {
                path.Add(roomSource);

                roomSource = roomSources[roomSource];
            } while (roomSource != from);

            path.Add(from);
            path.Reverse();

            return path;
        }

        private List<Room> GetNeighbouringRooms(Room room)
        {
            List<Room> rooms = new List<Room>();

            if (room.NorthCorridor != null && !room.NorthCorridor.Collapsed)
            {
                if (room.NorthCorridor.Room1 == room)
                {
                    rooms.AddIfNotNull(room.NorthCorridor.Room2);
                }
                else
                {
                    rooms.AddIfNotNull(room.NorthCorridor.Room1);
                }
            }

            if (room.EastCorridor != null && !room.EastCorridor.Collapsed)
            {
                if (room.EastCorridor.Room1 == room)
                {
                    rooms.AddIfNotNull(room.EastCorridor.Room2);
                }
                else
                {
                    rooms.AddIfNotNull(room.EastCorridor.Room1);
                }
            }

            if (room.SouthCorridor != null && !room.SouthCorridor.Collapsed)
            {
                if (room.SouthCorridor.Room1 == room)
                {
                    rooms.AddIfNotNull(room.SouthCorridor.Room2);
                }
                else
                {
                    rooms.AddIfNotNull(room.SouthCorridor.Room1);
                }
            }

            if (room.WestCorridor != null && !room.WestCorridor.Collapsed)
            {
                if (room.WestCorridor.Room1 == room)
                {
                    rooms.AddIfNotNull(room.WestCorridor.Room2);
                }
                else
                {
                    rooms.AddIfNotNull(room.WestCorridor.Room1);
                }
            }

            return rooms;
        }

        private List<Corridor> GetConnectedCorridors(Room room)
        {
            var corridors = new List<Corridor>();

            if (room.NorthCorridor != null && !room.NorthCorridor.Collapsed)
            {
                corridors.Add(room.NorthCorridor);
            }

            if (room.EastCorridor != null && !room.EastCorridor.Collapsed)
            {
                corridors.Add(room.EastCorridor);
            }

            if (room.SouthCorridor != null && !room.SouthCorridor.Collapsed)
            {
                corridors.Add(room.SouthCorridor);
            }

            if (room.WestCorridor != null && !room.WestCorridor.Collapsed)
            {
                corridors.Add(room.WestCorridor);
            }

            return corridors;
        }

        private Room GetOtherRoom(Corridor corridor, Room room)
        {
            if (corridor.Room1 == room)
            {
                return corridor.Room2;
            }

            return corridor.Room1;
        }
    }
}
