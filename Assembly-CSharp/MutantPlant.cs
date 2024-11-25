using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000991 RID: 2449
[SerializationConfig(MemberSerialization.OptIn)]
public class MutantPlant : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06004761 RID: 18273 RVA: 0x00198766 File Offset: 0x00196966
	public List<string> MutationIDs
	{
		get
		{
			return this.mutationIDs;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06004762 RID: 18274 RVA: 0x0019876E File Offset: 0x0019696E
	public bool IsOriginal
	{
		get
		{
			return this.mutationIDs == null || this.mutationIDs.Count == 0;
		}
	}

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06004763 RID: 18275 RVA: 0x00198788 File Offset: 0x00196988
	public bool IsIdentified
	{
		get
		{
			return this.analyzed && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.SubSpeciesID);
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06004764 RID: 18276 RVA: 0x001987A4 File Offset: 0x001969A4
	// (set) Token: 0x06004765 RID: 18277 RVA: 0x001987C7 File Offset: 0x001969C7
	public Tag SpeciesID
	{
		get
		{
			global::Debug.Assert(this.speciesID != null, "Ack, forgot to configure the species ID for this mutantPlant!");
			return this.speciesID;
		}
		set
		{
			this.speciesID = value;
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06004766 RID: 18278 RVA: 0x001987D0 File Offset: 0x001969D0
	public Tag SubSpeciesID
	{
		get
		{
			if (this.cachedSubspeciesID == null)
			{
				this.cachedSubspeciesID = this.GetSubSpeciesInfo().ID;
			}
			return this.cachedSubspeciesID;
		}
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x001987FC File Offset: 0x001969FC
	protected override void OnPrefabInit()
	{
		base.Subscribe<MutantPlant>(-2064133523, MutantPlant.OnAbsorbDelegate);
		base.Subscribe<MutantPlant>(1335436905, MutantPlant.OnSplitFromChunkDelegate);
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x00198820 File Offset: 0x00196A20
	protected override void OnSpawn()
	{
		if (this.IsOriginal || this.HasTag(GameTags.Plant))
		{
			this.analyzed = true;
		}
		if (!this.IsOriginal)
		{
			this.AddTag(GameTags.MutatedSeed);
		}
		this.AddTag(this.SubSpeciesID);
		Components.MutantPlants.Add(this);
		base.OnSpawn();
		this.ApplyMutations();
		this.UpdateNameAndTags();
		PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(this.GetSubSpeciesInfo(), this);
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x00198896 File Offset: 0x00196A96
	protected override void OnCleanUp()
	{
		Components.MutantPlants.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x001988AC File Offset: 0x00196AAC
	private void OnAbsorb(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		global::Debug.Assert(component != null && this.SubSpeciesID == component.SubSpeciesID, "Two seeds of different subspecies just absorbed!");
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x001988EC File Offset: 0x00196AEC
	private void OnSplitFromChunk(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		if (component != null)
		{
			component.CopyMutationsTo(this);
		}
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x00198918 File Offset: 0x00196B18
	public void Mutate()
	{
		List<string> list = (this.mutationIDs != null) ? new List<string>(this.mutationIDs) : new List<string>();
		while (list.Count >= 1 && list.Count > 0)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		list.Add(Db.Get().PlantMutations.GetRandomMutation(this.PrefabID().Name).Id);
		this.SetSubSpecies(list);
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x00198995 File Offset: 0x00196B95
	public void Analyze()
	{
		this.analyzed = true;
		this.UpdateNameAndTags();
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x001989A4 File Offset: 0x00196BA4
	public void ApplyMutations()
	{
		if (this.IsOriginal)
		{
			return;
		}
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).ApplyTo(this);
		}
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x00198A10 File Offset: 0x00196C10
	public void DummySetSubspecies(List<string> mutations)
	{
		this.mutationIDs = mutations;
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x00198A19 File Offset: 0x00196C19
	public void SetSubSpecies(List<string> mutations)
	{
		if (base.gameObject.HasTag(this.SubSpeciesID))
		{
			base.gameObject.RemoveTag(this.SubSpeciesID);
		}
		this.cachedSubspeciesID = Tag.Invalid;
		this.mutationIDs = mutations;
		this.UpdateNameAndTags();
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x00198A57 File Offset: 0x00196C57
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpeciesInfo()
	{
		return new PlantSubSpeciesCatalog.SubSpeciesInfo(this.SpeciesID, this.mutationIDs);
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x00198A6A File Offset: 0x00196C6A
	public void CopyMutationsTo(MutantPlant target)
	{
		target.SetSubSpecies(this.mutationIDs);
		target.analyzed = this.analyzed;
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x00198A84 File Offset: 0x00196C84
	public void UpdateNameAndTags()
	{
		bool flag = !base.IsInitialized() || this.IsIdentified;
		bool flag2 = PlantSubSpeciesCatalog.Instance == null || PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(this.SpeciesID).Count == 1;
		KPrefabID component = base.GetComponent<KPrefabID>();
		component.AddTag(this.SubSpeciesID, false);
		component.SetTag(GameTags.UnidentifiedSeed, !flag);
		base.gameObject.name = component.PrefabTag.ToString() + " (" + this.SubSpeciesID.ToString() + ")";
		base.GetComponent<KSelectable>().SetName(this.GetSubSpeciesInfo().GetNameWithMutations(component.PrefabTag.ProperName(), flag, flag2));
		KSelectable component2 = base.GetComponent<KSelectable>();
		foreach (Guid guid in this.statusItemHandles)
		{
			component2.RemoveStatusItem(guid, false);
		}
		this.statusItemHandles.Clear();
		if (!flag2)
		{
			if (this.IsOriginal)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.OriginalPlantMutation, null));
				return;
			}
			if (!flag)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.UnknownMutation, null));
				return;
			}
			foreach (string id in this.mutationIDs)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.SpecificPlantMutation, Db.Get().PlantMutations.Get(id)));
			}
		}
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x00198C78 File Offset: 0x00196E78
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.IsOriginal)
		{
			return null;
		}
		List<Descriptor> result = new List<Descriptor>();
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).GetDescriptors(ref result, go);
		}
		return result;
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x00198CF0 File Offset: 0x00196EF0
	public List<string> GetSoundEvents()
	{
		List<string> list = new List<string>();
		if (this.mutationIDs != null)
		{
			foreach (string id in this.mutationIDs)
			{
				PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
				list.AddRange(plantMutation.AdditionalSoundEvents);
			}
		}
		return list;
	}

	// Token: 0x04002E9D RID: 11933
	[Serialize]
	private bool analyzed;

	// Token: 0x04002E9E RID: 11934
	[Serialize]
	private List<string> mutationIDs;

	// Token: 0x04002E9F RID: 11935
	private List<Guid> statusItemHandles = new List<Guid>();

	// Token: 0x04002EA0 RID: 11936
	private const int MAX_MUTATIONS = 1;

	// Token: 0x04002EA1 RID: 11937
	[SerializeField]
	private Tag speciesID;

	// Token: 0x04002EA2 RID: 11938
	private Tag cachedSubspeciesID;

	// Token: 0x04002EA3 RID: 11939
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04002EA4 RID: 11940
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnSplitFromChunk(data);
	});
}
