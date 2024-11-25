using System;
using UnityEngine;

// Token: 0x02000AA4 RID: 2724
[SkipSaveFileSerialization]
public class Snorer : StateMachineComponent<Snorer.StatesInstance>
{
	// Token: 0x0600502F RID: 20527 RVA: 0x001CD03E File Offset: 0x001CB23E
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04003545 RID: 13637
	private static readonly HashedString HeadHash = "snapTo_mouth";

	// Token: 0x02001AD5 RID: 6869
	public class StatesInstance : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.GameInstance
	{
		// Token: 0x0600A13B RID: 41275 RVA: 0x00382659 File Offset: 0x00380859
		public StatesInstance(Snorer master) : base(master)
		{
		}

		// Token: 0x0600A13C RID: 41276 RVA: 0x00382664 File Offset: 0x00380864
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x0600A13D RID: 41277 RVA: 0x00382688 File Offset: 0x00380888
		public void StartSmallSnore()
		{
			this.snoreHandle = GameScheduler.Instance.Schedule("snorelines", 2f, new Action<object>(this.StartSmallSnoreInternal), null, null);
		}

		// Token: 0x0600A13E RID: 41278 RVA: 0x003826B4 File Offset: 0x003808B4
		private void StartSmallSnoreInternal(object data)
		{
			this.snoreHandle.ClearScheduler();
			bool flag;
			Matrix4x4 symbolTransform = base.smi.master.GetComponent<KBatchedAnimController>().GetSymbolTransform(Snorer.HeadHash, out flag);
			if (flag)
			{
				Vector3 position = symbolTransform.GetColumn(3);
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				this.snoreEffect = FXHelpers.CreateEffect("snore_fx_kanim", position, null, false, Grid.SceneLayer.Front, false);
				this.snoreEffect.destroyOnAnimComplete = true;
				this.snoreEffect.Play("snore", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600A13F RID: 41279 RVA: 0x0038274A File Offset: 0x0038094A
		public void StopSmallSnore()
		{
			this.snoreHandle.ClearScheduler();
			if (this.snoreEffect != null)
			{
				this.snoreEffect.PlayMode = KAnim.PlayMode.Once;
			}
			this.snoreEffect = null;
		}

		// Token: 0x0600A140 RID: 41280 RVA: 0x00382778 File Offset: 0x00380978
		public void StartSnoreBGEffect()
		{
			AcousticDisturbance.Emit(base.smi.master.gameObject, 3);
		}

		// Token: 0x0600A141 RID: 41281 RVA: 0x00382790 File Offset: 0x00380990
		public void StopSnoreBGEffect()
		{
		}

		// Token: 0x04007DE1 RID: 32225
		private SchedulerHandle snoreHandle;

		// Token: 0x04007DE2 RID: 32226
		private KBatchedAnimController snoreEffect;

		// Token: 0x04007DE3 RID: 32227
		private KBatchedAnimController snoreBGEffect;

		// Token: 0x04007DE4 RID: 32228
		private const float BGEmissionRadius = 3f;
	}

	// Token: 0x02001AD6 RID: 6870
	public class States : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer>
	{
		// Token: 0x0600A142 RID: 41282 RVA: 0x00382794 File Offset: 0x00380994
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.sleeping, (Snorer.StatesInstance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.DefaultState(this.sleeping.quiet).Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSmallSnore();
			}).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSmallSnore();
			}).Transition(this.idle, (Snorer.StatesInstance smi) => !smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.quiet.Enter("ScheduleNextSnore", delegate(Snorer.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.sleeping.snoring);
			});
			this.sleeping.snoring.Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSnoreBGEffect();
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.sleeping.quiet).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSnoreBGEffect();
			});
		}

		// Token: 0x0600A143 RID: 41283 RVA: 0x00382918 File Offset: 0x00380B18
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(5f, 1f), 3f), 10f);
		}

		// Token: 0x04007DE5 RID: 32229
		public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State idle;

		// Token: 0x04007DE6 RID: 32230
		public Snorer.States.SleepStates sleeping;

		// Token: 0x02002605 RID: 9733
		public class SleepStates : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State
		{
			// Token: 0x0400A936 RID: 43318
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State quiet;

			// Token: 0x0400A937 RID: 43319
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State snoring;
		}
	}
}
