using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoomProcess.Data;
using RoomProcess.Helpers;
using RoomProcess.InterfaceRepository;
using RoomProcess.Profiles;
using RoomProcess.Repository;
using RoomProcess.Services.KorisnikService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ZA BAZU 
builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//

//ZA CORS, vezano za front da omoguci uzimanje podataka
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(
    options => {
        options.AddPolicy("Cors Policy", policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
        });
    });
//

// Scope!!!
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

//Token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = true,//bilo false
            ValidIssuer= builder.Configuration.GetSection("AppSettings:Issuer").Value,
            ValidateAudience = false
        };

    });
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

// ZA FRONTEND ROUTING I CORS
app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
//

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
