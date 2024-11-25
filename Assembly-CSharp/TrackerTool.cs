using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
public class TrackerTool : KMonoBehaviour
{
	// Token: 0x06002449 RID: 9289 RVA: 0x000CA604 File Offset: 0x000C8804
	protected override void OnSpawn()
	{
		TrackerTool.Instance = this;
		base.OnSpawn();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			this.AddNewWorldTrackers(worldContainer.id);
		}
		foreach (object obj in Components.LiveMinionIdentities)
		{
			this.AddMinionTrackers((MinionIdentity)obj);
		}
		Components.LiveMinionIdentities.OnAdd += this.AddMinionTrackers;
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x000CA700 File Offset: 0x000C8900
	protected override void OnForcedCleanUp()
	{
		TrackerTool.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x000CA710 File Offset: 0x000C8910
	private void AddMinionTrackers(MinionIdentity identity)
	{
		this.minionTrackers.Add(identity, new List<MinionTracker>());
		identity.Subscribe(1969584890, delegate(object data)
		{
			this.minionTrackers.Remove(identity);
		});
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x000CA764 File Offset: 0x000C8964
	private void Refresh(object data)
	{
		int worldID = (int)data;
		this.AddNewWorldTrackers(worldID);
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x000CA780 File Offset: 0x000C8980
	private void RemoveWorld(object data)
	{
		int world_id = (int)data;
		this.worldTrackers.RemoveAll((WorldTracker match) => match.WorldID == world_id);
	}

	// Token: 0x0600244E RID: 9294 RVA: 0x000CA7B7 File Offset: 0x000C89B7
	public bool IsRocketInterior(int worldID)
	{
		return ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
	}

	// Token: 0x0600244F RID: 9295 RVA: 0x000CA7CC File Offset: 0x000C89CC
	private void AddNewWorldTrackers(int worldID)
	{
		this.worldTrackers.Add(new StressTracker(worldID));
		this.worldTrackers.Add(new KCalTracker(worldID));
		this.worldTrackers.Add(new IdleTracker(worldID));
		this.worldTrackers.Add(new BreathabilityTracker(worldID));
		this.worldTrackers.Add(new PowerUseTracker(worldID));
		this.worldTrackers.Add(new BatteryTracker(worldID));
		this.worldTrackers.Add(new CropTracker(worldID));
		this.worldTrackers.Add(new WorkingToiletTracker(worldID));
		this.worldTrackers.Add(new RadiationTracker(worldID));
		if (SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"))
		{
			this.worldTrackers.Add(new ElectrobankJoulesTracker(worldID));
		}
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.worldTrackers.Add(new RocketFuelTracker(worldID));
			this.worldTrackers.Add(new RocketOxidizerTracker(worldID));
		}
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			this.worldTrackers.Add(new WorkTimeTracker(worldID, Db.Get().ChoreGroups[i]));
			this.worldTrackers.Add(new ChoreCountTracker(worldID, Db.Get().ChoreGroups[i]));
		}
		this.worldTrackers.Add(new AllChoresCountTracker(worldID));
		this.worldTrackers.Add(new AllWorkTimeTracker(worldID));
		foreach (Tag tag in GameTags.CalorieCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag));
			foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag))
			{
				this.AddResourceTracker(worldID, gameObject.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag2 in GameTags.UnitCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag2));
			foreach (GameObject gameObject2 in Assets.GetPrefabsWithTag(tag2))
			{
				this.AddResourceTracker(worldID, gameObject2.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag3 in GameTags.MaterialCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag3));
			foreach (GameObject gameObject3 in Assets.GetPrefabsWithTag(tag3))
			{
				this.AddResourceTracker(worldID, gameObject3.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag4 in GameTags.OtherEntityTags)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag4));
			foreach (GameObject gameObject4 in Assets.GetPrefabsWithTag(tag4))
			{
				this.AddResourceTracker(worldID, gameObject4.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (GameObject gameObject5 in Assets.GetPrefabsWithTag(GameTags.CookingIngredient))
		{
			this.AddResourceTracker(worldID, gameObject5.GetComponent<KPrefabID>().PrefabTag);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.AddResourceTracker(worldID, foodInfo.Id);
		}
		foreach (Element element in ElementLoader.elements)
		{
			this.AddResourceTracker(worldID, element.tag);
		}
	}

	// Token: 0x06002450 RID: 9296 RVA: 0x000CAC94 File Offset: 0x000C8E94
	private void AddResourceTracker(int worldID, Tag tag)
	{
		if (this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag) != null)
		{
			return;
		}
		this.worldTrackers.Add(new ResourceTracker(worldID, tag));
	}

	// Token: 0x06002451 RID: 9297 RVA: 0x000CACEC File Offset: 0x000C8EEC
	public ResourceTracker GetResourceStatistic(int worldID, Tag tag)
	{
		return (ResourceTracker)this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag);
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000CAD2C File Offset: 0x000C8F2C
	public WorldTracker GetWorldTracker<T>(int worldID) where T : WorldTracker
	{
		return (T)((object)this.worldTrackers.Find((WorldTracker match) => match is T && ((T)((object)match)).WorldID == worldID));
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x000CAD68 File Offset: 0x000C8F68
	public ChoreCountTracker GetChoreGroupTracker(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreCountTracker)this.worldTrackers.Find((WorldTracker match) => match is ChoreCountTracker && ((ChoreCountTracker)match).WorldID == worldID && ((ChoreCountTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x000CADA8 File Offset: 0x000C8FA8
	public WorkTimeTracker GetWorkTimeTracker(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeTracker)this.worldTrackers.Find((WorldTracker match) => match is WorkTimeTracker && ((WorkTimeTracker)match).WorldID == worldID && ((WorkTimeTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x000CADE5 File Offset: 0x000C8FE5
	public MinionTracker GetMinionTracker<T>(MinionIdentity identity) where T : MinionTracker
	{
		return (T)((object)this.minionTrackers[identity].Find((MinionTracker match) => match is T));
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x000CAE24 File Offset: 0x000C9024
	public void Update()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		if (!this.trackerActive)
		{
			return;
		}
		if (this.minionTrackers.Count > 0)
		{
			this.updatingMinionTracker++;
			if (this.updatingMinionTracker >= this.minionTrackers.Count)
			{
				this.updatingMinionTracker = 0;
			}
			KeyValuePair<MinionIdentity, List<MinionTracker>> keyValuePair = this.minionTrackers.ElementAt(this.updatingMinionTracker);
			for (int i = 0; i < keyValuePair.Value.Count; i++)
			{
				keyValuePair.Value[i].UpdateData();
			}
		}
		if (this.worldTrackers.Count > 0)
		{
			for (int j = 0; j < this.numUpdatesPerFrame; j++)
			{
				this.updatingWorldTracker++;
				if (this.updatingWorldTracker >= this.worldTrackers.Count)
				{
					this.updatingWorldTracker = 0;
				}
				this.worldTrackers[this.updatingWorldTracker].UpdateData();
			}
		}
	}

	// Token: 0x040014AC RID: 5292
	public static TrackerTool Instance;

	// Token: 0x040014AD RID: 5293
	private List<WorldTracker> worldTrackers = new List<WorldTracker>();

	// Token: 0x040014AE RID: 5294
	private Dictionary<MinionIdentity, List<MinionTracker>> minionTrackers = new Dictionary<MinionIdentity, List<MinionTracker>>();

	// Token: 0x040014AF RID: 5295
	private int updatingWorldTracker;

	// Token: 0x040014B0 RID: 5296
	private int updatingMinionTracker;

	// Token: 0x040014B1 RID: 5297
	public bool trackerActive = true;

	// Token: 0x040014B2 RID: 5298
	private int numUpdatesPerFrame = 50;
}
