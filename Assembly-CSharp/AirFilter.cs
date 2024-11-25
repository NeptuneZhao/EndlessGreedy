using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000644 RID: 1604
[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002743 RID: 10051 RVA: 0x000DF9D7 File Offset: 0x000DDBD7
	public bool HasFilter()
	{
		return this.elementConverter.HasEnoughMass(this.filterTag, false);
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x000DF9EB File Offset: 0x000DDBEB
	public bool IsConvertable()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x000DF9F9 File Offset: 0x000DDBF9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x000DFA0C File Offset: 0x000DDC0C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04001690 RID: 5776
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001691 RID: 5777
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001692 RID: 5778
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04001693 RID: 5779
	[MyCmpGet]
	private ElementConsumer elementConsumer;

	// Token: 0x04001694 RID: 5780
	public Tag filterTag;

	// Token: 0x02001427 RID: 5159
	public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
	{
		// Token: 0x06008994 RID: 35220 RVA: 0x00330CCE File Offset: 0x0032EECE
		public StatesInstance(AirFilter smi) : base(smi)
		{
		}
	}

	// Token: 0x02001428 RID: 5160
	public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
	{
		// Token: 0x06008995 RID: 35221 RVA: 0x00330CD8 File Offset: 0x0032EED8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waiting;
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational);
			this.hasFilter.EventTransition(GameHashes.OperationalChanged, this.waiting, (AirFilter.StatesInstance smi) => !smi.master.operational.IsOperational).Enter("EnableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit("DisableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).DefaultState(this.hasFilter.idle);
			this.hasFilter.idle.EventTransition(GameHashes.OnStorageChange, this.hasFilter.converting, (AirFilter.StatesInstance smi) => smi.master.IsConvertable());
			this.hasFilter.converting.Enter("SetActive(true)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.hasFilter.idle, (AirFilter.StatesInstance smi) => !smi.master.IsConvertable());
		}

		// Token: 0x040068F9 RID: 26873
		public AirFilter.States.ReadyStates hasFilter;

		// Token: 0x040068FA RID: 26874
		public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;

		// Token: 0x020024AD RID: 9389
		public class ReadyStates : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State
		{
			// Token: 0x0400A286 RID: 41606
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State idle;

			// Token: 0x0400A287 RID: 41607
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;
		}
	}
}
