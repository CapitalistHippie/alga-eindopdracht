using System;
using System.Collections.Generic;
using System.Linq;
using Roguelike.ExtensionMethods;
using Roguelike.models;

namespace Roguelike
{
    public class DungeonService : IDungeonService
    {
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
    }
}
