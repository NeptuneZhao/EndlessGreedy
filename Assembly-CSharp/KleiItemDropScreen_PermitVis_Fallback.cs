using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C80 RID: 3200
public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
	// Token: 0x0600627B RID: 25211 RVA: 0x0024C351 File Offset: 0x0024A551
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.sprite.sprite = info.Sprite;
	}

	// Token: 0x040042D7 RID: 17111
	[SerializeField]
	private Image sprite;
}
