using Aps.Api.ContextApp;
using Aps.Api.Model;
using Aps.Api.ViewModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase("aps_db"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/information", async (InformationRequest informationRequest, Context db) =>
{
    var information = Information.CreateInformation(informationRequest.Title, informationRequest.Local, informationRequest.Local, informationRequest.Image);
    db.Information.Add(information);
    await db.SaveChangesAsync();

    return Results.Created($"/information/{information.Id}", information);
})
.Produces<InformationRequest>()
.WithOpenApi();

app.MapGet("/information", async(Context db) =>
{
    var informations = await db.Information.ToListAsync();

    if (informations is null) return Results.NotFound();

    return Results.Ok(informations);
});



app.Run();
