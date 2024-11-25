using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000726 RID: 1830
public class MassageTable : RelaxationPoint, IGameObjectEffectDescriptor, IActivationRangeTarget
{
	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06003088 RID: 12424 RVA: 0x0010BF17 File Offset: 0x0010A117
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06003089 RID: 12425 RVA: 0x0010BF23 File Offset: 0x0010A123
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x0010BF2F File Offset: 0x0010A12F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<MassageTable>(-905833192, MassageTable.OnCopySettingsDelegate);
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x0010BF48 File Offset: 0x0010A148
	private void OnCopySettings(object data)
	{
		MassageTable component = ((GameObject)data).GetComponent<MassageTable>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x0010BF84 File Offset: 0x0010A184
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
		{
			string effect_id = MassageTable.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x0010BFC0 File Offset: 0x0010A1C0
	public new List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		if (MassageTable.EffectsRemoved.Length != 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			list.Add(item2);
			for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
			{
				string text = MassageTable.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item3 = default(Descriptor);
				item3.IncreaseIndent();
				item3.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		return list;
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x0010C120 File Offset: 0x0010A320
	protected override WorkChore<RelaxationPoint> CreateWorkChore()
	{
		WorkChore<RelaxationPoint> workChore = new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.StressHeal, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		workChore.AddPrecondition(MassageTable.IsStressAboveActivationRange, this);
		return workChore;
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x0600308F RID: 12431 RVA: 0x0010C170 File Offset: 0x0010A370
	// (set) Token: 0x06003090 RID: 12432 RVA: 0x0010C178 File Offset: 0x0010A378
	public float ActivateValue
	{
		get
		{
			return this.activateValue;
		}
		set
		{
			this.activateValue = value;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06003091 RID: 12433 RVA: 0x0010C181 File Offset: 0x0010A381
	// (set) Token: 0x06003092 RID: 12434 RVA: 0x0010C189 File Offset: 0x0010A389
	public float DeactivateValue
	{
		get
		{
			return this.stopStressingValue;
		}
		set
		{
			this.stopStressingValue = value;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06003093 RID: 12435 RVA: 0x0010C192 File Offset: 0x0010A392
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06003094 RID: 12436 RVA: 0x0010C195 File Offset: 0x0010A395
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06003095 RID: 12437 RVA: 0x0010C19C File Offset: 0x0010A39C
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06003096 RID: 12438 RVA: 0x0010C1A3 File Offset: 0x0010A3A3
	public string ActivationRangeTitleText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06003097 RID: 12439 RVA: 0x0010C1AF File Offset: 0x0010A3AF
	public string ActivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.ACTIVATE;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06003098 RID: 12440 RVA: 0x0010C1BB File Offset: 0x0010A3BB
	public string DeactivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.DEACTIVATE;
		}
	}

	// Token: 0x04001C7D RID: 7293
	[Serialize]
	private float activateValue = 50f;

	// Token: 0x04001C7E RID: 7294
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x04001C7F RID: 7295
	private static readonly EventSystem.IntraObjectHandler<MassageTable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MassageTable>(delegate(MassageTable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001C80 RID: 7296
	private static readonly Chore.Precondition IsStressAboveActivationRange = new Chore.Precondition
	{
		id = "IsStressAboveActivationRange",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STRESS_ABOVE_ACTIVATION_RANGE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			IActivationRangeTarget activationRangeTarget = (IActivationRangeTarget)data;
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).value >= activationRangeTarget.ActivateValue;
		}
	};
}
