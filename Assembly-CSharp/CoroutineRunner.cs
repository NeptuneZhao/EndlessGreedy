using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class CoroutineRunner : MonoBehaviour
{
	// Token: 0x060015C7 RID: 5575 RVA: 0x00077104 File Offset: 0x00075304
	public Promise Run(IEnumerator routine)
	{
		return new Promise(delegate(System.Action resolve)
		{
			this.StartCoroutine(this.RunRoutine(routine, resolve));
		});
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x0007712C File Offset: 0x0007532C
	public ValueTuple<Promise, System.Action> RunCancellable(IEnumerator routine)
	{
		Promise promise = new Promise();
		Coroutine coroutine = base.StartCoroutine(this.RunRoutine(routine, new System.Action(promise.Resolve)));
		System.Action item = delegate()
		{
			this.StopCoroutine(coroutine);
		};
		return new ValueTuple<Promise, System.Action>(promise, item);
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x0007717D File Offset: 0x0007537D
	private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
	{
		yield return routine;
		completedCallback();
		yield break;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x00077193 File Offset: 0x00075393
	public static CoroutineRunner Create()
	{
		return new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x000771A4 File Offset: 0x000753A4
	public static Promise RunOne(IEnumerator routine)
	{
		CoroutineRunner runner = CoroutineRunner.Create();
		return runner.Run(routine).Then(delegate
		{
			UnityEngine.Object.Destroy(runner.gameObject);
		});
	}
}
