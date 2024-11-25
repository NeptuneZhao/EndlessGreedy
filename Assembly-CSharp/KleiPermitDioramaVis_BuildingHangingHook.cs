using System;
using Database;
using UnityEngine;

// Token: 0x02000C8A RID: 3210
public class KleiPermitDioramaVis_BuildingHangingHook : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062B2 RID: 25266 RVA: 0x0024D5AC File Offset: 0x0024B7AC
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062B3 RID: 25267 RVA: 0x0024D5B4 File Offset: 0x0024B7B4
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062B4 RID: 25268 RVA: 0x0024D5B8 File Offset: 0x0024B7B8
	public void ConfigureWith(PermitResource permit)
	{
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource)permit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit), Alignment.Top());
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x040042FA RID: 17146
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x040042FB RID: 17147
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
