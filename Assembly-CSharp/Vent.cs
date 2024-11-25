using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B51 RID: 2897
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x0600568F RID: 22159 RVA: 0x001EEFBF File Offset: 0x001ED1BF
	// (set) Token: 0x06005690 RID: 22160 RVA: 0x001EEFC7 File Offset: 0x001ED1C7
	public int SortKey
	{
		get
		{
			return this.sortKey;
		}
		set
		{
			this.sortKey = value;
		}
	}

	// Token: 0x06005691 RID: 22161 RVA: 0x001EEFD0 File Offset: 0x001ED1D0
	public void UpdateVentedMass(SimHashes element, float mass)
	{
		if (!this.lifeTimeVentMass.ContainsKey(element))
		{
			this.lifeTimeVentMass.Add(element, mass);
			return;
		}
		Dictionary<SimHashes, float> dictionary = this.lifeTimeVentMass;
		dictionary[element] += mass;
	}

	// Token: 0x06005692 RID: 22162 RVA: 0x001EF012 File Offset: 0x001ED212
	public float GetVentedMass(SimHashes element)
	{
		if (this.lifeTimeVentMass.ContainsKey(element))
		{
			return this.lifeTimeVentMass[element];
		}
		return 0f;
	}

	// Token: 0x06005693 RID: 22163 RVA: 0x001EF034 File Offset: 0x001ED234
	public bool Closed()
	{
		bool flag = false;
		return (this.operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag) || (this.operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag, out flag) && !flag);
	}

	// Token: 0x06005694 RID: 22164 RVA: 0x001EF080 File Offset: 0x001ED280
	protected override void OnSpawn()
	{
		Building component = base.GetComponent<Building>();
		this.cell = component.GetUtilityOutputCell();
		this.smi = new Vent.StatesInstance(this);
		this.smi.StartSM();
	}

	// Token: 0x06005695 RID: 22165 RVA: 0x001EF0B8 File Offset: 0x001ED2B8
	public Vent.State GetEndPointState()
	{
		Vent.State result = Vent.State.Invalid;
		Endpoint endpoint = this.endpointType;
		if (endpoint != Endpoint.Source)
		{
			if (endpoint == Endpoint.Sink)
			{
				result = Vent.State.Ready;
				int num = this.cell;
				if (!this.IsValidOutputCell(num))
				{
					result = (Grid.Solid[num] ? Vent.State.Blocked : Vent.State.OverPressure);
				}
			}
		}
		else
		{
			result = (this.IsConnected() ? Vent.State.Ready : Vent.State.Blocked);
		}
		return result;
	}

	// Token: 0x06005696 RID: 22166 RVA: 0x001EF10C File Offset: 0x001ED30C
	public bool IsConnected()
	{
		UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
		return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06005697 RID: 22167 RVA: 0x001EF140 File Offset: 0x001ED340
	public bool IsBlocked
	{
		get
		{
			return this.GetEndPointState() != Vent.State.Ready;
		}
	}

	// Token: 0x06005698 RID: 22168 RVA: 0x001EF150 File Offset: 0x001ED350
	private bool IsValidOutputCell(int output_cell)
	{
		bool result = false;
		if ((this.structure == null || !this.structure.IsEntombed() || !this.Closed()) && !Grid.Solid[output_cell])
		{
			result = (Grid.Mass[output_cell] < this.overpressureMass);
		}
		return result;
	}

	// Token: 0x06005699 RID: 22169 RVA: 0x001EF1A4 File Offset: 0x001ED3A4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, formattedMass), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x0400389A RID: 14490
	private int cell = -1;

	// Token: 0x0400389B RID: 14491
	private int sortKey;

	// Token: 0x0400389C RID: 14492
	[Serialize]
	public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();

	// Token: 0x0400389D RID: 14493
	private Vent.StatesInstance smi;

	// Token: 0x0400389E RID: 14494
	[SerializeField]
	public ConduitType conduitType = ConduitType.Gas;

	// Token: 0x0400389F RID: 14495
	[SerializeField]
	public Endpoint endpointType;

	// Token: 0x040038A0 RID: 14496
	[SerializeField]
	public float overpressureMass = 1f;

	// Token: 0x040038A1 RID: 14497
	[NonSerialized]
	public bool showConnectivityIcons = true;

	// Token: 0x040038A2 RID: 14498
	[MyCmpGet]
	[NonSerialized]
	public Structure structure;

	// Token: 0x040038A3 RID: 14499
	[MyCmpGet]
	[NonSerialized]
	public Operational operational;

	// Token: 0x02001BA1 RID: 7073
	public enum State
	{
		// Token: 0x04008040 RID: 32832
		Invalid,
		// Token: 0x04008041 RID: 32833
		Ready,
		// Token: 0x04008042 RID: 32834
		Blocked,
		// Token: 0x04008043 RID: 32835
		OverPressure,
		// Token: 0x04008044 RID: 32836
		Closed
	}

	// Token: 0x02001BA2 RID: 7074
	public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
	{
		// Token: 0x0600A3F5 RID: 41973 RVA: 0x0038AEB1 File Offset: 0x003890B1
		public StatesInstance(Vent master) : base(master)
		{
			this.exhaust = master.GetComponent<Exhaust>();
		}

		// Token: 0x0600A3F6 RID: 41974 RVA: 0x0038AEC6 File Offset: 0x003890C6
		public bool NeedsExhaust()
		{
			return this.exhaust != null && base.master.GetEndPointState() != Vent.State.Ready && base.master.endpointType == Endpoint.Source;
		}

		// Token: 0x0600A3F7 RID: 41975 RVA: 0x0038AEF4 File Offset: 0x003890F4
		public bool Blocked()
		{
			return base.master.GetEndPointState() == Vent.State.Blocked && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x0600A3F8 RID: 41976 RVA: 0x0038AF14 File Offset: 0x00389114
		public bool OverPressure()
		{
			return this.exhaust != null && base.master.GetEndPointState() == Vent.State.OverPressure && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x0600A3F9 RID: 41977 RVA: 0x0038AF44 File Offset: 0x00389144
		public void CheckTransitions()
		{
			if (this.NeedsExhaust())
			{
				base.smi.GoTo(base.sm.needExhaust);
				return;
			}
			if (base.master.Closed())
			{
				base.smi.GoTo(base.sm.closed);
				return;
			}
			if (this.Blocked())
			{
				base.smi.GoTo(base.sm.open.blocked);
				return;
			}
			if (this.OverPressure())
			{
				base.smi.GoTo(base.sm.open.overPressure);
				return;
			}
			base.smi.GoTo(base.sm.open.idle);
		}

		// Token: 0x0600A3FA RID: 41978 RVA: 0x0038AFF7 File Offset: 0x003891F7
		public StatusItem SelectStatusItem(StatusItem gas_status_item, StatusItem liquid_status_item)
		{
			if (base.master.conduitType != ConduitType.Gas)
			{
				return liquid_status_item;
			}
			return gas_status_item;
		}

		// Token: 0x04008045 RID: 32837
		private Exhaust exhaust;
	}

	// Token: 0x02001BA3 RID: 7075
	public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
	{
		// Token: 0x0600A3FB RID: 41979 RVA: 0x0038B00C File Offset: 0x0038920C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.open.idle;
			this.root.Update("CheckTransitions", delegate(Vent.StatesInstance smi, float dt)
			{
				smi.CheckTransitions();
			}, UpdateRate.SIM_200ms, false);
			this.open.TriggerOnEnter(GameHashes.VentOpen, null);
			this.closed.TriggerOnEnter(GameHashes.VentClosed, null);
			this.open.blocked.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed), null);
			this.open.overPressure.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure), null);
		}

		// Token: 0x04008046 RID: 32838
		public Vent.States.OpenState open;

		// Token: 0x04008047 RID: 32839
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State closed;

		// Token: 0x04008048 RID: 32840
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;

		// Token: 0x02002627 RID: 9767
		public class OpenState : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State
		{
			// Token: 0x0400A9C4 RID: 43460
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;

			// Token: 0x0400A9C5 RID: 43461
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;

			// Token: 0x0400A9C6 RID: 43462
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
		}
	}
}
