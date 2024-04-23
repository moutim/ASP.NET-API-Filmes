using EstagiariosAPI.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
        };
    });

// Adicione uma política de autorização padrão que exige autenticação
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Autenticação JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    options.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    object value = options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

var app = builder.Build();

// Configuração para tentar rodar no Railway
app.Run("http://0.0.0.0:" + configuration["PORT"]);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

string myApiVersion = "v1";
string myAppName = "EstagiariosAPI";
string swaggerVersion = "4.15.5";
app.UseSwagger();

string pkgUrl = "https://unpkg.com/swagger-ui-dist@";
app.UseSwaggerUI(c => {
    c.HeadContent =
        $"<link rel=\"stylesheet\" type=\"text/css\" " +
        $"href=\"{pkgUrl}{swaggerVersion}/swagger-ui.css\" />";
    c.InjectStylesheet($"{pkgUrl}{swaggerVersion}/swagger-ui.css", "text/css");
    c.InjectJavascript($"{pkgUrl}{swaggerVersion}/swagger-ui-standalone-preset.js", "text/javascript");
    c.InjectJavascript($"{pkgUrl}{swaggerVersion}/swagger-ui-bundle.js", "text/javascript");
    c.SwaggerEndpoint($"/swagger/{myApiVersion}/swagger.json", myAppName);
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
