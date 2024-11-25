using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class ArtifactPOIConfig : IMultiEntityConfig
{
	// Token: 0x06000E00 RID: 3584 RVA: 0x0005123C File Offset: 0x0004F43C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (ArtifactPOIConfig.ArtifactPOIParams artifactPOIParams in this.GenerateConfigs())
		{
			list.Add(ArtifactPOIConfig.CreateArtifactPOI(artifactPOIParams.id, artifactPOIParams.anim, Strings.Get(artifactPOIParams.nameStringKey), Strings.Get(artifactPOIParams.descStringKey), artifactPOIParams.poiType.idHash));
		}
		return list;
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x000512D4 File Offset: 0x0004F4D4
	public static GameObject CreateArtifactPOI(string id, string anim, string name, string desc, HashedString poiType)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = poiType;
		ArtifactPOIClusterGridEntity artifactPOIClusterGridEntity = gameObject.AddOrGet<ArtifactPOIClusterGridEntity>();
		artifactPOIClusterGridEntity.m_name = name;
		artifactPOIClusterGridEntity.m_Anim = anim;
		gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextSpaceEntry));
		return gameObject;
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00051329 File Offset: 0x0004F529
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0005132B File Offset: 0x0004F52B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00051330 File Offset: 0x0004F530
	private List<ArtifactPOIConfig.ArtifactPOIParams> GenerateConfigs()
	{
		List<ArtifactPOIConfig.ArtifactPOIParams> list = new List<ArtifactPOIConfig.ArtifactPOIParams>();
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_1", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation1", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_2", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation2", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_3", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation3", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_4", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation4", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_5", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation5", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_6", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation6", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_7", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation7", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_8", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation8", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("russels_teapot", new ArtifactPOIConfigurator.ArtifactPOIType("RussellsTeapot", "artifact_TeaPot", true, 30000f, 60000f, "EXPANSION1_ID")));
		list.RemoveAll((ArtifactPOIConfig.ArtifactPOIParams poi) => !poi.poiType.dlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentSubscribed(poi.poiType.dlcID));
		return list;
	}

	// Token: 0x040008CD RID: 2253
	public const string GravitasSpaceStation1 = "GravitasSpaceStation1";

	// Token: 0x040008CE RID: 2254
	public const string GravitasSpaceStation2 = "GravitasSpaceStation2";

	// Token: 0x040008CF RID: 2255
	public const string GravitasSpaceStation3 = "GravitasSpaceStation3";

	// Token: 0x040008D0 RID: 2256
	public const string GravitasSpaceStation4 = "GravitasSpaceStation4";

	// Token: 0x040008D1 RID: 2257
	public const string GravitasSpaceStation5 = "GravitasSpaceStation5";

	// Token: 0x040008D2 RID: 2258
	public const string GravitasSpaceStation6 = "GravitasSpaceStation6";

	// Token: 0x040008D3 RID: 2259
	public const string GravitasSpaceStation7 = "GravitasSpaceStation7";

	// Token: 0x040008D4 RID: 2260
	public const string GravitasSpaceStation8 = "GravitasSpaceStation8";

	// Token: 0x040008D5 RID: 2261
	public const string RussellsTeapot = "RussellsTeapot";

	// Token: 0x02001106 RID: 4358
	public struct ArtifactPOIParams
	{
		// Token: 0x06007DFD RID: 32253 RVA: 0x00309928 File Offset: 0x00307B28
		public ArtifactPOIParams(string anim, ArtifactPOIConfigurator.ArtifactPOIType poiType)
		{
			this.id = "ArtifactSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x04005EA4 RID: 24228
		public string id;

		// Token: 0x04005EA5 RID: 24229
		public string anim;

		// Token: 0x04005EA6 RID: 24230
		public StringKey nameStringKey;

		// Token: 0x04005EA7 RID: 24231
		public StringKey descStringKey;

		// Token: 0x04005EA8 RID: 24232
		public ArtifactPOIConfigurator.ArtifactPOIType poiType;
	}
}
