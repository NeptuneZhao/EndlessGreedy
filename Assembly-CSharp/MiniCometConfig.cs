using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class MiniCometConfig : IEntityConfig
{
	// Token: 0x06000F57 RID: 3927 RVA: 0x000589E2 File Offset: 0x00056BE2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x000589EC File Offset: 0x00056BEC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MiniCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.MINICOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		MiniComet miniComet = gameObject.AddOrGet<MiniComet>();
		Sim.PhysicsData defaultValues = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues;
		miniComet.impactSound = "MeteorDamage_Rock";
		miniComet.flyingSoundID = 2;
		miniComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		gameObject.AddOrGet<PrimaryElement>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.HideFromSpawnTool);
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		return gameObject;
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00058ADA File Offset: 0x00056CDA
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00058ADC File Offset: 0x00056CDC
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000976 RID: 2422
	public static readonly string ID = "MiniComet";

	// Token: 0x04000977 RID: 2423
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000978 RID: 2424
	private const int ADDED_CELLS = 6;
}
