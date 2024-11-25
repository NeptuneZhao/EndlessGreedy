using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
public class BundledAssetsLoader : KMonoBehaviour
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06003569 RID: 13673 RVA: 0x001229D9 File Offset: 0x00120BD9
	// (set) Token: 0x0600356A RID: 13674 RVA: 0x001229E1 File Offset: 0x00120BE1
	public BundledAssets Expansion1Assets { get; private set; }

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x0600356B RID: 13675 RVA: 0x001229EA File Offset: 0x00120BEA
	// (set) Token: 0x0600356C RID: 13676 RVA: 0x001229F2 File Offset: 0x00120BF2
	public List<BundledAssets> DlcAssetsList { get; private set; }

	// Token: 0x0600356D RID: 13677 RVA: 0x001229FC File Offset: 0x00120BFC
	protected override void OnPrefabInit()
	{
		BundledAssetsLoader.instance = this;
		if (DlcManager.IsExpansion1Active())
		{
			global::Debug.Log("Loading Expansion1 assets from bundle");
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
			global::Debug.Assert(assetBundle != null, "Expansion1 is Active but its asset bundle failed to load");
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
			global::Debug.Assert(gameObject != null, "Could not load the Expansion1Assets prefab");
			this.Expansion1Assets = Util.KInstantiate(gameObject, base.gameObject, null).GetComponent<BundledAssets>();
		}
		this.DlcAssetsList = new List<BundledAssets>(DlcManager.DLC_PACKS.Count);
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (DlcManager.IsContentSubscribed(keyValuePair.Key))
			{
				global::Debug.Log("Loading DLC " + keyValuePair.Key + " assets from bundle");
				AssetBundle assetBundle2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName(keyValuePair.Key)));
				global::Debug.Assert(assetBundle2 != null, "DLC " + keyValuePair.Key + " is Active but its asset bundle failed to load");
				GameObject gameObject2 = assetBundle2.LoadAsset<GameObject>(keyValuePair.Value.directory + "Assets");
				global::Debug.Assert(gameObject2 != null, "Could not load the " + keyValuePair.Key + " prefab");
				this.DlcAssetsList.Add(Util.KInstantiate(gameObject2, base.gameObject, null).GetComponent<BundledAssets>());
			}
		}
	}

	// Token: 0x04001FB6 RID: 8118
	public static BundledAssetsLoader instance;
}
