using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C6A RID: 3178
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenSpriteItem")]
public class InfoScreenSpriteItem : KMonoBehaviour
{
	// Token: 0x06006176 RID: 24950 RVA: 0x002446B8 File Offset: 0x002428B8
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
		float num = sprite.rect.width / sprite.rect.height;
		this.layout.preferredWidth = this.layout.preferredHeight * num;
	}

	// Token: 0x04004211 RID: 16913
	[SerializeField]
	private Image image;

	// Token: 0x04004212 RID: 16914
	[SerializeField]
	private LayoutElement layout;
}
