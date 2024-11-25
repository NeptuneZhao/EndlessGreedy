using System;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class ModularLaunchpadPortGasConfig : IBuildingConfig
{
	// Token: 0x06000FE6 RID: 4070 RVA: 0x0005AA3A File Offset: 0x00058C3A
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x0005AA41 File Offset: 0x00058C41
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGas", "conduit_port_gas_loader_kanim", ConduitType.Gas, true, 2, 2);
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x0005AA56 File Offset: 0x00058C56
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, true);
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0005AA66 File Offset: 0x00058C66
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x040009AB RID: 2475
	public const string ID = "ModularLaunchpadPortGas";
}
