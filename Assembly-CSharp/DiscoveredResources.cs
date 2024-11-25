using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000858 RID: 2136
[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
	// Token: 0x06003B73 RID: 15219 RVA: 0x0014761A File Offset: 0x0014581A
	public static void DestroyInstance()
	{
		DiscoveredResources.Instance = null;
	}

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06003B74 RID: 15220 RVA: 0x00147624 File Offset: 0x00145824
	// (remove) Token: 0x06003B75 RID: 15221 RVA: 0x0014765C File Offset: 0x0014585C
	public event Action<Tag, Tag> OnDiscover;

	// Token: 0x06003B76 RID: 15222 RVA: 0x00147694 File Offset: 0x00145894
	public void Discover(Tag tag, Tag categoryTag)
	{
		bool flag = this.Discovered.Add(tag);
		this.DiscoverCategory(categoryTag, tag);
		if (flag)
		{
			if (this.OnDiscover != null)
			{
				this.OnDiscover(categoryTag, tag);
			}
			if (!this.newDiscoveries.ContainsKey(tag))
			{
				this.newDiscoveries.Add(tag, (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
			}
		}
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x001476FC File Offset: 0x001458FC
	public void Discover(Tag tag)
	{
		this.Discover(tag, DiscoveredResources.GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x00147715 File Offset: 0x00145915
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DiscoveredResources.Instance = this;
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x00147723 File Offset: 0x00145923
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FilterDisabledContent();
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x00147734 File Offset: 0x00145934
	private void FilterDisabledContent()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		foreach (Tag tag in this.Discovered)
		{
			Element element = ElementLoader.GetElement(tag);
			if (element != null && element.disabled)
			{
				hashSet.Add(tag);
			}
			else
			{
				GameObject gameObject = Assets.TryGetPrefab(tag);
				if (gameObject != null && gameObject.HasTag(GameTags.DeprecatedContent))
				{
					hashSet.Add(tag);
				}
				else if (gameObject == null)
				{
					hashSet.Add(tag);
				}
			}
		}
		foreach (Tag item in hashSet)
		{
			this.Discovered.Remove(item);
		}
		foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in this.DiscoveredCategories)
		{
			foreach (Tag item2 in hashSet)
			{
				if (keyValuePair.Value.Contains(item2))
				{
					keyValuePair.Value.Remove(item2);
				}
			}
		}
		foreach (string s in new List<string>
		{
			"Pacu",
			"PacuCleaner",
			"PacuTropical",
			"PacuBaby",
			"PacuCleanerBaby",
			"PacuTropicalBaby"
		})
		{
			if (this.DiscoveredCategories.ContainsKey(s))
			{
				List<Tag> list = this.DiscoveredCategories[s].ToList<Tag>();
				SolidConsumerMonitor.Def def = Assets.GetPrefab(s).GetDef<SolidConsumerMonitor.Def>();
				foreach (Tag tag2 in list)
				{
					if (def.diet.GetDietInfo(tag2) == null)
					{
						this.DiscoveredCategories[s].Remove(tag2);
					}
				}
			}
		}
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x001479DC File Offset: 0x00145BDC
	public bool CheckAllDiscoveredAreNew()
	{
		foreach (Tag key in this.Discovered)
		{
			if (!this.newDiscoveries.ContainsKey(key))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x00147A40 File Offset: 0x00145C40
	private void DiscoverCategory(Tag category_tag, Tag item_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.DiscoveredCategories.TryGetValue(category_tag, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.DiscoveredCategories[category_tag] = hashSet;
		}
		hashSet.Add(item_tag);
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x00147A78 File Offset: 0x00145C78
	public HashSet<Tag> GetDiscovered()
	{
		return this.Discovered;
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x00147A80 File Offset: 0x00145C80
	public bool IsDiscovered(Tag tag)
	{
		return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x00147AA0 File Offset: 0x00145CA0
	public bool AnyDiscovered(ICollection<Tag> tags)
	{
		foreach (Tag tag in tags)
		{
			if (this.IsDiscovered(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x00147AF4 File Offset: 0x00145CF4
	public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return this.DiscoveredCategories.TryGetValue(tag, out resources);
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x00147B04 File Offset: 0x00145D04
	public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		HashSet<Tag> result;
		if (this.DiscoveredCategories.TryGetValue(tag, out result))
		{
			return result;
		}
		return new HashSet<Tag>();
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x00147B28 File Offset: 0x00145D28
	public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag key in tagSet)
		{
			HashSet<Tag> value;
			if (this.DiscoveredCategories.TryGetValue(key, out value))
			{
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x00147B88 File Offset: 0x00145D88
	public static Tag GetCategoryForTags(HashSet<Tag> tags)
	{
		Tag result = Tag.Invalid;
		foreach (Tag tag in tags)
		{
			if (GameTags.AllCategories.Contains(tag) || GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				result = tag;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x00147BF4 File Offset: 0x00145DF4
	public static Tag GetCategoryForEntity(KPrefabID entity)
	{
		ElementChunk component = entity.GetComponent<ElementChunk>();
		if (component != null)
		{
			return component.GetComponent<PrimaryElement>().Element.materialCategory;
		}
		return DiscoveredResources.GetCategoryForTags(entity.Tags);
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x00147C30 File Offset: 0x00145E30
	public void Sim4000ms(float dt)
	{
		float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, float> keyValuePair in this.newDiscoveries)
		{
			if (num - keyValuePair.Value > 3f)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.newDiscoveries.Remove(key);
		}
	}

	// Token: 0x040023F1 RID: 9201
	public static DiscoveredResources Instance;

	// Token: 0x040023F2 RID: 9202
	[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

	// Token: 0x040023F3 RID: 9203
	[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

	// Token: 0x040023F5 RID: 9205
	[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();
}
