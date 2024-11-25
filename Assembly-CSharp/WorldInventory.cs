using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000B66 RID: 2918
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x0600579A RID: 22426 RVA: 0x001F4EEB File Offset: 0x001F30EB
	public WorldContainer WorldContainer
	{
		get
		{
			if (this.m_worldContainer == null)
			{
				this.m_worldContainer = base.GetComponent<WorldContainer>();
			}
			return this.m_worldContainer;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x0600579B RID: 22427 RVA: 0x001F4F0D File Offset: 0x001F310D
	public bool HasValidCount
	{
		get
		{
			return this.hasValidCount;
		}
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x0600579C RID: 22428 RVA: 0x001F4F18 File Offset: 0x001F3118
	private int worldId
	{
		get
		{
			WorldContainer worldContainer = this.WorldContainer;
			if (!(worldContainer != null))
			{
				return -1;
			}
			return worldContainer.id;
		}
	}

	// Token: 0x0600579D RID: 22429 RVA: 0x001F4F40 File Offset: 0x001F3140
	protected override void OnPrefabInit()
	{
		base.Subscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Subscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
		this.m_worldContainer = base.GetComponent<WorldContainer>();
	}

	// Token: 0x0600579E RID: 22430 RVA: 0x001F4FB0 File Offset: 0x001F31B0
	protected override void OnCleanUp()
	{
		base.Unsubscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Unsubscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.OnCleanUp();
	}

	// Token: 0x0600579F RID: 22431 RVA: 0x001F5008 File Offset: 0x001F3208
	private void GenerateInventoryReport(object data)
	{
		int num = 0;
		int num2 = 0;
		foreach (Brain brain in Components.Brains.GetWorldItems(this.worldId, false))
		{
			CreatureBrain creatureBrain = brain as CreatureBrain;
			if (creatureBrain != null)
			{
				if (creatureBrain.HasTag(GameTags.Creatures.Wild))
				{
					num++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
				else
				{
					num2++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null && component.IsModuleInterior)
			{
				Clustercraft clustercraft = component.GetComponent<ClusterGridEntity>() as Clustercraft;
				if (clustercraft != null && clustercraft.Status != Clustercraft.CraftStatus.Grounded)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, clustercraft.Name, null);
					return;
				}
			}
		}
		else
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName, null);
				}
			}
		}
	}

	// Token: 0x060057A0 RID: 22432 RVA: 0x001F5198 File Offset: 0x001F3398
	protected override void OnSpawn()
	{
		this.Prober = MinionGroupProber.Get();
		base.StartCoroutine(this.InitialRefresh());
	}

	// Token: 0x060057A1 RID: 22433 RVA: 0x001F51B2 File Offset: 0x001F33B2
	private IEnumerator InitialRefresh()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		for (int j = 0; j < Components.Pickupables.Count; j++)
		{
			Pickupable pickupable = Components.Pickupables[j];
			if (pickupable != null)
			{
				ReachabilityMonitor.Instance smi = pickupable.GetSMI<ReachabilityMonitor.Instance>();
				if (smi != null)
				{
					smi.UpdateReachability();
				}
			}
		}
		yield break;
	}

	// Token: 0x060057A2 RID: 22434 RVA: 0x001F51BA File Offset: 0x001F33BA
	public bool IsReachable(Pickupable pickupable)
	{
		return this.Prober.IsReachable(pickupable);
	}

	// Token: 0x060057A3 RID: 22435 RVA: 0x001F51C8 File Offset: 0x001F33C8
	public float GetTotalAmount(Tag tag, bool includeRelatedWorlds)
	{
		float result = 0f;
		this.accessibleAmounts.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x060057A4 RID: 22436 RVA: 0x001F51EC File Offset: 0x001F33EC
	public ICollection<Pickupable> GetPickupables(Tag tag, bool includeRelatedWorlds = false)
	{
		if (!includeRelatedWorlds)
		{
			HashSet<Pickupable> result = null;
			this.Inventory.TryGetValue(tag, out result);
			return result;
		}
		return ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
	}

	// Token: 0x060057A5 RID: 22437 RVA: 0x001F5218 File Offset: 0x001F3418
	public List<Pickupable> CreatePickupablesList(Tag tag)
	{
		HashSet<Pickupable> hashSet = null;
		this.Inventory.TryGetValue(tag, out hashSet);
		if (hashSet == null)
		{
			return null;
		}
		return hashSet.ToList<Pickupable>();
	}

	// Token: 0x060057A6 RID: 22438 RVA: 0x001F5244 File Offset: 0x001F3444
	public float GetAmount(Tag tag, bool includeRelatedWorlds)
	{
		float num;
		if (!includeRelatedWorlds)
		{
			num = this.GetTotalAmount(tag, includeRelatedWorlds);
			num -= MaterialNeeds.GetAmount(tag, this.worldId, includeRelatedWorlds);
		}
		else
		{
			num = ClusterUtil.GetAmountFromRelatedWorlds(this, tag);
		}
		return Mathf.Max(num, 0f);
	}

	// Token: 0x060057A7 RID: 22439 RVA: 0x001F528C File Offset: 0x001F348C
	public int GetCountWithAdditionalTag(Tag tag, Tag additionalTag, bool includeRelatedWorlds = false)
	{
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		int num = 0;
		if (collection2 != null)
		{
			if (additionalTag.IsValid)
			{
				using (IEnumerator<Pickupable> enumerator = collection2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasTag(additionalTag))
						{
							num++;
						}
					}
					return num;
				}
			}
			num = collection2.Count;
		}
		return num;
	}

	// Token: 0x060057A8 RID: 22440 RVA: 0x001F5308 File Offset: 0x001F3508
	public float GetAmountWithoutTag(Tag tag, bool includeRelatedWorlds = false, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmount(tag, includeRelatedWorlds);
		}
		float num = 0f;
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		if (collection2 != null)
		{
			foreach (Pickupable pickupable in collection2)
			{
				if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !pickupable.KPrefabID.HasAnyTags(forbiddenTags))
				{
					num += pickupable.TotalAmount;
				}
			}
		}
		return num;
	}

	// Token: 0x060057A9 RID: 22441 RVA: 0x001F53B0 File Offset: 0x001F35B0
	private void Update()
	{
		int num = 0;
		Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
		int worldId = this.worldId;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Tag, HashSet<Pickupable>> keyValuePair = enumerator.Current;
			if (num == this.accessibleUpdateIndex || this.firstUpdate)
			{
				Tag key = keyValuePair.Key;
				IEnumerable<Pickupable> value = keyValuePair.Value;
				float num2 = 0f;
				foreach (Pickupable pickupable in value)
				{
					if (pickupable != null && pickupable.GetMyWorldId() == worldId && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
					{
						num2 += pickupable.TotalAmount;
					}
				}
				if (!this.hasValidCount && this.accessibleUpdateIndex + 1 >= this.Inventory.Count)
				{
					this.hasValidCount = true;
					if (this.worldId == ClusterManager.Instance.activeWorldId)
					{
						this.hasValidCount = true;
						PinnedResourcesPanel.Instance.ClearExcessiveNewItems();
						PinnedResourcesPanel.Instance.Refresh();
					}
				}
				this.accessibleAmounts[key] = num2;
				this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
				break;
			}
			num++;
		}
		this.firstUpdate = false;
	}

	// Token: 0x060057AA RID: 22442 RVA: 0x001F550C File Offset: 0x001F370C
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x060057AB RID: 22443 RVA: 0x001F5514 File Offset: 0x001F3714
	private void OnAddedFetchable(object data)
	{
		GameObject gameObject = (GameObject)data;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		if (component.HasAnyTags(WorldInventory.NonCritterEntitiesTags))
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2.GetMyWorldId() != this.worldId)
		{
			return;
		}
		Tag tag = component.PrefabID();
		if (!this.Inventory.ContainsKey(tag))
		{
			Tag categoryForEntity = DiscoveredResources.GetCategoryForEntity(component);
			DebugUtil.DevAssertArgs(categoryForEntity.IsValid, new object[]
			{
				component2.name,
				"was found by worldinventory but doesn't have a category! Add it to the element definition."
			});
			DiscoveredResources.Instance.Discover(tag, categoryForEntity);
		}
		HashSet<Pickupable> hashSet;
		if (!this.Inventory.TryGetValue(tag, out hashSet))
		{
			hashSet = new HashSet<Pickupable>();
			this.Inventory[tag] = hashSet;
		}
		hashSet.Add(component2);
		foreach (Tag key in component.Tags)
		{
			if (!this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet = new HashSet<Pickupable>();
				this.Inventory[key] = hashSet;
			}
			hashSet.Add(component2);
		}
	}

	// Token: 0x060057AC RID: 22444 RVA: 0x001F5640 File Offset: 0x001F3840
	private void OnRemovedFetchable(object data)
	{
		Pickupable component = ((GameObject)data).GetComponent<Pickupable>();
		KPrefabID kprefabID = component.KPrefabID;
		HashSet<Pickupable> hashSet;
		if (this.Inventory.TryGetValue(kprefabID.PrefabTag, out hashSet))
		{
			hashSet.Remove(component);
		}
		foreach (Tag key in kprefabID.Tags)
		{
			if (this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet.Remove(component);
			}
		}
	}

	// Token: 0x060057AD RID: 22445 RVA: 0x001F56D8 File Offset: 0x001F38D8
	public Dictionary<Tag, float> GetAccessibleAmounts()
	{
		return this.accessibleAmounts;
	}

	// Token: 0x04003944 RID: 14660
	private WorldContainer m_worldContainer;

	// Token: 0x04003945 RID: 14661
	[Serialize]
	public List<Tag> pinnedResources = new List<Tag>();

	// Token: 0x04003946 RID: 14662
	[Serialize]
	public List<Tag> notifyResources = new List<Tag>();

	// Token: 0x04003947 RID: 14663
	private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();

	// Token: 0x04003948 RID: 14664
	private MinionGroupProber Prober;

	// Token: 0x04003949 RID: 14665
	private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();

	// Token: 0x0400394A RID: 14666
	private bool hasValidCount;

	// Token: 0x0400394B RID: 14667
	private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>(delegate(WorldInventory component, object data)
	{
		component.GenerateInventoryReport(data);
	});

	// Token: 0x0400394C RID: 14668
	private int accessibleUpdateIndex;

	// Token: 0x0400394D RID: 14669
	private bool firstUpdate = true;

	// Token: 0x0400394E RID: 14670
	private static Tag[] NonCritterEntitiesTags = new Tag[]
	{
		GameTags.DupeBrain,
		GameTags.Robot
	};
}
