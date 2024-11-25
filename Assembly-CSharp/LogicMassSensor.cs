using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000711 RID: 1809
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
	// Token: 0x06002F30 RID: 12080 RVA: 0x00107BC8 File Offset: 0x00105DC8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002F31 RID: 12081 RVA: 0x00107BE4 File Offset: 0x00105DE4
	private void OnCopySettings(object data)
	{
		LogicMassSensor component = ((GameObject)data).GetComponent<LogicMassSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002F32 RID: 12082 RVA: 0x00107C20 File Offset: 0x00105E20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisualState(true);
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", base.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new Action<object>(this.OnActivatorsChanged));
		base.OnToggle += this.SwitchToggled;
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x00107CEE File Offset: 0x00105EEE
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002F34 RID: 12084 RVA: 0x00107D28 File Offset: 0x00105F28
	private void Update()
	{
		this.toggleCooldown = Mathf.Max(0f, this.toggleCooldown - Time.deltaTime);
		if (this.toggleCooldown == 0f)
		{
			float currentValue = this.CurrentValue;
			if ((this.activateAboveThreshold ? (currentValue > this.threshold) : (currentValue < this.threshold)) != base.IsSwitchedOn)
			{
				this.Toggle();
				this.toggleCooldown = 0.15f;
			}
			this.UpdateVisualState(false);
		}
	}

	// Token: 0x06002F35 RID: 12085 RVA: 0x00107DA4 File Offset: 0x00105FA4
	private void OnSolidChanged(object data)
	{
		int i = Grid.CellAbove(this.NaturalBuildingCell());
		if (Grid.Solid[i])
		{
			this.massSolid = Grid.Mass[i];
			return;
		}
		this.massSolid = 0f;
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x00107DE8 File Offset: 0x00105FE8
	private void OnPickupablesChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (!(pickupable == null) && !pickupable.wasAbsorbed)
			{
				KPrefabID kprefabID = pickupable.KPrefabID;
				if (!kprefabID.HasTag(GameTags.Creature) || (kprefabID.HasTag(GameTags.Creatures.Walker) || kprefabID.HasTag(GameTags.Creatures.Hoverer) || kprefabID.HasTag(GameTags.Creatures.Flopping)))
				{
					num += pickupable.PrimaryElement.Mass;
				}
			}
		}
		pooledList.Recycle();
		this.massPickupables = num;
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x00107ED4 File Offset: 0x001060D4
	private void OnActivatorsChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			FloorSwitchActivator floorSwitchActivator = pooledList[i].obj as FloorSwitchActivator;
			if (!(floorSwitchActivator == null))
			{
				num += floorSwitchActivator.PrimaryElement.Mass;
			}
		}
		pooledList.Recycle();
		this.massActivators = num;
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06002F38 RID: 12088 RVA: 0x00107F70 File Offset: 0x00106170
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06002F39 RID: 12089 RVA: 0x00107F77 File Offset: 0x00106177
	// (set) Token: 0x06002F3A RID: 12090 RVA: 0x00107F7F File Offset: 0x0010617F
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06002F3B RID: 12091 RVA: 0x00107F88 File Offset: 0x00106188
	// (set) Token: 0x06002F3C RID: 12092 RVA: 0x00107F90 File Offset: 0x00106190
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06002F3D RID: 12093 RVA: 0x00107F99 File Offset: 0x00106199
	public float CurrentValue
	{
		get
		{
			return this.massSolid + this.massPickupables + this.massActivators;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06002F3E RID: 12094 RVA: 0x00107FAF File Offset: 0x001061AF
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06002F3F RID: 12095 RVA: 0x00107FB7 File Offset: 0x001061B7
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x00107FBF File Offset: 0x001061BF
	public float GetRangeMinInputField()
	{
		return this.rangeMin;
	}

	// Token: 0x06002F41 RID: 12097 RVA: 0x00107FC7 File Offset: 0x001061C7
	public float GetRangeMaxInputField()
	{
		return this.rangeMax;
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06002F42 RID: 12098 RVA: 0x00107FCF File Offset: 0x001061CF
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06002F43 RID: 12099 RVA: 0x00107FD6 File Offset: 0x001061D6
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06002F44 RID: 12100 RVA: 0x00107FE2 File Offset: 0x001061E2
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x00107FF0 File Offset: 0x001061F0
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.Kilogram;
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x0010800F File Offset: 0x0010620F
	public float ProcessedSliderValue(float input)
	{
		input = Mathf.Round(input);
		return input;
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x0010801A File Offset: 0x0010621A
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002F48 RID: 12104 RVA: 0x0010801D File Offset: 0x0010621D
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(false);
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06002F49 RID: 12105 RVA: 0x00108025 File Offset: 0x00106225
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06002F4A RID: 12106 RVA: 0x00108028 File Offset: 0x00106228
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06002F4B RID: 12107 RVA: 0x0010802B File Offset: 0x0010622B
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x00108038 File Offset: 0x00106238
	private void SwitchToggled(bool toggled_on)
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);
	}

	// Token: 0x06002F4D RID: 12109 RVA: 0x00108054 File Offset: 0x00106254
	private void UpdateVisualState(bool force = false)
	{
		bool flag = this.CurrentValue > this.threshold;
		if (flag != this.was_pressed || this.was_on != base.IsSwitchedOn || force)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (flag)
			{
				if (force)
				{
					component.Play(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(base.IsSwitchedOn ? "on_down_pre" : "off_down_pre", KAnim.PlayMode.Once, 1f, 0f);
					component.Queue(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			else if (force)
			{
				component.Play(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play(base.IsSwitchedOn ? "on_up_pre" : "off_up_pre", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.was_pressed = flag;
			this.was_on = base.IsSwitchedOn;
		}
	}

	// Token: 0x06002F4E RID: 12110 RVA: 0x001081C4 File Offset: 0x001063C4
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001BC5 RID: 7109
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001BC6 RID: 7110
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001BC7 RID: 7111
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04001BC8 RID: 7112
	private bool was_pressed;

	// Token: 0x04001BC9 RID: 7113
	private bool was_on;

	// Token: 0x04001BCA RID: 7114
	public float rangeMin;

	// Token: 0x04001BCB RID: 7115
	public float rangeMax = 1f;

	// Token: 0x04001BCC RID: 7116
	[Serialize]
	private float massSolid;

	// Token: 0x04001BCD RID: 7117
	[Serialize]
	private float massPickupables;

	// Token: 0x04001BCE RID: 7118
	[Serialize]
	private float massActivators;

	// Token: 0x04001BCF RID: 7119
	private const float MIN_TOGGLE_TIME = 0.15f;

	// Token: 0x04001BD0 RID: 7120
	private float toggleCooldown = 0.15f;

	// Token: 0x04001BD1 RID: 7121
	private HandleVector<int>.Handle solidChangedEntry;

	// Token: 0x04001BD2 RID: 7122
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04001BD3 RID: 7123
	private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;

	// Token: 0x04001BD4 RID: 7124
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BD5 RID: 7125
	private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>(delegate(LogicMassSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
