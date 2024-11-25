using System;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class ModularLaunchpadPortLiquidUnloaderConfig : IBuildingConfig
{
	// Token: 0x06000FF5 RID: 4085 RVA: 0x0005AAF1 File Offset: 0x00058CF1
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0005AAF8 File Offset: 0x00058CF8
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquidUnloader", "conduit_port_liquid_unloader_kanim", ConduitType.Liquid, false, 2, 3);
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0005AB0D File Offset: 0x00058D0D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, false);
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0005AB1D File Offset: 0x00058D1D
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x040009AE RID: 2478
	public const string ID = "ModularLaunchpadPortLiquidUnloader";
}
