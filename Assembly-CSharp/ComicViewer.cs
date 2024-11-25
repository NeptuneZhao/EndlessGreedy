using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1A RID: 3098
public class ComicViewer : KScreen
{
	// Token: 0x06005F0F RID: 24335 RVA: 0x00235374 File Offset: 0x00233574
	public void ShowComic(ComicData comic, bool isVictoryComic)
	{
		for (int i = 0; i < Mathf.Max(comic.images.Length, comic.stringKeys.Length); i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.panelPrefab, this.contentContainer, true);
			this.activePanels.Add(gameObject);
			gameObject.GetComponentInChildren<Image>().sprite = comic.images[i];
			gameObject.GetComponentInChildren<LocText>().SetText(comic.stringKeys[i]);
		}
		this.closeButton.ClearOnClick();
		if (isVictoryComic)
		{
			this.closeButton.onClick += delegate()
			{
				this.Stop();
				this.Show(false);
			};
			return;
		}
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
	}

	// Token: 0x06005F10 RID: 24336 RVA: 0x00235423 File Offset: 0x00233623
	public void Stop()
	{
		this.OnStop();
		this.Show(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04003FE0 RID: 16352
	public GameObject panelPrefab;

	// Token: 0x04003FE1 RID: 16353
	public GameObject contentContainer;

	// Token: 0x04003FE2 RID: 16354
	public List<GameObject> activePanels = new List<GameObject>();

	// Token: 0x04003FE3 RID: 16355
	public KButton closeButton;

	// Token: 0x04003FE4 RID: 16356
	public System.Action OnStop;
}
