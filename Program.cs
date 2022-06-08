using Messenger;
using Messenger.Database;
using Messenger.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Messenger.Hubs;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MessengerContext>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<JWTService>();
builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:3001")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();    
app.UseAuthorization();     

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
     name: "default",
     pattern: "{controller=Info}/{action=Index}/{id?}");
});
app.MapHub<ChatHub>("/chat");

app.Run();

//Scaffold-DbContext "Server=.\SQLEXPRESS;Database=Messenger;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer