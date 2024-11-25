using System;
using Database;
using UnityEngine;

// Token: 0x02000C8C RID: 3212
public class KleiPermitDioramaVis_BuildingOnFloor : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062BA RID: 25274 RVA: 0x0024D793 File Offset: 0x0024B993
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062BB RID: 25275 RVA: 0x0024D79B File Offset: 0x0024B99B
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x0024D7A0 File Offset: 0x0024B9A0
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x040042FE RID: 17150
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
