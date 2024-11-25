using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using KMod;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000651 RID: 1617
[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x0600278F RID: 10127 RVA: 0x000E1108 File Offset: 0x000DF308
	protected override void OnPrefabInit()
	{
		Assets.instance = this;
		if (KPlayerPrefs.HasKey("TemperatureUnit"))
		{
			GameUtil.temperatureUnit = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt("TemperatureUnit");
		}
		if (KPlayerPrefs.HasKey("MassUnit"))
		{
			GameUtil.massUnit = (GameUtil.MassUnit)KPlayerPrefs.GetInt("MassUnit");
		}
		RecipeManager.DestroyInstance();
		RecipeManager.Get();
		Assets.AnimMaterial = this.AnimMaterialAsset;
		Assets.Prefabs = new List<KPrefabID>(from x in this.PrefabAssets
		where x != null
		select x);
		Assets.PrefabsByTag.Clear();
		Assets.PrefabsByAdditionalTags.Clear();
		Assets.CountableTags.Clear();
		Assets.Sprites = new Dictionary<HashedString, Sprite>();
		foreach (Sprite sprite in this.SpriteAssets)
		{
			if (!(sprite == null))
			{
				HashedString key = new HashedString(sprite.name);
				Assets.Sprites.Add(key, sprite);
			}
		}
		Assets.TintedSprites = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		Assets.Materials = (from x in this.MaterialAssets
		where x != null
		select x).ToList<Material>();
		Assets.Textures = (from x in this.TextureAssets
		where x != null
		select x).ToList<Texture2D>();
		Assets.TextureAtlases = (from x in this.TextureAtlasAssets
		where x != null
		select x).ToList<TextureAtlas>();
		Assets.BlockTileDecorInfos = (from x in this.BlockTileDecorInfoAssets
		where x != null
		select x).ToList<BlockTileDecorInfo>();
		this.LoadAnims();
		Assets.UIPrefabs = this.UIPrefabAssets;
		Assets.DebugFont = this.DebugFontAsset;
		AsyncLoadManager<IGlobalAsyncLoader>.Run();
		GameAudioSheets.Get().Initialize();
		this.SubstanceListHookup();
		this.CreatePrefabs();
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000E1360 File Offset: 0x000DF560
	private void CreatePrefabs()
	{
		Db.Get();
		Assets.BuildingDefs = new List<BuildingDef>();
		foreach (KPrefabID kprefabID in this.PrefabAssets)
		{
			if (!(kprefabID == null))
			{
				kprefabID.InitializeTags(true);
				Assets.AddPrefab(kprefabID);
			}
		}
		LegacyModMain.Load();
		Db.Get().PostProcess();
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000E13E4 File Offset: 0x000DF5E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Db.Get();
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000E13F4 File Offset: 0x000DF5F4
	private static void TryAddCountableTag(KPrefabID prefab)
	{
		foreach (Tag tag in GameTags.DisplayAsUnits)
		{
			if (prefab.HasTag(tag))
			{
				Assets.AddCountableTag(prefab.PrefabTag);
				break;
			}
		}
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000E1450 File Offset: 0x000DF650
	public static void AddCountableTag(Tag tag)
	{
		Assets.CountableTags.Add(tag);
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000E145E File Offset: 0x000DF65E
	public static bool IsTagCountable(Tag tag)
	{
		return Assets.CountableTags.Contains(tag);
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000E146B File Offset: 0x000DF66B
	private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab)
	{
		if (prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
		{
			Assets.SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
		}
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x000E148B File Offset: 0x000DF68B
	public static bool IsTagSolidTransferArmConveyable(Tag tag)
	{
		return Assets.SolidTransferArmConeyableTags.Contains(tag);
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x000E1498 File Offset: 0x000DF698
	private void LoadAnims()
	{
		KAnimBatchManager.DestroyInstance();
		KAnimGroupFile.DestroyInstance();
		KGlobalAnimParser.DestroyInstance();
		KAnimBatchManager.CreateInstance();
		KGlobalAnimParser.CreateInstance();
		KAnimGroupFile.LoadGroupResourceFile();
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			this.AnimAssets.AddRange(BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);
		}
		foreach (BundledAssets bundledAssets in BundledAssetsLoader.instance.DlcAssetsList)
		{
			this.AnimAssets.AddRange(bundledAssets.AnimAssets);
		}
		Assets.Anims = (from x in this.AnimAssets
		where x != null
		select x).ToList<KAnimFile>();
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		Assets.AnimTable.Clear();
		foreach (KAnimFile kanimFile in Assets.Anims)
		{
			if (kanimFile != null)
			{
				HashedString key = kanimFile.name;
				Assets.AnimTable[key] = kanimFile;
			}
		}
		KAnimGroupFile.MapNamesToAnimFiles(Assets.AnimTable);
		Global.Instance.modManager.Load(Content.Animation);
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		foreach (KAnimFile kanimFile2 in Assets.ModLoadedKAnims)
		{
			if (kanimFile2 != null)
			{
				HashedString key2 = kanimFile2.name;
				Assets.AnimTable[key2] = kanimFile2;
			}
		}
		global::Debug.Assert(Assets.AnimTable.Count > 0, "Anim Assets not yet loaded");
		KAnimGroupFile.LoadAll();
		foreach (KAnimFile kanimFile3 in Assets.Anims)
		{
			kanimFile3.FinalizeLoading();
		}
		KAnimBatchManager.Instance().CompleteInit();
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000E16DC File Offset: 0x000DF8DC
	private void SubstanceListHookup()
	{
		Dictionary<string, SubstanceTable> dictionary = new Dictionary<string, SubstanceTable>
		{
			{
				"",
				this.substanceTable
			}
		};
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			dictionary["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;
		}
		Hashtable hashtable = new Hashtable();
		ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
		ElementLoader.Load(ref hashtable, dictionary);
		List<Element> list = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingMetalOre));
		GameTags.StartingMetalOres = new Tag[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			GameTags.StartingMetalOres[i] = list[i].tag;
		}
		List<Element> list2 = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingRefinedMetalOre));
		GameTags.StartingRefinedMetalOres = new Tag[list2.Count];
		for (int j = 0; j < list2.Count; j++)
		{
			GameTags.StartingRefinedMetalOres[j] = list2[j].tag;
		}
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x000E181E File Offset: 0x000DFA1E
	public static string GetSimpleSoundEventName(EventReference event_ref)
	{
		return Assets.GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
	}

	// Token: 0x0600279A RID: 10138 RVA: 0x000E182C File Offset: 0x000DFA2C
	public static string GetSimpleSoundEventName(string path)
	{
		string text = null;
		if (!Assets.simpleSoundEventNames.TryGetValue(path, out text))
		{
			int num = path.LastIndexOf('/');
			text = ((num != -1) ? path.Substring(num + 1) : path);
			Assets.simpleSoundEventNames[path] = text;
		}
		return text;
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x000E1874 File Offset: 0x000DFA74
	private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
	{
		int count = defs.Count;
		for (int i = 0; i < count; i++)
		{
			if (defs[i].PrefabID == prefab_id)
			{
				return defs[i];
			}
		}
		return null;
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x000E18B1 File Offset: 0x000DFAB1
	public static BuildingDef GetBuildingDef(string prefab_id)
	{
		return Assets.GetDef(Assets.BuildingDefs, prefab_id);
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000E18C0 File Offset: 0x000DFAC0
	public static TintedSprite GetTintedSprite(string name)
	{
		TintedSprite result = null;
		if (Assets.TintedSprites != null)
		{
			for (int i = 0; i < Assets.TintedSprites.Count; i++)
			{
				if (Assets.TintedSprites[i].sprite.name == name)
				{
					result = Assets.TintedSprites[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000E1918 File Offset: 0x000DFB18
	public static Sprite GetSprite(HashedString name)
	{
		Sprite result = null;
		if (Assets.Sprites != null)
		{
			Assets.Sprites.TryGetValue(name, out result);
		}
		return result;
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000E193D File Offset: 0x000DFB3D
	public static VideoClip GetVideo(string name)
	{
		return Resources.Load<VideoClip>("video_webm/" + name);
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000E1950 File Offset: 0x000DFB50
	public static Texture2D GetTexture(string name)
	{
		Texture2D result = null;
		if (Assets.Textures != null)
		{
			for (int i = 0; i < Assets.Textures.Count; i++)
			{
				if (Assets.Textures[i].name == name)
				{
					result = Assets.Textures[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x000E19A4 File Offset: 0x000DFBA4
	public static ComicData GetComic(string id)
	{
		foreach (ComicData comicData in Assets.instance.comics)
		{
			if (comicData.name == id)
			{
				return comicData;
			}
		}
		return null;
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000E19E0 File Offset: 0x000DFBE0
	public static void AddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		prefab.InitializeTags(true);
		prefab.UpdateSaveLoadTag();
		if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
		{
			string str = "Tried loading prefab with duplicate tag, ignoring: ";
			Tag prefabTag = prefab.PrefabTag;
			global::Debug.LogWarning(str + prefabTag.ToString());
			return;
		}
		Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
		foreach (Tag key in prefab.Tags)
		{
			if (!Assets.PrefabsByAdditionalTags.ContainsKey(key))
			{
				Assets.PrefabsByAdditionalTags[key] = new List<KPrefabID>();
			}
			Assets.PrefabsByAdditionalTags[key].Add(prefab);
		}
		Assets.Prefabs.Add(prefab);
		Assets.TryAddCountableTag(prefab);
		Assets.TryAddSolidTransferArmConveyableTag(prefab);
		if (Assets.OnAddPrefab != null)
		{
			Assets.OnAddPrefab(prefab);
		}
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x000E1AE4 File Offset: 0x000DFCE4
	public static void RegisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Combine(Assets.OnAddPrefab, on_add);
		foreach (KPrefabID obj in Assets.Prefabs)
		{
			on_add(obj);
		}
	}

	// Token: 0x060027A4 RID: 10148 RVA: 0x000E1B4C File Offset: 0x000DFD4C
	public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Remove(Assets.OnAddPrefab, on_add);
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x000E1B63 File Offset: 0x000DFD63
	public static void ClearOnAddPrefab()
	{
		Assets.OnAddPrefab = null;
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x000E1B6C File Offset: 0x000DFD6C
	public static GameObject GetPrefab(Tag tag)
	{
		GameObject gameObject = Assets.TryGetPrefab(tag);
		if (gameObject == null)
		{
			string str = "Missing prefab: ";
			Tag tag2 = tag;
			global::Debug.LogWarning(str + tag2.ToString());
		}
		return gameObject;
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000E1BA8 File Offset: 0x000DFDA8
	public static GameObject TryGetPrefab(Tag tag)
	{
		KPrefabID kprefabID = null;
		Assets.PrefabsByTag.TryGetValue(tag, out kprefabID);
		if (!(kprefabID != null))
		{
			return null;
		}
		return kprefabID.gameObject;
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000E1BD8 File Offset: 0x000DFDD8
	public static List<GameObject> GetPrefabsWithTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
		{
			for (int i = 0; i < Assets.PrefabsByAdditionalTags[tag].Count; i++)
			{
				list.Add(Assets.PrefabsByAdditionalTags[tag][i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000E1C30 File Offset: 0x000DFE30
	public static List<GameObject> GetPrefabsWithComponent<Type>()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000E1C86 File Offset: 0x000DFE86
	public static GameObject GetPrefabWithComponent<Type>()
	{
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Type>();
		global::Debug.Assert(prefabsWithComponent.Count > 0, "There are no prefabs of type " + typeof(Type).Name);
		return prefabsWithComponent[0];
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000E1CBC File Offset: 0x000DFEBC
	public static List<Tag> GetPrefabTagsWithComponent<Type>()
	{
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].PrefabID());
			}
		}
		return list;
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000E1D14 File Offset: 0x000DFF14
	public static Assets GetInstanceEditorOnly()
	{
		Assets[] array = (Assets[])Resources.FindObjectsOfTypeAll(typeof(Assets));
		if (array != null)
		{
			int num = array.Length;
		}
		return array[0];
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000E1D40 File Offset: 0x000DFF40
	public static TextureAtlas GetTextureAtlas(string name)
	{
		foreach (TextureAtlas textureAtlas in Assets.TextureAtlases)
		{
			if (textureAtlas.name == name)
			{
				return textureAtlas;
			}
		}
		return null;
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000E1DA0 File Offset: 0x000DFFA0
	public static Material GetMaterial(string name)
	{
		foreach (Material material in Assets.Materials)
		{
			if (material.name == name)
			{
				return material;
			}
		}
		return null;
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000E1E00 File Offset: 0x000E0000
	public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
	{
		foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
		{
			if (blockTileDecorInfo.name == name)
			{
				return blockTileDecorInfo;
			}
		}
		global::Debug.LogError("Could not find BlockTileDecorInfo named [" + name + "]");
		return null;
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000E1E78 File Offset: 0x000E0078
	public static KAnimFile GetAnim(HashedString name)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			return null;
		}
		KAnimFile kanimFile = null;
		Assets.AnimTable.TryGetValue(name, out kanimFile);
		if (kanimFile == null)
		{
			global::Debug.LogWarning("Missing Anim: [" + name.ToString() + "]. You may have to run Collect Anim on the Assets prefab");
		}
		return kanimFile;
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000E1ED5 File Offset: 0x000E00D5
	public static bool TryGetAnim(HashedString name, out KAnimFile anim)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			anim = null;
			return false;
		}
		Assets.AnimTable.TryGetValue(name, out anim);
		return anim != null;
	}

	// Token: 0x060027B2 RID: 10162 RVA: 0x000E1F04 File Offset: 0x000E0104
	public void OnAfterDeserialize()
	{
		this.TintedSpriteAssets = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		this.TintedSpriteAssets.Sort((TintedSprite a, TintedSprite b) => a.name.CompareTo(b.name));
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x000E1F70 File Offset: 0x000E0170
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x000E1F74 File Offset: 0x000E0174
	public static void AddBuildingDef(BuildingDef def)
	{
		Assets.BuildingDefs = (from x in Assets.BuildingDefs
		where x.PrefabID != def.PrefabID
		select x).ToList<BuildingDef>();
		Assets.BuildingDefs.Add(def);
	}

	// Token: 0x040016CF RID: 5839
	public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();

	// Token: 0x040016D0 RID: 5840
	private static Action<KPrefabID> OnAddPrefab;

	// Token: 0x040016D1 RID: 5841
	public static List<BuildingDef> BuildingDefs;

	// Token: 0x040016D2 RID: 5842
	public List<KPrefabID> PrefabAssets = new List<KPrefabID>();

	// Token: 0x040016D3 RID: 5843
	public static List<KPrefabID> Prefabs = new List<KPrefabID>();

	// Token: 0x040016D4 RID: 5844
	private static HashSet<Tag> CountableTags = new HashSet<Tag>();

	// Token: 0x040016D5 RID: 5845
	private static HashSet<Tag> SolidTransferArmConeyableTags = new HashSet<Tag>();

	// Token: 0x040016D6 RID: 5846
	public List<Sprite> SpriteAssets;

	// Token: 0x040016D7 RID: 5847
	public static Dictionary<HashedString, Sprite> Sprites;

	// Token: 0x040016D8 RID: 5848
	public List<string> videoClipNames;

	// Token: 0x040016D9 RID: 5849
	private const string VIDEO_ASSET_PATH = "video_webm";

	// Token: 0x040016DA RID: 5850
	public List<TintedSprite> TintedSpriteAssets;

	// Token: 0x040016DB RID: 5851
	public static List<TintedSprite> TintedSprites;

	// Token: 0x040016DC RID: 5852
	public List<Texture2D> TextureAssets;

	// Token: 0x040016DD RID: 5853
	public static List<Texture2D> Textures;

	// Token: 0x040016DE RID: 5854
	public static List<TextureAtlas> TextureAtlases;

	// Token: 0x040016DF RID: 5855
	public List<TextureAtlas> TextureAtlasAssets;

	// Token: 0x040016E0 RID: 5856
	public static List<Material> Materials;

	// Token: 0x040016E1 RID: 5857
	public List<Material> MaterialAssets;

	// Token: 0x040016E2 RID: 5858
	public static List<Shader> Shaders;

	// Token: 0x040016E3 RID: 5859
	public List<Shader> ShaderAssets;

	// Token: 0x040016E4 RID: 5860
	public static List<BlockTileDecorInfo> BlockTileDecorInfos;

	// Token: 0x040016E5 RID: 5861
	public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;

	// Token: 0x040016E6 RID: 5862
	public Material AnimMaterialAsset;

	// Token: 0x040016E7 RID: 5863
	public static Material AnimMaterial;

	// Token: 0x040016E8 RID: 5864
	public DiseaseVisualization DiseaseVisualization;

	// Token: 0x040016E9 RID: 5865
	public Sprite LegendColourBox;

	// Token: 0x040016EA RID: 5866
	public Texture2D invalidAreaTex;

	// Token: 0x040016EB RID: 5867
	public Assets.UIPrefabData UIPrefabAssets;

	// Token: 0x040016EC RID: 5868
	public static Assets.UIPrefabData UIPrefabs;

	// Token: 0x040016ED RID: 5869
	private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();

	// Token: 0x040016EE RID: 5870
	private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();

	// Token: 0x040016EF RID: 5871
	public List<KAnimFile> AnimAssets;

	// Token: 0x040016F0 RID: 5872
	public static List<KAnimFile> Anims;

	// Token: 0x040016F1 RID: 5873
	private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();

	// Token: 0x040016F2 RID: 5874
	public Font DebugFontAsset;

	// Token: 0x040016F3 RID: 5875
	public static Font DebugFont;

	// Token: 0x040016F4 RID: 5876
	public SubstanceTable substanceTable;

	// Token: 0x040016F5 RID: 5877
	[SerializeField]
	public TextAsset elementAudio;

	// Token: 0x040016F6 RID: 5878
	[SerializeField]
	public TextAsset personalitiesFile;

	// Token: 0x040016F7 RID: 5879
	public LogicModeUI logicModeUIData;

	// Token: 0x040016F8 RID: 5880
	public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;

	// Token: 0x040016F9 RID: 5881
	public DigPlacerConfig.DigPlacerAssets digPlacerAssets;

	// Token: 0x040016FA RID: 5882
	public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;

	// Token: 0x040016FB RID: 5883
	public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;

	// Token: 0x040016FC RID: 5884
	public ComicData[] comics;

	// Token: 0x040016FD RID: 5885
	public static Assets instance;

	// Token: 0x040016FE RID: 5886
	private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();

	// Token: 0x0200142E RID: 5166
	[Serializable]
	public struct UIPrefabData
	{
		// Token: 0x04006904 RID: 26884
		public ProgressBar ProgressBar;

		// Token: 0x04006905 RID: 26885
		public HealthBar HealthBar;

		// Token: 0x04006906 RID: 26886
		public GameObject ResourceVisualizer;

		// Token: 0x04006907 RID: 26887
		public GameObject KAnimVisualizer;

		// Token: 0x04006908 RID: 26888
		public Image RegionCellBlocked;

		// Token: 0x04006909 RID: 26889
		public RectTransform PriorityOverlayIcon;

		// Token: 0x0400690A RID: 26890
		public RectTransform HarvestWhenReadyOverlayIcon;

		// Token: 0x0400690B RID: 26891
		public Assets.TableScreenAssets TableScreenWidgets;
	}

	// Token: 0x0200142F RID: 5167
	[Serializable]
	public struct TableScreenAssets
	{
		// Token: 0x0400690C RID: 26892
		public Material DefaultUIMaterial;

		// Token: 0x0400690D RID: 26893
		public Material DesaturatedUIMaterial;

		// Token: 0x0400690E RID: 26894
		public GameObject MinionPortrait;

		// Token: 0x0400690F RID: 26895
		public GameObject GenericPortrait;

		// Token: 0x04006910 RID: 26896
		public GameObject TogglePortrait;

		// Token: 0x04006911 RID: 26897
		public GameObject ButtonLabel;

		// Token: 0x04006912 RID: 26898
		public GameObject ButtonLabelWhite;

		// Token: 0x04006913 RID: 26899
		public GameObject Label;

		// Token: 0x04006914 RID: 26900
		public GameObject LabelHeader;

		// Token: 0x04006915 RID: 26901
		public GameObject Checkbox;

		// Token: 0x04006916 RID: 26902
		public GameObject BlankCell;

		// Token: 0x04006917 RID: 26903
		public GameObject SuperCheckbox_Horizontal;

		// Token: 0x04006918 RID: 26904
		public GameObject SuperCheckbox_Vertical;

		// Token: 0x04006919 RID: 26905
		public GameObject Spacer;

		// Token: 0x0400691A RID: 26906
		public GameObject NumericDropDown;

		// Token: 0x0400691B RID: 26907
		public GameObject DropDownHeader;

		// Token: 0x0400691C RID: 26908
		public GameObject PriorityGroupSelector;

		// Token: 0x0400691D RID: 26909
		public GameObject PriorityGroupSelectorHeader;

		// Token: 0x0400691E RID: 26910
		public GameObject PrioritizeRowWidget;

		// Token: 0x0400691F RID: 26911
		public GameObject PrioritizeRowHeaderWidget;
	}
}
