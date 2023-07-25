using CodeMechanic.Embeds;
using CodeMechanic.FileSystem;
using CodeMechanic.Neo4j.Repos;
using TPOT_Links.Controllers;
using TPOT_Links.Pages.Admin.Emails;

var policyName = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Load and inject .env files & values
DotEnv.Load();

// Add services to the container.
builder.Services.AddRazorPages();

// Allow CORS: https://www.stackhawk.com/blog/net-cors-guide-what-it-is-and-how-to-enable-it/#enable-cors-and-fix-the-problem
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        builder =>
        {
            builder
                // .WithOrigins("http://localhost:3000", "tpot-links-mkii")
                .AllowAnyOrigin()
                .WithMethods("GET")
                .AllowAnyHeader();
        });
});

builder.Services.ConfigureAirtable();
builder.Services.ConfigureNeo4j();

builder.Services.AddTransient<IEmbeddedResourceQuery, EmbeddedResourceQuery>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<INeo4JRepo, Neo4JRepo>();
builder.Services.AddTransient<ICarService, CarService>();

builder.Services.AddControllers();

var app = builder.Build();

// source: https://github.com/tutorialseu/sending-emails-in-asp/blob/main/Program.cs

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();