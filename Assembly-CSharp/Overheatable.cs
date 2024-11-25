using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200059D RID: 1437
[SkipSaveFileSerialization]
public class Overheatable : StateMachineComponent<Overheatable.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060021E2 RID: 8674 RVA: 0x000BC91D File Offset: 0x000BAB1D
	public void ResetTemperature()
	{
		base.GetComponent<PrimaryElement>().Temperature = 293.15f;
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000BC930 File Offset: 0x000BAB30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overheatTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.OverheatTemperature);
		this.fatalTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.FatalTemperature);
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000BC984 File Offset: 0x000BAB84
	private void InitializeModifiers()
	{
		if (this.modifiersInitialized)
		{
			return;
		}
		this.modifiersInitialized = true;
		AttributeModifier modifier = new AttributeModifier(this.overheatTemp.Id, this.baseOverheatTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true)
		{
			OverrideTimeSlice = new GameUtil.TimeSlice?(GameUtil.TimeSlice.None)
		};
		AttributeModifier modifier2 = new AttributeModifier(this.fatalTemp.Id, this.baseFatalTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true);
		this.GetAttributes().Add(modifier);
		this.GetAttributes().Add(modifier2);
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000BCA10 File Offset: 0x000BAC10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitializeModifiers();
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
		{
			GameComps.StructureTemperatures.Disable(handle);
			GameComps.StructureTemperatures.Enable(handle);
		}
		base.smi.StartSM();
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060021E6 RID: 8678 RVA: 0x000BCA71 File Offset: 0x000BAC71
	public float OverheatTemperature
	{
		get
		{
			this.InitializeModifiers();
			if (this.overheatTemp == null)
			{
				return 10000f;
			}
			return this.overheatTemp.GetTotalValue();
		}
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000BCA94 File Offset: 0x000BAC94
	public Notification CreateOverheatedNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000BCAF4 File Offset: 0x000BACF4
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP, text);
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000BCB5C File Offset: 0x000BAD5C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.overheatTemp != null && this.fatalTemp != null)
		{
			string formattedValue = this.overheatTemp.GetFormattedValue();
			string formattedValue2 = this.fatalTemp.GetFormattedValue();
			string text = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			text = text + "\n\n" + this.overheatTemp.GetAttributeValueTooltip();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedValue, formattedValue2), string.Format(text, formattedValue, formattedValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseOverheatTemp != 0f)
		{
			string formattedTemperature = GameUtil.GetFormattedTemperature(this.baseOverheatTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string formattedTemperature2 = GameUtil.GetFormattedTemperature(this.baseFatalTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string format = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedTemperature, formattedTemperature2), string.Format(format, formattedTemperature, formattedTemperature2), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04001310 RID: 4880
	private bool modifiersInitialized;

	// Token: 0x04001311 RID: 4881
	private AttributeInstance overheatTemp;

	// Token: 0x04001312 RID: 4882
	private AttributeInstance fatalTemp;

	// Token: 0x04001313 RID: 4883
	public float baseOverheatTemp;

	// Token: 0x04001314 RID: 4884
	public float baseFatalTemp;

	// Token: 0x0200138E RID: 5006
	public class StatesInstance : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.GameInstance
	{
		// Token: 0x0600877F RID: 34687 RVA: 0x0032BCF0 File Offset: 0x00329EF0
		public StatesInstance(Overheatable smi) : base(smi)
		{
		}

		// Token: 0x06008780 RID: 34688 RVA: 0x0032BCFC File Offset: 0x00329EFC
		public void TryDoOverheatDamage()
		{
			if (Time.time - this.lastOverheatDamageTime < 7.5f)
			{
				return;
			}
			this.lastOverheatDamageTime += 7.5f;
			base.master.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BUILDING_OVERHEATED,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.OVERHEAT,
				fullDamageEffectName = "smoke_damage_kanim"
			});
		}

		// Token: 0x040066FE RID: 26366
		public float lastOverheatDamageTime;
	}

	// Token: 0x0200138F RID: 5007
	public class States : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable>
	{
		// Token: 0x06008781 RID: 34689 RVA: 0x0032BD84 File Offset: 0x00329F84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.safeTemperature;
			this.root.EventTransition(GameHashes.BuildingBroken, this.invulnerable, null);
			this.invulnerable.EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(Overheatable.StatesInstance smi)
			{
				smi.master.ResetTemperature();
			}).EventTransition(GameHashes.BuildingPartiallyRepaired, this.safeTemperature, null);
			this.safeTemperature.TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).EventTransition(GameHashes.BuildingOverheated, this.overheated, null);
			this.overheated.Enter(delegate(Overheatable.StatesInstance smi)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, true);
			}).EventTransition(GameHashes.BuildingNoLongerOverheated, this.safeTemperature, null).ToggleStatusItem(Db.Get().BuildingStatusItems.Overheated, null).ToggleNotification((Overheatable.StatesInstance smi) => smi.master.CreateOverheatedNotification()).TriggerOnEnter(GameHashes.TooHotWarning, null).Enter("InitOverheatTime", delegate(Overheatable.StatesInstance smi)
			{
				smi.lastOverheatDamageTime = Time.time;
			}).Update("OverheatDamage", delegate(Overheatable.StatesInstance smi, float dt)
			{
				smi.TryDoOverheatDamage();
			}, UpdateRate.SIM_4000ms, false);
		}

		// Token: 0x040066FF RID: 26367
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State invulnerable;

		// Token: 0x04006700 RID: 26368
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State safeTemperature;

		// Token: 0x04006701 RID: 26369
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State overheated;
	}
}
