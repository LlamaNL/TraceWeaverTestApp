using WebVella.BlazorTrace;

namespace TraceWeaverTestWASM;

public static class DirtyHack
{
	public static IWvBlazorTraceService TracerService { get; set; } = default!;
}
