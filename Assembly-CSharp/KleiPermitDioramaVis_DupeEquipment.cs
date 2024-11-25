using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C90 RID: 3216
public class KleiPermitDioramaVis_DupeEquipment : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062CB RID: 25291 RVA: 0x0024D948 File Offset: 0x0024BB48
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062CC RID: 25292 RVA: 0x0024D950 File Offset: 0x0024BB50
	public void ConfigureSetup()
	{
		this.uiMannequin.shouldShowOutfitWithDefaultItems = false;
	}

	// Token: 0x060062CD RID: 25293 RVA: 0x0024D960 File Offset: 0x0024BB60
	public void ConfigureWith(PermitResource permit)
	{
		ClothingItemResource clothingItemResource = permit as ClothingItemResource;
		if (clothingItemResource != null)
		{
			this.uiMannequin.SetOutfit(clothingItemResource.outfitType, new ClothingItemResource[]
			{
				clothingItemResource
			});
			this.uiMannequin.ReactToClothingItemChange(clothingItemResource.Category);
		}
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(permit.Category);
	}

	// Token: 0x04004306 RID: 17158
	[SerializeField]
	private UIMannequin uiMannequin;

	// Token: 0x04004307 RID: 17159
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04004308 RID: 17160
	[SerializeField]
	private Sprite clothingBG;

	// Token: 0x04004309 RID: 17161
	[SerializeField]
	private Sprite atmosuitBG;
}
