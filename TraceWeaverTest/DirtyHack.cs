using System.Net.NetworkInformation;
using WebVella.BlazorTrace;

namespace TraceWeaverTest;

public static class DirtyHack
{
	public static IServiceProvider ServiceProvider { get; set; } = default!;
	public static IWvBlazorTraceService TracerService { get; set; } = default!;
}
