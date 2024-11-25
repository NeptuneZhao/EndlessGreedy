using System;
using Database;
using UnityEngine;

// Token: 0x02000C8E RID: 3214
public class KleiPermitDioramaVis_BuildingPresentationStand : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062C2 RID: 25282 RVA: 0x0024D824 File Offset: 0x0024BA24
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062C3 RID: 25283 RVA: 0x0024D82C File Offset: 0x0024BA2C
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062C4 RID: 25284 RVA: 0x0024D830 File Offset: 0x0024BA30
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.anchorPos, KleiPermitVisUtil.GetBuildingDef(permit), this.lastAlignment);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x060062C5 RID: 25285 RVA: 0x0024D888 File Offset: 0x0024BA88
	public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
	{
		this.lastAlignment = alignment;
		this.anchorPos = new Vector2(alignment.x.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-160f, 160f)), alignment.y.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-156f, 156f)));
		return this;
	}

	// Token: 0x04004300 RID: 17152
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04004301 RID: 17153
	private Alignment lastAlignment;

	// Token: 0x04004302 RID: 17154
	private Vector2 anchorPos;

	// Token: 0x04004303 RID: 17155
	public const float LEFT = -160f;

	// Token: 0x04004304 RID: 17156
	public const float TOP = 156f;
}
