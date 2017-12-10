using Microsoft.Extensions.DependencyInjection;
using System;

namespace Roguelike
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDungeonGenerator, DungeonGenerator>()
                .AddSingleton<IDungeonService, DungeonService>()
                .AddSingleton<IGameService, GameService>()
                .BuildServiceProvider();

            var gameService = serviceProvider.GetService<IGameService>();

            gameService.Start();
        }
    }
}
