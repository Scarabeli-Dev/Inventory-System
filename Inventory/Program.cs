using Inventory.Data;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

var builder = WebApplication.CreateBuilder(args);

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InventoryContext>(options =>
         options.UseMySql(connectionString, ServerVersion.Parse("8.0.33-mysql")));

// Add services to the container.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<InventoryContext>();
builder.Services.AddControllersWithViews();

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        politica =>
        {
            politica.RequireRole("Admin");
        });
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add Services
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IAddressingService, AddressingService>();
builder.Services.AddScoped<IInventoryStartService, InventoryStartService>();
builder.Services.AddScoped<IAddressingsStockTakingService, AddressingsStockTakingService>();
builder.Services.AddScoped<IStockTakingService, StockTakingService>();
builder.Services.AddScoped<IItemService, ItemService>();

// Add Paging List
builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap4";
    options.PageParameterName = "pageindex";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
