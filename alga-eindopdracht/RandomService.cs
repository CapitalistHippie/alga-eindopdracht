using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    public class RandomService : IRandomService
    {
        public Random Random { get; set; }

        public RandomService()
        {
            Random = new Random();
        }

        public Random GetRandom()
        {
            return Random;
        }
    }
}
