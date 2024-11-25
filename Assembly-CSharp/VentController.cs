using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class VentController : GameStateMachine<VentController, VentController.Instance>
{
	// Token: 0x06000197 RID: 407 RVA: 0x0000AE90 File Offset: 0x00009090
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventHandler(GameHashes.VentAnimatingChanged, new GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameEvent.Callback(VentController.UpdateMeterColor)).EventTransition(GameHashes.VentClosed, this.closed, (VentController.Instance smi) => smi.GetComponent<Vent>().Closed()).EventTransition(GameHashes.VentOpen, this.off, (VentController.Instance smi) => !smi.GetComponent<Vent>().Closed());
		this.off.PlayAnim("off").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating));
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State.Callback(VentController.PlayOutputMeterAnim)).EventTransition(GameHashes.VentAnimatingChanged, this.working_pst, GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Not(new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating)));
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.closed.PlayAnim("closed").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating));
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000AFF6 File Offset: 0x000091F6
	public static void PlayOutputMeterAnim(VentController.Instance smi)
	{
		smi.PlayMeterAnim();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000AFFE File Offset: 0x000091FE
	public static bool IsAnimating(VentController.Instance smi)
	{
		return smi.exhaust.IsAnimating();
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000B00C File Offset: 0x0000920C
	public static void UpdateMeterColor(VentController.Instance smi, object data)
	{
		if (data != null)
		{
			Color32 meterOutputColor = (Color32)data;
			smi.SetMeterOutputColor(meterOutputColor);
		}
	}

	// Token: 0x04000106 RID: 262
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000107 RID: 263
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000108 RID: 264
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000109 RID: 265
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x0400010A RID: 266
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State closed;

	// Token: 0x0400010B RID: 267
	public StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.BoolParameter isAnimating;

	// Token: 0x02000FB8 RID: 4024
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B5E RID: 23390
		public bool usingDynamicColor;

		// Token: 0x04005B5F RID: 23391
		public string outputSubstanceAnimName;
	}

	// Token: 0x02000FB9 RID: 4025
	public new class Instance : GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007A58 RID: 31320 RVA: 0x00301F04 File Offset: 0x00300104
		public Instance(IStateMachineTarget master, VentController.Def def) : base(master, def)
		{
			if (def.usingDynamicColor)
			{
				this.outputSubstanceMeter = new MeterController(this.anim, "meter_target", def.outputSubstanceAnimName, Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			}
		}

		// Token: 0x06007A59 RID: 31321 RVA: 0x00301F3A File Offset: 0x0030013A
		public void PlayMeterAnim()
		{
			if (this.outputSubstanceMeter != null)
			{
				this.outputSubstanceMeter.meterController.Play(this.outputSubstanceMeter.meterController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06007A5A RID: 31322 RVA: 0x00301F74 File Offset: 0x00300174
		public void SetMeterOutputColor(Color32 color)
		{
			if (this.outputSubstanceMeter != null)
			{
				this.outputSubstanceMeter.meterController.TintColour = color;
			}
		}

		// Token: 0x04005B60 RID: 23392
		[MyCmpGet]
		private KBatchedAnimController anim;

		// Token: 0x04005B61 RID: 23393
		[MyCmpGet]
		public Exhaust exhaust;

		// Token: 0x04005B62 RID: 23394
		private MeterController outputSubstanceMeter;
	}
}
