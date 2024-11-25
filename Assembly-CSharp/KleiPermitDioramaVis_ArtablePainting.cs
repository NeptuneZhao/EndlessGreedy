using System;
using Database;
using UnityEngine;

// Token: 0x02000C87 RID: 3207
public class KleiPermitDioramaVis_ArtablePainting : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062A6 RID: 25254 RVA: 0x0024D436 File Offset: 0x0024B636
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062A7 RID: 25255 RVA: 0x0024D43E File Offset: 0x0024B63E
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x060062A8 RID: 25256 RVA: 0x0024D454 File Offset: 0x0024B654
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f * (float)buildingDef.HeightInCells / 2f + 176f);
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.9f;
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x040042F6 RID: 17142
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x040042F7 RID: 17143
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
