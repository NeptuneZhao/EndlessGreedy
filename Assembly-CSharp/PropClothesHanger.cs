using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class PropClothesHanger : IEntityConfig
{
	// Token: 0x06001153 RID: 4435 RVA: 0x00061384 File Offset: 0x0005F584
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x0006138C File Offset: 0x0005F58C
	public GameObject CreatePrefab()
	{
		string id = "PropClothesHanger";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("unlock_clothing_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas,
			GameTags.RoomProberBuilding
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Cinnabar, true);
		component.Temperature = 294.15f;
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.dropOnDeconstruct = true;
		gameObject.AddOrGet<Deconstructable>().audioSize = "small";
		return gameObject;
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x00061484 File Offset: 0x0005F684
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"Warm_Vest"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x000614BB File Offset: 0x0005F6BB
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Deconstructable>().SetWorkTime(5f);
	}
}
