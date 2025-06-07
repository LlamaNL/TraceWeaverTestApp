using MethodDecorator.Fody.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using WebVella.BlazorTrace;

namespace TraceWeaverTest.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class)]
public class TraceAttribute : Attribute, IMethodDecorator
{
	private ComponentBase _targetInstance = default!;
	private MethodBase _method = default!;
	private IWvBlazorTraceService _wvBlazorTraceService = default!;
	private IServiceProvider _serviceProvider = default!;
	private IServiceScope _scope = default!;

	public TraceAttribute()
	{
		_serviceProvider = DirtyHack.ServiceProvider;
	}

	public void Init(object instance, MethodBase method, object[] args)
	{
		_targetInstance = (instance as ComponentBase)!;
		_method = method;
	}

	public void OnEntry()
	{
		_scope = _serviceProvider.CreateScope();
		var tracerService = _scope.ServiceProvider.GetRequiredService<IWvBlazorTraceService>();
		tracerService.OnEnter(_targetInstance);
	}

	public void OnExit()
	{
		var tracerService = _scope.ServiceProvider.GetRequiredService<IWvBlazorTraceService>();
		tracerService.OnExit(_targetInstance);
		_scope.Dispose();
	}

	public void OnException(Exception exception)
	{
		// do nothing
	}
}
