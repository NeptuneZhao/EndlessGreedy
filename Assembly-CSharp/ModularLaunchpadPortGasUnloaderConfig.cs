using System;
using UnityEngine;

// Token: 0x020002F6 RID: 758
public class ModularLaunchpadPortGasUnloaderConfig : IBuildingConfig
{
	// Token: 0x06000FEB RID: 4075 RVA: 0x0005AA77 File Offset: 0x00058C77
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x0005AA7E File Offset: 0x00058C7E
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGasUnloader", "conduit_port_gas_unloader_kanim", ConduitType.Gas, false, 2, 3);
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x0005AA93 File Offset: 0x00058C93
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, false);
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0005AAA3 File Offset: 0x00058CA3
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x040009AC RID: 2476
	public const string ID = "ModularLaunchpadPortGasUnloader";
}
