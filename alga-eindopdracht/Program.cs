using Microsoft.Extensions.DependencyInjection;

namespace Roguelike
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IRandomService, RandomService>()
                .AddSingleton<IDungeonService, DungeonService>()
                .AddSingleton<IDungeonGenerator, DungeonGenerator>()
                .AddSingleton<IGameService, GameService>()
                .BuildServiceProvider();

            var gameService = serviceProvider.GetService<IGameService>();

            gameService.Start();
        }
    }
}
