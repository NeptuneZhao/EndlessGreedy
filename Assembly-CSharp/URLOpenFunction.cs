using System;
using UnityEngine;

// Token: 0x02000DE7 RID: 3559
public class URLOpenFunction : MonoBehaviour
{
	// Token: 0x0600710B RID: 28939 RVA: 0x002AC7F2 File Offset: 0x002AA9F2
	private void Start()
	{
		if (this.triggerButton != null)
		{
			this.triggerButton.ClearOnClick();
			this.triggerButton.onClick += delegate()
			{
				this.OpenUrl(this.fixedURL);
			};
		}
	}

	// Token: 0x0600710C RID: 28940 RVA: 0x002AC824 File Offset: 0x002AAA24
	public void OpenUrl(string url)
	{
		if (url == "blueprints")
		{
			if (LockerMenuScreen.Instance != null)
			{
				LockerMenuScreen.Instance.ShowInventoryScreen();
				return;
			}
		}
		else
		{
			App.OpenWebURL(url);
		}
	}

	// Token: 0x0600710D RID: 28941 RVA: 0x002AC851 File Offset: 0x002AAA51
	public void SetURL(string url)
	{
		this.fixedURL = url;
	}

	// Token: 0x04004DB5 RID: 19893
	[SerializeField]
	private KButton triggerButton;

	// Token: 0x04004DB6 RID: 19894
	[SerializeField]
	private string fixedURL;
}
