using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StockTradingApplication.Models;
using StockTradingApplication.Services;
using StockTradingApplication.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<StockTradingApplicationContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
builder.Services.AddScoped<IBankService,BankService>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Theme = ScalarTheme.Kepler;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
