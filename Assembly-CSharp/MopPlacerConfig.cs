using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class MopPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x0600110C RID: 4364 RVA: 0x0006018C File Offset: 0x0005E38C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MopPlacerConfig.ID, MISC.PLACERS.MOPPLACER.NAME, Assets.instance.mopPlacerAssets.material);
		gameObject.AddTag(GameTags.NotConversationTopic);
		Moppable moppable = gameObject.AddOrGet<Moppable>();
		moppable.synchronizeAnims = false;
		moppable.amountMoppedPerTick = 20f;
		gameObject.AddOrGet<Cancellable>();
		return gameObject;
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x000601E6 File Offset: 0x0005E3E6
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000601E8 File Offset: 0x0005E3E8
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A6A RID: 2666
	public static string ID = "MopPlacer";

	// Token: 0x02001135 RID: 4405
	[Serializable]
	public class MopPlacerAssets
	{
		// Token: 0x04005F85 RID: 24453
		public Material material;
	}
}
