using System;
using Database;
using UnityEngine;

// Token: 0x02000C8B RID: 3211
public class KleiPermitDioramaVis_BuildingOnBackground : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062B6 RID: 25270 RVA: 0x0024D620 File Offset: 0x0024B820
	public void ConfigureSetup()
	{
		this.buildingKAnimPrefab.gameObject.SetActive(false);
		this.buildingKAnimArray = new KBatchedAnimController[9];
		for (int i = 0; i < this.buildingKAnimArray.Length; i++)
		{
			this.buildingKAnimArray[i] = (KBatchedAnimController)UnityEngine.Object.Instantiate(this.buildingKAnimPrefab, this.buildingKAnimPrefab.transform.parent, false);
		}
		Vector2 anchoredPosition = this.buildingKAnimPrefab.rectTransform().anchoredPosition;
		Vector2 a = 175f * Vector2.one;
		Vector2 a2 = anchoredPosition + a * new Vector2(-1f, 0f);
		int num = 0;
		for (int j = 0; j < 3; j++)
		{
			int k = 0;
			while (k < 3)
			{
				this.buildingKAnimArray[num].rectTransform().anchoredPosition = a2 + a * new Vector2((float)j, (float)k);
				this.buildingKAnimArray[num].gameObject.SetActive(true);
				k++;
				num++;
			}
		}
	}

	// Token: 0x060062B7 RID: 25271 RVA: 0x0024D724 File Offset: 0x0024B924
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062B8 RID: 25272 RVA: 0x0024D72C File Offset: 0x0024B92C
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		DebugUtil.DevAssert(buildingDef.WidthInCells == 1, "assert failed", null);
		DebugUtil.DevAssert(buildingDef.HeightInCells == 1, "assert failed", null);
		KBatchedAnimController[] array = this.buildingKAnimArray;
		for (int i = 0; i < array.Length; i++)
		{
			KleiPermitVisUtil.ConfigureToRenderBuilding(array[i], buildingPermit);
		}
	}

	// Token: 0x040042FC RID: 17148
	[SerializeField]
	private KBatchedAnimController buildingKAnimPrefab;

	// Token: 0x040042FD RID: 17149
	private KBatchedAnimController[] buildingKAnimArray;
}
