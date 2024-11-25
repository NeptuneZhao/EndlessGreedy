using System;
using STRINGS;
using UnityEngine;

// Token: 0x020008E6 RID: 2278
public class HEPFuelTank : KMonoBehaviour, IFuelTank, IUserControlledCapacity
{
	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06004144 RID: 16708 RVA: 0x00172F40 File Offset: 0x00171140
	public IStorage Storage
	{
		get
		{
			return this.hepStorage;
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06004145 RID: 16709 RVA: 0x00172F48 File Offset: 0x00171148
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x00172F50 File Offset: 0x00171150
	public void DEBUG_FillTank()
	{
		this.hepStorage.Store(this.hepStorage.RemainingCapacity());
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06004147 RID: 16711 RVA: 0x00172F69 File Offset: 0x00171169
	// (set) Token: 0x06004148 RID: 16712 RVA: 0x00172F76 File Offset: 0x00171176
	public float UserMaxCapacity
	{
		get
		{
			return this.hepStorage.capacity;
		}
		set
		{
			this.hepStorage.capacity = value;
			base.Trigger(-795826715, this);
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06004149 RID: 16713 RVA: 0x00172F90 File Offset: 0x00171190
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x0600414A RID: 16714 RVA: 0x00172F97 File Offset: 0x00171197
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x0600414B RID: 16715 RVA: 0x00172F9F File Offset: 0x0017119F
	public float AmountStored
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x0600414C RID: 16716 RVA: 0x00172FAC File Offset: 0x001711AC
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x0600414D RID: 16717 RVA: 0x00172FAF File Offset: 0x001711AF
	public LocString CapacityUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x00172FB8 File Offset: 0x001711B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		this.m_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.m_meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<HEPFuelTank>(-795826715, HEPFuelTank.OnStorageChangedDelegate);
		base.Subscribe<HEPFuelTank>(-1837862626, HEPFuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x00173061 File Offset: 0x00171261
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HEPFuelTank>(-905833192, HEPFuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x0017307A File Offset: 0x0017127A
	private void OnStorageChange(object data)
	{
		this.m_meter.SetPositionPercent(this.hepStorage.Particles / Mathf.Max(1f, this.hepStorage.capacity));
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x001730A8 File Offset: 0x001712A8
	private void OnCopySettings(object data)
	{
		HEPFuelTank component = ((GameObject)data).GetComponent<HEPFuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x04002B49 RID: 11081
	[MyCmpReq]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04002B4A RID: 11082
	public float physicalFuelCapacity;

	// Token: 0x04002B4B RID: 11083
	private MeterController m_meter;

	// Token: 0x04002B4C RID: 11084
	public bool consumeFuelOnLand;

	// Token: 0x04002B4D RID: 11085
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002B4E RID: 11086
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002B4F RID: 11087
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnCopySettings(data);
	});
}
