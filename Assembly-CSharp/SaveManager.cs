using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
[AddComponentMenu("KMonoBehaviour/scripts/SaveManager")]
public class SaveManager : KMonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060022DD RID: 8925 RVA: 0x000C2838 File Offset: 0x000C0A38
	// (remove) Token: 0x060022DE RID: 8926 RVA: 0x000C2870 File Offset: 0x000C0A70
	public event Action<SaveLoadRoot> onRegister;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060022DF RID: 8927 RVA: 0x000C28A8 File Offset: 0x000C0AA8
	// (remove) Token: 0x060022E0 RID: 8928 RVA: 0x000C28E0 File Offset: 0x000C0AE0
	public event Action<SaveLoadRoot> onUnregister;

	// Token: 0x060022E1 RID: 8929 RVA: 0x000C2915 File Offset: 0x000C0B15
	protected override void OnPrefabInit()
	{
		Assets.RegisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000C2928 File Offset: 0x000C0B28
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Assets.UnregisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000C2944 File Offset: 0x000C0B44
	private void OnAddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		Tag saveLoadTag = prefab.GetSaveLoadTag();
		this.prefabMap[saveLoadTag] = prefab.gameObject;
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000C2974 File Offset: 0x000C0B74
	public Dictionary<Tag, List<SaveLoadRoot>> GetLists()
	{
		return this.sceneObjects;
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000C297C File Offset: 0x000C0B7C
	private List<SaveLoadRoot> GetSaveLoadRootList(SaveLoadRoot saver)
	{
		KPrefabID component = saver.GetComponent<KPrefabID>();
		if (component == null)
		{
			DebugUtil.LogErrorArgs(saver.gameObject, new object[]
			{
				"All savers must also have a KPrefabID on them but",
				saver.gameObject.name,
				"does not have one."
			});
			return null;
		}
		List<SaveLoadRoot> list;
		if (!this.sceneObjects.TryGetValue(component.GetSaveLoadTag(), out list))
		{
			list = new List<SaveLoadRoot>();
			this.sceneObjects[component.GetSaveLoadTag()] = list;
		}
		return list;
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000C29F8 File Offset: 0x000C0BF8
	public void Register(SaveLoadRoot root)
	{
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Add(root);
		if (this.onRegister != null)
		{
			this.onRegister(root);
		}
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000C2A2C File Offset: 0x000C0C2C
	public void Unregister(SaveLoadRoot root)
	{
		if (this.onRegister != null)
		{
			this.onUnregister(root);
		}
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Remove(root);
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000C2A64 File Offset: 0x000C0C64
	public GameObject GetPrefab(Tag tag)
	{
		GameObject result = null;
		if (this.prefabMap.TryGetValue(tag, out result))
		{
			return result;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Item not found in prefabMap",
			"[" + tag.Name + "]"
		});
		return null;
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000C2AB4 File Offset: 0x000C0CB4
	public void Save(BinaryWriter writer)
	{
		writer.Write(SaveManager.SAVE_HEADER);
		writer.Write(7);
		writer.Write(35);
		int num = 0;
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			if (keyValuePair.Value.Count > 0)
			{
				num++;
			}
		}
		writer.Write(num);
		this.orderedKeys.Clear();
		this.orderedKeys.AddRange(this.sceneObjects.Keys);
		this.orderedKeys.Remove(SaveGame.Instance.PrefabID());
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name == "StickerBomb"
		select a).ToList<Tag>();
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name.Contains("UnderConstruction")
		select a).ToList<Tag>();
		this.Write(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot>(new SaveLoadRoot[]
		{
			SaveGame.Instance.GetComponent<SaveLoadRoot>()
		}), writer);
		foreach (Tag key in this.orderedKeys)
		{
			List<SaveLoadRoot> list = this.sceneObjects[key];
			if (list.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot in list)
				{
					if (!(saveLoadRoot == null) && saveLoadRoot.GetComponent<SimCellOccupier>() != null)
					{
						this.Write(key, list, writer);
						break;
					}
				}
			}
		}
		foreach (Tag key2 in this.orderedKeys)
		{
			List<SaveLoadRoot> list2 = this.sceneObjects[key2];
			if (list2.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot2 in list2)
				{
					if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<SimCellOccupier>() == null)
					{
						this.Write(key2, list2, writer);
						break;
					}
				}
			}
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000C2D68 File Offset: 0x000C0F68
	private void Write(Tag key, List<SaveLoadRoot> value, BinaryWriter writer)
	{
		int count = value.Count;
		Tag tag = key;
		writer.WriteKleiString(tag.Name);
		writer.Write(count);
		long position = writer.BaseStream.Position;
		int value2 = -1;
		writer.Write(value2);
		long position2 = writer.BaseStream.Position;
		foreach (SaveLoadRoot saveLoadRoot in value)
		{
			if (saveLoadRoot != null)
			{
				saveLoadRoot.Save(writer);
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Null game object when saving"
				});
			}
		}
		long position3 = writer.BaseStream.Position;
		long num = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000C2E50 File Offset: 0x000C1050
	public bool Load(IReader reader)
	{
		char[] array = reader.ReadChars(SaveManager.SAVE_HEADER.Length);
		if (array == null || array.Length != SaveManager.SAVE_HEADER.Length)
		{
			return false;
		}
		for (int i = 0; i < SaveManager.SAVE_HEADER.Length; i++)
		{
			if (array[i] != SaveManager.SAVE_HEADER[i])
			{
				return false;
			}
		}
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		if (num != 7 || num2 > 35)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("SAVE FILE VERSION MISMATCH! Expected {0}.{1} but got {2}.{3}", new object[]
				{
					7,
					35,
					num,
					num2
				})
			});
			return false;
		}
		this.ClearScene();
		try
		{
			int num3 = reader.ReadInt32();
			for (int j = 0; j < num3; j++)
			{
				string text = reader.ReadKleiString();
				int num4 = reader.ReadInt32();
				int length = reader.ReadInt32();
				Tag key = TagManager.Create(text);
				GameObject prefab;
				if (!this.prefabMap.TryGetValue(key, out prefab))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not find prefab '" + text + "'"
					});
					reader.SkipBytes(length);
				}
				else
				{
					List<SaveLoadRoot> value = new List<SaveLoadRoot>(num4);
					this.sceneObjects[key] = value;
					for (int k = 0; k < num4; k++)
					{
						SaveLoadRoot x = SaveLoadRoot.Load(prefab, reader);
						if (SaveManager.DEBUG_OnlyLoadThisCellsObjects == -1 && x == null)
						{
							global::Debug.LogError("Error loading data [" + text + "]");
							return false;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Error deserializing prefabs\n\n",
				ex.ToString()
			});
			throw ex;
		}
		return true;
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000C3014 File Offset: 0x000C1214
	private void ClearScene()
	{
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			foreach (SaveLoadRoot saveLoadRoot in keyValuePair.Value)
			{
				UnityEngine.Object.Destroy(saveLoadRoot.gameObject);
			}
		}
		this.sceneObjects.Clear();
	}

	// Token: 0x040013AF RID: 5039
	public const int SAVE_MAJOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x040013B0 RID: 5040
	public const int SAVE_MAJOR_VERSION = 7;

	// Token: 0x040013B1 RID: 5041
	public const int SAVE_MINOR_VERSION_EXPLICIT_VALUE_TYPES = 4;

	// Token: 0x040013B2 RID: 5042
	public const int SAVE_MINOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x040013B3 RID: 5043
	public const int SAVE_MINOR_VERSION_MOD_IDENTIFIER = 8;

	// Token: 0x040013B4 RID: 5044
	public const int SAVE_MINOR_VERSION_FINITE_SPACE_RESOURCES = 9;

	// Token: 0x040013B5 RID: 5045
	public const int SAVE_MINOR_VERSION_COLONY_REQ_ACHIEVEMENTS = 10;

	// Token: 0x040013B6 RID: 5046
	public const int SAVE_MINOR_VERSION_TRACK_NAV_DISTANCE = 11;

	// Token: 0x040013B7 RID: 5047
	public const int SAVE_MINOR_VERSION_EXPANDED_WORLD_INFO = 12;

	// Token: 0x040013B8 RID: 5048
	public const int SAVE_MINOR_VERSION_BASIC_COMFORTS_FIX = 13;

	// Token: 0x040013B9 RID: 5049
	public const int SAVE_MINOR_VERSION_PLATFORM_TRAIT_NAMES = 14;

	// Token: 0x040013BA RID: 5050
	public const int SAVE_MINOR_VERSION_ADD_JOY_REACTIONS = 15;

	// Token: 0x040013BB RID: 5051
	public const int SAVE_MINOR_VERSION_NEW_AUTOMATION_WARNING = 16;

	// Token: 0x040013BC RID: 5052
	public const int SAVE_MINOR_VERSION_ADD_GUID_TO_HEADER = 17;

	// Token: 0x040013BD RID: 5053
	public const int SAVE_MINOR_VERSION_EXPANSION_1_INTRODUCED = 20;

	// Token: 0x040013BE RID: 5054
	public const int SAVE_MINOR_VERSION_CONTENT_SETTINGS = 21;

	// Token: 0x040013BF RID: 5055
	public const int SAVE_MINOR_VERSION_COLONY_REQ_REMOVE_SERIALIZATION = 22;

	// Token: 0x040013C0 RID: 5056
	public const int SAVE_MINOR_VERSION_ROTTABLE_TUNING = 23;

	// Token: 0x040013C1 RID: 5057
	public const int SAVE_MINOR_VERSION_LAUNCH_PAD_SOLIDITY = 24;

	// Token: 0x040013C2 RID: 5058
	public const int SAVE_MINOR_VERSION_BASE_GAME_MERGEDOWN = 25;

	// Token: 0x040013C3 RID: 5059
	public const int SAVE_MINOR_VERSION_FALLING_WATER_WORLDIDX_SERIALIZATION = 26;

	// Token: 0x040013C4 RID: 5060
	public const int SAVE_MINOR_VERSION_ROCKET_RANGE_REBALANCE = 27;

	// Token: 0x040013C5 RID: 5061
	public const int SAVE_MINOR_VERSION_ENTITIES_WRONG_LAYER = 28;

	// Token: 0x040013C6 RID: 5062
	public const int SAVE_MINOR_VERSION_TAGBITS_REWORK = 29;

	// Token: 0x040013C7 RID: 5063
	public const int SAVE_MINOR_VERSION_ACCESSORY_SLOT_UPGRADE = 30;

	// Token: 0x040013C8 RID: 5064
	public const int SAVE_MINOR_VERSION_GEYSER_CAN_BE_RENAMED = 31;

	// Token: 0x040013C9 RID: 5065
	public const int SAVE_MINOR_VERSION_SPACE_SCANNERS_TELESCOPES = 32;

	// Token: 0x040013CA RID: 5066
	public const int SAVE_MINOR_VERSION_U50_CRITTERS = 33;

	// Token: 0x040013CB RID: 5067
	public const int SAVE_MINOR_VERSION_DLC_ADD_ONS = 34;

	// Token: 0x040013CC RID: 5068
	public const int SAVE_MINOR_VERSION_U53_SCHEDULES = 35;

	// Token: 0x040013CD RID: 5069
	public const int SAVE_MINOR_VERSION = 35;

	// Token: 0x040013CE RID: 5070
	private Dictionary<Tag, GameObject> prefabMap = new Dictionary<Tag, GameObject>();

	// Token: 0x040013CF RID: 5071
	private Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = new Dictionary<Tag, List<SaveLoadRoot>>();

	// Token: 0x040013D2 RID: 5074
	public static int DEBUG_OnlyLoadThisCellsObjects = -1;

	// Token: 0x040013D3 RID: 5075
	private static readonly char[] SAVE_HEADER = new char[]
	{
		'K',
		'S',
		'A',
		'V'
	};

	// Token: 0x040013D4 RID: 5076
	private List<Tag> orderedKeys = new List<Tag>();

	// Token: 0x020013B0 RID: 5040
	private enum BoundaryTag : uint
	{
		// Token: 0x04006791 RID: 26513
		Component = 3735928559U,
		// Token: 0x04006792 RID: 26514
		Prefab = 3131961357U,
		// Token: 0x04006793 RID: 26515
		Complete = 3735929054U
	}
}
