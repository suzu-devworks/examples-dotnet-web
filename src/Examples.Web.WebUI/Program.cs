using Examples.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddInMemoryCollection(
    new Dictionary<string, string?>
    {
        ["ConfigurationKey1"] = "From WebApplicationBuilder: overwrite.",
        ["ConfigurationKey2"] = "From WebApplicationBuilder: overwrite."
    });

// Add services to the container.
builder.Services.AddRazorPages();

//# Configure Infrastructures.
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
