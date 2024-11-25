using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06002AD0 RID: 10960
	public abstract float CurrentValue { get; }

	// Token: 0x06002AD1 RID: 10961 RVA: 0x000F139F File Offset: 0x000EF59F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000F13B8 File Offset: 0x000EF5B8
	private void OnCopySettings(object data)
	{
		ConduitThresholdSensor component = ((GameObject)data).GetComponent<ConduitThresholdSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x000F13F4 File Offset: 0x000EF5F4
	protected override void ConduitUpdate(float dt)
	{
		if (this.GetContainedMass() <= 0f && !this.dirty)
		{
			return;
		}
		float currentValue = this.CurrentValue;
		this.dirty = false;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x000F1480 File Offset: 0x000EF680
	private float GetContainedMass()
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			return Conduit.GetFlowManager(this.conduitType).GetContents(cell).mass;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents.pickupableHandle);
		if (pickupable != null)
		{
			return pickupable.PrimaryElement.Mass;
		}
		return 0f;
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x000F14F3 File Offset: 0x000EF6F3
	// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x000F14FB File Offset: 0x000EF6FB
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000F150B File Offset: 0x000EF70B
	// (set) Token: 0x06002AD8 RID: 10968 RVA: 0x000F1513 File Offset: 0x000EF713
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x040018A5 RID: 6309
	[SerializeField]
	[Serialize]
	protected float threshold;

	// Token: 0x040018A6 RID: 6310
	[SerializeField]
	[Serialize]
	protected bool activateAboveThreshold = true;

	// Token: 0x040018A7 RID: 6311
	[Serialize]
	private bool dirty = true;

	// Token: 0x040018A8 RID: 6312
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040018A9 RID: 6313
	private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>(delegate(ConduitThresholdSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
