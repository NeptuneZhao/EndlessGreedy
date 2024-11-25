using System;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class AsteroidConfig : IEntityConfig
{
	// Token: 0x06000E06 RID: 3590 RVA: 0x000514F7 File Offset: 0x0004F6F7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00051500 File Offset: 0x0004F700
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Asteroid", "Asteroid", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<WorldInventory>();
		gameObject.AddOrGet<WorldContainer>();
		gameObject.AddOrGet<AsteroidGridEntity>();
		gameObject.AddOrGet<OrbitalMechanics>();
		gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		return gameObject;
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0005154E File Offset: 0x0004F74E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x00051550 File Offset: 0x0004F750
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008D6 RID: 2262
	public const string ID = "Asteroid";
}
