using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D2F RID: 3375
public class ScenariosMenu : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x06006A1C RID: 27164 RVA: 0x0027FBA8 File Offset: 0x0027DDA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.RebuildScreen();
	}

	// Token: 0x06006A1D RID: 27165 RVA: 0x0027FC2C File Offset: 0x0027DE2C
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.RebuildUGCButtons();
	}

	// Token: 0x06006A1E RID: 27166 RVA: 0x0027FC90 File Offset: 0x0027DE90
	private void RebuildUGCButtons()
	{
		ListPool<SteamUGCService.Mod, ScenariosMenu>.PooledList pooledList = ListPool<SteamUGCService.Mod, ScenariosMenu>.Allocate();
		bool flag = pooledList.Count > 0;
		this.noScenariosText.gameObject.SetActive(!flag);
		this.contentRoot.gameObject.SetActive(flag);
		bool flag2 = true;
		if (pooledList.Count != 0)
		{
			for (int i = 0; i < pooledList.Count; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.ugcButtonPrefab, this.ugcContainer, false);
				gameObject.name = pooledList[i].title + "_button";
				gameObject.gameObject.SetActive(true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Title").SetText(pooledList[i].title);
				Texture2D previewImage = pooledList[i].previewImage;
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				KButton component2 = gameObject.GetComponent<KButton>();
				int index = i;
				PublishedFileId_t item = pooledList[index].fileId;
				component2.onClick += delegate()
				{
					this.ShowDetails(item);
				};
				component2.onDoubleClick += delegate()
				{
					this.LoadScenario(item);
				};
				this.buttons.Add(gameObject);
				if (item == this.activeItem)
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.HideDetails();
		}
		pooledList.Recycle();
	}

	// Token: 0x06006A1F RID: 27167 RVA: 0x0027FE3C File Offset: 0x0027E03C
	private void LoadScenario(PublishedFileId_t item)
	{
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		DebugUtil.LogArgs(new object[]
		{
			"LoadScenario",
			text,
			num,
			num2
		});
		System.DateTime dateTime;
		byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, new string[]
		{
			".sav"
		}, out dateTime, false);
		string text2 = Path.Combine(SaveLoader.GetSavePrefix(), "scenario.sav");
		File.WriteAllBytes(text2, bytesFromZip);
		SaveLoader.SetActiveSaveFilePath(text2);
		Time.timeScale = 0f;
		App.LoadScene("backend");
	}

	// Token: 0x06006A20 RID: 27168 RVA: 0x0027FECD File Offset: 0x0027E0CD
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x06006A21 RID: 27169 RVA: 0x0027FF04 File Offset: 0x0027E104
	private void ShowDetails(PublishedFileId_t item)
	{
		this.activeItem = item;
		SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(item);
		if (mod != null)
		{
			this.scenarioTitle.text = mod.title;
			this.scenarioDetails.text = mod.description;
		}
		this.loadScenarioButton.onClick += delegate()
		{
			this.LoadScenario(item);
		};
		this.detailsRoot.gameObject.SetActive(true);
	}

	// Token: 0x06006A22 RID: 27170 RVA: 0x0027FF8F File Offset: 0x0027E18F
	private void HideDetails()
	{
		this.detailsRoot.gameObject.SetActive(false);
	}

	// Token: 0x06006A23 RID: 27171 RVA: 0x0027FFA2 File Offset: 0x0027E1A2
	protected override void OnActivate()
	{
		base.OnActivate();
		SteamUGCService.Instance.AddClient(this);
		this.HideDetails();
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x0027FFBB File Offset: 0x0027E1BB
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		SteamUGCService.Instance.RemoveClient(this);
	}

	// Token: 0x06006A25 RID: 27173 RVA: 0x0027FFCE File Offset: 0x0027E1CE
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=scenario");
	}

	// Token: 0x06006A26 RID: 27174 RVA: 0x0027FFDA File Offset: 0x0027E1DA
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		this.RebuildScreen();
	}

	// Token: 0x0400484C RID: 18508
	public const string TAG_SCENARIO = "scenario";

	// Token: 0x0400484D RID: 18509
	public KButton textButton;

	// Token: 0x0400484E RID: 18510
	public KButton dismissButton;

	// Token: 0x0400484F RID: 18511
	public KButton closeButton;

	// Token: 0x04004850 RID: 18512
	public KButton workshopButton;

	// Token: 0x04004851 RID: 18513
	public KButton loadScenarioButton;

	// Token: 0x04004852 RID: 18514
	[Space]
	public GameObject ugcContainer;

	// Token: 0x04004853 RID: 18515
	public GameObject ugcButtonPrefab;

	// Token: 0x04004854 RID: 18516
	public LocText noScenariosText;

	// Token: 0x04004855 RID: 18517
	public RectTransform contentRoot;

	// Token: 0x04004856 RID: 18518
	public RectTransform detailsRoot;

	// Token: 0x04004857 RID: 18519
	public LocText scenarioTitle;

	// Token: 0x04004858 RID: 18520
	public LocText scenarioDetails;

	// Token: 0x04004859 RID: 18521
	private PublishedFileId_t activeItem;

	// Token: 0x0400485A RID: 18522
	private List<GameObject> buttons = new List<GameObject>();
}
