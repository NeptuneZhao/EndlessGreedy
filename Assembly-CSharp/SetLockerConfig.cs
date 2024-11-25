using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class SetLockerConfig : IEntityConfig
{
	// Token: 0x06001375 RID: 4981 RVA: 0x0006B76C File Offset: 0x0006996C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0006B774 File Offset: 0x00069974
	public GameObject CreatePrefab()
	{
		string id = "SetLocker";
		string name = STRINGS.BUILDINGS.PREFABS.SETLOCKER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.SETLOCKER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_locker_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
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

	// Token: 0x06001377 RID: 4983 RVA: 0x0006B86C File Offset: 0x00069A6C
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

	// Token: 0x06001378 RID: 4984 RVA: 0x0006B8B4 File Offset: 0x00069AB4
	public void OnSpawn(GameObject inst)
	{
	}
}
