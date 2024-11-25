using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public readonly struct Updater : IEnumerator
{
	// Token: 0x0600165A RID: 5722 RVA: 0x00078739 File Offset: 0x00076939
	public Updater(Func<float, UpdaterResult> fn)
	{
		this.fn = fn;
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x00078742 File Offset: 0x00076942
	public UpdaterResult Internal_Update(float deltaTime)
	{
		return this.fn(deltaTime);
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600165C RID: 5724 RVA: 0x00078750 File Offset: 0x00076950
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x00078753 File Offset: 0x00076953
	bool IEnumerator.MoveNext()
	{
		return this.fn(Updater.GetDeltaTime()) == UpdaterResult.NotComplete;
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x00078768 File Offset: 0x00076968
	void IEnumerator.Reset()
	{
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x0007876A File Offset: 0x0007696A
	public static implicit operator Updater(Promise promise)
	{
		return Updater.Until(() => promise.IsResolved);
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x00078788 File Offset: 0x00076988
	public static Updater Until(Func<bool> fn)
	{
		return new Updater(delegate(float dt)
		{
			if (!fn())
			{
				return UpdaterResult.NotComplete;
			}
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x000787A6 File Offset: 0x000769A6
	public static Updater While(Func<bool> isTrueFn)
	{
		return new Updater(delegate(float dt)
		{
			if (!isTrueFn())
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x000787C4 File Offset: 0x000769C4
	public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
	{
		Updater whileNotTrueUpdater = Updater.None();
		return new Updater(delegate(float dt)
		{
			if (whileNotTrueUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				if (!isTrueFn())
				{
					return UpdaterResult.Complete;
				}
				whileNotTrueUpdater = getUpdaterWhileNotTrueFn();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x000787F4 File Offset: 0x000769F4
	public static Updater None()
	{
		return new Updater((float dt) => UpdaterResult.Complete);
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x0007881A File Offset: 0x00076A1A
	public static Updater WaitOneFrame()
	{
		return Updater.WaitFrames(1);
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x00078822 File Offset: 0x00076A22
	public static Updater WaitFrames(int framesToWait)
	{
		int frame = 0;
		return new Updater(delegate(float dt)
		{
			int frame = frame;
			frame++;
			if (framesToWait <= frame)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x00078847 File Offset: 0x00076A47
	public static Updater WaitForSeconds(float secondsToWait)
	{
		float currentSeconds = 0f;
		return new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			if (secondsToWait <= currentSeconds)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x00078870 File Offset: 0x00076A70
	public static Updater Ease(Action<float> fn, float from, float to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<float>(fn, new Func<float, float, float, float>(Mathf.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0007888B File Offset: 0x00076A8B
	public static Updater Ease(Action<Vector2> fn, Vector2 from, Vector2 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector2>(fn, new Func<Vector2, Vector2, float, Vector2>(Vector2.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x000788A6 File Offset: 0x00076AA6
	public static Updater Ease(Action<Vector3> fn, Vector3 from, Vector3 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector3>(fn, new Func<Vector3, Vector3, float, Vector3>(Vector3.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x000788C4 File Offset: 0x00076AC4
	public static Updater GenericEase<T>(Action<T> useFn, Func<T, T, float, T> interpolateFn, Easing.EasingFn easingFn, T from, T to, float duration, float delay)
	{
		Updater.<>c__DisplayClass18_0<T> CS$<>8__locals1 = new Updater.<>c__DisplayClass18_0<T>();
		CS$<>8__locals1.useFn = useFn;
		CS$<>8__locals1.interpolateFn = interpolateFn;
		CS$<>8__locals1.from = from;
		CS$<>8__locals1.to = to;
		CS$<>8__locals1.easingFn = easingFn;
		CS$<>8__locals1.duration = duration;
		if (CS$<>8__locals1.easingFn == null)
		{
			CS$<>8__locals1.easingFn = Easing.SmoothStep;
		}
		CS$<>8__locals1.currentSeconds = 0f;
		CS$<>8__locals1.<GenericEase>g__UseKeyframeAt|0(0f);
		Updater updater = new Updater(delegate(float dt)
		{
			CS$<>8__locals1.currentSeconds += dt;
			if (CS$<>8__locals1.currentSeconds < CS$<>8__locals1.duration)
			{
				base.<GenericEase>g__UseKeyframeAt|0(CS$<>8__locals1.currentSeconds / CS$<>8__locals1.duration);
				return UpdaterResult.NotComplete;
			}
			base.<GenericEase>g__UseKeyframeAt|0(1f);
			return UpdaterResult.Complete;
		});
		if (delay > 0f)
		{
			return Updater.Series(new Updater[]
			{
				Updater.WaitForSeconds(delay),
				updater
			});
		}
		return updater;
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x0007896B File Offset: 0x00076B6B
	public static Updater Do(System.Action fn)
	{
		return new Updater(delegate(float dt)
		{
			fn();
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x00078989 File Offset: 0x00076B89
	public static Updater Do(Func<Updater> fn)
	{
		bool didInitalize = false;
		Updater target = default(Updater);
		return new Updater(delegate(float dt)
		{
			if (!didInitalize)
			{
				target = fn();
				didInitalize = true;
			}
			return target.Internal_Update(dt);
		});
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x000789BA File Offset: 0x00076BBA
	public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(Option.None, makeUpdaterFns);
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x000789CC File Offset: 0x00076BCC
	public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(loopCount, makeUpdaterFns);
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x000789DC File Offset: 0x00076BDC
	public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
	{
		if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
		{
			return Updater.None();
		}
		int completedLoopCount = 0;
		int currentIndex = 0;
		Updater currentUpdater = makeUpdaterFns[currentIndex]();
		return new Updater(delegate(float dt)
		{
			if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				int num = currentIndex;
				currentIndex = num + 1;
				if (currentIndex >= makeUpdaterFns.Length)
				{
					currentIndex -= makeUpdaterFns.Length;
					num = completedLoopCount;
					completedLoopCount = num + 1;
					if (limitLoopCount.IsSome() && completedLoopCount >= limitLoopCount.Unwrap())
					{
						return UpdaterResult.Complete;
					}
				}
				currentUpdater = makeUpdaterFns[currentIndex]();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x00078A4B File Offset: 0x00076C4B
	public static Updater Parallel(params Updater[] updaters)
	{
		bool[] isCompleted = new bool[updaters.Length];
		return new Updater(delegate(float dt)
		{
			bool flag = false;
			for (int i = 0; i < updaters.Length; i++)
			{
				if (!isCompleted[i])
				{
					if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
					{
						isCompleted[i] = true;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x00078A7C File Offset: 0x00076C7C
	public static Updater Series(params Updater[] updaters)
	{
		int i = 0;
		return new Updater(delegate(float dt)
		{
			int i;
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
			{
				i = i;
				i++;
			}
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x00078AA4 File Offset: 0x00076CA4
	public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
	{
		Updater.<>c__DisplayClass26_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass26_0();
		CS$<>8__locals1.coroutine = coroutine;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<RunRoutine>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x00078ADC File Offset: 0x00076CDC
	public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
	{
		return Updater.Run(monoBehaviour, Updater.Series(updaters));
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x00078AEC File Offset: 0x00076CEC
	public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
	{
		Updater.<>c__DisplayClass28_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass28_0();
		CS$<>8__locals1.updater = updater;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<Run>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x00078B24 File Offset: 0x00076D24
	public static float GetDeltaTime()
	{
		return Time.unscaledDeltaTime;
	}

	// Token: 0x04000C92 RID: 3218
	public readonly Func<float, UpdaterResult> fn;
}
