using System.IO.Compression;
using GameStore.Client.Models;

namespace GameStore.Client
{
    public static class GameClient
    {
        private static readonly List<Game> games = new(){
            new Game(){
            Id=1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 2, 1)
            },
            new Game(){
            Id=2,
            Name = "Final Fantasy XIV",
            Genre = "Roleplaying",
            Price = 59.99M,
            ReleaseDate = new DateTime(2010, 9, 30)
            },
            new Game(){
            Id=3,
            Name = "FIFA23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateTime(2022, 9, 2)
            }
        };

        public static Game[] GetGames() => games.ToArray();

        public static void AddGame(Game game)
        {
            game.Id = games.Max(game => game.Id) + 1;
            games.Add(game);
        }

        public static Game GeTGame(int Id) => games.Find(x => x.Id == Id) ?? throw new Exception("Could not find the game");

        public static void UpDateGame(Game updatedGame)
        {
            Game existingGame = GeTGame(updatedGame.Id);
            existingGame.Name = updatedGame.Name;
            existingGame.Genre = updatedGame.Genre;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;
        }

        public static void DeleteGame(int id)
        {
            Game game = GeTGame(id);
            games.Remove(game);
        }
    }


}