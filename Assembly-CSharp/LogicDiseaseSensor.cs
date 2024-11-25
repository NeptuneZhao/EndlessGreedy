using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000705 RID: 1797
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDiseaseSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002E43 RID: 11843 RVA: 0x00103441 File Offset: 0x00101641
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicDiseaseSensor>(-905833192, LogicDiseaseSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x0010345C File Offset: 0x0010165C
	private void OnCopySettings(object data)
	{
		LogicDiseaseSensor component = ((GameObject)data).GetComponent<LogicDiseaseSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x00103496 File Offset: 0x00101696
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x001034D8 File Offset: 0x001016D8
	public void Sim200ms(float dt)
	{
		if (this.sampleIdx < 8)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.samples[this.sampleIdx] = Grid.DiseaseCount[i];
				this.sampleIdx++;
			}
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
		this.animController.SetSymbolVisiblity(LogicDiseaseSensor.TINT_SYMBOL, currentValue > 0f);
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x001035B3 File Offset: 0x001017B3
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06002E48 RID: 11848 RVA: 0x001035C2 File Offset: 0x001017C2
	// (set) Token: 0x06002E49 RID: 11849 RVA: 0x001035CA File Offset: 0x001017CA
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

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06002E4A RID: 11850 RVA: 0x001035D3 File Offset: 0x001017D3
	// (set) Token: 0x06002E4B RID: 11851 RVA: 0x001035DB File Offset: 0x001017DB
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

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06002E4C RID: 11852 RVA: 0x001035E4 File Offset: 0x001017E4
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += (float)this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06002E4D RID: 11853 RVA: 0x00103616 File Offset: 0x00101816
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06002E4E RID: 11854 RVA: 0x0010361D File Offset: 0x0010181D
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x00103624 File Offset: 0x00101824
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x0010362B File Offset: 0x0010182B
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06002E51 RID: 11857 RVA: 0x00103632 File Offset: 0x00101832
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06002E52 RID: 11858 RVA: 0x00103639 File Offset: 0x00101839
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06002E53 RID: 11859 RVA: 0x00103645 File Offset: 0x00101845
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x00103651 File Offset: 0x00101851
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x0010365C File Offset: 0x0010185C
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x0010365F File Offset: 0x0010185F
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x00103662 File Offset: 0x00101862
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06002E58 RID: 11864 RVA: 0x00103669 File Offset: 0x00101869
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06002E59 RID: 11865 RVA: 0x0010366C File Offset: 0x0010186C
	public int IncrementScale
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06002E5A RID: 11866 RVA: 0x00103670 File Offset: 0x00101870
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x0010367D File Offset: 0x0010187D
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x0010369C File Offset: 0x0010189C
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(LogicDiseaseSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int i = Grid.PosToCell(this);
				byte b = Grid.DiseaseIdx[i];
				Color32 c = Color.white;
				if (b != 255)
				{
					Disease disease = Db.Get().Diseases[(int)b];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(LogicDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(LogicDiseaseSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x00103760 File Offset: 0x00101960
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06002E5E RID: 11870 RVA: 0x001037B3 File Offset: 0x001019B3
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x04001AFF RID: 6911
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001B00 RID: 6912
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001B01 RID: 6913
	private KBatchedAnimController animController;

	// Token: 0x04001B02 RID: 6914
	private bool wasOn;

	// Token: 0x04001B03 RID: 6915
	private const float rangeMin = 0f;

	// Token: 0x04001B04 RID: 6916
	private const float rangeMax = 100000f;

	// Token: 0x04001B05 RID: 6917
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001B06 RID: 6918
	private int[] samples = new int[8];

	// Token: 0x04001B07 RID: 6919
	private int sampleIdx;

	// Token: 0x04001B08 RID: 6920
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B09 RID: 6921
	private static readonly EventSystem.IntraObjectHandler<LogicDiseaseSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicDiseaseSensor>(delegate(LogicDiseaseSensor component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001B0A RID: 6922
	private static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x04001B0B RID: 6923
	private static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};

	// Token: 0x04001B0C RID: 6924
	private static readonly HashedString TINT_SYMBOL = "germs";
}
