using System;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class ModularLaunchpadPortLiquidConfig : IBuildingConfig
{
	// Token: 0x06000FF0 RID: 4080 RVA: 0x0005AAB4 File Offset: 0x00058CB4
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0005AABB File Offset: 0x00058CBB
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquid", "conduit_port_liquid_loader_kanim", ConduitType.Liquid, true, 2, 2);
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0005AAD0 File Offset: 0x00058CD0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, true);
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0005AAE0 File Offset: 0x00058CE0
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x040009AD RID: 2477
	public const string ID = "ModularLaunchpadPortLiquid";
}
