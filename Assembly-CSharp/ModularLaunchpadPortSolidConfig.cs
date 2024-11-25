using System;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public class ModularLaunchpadPortSolidConfig : IBuildingConfig
{
	// Token: 0x06000FFA RID: 4090 RVA: 0x0005AB2E File Offset: 0x00058D2E
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0005AB35 File Offset: 0x00058D35
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolid", "conduit_port_solid_loader_kanim", ConduitType.Solid, true, 2, 2);
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0005AB4A File Offset: 0x00058D4A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, true);
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0005AB5A File Offset: 0x00058D5A
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x040009AF RID: 2479
	public const string ID = "ModularLaunchpadPortSolid";
}
