using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000702 RID: 1794
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicClusterLocationSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06002E06 RID: 11782 RVA: 0x00102759 File Offset: 0x00100959
	public bool ActiveInSpace
	{
		get
		{
			return this.activeInSpace;
		}
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x00102761 File Offset: 0x00100961
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicClusterLocationSensor>(-905833192, LogicClusterLocationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x0010277C File Offset: 0x0010097C
	private void OnCopySettings(object data)
	{
		LogicClusterLocationSensor component = ((GameObject)data).GetComponent<LogicClusterLocationSensor>();
		if (component != null)
		{
			this.activeLocations.Clear();
			for (int i = 0; i < component.activeLocations.Count; i++)
			{
				this.SetLocationEnabled(component.activeLocations[i], true);
			}
			this.activeInSpace = component.activeInSpace;
		}
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x001027DE File Offset: 0x001009DE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x00102811 File Offset: 0x00100A11
	public void SetLocationEnabled(AxialI location, bool setting)
	{
		if (!setting)
		{
			this.activeLocations.Remove(location);
			return;
		}
		if (!this.activeLocations.Contains(location))
		{
			this.activeLocations.Add(location);
		}
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x0010283E File Offset: 0x00100A3E
	public void SetSpaceEnabled(bool setting)
	{
		this.activeInSpace = setting;
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x00102848 File Offset: 0x00100A48
	public void Sim200ms(float dt)
	{
		bool state = this.CheckCurrentLocationSelected();
		this.SetState(state);
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x00102864 File Offset: 0x00100A64
	private bool CheckCurrentLocationSelected()
	{
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		return this.activeLocations.Contains(myWorldLocation) || (this.activeInSpace && this.CheckInEmptySpace());
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x001028A0 File Offset: 0x00100AA0
	private bool CheckInEmptySpace()
	{
		bool result = true;
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x00102914 File Offset: 0x00100B14
	public bool CheckLocationSelected(AxialI location)
	{
		return this.activeLocations.Contains(location);
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x00102922 File Offset: 0x00100B22
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E11 RID: 11793 RVA: 0x00102931 File Offset: 0x00100B31
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002E12 RID: 11794 RVA: 0x00102950 File Offset: 0x00100B50
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			bool flag = true;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				component.Play(this.switchedOn ? "on_space_pre" : "on_space_pst", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(this.switchedOn ? "on_space" : "off_space", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play(this.switchedOn ? "on_asteroid_pre" : "on_asteroid_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on_asteroid" : "off_asteroid", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x00102A9C File Offset: 0x00100C9C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001ADD RID: 6877
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001ADE RID: 6878
	[Serialize]
	private List<AxialI> activeLocations = new List<AxialI>();

	// Token: 0x04001ADF RID: 6879
	[Serialize]
	private bool activeInSpace = true;

	// Token: 0x04001AE0 RID: 6880
	private bool wasOn;

	// Token: 0x04001AE1 RID: 6881
	private static readonly EventSystem.IntraObjectHandler<LogicClusterLocationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicClusterLocationSensor>(delegate(LogicClusterLocationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
