﻿using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200068C RID: 1676
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConfigManager")]
public class BuildingConfigManager : KMonoBehaviour
{
	// Token: 0x060029C6 RID: 10694 RVA: 0x000EB604 File Offset: 0x000E9804
	protected override void OnPrefabInit()
	{
		BuildingConfigManager.Instance = this;
		this.baseTemplate = new GameObject("BuildingTemplate");
		this.baseTemplate.SetActive(false);
		this.baseTemplate.AddComponent<KPrefabID>();
		this.baseTemplate.AddComponent<KSelectable>();
		this.baseTemplate.AddComponent<Modifiers>();
		this.baseTemplate.AddComponent<PrimaryElement>();
		this.baseTemplate.AddComponent<BuildingComplete>();
		this.baseTemplate.AddComponent<StateMachineController>();
		this.baseTemplate.AddComponent<Deconstructable>();
		this.baseTemplate.AddComponent<Reconstructable>();
		this.baseTemplate.AddComponent<SaveLoadRoot>();
		this.baseTemplate.AddComponent<OccupyArea>();
		this.baseTemplate.AddComponent<DecorProvider>();
		this.baseTemplate.AddComponent<Operational>();
		this.baseTemplate.AddComponent<BuildingEnabledButton>();
		this.baseTemplate.AddComponent<Prioritizable>();
		this.baseTemplate.AddComponent<BuildingHP>();
		this.baseTemplate.AddComponent<LoopingSounds>();
		this.baseTemplate.AddComponent<InvalidPortReporter>();
		this.defaultBuildingCompleteKComponents.Add(typeof(RequiresFoundation));
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x000EB715 File Offset: 0x000E9915
	public static string GetUnderConstructionName(string name)
	{
		return name + "UnderConstruction";
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x000EB724 File Offset: 0x000E9924
	public void RegisterBuilding(IBuildingConfig config)
	{
		string[] requiredDlcIds = config.GetRequiredDlcIds();
		string[] forbiddenDlcIds = config.GetForbiddenDlcIds();
		if (config.GetDlcIds() != null)
		{
			DlcManager.ConvertAvailableToRequireAndForbidden(config.GetDlcIds(), out requiredDlcIds, out forbiddenDlcIds);
		}
		if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
		{
			return;
		}
		BuildingDef buildingDef = config.CreateBuildingDef();
		buildingDef.RequiredDlcIds = requiredDlcIds;
		buildingDef.ForbiddenDlcIds = forbiddenDlcIds;
		this.configTable[config] = buildingDef;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.baseTemplate);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
		gameObject.name = buildingDef.PrefabID + "Template";
		gameObject.GetComponent<Building>().Def = buildingDef;
		gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);
		gameObject.AddTag(GameTags.RoomProberBuilding);
		if (buildingDef.Deprecated)
		{
			gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		}
		config.ConfigureBuildingTemplate(gameObject, buildingDef.Tag);
		buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);
		bool flag = true;
		for (int i = 0; i < this.NonBuildableBuildings.Length; i++)
		{
			if (buildingDef.PrefabID == this.NonBuildableBuildings[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
			buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
			buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);
			GameObject buildingPreview = buildingDef.BuildingPreview;
			buildingPreview.name += "Preview";
		}
		buildingDef.PostProcess();
		config.DoPostConfigureComplete(buildingDef.BuildingComplete);
		if (flag)
		{
			config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
			config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
		}
		Assets.AddBuildingDef(buildingDef);
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x000EB8E4 File Offset: 0x000E9AE4
	public void ConfigurePost()
	{
		foreach (KeyValuePair<IBuildingConfig, BuildingDef> keyValuePair in this.configTable)
		{
			keyValuePair.Key.ConfigurePost(keyValuePair.Value);
		}
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x000EB944 File Offset: 0x000E9B44
	public void IgnoreDefaultKComponent(Type type_to_ignore, Tag building_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.ignoredDefaultKComponents.TryGetValue(type_to_ignore, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.ignoredDefaultKComponents[type_to_ignore] = hashSet;
		}
		hashSet.Add(building_tag);
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x000EB97C File Offset: 0x000E9B7C
	private bool IsIgnoredDefaultKComponent(Tag building_tag, Type type)
	{
		bool result = false;
		HashSet<Tag> hashSet;
		if (this.ignoredDefaultKComponents.TryGetValue(type, out hashSet) && hashSet.Contains(building_tag))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x000EB9A8 File Offset: 0x000E9BA8
	public void AddBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Add(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Add(go);
			}
		}
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x000EBA5C File Offset: 0x000E9C5C
	public void DestroyBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Remove(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Remove(go);
			}
		}
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x000EBB10 File Offset: 0x000E9D10
	public void AddDefaultBuildingCompleteKComponent(Type kcomponent_type)
	{
		this.defaultKComponents.Add(kcomponent_type);
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x000EBB20 File Offset: 0x000E9D20
	public void AddBuildingCompleteKComponent(Tag prefab_tag, Type kcomponent_type)
	{
		HashSet<Type> hashSet;
		if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			hashSet = new HashSet<Type>();
			this.buildingCompleteKComponents[prefab_tag] = hashSet;
		}
		hashSet.Add(kcomponent_type);
	}

	// Token: 0x0400180F RID: 6159
	public static BuildingConfigManager Instance;

	// Token: 0x04001810 RID: 6160
	private GameObject baseTemplate;

	// Token: 0x04001811 RID: 6161
	private Dictionary<IBuildingConfig, BuildingDef> configTable = new Dictionary<IBuildingConfig, BuildingDef>();

	// Token: 0x04001812 RID: 6162
	private string[] NonBuildableBuildings = new string[]
	{
		"Headquarters"
	};

	// Token: 0x04001813 RID: 6163
	private HashSet<Type> defaultKComponents = new HashSet<Type>();

	// Token: 0x04001814 RID: 6164
	private HashSet<Type> defaultBuildingCompleteKComponents = new HashSet<Type>();

	// Token: 0x04001815 RID: 6165
	private Dictionary<Type, HashSet<Tag>> ignoredDefaultKComponents = new Dictionary<Type, HashSet<Tag>>();

	// Token: 0x04001816 RID: 6166
	private Dictionary<Tag, HashSet<Type>> buildingCompleteKComponents = new Dictionary<Tag, HashSet<Type>>();
}
