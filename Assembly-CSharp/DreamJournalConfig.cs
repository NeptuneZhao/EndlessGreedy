using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class DreamJournalConfig : IEntityConfig
{
	// Token: 0x06000B42 RID: 2882 RVA: 0x00042F83 File Offset: 0x00041183
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00042F8A File Offset: 0x0004118A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00042F8C File Offset: 0x0004118C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00042F90 File Offset: 0x00041190
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dream_journal_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DreamJournalConfig.ID.Name, ITEMS.DREAMJOURNAL.NAME, ITEMS.DREAMJOURNAL.DESC, 1f, true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.StoryTraitResource
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 25f;
		return gameObject;
	}

	// Token: 0x0400077A RID: 1914
	public static Tag ID = new Tag("DreamJournal");

	// Token: 0x0400077B RID: 1915
	public const float MASS = 1f;

	// Token: 0x0400077C RID: 1916
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x0400077D RID: 1917
	private const string ANIM_FILE = "dream_journal_kanim";

	// Token: 0x0400077E RID: 1918
	private const string INITIAL_ANIM = "object";

	// Token: 0x0400077F RID: 1919
	public const int MAX_STACK_SIZE = 25;
}
