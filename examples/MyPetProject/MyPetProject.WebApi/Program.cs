using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyPetProject.Auth;
using MyPetProject.WebApi;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    AddJwtAuthentication(builder);
    AddApiServices(builder);
    AddModules(builder);

    WebApplication app = builder.Build();
    UseDebugServices(app);
    app.UseExceptionMiddleware();
    app.MapControllers();
    app.MapHealthChecks("/health");
    app.UseAuthentication();
    app.UseAuthorization();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

void AddJwtAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
}

void AddApiServices(WebApplicationBuilder builder)
{
    builder.Services.AddHealthChecks();
    builder.Services
        .AddControllers()
        .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerDocumentation();
}

void AddModules(WebApplicationBuilder builder)
{
    builder.Services.AddAuthModule(builder.Configuration);
}

void UseDebugServices(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        return;
    }

    app.UseSwaggerDocumentation();
    app.UseCors(x =>
    {
        x.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
}