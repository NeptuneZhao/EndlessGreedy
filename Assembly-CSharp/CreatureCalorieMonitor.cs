using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class CreatureCalorieMonitor : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
	// Token: 0x06001F0F RID: 7951 RVA: 0x000ADD0C File Offset: 0x000ABF0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(CreatureCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback(CreatureCalorieMonitor.ReadyToPoop), delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<CreatureCalorieMonitor.Instance, float>(CreatureCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.hungry, (CreatureCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.DefaultState(this.hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry());
		this.hungry.hungry.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms).Transition(this.hungry.outofcalories, (CreatureCalorieMonitor.Instance smi) => smi.IsOutOfCalories(), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.DefaultState(this.hungry.outofcalories.wild).Transition(this.hungry.hungry, (CreatureCalorieMonitor.Instance smi) => !smi.IsOutOfCalories(), UpdateRate.SIM_1000ms);
		this.hungry.outofcalories.wild.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.tame, true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.tame.Enter("StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.StarvationStartTime)).Exit("ClearStarvationTime", delegate(CreatureCalorieMonitor.Instance smi)
		{
			this.starvationStartTime.Set(Mathf.Min(-(GameClock.Instance.GetTime() - this.starvationStartTime.Get(smi)), 0f), smi, false);
		}).Transition(this.hungry.outofcalories.starvedtodeath, (CreatureCalorieMonitor.Instance smi) => smi.GetDeathTimeRemaining() <= 0f, UpdateRate.SIM_1000ms).TagTransition(GameTags.Creatures.PausedHunger, this.pause.starvingPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.wild, false).ToggleStatusItem(CREATURES.STATUSITEMS.STARVING.NAME, CREATURES.STATUSITEMS.STARVING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, default(HashedString), 129022, (string str, CreatureCalorieMonitor.Instance smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining(), "F1", false)), null, null).ToggleNotification((CreatureCalorieMonitor.Instance smi) => new Notification(CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, (List<Notification> notifications, object data) => CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP + notifications.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false)).ToggleEffect((CreatureCalorieMonitor.Instance smi) => this.outOfCaloriesTame);
		this.hungry.outofcalories.starvedtodeath.Enter(delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
		});
		this.pause.commonPause.TagTransition(GameTags.Creatures.PausedHunger, this.normal, true);
		this.pause.starvingPause.Exit("Recalculate StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.RecalculateStartTimeOnUnpause)).TagTransition(GameTags.Creatures.PausedHunger, this.hungry.outofcalories.tame, true);
		this.outOfCaloriesTame = new Effect("OutOfCaloriesTame", CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
		this.outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, false, false, true));
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000AE19C File Offset: 0x000AC39C
	private static bool ReadyToPoop(CreatureCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping && !smi.IsInsideState(smi.sm.pause);
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000AE1E9 File Offset: 0x000AC3E9
	private static void UpdateMetabolismCalorieModifier(CreatureCalorieMonitor.Instance smi, float dt)
	{
		if (smi.IsInsideState(smi.sm.pause))
		{
			return;
		}
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000AE221 File Offset: 0x000AC421
	private static void StarvationStartTime(CreatureCalorieMonitor.Instance smi)
	{
		if (smi.sm.starvationStartTime.Get(smi) <= 0f)
		{
			smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi, false);
		}
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x000AE258 File Offset: 0x000AC458
	private static void RecalculateStartTimeOnUnpause(CreatureCalorieMonitor.Instance smi)
	{
		float num = smi.sm.starvationStartTime.Get(smi);
		if (num < 0f)
		{
			float value = GameClock.Instance.GetTime() - Mathf.Abs(num);
			smi.sm.starvationStartTime.Set(value, smi, false);
		}
	}

	// Token: 0x04001180 RID: 4480
	public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State normal;

	// Token: 0x04001181 RID: 4481
	public CreatureCalorieMonitor.PauseStates pause;

	// Token: 0x04001182 RID: 4482
	private CreatureCalorieMonitor.HungryStates hungry;

	// Token: 0x04001183 RID: 4483
	private Effect outOfCaloriesTame;

	// Token: 0x04001184 RID: 4484
	public StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.FloatParameter starvationStartTime;

	// Token: 0x0200131A RID: 4890
	public struct CaloriesConsumedEvent
	{
		// Token: 0x04006589 RID: 25993
		public Tag tag;

		// Token: 0x0400658A RID: 25994
		public float calories;
	}

	// Token: 0x0200131B RID: 4891
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060085E9 RID: 34281 RVA: 0x003273A3 File Offset: 0x003255A3
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x060085EA RID: 34282 RVA: 0x003273CC File Offset: 0x003255CC
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			CreatureCalorieMonitor.Stomach stomach = obj.GetSMI<CreatureCalorieMonitor.Instance>().stomach;
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			if (stomach.diet.consumedTags.Count > 0)
			{
				string newValue = string.Join(", ", (from t in stomach.diet.consumedTags
				select t.Key.ProperName()).ToArray<string>());
				string text = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					text = string.Join("\n", (from s in stomach.diet.consumedTags.Select(delegate(KeyValuePair<Tag, float> t)
					{
						float consumer_caloriesLossPerCaloriesPerKG = -calorie_loss_per_second / t.Value;
						GameObject prefab = Assets.GetPrefab(t.Key.ToString());
						IPlantConsumptionInstructions plantConsumptionInstructions = prefab.GetComponent<IPlantConsumptionInstructions>();
						plantConsumptionInstructions = ((plantConsumptionInstructions != null) ? plantConsumptionInstructions : prefab.GetSMI<IPlantConsumptionInstructions>());
						if (plantConsumptionInstructions == null)
						{
							return null;
						}
						return UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", plantConsumptionInstructions.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG));
					})
					where !string.IsNullOrEmpty(s)
					select s).ToArray<string>());
				}
				if (this.diet.CanEatAnyNonDirectlyEdiblePlant)
				{
					if (this.diet.CanEatAnyPlantDirectly)
					{
						text += "\n";
					}
					Diet.Info info;
					text += string.Join("\n", (from t in stomach.diet.consumedTags.FindAll((KeyValuePair<Tag, float> t) => this.diet.directlyEatenPlantInfos.FirstOrDefault((Diet.Info info) => info.consumedTags.Contains(t.Key)) == null)
					select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", text), Descriptor.DescriptorType.Effect, false));
			}
			if (stomach.diet.producedTags.Count > 0)
			{
				string newValue2 = string.Join(", ", (from t in stomach.diet.producedTags
				select t.Key.ProperName()).ToArray<string>());
				string text2 = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					List<KeyValuePair<Tag, float>> list2 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> keyValuePair in stomach.diet.producedTags)
					{
						foreach (Diet.Info info in this.diet.directlyEatenPlantInfos)
						{
							if (info.producedElement == keyValuePair.Key)
							{
								float consumed_mass = -calorie_loss_per_second / info.caloriesPerKg * 600f;
								float num = info.ConvertConsumptionMassToProducedMass(consumed_mass);
								list2.Add(new KeyValuePair<Tag, float>(keyValuePair.Key, num / 600f));
							}
						}
					}
					text2 = string.Join("\n", (from t in list2
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM_FROM_PLANT.text.Replace("{Item}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
					text2 += "\n";
				}
				else if (stomach.diet.CanEatAnyNonDirectlyEdiblePlant)
				{
					List<KeyValuePair<Tag, float>> list3 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> keyValuePair2 in stomach.diet.producedTags)
					{
						foreach (Diet.Info info2 in this.diet.noPlantInfos)
						{
							if (info2.producedElement == keyValuePair2.Key)
							{
								list3.Add(new KeyValuePair<Tag, float>(info2.producedElement, info2.producedConversionRate));
							}
						}
					}
					text2 += string.Join("\n", (from t in list3
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue2), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", text2), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x0400658B RID: 25995
		public Diet diet;

		// Token: 0x0400658C RID: 25996
		public float minConsumedCaloriesBeforePooping = 100f;

		// Token: 0x0400658D RID: 25997
		public float maxPoopSizeKG = -1f;

		// Token: 0x0400658E RID: 25998
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x0400658F RID: 25999
		public float deathTimer = 6000f;

		// Token: 0x04006590 RID: 26000
		public bool storePoop;
	}

	// Token: 0x0200131C RID: 4892
	public class PauseStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04006591 RID: 26001
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State commonPause;

		// Token: 0x04006592 RID: 26002
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvingPause;
	}

	// Token: 0x0200131D RID: 4893
	public class HungryStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04006593 RID: 26003
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State hungry;

		// Token: 0x04006594 RID: 26004
		public CreatureCalorieMonitor.HungryStates.OutOfCaloriesState outofcalories;

		// Token: 0x02002496 RID: 9366
		public class OutOfCaloriesState : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
		{
			// Token: 0x0400A23B RID: 41531
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State wild;

			// Token: 0x0400A23C RID: 41532
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame;

			// Token: 0x0400A23D RID: 41533
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvedtodeath;
		}
	}

	// Token: 0x0200131E RID: 4894
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Stomach
	{
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060085EE RID: 34286 RVA: 0x00327904 File Offset: 0x00325B04
		// (set) Token: 0x060085EF RID: 34287 RVA: 0x0032790C File Offset: 0x00325B0C
		public Diet diet { get; private set; }

		// Token: 0x060085F0 RID: 34288 RVA: 0x00327918 File Offset: 0x00325B18
		public Stomach(GameObject owner, float minConsumedCaloriesBeforePooping, float max_poop_size_in_kg, bool storePoop)
		{
			this.diet = DietManager.Instance.GetPrefabDiet(owner);
			this.owner = owner;
			this.minConsumedCaloriesBeforePooping = minConsumedCaloriesBeforePooping;
			this.storePoop = storePoop;
			this.maxPoopSizeInKG = max_poop_size_in_kg;
		}

		// Token: 0x060085F1 RID: 34289 RVA: 0x00327964 File Offset: 0x00325B64
		public void Poop()
		{
			this.shouldContinuingPooping = true;
			float num = 0f;
			Tag tag = Tag.Invalid;
			byte disease_idx = byte.MaxValue;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
					{
						float num4 = (this.maxPoopSizeInKG < 0f) ? float.MaxValue : this.maxPoopSizeInKG;
						float b = Mathf.Clamp(num4 - num, 0f, num4);
						float num5 = Mathf.Min(dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories)), b);
						num += num5;
						tag = dietInfo.producedElement;
						if (dietInfo.diseaseIdx != 255)
						{
							disease_idx = dietInfo.diseaseIdx;
							if (!this.storePoop && dietInfo.emmitDiseaseOnCell)
							{
								num3 += (int)(dietInfo.diseasePerKgProduced * num5);
							}
							else
							{
								num2 += (int)(dietInfo.diseasePerKgProduced * num5);
							}
						}
						caloriesConsumedEntry.calories = Mathf.Clamp(caloriesConsumedEntry.calories - dietInfo.ConvertConsumptionMassToCalories(dietInfo.ConvertProducedMassToConsumptionMass(num5)), 0f, float.MaxValue);
						this.caloriesConsumed[i] = caloriesConsumedEntry;
						flag = (flag || dietInfo.produceSolidTile);
					}
				}
			}
			if (num <= 0f || tag == Tag.Invalid)
			{
				this.shouldContinuingPooping = false;
				return;
			}
			string text = null;
			Element element = ElementLoader.GetElement(tag);
			if (element != null)
			{
				text = element.name;
			}
			int num6 = Grid.PosToCell(this.owner.transform.GetPosition());
			float temperature = this.owner.GetComponent<PrimaryElement>().Temperature;
			DebugUtil.DevAssert(!this.storePoop || !flag, "Stomach cannot both store poop & create a solid tile.", null);
			if (this.storePoop)
			{
				Storage component = this.owner.GetComponent<Storage>();
				if (element == null)
				{
					GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(tag), Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					component2.Mass = num;
					component2.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component2.Temperature = temperature;
					gameObject.SetActive(true);
					component.Store(gameObject, true, false, true, false);
					text = gameObject.GetProperName();
				}
				else if (element.IsLiquid)
				{
					component.AddLiquid(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else if (element.IsGas)
				{
					component.AddGasChunk(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else
				{
					component.AddOre(element.id, num, temperature, disease_idx, num2, false, true);
				}
			}
			else
			{
				if (element == null)
				{
					GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(tag), Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
					PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
					component3.Mass = num;
					component3.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component3.Temperature = temperature;
					gameObject2.SetActive(true);
					text = gameObject2.GetProperName();
				}
				else if (element.IsLiquid)
				{
					FallingWater.instance.AddParticle(num6, element.idx, num, temperature, disease_idx, num2, true, false, false, false);
				}
				else if (element.IsGas)
				{
					SimMessages.AddRemoveSubstance(num6, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
				}
				else if (flag)
				{
					int num7 = this.owner.GetComponent<Facing>().GetFrontCell();
					if (!Grid.IsValidCell(num7))
					{
						global::Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", new object[]
						{
							this.owner,
							element.name,
							num7,
							num6
						});
						num7 = num6;
					}
					SimMessages.AddRemoveSubstance(num7, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
				}
				else
				{
					element.substance.SpawnResource(Grid.CellToPosCCC(num6, Grid.SceneLayer.Ore), num, temperature, disease_idx, num2, false, false, false);
				}
				if (num3 > 0)
				{
					SimMessages.ModifyDiseaseOnCell(num6, disease_idx, num3);
				}
			}
			if (this.GetTotalConsumedCalories() <= 0f)
			{
				this.shouldContinuingPooping = false;
			}
			KPrefabID component4 = this.owner.GetComponent<KPrefabID>();
			if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component4.PrefabTag))
			{
				Game.Instance.savedInfo.creaturePoopAmount.Add(component4.PrefabTag, 0f);
			}
			Dictionary<Tag, float> creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount;
			Tag prefabTag = component4.PrefabTag;
			creaturePoopAmount[prefabTag] += num;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, this.owner.transform, 1.5f, false);
			this.owner.Trigger(-1844238272, null);
		}

		// Token: 0x060085F2 RID: 34290 RVA: 0x00327E63 File Offset: 0x00326063
		public List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> GetCalorieEntries()
		{
			return this.caloriesConsumed;
		}

		// Token: 0x060085F3 RID: 34291 RVA: 0x00327E6C File Offset: 0x0032606C
		public float GetTotalConsumedCalories()
		{
			float num = 0f;
			foreach (CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry in this.caloriesConsumed)
			{
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						num += caloriesConsumedEntry.calories;
					}
				}
			}
			return num;
		}

		// Token: 0x060085F4 RID: 34292 RVA: 0x00327EFC File Offset: 0x003260FC
		public float GetFullness()
		{
			return this.GetTotalConsumedCalories() / this.minConsumedCaloriesBeforePooping;
		}

		// Token: 0x060085F5 RID: 34293 RVA: 0x00327F0C File Offset: 0x0032610C
		public bool IsReadyToPoop()
		{
			float totalConsumedCalories = this.GetTotalConsumedCalories();
			return totalConsumedCalories > 0f && (this.shouldContinuingPooping || totalConsumedCalories >= this.minConsumedCaloriesBeforePooping);
		}

		// Token: 0x060085F6 RID: 34294 RVA: 0x00327F40 File Offset: 0x00326140
		public void Consume(Tag tag, float calories)
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.tag == tag)
				{
					caloriesConsumedEntry.calories += calories;
					this.caloriesConsumed[i] = caloriesConsumedEntry;
					this.caloriesConsumed[i] = caloriesConsumedEntry;
					return;
				}
			}
			CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry item = default(CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry);
			item.tag = tag;
			item.calories = calories;
			this.caloriesConsumed.Add(item);
		}

		// Token: 0x060085F7 RID: 34295 RVA: 0x00327FCC File Offset: 0x003261CC
		public Tag GetNextPoopEntry()
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						return dietInfo.producedElement;
					}
				}
			}
			return Tag.Invalid;
		}

		// Token: 0x04006595 RID: 26005
		[Serialize]
		private List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> caloriesConsumed = new List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry>();

		// Token: 0x04006596 RID: 26006
		[Serialize]
		private bool shouldContinuingPooping;

		// Token: 0x04006597 RID: 26007
		private float minConsumedCaloriesBeforePooping;

		// Token: 0x04006598 RID: 26008
		private float maxPoopSizeInKG;

		// Token: 0x0400659A RID: 26010
		private GameObject owner;

		// Token: 0x0400659B RID: 26011
		private bool storePoop;

		// Token: 0x02002497 RID: 9367
		[Serializable]
		public struct CaloriesConsumedEntry
		{
			// Token: 0x0400A23E RID: 41534
			public Tag tag;

			// Token: 0x0400A23F RID: 41535
			public float calories;
		}
	}

	// Token: 0x0200131F RID: 4895
	public new class Instance : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x060085F8 RID: 34296 RVA: 0x0032803C File Offset: 0x0032623C
		public Instance(IStateMachineTarget master, CreatureCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, def.maxPoopSizeKG, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x060085F9 RID: 34297 RVA: 0x00328121 File Offset: 0x00326321
		public override void StartSM()
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			base.StartSM();
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x0032813C File Offset: 0x0032633C
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.calories.value += caloriesConsumedEvent.calories;
			this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x060085FB RID: 34299 RVA: 0x0032818A File Offset: 0x0032638A
		public float GetDeathTimeRemaining()
		{
			return base.smi.def.deathTimer - (GameClock.Instance.GetTime() - base.sm.starvationStartTime.Get(base.smi));
		}

		// Token: 0x060085FC RID: 34300 RVA: 0x003281BE File Offset: 0x003263BE
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x060085FD RID: 34301 RVA: 0x003281D6 File Offset: 0x003263D6
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x060085FE RID: 34302 RVA: 0x003281EF File Offset: 0x003263EF
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x003281FE File Offset: 0x003263FE
		public bool IsOutOfCalories()
		{
			return this.GetCalories0to1() <= 0f;
		}

		// Token: 0x0400659C RID: 26012
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x0400659D RID: 26013
		public AmountInstance calories;

		// Token: 0x0400659E RID: 26014
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x0400659F RID: 26015
		public float lastMealOrPoopTime;

		// Token: 0x040065A0 RID: 26016
		public AttributeInstance metabolism;

		// Token: 0x040065A1 RID: 26017
		public AttributeModifier deltaCalorieMetabolismModifier;

		// Token: 0x040065A2 RID: 26018
		public KPrefabID prefabID;
	}
}
