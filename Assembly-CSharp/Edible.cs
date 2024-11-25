using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200055C RID: 1372
[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor, ISaveLoadable, IExtendSplitting
{
	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x000B1EB5 File Offset: 0x000B00B5
	// (set) Token: 0x06001FAA RID: 8106 RVA: 0x000B1EC2 File Offset: 0x000B00C2
	public float Units
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			this.primaryElement.Units = value;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000B1ED0 File Offset: 0x000B00D0
	public float MassPerUnit
	{
		get
		{
			return this.primaryElement.MassPerUnit;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06001FAC RID: 8108 RVA: 0x000B1EDD File Offset: 0x000B00DD
	// (set) Token: 0x06001FAD RID: 8109 RVA: 0x000B1EF1 File Offset: 0x000B00F1
	public float Calories
	{
		get
		{
			return this.Units * this.foodInfo.CaloriesPerUnit;
		}
		set
		{
			this.Units = value / this.foodInfo.CaloriesPerUnit;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06001FAE RID: 8110 RVA: 0x000B1F06 File Offset: 0x000B0106
	// (set) Token: 0x06001FAF RID: 8111 RVA: 0x000B1F0E File Offset: 0x000B010E
	public EdiblesManager.FoodInfo FoodInfo
	{
		get
		{
			return this.foodInfo;
		}
		set
		{
			this.foodInfo = value;
			this.FoodID = this.foodInfo.Id;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x000B1F28 File Offset: 0x000B0128
	// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x000B1F30 File Offset: 0x000B0130
	public bool isBeingConsumed { get; private set; }

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x000B1F39 File Offset: 0x000B0139
	public List<SpiceInstance> Spices
	{
		get
		{
			return this.spices;
		}
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000B1F44 File Offset: 0x000B0144
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
		base.OnPrefabInit();
		if (this.foodInfo == null)
		{
			if (this.FoodID == null)
			{
				global::Debug.LogError("No food FoodID");
			}
			this.foodInfo = EdiblesManager.GetFoodInfo(this.FoodID);
		}
		base.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
		base.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
		this.synchronizeAnims = false;
		Components.Edibles.Add(this);
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000B1FF8 File Offset: 0x000B01F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ToggleGenericSpicedTag(base.gameObject.HasTag(GameTags.SpicedFood));
		if (this.spices != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				this.ApplySpiceEffects(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
		if (base.GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, this);
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000B20B0 File Offset: 0x000B02B0
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkAnims;
			}
			return Edible.saltHatWorkAnims;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkAnims;
			}
			return Edible.saltWorkAnims;
		}
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000B2108 File Offset: 0x000B0308
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkPstAnim;
			}
			return Edible.saltHatWorkPstAnim;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkPstAnim;
			}
			return Edible.saltWorkPstAnim;
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000B215E File Offset: 0x000B035E
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountProduced(this.Calories);
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000B2170 File Offset: 0x000B0370
	public float GetFeedingTime(WorkerBase worker)
	{
		float num = this.Calories * 2E-05f;
		if (worker != null)
		{
			BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
			if (smi != null && smi.IsBingeEating())
			{
				num /= 2f;
			}
		}
		return num;
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000B21B0 File Offset: 0x000B03B0
	protected override void OnStartWork(WorkerBase worker)
	{
		this.totalFeedingTime = this.GetFeedingTime(worker);
		base.SetWorkTime(this.totalFeedingTime);
		this.caloriesConsumed = 0f;
		this.unitsConsumed = 0f;
		this.totalUnits = this.Units;
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		this.totalConsumableCalories = this.Units * this.foodInfo.CaloriesPerUnit;
		this.StartConsuming();
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000B2228 File Offset: 0x000B0428
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.currentlyLit)
		{
			if (this.currentModifier != this.caloriesLitSpaceModifier)
			{
				worker.GetAttributes().Remove(this.currentModifier);
				worker.GetAttributes().Add(this.caloriesLitSpaceModifier);
				this.currentModifier = this.caloriesLitSpaceModifier;
			}
		}
		else if (this.currentModifier != this.caloriesModifier)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			worker.GetAttributes().Add(this.caloriesModifier);
			this.currentModifier = this.caloriesModifier;
		}
		return this.OnTickConsume(worker, dt);
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000B22BF File Offset: 0x000B04BF
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.currentModifier != null)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			this.currentModifier = null;
		}
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		this.StopConsuming(worker);
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000B22F8 File Offset: 0x000B04F8
	private bool OnTickConsume(WorkerBase worker, float dt)
	{
		if (!this.isBeingConsumed)
		{
			DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
			return true;
		}
		bool result = false;
		float num = dt / this.totalFeedingTime;
		float num2 = num * this.totalConsumableCalories;
		if (this.caloriesConsumed + num2 > this.totalConsumableCalories)
		{
			num2 = this.totalConsumableCalories - this.caloriesConsumed;
		}
		this.caloriesConsumed += num2;
		worker.GetAmounts().Get("Calories").value += num2;
		float num3 = this.totalUnits * num;
		if (this.Units - num3 < 0f)
		{
			num3 = this.Units;
		}
		this.Units -= num3;
		this.unitsConsumed += num3;
		if (this.Units <= 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001FBD RID: 8125 RVA: 0x000B23C1 File Offset: 0x000B05C1
	public void SpiceEdible(SpiceInstance spice, StatusItem status)
	{
		this.spices.Add(spice);
		this.ApplySpiceEffects(spice, status);
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x000B23D8 File Offset: 0x000B05D8
	protected virtual void ApplySpiceEffects(SpiceInstance spice, StatusItem status)
	{
		base.GetComponent<KPrefabID>().AddTag(spice.Id, true);
		this.ToggleGenericSpicedTag(true);
		base.GetComponent<KSelectable>().AddStatusItem(status, this.spices);
		if (spice.FoodModifier != null)
		{
			base.gameObject.GetAttributes().Add(spice.FoodModifier);
		}
		if (spice.CalorieModifier != null)
		{
			this.Calories += spice.CalorieModifier.Value;
		}
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x000B2454 File Offset: 0x000B0654
	private void ToggleGenericSpicedTag(bool isSpiced)
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (isSpiced)
		{
			component.RemoveTag(GameTags.UnspicedFood);
			component.AddTag(GameTags.SpicedFood, true);
			return;
		}
		component.RemoveTag(GameTags.SpicedFood);
		component.AddTag(GameTags.UnspicedFood, false);
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000B249C File Offset: 0x000B069C
	public bool CanAbsorb(Edible other)
	{
		bool flag = this.spices.Count == other.spices.Count;
		flag &= (base.gameObject.HasTag(GameTags.Rehydrated) == other.gameObject.HasTag(GameTags.Rehydrated));
		flag &= (!base.gameObject.HasTag(GameTags.Dehydrated) && !other.gameObject.HasTag(GameTags.Dehydrated));
		int num = 0;
		while (flag && num < this.spices.Count)
		{
			int num2 = 0;
			while (flag && num2 < other.spices.Count)
			{
				flag = (this.spices[num].Id == other.spices[num2].Id);
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000B256D File Offset: 0x000B076D
	private void StartConsuming()
	{
		DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started", null);
		this.isBeingConsumed = true;
		base.worker.Trigger(1406130139, this);
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000B259C File Offset: 0x000B079C
	private void StopConsuming(WorkerBase worker)
	{
		DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()", null);
		this.isBeingConsumed = false;
		for (int i = 0; i < this.foodInfo.Effects.Count; i++)
		{
			worker.GetComponent<Effects>().Add(this.foodInfo.Effects[i], true);
		}
		ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
		this.AddOnConsumeEffects(worker);
		worker.Trigger(1121894420, this);
		base.Trigger(-10536414, worker.gameObject);
		this.unitsConsumed = float.NaN;
		this.caloriesConsumed = float.NaN;
		this.totalUnits = float.NaN;
		if (this.Units < 0.001f)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000B2689 File Offset: 0x000B0889
	public static string GetEffectForFoodQuality(int qualityLevel)
	{
		qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
		return Edible.qualityEffects[qualityLevel];
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000B26A0 File Offset: 0x000B08A0
	private void AddOnConsumeEffects(WorkerBase worker)
	{
		int num = Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
		int qualityLevel = this.FoodInfo.Quality + num;
		Effects component = worker.GetComponent<Effects>();
		component.Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
		for (int i = 0; i < this.spices.Count; i++)
		{
			Effect statBonus = this.spices[i].StatBonus;
			if (statBonus != null)
			{
				float duration = statBonus.duration;
				statBonus.duration = this.caloriesConsumed * 0.001f / 1000f * 600f;
				component.Add(statBonus, true);
				statBonus.duration = duration;
			}
		}
		if (base.gameObject.HasTag(GameTags.Rehydrated))
		{
			component.Add(FoodRehydratorConfig.RehydrationEffect, true);
		}
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000B2780 File Offset: 0x000B0980
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Edibles.Remove(this);
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000B2793 File Offset: 0x000B0993
	public int GetQuality()
	{
		return this.foodInfo.Quality;
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000B27A0 File Offset: 0x000B09A0
	public int GetMorale()
	{
		int num = 0;
		string effectForFoodQuality = Edible.GetEffectForFoodQuality(this.foodInfo.Quality);
		foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
		{
			if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
			{
				num += Mathf.RoundToInt(attributeModifier.Value);
			}
		}
		return num;
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000B2840 File Offset: 0x000B0A40
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Information, false));
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), Descriptor.DescriptorType.Effect, false));
		int morale = this.GetMorale();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), Descriptor.DescriptorType.Effect, false));
		foreach (string text in this.foodInfo.Effects)
		{
			string text2 = "";
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(text).SelfModifiers)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					"\n    • ",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
					": ",
					attributeModifier.GetFormattedString()
				});
			}
			list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION") + text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000B2A9C File Offset: 0x000B0C9C
	public void ApplySpicesToOtherEdible(Edible other)
	{
		if (this.spices != null && other != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				other.SpiceEdible(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000B2AE8 File Offset: 0x000B0CE8
	public void OnSplitTick(Pickupable thePieceTaken)
	{
		Edible component = thePieceTaken.GetComponent<Edible>();
		this.ApplySpicesToOtherEdible(component);
		if (base.GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			component.AddTag(GameTags.Rehydrated);
		}
	}

	// Token: 0x040011D5 RID: 4565
	private PrimaryElement primaryElement;

	// Token: 0x040011D6 RID: 4566
	public string FoodID;

	// Token: 0x040011D7 RID: 4567
	private EdiblesManager.FoodInfo foodInfo;

	// Token: 0x040011D9 RID: 4569
	public float unitsConsumed = float.NaN;

	// Token: 0x040011DA RID: 4570
	public float caloriesConsumed = float.NaN;

	// Token: 0x040011DB RID: 4571
	private float totalFeedingTime = float.NaN;

	// Token: 0x040011DC RID: 4572
	private float totalUnits = float.NaN;

	// Token: 0x040011DD RID: 4573
	private float totalConsumableCalories = float.NaN;

	// Token: 0x040011DE RID: 4574
	[Serialize]
	private List<SpiceInstance> spices = new List<SpiceInstance>();

	// Token: 0x040011DF RID: 4575
	private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x040011E0 RID: 4576
	private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", (1f + DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS) / 2E-05f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x040011E1 RID: 4577
	private AttributeModifier currentModifier;

	// Token: 0x040011E2 RID: 4578
	private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		component.OnCraft(data);
	});

	// Token: 0x040011E3 RID: 4579
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040011E4 RID: 4580
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x040011E5 RID: 4581
	private static readonly HashedString[] saltWorkAnims = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x040011E6 RID: 4582
	private static readonly HashedString[] saltHatWorkAnims = new HashedString[]
	{
		"salt_hat_pre",
		"salt_hat_loop"
	};

	// Token: 0x040011E7 RID: 4583
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040011E8 RID: 4584
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};

	// Token: 0x040011E9 RID: 4585
	private static readonly HashedString[] saltWorkPstAnim = new HashedString[]
	{
		"salt_pst"
	};

	// Token: 0x040011EA RID: 4586
	private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[]
	{
		"salt_hat_pst"
	};

	// Token: 0x040011EB RID: 4587
	private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>
	{
		{
			-1,
			"EdibleMinus3"
		},
		{
			0,
			"EdibleMinus2"
		},
		{
			1,
			"EdibleMinus1"
		},
		{
			2,
			"Edible0"
		},
		{
			3,
			"Edible1"
		},
		{
			4,
			"Edible2"
		},
		{
			5,
			"Edible3"
		}
	};

	// Token: 0x02001357 RID: 4951
	public class EdibleStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060086B0 RID: 34480 RVA: 0x003298ED File Offset: 0x00327AED
		// (set) Token: 0x060086B1 RID: 34481 RVA: 0x003298F5 File Offset: 0x00327AF5
		public float amount { get; private set; }

		// Token: 0x060086B2 RID: 34482 RVA: 0x003298FE File Offset: 0x00327AFE
		public EdibleStartWorkInfo(Workable workable, float amount) : base(workable)
		{
			this.amount = amount;
		}
	}
}
