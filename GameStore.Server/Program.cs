using GameStore.Server.Models;
using GameStore.Server.Data;
using Microsoft.EntityFrameworkCore;
using GameStore.Server.Data.Configurations;
/*List<Game> games = new(){
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
        }; */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("http://localhost:5271")
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

var connString = builder.Configuration.GetConnectionString("GameStoreContext");
builder.Services.AddSqlServer<GameStoreContext>(connString);
var app = builder.Build();

app.UseCors();

var group = app.MapGroup("/games").WithParameterValidation();

//GET /Games
group.MapGet("/", async (GameStoreContext context) =>

await context.Games.AsNoTracking().ToListAsync()
);

//GET /GAMES/{id}

group.MapGet("/{id}", async (int id, GameStoreContext context) =>
{
    Game? game = await context.Games.FindAsync(id); //games.Find(g => g.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(game);
})
.WithName("GetGame");

//POST /Games
group.MapPost("/", async (Game game, GameStoreContext context) =>
{
    //game.Id = games.Max(game => game.Id) + 1;
    //games.Add(game);

    context.Games.Add(game);
    await context.SaveChangesAsync();
    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});


//PUT /Games/{id}
group.MapPut("/{id}", async (int id, Game updatedGame, GameStoreContext context) =>
{

    var rowsAffected = await context.Games.Where(game => game.Id == id)
        .ExecuteUpdateAsync(updates =>

            updates.SetProperty(game => game.Name, updatedGame.Name)
            .SetProperty(game => game.Genre, updatedGame.Genre)
            .SetProperty(game => game.Price, updatedGame.Price)
            .SetProperty(game => game.ReleaseDate, updatedGame.ReleaseDate));

    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
    // Game? existingGame = await context.Games.FindAsync(id); //games.Find(g => g.Id == id);//games.Find(g => g.Id == id);

    // if (existingGame is null)
    // {
    //     // updatedGame.Id = id;
    //     // context.Games.Add(updatedGame);
    //     // await context.SaveChangesAsync();
    //     // return Results.CreatedAtRoute("GetGame", new { id = updatedGame.Id }, updatedGame);
    //     return Results.NotFound();
    // }

    // existingGame.Name = updatedGame.Name;
    // existingGame.Genre = updatedGame.Genre;
    // existingGame.Price = updatedGame.Price;
    // existingGame.ReleaseDate = updatedGame.ReleaseDate;
    // await context.SaveChangesAsync();
    // return Results.NoContent();
});

//DELETE /Games/{id}
group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id)
        .ExecuteDeleteAsync();
    // Game? game = await context.Games.FindAsync(id); //games.Find(g => g.Id == id);//games.Find(g => g.Id == id);

    // if (game is null)
    // {
    //     return Results.NotFound();
    //     //return Results.NoContent();
    // }
    // games.Remove(game);
    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});



app.Run();

