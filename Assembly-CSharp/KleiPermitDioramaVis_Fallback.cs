using System;
using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C91 RID: 3217
public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060062CF RID: 25295 RVA: 0x0024D9C1 File Offset: 0x0024BBC1
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x0024D9C9 File Offset: 0x0024BBC9
	public void ConfigureSetup()
	{
	}

	// Token: 0x060062D1 RID: 25297 RVA: 0x0024D9CB File Offset: 0x0024BBCB
	public void ConfigureWith(PermitResource permit)
	{
		this.sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
		this.editorOnlyErrorMessageParent.gameObject.SetActive(false);
	}

	// Token: 0x060062D2 RID: 25298 RVA: 0x0024D9EE File Offset: 0x0024BBEE
	public KleiPermitDioramaVis_Fallback WithError(string error)
	{
		this.error = error;
		global::Debug.Log("[KleiInventoryScreen Error] Had to use fallback vis. " + error);
		return this;
	}

	// Token: 0x0400430A RID: 17162
	[SerializeField]
	private Image sprite;

	// Token: 0x0400430B RID: 17163
	[SerializeField]
	private RectTransform editorOnlyErrorMessageParent;

	// Token: 0x0400430C RID: 17164
	[SerializeField]
	private TextMeshProUGUI editorOnlyErrorMessageText;

	// Token: 0x0400430D RID: 17165
	private Option<string> error;
}
