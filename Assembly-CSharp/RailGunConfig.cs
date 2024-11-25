using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class RailGunConfig : IBuildingConfig
{
	// Token: 0x06001253 RID: 4691 RVA: 0x000646ED File Offset: 0x000628ED
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x000646F4 File Offset: 0x000628F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RailGun";
		int width = 5;
		int height = 6;
		string anim = "rail_gun_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.DefaultAnimState = "off";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(-2, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RailGun.PORT_ID, new CellOffset(-2, 2), STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(2, 0), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0006484C File Offset: 0x00062A4C
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x00064884 File Offset: 0x00062A84
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		RailGun railGun = go.AddOrGet<RailGun>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		ClusterDestinationSelector clusterDestinationSelector = go.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		railGun.liquidPortInfo = this.liquidInputPort;
		railGun.gasPortInfo = this.gasInputPort;
		railGun.solidPortInfo = this.solidInputPort;
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.capacity = 200f;
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.showInUI = false;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00064918 File Offset: 0x00062B18
	public override void DoPostConfigureComplete(GameObject go)
	{
		List<Tag> list = new List<Tag>();
		list.AddRange(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD);
		list.AddRange(STORAGEFILTERS.GASES);
		list.AddRange(STORAGEFILTERS.FOOD);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.storageFilters = list;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.capacityKg = 1200f;
		go.GetComponent<HighEnergyParticlePort>().requireOperational = false;
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x00064996 File Offset: 0x00062B96
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		this.AttachPorts(go);
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x000649A5 File Offset: 0x00062BA5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AttachPorts(go);
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x000649B4 File Offset: 0x00062BB4
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = -2;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.AllOrNothingVisibility = true;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(RailGunConfig.RailGunSkyVisibility);
		};
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x00064A08 File Offset: 0x00062C08
	private static bool RailGunSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "RailGun assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000A99 RID: 2713
	public const string ID = "RailGun";

	// Token: 0x04000A9A RID: 2714
	public const string PORT_ID = "HEP_STORAGE";

	// Token: 0x04000A9B RID: 2715
	public const int RANGE = 20;

	// Token: 0x04000A9C RID: 2716
	public const float BASE_PARTICLE_COST = 0f;

	// Token: 0x04000A9D RID: 2717
	public const float HEX_PARTICLE_COST = 10f;

	// Token: 0x04000A9E RID: 2718
	public const float HEP_CAPACITY = 200f;

	// Token: 0x04000A9F RID: 2719
	public const float TAKEOFF_VELOCITY = 35f;

	// Token: 0x04000AA0 RID: 2720
	public const int MAINTENANCE_AFTER_NUM_PAYLOADS = 6;

	// Token: 0x04000AA1 RID: 2721
	public const int MAINTENANCE_COOLDOWN = 30;

	// Token: 0x04000AA2 RID: 2722
	public const float CAPACITY = 1200f;

	// Token: 0x04000AA3 RID: 2723
	private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(-1, 0));

	// Token: 0x04000AA4 RID: 2724
	private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

	// Token: 0x04000AA5 RID: 2725
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));
}
