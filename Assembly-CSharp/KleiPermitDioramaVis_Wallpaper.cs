using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C93 RID: 3219
public class KleiPermitDioramaVis_Wallpaper : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062DD RID: 25309 RVA: 0x0024DCBA File Offset: 0x0024BEBA
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062DE RID: 25310 RVA: 0x0024DCC2 File Offset: 0x0024BEC2
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x0024DCC4 File Offset: 0x0024BEC4
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemSprite.rectTransform().sizeDelta = Vector2.one * 176f;
		this.itemSprite.sprite = permitPresentationInfo.sprite;
		if (!this.itemSpriteDidInit)
		{
			this.itemSpriteDidInit = true;
			this.itemSpritePosStart = this.itemSprite.rectTransform.anchoredPosition + new Vector2(0f, 16f);
			this.itemSpritePosEnd = this.itemSprite.rectTransform.anchoredPosition;
		}
		this.itemSprite.StartCoroutine(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float alpha)
			{
				this.itemSprite.color = new Color(1f, 1f, 1f, alpha);
			}, 0f, 1f, 0.2f, Easing.SmoothStep, 0.1f),
			Updater.Ease(delegate(Vector2 position)
			{
				this.itemSprite.rectTransform.anchoredPosition = position;
			}, this.itemSpritePosStart, this.itemSpritePosEnd, 0.2f, Easing.SmoothStep, 0.1f)
		}));
	}

	// Token: 0x04004319 RID: 17177
	[SerializeField]
	private Image itemSprite;

	// Token: 0x0400431A RID: 17178
	private bool itemSpriteDidInit;

	// Token: 0x0400431B RID: 17179
	private Vector2 itemSpritePosStart;

	// Token: 0x0400431C RID: 17180
	private Vector2 itemSpritePosEnd;
}
