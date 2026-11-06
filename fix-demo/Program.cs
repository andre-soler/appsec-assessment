using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("demo-super-secret-key-at-least-32-bytes!!"));

var vehicles = new List<Vehicle>
{
    new(1, "driver-a@example.com", "37.40,-94.70"),
    new(2, "driver-b@example.com", "40.71,-74.00"),
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (LoginRequest req) =>
{
    var handler = new JsonWebTokenHandler();
    var token = handler.CreateToken(new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Email, req.Email)]),
        Expires = DateTime.UtcNow.AddMinutes(30),
        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
    });
    return Results.Ok(new { token });
});

// CORRIGIDO: compara o dono do veículo com o dono do token.
app.MapGet("/vehicle/{id}/location", (int id, ClaimsPrincipal user) =>
{
    var userEmail = user.FindFirstValue(ClaimTypes.Email);
    var vehicle = vehicles.FirstOrDefault(v => v.Id == id && v.OwnerEmail == userEmail);
    return vehicle is null ? Results.NotFound() : Results.Ok(new { vehicle.Location });
}).RequireAuthorization();

app.Run();

record Vehicle(int Id, string OwnerEmail, string Location);
record LoginRequest(string Email);
