using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class StickerBombConfig : IEntityConfig
{
	// Token: 0x06001487 RID: 5255 RVA: 0x000709D6 File Offset: 0x0006EBD6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x000709E0 File Offset: 0x0006EBE0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("StickerBomb", STRINGS.BUILDINGS.PREFABS.STICKERBOMB.NAME, STRINGS.BUILDINGS.PREFABS.STICKERBOMB.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "off", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
		gameObject.AddOrGet<StickerBomb>();
		return gameObject;
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x00070A4A File Offset: 0x0006EC4A
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER2);
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x00070A74 File Offset: 0x0006EC74
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000BB8 RID: 3000
	public const string ID = "StickerBomb";
}
