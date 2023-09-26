using EthenAPI.Data;
using EthenAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment env = builder.Environment;

var ConnectionStringsSettings = Configuration.GetSection("ConnectionStrings");
ConnectionString = ConnectionStringsSettings["Default"];
jwtSettings = new JwtSettings();
Configuration.GetSection("JwtSettings").Bind(jwtSettings);



// Add services to the container.
// Configure JWT authentication
var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.ValidIssuer,
        ValidAudience = jwtSettings.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                var message = "The token has expired.";
                var result = JsonConvert.SerializeObject(new { message });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.WriteAsync(result);
                context.Response.CompleteAsync().Wait();
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddDbContext<MainDbContext>(opts =>
    opts.UseSqlServer(ConnectionString));

builder.Services.AddTransient<JwtTokenService>();
builder.Services.AddTransient<IAssetMetaDataService,AssetMetaDataService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


#region Global Declarations
public partial class Program
{    
    private static string ConnectionString ="";
    public static JwtSettings jwtSettings;
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string TokenLifetimeMinutes { get; set; }
        public string RefreshTokenLifetimeMinutes { get; set; }
    }
}
#endregion
