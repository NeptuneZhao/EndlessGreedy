using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035B RID: 859
public class PropGravitasFirstAidKitConfig : IEntityConfig
{
	// Token: 0x060011D5 RID: 4565 RVA: 0x00062C6C File Offset: 0x00060E6C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00062C74 File Offset: 0x00060E74
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFirstAidKit";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_first_aid_kit_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x00062D38 File Offset: 0x00060F38
	public static string[][] GetLockerBaseContents()
	{
		string text = DlcManager.FeatureRadiationEnabled() ? "BasicRadPill" : "IntermediateCure";
		return new string[][]
		{
			new string[]
			{
				"BasicCure",
				"BasicCure",
				"BasicCure"
			},
			new string[]
			{
				text,
				text
			}
		};
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x00062D91 File Offset: 0x00060F91
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropGravitasFirstAidKitConfig.GetLockerBaseContents();
		component.ChooseContents();
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x00062DBE File Offset: 0x00060FBE
	public void OnSpawn(GameObject inst)
	{
	}
}
