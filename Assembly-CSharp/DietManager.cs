using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000857 RID: 2135
[AddComponentMenu("KMonoBehaviour/scripts/DietManager")]
public class DietManager : KMonoBehaviour
{
	// Token: 0x06003B6A RID: 15210 RVA: 0x0014729A File Offset: 0x0014549A
	public static void DestroyInstance()
	{
		DietManager.Instance = null;
	}

	// Token: 0x06003B6B RID: 15211 RVA: 0x001472A2 File Offset: 0x001454A2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.diets = DietManager.CollectSaveDiets(null);
		DietManager.Instance = this;
	}

	// Token: 0x06003B6C RID: 15212 RVA: 0x001472BC File Offset: 0x001454BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscovered())
		{
			this.Discover(tag);
		}
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			Diet.Info[] infos = keyValuePair.Value.infos;
			for (int i = 0; i < infos.Length; i++)
			{
				foreach (Tag tag2 in infos[i].consumedTags)
				{
					if (Assets.GetPrefab(tag2) == null)
					{
						global::Debug.LogError(string.Format("Could not find prefab {0}, required by diet for {1}", tag2, keyValuePair.Key));
					}
				}
			}
		}
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x00147404 File Offset: 0x00145604
	private void Discover(Tag tag)
	{
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			if (keyValuePair.Value.GetDietInfo(tag) != null)
			{
				DiscoveredResources.Instance.Discover(tag, keyValuePair.Key);
			}
		}
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x00147474 File Offset: 0x00145674
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		this.Discover(tag);
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x00147480 File Offset: 0x00145680
	public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = diet;
			}
		}
		return dictionary;
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x00147528 File Offset: 0x00145728
	public static Dictionary<Tag, Diet> CollectSaveDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = new Diet(diet);
				dictionary[kprefabID.PrefabTag].FilterDLC();
			}
		}
		return dictionary;
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x001475E8 File Offset: 0x001457E8
	public Diet GetPrefabDiet(GameObject owner)
	{
		Diet result;
		if (this.diets.TryGetValue(owner.GetComponent<KPrefabID>().PrefabTag, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x040023EF RID: 9199
	private Dictionary<Tag, Diet> diets;

	// Token: 0x040023F0 RID: 9200
	public static DietManager Instance;
}
