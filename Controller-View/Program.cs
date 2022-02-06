using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using TestJWT.Helpers;
using TestJWT.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SchoolContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

// Add services to the container.
{
    var services = builder.Services;
    services.AddCors();

    services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true); ;

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

{
    app.UseCors(x => x
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
    app.UseMiddleware<JwtMiddleware>();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
