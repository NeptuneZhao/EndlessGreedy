using System;
using UnityEngine;

// Token: 0x020008D0 RID: 2256
public class ElementSpout : StateMachineComponent<ElementSpout.StatesInstance>
{
	// Token: 0x06004025 RID: 16421 RVA: 0x0016B2B0 File Offset: 0x001694B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Grid.Objects[cell, 2] = base.gameObject;
		base.smi.StartSM();
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x0016B2F1 File Offset: 0x001694F1
	public void SetEmitter(ElementEmitter emitter)
	{
		this.emitter = emitter;
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x0016B2FA File Offset: 0x001694FA
	public void ConfigureEmissionSettings(float emissionPollFrequency = 3f, float emissionIrregularity = 1.5f, float maxPressure = 1.5f, float perEmitAmount = 0.5f)
	{
		this.maxPressure = maxPressure;
		this.emissionPollFrequency = emissionPollFrequency;
		this.emissionIrregularity = emissionIrregularity;
		this.perEmitAmount = perEmitAmount;
	}

	// Token: 0x04002A60 RID: 10848
	[SerializeField]
	private ElementEmitter emitter;

	// Token: 0x04002A61 RID: 10849
	[MyCmpAdd]
	private KBatchedAnimController anim;

	// Token: 0x04002A62 RID: 10850
	public float maxPressure = 1.5f;

	// Token: 0x04002A63 RID: 10851
	public float emissionPollFrequency = 3f;

	// Token: 0x04002A64 RID: 10852
	public float emissionIrregularity = 1.5f;

	// Token: 0x04002A65 RID: 10853
	public float perEmitAmount = 0.5f;

	// Token: 0x02001806 RID: 6150
	public class StatesInstance : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.GameInstance
	{
		// Token: 0x06009739 RID: 38713 RVA: 0x003648C3 File Offset: 0x00362AC3
		public StatesInstance(ElementSpout smi) : base(smi)
		{
		}

		// Token: 0x0600973A RID: 38714 RVA: 0x003648CC File Offset: 0x00362ACC
		private bool CanEmitOnCell(int cell, float max_pressure, Element.State expected_state)
		{
			return Grid.Mass[cell] < max_pressure && (Grid.Element[cell].IsState(expected_state) || Grid.Element[cell].IsVacuum);
		}

		// Token: 0x0600973B RID: 38715 RVA: 0x003648FC File Offset: 0x00362AFC
		public bool CanEmitAnywhere()
		{
			int cell = Grid.PosToCell(base.smi.transform.GetPosition());
			int cell2 = Grid.CellLeft(cell);
			int cell3 = Grid.CellRight(cell);
			int cell4 = Grid.CellAbove(cell);
			Element.State state = ElementLoader.FindElementByHash(base.smi.master.emitter.outputElement.elementHash).state;
			return false || this.CanEmitOnCell(cell, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell2, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell3, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell4, base.smi.master.maxPressure, state);
		}
	}

	// Token: 0x02001807 RID: 6151
	public class States : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout>
	{
		// Token: 0x0600973C RID: 38716 RVA: 0x003649D4 File Offset: 0x00362BD4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.DefaultState(this.idle.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
			}).ScheduleGoTo((ElementSpout.StatesInstance smi) => smi.master.emissionPollFrequency, this.emit);
			this.idle.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.idle.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.DefaultState(this.emit.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				float num = 1f + UnityEngine.Random.Range(0f, smi.master.emissionIrregularity);
				float massGenerationRate = smi.master.perEmitAmount / num;
				smi.master.emitter.SetEmitting(true);
				smi.master.emitter.emissionFrequency = 1f;
				smi.master.emitter.outputElement.massGenerationRate = massGenerationRate;
				smi.ScheduleGoTo(num, this.idle);
			});
			this.emit.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutEmitting, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("emit", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(true);
			}).Transition(this.emit.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(false);
			}).Transition(this.emit.unblocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
		}

		// Token: 0x040074BB RID: 29883
		public ElementSpout.States.Idle idle;

		// Token: 0x040074BC RID: 29884
		public ElementSpout.States.Emitting emit;

		// Token: 0x0200259A RID: 9626
		public class Idle : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x0400A773 RID: 42867
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x0400A774 RID: 42868
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}

		// Token: 0x0200259B RID: 9627
		public class Emitting : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x0400A775 RID: 42869
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x0400A776 RID: 42870
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}
	}
}
