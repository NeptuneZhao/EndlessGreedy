using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class ProExoSetLockerConfig : IEntityConfig
{
	// Token: 0x06001352 RID: 4946 RVA: 0x0006B08D File Offset: 0x0006928D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x0006B094 File Offset: 0x00069294
	public GameObject CreatePrefab()
	{
		string id = "PropExoSetLocker";
		string name = STRINGS.BUILDINGS.PREFABS.PROPEXOSETLOCKER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPEXOSETLOCKER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("zipper_locker_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_zipper_locker_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.numDataBanks = new int[]
		{
			1,
			4
		};
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x0006B188 File Offset: 0x00069388
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"Warm_Vest"
			},
			new string[]
			{
				"Funky_Vest"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x0006B1D0 File Offset: 0x000693D0
	public void OnSpawn(GameObject inst)
	{
	}
}
