using System.Collections.Generic;

namespace Roguelike.models
{
    public class Dungeon
    {
        public List<List<Room>> DungeonRows { get; set; }
        public List<Corridor> Corridors { get; set; }

        public Room StartRoom { get; set; }
        public Room EndRoom { get; set; }

        public Dungeon()
        {
            DungeonRows = new List<List<Room>>();
            Corridors = new List<Corridor>();
        }
    }
}
