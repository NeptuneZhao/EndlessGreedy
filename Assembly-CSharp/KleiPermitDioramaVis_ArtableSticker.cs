using System;
using Database;
using UnityEngine;

// Token: 0x02000C89 RID: 3209
public class KleiPermitDioramaVis_ArtableSticker : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062AE RID: 25262 RVA: 0x0024D568 File Offset: 0x0024B768
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062AF RID: 25263 RVA: 0x0024D570 File Offset: 0x0024B770
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x060062B0 RID: 25264 RVA: 0x0024D584 File Offset: 0x0024B784
	public void ConfigureWith(PermitResource permit)
	{
		DbStickerBomb artablePermit = (DbStickerBomb)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
	}

	// Token: 0x040042F9 RID: 17145
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
