using System;
using UnityEngine;

// Token: 0x02000DFD RID: 3581
[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
	// Token: 0x0600719B RID: 29083 RVA: 0x002AFCC4 File Offset: 0x002ADEC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClick));
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace() && KPlayerPrefs.GetInt(this.saveStatePreferenceKey, 1) != 1 && this.toggle.CurrentState == 0)
		{
			this.OnClick();
		}
	}

	// Token: 0x0600719C RID: 29084 RVA: 0x002AFD30 File Offset: 0x002ADF30
	private void OnClick()
	{
		this.toggle.NextState();
		this.content.SetActive(this.toggle.CurrentState == 0);
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace())
		{
			KPlayerPrefs.SetInt(this.saveStatePreferenceKey, (this.toggle.CurrentState == 0) ? 1 : 0);
		}
	}

	// Token: 0x04004E69 RID: 20073
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x04004E6A RID: 20074
	[SerializeField]
	public GameObject content;

	// Token: 0x04004E6B RID: 20075
	[SerializeField]
	private string saveStatePreferenceKey;

	// Token: 0x04004E6C RID: 20076
	private const int onState = 0;
}
