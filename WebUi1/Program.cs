

var builder = WebApplication.CreateBuilder(args);

// add services to the container builder

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebUi1", Version = "v1" });
});


// configuer the http request pipeline
var app = builder.Build();
string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (env == Environments.Development)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebUi1 v1"));
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();



app.UseCors(policy => policy.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();//it will use index first "angular index"
app.UseStaticFiles();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");



AppContext.SetSwitch("Npsql.EnableLegacyTimestampBehavior", true); // datetime issue in new pg db that required UTC date

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error accured during migration");
}

await app.RunAsync();


