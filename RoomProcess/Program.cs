using Microsoft.EntityFrameworkCore;
using RoomProcess.Data;
using RoomProcess.Helpers;
using RoomProcess.InterfaceRepository;
using RoomProcess.Profiles;
using RoomProcess.Repository;
using RoomProcess.Services.KorisnikService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ZA BAZU MAJMUNE
builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//

// MORA MAJMUNE!!!
builder.Services.AddScoped<IKorisnikRepository, KorisnikRepository>();
builder.Services.AddScoped<IUlogaRepository, UlogaRepository>();
builder.Services.AddScoped<IObjekatRepository, ObjekatRepository>();
builder.Services.AddScoped<ITipObjektaRepository, TipObjektaRepository>();
builder.Services.AddScoped<IRecenzijaRepository, RecenzijaRepository>();
builder.Services.AddScoped<IRezervacijaRepository, RezervacijaRepository>();
builder.Services.AddScoped<IPopustRepository, PopustRepository>();
builder.Services.AddScoped<IOpremaRepository, OpremaRepository>();
builder.Services.AddScoped<IKorisnikService, KorisnikService>();
//

//HELPER
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
//

//MAPPER
builder.Services.AddAutoMapper(typeof(MappingProfiles));
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
