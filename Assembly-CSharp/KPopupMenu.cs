using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C76 RID: 3190
public class KPopupMenu : KScreen
{
	// Token: 0x06006208 RID: 25096 RVA: 0x00249818 File Offset: 0x00247A18
	public void SetOptions(IList<string> options)
	{
		List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
		for (int i = 0; i < options.Count; i++)
		{
			int index = i;
			string option = options[i];
			list.Add(new KButtonMenu.ButtonInfo(option, global::Action.NumActions, delegate()
			{
				this.SelectOption(option, index);
			}, null, null));
		}
		this.Buttons = list.ToArray();
	}

	// Token: 0x06006209 RID: 25097 RVA: 0x00249890 File Offset: 0x00247A90
	public void OnClick()
	{
		if (this.Buttons != null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.buttonMenu.SetButtons(this.Buttons);
			this.buttonMenu.RefreshButtons();
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600620A RID: 25098 RVA: 0x002498E7 File Offset: 0x00247AE7
	public void SelectOption(string option, int index)
	{
		if (this.OnSelect != null)
		{
			this.OnSelect(option, index);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600620B RID: 25099 RVA: 0x0024990A File Offset: 0x00247B0A
	public IList<KButtonMenu.ButtonInfo> GetButtons()
	{
		return this.Buttons;
	}

	// Token: 0x04004279 RID: 17017
	[SerializeField]
	private KButtonMenu buttonMenu;

	// Token: 0x0400427A RID: 17018
	private KButtonMenu.ButtonInfo[] Buttons;

	// Token: 0x0400427B RID: 17019
	public Action<string, int> OnSelect;
}
