using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009EA RID: 2538
public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004992 RID: 18834 RVA: 0x001A56A7 File Offset: 0x001A38A7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004993 RID: 18835 RVA: 0x001A56BA File Offset: 0x001A38BA
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04003023 RID: 12323
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04003024 RID: 12324
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003025 RID: 12325
	public SimHashes emittedElement;

	// Token: 0x04003026 RID: 12326
	public float emitRate;

	// Token: 0x020019ED RID: 6637
	public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
	{
		// Token: 0x06009E72 RID: 40562 RVA: 0x00377F3B File Offset: 0x0037613B
		public StatesInstance(PlantElementEmitter master) : base(master)
		{
		}

		// Token: 0x06009E73 RID: 40563 RVA: 0x00377F44 File Offset: 0x00376144
		public bool IsWilting()
		{
			return !(base.master.wiltCondition == null) && base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}
	}

	// Token: 0x020019EE RID: 6638
	public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
	{
		// Token: 0x06009E74 RID: 40564 RVA: 0x00377F80 File Offset: 0x00376180
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.healthy;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (PlantElementEmitter.StatesInstance smi) => smi.IsWilting()).Update("PlantEmit", delegate(PlantElementEmitter.StatesInstance smi, float dt)
			{
				SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
			}, UpdateRate.SIM_4000ms, false);
			this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy, null);
		}

		// Token: 0x04007ADF RID: 31455
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;

		// Token: 0x04007AE0 RID: 31456
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;
	}
}
