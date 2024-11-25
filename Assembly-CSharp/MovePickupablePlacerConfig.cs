using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class MovePickupablePlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06001111 RID: 4369 RVA: 0x00060200 File Offset: 0x0005E400
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MovePickupablePlacerConfig.ID, MISC.PLACERS.MOVEPICKUPABLEPLACER.NAME, Assets.instance.movePickupToPlacerAssets.material);
		gameObject.AddOrGet<CancellableMove>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.showUnreachableStatus = true;
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00060264 File Offset: 0x0005E464
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x00060266 File Offset: 0x0005E466
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A6B RID: 2667
	public static string ID = "MovePickupablePlacer";

	// Token: 0x02001136 RID: 4406
	[Serializable]
	public class MovePickupablePlacerAssets
	{
		// Token: 0x04005F86 RID: 24454
		public Material material;
	}
}
