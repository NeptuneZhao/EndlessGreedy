using System;
using Database;
using UnityEngine;

// Token: 0x02000C8D RID: 3213
public class KleiPermitDioramaVis_BuildingOnFloorBig : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062BE RID: 25278 RVA: 0x0024D7DC File Offset: 0x0024B9DC
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062BF RID: 25279 RVA: 0x0024D7E4 File Offset: 0x0024B9E4
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062C0 RID: 25280 RVA: 0x0024D7E8 File Offset: 0x0024B9E8
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x040042FF RID: 17151
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
