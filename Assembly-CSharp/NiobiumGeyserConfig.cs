using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class NiobiumGeyserConfig : IEntityConfig
{
	// Token: 0x0600075D RID: 1885 RVA: 0x000310C5 File Offset: 0x0002F2C5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x000310CC File Offset: 0x0002F2CC
	public GameObject CreatePrefab()
	{
		GeyserConfigurator.GeyserType geyserType = new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "");
		GameObject gameObject = GeyserGenericConfig.CreateGeyser("NiobiumGeyser", "geyser_molten_niobium_kanim", 3, 3, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.NAME, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.DESC, geyserType.idHash, geyserType.geyserTemperature);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		return gameObject;
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00031172 File Offset: 0x0002F372
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00031174 File Offset: 0x0002F374
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400053D RID: 1341
	public const string ID = "NiobiumGeyser";
}
