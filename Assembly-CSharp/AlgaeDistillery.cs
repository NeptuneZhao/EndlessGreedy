using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200067C RID: 1660
[SerializationConfig(MemberSerialization.OptIn)]
public class AlgaeDistillery : StateMachineComponent<AlgaeDistillery.StatesInstance>
{
	// Token: 0x0600291D RID: 10525 RVA: 0x000E8A7E File Offset: 0x000E6C7E
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x040017A7 RID: 6055
	[SerializeField]
	public Tag emitTag;

	// Token: 0x040017A8 RID: 6056
	[SerializeField]
	public float emitMass;

	// Token: 0x040017A9 RID: 6057
	[SerializeField]
	public Vector3 emitOffset;

	// Token: 0x040017AA RID: 6058
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040017AB RID: 6059
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x040017AC RID: 6060
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0200145F RID: 5215
	public class StatesInstance : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.GameInstance
	{
		// Token: 0x06008A6B RID: 35435 RVA: 0x00333904 File Offset: 0x00331B04
		public StatesInstance(AlgaeDistillery smi) : base(smi)
		{
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x00333910 File Offset: 0x00331B10
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				storage.Drop(gameObject, true).transform.SetPosition(base.transform.GetPosition() + base.master.emitOffset);
			}
		}
	}

	// Token: 0x02001460 RID: 5216
	public class States : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery>
	{
		// Token: 0x06008A6D RID: 35437 RVA: 0x00333994 File Offset: 0x00331B94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (AlgaeDistillery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (AlgaeDistillery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (AlgaeDistillery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (AlgaeDistillery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x04006992 RID: 27026
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State disabled;

		// Token: 0x04006993 RID: 27027
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State waiting;

		// Token: 0x04006994 RID: 27028
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State converting;

		// Token: 0x04006995 RID: 27029
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State overpressure;
	}
}
