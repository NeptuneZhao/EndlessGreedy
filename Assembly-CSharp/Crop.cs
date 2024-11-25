using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007FC RID: 2044
[AddComponentMenu("KMonoBehaviour/scripts/Crop")]
public class Crop : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x0600387D RID: 14461 RVA: 0x001346B4 File Offset: 0x001328B4
	public string cropId
	{
		get
		{
			return this.cropVal.cropId;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x0600387E RID: 14462 RVA: 0x001346C1 File Offset: 0x001328C1
	// (set) Token: 0x0600387F RID: 14463 RVA: 0x001346C9 File Offset: 0x001328C9
	public Storage PlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
		set
		{
			this.planterStorage = value;
		}
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x001346D2 File Offset: 0x001328D2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Crops.Add(this);
		this.yield = this.GetAttributes().Add(Db.Get().PlantAttributes.YieldAmount);
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x00134705 File Offset: 0x00132905
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Crop>(1272413801, Crop.OnHarvestDelegate);
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x0013471E File Offset: 0x0013291E
	public void Configure(Crop.CropVal cropval)
	{
		this.cropVal = cropval;
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x00134727 File Offset: 0x00132927
	public bool CanGrow()
	{
		return this.cropVal.renewable;
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x00134734 File Offset: 0x00132934
	public void SpawnConfiguredFruit(object callbackParam)
	{
		if (this == null)
		{
			return;
		}
		Crop.CropVal cropVal = this.cropVal;
		if (!string.IsNullOrEmpty(cropVal.cropId))
		{
			this.SpawnSomeFruit(cropVal.cropId, this.yield.GetTotalValue());
			base.Trigger(-1072826864, this);
		}
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x00134788 File Offset: 0x00132988
	public void SpawnSomeFruit(Tag cropID, float amount)
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(cropID), base.transform.GetPosition() + new Vector3(0f, 0.75f, 0f), Grid.SceneLayer.Ore, null, 0);
		if (gameObject != null)
		{
			MutantPlant component = base.GetComponent<MutantPlant>();
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component != null && component.IsOriginal && component2 != null && base.GetComponent<SeedProducer>().RollForMutation())
			{
				component2.Mutate();
			}
			gameObject.SetActive(true);
			PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
			component3.Units = amount;
			component3.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			base.Trigger(35625290, gameObject);
			Edible component4 = gameObject.GetComponent<Edible>();
			if (component4)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component4.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.HARVESTED, "{0}", component4.GetProperName()), UI.ENDOFDAYREPORT.NOTES.HARVESTED_CONTEXT);
				return;
			}
		}
		else
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"tried to spawn an invalid crop prefab:",
				cropID
			});
		}
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x001348AC File Offset: 0x00132AAC
	protected override void OnCleanUp()
	{
		Components.Crops.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x001348BF File Offset: 0x00132ABF
	private void OnHarvest(object obj)
	{
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x001348C1 File Offset: 0x00132AC1
	public List<Descriptor> RequirementDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x001348C8 File Offset: 0x00132AC8
	public List<Descriptor> InformationDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Tag tag = new Tag(this.cropVal.cropId);
		GameObject prefab = Assets.GetPrefab(tag);
		Edible component = prefab.GetComponent<Edible>();
		Klei.AI.Attribute yieldAmount = Db.Get().PlantAttributes.YieldAmount;
		float preModifiedAttributeValue = go.GetComponent<Modifiers>().GetPreModifiedAttributeValue(yieldAmount);
		if (component != null)
		{
			DebugUtil.Assert(GameTags.DisplayAsCalories.Contains(tag), "Trying to display crop info for an edible fruit which isn't displayed as calories!", tag.ToString());
			float caloriesPerUnit = component.FoodInfo.CaloriesPerUnit;
			float calories = caloriesPerUnit * preModifiedAttributeValue;
			string text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
			Descriptor item = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD, "", GameUtil.GetFormattedCalories(caloriesPerUnit, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else
		{
			string text;
			if (GameTags.DisplayAsUnits.Contains(tag))
			{
				text = GameUtil.GetFormattedUnits((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, false, "");
			}
			else
			{
				text = GameUtil.GetFormattedMass((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			}
			Descriptor item2 = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD_NONFOOD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD_NONFOOD, text), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x00134A38 File Offset: 0x00132C38
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(go))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.InformationDescriptors(go))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x040021EE RID: 8686
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040021EF RID: 8687
	public Crop.CropVal cropVal;

	// Token: 0x040021F0 RID: 8688
	private AttributeInstance yield;

	// Token: 0x040021F1 RID: 8689
	public string domesticatedDesc = "";

	// Token: 0x040021F2 RID: 8690
	private Storage planterStorage;

	// Token: 0x040021F3 RID: 8691
	private static readonly EventSystem.IntraObjectHandler<Crop> OnHarvestDelegate = new EventSystem.IntraObjectHandler<Crop>(delegate(Crop component, object data)
	{
		component.OnHarvest(data);
	});

	// Token: 0x020016DF RID: 5855
	[Serializable]
	public struct CropVal
	{
		// Token: 0x060093CF RID: 37839 RVA: 0x0035A232 File Offset: 0x00358432
		public CropVal(string crop_id, float crop_duration, int num_produced = 1, bool renewable = true)
		{
			this.cropId = crop_id;
			this.cropDuration = crop_duration;
			this.numProduced = num_produced;
			this.renewable = renewable;
		}

		// Token: 0x04007112 RID: 28946
		public string cropId;

		// Token: 0x04007113 RID: 28947
		public float cropDuration;

		// Token: 0x04007114 RID: 28948
		public int numProduced;

		// Token: 0x04007115 RID: 28949
		public bool renewable;
	}
}
