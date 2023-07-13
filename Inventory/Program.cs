using FastReport.Data;
using Inventory.Data;
using Inventory.Helpers;
using Inventory.Helpers.Interfaces;
using Inventory.Models.Account;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ReflectionIT.Mvc.Paging;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InventoryContext>(options =>
         options.UseMySql(connectionString, ServerVersion.Parse("8.0.33-mysql")));

// Add services to the container.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    // SignIn
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    // Password
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<Role>()
    .AddRoleManager<RoleManager<Role>>()
    .AddSignInManager<SignInManager<User>>()
    .AddRoleValidator<RoleValidator<Role>>()
    .AddEntityFrameworkStores<InventoryContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Usuario/Login/";
                });


builder.Services.AddControllersWithViews();

// Regional Configurations
var enUsCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = enUsCulture;
CultureInfo.DefaultThreadCurrentUICulture = enUsCulture;


// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        politica =>
        {
            politica.RequireRole("Admin");
        });
});


FastReport.Utils.RegisteredObjects.AddConnection(typeof(MySqlDataConnection));


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add Helpers
builder.Services.AddScoped<IUtil, Util>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<HelperFastReport>();

// Add Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAddressingService, AddressingService>();
builder.Services.AddScoped<IAddressingsInventoryStartService, AddressingsInventoryStartService>();
builder.Services.AddScoped<IItemAddressingService, ItemAddressingService>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IInventoryMovementService, InventoryMovementService>();
builder.Services.AddScoped<IInventoryStartService, InventoryStartService>();
builder.Services.AddScoped<IItemAddressingService, ItemAddressingService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IPerishableItemService, PerishableItemService>();
builder.Services.AddScoped<IReportViewService, ReportViewService>();
builder.Services.AddScoped<IStockTakingService, StockTakingService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();

// Add Paging List
builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap5";
    options.PageParameterName = "pageindex";
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Usuario/Login"; // Página de login personalizada
    options.LogoutPath = "/Usuario/Logout"; // Página de logout personalizada
    options.AccessDeniedPath = "/Usuario/AccessDenied"; // Página de acesso negado personalizada
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

app.UseFastReport();
app.UseRouting();

CriarPerfisUsuarios(app);

app.UseAuthentication();
app.UseCookiePolicy();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/Usuario/Login" && context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/"); // Página inicial do seu aplicativo
        return;
    }
    await next();
});
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = new PathString("/Resources")
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Warehouses}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

void CriarPerfisUsuarios(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
        service.SeedRoles();
        service.SeedUsers();
    }
}