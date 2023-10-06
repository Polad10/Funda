using Funda.Services;
using Funda.Services.Interfaces;
using Funda.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient(FundaApiConstants.FundaHttpClientName, client =>
{
    client.BaseAddress = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f");
});

builder.Services.AddSingleton<IFundaApi, FundaApi>();
builder.Services.AddSingleton<IFundaStat, FundaStat>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
