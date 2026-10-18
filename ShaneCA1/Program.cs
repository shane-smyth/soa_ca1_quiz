using ShaneCA1;
using ShaneCA1.Components;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IMovieService, MovieService>(client =>
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/"));
builder.Services.AddScoped<MovieGame>();

builder.Services.AddHttpClient<ICountryService, CountryService>(client =>
{
    client.BaseAddress = new Uri("https://api.restcountries.com/");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", builder.Configuration["RestCountriesApiKey"]);
});
builder.Services.AddScoped<FlagGame>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();