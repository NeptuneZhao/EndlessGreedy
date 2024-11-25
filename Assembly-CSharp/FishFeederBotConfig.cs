using System;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class FishFeederBotConfig : IEntityConfig
{
	// Token: 0x06000694 RID: 1684 RVA: 0x0002C7A4 File Offset: 0x0002A9A4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002C7AC File Offset: 0x0002A9AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("FishFeederBot", "FishFeederBot", true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("fishfeeder_kanim")
		};
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		SymbolOverrideControllerUtil.AddToPrefab(kbatchedAnimController.gameObject);
		return gameObject;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0002C804 File Offset: 0x0002AA04
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002C806 File Offset: 0x0002AA06
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004B2 RID: 1202
	public const string ID = "FishFeederBot";
}
