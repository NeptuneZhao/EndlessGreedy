using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000704 RID: 1796
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCritterCountSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002E25 RID: 11813 RVA: 0x00103072 File Offset: 0x00101272
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectable = base.GetComponent<KSelectable>();
		base.Subscribe<LogicCritterCountSensor>(-905833192, LogicCritterCountSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002E26 RID: 11814 RVA: 0x00103098 File Offset: 0x00101298
	private void OnCopySettings(object data)
	{
		LogicCritterCountSensor component = ((GameObject)data).GetComponent<LogicCritterCountSensor>();
		if (component != null)
		{
			this.countThreshold = component.countThreshold;
			this.activateOnGreaterThan = component.activateOnGreaterThan;
			this.countCritters = component.countCritters;
			this.countEggs = component.countEggs;
		}
	}

	// Token: 0x06002E27 RID: 11815 RVA: 0x001030EA File Offset: 0x001012EA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x00103120 File Offset: 0x00101320
	public void Sim200ms(float dt)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			this.currentCount = 0;
			if (this.countCritters)
			{
				this.currentCount += roomOfGameObject.cavity.creatures.Count;
			}
			if (this.countEggs)
			{
				this.currentCount += roomOfGameObject.cavity.eggs.Count;
			}
			bool state = this.activateOnGreaterThan ? (this.currentCount > this.countThreshold) : (this.currentCount < this.countThreshold);
			this.SetState(state);
			if (this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.selectable.RemoveStatusItem(this.roomStatusGUID, false);
				return;
			}
		}
		else
		{
			if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.roomStatusGUID = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom, null);
			}
			this.SetState(false);
		}
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x0010323C File Offset: 0x0010143C
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x0010324B File Offset: 0x0010144B
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002E2B RID: 11819 RVA: 0x0010326C File Offset: 0x0010146C
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.switchedOn)
			{
				component.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			component.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x0010330C File Offset: 0x0010150C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06002E2D RID: 11821 RVA: 0x0010335F File Offset: 0x0010155F
	// (set) Token: 0x06002E2E RID: 11822 RVA: 0x00103368 File Offset: 0x00101568
	public float Threshold
	{
		get
		{
			return (float)this.countThreshold;
		}
		set
		{
			this.countThreshold = (int)value;
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06002E2F RID: 11823 RVA: 0x00103372 File Offset: 0x00101572
	// (set) Token: 0x06002E30 RID: 11824 RVA: 0x0010337A File Offset: 0x0010157A
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnGreaterThan;
		}
		set
		{
			this.activateOnGreaterThan = value;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06002E31 RID: 11825 RVA: 0x00103383 File Offset: 0x00101583
	public float CurrentValue
	{
		get
		{
			return (float)this.currentCount;
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06002E32 RID: 11826 RVA: 0x0010338C File Offset: 0x0010158C
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06002E33 RID: 11827 RVA: 0x00103393 File Offset: 0x00101593
	public float RangeMax
	{
		get
		{
			return 64f;
		}
	}

	// Token: 0x06002E34 RID: 11828 RVA: 0x0010339A File Offset: 0x0010159A
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x06002E35 RID: 11829 RVA: 0x001033A2 File Offset: 0x001015A2
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06002E36 RID: 11830 RVA: 0x001033AA File Offset: 0x001015AA
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TITLE;
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06002E37 RID: 11831 RVA: 0x001033B1 File Offset: 0x001015B1
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.VALUE_NAME;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06002E38 RID: 11832 RVA: 0x001033B8 File Offset: 0x001015B8
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06002E39 RID: 11833 RVA: 0x001033C4 File Offset: 0x001015C4
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x001033D0 File Offset: 0x001015D0
	public string Format(float value, bool units)
	{
		return value.ToString();
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x001033D9 File Offset: 0x001015D9
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x001033E1 File Offset: 0x001015E1
	public float ProcessedInputValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x001033E9 File Offset: 0x001015E9
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06002E3E RID: 11838 RVA: 0x001033F5 File Offset: 0x001015F5
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06002E3F RID: 11839 RVA: 0x001033F8 File Offset: 0x001015F8
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06002E40 RID: 11840 RVA: 0x001033FB File Offset: 0x001015FB
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001AF5 RID: 6901
	private bool wasOn;

	// Token: 0x04001AF6 RID: 6902
	[Serialize]
	public bool countEggs = true;

	// Token: 0x04001AF7 RID: 6903
	[Serialize]
	public bool countCritters = true;

	// Token: 0x04001AF8 RID: 6904
	[Serialize]
	public int countThreshold;

	// Token: 0x04001AF9 RID: 6905
	[Serialize]
	public bool activateOnGreaterThan = true;

	// Token: 0x04001AFA RID: 6906
	[Serialize]
	public int currentCount;

	// Token: 0x04001AFB RID: 6907
	private KSelectable selectable;

	// Token: 0x04001AFC RID: 6908
	private Guid roomStatusGUID;

	// Token: 0x04001AFD RID: 6909
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001AFE RID: 6910
	private static readonly EventSystem.IntraObjectHandler<LogicCritterCountSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCritterCountSensor>(delegate(LogicCritterCountSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
