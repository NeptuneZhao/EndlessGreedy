using System;
using KSerialization;
using UnityEngine;

// Token: 0x020009E5 RID: 2533
public class PedestalArtifactSpawner : KMonoBehaviour
{
	// Token: 0x06004979 RID: 18809 RVA: 0x001A4C78 File Offset: 0x001A2E78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject gameObject in this.storage.items)
		{
			if (ArtifactSelector.Instance.GetArtifactType(gameObject.name) == ArtifactType.Terrestrial)
			{
				gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
			}
		}
		if (this.artifactSpawned)
		{
			return;
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Terrestrial)), base.transform.position);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
		this.storage.Store(gameObject2, false, false, true, false);
		this.receptacle.ForceDeposit(gameObject2);
		this.artifactSpawned = true;
	}

	// Token: 0x0400300E RID: 12302
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400300F RID: 12303
	[MyCmpReq]
	private SingleEntityReceptacle receptacle;

	// Token: 0x04003010 RID: 12304
	[Serialize]
	private bool artifactSpawned;
}
