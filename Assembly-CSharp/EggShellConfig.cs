using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000222 RID: 546
public class EggShellConfig : IEntityConfig
{
	// Token: 0x06000B48 RID: 2888 RVA: 0x00043029 File Offset: 0x00041229
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x00043030 File Offset: 0x00041230
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EggShell", ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.DESC, 1f, false, Assets.GetAnim("eggshells_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x000430B0 File Offset: 0x000412B0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x000430B2 File Offset: 0x000412B2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000780 RID: 1920
	public const string ID = "EggShell";

	// Token: 0x04000781 RID: 1921
	public static readonly Tag TAG = TagManager.Create("EggShell");

	// Token: 0x04000782 RID: 1922
	public const float EGG_TO_SHELL_RATIO = 0.5f;
}
