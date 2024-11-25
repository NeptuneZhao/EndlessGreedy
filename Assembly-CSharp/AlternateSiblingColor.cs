using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BC9 RID: 3017
public class AlternateSiblingColor : KMonoBehaviour
{
	// Token: 0x06005C18 RID: 23576 RVA: 0x0021B9C8 File Offset: 0x00219BC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int siblingIndex = base.transform.GetSiblingIndex();
		this.RefreshColor(siblingIndex % 2 == 0);
	}

	// Token: 0x06005C19 RID: 23577 RVA: 0x0021B9F3 File Offset: 0x00219BF3
	private void RefreshColor(bool evenIndex)
	{
		if (this.image == null)
		{
			return;
		}
		this.image.color = (evenIndex ? this.evenColor : this.oddColor);
	}

	// Token: 0x06005C1A RID: 23578 RVA: 0x0021BA20 File Offset: 0x00219C20
	private void Update()
	{
		if (this.mySiblingIndex != base.transform.GetSiblingIndex())
		{
			this.mySiblingIndex = base.transform.GetSiblingIndex();
			this.RefreshColor(this.mySiblingIndex % 2 == 0);
		}
	}

	// Token: 0x04003D9A RID: 15770
	public Color evenColor;

	// Token: 0x04003D9B RID: 15771
	public Color oddColor;

	// Token: 0x04003D9C RID: 15772
	public Image image;

	// Token: 0x04003D9D RID: 15773
	private int mySiblingIndex;
}
