using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000102 RID: 258
[EntityConfigOrder(2)]
public class BabyDreckoConfig : IEntityConfig
{
	// Token: 0x060004C9 RID: 1225 RVA: 0x00025609 File Offset: 0x00023809
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00025610 File Offset: 0x00023810
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoConfig.CreateDrecko("DreckoBaby", CREATURES.SPECIES.DRECKO.BABY.NAME, CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Drecko", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0002564E File Offset: 0x0002384E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00025650 File Offset: 0x00023850
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400034A RID: 842
	public const string ID = "DreckoBaby";
}
