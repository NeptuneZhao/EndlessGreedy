using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000869 RID: 2153
[AddComponentMenu("KMonoBehaviour/scripts/EdiblesManager")]
public class EdiblesManager : KMonoBehaviour
{
	// Token: 0x06003C13 RID: 15379 RVA: 0x0014DCB4 File Offset: 0x0014BEB4
	public static List<EdiblesManager.FoodInfo> GetAllLoadedFoodTypes()
	{
		return (from x in EdiblesManager.s_allFoodTypes
		where DlcManager.IsContentSubscribed(x.DlcId)
		select x).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x0014DCE4 File Offset: 0x0014BEE4
	public static List<EdiblesManager.FoodInfo> GetAllFoodTypes()
	{
		global::Debug.Assert(SaveLoader.Instance != null, "Call GetAllLoadedFoodTypes from the frontend");
		return (from x in EdiblesManager.s_allFoodTypes
		where SaveLoader.Instance.IsDLCActiveForCurrentSave(x.DlcId)
		select x).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x0014DD34 File Offset: 0x0014BF34
	public static EdiblesManager.FoodInfo GetFoodInfo(string foodID)
	{
		string key = foodID.Replace("Compost", "");
		EdiblesManager.FoodInfo result = null;
		EdiblesManager.s_allFoodMap.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x0014DD63 File Offset: 0x0014BF63
	public static bool TryGetFoodInfo(string foodID, out EdiblesManager.FoodInfo info)
	{
		info = null;
		if (string.IsNullOrEmpty(foodID))
		{
			return false;
		}
		info = EdiblesManager.GetFoodInfo(foodID);
		return info != null;
	}

	// Token: 0x04002464 RID: 9316
	private static List<EdiblesManager.FoodInfo> s_allFoodTypes = new List<EdiblesManager.FoodInfo>();

	// Token: 0x04002465 RID: 9317
	private static Dictionary<string, EdiblesManager.FoodInfo> s_allFoodMap = new Dictionary<string, EdiblesManager.FoodInfo>();

	// Token: 0x02001777 RID: 6007
	public class FoodInfo : IConsumableUIItem
	{
		// Token: 0x060095DA RID: 38362 RVA: 0x003605A8 File Offset: 0x0035E7A8
		public FoodInfo(string id, string dlcId, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot)
		{
			this.Id = id;
			this.DlcId = dlcId;
			this.CaloriesPerUnit = caloriesPerUnit;
			this.Quality = quality;
			this.PreserveTemperature = preserveTemperatue;
			this.RotTemperature = rotTemperature;
			this.StaleTime = spoilTime / 2f;
			this.SpoilTime = spoilTime;
			this.CanRot = can_rot;
			this.Name = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".NAME");
			this.Description = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".DESC");
			this.Effects = new List<string>();
			EdiblesManager.s_allFoodTypes.Add(this);
			EdiblesManager.s_allFoodMap[this.Id] = this;
		}

		// Token: 0x060095DB RID: 38363 RVA: 0x00360677 File Offset: 0x0035E877
		public EdiblesManager.FoodInfo AddEffects(List<string> effects, string[] dlcIds)
		{
			if (DlcManager.IsDlcListValidForCurrentContent(dlcIds))
			{
				this.Effects.AddRange(effects);
			}
			return this;
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060095DC RID: 38364 RVA: 0x0036068E File Offset: 0x0035E88E
		public string ConsumableId
		{
			get
			{
				return this.Id;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060095DD RID: 38365 RVA: 0x00360696 File Offset: 0x0035E896
		public string ConsumableName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060095DE RID: 38366 RVA: 0x0036069E File Offset: 0x0035E89E
		public int MajorOrder
		{
			get
			{
				return this.Quality;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x060095DF RID: 38367 RVA: 0x003606A6 File Offset: 0x0035E8A6
		public int MinorOrder
		{
			get
			{
				return (int)this.CaloriesPerUnit;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x060095E0 RID: 38368 RVA: 0x003606AF File Offset: 0x0035E8AF
		public bool Display
		{
			get
			{
				return this.CaloriesPerUnit != 0f;
			}
		}

		// Token: 0x040072C4 RID: 29380
		public string Id;

		// Token: 0x040072C5 RID: 29381
		public string DlcId;

		// Token: 0x040072C6 RID: 29382
		public string Name;

		// Token: 0x040072C7 RID: 29383
		public string Description;

		// Token: 0x040072C8 RID: 29384
		public float CaloriesPerUnit;

		// Token: 0x040072C9 RID: 29385
		public float PreserveTemperature;

		// Token: 0x040072CA RID: 29386
		public float RotTemperature;

		// Token: 0x040072CB RID: 29387
		public float StaleTime;

		// Token: 0x040072CC RID: 29388
		public float SpoilTime;

		// Token: 0x040072CD RID: 29389
		public bool CanRot;

		// Token: 0x040072CE RID: 29390
		public int Quality;

		// Token: 0x040072CF RID: 29391
		public List<string> Effects;
	}
}
