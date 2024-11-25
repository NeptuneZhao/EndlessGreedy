﻿using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000541 RID: 1345
public class BeehiveCalorieMonitor : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>
{
	// Token: 0x06001EEE RID: 7918 RVA: 0x000AD210 File Offset: 0x000AB410
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeehiveCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.Transition.ConditionCallback(BeehiveCalorieMonitor.ReadyToPoop), delegate(BeehiveCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<BeehiveCalorieMonitor.Instance, float>(BeehiveCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.Transition(this.hungry, (BeehiveCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry()).ToggleStatusItem(Db.Get().CreatureStatusItems.HiveHungry, null).Transition(this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000AD356 File Offset: 0x000AB556
	private static bool ReadyToPoop(BeehiveCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping;
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x000AD383 File Offset: 0x000AB583
	private static void UpdateMetabolismCalorieModifier(BeehiveCalorieMonitor.Instance smi, float dt)
	{
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x04001173 RID: 4467
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State normal;

	// Token: 0x04001174 RID: 4468
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State hungry;

	// Token: 0x02001307 RID: 4871
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600858A RID: 34186 RVA: 0x003264A8 File Offset: 0x003246A8
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x0600858B RID: 34187 RVA: 0x003264D0 File Offset: 0x003246D0
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			BeehiveCalorieMonitor.Instance smi = obj.GetSMI<BeehiveCalorieMonitor.Instance>();
			string newValue = string.Join(", ", (from t in smi.stomach.diet.consumedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue2 = string.Join("\n", (from t in smi.stomach.diet.consumedTags
			select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2), Descriptor.DescriptorType.Effect, false));
			string newValue3 = string.Join(", ", (from t in smi.stomach.diet.producedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue4 = string.Join("\n", (from t in smi.stomach.diet.producedTags
			select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4), Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x04006549 RID: 25929
		public Diet diet;

		// Token: 0x0400654A RID: 25930
		public float minConsumedCaloriesBeforePooping = 100f;

		// Token: 0x0400654B RID: 25931
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x0400654C RID: 25932
		public bool storePoop = true;
	}

	// Token: 0x02001308 RID: 4872
	public new class Instance : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x0600858D RID: 34189 RVA: 0x0032674C File Offset: 0x0032494C
		public Instance(IStateMachineTarget master, BeehiveCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, -1f, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x0600858E RID: 34190 RVA: 0x00326830 File Offset: 0x00324A30
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.calories.value += caloriesConsumedEvent.calories;
			this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x0600858F RID: 34191 RVA: 0x0032687E File Offset: 0x00324A7E
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x06008590 RID: 34192 RVA: 0x00326896 File Offset: 0x00324A96
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06008591 RID: 34193 RVA: 0x003268AF File Offset: 0x00324AAF
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x0400654D RID: 25933
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x0400654E RID: 25934
		public AmountInstance calories;

		// Token: 0x0400654F RID: 25935
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04006550 RID: 25936
		public float lastMealOrPoopTime;

		// Token: 0x04006551 RID: 25937
		public AttributeInstance metabolism;

		// Token: 0x04006552 RID: 25938
		public AttributeModifier deltaCalorieMetabolismModifier;
	}
}
