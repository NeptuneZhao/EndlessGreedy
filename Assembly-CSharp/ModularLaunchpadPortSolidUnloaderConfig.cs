using System;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class ModularLaunchpadPortSolidUnloaderConfig : IBuildingConfig
{
	// Token: 0x06000FFF RID: 4095 RVA: 0x0005AB6B File Offset: 0x00058D6B
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0005AB72 File Offset: 0x00058D72
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolidUnloader", "conduit_port_solid_unloader_kanim", ConduitType.Solid, false, 2, 3);
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x0005AB87 File Offset: 0x00058D87
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, false);
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x0005AB97 File Offset: 0x00058D97
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x040009B0 RID: 2480
	public const string ID = "ModularLaunchpadPortSolidUnloader";
}
