﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200083A RID: 2106
[Serializable]
public class Def : ScriptableObject
{
	// Token: 0x06003ADD RID: 15069 RVA: 0x00143119 File Offset: 0x00141319
	public virtual void InitDef()
	{
		this.Tag = TagManager.Create(this.PrefabID);
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06003ADE RID: 15070 RVA: 0x0014312C File Offset: 0x0014132C
	public virtual string Name
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x00143130 File Offset: 0x00141330
	public static global::Tuple<Sprite, Color> GetUISprite(object item, string animName = "ui", bool centered = false)
	{
		if (item is Substance)
		{
			return Def.GetUISprite(ElementLoader.FindElementByHash((item as Substance).elementID), animName, centered);
		}
		if (item is Element)
		{
			if ((item as Element).IsSolid)
			{
				return new global::Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim((item as Element).substance.anim, animName, centered, ""), Color.white);
			}
			if ((item as Element).IsLiquid)
			{
				return new global::Tuple<Sprite, Color>(Assets.GetSprite("element_liquid"), (item as Element).substance.uiColour);
			}
			if ((item as Element).IsGas)
			{
				return new global::Tuple<Sprite, Color>(Assets.GetSprite("element_gas"), (item as Element).substance.uiColour);
			}
			return new global::Tuple<Sprite, Color>(Assets.GetSprite("unknown_far"), Color.black);
		}
		else
		{
			if (item is AsteroidGridEntity)
			{
				return new global::Tuple<Sprite, Color>(((AsteroidGridEntity)item).GetUISprite(), Color.white);
			}
			if (item is GameObject)
			{
				GameObject gameObject = item as GameObject;
				if (ElementLoader.GetElement(gameObject.PrefabID()) != null)
				{
					return Def.GetUISprite(ElementLoader.GetElement(gameObject.PrefabID()), animName, centered);
				}
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				CreatureBrain component2 = gameObject.GetComponent<CreatureBrain>();
				if (component2 != null)
				{
					animName = component2.symbolPrefix + "ui";
				}
				SpaceArtifact component3 = gameObject.GetComponent<SpaceArtifact>();
				if (component3 != null)
				{
					animName = component3.GetUIAnim();
				}
				if (component.HasTag(GameTags.Egg))
				{
					IncubationMonitor.Def def = gameObject.GetDef<IncubationMonitor.Def>();
					if (def != null)
					{
						GameObject prefab = Assets.GetPrefab(def.spawnedCreature);
						if (prefab)
						{
							component2 = prefab.GetComponent<CreatureBrain>();
							if (component2 && !string.IsNullOrEmpty(component2.symbolPrefix))
							{
								animName = component2.symbolPrefix + animName;
							}
						}
					}
				}
				if (component.HasTag(GameTags.MoltShell))
				{
					animName = gameObject.GetComponent<SimpleMassStatusItem>().symbolPrefix + animName;
				}
				if (component.HasTag(GameTags.BionicUpgrade))
				{
					animName = BionicUpgradeComponentConfig.UpgradesData[component.PrefabID()].uiAnimName;
				}
				KBatchedAnimController component4 = gameObject.GetComponent<KBatchedAnimController>();
				if (component4)
				{
					Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component4.AnimFiles[0], animName, centered, "");
					return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
				}
				if (gameObject.GetComponent<Building>() != null)
				{
					Sprite uisprite = gameObject.GetComponent<Building>().Def.GetUISprite(animName, centered);
					return new global::Tuple<Sprite, Color>(uisprite, (uisprite != null) ? Color.white : Color.clear);
				}
				global::Debug.LogWarningFormat("Can't get sprite for type {0} (no KBatchedAnimController)", new object[]
				{
					item.ToString()
				});
				return new global::Tuple<Sprite, Color>(Assets.GetSprite("unknown"), Color.grey);
			}
			else
			{
				if (!(item is string))
				{
					if (item is Tag)
					{
						if (ElementLoader.GetElement((Tag)item) != null)
						{
							return Def.GetUISprite(ElementLoader.GetElement((Tag)item), animName, centered);
						}
						if (Assets.GetPrefab((Tag)item) != null)
						{
							return Def.GetUISprite(Assets.GetPrefab((Tag)item), animName, centered);
						}
						if (Assets.GetSprite(((Tag)item).Name) != null)
						{
							return new global::Tuple<Sprite, Color>(Assets.GetSprite(((Tag)item).Name), Color.white);
						}
					}
					DebugUtil.DevAssertArgs(false, new object[]
					{
						"Can't get sprite for type ",
						item.ToString()
					});
					return new global::Tuple<Sprite, Color>(Assets.GetSprite("unknown"), Color.grey);
				}
				if (Db.Get().Amounts.Exists(item as string))
				{
					return new global::Tuple<Sprite, Color>(Assets.GetSprite(Db.Get().Amounts.Get(item as string).uiSprite), Color.white);
				}
				if (Db.Get().Attributes.Exists(item as string))
				{
					return new global::Tuple<Sprite, Color>(Assets.GetSprite(Db.Get().Attributes.Get(item as string).uiSprite), Color.white);
				}
				return Def.GetUISprite((item as string).ToTag(), animName, centered);
			}
		}
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0014357E File Offset: 0x0014177E
	public static global::Tuple<Sprite, Color> GetUISprite(Tag prefabID, string facadeID)
	{
		if (Assets.GetPrefab(prefabID).GetComponent<Equippable>() != null && !facadeID.IsNullOrWhiteSpace())
		{
			return Db.GetEquippableFacades().Get(facadeID).GetUISprite();
		}
		return Def.GetUISprite(prefabID, "ui", false);
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x001435BD File Offset: 0x001417BD
	public static Sprite GetFacadeUISprite(string facadeID)
	{
		return Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(Db.GetBuildingFacades().Get(facadeID).AnimFile), "ui", false, "");
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x001435EC File Offset: 0x001417EC
	public static Sprite GetUISpriteFromMultiObjectAnim(KAnimFile animFile, string animName = "ui", bool centered = false, string symbolName = "")
	{
		global::Tuple<KAnimFile, string, bool> key = new global::Tuple<KAnimFile, string, bool>(animFile, animName, centered);
		if (Def.knownUISprites.ContainsKey(key))
		{
			return Def.knownUISprites[key];
		}
		if (animFile == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				animName,
				"missing Anim File"
			});
			return Assets.GetSprite("unknown");
		}
		KAnimFileData data = animFile.GetData();
		if (data == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				animName,
				"KAnimFileData is null"
			});
			return Assets.GetSprite("unknown");
		}
		if (data.build == null)
		{
			return Assets.GetSprite("unknown");
		}
		if (string.IsNullOrEmpty(symbolName))
		{
			symbolName = animName;
		}
		KAnimHashedString symbol_name = new KAnimHashedString(symbolName);
		KAnim.Build.Symbol symbol = data.build.GetSymbol(symbol_name);
		if (symbol == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				animFile.name,
				animName,
				"placeSymbol [",
				symbolName,
				"] is missing"
			});
			return Assets.GetSprite("unknown");
		}
		int frame = 0;
		KAnim.Build.SymbolFrameInstance frame2 = symbol.GetFrame(frame);
		Texture2D texture = data.build.GetTexture(0);
		global::Debug.Assert(texture != null, "Invalid texture on " + animFile.name);
		float x = frame2.uvMin.x;
		float x2 = frame2.uvMax.x;
		float y = frame2.uvMax.y;
		float y2 = frame2.uvMin.y;
		int num = (int)((float)texture.width * Mathf.Abs(x2 - x));
		int num2 = (int)((float)texture.height * Mathf.Abs(y2 - y));
		float num3 = Mathf.Abs(frame2.bboxMax.x - frame2.bboxMin.x);
		Rect rect = default(Rect);
		rect.width = (float)num;
		rect.height = (float)num2;
		rect.x = (float)((int)((float)texture.width * x));
		rect.y = (float)((int)((float)texture.height * y));
		float pixelsPerUnit = 100f;
		if (num != 0)
		{
			pixelsPerUnit = 100f / (num3 / (float)num);
		}
		Sprite sprite = Sprite.Create(texture, rect, centered ? new Vector2(0.5f, 0.5f) : Vector2.zero, pixelsPerUnit, 0U, SpriteMeshType.FullRect);
		sprite.name = string.Format("{0}:{1}:{2}", texture.name, animName, centered);
		Def.knownUISprites[key] = sprite;
		return sprite;
	}

	// Token: 0x06003AE3 RID: 15075 RVA: 0x00143864 File Offset: 0x00141A64
	public static KAnimFile GetAnimFileFromPrefabWithTag(GameObject prefab, string desiredAnimName, out string animName)
	{
		animName = desiredAnimName;
		if (prefab == null)
		{
			return null;
		}
		CreatureBrain component = prefab.GetComponent<CreatureBrain>();
		if (component != null)
		{
			animName = component.symbolPrefix + animName;
		}
		SpaceArtifact component2 = prefab.GetComponent<SpaceArtifact>();
		if (component2 != null)
		{
			animName = component2.GetUIAnim();
		}
		if (prefab.HasTag(GameTags.Egg))
		{
			IncubationMonitor.Def def = prefab.GetDef<IncubationMonitor.Def>();
			if (def != null)
			{
				GameObject prefab2 = Assets.GetPrefab(def.spawnedCreature);
				if (prefab2)
				{
					component = prefab2.GetComponent<CreatureBrain>();
					if (component && !string.IsNullOrEmpty(component.symbolPrefix))
					{
						animName = component.symbolPrefix + animName;
					}
				}
			}
		}
		if (prefab.HasTag(GameTags.MoltShell))
		{
			SimpleMassStatusItem component3 = prefab.GetComponent<SimpleMassStatusItem>();
			animName = component3.symbolPrefix + animName;
		}
		return prefab.GetComponent<KBatchedAnimController>().AnimFiles[0];
	}

	// Token: 0x06003AE4 RID: 15076 RVA: 0x0014393E File Offset: 0x00141B3E
	public static KAnimFile GetAnimFileFromPrefabWithTag(Tag prefabID, string desiredAnimName, out string animName)
	{
		return Def.GetAnimFileFromPrefabWithTag(Assets.GetPrefab(prefabID), desiredAnimName, out animName);
	}

	// Token: 0x040023C4 RID: 9156
	public string PrefabID;

	// Token: 0x040023C5 RID: 9157
	public Tag Tag;

	// Token: 0x040023C6 RID: 9158
	private static Dictionary<global::Tuple<KAnimFile, string, bool>, Sprite> knownUISprites = new Dictionary<global::Tuple<KAnimFile, string, bool>, Sprite>();

	// Token: 0x040023C7 RID: 9159
	public const string DEFAULT_SPRITE = "unknown";
}
