using System;
using System.Collections.Generic;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x0200064D RID: 1613
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactFinder")]
public class ArtifactFinder : KMonoBehaviour
{
	// Token: 0x06002773 RID: 10099 RVA: 0x000E0A2C File Offset: 0x000DEC2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ArtifactFinder>(-887025858, ArtifactFinder.OnLandDelegate);
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x000E0A48 File Offset: 0x000DEC48
	public ArtifactTier GetArtifactDropTier(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactDropRate artifactDropTable = destination.GetDestinationType().artifactDropTable;
		bool flag = minionID.traitIDs.Contains("Archaeologist");
		if (artifactDropTable != null)
		{
			float num = artifactDropTable.totalWeight;
			if (flag)
			{
				num -= artifactDropTable.GetTierWeight(DECOR.SPACEARTIFACT.TIER_NONE);
			}
			float num2 = UnityEngine.Random.value * num;
			foreach (global::Tuple<ArtifactTier, float> tuple in artifactDropTable.rates)
			{
				if (!flag || (flag && tuple.first != DECOR.SPACEARTIFACT.TIER_NONE))
				{
					num2 -= tuple.second;
				}
				if (num2 <= 0f)
				{
					return tuple.first;
				}
			}
		}
		return DECOR.SPACEARTIFACT.TIER0;
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x000E0B14 File Offset: 0x000DED14
	public List<string> GetArtifactsOfTier(ArtifactTier tier)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<ArtifactType, List<string>> keyValuePair in ArtifactConfig.artifactItems)
		{
			foreach (string text in keyValuePair.Value)
			{
				if (Assets.GetPrefab(text.ToTag()).GetComponent<SpaceArtifact>().GetArtifactTier() == tier)
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x000E0BC4 File Offset: 0x000DEDC4
	public string SearchForArtifact(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactTier artifactDropTier = this.GetArtifactDropTier(minionID, destination);
		if (artifactDropTier == DECOR.SPACEARTIFACT.TIER_NONE)
		{
			return null;
		}
		List<string> artifactsOfTier = this.GetArtifactsOfTier(artifactDropTier);
		return artifactsOfTier[UnityEngine.Random.Range(0, artifactsOfTier.Count)];
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x000E0C00 File Offset: 0x000DEE00
	public void OnLand(object data)
	{
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>()));
		foreach (MinionStorage.Info info in this.minionStorage.GetStoredMinionInfo())
		{
			StoredMinionIdentity minionID = info.serializedMinion.Get<StoredMinionIdentity>();
			string text = this.SearchForArtifact(minionID, spacecraftDestination);
			if (text != null)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(text.ToTag()), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
			}
		}
	}

	// Token: 0x040016C0 RID: 5824
	public const string ID = "ArtifactFinder";

	// Token: 0x040016C1 RID: 5825
	[MyCmpReq]
	private MinionStorage minionStorage;

	// Token: 0x040016C2 RID: 5826
	private static readonly EventSystem.IntraObjectHandler<ArtifactFinder> OnLandDelegate = new EventSystem.IntraObjectHandler<ArtifactFinder>(delegate(ArtifactFinder component, object data)
	{
		component.OnLand(data);
	});
}
