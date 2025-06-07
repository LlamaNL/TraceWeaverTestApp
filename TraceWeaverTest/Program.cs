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
#if DEBUG
	EnableTracing = true,
#else
	EnableTracing = false,
#endif
});
#if DEBUG
//Snapshots require bigger hub message size
builder.Services.Configure<HubOptions>(options =>
{
	options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
});
//To get the message size error if it got bigger than the above
builder.Services.AddSignalR(o =>
{
	o.EnableDetailedErrors = true;
});
#endif
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
