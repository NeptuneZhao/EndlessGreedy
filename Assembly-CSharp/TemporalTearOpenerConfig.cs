using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class TemporalTearOpenerConfig : IBuildingConfig
{
	// Token: 0x060014DD RID: 5341 RVA: 0x000731B2 File Offset: 0x000713B2
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x000731BC File Offset: 0x000713BC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TemporalTearOpener";
		int width = 3;
		int height = 4;
		string anim = "temporal_tear_opener_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.DefaultAnimState = "off";
		buildingDef.Entombable = false;
		buildingDef.Invincible = true;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x00073280 File Offset: 0x00071480
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.capacity = 1000f;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
		TemporalTearOpener.Def def = go.AddOrGetDef<TemporalTearOpener.Def>();
		def.numParticlesToOpen = 10000f;
		def.consumeRate = 5f;
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x00073302 File Offset: 0x00071502
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x04000BE5 RID: 3045
	public const string ID = "TemporalTearOpener";

	// Token: 0x04000BE6 RID: 3046
	public const string PORT_ID = "HEP_STORAGE";

	// Token: 0x04000BE7 RID: 3047
	public const float PARTICLES_CAPACITY = 1000f;

	// Token: 0x04000BE8 RID: 3048
	public const float NUM_PARTICLES_TO_OPEN_TEAR = 10000f;

	// Token: 0x04000BE9 RID: 3049
	public const float PARTICLE_CONSUME_RATE = 5f;
}
