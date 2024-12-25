global using UserAuthenticationAPI.Models;
global using UserAuthenticationAPI.Data;
global using MongoDB.Driver;
global using UserAuthenticationAPI.Dtos.User;
global using UserAuthenticationAPI.Services.AuthenticationService;
global using UserAuthenticationAPI.Services.UserService;
global using UserAuthenticationAPI.Services.StatisticsService;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "UserAuthenticationAPI", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddSingleton<DataContext>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Specifies the scheme used for authenticating users. app to validate JWT tokens for every request.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    //  Determines what happens when authentication fails. (unauthorized request). app responds with a 401 Unauthorized status.
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters   //Specifies rules for validating JWT tokens
    {
        ValidateIssuerSigningKey = true,  // Ensures the token’s signature is verified using the specified signing key.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = false, //  Verifies the iss (issuer) claim in the token
        ValidateAudience = false, // Verifies the aud (audience) claim in the token.
        ClockSkew = TimeSpan.Zero // ensures tokens are valid only within their exact exp (expiration) time.
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();    // Verifies the user’s identity by validating the JWT token in the request headers.
app.UseAuthorization();     // Checks if the authenticated user has the necessary permissions (roles or claims) to access a specific endpoint.

app.MapControllers();

app.Run();
