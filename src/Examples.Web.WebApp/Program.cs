using Examples.Web.Infrastructure;
using Examples.Web.WebApp.Applications.Environments;
using Examples.Web.WebApp.Applications.Laboratories.Filters;

var builder = WebApplication.CreateBuilder(args);

//# Add default configuration.
builder.Configuration.AddDefaultConfiguration();

// Add services to the container.
builder.Services.AddRazorPages();

//# configure Filters for processing order demonstration.
builder.Services.AddProcessingOrderFilters();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
