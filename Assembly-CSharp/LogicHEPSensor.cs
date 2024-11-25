using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200070E RID: 1806
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHEPSensor : Switch, ISaveLoadable, IThresholdSwitch, ISimEveryTick
{
	// Token: 0x06002EEA RID: 12010 RVA: 0x00106EC6 File Offset: 0x001050C6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicHEPSensor>(-905833192, LogicHEPSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x00106EE0 File Offset: 0x001050E0
	private void OnCopySettings(object data)
	{
		LogicHEPSensor component = ((GameObject)data).GetComponent<LogicHEPSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x00106F1C File Offset: 0x0010511C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x00106F85 File Offset: 0x00105185
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x00106FB8 File Offset: 0x001051B8
	public void SimEveryTick(float dt)
	{
		if (this.waitForLogicTick)
		{
			return;
		}
		Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
		ListPool<ScenePartitionerEntry, LogicHEPSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicHEPSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		float num = 0f;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			HighEnergyParticle component = (scenePartitionerEntry.obj as KCollider2D).gameObject.GetComponent<HighEnergyParticle>();
			if (!(component == null) && component.isCollideable)
			{
				num += component.payload;
			}
		}
		pooledList.Recycle();
		this.foundPayload = num;
		bool flag = (this.activateOnHigherThan && num > this.thresholdPayload) || (!this.activateOnHigherThan && num < this.thresholdPayload);
		if (flag != this.switchedOn)
		{
			this.waitForLogicTick = true;
		}
		this.SetState(flag);
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x001070C4 File Offset: 0x001052C4
	private void LogicTick()
	{
		this.waitForLogicTick = false;
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x001070CD File Offset: 0x001052CD
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x001070DC File Offset: 0x001052DC
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x001070FC File Offset: 0x001052FC
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x00107184 File Offset: 0x00105384
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x001071D7 File Offset: 0x001053D7
	// (set) Token: 0x06002EF5 RID: 12021 RVA: 0x001071DF File Offset: 0x001053DF
	public float Threshold
	{
		get
		{
			return this.thresholdPayload;
		}
		set
		{
			this.thresholdPayload = value;
			this.dirty = true;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x001071EF File Offset: 0x001053EF
	// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x001071F7 File Offset: 0x001053F7
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x00107207 File Offset: 0x00105407
	public float CurrentValue
	{
		get
		{
			return this.foundPayload;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x0010720F File Offset: 0x0010540F
	public float RangeMin
	{
		get
		{
			return this.minPayload;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06002EFA RID: 12026 RVA: 0x00107217 File Offset: 0x00105417
	public float RangeMax
	{
		get
		{
			return this.maxPayload;
		}
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x0010721F File Offset: 0x0010541F
	public float GetRangeMinInputField()
	{
		return this.minPayload;
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x00107227 File Offset: 0x00105427
	public float GetRangeMaxInputField()
	{
		return this.maxPayload;
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06002EFD RID: 12029 RVA: 0x0010722F File Offset: 0x0010542F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.HEPSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06002EFE RID: 12030 RVA: 0x00107236 File Offset: 0x00105436
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06002EFF RID: 12031 RVA: 0x0010723D File Offset: 0x0010543D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06002F00 RID: 12032 RVA: 0x00107249 File Offset: 0x00105449
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x00107255 File Offset: 0x00105455
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedHighEnergyParticles(value, GameUtil.TimeSlice.None, units);
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x0010725F File Offset: 0x0010545F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x00107267 File Offset: 0x00105467
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x0010726A File Offset: 0x0010546A
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06002F05 RID: 12037 RVA: 0x00107271 File Offset: 0x00105471
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06002F06 RID: 12038 RVA: 0x00107274 File Offset: 0x00105474
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06002F07 RID: 12039 RVA: 0x00107278 File Offset: 0x00105478
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(30f, 50f),
				new NonLinearSlider.Range(30f, 200f),
				new NonLinearSlider.Range(40f, 500f)
			};
		}
	}

	// Token: 0x04001BA0 RID: 7072
	[Serialize]
	public float thresholdPayload;

	// Token: 0x04001BA1 RID: 7073
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x04001BA2 RID: 7074
	[Serialize]
	public bool dirty = true;

	// Token: 0x04001BA3 RID: 7075
	private readonly float minPayload;

	// Token: 0x04001BA4 RID: 7076
	private readonly float maxPayload = 500f;

	// Token: 0x04001BA5 RID: 7077
	private float foundPayload;

	// Token: 0x04001BA6 RID: 7078
	private bool waitForLogicTick;

	// Token: 0x04001BA7 RID: 7079
	private bool wasOn;

	// Token: 0x04001BA8 RID: 7080
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BA9 RID: 7081
	private static readonly EventSystem.IntraObjectHandler<LogicHEPSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicHEPSensor>(delegate(LogicHEPSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
