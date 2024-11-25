using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Steamworks;
using UnityEngine;

// Token: 0x02000B6D RID: 2925
public class SteamUGCService : MonoBehaviour
{
	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x060057DE RID: 22494 RVA: 0x001FA036 File Offset: 0x001F8236
	public static SteamUGCService Instance
	{
		get
		{
			return SteamUGCService.instance;
		}
	}

	// Token: 0x060057DF RID: 22495 RVA: 0x001FA040 File Offset: 0x001F8240
	private SteamUGCService()
	{
		this.on_subscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(this.OnItemSubscribed));
		this.on_unsubscribed = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(this.OnItemUnsubscribed));
		this.on_updated = Callback<RemoteStoragePublishedFileUpdated_t>.Create(new Callback<RemoteStoragePublishedFileUpdated_t>.DispatchDelegate(this.OnItemUpdated));
		this.on_query_completed = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryDetailsCompleted));
		this.on_download_completed = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(this.OnDownloadItemComplete));
		this.mods = new List<SteamUGCService.Mod>();
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x001FA14C File Offset: 0x001F834C
	public static void Initialize()
	{
		if (SteamUGCService.instance != null)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("/SteamManager");
		SteamUGCService.instance = gameObject.GetComponent<SteamUGCService>();
		if (SteamUGCService.instance == null)
		{
			SteamUGCService.instance = gameObject.AddComponent<SteamUGCService>();
		}
	}

	// Token: 0x060057E1 RID: 22497 RVA: 0x001FA198 File Offset: 0x001F8398
	public void AddClient(SteamUGCService.IClient client)
	{
		this.clients.Add(client);
		ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
		foreach (SteamUGCService.Mod mod in this.mods)
		{
			pooledList.Add(mod.fileId);
		}
		client.UpdateMods(pooledList, Enumerable.Empty<PublishedFileId_t>(), Enumerable.Empty<PublishedFileId_t>(), Enumerable.Empty<SteamUGCService.Mod>());
		pooledList.Recycle();
	}

	// Token: 0x060057E2 RID: 22498 RVA: 0x001FA220 File Offset: 0x001F8420
	public void RemoveClient(SteamUGCService.IClient client)
	{
		this.clients.Remove(client);
	}

	// Token: 0x060057E3 RID: 22499 RVA: 0x001FA230 File Offset: 0x001F8430
	public void Awake()
	{
		global::Debug.Assert(SteamUGCService.instance == null);
		SteamUGCService.instance = this;
		uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
		if (numSubscribedItems != 0U)
		{
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(array, numSubscribedItems);
			this.downloads.UnionWith(array);
		}
	}

	// Token: 0x060057E4 RID: 22500 RVA: 0x001FA278 File Offset: 0x001F8478
	public bool IsSubscribed(PublishedFileId_t item)
	{
		return this.downloads.Contains(item) || this.proxies.Contains(item) || this.queries.Contains(item) || this.publishes.Any((SteamUGCDetails_t candidate) => candidate.m_nPublishedFileId == item) || this.mods.Exists((SteamUGCService.Mod candidate) => candidate.fileId == item);
	}

	// Token: 0x060057E5 RID: 22501 RVA: 0x001FA300 File Offset: 0x001F8500
	public SteamUGCService.Mod FindMod(PublishedFileId_t item)
	{
		return this.mods.Find((SteamUGCService.Mod candidate) => candidate.fileId == item);
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x001FA331 File Offset: 0x001F8531
	private void OnDestroy()
	{
		global::Debug.Assert(SteamUGCService.instance == this);
		SteamUGCService.instance = null;
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x001FA34C File Offset: 0x001F854C
	private Texture2D LoadPreviewImage(SteamUGCDetails_t details)
	{
		byte[] array = null;
		if (details.m_hPreviewFile != UGCHandle_t.Invalid)
		{
			SteamRemoteStorage.UGCDownload(details.m_hPreviewFile, 0U);
			array = new byte[details.m_nPreviewFileSize];
			if (SteamRemoteStorage.UGCRead(details.m_hPreviewFile, array, details.m_nPreviewFileSize, 0U, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished) != details.m_nPreviewFileSize)
			{
				if (this.retry_counts[details.m_nPublishedFileId] % 100 == 0)
				{
					global::Debug.LogFormat("Steam: Preview image load failed", Array.Empty<object>());
				}
				array = null;
			}
		}
		if (array == null)
		{
			System.DateTime dateTime;
			array = SteamUGCService.GetBytesFromZip(details.m_nPublishedFileId, SteamUGCService.previewFileNames, out dateTime, false);
		}
		Texture2D texture2D = null;
		if (array != null)
		{
			texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(array);
		}
		else
		{
			Dictionary<PublishedFileId_t, int> dictionary = this.retry_counts;
			PublishedFileId_t nPublishedFileId = details.m_nPublishedFileId;
			dictionary[nPublishedFileId]++;
		}
		return texture2D;
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x001FA41C File Offset: 0x001F861C
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		this.downloads.ExceptWith(this.removals);
		this.publishes.RemoveWhere((SteamUGCDetails_t publish) => this.removals.Contains(publish.m_nPublishedFileId));
		this.previews.RemoveWhere((SteamUGCDetails_t publish) => this.removals.Contains(publish.m_nPublishedFileId));
		this.proxies.ExceptWith(this.removals);
		HashSetPool<SteamUGCService.Mod, SteamUGCService>.PooledHashSet loaded_previews = HashSetPool<SteamUGCService.Mod, SteamUGCService>.Allocate();
		HashSetPool<PublishedFileId_t, SteamUGCService>.PooledHashSet cancelled_previews = HashSetPool<PublishedFileId_t, SteamUGCService>.Allocate();
		SteamUGCService.Mod mod;
		foreach (SteamUGCDetails_t steamUGCDetails_t in this.previews)
		{
			mod = this.FindMod(steamUGCDetails_t.m_nPublishedFileId);
			DebugUtil.DevAssert(mod != null, "expect mod with pending preview to be published", null);
			mod.previewImage = this.LoadPreviewImage(steamUGCDetails_t);
			if (mod.previewImage != null)
			{
				loaded_previews.Add(mod);
			}
			else if (1000 < this.retry_counts[steamUGCDetails_t.m_nPublishedFileId])
			{
				cancelled_previews.Add(mod.fileId);
			}
		}
		this.previews.RemoveWhere((SteamUGCDetails_t publish) => loaded_previews.Any((SteamUGCService.Mod mod) => mod.fileId == publish.m_nPublishedFileId) || cancelled_previews.Contains(publish.m_nPublishedFileId));
		cancelled_previews.Recycle();
		ListPool<SteamUGCService.Mod, SteamUGCService>.PooledList pooledList = ListPool<SteamUGCService.Mod, SteamUGCService>.Allocate();
		HashSetPool<PublishedFileId_t, SteamUGCService>.PooledHashSet published = HashSetPool<PublishedFileId_t, SteamUGCService>.Allocate();
		foreach (SteamUGCDetails_t steamUGCDetails_t2 in this.publishes)
		{
			if ((SteamUGC.GetItemState(steamUGCDetails_t2.m_nPublishedFileId) & 48U) == 0U)
			{
				global::Debug.LogFormat("Steam: updating info for mod {0}", new object[]
				{
					steamUGCDetails_t2.m_rgchTitle
				});
				SteamUGCService.Mod mod2 = new SteamUGCService.Mod(steamUGCDetails_t2, this.LoadPreviewImage(steamUGCDetails_t2));
				pooledList.Add(mod2);
				if (steamUGCDetails_t2.m_hPreviewFile != UGCHandle_t.Invalid && mod2.previewImage == null)
				{
					this.previews.Add(steamUGCDetails_t2);
				}
				published.Add(steamUGCDetails_t2.m_nPublishedFileId);
			}
		}
		this.publishes.RemoveWhere((SteamUGCDetails_t publish) => published.Contains(publish.m_nPublishedFileId));
		published.Recycle();
		foreach (PublishedFileId_t publishedFileId_t in this.proxies)
		{
			global::Debug.LogFormat("Steam: proxy mod {0}", new object[]
			{
				publishedFileId_t
			});
			pooledList.Add(new SteamUGCService.Mod(publishedFileId_t));
		}
		this.proxies.Clear();
		ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList2 = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
		ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList3 = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
		using (List<SteamUGCService.Mod>.Enumerator enumerator3 = pooledList.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				SteamUGCService.Mod mod = enumerator3.Current;
				int num = this.mods.FindIndex((SteamUGCService.Mod candidate) => candidate.fileId == mod.fileId);
				if (num == -1)
				{
					this.mods.Add(mod);
					pooledList2.Add(mod.fileId);
				}
				else
				{
					this.mods[num] = mod;
					pooledList3.Add(mod.fileId);
				}
			}
		}
		pooledList.Recycle();
		bool flag = this.details_query == UGCQueryHandle_t.Invalid;
		if (pooledList2.Count != 0 || pooledList3.Count != 0 || (flag && this.removals.Count != 0) || loaded_previews.Count != 0)
		{
			foreach (SteamUGCService.IClient client in this.clients)
			{
				IEnumerable<PublishedFileId_t> added = pooledList2;
				IEnumerable<PublishedFileId_t> updated = pooledList3;
				IEnumerable<PublishedFileId_t> removed;
				if (!flag)
				{
					removed = Enumerable.Empty<PublishedFileId_t>();
				}
				else
				{
					IEnumerable<PublishedFileId_t> enumerable = this.removals;
					removed = enumerable;
				}
				client.UpdateMods(added, updated, removed, loaded_previews);
			}
		}
		pooledList2.Recycle();
		pooledList3.Recycle();
		loaded_previews.Recycle();
		if (flag)
		{
			using (HashSet<PublishedFileId_t>.Enumerator enumerator2 = this.removals.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PublishedFileId_t removal = enumerator2.Current;
					this.mods.RemoveAll((SteamUGCService.Mod candidate) => candidate.fileId == removal);
				}
			}
			this.removals.Clear();
		}
		foreach (PublishedFileId_t publishedFileId_t2 in this.downloads)
		{
			EItemState itemState = (EItemState)SteamUGC.GetItemState(publishedFileId_t2);
			if (((itemState & EItemState.k_EItemStateInstalled) == EItemState.k_EItemStateNone || (itemState & EItemState.k_EItemStateNeedsUpdate) != EItemState.k_EItemStateNone) && (itemState & (EItemState.k_EItemStateDownloading | EItemState.k_EItemStateDownloadPending)) == EItemState.k_EItemStateNone && SteamUGC.DownloadItem(publishedFileId_t2, false))
			{
				this.awaiting_download.Add(publishedFileId_t2);
			}
		}
		if (this.details_query == UGCQueryHandle_t.Invalid)
		{
			this.queries.UnionWith(this.downloads);
			this.queries.ExceptWith(this.awaiting_download);
			this.downloads.Clear();
			if (this.queries.Count != 0)
			{
				this.details_query = SteamUGC.CreateQueryUGCDetailsRequest(this.queries.ToArray<PublishedFileId_t>(), (uint)this.queries.Count);
				SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(this.details_query);
				this.on_query_completed.Set(hAPICall, null);
			}
		}
	}

	// Token: 0x060057E9 RID: 22505 RVA: 0x001FA9F4 File Offset: 0x001F8BF4
	private void OnSteamUGCQueryDetailsCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
	{
		EResult eResult = pCallback.m_eResult;
		if (eResult != EResult.k_EResultOK)
		{
			if (eResult != EResult.k_EResultBusy)
			{
				string[] array = new string[10];
				array[0] = "Steam: [OnSteamUGCQueryDetailsCompleted] - handle: ";
				int num = 1;
				UGCQueryHandle_t handle = pCallback.m_handle;
				array[num] = handle.ToString();
				array[2] = " -- Result: ";
				array[3] = pCallback.m_eResult.ToString();
				array[4] = " -- NUm results: ";
				array[5] = pCallback.m_unNumResultsReturned.ToString();
				array[6] = " --Total Matching: ";
				array[7] = pCallback.m_unTotalMatchingResults.ToString();
				array[8] = " -- cached: ";
				array[9] = pCallback.m_bCachedData.ToString();
				global::Debug.Log(string.Concat(array));
				HashSet<PublishedFileId_t> hashSet = this.proxies;
				this.proxies = this.queries;
				this.queries = hashSet;
			}
			else
			{
				string[] array2 = new string[5];
				array2[0] = "Steam: [OnSteamUGCQueryDetailsCompleted] - handle: ";
				int num2 = 1;
				UGCQueryHandle_t handle = pCallback.m_handle;
				array2[num2] = handle.ToString();
				array2[2] = " -- Result: ";
				array2[3] = pCallback.m_eResult.ToString();
				array2[4] = " Resending";
				global::Debug.Log(string.Concat(array2));
			}
		}
		else
		{
			for (uint num3 = 0U; num3 < pCallback.m_unNumResultsReturned; num3 += 1U)
			{
				SteamUGCDetails_t steamUGCDetails_t = default(SteamUGCDetails_t);
				if (SteamUGC.GetQueryUGCResult(this.details_query, num3, out steamUGCDetails_t))
				{
					if (!this.removals.Contains(steamUGCDetails_t.m_nPublishedFileId))
					{
						this.publishes.Add(steamUGCDetails_t);
						this.retry_counts[steamUGCDetails_t.m_nPublishedFileId] = 0;
					}
					this.queries.Remove(steamUGCDetails_t.m_nPublishedFileId);
				}
				else
				{
					KCrashReporter.ReportDevNotification("SteamUGCService.GetQueryUGCResult details_query is an invalid handle!", Environment.StackTrace, "", false, null);
				}
			}
		}
		SteamUGC.ReleaseQueryUGCRequest(this.details_query);
		this.details_query = UGCQueryHandle_t.Invalid;
	}

	// Token: 0x060057EA RID: 22506 RVA: 0x001FABC5 File Offset: 0x001F8DC5
	private void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t pCallback)
	{
		this.downloads.Add(pCallback.m_nPublishedFileId);
	}

	// Token: 0x060057EB RID: 22507 RVA: 0x001FABD9 File Offset: 0x001F8DD9
	private void OnItemUpdated(RemoteStoragePublishedFileUpdated_t pCallback)
	{
		this.downloads.Add(pCallback.m_nPublishedFileId);
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x001FABED File Offset: 0x001F8DED
	private void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t pCallback)
	{
		this.removals.Add(pCallback.m_nPublishedFileId);
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x001FAC04 File Offset: 0x001F8E04
	private void OnDownloadItemComplete(DownloadItemResult_t callback)
	{
		if (SteamManager.ONI_STEAM_APP_IDS.Contains(callback.m_unAppID) && callback.m_eResult == EResult.k_EResultOK)
		{
			this.queries.Add(callback.m_nPublishedFileId);
			this.awaiting_download.Remove(callback.m_nPublishedFileId);
		}
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x001FAC50 File Offset: 0x001F8E50
	public static byte[] GetBytesFromZip(PublishedFileId_t item, string[] filesToExtract, out System.DateTime lastModified, bool getFirstMatch = false)
	{
		byte[] result = null;
		lastModified = System.DateTime.MinValue;
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		try
		{
			lastModified = File.GetLastWriteTimeUtc(text);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipFile zipFile = ZipFile.Read(text))
				{
					ZipEntry zipEntry = null;
					foreach (string text2 in filesToExtract)
					{
						if (text2.Length > 4)
						{
							if (zipFile.ContainsEntry(text2))
							{
								zipEntry = zipFile[text2];
							}
						}
						else
						{
							foreach (ZipEntry zipEntry2 in zipFile.Entries)
							{
								if (zipEntry2.FileName.EndsWith(text2))
								{
									zipEntry = zipEntry2;
									break;
								}
							}
						}
						if (zipEntry != null)
						{
							break;
						}
					}
					if (zipEntry != null)
					{
						zipEntry.Extract(memoryStream);
						memoryStream.Flush();
						result = memoryStream.ToArray();
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	// Token: 0x04003969 RID: 14697
	private UGCQueryHandle_t details_query = UGCQueryHandle_t.Invalid;

	// Token: 0x0400396A RID: 14698
	private Callback<RemoteStoragePublishedFileSubscribed_t> on_subscribed;

	// Token: 0x0400396B RID: 14699
	private Callback<RemoteStoragePublishedFileUpdated_t> on_updated;

	// Token: 0x0400396C RID: 14700
	private Callback<RemoteStoragePublishedFileUnsubscribed_t> on_unsubscribed;

	// Token: 0x0400396D RID: 14701
	private CallResult<SteamUGCQueryCompleted_t> on_query_completed;

	// Token: 0x0400396E RID: 14702
	private Callback<DownloadItemResult_t> on_download_completed;

	// Token: 0x0400396F RID: 14703
	private HashSet<PublishedFileId_t> downloads = new HashSet<PublishedFileId_t>();

	// Token: 0x04003970 RID: 14704
	private HashSet<PublishedFileId_t> queries = new HashSet<PublishedFileId_t>();

	// Token: 0x04003971 RID: 14705
	private HashSet<PublishedFileId_t> proxies = new HashSet<PublishedFileId_t>();

	// Token: 0x04003972 RID: 14706
	private HashSet<SteamUGCDetails_t> publishes = new HashSet<SteamUGCDetails_t>();

	// Token: 0x04003973 RID: 14707
	private HashSet<PublishedFileId_t> removals = new HashSet<PublishedFileId_t>();

	// Token: 0x04003974 RID: 14708
	private HashSet<SteamUGCDetails_t> previews = new HashSet<SteamUGCDetails_t>();

	// Token: 0x04003975 RID: 14709
	private HashSet<PublishedFileId_t> awaiting_download = new HashSet<PublishedFileId_t>();

	// Token: 0x04003976 RID: 14710
	private List<SteamUGCService.Mod> mods = new List<SteamUGCService.Mod>();

	// Token: 0x04003977 RID: 14711
	private Dictionary<PublishedFileId_t, int> retry_counts = new Dictionary<PublishedFileId_t, int>();

	// Token: 0x04003978 RID: 14712
	private static readonly string[] previewFileNames = new string[]
	{
		"preview.png",
		"Preview.png",
		"PREVIEW.png",
		".png",
		".jpg"
	};

	// Token: 0x04003979 RID: 14713
	private List<SteamUGCService.IClient> clients = new List<SteamUGCService.IClient>();

	// Token: 0x0400397A RID: 14714
	private static SteamUGCService instance;

	// Token: 0x0400397B RID: 14715
	private const EItemState DOWNLOADING_MASK = EItemState.k_EItemStateDownloading | EItemState.k_EItemStateDownloadPending;

	// Token: 0x0400397C RID: 14716
	private const int RETRY_THRESHOLD = 1000;

	// Token: 0x02001BC9 RID: 7113
	public class Mod
	{
		// Token: 0x0600A482 RID: 42114 RVA: 0x0038CB8C File Offset: 0x0038AD8C
		public Mod(SteamUGCDetails_t item, Texture2D previewImage)
		{
			this.title = item.m_rgchTitle;
			this.description = item.m_rgchDescription;
			this.fileId = item.m_nPublishedFileId;
			this.lastUpdateTime = (ulong)item.m_rtimeUpdated;
			this.tags = new List<string>(item.m_rgchTags.Split(',', StringSplitOptions.None));
			this.previewImage = previewImage;
		}

		// Token: 0x0600A483 RID: 42115 RVA: 0x0038CBF3 File Offset: 0x0038ADF3
		public Mod(PublishedFileId_t id)
		{
			this.title = string.Empty;
			this.description = string.Empty;
			this.fileId = id;
			this.lastUpdateTime = 0UL;
			this.tags = new List<string>();
			this.previewImage = null;
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x0600A484 RID: 42116 RVA: 0x0038CC32 File Offset: 0x0038AE32
		// (set) Token: 0x0600A485 RID: 42117 RVA: 0x0038CC3A File Offset: 0x0038AE3A
		public string title { get; private set; }

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600A486 RID: 42118 RVA: 0x0038CC43 File Offset: 0x0038AE43
		// (set) Token: 0x0600A487 RID: 42119 RVA: 0x0038CC4B File Offset: 0x0038AE4B
		public string description { get; private set; }

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600A488 RID: 42120 RVA: 0x0038CC54 File Offset: 0x0038AE54
		// (set) Token: 0x0600A489 RID: 42121 RVA: 0x0038CC5C File Offset: 0x0038AE5C
		public PublishedFileId_t fileId { get; private set; }

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x0600A48A RID: 42122 RVA: 0x0038CC65 File Offset: 0x0038AE65
		// (set) Token: 0x0600A48B RID: 42123 RVA: 0x0038CC6D File Offset: 0x0038AE6D
		public ulong lastUpdateTime { get; private set; }

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x0600A48C RID: 42124 RVA: 0x0038CC76 File Offset: 0x0038AE76
		// (set) Token: 0x0600A48D RID: 42125 RVA: 0x0038CC7E File Offset: 0x0038AE7E
		public List<string> tags { get; private set; }

		// Token: 0x040080BD RID: 32957
		public Texture2D previewImage;
	}

	// Token: 0x02001BCA RID: 7114
	public interface IClient
	{
		// Token: 0x0600A48E RID: 42126
		void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews);
	}
}
