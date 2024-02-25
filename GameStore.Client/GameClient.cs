using System.IO.Compression;
using GameStore.Client.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace GameStore.Client
{
    public class GameClient
    {
        private readonly HttpClient httpClient;
        public GameClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Game[]?> GetGamesAsync() => await httpClient.GetFromJsonAsync<Game[]>("games");
        public async Task AddGameAsync(Game game) => await httpClient.PostAsJsonAsync("games", game);
        public async Task<Game> GetGameAsync(int id) => await httpClient.GetFromJsonAsync<Game>($"games/{id}") ?? throw new Exception("Could not find the game");
        public async Task UpDateGameAsync(Game updatedGame) => await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);
        public async Task DeleteGameAsync(int id) => await httpClient.DeleteAsync($"games/{id}");
    }
}