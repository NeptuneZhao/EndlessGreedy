using System;
using UnityEngine;

// Token: 0x0200096E RID: 2414
public class BlinkMonitor : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>
{
	// Token: 0x060046BF RID: 18111 RVA: 0x001948BC File Offset: 0x00192ABC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.CreateEyes)).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.DestroyEyes));
		this.satisfied.ScheduleGoTo(new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime), this.blinking);
		this.blinking.EnterTransition(this.satisfied, GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Not(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Transition.ConditionCallback(BlinkMonitor.CanBlink))).Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.BeginBlinking)).Update(new Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.eyes).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.EndBlinking));
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x00194986 File Offset: 0x00192B86
	private static bool CanBlink(BlinkMonitor.Instance smi)
	{
		return SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject) && smi.Get<Navigator>().CurrentNavType != NavType.Ladder;
	}

	// Token: 0x060046C1 RID: 18113 RVA: 0x001949A8 File Offset: 0x00192BA8
	private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
	{
		return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x001949C4 File Offset: 0x00192BC4
	private static void CreateEyes(BlinkMonitor.Instance smi)
	{
		smi.eyes = Util.KInstantiate(Assets.GetPrefab(EyeAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.eyes.gameObject.SetActive(true);
		smi.sm.eyes.Set(smi.eyes.gameObject, smi, false);
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x00194A21 File Offset: 0x00192C21
	private static void DestroyEyes(BlinkMonitor.Instance smi)
	{
		if (smi.eyes != null)
		{
			Util.KDestroyGameObject(smi.eyes);
			smi.eyes = null;
		}
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x00194A43 File Offset: 0x00192C43
	public static void BeginBlinking(BlinkMonitor.Instance smi)
	{
		smi.eyes.Play(smi.eye_anim, KAnim.PlayMode.Once, 1f, 0f);
		BlinkMonitor.UpdateBlinking(smi, 0f);
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x00194A71 File Offset: 0x00192C71
	public static void EndBlinking(BlinkMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x00194A88 File Offset: 0x00192C88
	public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
	{
		int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
		KAnimBatch batch = smi.eyes.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return;
		}
		KAnim.Anim.Frame frame;
		if (!smi.eyes.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return;
		}
		HashedString hash = HashedString.Invalid;
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					hash = frameElement.symbol;
					break;
				}
			}
		}
		smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol(hash), 3);
	}

	// Token: 0x04002E19 RID: 11801
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State satisfied;

	// Token: 0x04002E1A RID: 11802
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State blinking;

	// Token: 0x04002E1B RID: 11803
	public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.TargetParameter eyes;

	// Token: 0x04002E1C RID: 11804
	private static HashedString HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x020018FC RID: 6396
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018FD RID: 6397
	public class Tuning : TuningData<BlinkMonitor.Tuning>
	{
		// Token: 0x04007811 RID: 30737
		public float randomBlinkIntervalMin;

		// Token: 0x04007812 RID: 30738
		public float randomBlinkIntervalMax;
	}

	// Token: 0x020018FE RID: 6398
	public new class Instance : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.GameInstance
	{
		// Token: 0x06009ACD RID: 39629 RVA: 0x0036E3A6 File Offset: 0x0036C5A6
		public Instance(IStateMachineTarget master, BlinkMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009ACE RID: 39630 RVA: 0x0036E3B0 File Offset: 0x0036C5B0
		public bool IsBlinking()
		{
			return base.IsInsideState(base.sm.blinking);
		}

		// Token: 0x06009ACF RID: 39631 RVA: 0x0036E3C3 File Offset: 0x0036C5C3
		public void Blink()
		{
			this.GoTo(base.sm.blinking);
		}

		// Token: 0x04007813 RID: 30739
		public KBatchedAnimController eyes;

		// Token: 0x04007814 RID: 30740
		public string eye_anim;
	}
}
