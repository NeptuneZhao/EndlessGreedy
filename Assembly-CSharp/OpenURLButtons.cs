using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// Token: 0x02000CFD RID: 3325
[AddComponentMenu("KMonoBehaviour/scripts/OpenURLButtons")]
public class OpenURLButtons : KMonoBehaviour
{
	// Token: 0x06006724 RID: 26404 RVA: 0x002680B4 File Offset: 0x002662B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		for (int i = 0; i < this.buttonData.Count; i++)
		{
			OpenURLButtons.URLButtonData data = this.buttonData[i];
			GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, base.gameObject, true);
			string text = Strings.Get(data.stringKey);
			gameObject.GetComponentInChildren<LocText>().SetText(text);
			switch (data.urlType)
			{
			case OpenURLButtons.URLButtonType.url:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.platformUrl:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPlatformURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.patchNotes:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPatchNotes();
				};
				break;
			case OpenURLButtons.URLButtonType.feedbackScreen:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenFeedbackScreen();
				};
				break;
			}
		}
	}

	// Token: 0x06006725 RID: 26405 RVA: 0x002681BF File Offset: 0x002663BF
	public void OpenPatchNotes()
	{
		Util.KInstantiateUI(this.patchNotesScreenPrefab, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x06006726 RID: 26406 RVA: 0x002681D8 File Offset: 0x002663D8
	public void OpenFeedbackScreen()
	{
		Util.KInstantiateUI(this.feedbackScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x06006727 RID: 26407 RVA: 0x002681F6 File Offset: 0x002663F6
	public void OpenURL(string URL)
	{
		App.OpenWebURL(URL);
	}

	// Token: 0x06006728 RID: 26408 RVA: 0x00268200 File Offset: 0x00266400
	public void OpenPlatformURL(string URL)
	{
		if (DistributionPlatform.Inst.Platform == "Steam" && DistributionPlatform.Inst.Initialized)
		{
			DistributionPlatform.Inst.GetAuthTicket(delegate(byte[] ticket)
			{
				string newValue = string.Concat(Array.ConvertAll<byte, string>(ticket, (byte x) => x.ToString("X2")));
				App.OpenWebURL(URL.Replace("{SteamID}", DistributionPlatform.Inst.LocalUser.Id.ToInt64().ToString()).Replace("{SteamTicket}", newValue));
			});
			return;
		}
		string value = URL.Replace("{SteamID}", "").Replace("{SteamTicket}", "");
		App.OpenWebURL("https://accounts.klei.com/login?goto={gotoUrl}".Replace("{gotoUrl}", WebUtility.HtmlEncode(value)));
	}

	// Token: 0x04004597 RID: 17815
	public GameObject buttonPrefab;

	// Token: 0x04004598 RID: 17816
	public List<OpenURLButtons.URLButtonData> buttonData;

	// Token: 0x04004599 RID: 17817
	[SerializeField]
	private GameObject patchNotesScreenPrefab;

	// Token: 0x0400459A RID: 17818
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x02001E03 RID: 7683
	public enum URLButtonType
	{
		// Token: 0x040088FE RID: 35070
		url,
		// Token: 0x040088FF RID: 35071
		platformUrl,
		// Token: 0x04008900 RID: 35072
		patchNotes,
		// Token: 0x04008901 RID: 35073
		feedbackScreen
	}

	// Token: 0x02001E04 RID: 7684
	[Serializable]
	public class URLButtonData
	{
		// Token: 0x04008902 RID: 35074
		public string stringKey;

		// Token: 0x04008903 RID: 35075
		public OpenURLButtons.URLButtonType urlType;

		// Token: 0x04008904 RID: 35076
		public string url;
	}
}
