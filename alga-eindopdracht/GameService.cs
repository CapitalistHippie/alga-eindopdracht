using Roguelike.models;
using System;
using System.Collections.Generic;

namespace Roguelike
{
    public class GameService : IGameService
    {
        private IDungeonGenerator _dungeonGenerator;
        private IDungeonService _dungeonService;

        public Dungeon Dungeon { get; set; }
        public Player Player { get; set; }

        public List<Room> ShortestPath { get; set; }
        public List<Room> SafestPath { get; set; }

        public bool CheatMode { get; set; }

        public string DrawMessage { get; set; }

        public GameService(IDungeonGenerator dungeonGenerator, IDungeonService dungeonService)
        {
            _dungeonGenerator = dungeonGenerator;
            _dungeonService = dungeonService;

            CheatMode = false;
        }

        public void Start()
        {
            var dungeonWidth = GetDungeonWidthFromUserInput();
            var dungeonHeight = GetDungeonHeightFromUserInput();

            Dungeon = _dungeonGenerator.Generate(dungeonWidth, dungeonHeight);
            Player = new Player();
            Player.Location = Dungeon.StartRoom;

            do
            {
                ShortestPath = _dungeonService.GetShortestPath(Player.Location, Dungeon.EndRoom);
                //SafestPath = _dungeonService.GetSafestPath(Player.Location, Dungeon.EndRoom);
                Draw();

                var input = Console.ReadLine().ToLower();
                var inputSplit = input.Split(' ');
                var command = inputSplit[0];

                switch (command)
                {
                    case "exit":
                        return;
                    case "cheat":
                        CheatMode = !CheatMode;
                        break;
                    case "talisman":
                        Talisman();
                        break;
                    case "startpunt":
                        Startpunt(inputSplit);
                        break;
                    case "eindpunt":
                        Eindpunt(inputSplit);
                        break;
                }
            } while (true);
        }

        private void Startpunt(string[] input)
        {
            int x;
            int y;

            if (!int.TryParse(input[1], out x))
            {
                DrawMessage = "Invalid input. Probeer een nummer mee te geven.";
            }
            else if (!int.TryParse(input[2], out y))
            {
                DrawMessage = "Invalid input. Probeer een nummer mee te geven.";
            }
            else if (x < 0 || x >= Dungeon.DungeonRows[0].Count || y < 0 || y >= Dungeon.DungeonRows.Count)
            {
                DrawMessage = "Parameter out of range.";
            }
            else
            {
                Dungeon.StartRoom = Dungeon.DungeonRows[y][x];
                Player.Location = Dungeon.StartRoom;
            }
        }

        private void Eindpunt(string[] input)
        {
            int x;
            int y;

            if (!int.TryParse(input[1], out x))
            {
                DrawMessage = "Invalid input. Probeer een nummer mee te geven.";
            }
            else if (!int.TryParse(input[2], out y))
            {
                DrawMessage = "Invalid input. Probeer een nummer mee te geven.";
            }
            else if (x < 0 || x >= Dungeon.DungeonRows[0].Count || y < 0 || y >= Dungeon.DungeonRows.Count)
            {
                DrawMessage = "Parameter out of range.";
            }
            else
            {
                Dungeon.EndRoom = Dungeon.DungeonRows[y][x];
            }
        }

        private void Talisman()
        {
            var steps = _dungeonService.GetDistanceInSteps(Player.Location, Dungeon.EndRoom);

            DrawMessage = $"Je bent {steps} stappen weg van de laatste kamer.";
        }

        private void Draw()
        {
            Console.Clear();

            if (CheatMode)
            {
                DrawDungeon(true);
            }
            else
            {
                DrawDungeon(false);
            }

            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("Cheat - Enables/disables cheat mode.");
            Console.WriteLine("Talisman - Vertelt je hoe veel stappen de uitgang van je vandaan is.");
            Console.WriteLine("Startpunt <x> <y> - Zet het startpunt op deze coördinaten.");
            Console.WriteLine("Eindpunt <x> <y> - Zet het eindpunt op deze coördinaten.");
            Console.WriteLine("Exit - Sluit het spel.");

            Console.WriteLine();
            if (DrawMessage != null)
            {
                Console.WriteLine(DrawMessage);
                Console.WriteLine();
                DrawMessage = null;
            }
        }

        private void DrawDungeon(bool ignoreVisitedBool)
        {
            foreach (var row in Dungeon.DungeonRows)
            {
                foreach (var room in row)
                {
                    if (!ignoreVisitedBool && !room.Visited)
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        if (ShortestPath.Contains(room))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        
                        ////if (SafestPath.Contains(room))
                        ////{
                        ////    Console.ForegroundColor = ConsoleColor.DarkGreen;
                        ////}

                        if (room == Player.Location)
                        {
                            Console.Write("$");
                        }
                        else if (room == Dungeon.StartRoom)
                        {
                            Console.Write("S");
                        }
                        else if (room == Dungeon.EndRoom)
                        {
                            Console.Write("E");
                        }
                        else if (room.Enemy != null)
                        {
                            Console.Write(room.Enemy.Level);
                        }
                        else
                        {
                            Console.Write("R");
                        }

                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    if (room.EastCorridor != null)
                    {
                        if (ignoreVisitedBool || (room.EastCorridor.Room1.Visited || room.EastCorridor.Room2.Visited))
                        {
                            if (room.EastCorridor.Collapsed)
                            {
                                Console.Write("~");
                            }
                            else
                            {
                                Console.Write("-");
                            }
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }

                Console.WriteLine();

                foreach (var room in row)
                {
                    if (room.SouthCorridor != null)
                    {
                        if (ignoreVisitedBool || (room.SouthCorridor.Room1.Visited || room.SouthCorridor.Room2.Visited))
                        {
                            if (room.SouthCorridor.Collapsed)
                            {
                                Console.Write("/");
                            }
                            else
                            {
                                Console.Write("|");
                            }
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }

                    Console.Write(" ");
                }

                Console.WriteLine();
            }
        }

        public int GetDungeonWidthFromUserInput()
        {
            int dungeonWidth;

            while (true)
            {
                Console.WriteLine("Insert dungeon width: ");

                var input = Console.ReadLine();

                if (!int.TryParse(input, out dungeonWidth))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
                else if (dungeonWidth < 2)
                {
                    Console.WriteLine("The dungeon must be at least 2 rooms wide. Please insert a higher number.");
                }
                else
                {
                    break;
                }
            }

            return dungeonWidth;
        }

        public int GetDungeonHeightFromUserInput()
        {
            int dungeonHeight;

            while (true)
            {
                Console.WriteLine("Insert dungeon height: ");

                var input = Console.ReadLine();

                if (!int.TryParse(input, out dungeonHeight))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
                else if (dungeonHeight < 2)
                {
                    Console.WriteLine("The dungeon must be at least 2 rooms high. Please insert a higher number.");
                }
                else
                {
                    break;
                }
            }

            return dungeonHeight;
        }
    }
}
