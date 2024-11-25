using System;
using UnityEngine;

// Token: 0x02000A4A RID: 2634
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06004C53 RID: 19539 RVA: 0x001B40B7 File Offset: 0x001B22B7
	public bool RequiresPower
	{
		get
		{
			return this.requirePower;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06004C54 RID: 19540 RVA: 0x001B40BF File Offset: 0x001B22BF
	public bool RequiresInputConduit
	{
		get
		{
			return this.requireConduit;
		}
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x001B40C7 File Offset: 0x001B22C7
	public void SetRequirements(bool power, bool conduit)
	{
		this.requirePower = power;
		this.requireConduit = conduit;
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06004C56 RID: 19542 RVA: 0x001B40D7 File Offset: 0x001B22D7
	public bool RequirementsMet
	{
		get
		{
			return this.requirementsMet;
		}
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x001B40DF File Offset: 0x001B22DF
	protected override void OnPrefabInit()
	{
		this.Bind();
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001B40E7 File Offset: 0x001B22E7
	protected override void OnSpawn()
	{
		this.CheckRequirements(true);
		this.Bind();
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x001B40F8 File Offset: 0x001B22F8
	[ContextMenu("Bind")]
	private void Bind()
	{
		if (this.requirePower)
		{
			this.energy = base.GetComponent<IEnergyConsumer>();
			this.button = base.GetComponent<BuildingEnabledButton>();
		}
		if (this.requireConduit && !this.conduitConsumer)
		{
			this.conduitConsumer = base.GetComponent<ConduitConsumer>();
		}
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x001B4146 File Offset: 0x001B2346
	public void Sim200ms(float dt)
	{
		this.CheckRequirements(false);
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x001B4150 File Offset: 0x001B2350
	private void CheckRequirements(bool forceEvent)
	{
		bool flag = true;
		bool flag2 = false;
		if (this.requirePower)
		{
			bool isConnected = this.energy.IsConnected;
			bool isPowered = this.energy.IsPowered;
			flag = (flag && isPowered && isConnected);
			bool show = this.VisualizeRequirement(RequireInputs.Requirements.NeedPower) && isConnected && !isPowered && (this.button == null || this.button.IsEnabled);
			bool show2 = this.VisualizeRequirement(RequireInputs.Requirements.NoWire) && !isConnected;
			this.needPowerStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, this.needPowerStatusGuid, show, this);
			this.noWireStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, this.noWireStatusGuid, show2, this);
			flag2 = (flag != this.RequirementsMet && base.GetComponent<Light2D>() != null);
		}
		if (this.requireConduit)
		{
			bool flag3 = !this.conduitConsumer.enabled || this.conduitConsumer.IsConnected;
			bool flag4 = !this.conduitConsumer.enabled || this.conduitConsumer.IsSatisfied;
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) && this.previouslyConnected != flag3)
			{
				this.previouslyConnected = flag3;
				StatusItem statusItem = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem = Db.Get().BuildingStatusItems.NeedLiquidIn;
					}
				}
				else
				{
					statusItem = Db.Get().BuildingStatusItems.NeedGasIn;
				}
				if (statusItem != null)
				{
					this.selectable.ToggleStatusItem(statusItem, !flag3, new global::Tuple<ConduitType, Tag>(this.conduitConsumer.TypeOfConduit, this.conduitConsumer.capacityTag));
				}
				this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag3);
			}
			flag = (flag && flag3);
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) && this.previouslySatisfied != flag4)
			{
				this.previouslySatisfied = flag4;
				StatusItem statusItem2 = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem2 = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
					}
				}
				else
				{
					statusItem2 = Db.Get().BuildingStatusItems.GasPipeEmpty;
				}
				if (this.requireConduitHasMass)
				{
					if (statusItem2 != null)
					{
						this.selectable.ToggleStatusItem(statusItem2, !flag4, this);
					}
					this.operational.SetFlag(RequireInputs.pipesHaveMass, flag4);
				}
			}
		}
		this.requirementsMet = flag;
		if (flag2)
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject != null)
			{
				Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
			}
		}
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001B43EC File Offset: 0x001B25EC
	public bool VisualizeRequirement(RequireInputs.Requirements r)
	{
		return (this.visualizeRequirements & r) == r;
	}

	// Token: 0x040032C0 RID: 12992
	[SerializeField]
	private bool requirePower = true;

	// Token: 0x040032C1 RID: 12993
	[SerializeField]
	private bool requireConduit;

	// Token: 0x040032C2 RID: 12994
	public bool requireConduitHasMass = true;

	// Token: 0x040032C3 RID: 12995
	public RequireInputs.Requirements visualizeRequirements = RequireInputs.Requirements.All;

	// Token: 0x040032C4 RID: 12996
	private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);

	// Token: 0x040032C5 RID: 12997
	private static readonly Operational.Flag pipesHaveMass = new Operational.Flag("pipesHaveMass", Operational.Flag.Type.Requirement);

	// Token: 0x040032C6 RID: 12998
	private Guid noWireStatusGuid;

	// Token: 0x040032C7 RID: 12999
	private Guid needPowerStatusGuid;

	// Token: 0x040032C8 RID: 13000
	private bool requirementsMet;

	// Token: 0x040032C9 RID: 13001
	private BuildingEnabledButton button;

	// Token: 0x040032CA RID: 13002
	private IEnergyConsumer energy;

	// Token: 0x040032CB RID: 13003
	public ConduitConsumer conduitConsumer;

	// Token: 0x040032CC RID: 13004
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040032CD RID: 13005
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040032CE RID: 13006
	private bool previouslyConnected = true;

	// Token: 0x040032CF RID: 13007
	private bool previouslySatisfied = true;

	// Token: 0x02001A50 RID: 6736
	[Flags]
	public enum Requirements
	{
		// Token: 0x04007C00 RID: 31744
		None = 0,
		// Token: 0x04007C01 RID: 31745
		NoWire = 1,
		// Token: 0x04007C02 RID: 31746
		NeedPower = 2,
		// Token: 0x04007C03 RID: 31747
		ConduitConnected = 4,
		// Token: 0x04007C04 RID: 31748
		ConduitEmpty = 8,
		// Token: 0x04007C05 RID: 31749
		AllPower = 3,
		// Token: 0x04007C06 RID: 31750
		AllConduit = 12,
		// Token: 0x04007C07 RID: 31751
		All = 15
	}
}
