using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000546 RID: 1350
public class Diet
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06001F00 RID: 7936 RVA: 0x000AD66C File Offset: 0x000AB86C
	// (set) Token: 0x06001F01 RID: 7937 RVA: 0x000AD674 File Offset: 0x000AB874
	public Diet.Info[] infos { get; private set; }

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000AD67D File Offset: 0x000AB87D
	// (set) Token: 0x06001F03 RID: 7939 RVA: 0x000AD685 File Offset: 0x000AB885
	public Diet.Info[] noPlantInfos { get; private set; }

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000AD68E File Offset: 0x000AB88E
	// (set) Token: 0x06001F05 RID: 7941 RVA: 0x000AD696 File Offset: 0x000AB896
	public Diet.Info[] directlyEatenPlantInfos { get; private set; }

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06001F06 RID: 7942 RVA: 0x000AD69F File Offset: 0x000AB89F
	public bool CanEatAnyNonDirectlyEdiblePlant
	{
		get
		{
			return this.noPlantInfos != null && this.noPlantInfos.Length != 0;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06001F07 RID: 7943 RVA: 0x000AD6B5 File Offset: 0x000AB8B5
	public bool CanEatAnyPlantDirectly
	{
		get
		{
			return this.directlyEatenPlantInfos != null && this.directlyEatenPlantInfos.Length != 0;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06001F08 RID: 7944 RVA: 0x000AD6CB File Offset: 0x000AB8CB
	public bool AllConsumablesAreDirectlyEdiblePlants
	{
		get
		{
			return this.CanEatAnyPlantDirectly && (this.noPlantInfos == null || this.noPlantInfos.Length == 0);
		}
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x000AD6EC File Offset: 0x000AB8EC
	public bool IsConsumedTagAbleToBeEatenDirectly(Tag tag)
	{
		if (this.directlyEatenPlantInfos == null)
		{
			return false;
		}
		for (int i = 0; i < this.directlyEatenPlantInfos.Length; i++)
		{
			if (this.directlyEatenPlantInfos[i].consumedTags.Contains(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x000AD730 File Offset: 0x000AB930
	private void UpdateSecondaryInfoArrays()
	{
		Diet.Info[] directlyEatenPlantInfos;
		if (this.infos != null)
		{
			directlyEatenPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatPlantDirectly || i.foodType == Diet.Info.FoodType.EatPlantStorage
			select i).ToArray<Diet.Info>();
		}
		else
		{
			directlyEatenPlantInfos = null;
		}
		this.directlyEatenPlantInfos = directlyEatenPlantInfos;
		Diet.Info[] noPlantInfos;
		if (this.infos != null)
		{
			noPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatSolid
			select i).ToArray<Diet.Info>();
		}
		else
		{
			noPlantInfos = null;
		}
		this.noPlantInfos = noPlantInfos;
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x000AD7C0 File Offset: 0x000AB9C0
	public Diet(params Diet.Info[] infos)
	{
		this.infos = infos;
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		for (int i = 0; i < infos.Length; i++)
		{
			Diet.Info info = infos[i];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000AD964 File Offset: 0x000ABB64
	public Diet(Diet diet)
	{
		this.infos = new Diet.Info[diet.infos.Length];
		for (int i = 0; i < diet.infos.Length; i++)
		{
			this.infos[i] = new Diet.Info(diet.infos[i]);
		}
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		Diet.Info[] infos = this.infos;
		for (int j = 0; j < infos.Length; j++)
		{
			Diet.Info info = infos[j];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000ADB40 File Offset: 0x000ABD40
	public Diet.Info GetDietInfo(Tag tag)
	{
		Diet.Info result = null;
		this.consumedTagToInfo.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x000ADB60 File Offset: 0x000ABD60
	public void FilterDLC()
	{
		foreach (Diet.Info info in this.infos)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag tag in info.consumedTags)
			{
				GameObject prefab = Assets.GetPrefab(tag);
				if (!SaveLoader.Instance.IsDlcListActiveForCurrentSave(prefab.GetComponent<KPrefabID>().requiredDlcIds))
				{
					list.Add(tag);
				}
			}
			using (List<Tag>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Tag invalid_tag = enumerator2.Current;
					info.consumedTags.Remove(invalid_tag);
					this.consumedTags.RemoveAll((KeyValuePair<Tag, float> t) => t.Key == invalid_tag);
					this.consumedTagToInfo.Remove(invalid_tag);
				}
			}
			GameObject gameObject = (info.producedElement != Tag.Invalid) ? Assets.GetPrefab(info.producedElement) : null;
			if (gameObject != null && !SaveLoader.Instance.IsDlcListActiveForCurrentSave(gameObject.GetComponent<KPrefabID>().requiredDlcIds))
			{
				info.consumedTags.Clear();
			}
		}
		this.infos = (from i in this.infos
		where i.consumedTags.Count > 0
		select i).ToArray<Diet.Info>();
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x0400117D RID: 4477
	public List<KeyValuePair<Tag, float>> consumedTags;

	// Token: 0x0400117E RID: 4478
	public List<KeyValuePair<Tag, float>> producedTags;

	// Token: 0x0400117F RID: 4479
	private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

	// Token: 0x02001313 RID: 4883
	public class Info
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060085BE RID: 34238 RVA: 0x00326FE9 File Offset: 0x003251E9
		// (set) Token: 0x060085BF RID: 34239 RVA: 0x00326FF1 File Offset: 0x003251F1
		public HashSet<Tag> consumedTags { get; private set; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060085C0 RID: 34240 RVA: 0x00326FFA File Offset: 0x003251FA
		// (set) Token: 0x060085C1 RID: 34241 RVA: 0x00327002 File Offset: 0x00325202
		public Tag producedElement { get; private set; }

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060085C2 RID: 34242 RVA: 0x0032700B File Offset: 0x0032520B
		// (set) Token: 0x060085C3 RID: 34243 RVA: 0x00327013 File Offset: 0x00325213
		public float caloriesPerKg { get; private set; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060085C4 RID: 34244 RVA: 0x0032701C File Offset: 0x0032521C
		// (set) Token: 0x060085C5 RID: 34245 RVA: 0x00327024 File Offset: 0x00325224
		public float producedConversionRate { get; private set; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060085C6 RID: 34246 RVA: 0x0032702D File Offset: 0x0032522D
		// (set) Token: 0x060085C7 RID: 34247 RVA: 0x00327035 File Offset: 0x00325235
		public byte diseaseIdx { get; private set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060085C8 RID: 34248 RVA: 0x0032703E File Offset: 0x0032523E
		// (set) Token: 0x060085C9 RID: 34249 RVA: 0x00327046 File Offset: 0x00325246
		public float diseasePerKgProduced { get; private set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060085CA RID: 34250 RVA: 0x0032704F File Offset: 0x0032524F
		// (set) Token: 0x060085CB RID: 34251 RVA: 0x00327057 File Offset: 0x00325257
		public bool emmitDiseaseOnCell { get; private set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060085CC RID: 34252 RVA: 0x00327060 File Offset: 0x00325260
		// (set) Token: 0x060085CD RID: 34253 RVA: 0x00327068 File Offset: 0x00325268
		public bool produceSolidTile { get; private set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060085CE RID: 34254 RVA: 0x00327071 File Offset: 0x00325271
		// (set) Token: 0x060085CF RID: 34255 RVA: 0x00327079 File Offset: 0x00325279
		public Diet.Info.FoodType foodType { get; private set; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060085D0 RID: 34256 RVA: 0x00327082 File Offset: 0x00325282
		// (set) Token: 0x060085D1 RID: 34257 RVA: 0x0032708A File Offset: 0x0032528A
		public string[] eatAnims { get; set; }

		// Token: 0x060085D2 RID: 34258 RVA: 0x00327094 File Offset: 0x00325294
		public Info(HashSet<Tag> consumed_tags, Tag produced_element, float calories_per_kg, float produced_conversion_rate = 1f, string disease_id = null, float disease_per_kg_produced = 0f, bool produce_solid_tile = false, Diet.Info.FoodType food_type = Diet.Info.FoodType.EatSolid, bool emmit_disease_on_cell = false, string[] eat_anims = null)
		{
			this.consumedTags = consumed_tags;
			this.producedElement = produced_element;
			this.caloriesPerKg = calories_per_kg;
			this.producedConversionRate = produced_conversion_rate;
			if (!string.IsNullOrEmpty(disease_id))
			{
				this.diseaseIdx = Db.Get().Diseases.GetIndex(disease_id);
			}
			else
			{
				this.diseaseIdx = byte.MaxValue;
			}
			this.diseasePerKgProduced = disease_per_kg_produced;
			this.emmitDiseaseOnCell = emmit_disease_on_cell;
			this.produceSolidTile = produce_solid_tile;
			this.foodType = food_type;
			if (eat_anims == null)
			{
				eat_anims = new string[]
				{
					"eat_pre",
					"eat_loop",
					"eat_pst"
				};
			}
			this.eatAnims = eat_anims;
		}

		// Token: 0x060085D3 RID: 34259 RVA: 0x00327144 File Offset: 0x00325344
		public Info(Diet.Info info)
		{
			this.consumedTags = new HashSet<Tag>(info.consumedTags);
			this.producedElement = info.producedElement;
			this.caloriesPerKg = info.caloriesPerKg;
			this.producedConversionRate = info.producedConversionRate;
			this.diseaseIdx = info.diseaseIdx;
			this.diseasePerKgProduced = info.diseasePerKgProduced;
			this.emmitDiseaseOnCell = info.emmitDiseaseOnCell;
			this.produceSolidTile = info.produceSolidTile;
			this.foodType = info.foodType;
			this.eatAnims = info.eatAnims;
		}

		// Token: 0x060085D4 RID: 34260 RVA: 0x003271D4 File Offset: 0x003253D4
		public bool IsMatch(Tag tag)
		{
			return this.consumedTags.Contains(tag);
		}

		// Token: 0x060085D5 RID: 34261 RVA: 0x003271E4 File Offset: 0x003253E4
		public bool IsMatch(HashSet<Tag> tags)
		{
			if (tags.Count < this.consumedTags.Count)
			{
				foreach (Tag item in tags)
				{
					if (this.consumedTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (Tag item2 in this.consumedTags)
			{
				if (tags.Contains(item2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060085D6 RID: 34262 RVA: 0x003272A0 File Offset: 0x003254A0
		public float ConvertCaloriesToConsumptionMass(float calories)
		{
			return calories / this.caloriesPerKg;
		}

		// Token: 0x060085D7 RID: 34263 RVA: 0x003272AA File Offset: 0x003254AA
		public float ConvertConsumptionMassToCalories(float mass)
		{
			return this.caloriesPerKg * mass;
		}

		// Token: 0x060085D8 RID: 34264 RVA: 0x003272B4 File Offset: 0x003254B4
		public float ConvertConsumptionMassToProducedMass(float consumed_mass)
		{
			return consumed_mass * this.producedConversionRate;
		}

		// Token: 0x060085D9 RID: 34265 RVA: 0x003272BE File Offset: 0x003254BE
		public float ConvertProducedMassToConsumptionMass(float produced_mass)
		{
			return produced_mass / this.producedConversionRate;
		}

		// Token: 0x02002492 RID: 9362
		public enum FoodType
		{
			// Token: 0x0400A22F RID: 41519
			EatSolid,
			// Token: 0x0400A230 RID: 41520
			EatPlantDirectly,
			// Token: 0x0400A231 RID: 41521
			EatPlantStorage
		}
	}
}
