using System;
using UnityEngine;

// Token: 0x02000DF3 RID: 3571
[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
	// Token: 0x0600715D RID: 29021 RVA: 0x002AE048 File Offset: 0x002AC248
	protected override void OnPrefabInit()
	{
		this.button.onClick += this.OnClick;
	}

	// Token: 0x0600715E RID: 29022 RVA: 0x002AE061 File Offset: 0x002AC261
	private void OnClick()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.GetText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x04004E0A RID: 19978
	public KButton button;

	// Token: 0x04004E0B RID: 19979
	public Func<string> GetText;
}
