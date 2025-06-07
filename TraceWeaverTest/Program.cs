using Microsoft.AspNetCore.SignalR;
using TraceWeaverTest;
using TraceWeaverTest.Components;
using WebVella.BlazorTrace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorTrace(new WvBlazorTraceConfiguration()
{
	EnableTracing = true,
});
builder.Services.Configure<HubOptions>(options =>
{
	options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
});
builder.Services.AddSignalR(o =>
{
	o.EnableDetailedErrors = true;
});

var app = builder.Build();

DirtyHack.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
