using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000745 RID: 1861
[SerializationConfig(MemberSerialization.OptIn)]
public class OxyliteRefinery : StateMachineComponent<OxyliteRefinery.StatesInstance>
{
	// Token: 0x06003197 RID: 12695 RVA: 0x00110F29 File Offset: 0x0010F129
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04001D27 RID: 7463
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001D28 RID: 7464
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D29 RID: 7465
	public Tag emitTag;

	// Token: 0x04001D2A RID: 7466
	public float emitMass;

	// Token: 0x04001D2B RID: 7467
	public Vector3 dropOffset;

	// Token: 0x020015B2 RID: 5554
	public class StatesInstance : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.GameInstance
	{
		// Token: 0x06008F89 RID: 36745 RVA: 0x00347F78 File Offset: 0x00346178
		public StatesInstance(OxyliteRefinery smi) : base(smi)
		{
		}

		// Token: 0x06008F8A RID: 36746 RVA: 0x00347F84 File Offset: 0x00346184
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				Vector3 position = base.transform.GetPosition() + base.master.dropOffset;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
				storage.Drop(gameObject, true);
			}
		}
	}

	// Token: 0x020015B3 RID: 5555
	public class States : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery>
	{
		// Token: 0x06008F8B RID: 36747 RVA: 0x0034801C File Offset: 0x0034621C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OxyliteRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (OxyliteRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (OxyliteRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Transition(this.waiting, (OxyliteRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x04006D93 RID: 28051
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State disabled;

		// Token: 0x04006D94 RID: 28052
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State waiting;

		// Token: 0x04006D95 RID: 28053
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State converting;
	}
}
