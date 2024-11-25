using System;
using Database;
using UnityEngine;

// Token: 0x02000C8F RID: 3215
public class KleiPermitDioramaVis_BuildingRocket : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062C7 RID: 25287 RVA: 0x0024D902 File Offset: 0x0024BB02
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062C8 RID: 25288 RVA: 0x0024D90A File Offset: 0x0024BB0A
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062C9 RID: 25289 RVA: 0x0024D90C File Offset: 0x0024BB0C
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04004305 RID: 17157
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
