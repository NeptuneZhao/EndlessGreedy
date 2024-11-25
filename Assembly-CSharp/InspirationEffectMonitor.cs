using System;
using UnityEngine;

// Token: 0x0200098A RID: 2442
public class InspirationEffectMonitor : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>
{
	// Token: 0x06004745 RID: 18245 RVA: 0x00197D28 File Offset: 0x00195F28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.CatchyTune, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.OnCatchyTune)).ParamTransition<bool>(this.shouldCatchyTune, this.catchyTune, (InspirationEffectMonitor.Instance smi, bool shouldCatchyTune) => shouldCatchyTune);
		this.catchyTune.Exit(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.shouldCatchyTune.Set(false, smi, false);
		}).ToggleEffect("HeardJoySinger").ToggleThought(Db.Get().Thoughts.CatchyTune, null).EventHandler(GameHashes.StartWork, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.TryThinkCatchyTune)).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HeardJoySinger, null).Enter(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.SingCatchyTune(smi);
		}).Update(delegate(InspirationEffectMonitor.Instance smi, float dt)
		{
			this.TryThinkCatchyTune(smi, null);
			this.inspirationTimeRemaining.Delta(-dt, smi);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<float>(this.inspirationTimeRemaining, this.idle, (InspirationEffectMonitor.Instance smi, float p) => p <= 0f);
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x00197E47 File Offset: 0x00196047
	private void OnCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		this.inspirationTimeRemaining.Set(600f, smi, false);
		this.shouldCatchyTune.Set(true, smi, false);
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x00197E6B File Offset: 0x0019606B
	private void TryThinkCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		if (UnityEngine.Random.Range(1, 101) > 66)
		{
			this.SingCatchyTune(smi);
		}
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x00197E80 File Offset: 0x00196080
	private void SingCatchyTune(InspirationEffectMonitor.Instance smi)
	{
		smi.master.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.CatchyTune);
		if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
		{
			smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
		}
	}

	// Token: 0x04002E8A RID: 11914
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.BoolParameter shouldCatchyTune;

	// Token: 0x04002E8B RID: 11915
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.FloatParameter inspirationTimeRemaining;

	// Token: 0x04002E8C RID: 11916
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State idle;

	// Token: 0x04002E8D RID: 11917
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State catchyTune;

	// Token: 0x02001949 RID: 6473
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200194A RID: 6474
	public new class Instance : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameInstance
	{
		// Token: 0x06009C01 RID: 39937 RVA: 0x0037175C File Offset: 0x0036F95C
		public Instance(IStateMachineTarget master, InspirationEffectMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009C02 RID: 39938 RVA: 0x00371766 File Offset: 0x0036F966
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x04007902 RID: 30978
		public SpeechMonitor.Instance speechMonitor;
	}
}
