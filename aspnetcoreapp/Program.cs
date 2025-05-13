using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<PokesiteDbContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("PokesiteDb"));
});

// Add custom services
builder.Services.AddScoped<IPokemonService, DatabasePokemonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();