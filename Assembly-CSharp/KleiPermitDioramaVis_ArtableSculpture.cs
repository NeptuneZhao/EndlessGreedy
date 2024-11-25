using System;
using Database;
using UnityEngine;

// Token: 0x02000C88 RID: 3208
public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062AA RID: 25258 RVA: 0x0024D510 File Offset: 0x0024B710
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062AB RID: 25259 RVA: 0x0024D518 File Offset: 0x0024B718
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x060062AC RID: 25260 RVA: 0x0024D52C File Offset: 0x0024B72C
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x040042F8 RID: 17144
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
