using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200093C RID: 2364
[SerializationConfig(MemberSerialization.OptIn)]
public class LeadSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060044BD RID: 17597 RVA: 0x001873C4 File Offset: 0x001855C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LeadSuitTank>(-1617557748, LeadSuitTank.OnEquippedDelegate);
		base.Subscribe<LeadSuitTank>(-170173755, LeadSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x060044BE RID: 17598 RVA: 0x001873EE File Offset: 0x001855EE
	public float PercentFull()
	{
		return this.batteryCharge;
	}

	// Token: 0x060044BF RID: 17599 RVA: 0x001873F6 File Offset: 0x001855F6
	public bool IsEmpty()
	{
		return this.batteryCharge <= 0f;
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x00187408 File Offset: 0x00185608
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x060044C1 RID: 17601 RVA: 0x0018741A File Offset: 0x0018561A
	public bool NeedsRecharging()
	{
		return this.PercentFull() <= 0.25f;
	}

	// Token: 0x060044C2 RID: 17602 RVA: 0x0018742C File Offset: 0x0018562C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.LEADSUIT_BATTERY, GameUtil.GetFormattedPercent(this.PercentFull() * 100f, GameUtil.TimeSlice.None));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x00187470 File Offset: 0x00185670
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.leadSuitMonitor = new LeadSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.leadSuitMonitor.StartSM();
		if (this.NeedsRecharging())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.SuitBatteryLow);
		}
	}

	// Token: 0x060044C4 RID: 17604 RVA: 0x001874E8 File Offset: 0x001856E8
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryLow);
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryOut);
			NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
		}
		if (this.leadSuitMonitor != null)
		{
			this.leadSuitMonitor.StopSM("Removed leadsuit tank");
			this.leadSuitMonitor = null;
		}
	}

	// Token: 0x04002CF1 RID: 11505
	[Serialize]
	public float batteryCharge = 1f;

	// Token: 0x04002CF2 RID: 11506
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x04002CF3 RID: 11507
	public float batteryDuration = 200f;

	// Token: 0x04002CF4 RID: 11508
	public float coolingOperationalTemperature = 333.15f;

	// Token: 0x04002CF5 RID: 11509
	public Tag coolantTag;

	// Token: 0x04002CF6 RID: 11510
	private LeadSuitMonitor.Instance leadSuitMonitor;

	// Token: 0x04002CF7 RID: 11511
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04002CF8 RID: 11512
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
