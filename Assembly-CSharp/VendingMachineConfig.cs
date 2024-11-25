using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class VendingMachineConfig : IEntityConfig
{
	// Token: 0x0600137A RID: 4986 RVA: 0x0006B8BE File Offset: 0x00069ABE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x0006B8C8 File Offset: 0x00069AC8
	public GameObject CreatePrefab()
	{
		string id = "VendingMachine";
		string name = STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("vendingmachine_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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
		setLocker.machineSound = "VendingMachine_LP";
		setLocker.overrideAnim = "anim_break_kanim";
		setLocker.dropOffset = new Vector2I(1, 1);
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x0006B9BC File Offset: 0x00069BBC
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"FieldRation"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x0006B9F3 File Offset: 0x00069BF3
	public void OnSpawn(GameObject inst)
	{
	}
}
