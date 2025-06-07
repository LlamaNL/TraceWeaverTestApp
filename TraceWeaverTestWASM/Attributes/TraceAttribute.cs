using MethodDecorator.Fody.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TraceWeaverTestWASM.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class TraceAttribute : Attribute, IMethodDecorator
{
	private ComponentBase _targetInstance = default!;
	private MethodBase _method = default!;
	private bool _isAsync = false;
	public void Init(object instance, MethodBase method, object[] args)
	{
		_targetInstance = (instance as ComponentBase)!;
		_method = method;
		_isAsync = method.IsDefined(typeof(AsyncStateMachineAttribute), false);
	}

	public void OnEntry()
	{
		if (DirtyHack.TracerService is not null)
		{
			DirtyHack.TracerService?.OnEnter(_targetInstance, methodName: _method.Name);
		}
	}

	public void OnExit()
	{
		if (!_isAsync && DirtyHack.TracerService is not null)
		{
			DirtyHack.TracerService?.OnExit(_targetInstance, methodName: _method.Name);
		}
	}

	public async Task OnTaskContinuation(Task task)
	{
		await task; // Ensure the task completes before proceeding
		if (DirtyHack.TracerService is not null)
		{
			DirtyHack.TracerService?.OnExit(_targetInstance, methodName: _method.Name);
		}
	}

	public void OnException(Exception exception)
	{
		// do nothing
	}
}