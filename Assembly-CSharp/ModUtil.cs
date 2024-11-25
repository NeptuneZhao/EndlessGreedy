using System;
using System.Collections.Generic;
using System.IO;
using KMod;
using TUNING;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public static class ModUtil
{
	// Token: 0x06000FD8 RID: 4056 RVA: 0x0005A648 File Offset: 0x00058848
	public static void AddBuildingToPlanScreen(HashedString category, string building_id)
	{
		ModUtil.AddBuildingToPlanScreen(category, building_id, "uncategorized");
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0005A656 File Offset: 0x00058856
	public static void AddBuildingToPlanScreen(HashedString category, string building_id, string subcategoryID)
	{
		ModUtil.AddBuildingToPlanScreen(category, building_id, subcategoryID, null, ModUtil.BuildingOrdering.After);
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0005A664 File Offset: 0x00058864
	public static void AddBuildingToPlanScreen(HashedString category, string building_id, string subcategoryID, string relativeBuildingId, ModUtil.BuildingOrdering ordering = ModUtil.BuildingOrdering.After)
	{
		int num = BUILDINGS.PLANORDER.FindIndex((PlanScreen.PlanInfo x) => x.category == category);
		if (num < 0)
		{
			global::Debug.LogWarning(string.Format("Mod: Unable to add '{0}' as category '{1}' does not exist", building_id, category));
			return;
		}
		List<KeyValuePair<string, string>> buildingAndSubcategoryData = BUILDINGS.PLANORDER[num].buildingAndSubcategoryData;
		KeyValuePair<string, string> item = new KeyValuePair<string, string>(building_id, subcategoryID);
		if (relativeBuildingId == null)
		{
			buildingAndSubcategoryData.Add(item);
			return;
		}
		int num2 = buildingAndSubcategoryData.FindIndex((KeyValuePair<string, string> x) => x.Key == relativeBuildingId);
		if (num2 == -1)
		{
			buildingAndSubcategoryData.Add(item);
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Mod: Building '",
				relativeBuildingId,
				"' doesn't exist, inserting '",
				building_id,
				"' at the end of the list instead"
			}));
			return;
		}
		int index = (ordering == ModUtil.BuildingOrdering.After) ? (num2 + 1) : Mathf.Max(num2 - 1, 0);
		buildingAndSubcategoryData.Insert(index, item);
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0005A75C File Offset: 0x0005895C
	[Obsolete("Use PlanScreen instead")]
	public static void AddBuildingToHotkeyBuildMenu(HashedString category, string building_id, global::Action hotkey)
	{
		BuildMenu.DisplayInfo info = BuildMenu.OrderedBuildings.GetInfo(category);
		if (info.category != category)
		{
			return;
		}
		(info.data as IList<BuildMenu.BuildingInfo>).Add(new BuildMenu.BuildingInfo(building_id, hotkey));
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x0005A79C File Offset: 0x0005899C
	public static KAnimFile AddKAnimMod(string name, KAnimFile.Mod anim_mod)
	{
		KAnimFile kanimFile = ScriptableObject.CreateInstance<KAnimFile>();
		kanimFile.mod = anim_mod;
		kanimFile.name = name;
		AnimCommandFile animCommandFile = new AnimCommandFile();
		KAnimGroupFile.GroupFile groupFile = new KAnimGroupFile.GroupFile();
		groupFile.groupID = animCommandFile.GetGroupName(kanimFile);
		groupFile.commandDirectory = "assets/" + name;
		animCommandFile.AddGroupFile(groupFile);
		if (KAnimGroupFile.GetGroupFile().AddAnimMod(groupFile, animCommandFile, kanimFile) == KAnimGroupFile.AddModResult.Added)
		{
			Assets.ModLoadedKAnims.Add(kanimFile);
		}
		return kanimFile;
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0005A80C File Offset: 0x00058A0C
	public static KAnimFile AddKAnim(string name, TextAsset anim_file, TextAsset build_file, IList<Texture2D> textures)
	{
		KAnimFile kanimFile = ScriptableObject.CreateInstance<KAnimFile>();
		kanimFile.Initialize(anim_file, build_file, textures);
		kanimFile.name = name;
		AnimCommandFile animCommandFile = new AnimCommandFile();
		KAnimGroupFile.GroupFile groupFile = new KAnimGroupFile.GroupFile();
		groupFile.groupID = animCommandFile.GetGroupName(kanimFile);
		groupFile.commandDirectory = "assets/" + name;
		animCommandFile.AddGroupFile(groupFile);
		KAnimGroupFile.GetGroupFile().AddAnimFile(groupFile, animCommandFile, kanimFile);
		Assets.ModLoadedKAnims.Add(kanimFile);
		return kanimFile;
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0005A87C File Offset: 0x00058A7C
	public static KAnimFile AddKAnim(string name, TextAsset anim_file, TextAsset build_file, Texture2D texture)
	{
		return ModUtil.AddKAnim(name, anim_file, build_file, new List<Texture2D>
		{
			texture
		});
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x0005A8A0 File Offset: 0x00058AA0
	public static Substance CreateSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour, Color32 ui_colour, Color32 conduit_colour)
	{
		return new Substance
		{
			name = name,
			nameTag = TagManager.Create(name),
			elementID = (SimHashes)Hash.SDBMLower(name),
			anim = kanim,
			colour = colour,
			uiColour = ui_colour,
			conduitColour = conduit_colour,
			material = material,
			renderedByWorld = ((state & Element.State.Solid) == Element.State.Solid)
		};
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x0005A903 File Offset: 0x00058B03
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Localization.RegisterForTranslation(locstring_tree_root);
		Localization.GenerateStringsTemplate(locstring_tree_root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
	}

	// Token: 0x0200111B RID: 4379
	public enum BuildingOrdering
	{
		// Token: 0x04005F20 RID: 24352
		Before,
		// Token: 0x04005F21 RID: 24353
		After
	}
}
