using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(setup => {
    setup.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<PokesiteDbContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("PokesiteDb"));
});

// Add custom services
builder.Services.AddScoped<IPokemonService, DatabasePokemonService>();
builder.Services.AddScoped<IMoveService, DatabaseMoveService>();
builder.Services.AddScoped<IItemService, DatabaseItemService>();

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();