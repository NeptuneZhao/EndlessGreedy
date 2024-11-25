using System;

// Token: 0x02000775 RID: 1909
public class StorageLockerSmart : StorageLocker
{
	// Token: 0x0600339C RID: 13212 RVA: 0x0011AD49 File Offset: 0x00118F49
	protected override void OnPrefabInit()
	{
		base.Initialize(true);
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x0011AD54 File Offset: 0x00118F54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ports = base.gameObject.GetComponent<LogicPorts>();
		base.Subscribe<StorageLockerSmart>(-1697596308, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		base.Subscribe<StorageLockerSmart>(-592767678, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x0011ADA0 File Offset: 0x00118FA0
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0011ADA8 File Offset: 0x00118FA8
	private void UpdateLogicAndActiveState()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
		this.operational.SetActive(isOperational, false);
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x060033A0 RID: 13216 RVA: 0x0011ADFF File Offset: 0x00118FFF
	// (set) Token: 0x060033A1 RID: 13217 RVA: 0x0011AE07 File Offset: 0x00119007
	public override float UserMaxCapacity
	{
		get
		{
			return base.UserMaxCapacity;
		}
		set
		{
			base.UserMaxCapacity = value;
			this.UpdateLogicAndActiveState();
		}
	}

	// Token: 0x04001E93 RID: 7827
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001E94 RID: 7828
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001E95 RID: 7829
	private static readonly EventSystem.IntraObjectHandler<StorageLockerSmart> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<StorageLockerSmart>(delegate(StorageLockerSmart component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
