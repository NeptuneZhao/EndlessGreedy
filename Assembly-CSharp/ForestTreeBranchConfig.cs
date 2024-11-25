﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class ForestTreeBranchConfig : IEntityConfig
{
	// Token: 0x0600070C RID: 1804 RVA: 0x0002EC1F File Offset: 0x0002CE1F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0002EC28 File Offset: 0x0002CE28
	public GameObject CreatePrefab()
	{
		string id = "ForestTreeBranch";
		string name = STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
		float mass = 8f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("tree_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 1;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.HideFromSpawnTool,
			GameTags.PlantBranch
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 288.15f, 313.15f, 448.15f, null, true, 0f, 0.15f, "WoodLog", true, true, false, true, 12000f, 0f, 9800f, "ForestTreeBranchOriginal", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
		gameObject.AddOrGet<TreeBud>();
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<BudUprootedMonitor>();
		PlantBranch.Def def = gameObject.AddOrGetDef<PlantBranch.Def>();
		def.preventStartSMIOnSpawn = true;
		def.onEarlySpawn = new Action<PlantBranch.Instance>(this.TranslateOldTrunkToNewSystem);
		def.animationSetupCallback = new Action<PlantBranchGrower.Instance, PlantBranch.Instance>(this.AdjustAnimation);
		return gameObject;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0002ED3C File Offset: 0x0002CF3C
	public void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch)
	{
		int base_cell = Grid.PosToCell(trunk);
		int offset_cell = Grid.PosToCell(branch);
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		StandardCropPlant component = branch.GetComponent<StandardCropPlant>();
		KBatchedAnimController component2 = branch.GetComponent<KBatchedAnimController>();
		component.anims = ForestTreeBranchConfig.animationSets[offset];
		component2.Offset = ForestTreeBranchConfig.animOffset[offset];
		component2.Play(component.anims.grow, KAnim.PlayMode.Paused, 1f, 0f);
		component.RefreshPositionPercent();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002EDB4 File Offset: 0x0002CFB4
	public void TranslateOldTrunkToNewSystem(PlantBranch.Instance smi)
	{
		BuddingTrunk andForgetOldTrunk = smi.GetComponent<TreeBud>().GetAndForgetOldTrunk();
		if (andForgetOldTrunk != null)
		{
			PlantBranchGrower.Instance smi2 = andForgetOldTrunk.GetSMI<PlantBranchGrower.Instance>();
			smi.SetTrunk(smi2);
		}
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0002EDE4 File Offset: 0x0002CFE4
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002EE00 File Offset: 0x0002D000
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002EE0C File Offset: 0x0002D00C
	// Note: this type is marked as 'beforefieldinit'.
	static ForestTreeBranchConfig()
	{
		Dictionary<CellOffset, StandardCropPlant.AnimSet> dictionary = new Dictionary<CellOffset, StandardCropPlant.AnimSet>();
		CellOffset key = new CellOffset(-1, 0);
		dictionary[key] = new StandardCropPlant.AnimSet
		{
			grow = "branch_a_grow",
			grow_pst = "branch_a_grow_pst",
			idle_full = "branch_a_idle_full",
			wilt_base = "branch_a_wilt",
			harvest = "branch_a_harvest"
		};
		CellOffset key2 = new CellOffset(-1, 1);
		dictionary[key2] = new StandardCropPlant.AnimSet
		{
			grow = "branch_b_grow",
			grow_pst = "branch_b_grow_pst",
			idle_full = "branch_b_idle_full",
			wilt_base = "branch_b_wilt",
			harvest = "branch_b_harvest"
		};
		CellOffset key3 = new CellOffset(-1, 2);
		dictionary[key3] = new StandardCropPlant.AnimSet
		{
			grow = "branch_c_grow",
			grow_pst = "branch_c_grow_pst",
			idle_full = "branch_c_idle_full",
			wilt_base = "branch_c_wilt",
			harvest = "branch_c_harvest"
		};
		CellOffset key4 = new CellOffset(0, 2);
		dictionary[key4] = new StandardCropPlant.AnimSet
		{
			grow = "branch_d_grow",
			grow_pst = "branch_d_grow_pst",
			idle_full = "branch_d_idle_full",
			wilt_base = "branch_d_wilt",
			harvest = "branch_d_harvest"
		};
		CellOffset key5 = new CellOffset(1, 2);
		dictionary[key5] = new StandardCropPlant.AnimSet
		{
			grow = "branch_e_grow",
			grow_pst = "branch_e_grow_pst",
			idle_full = "branch_e_idle_full",
			wilt_base = "branch_e_wilt",
			harvest = "branch_e_harvest"
		};
		CellOffset key6 = new CellOffset(1, 1);
		dictionary[key6] = new StandardCropPlant.AnimSet
		{
			grow = "branch_f_grow",
			grow_pst = "branch_f_grow_pst",
			idle_full = "branch_f_idle_full",
			wilt_base = "branch_f_wilt",
			harvest = "branch_f_harvest"
		};
		CellOffset key7 = new CellOffset(1, 0);
		dictionary[key7] = new StandardCropPlant.AnimSet
		{
			grow = "branch_g_grow",
			grow_pst = "branch_g_grow_pst",
			idle_full = "branch_g_idle_full",
			wilt_base = "branch_g_wilt",
			harvest = "branch_g_harvest"
		};
		ForestTreeBranchConfig.animationSets = dictionary;
		Dictionary<CellOffset, Vector3> dictionary2 = new Dictionary<CellOffset, Vector3>();
		key7 = new CellOffset(-1, 0);
		dictionary2[key7] = new Vector3(1f, 0f, 0f);
		key6 = new CellOffset(-1, 1);
		dictionary2[key6] = new Vector3(1f, -1f, 0f);
		key5 = new CellOffset(-1, 2);
		dictionary2[key5] = new Vector3(1f, -2f, 0f);
		key4 = new CellOffset(0, 2);
		dictionary2[key4] = new Vector3(0f, -2f, 0f);
		key3 = new CellOffset(1, 2);
		dictionary2[key3] = new Vector3(-1f, -2f, 0f);
		key2 = new CellOffset(1, 1);
		dictionary2[key2] = new Vector3(-1f, -1f, 0f);
		key = new CellOffset(1, 0);
		dictionary2[key] = new Vector3(-1f, 0f, 0f);
		ForestTreeBranchConfig.animOffset = dictionary2;
	}

	// Token: 0x040004FB RID: 1275
	public const string ID = "ForestTreeBranch";

	// Token: 0x040004FC RID: 1276
	public const float WOOD_AMOUNT = 300f;

	// Token: 0x040004FD RID: 1277
	private static Dictionary<CellOffset, StandardCropPlant.AnimSet> animationSets;

	// Token: 0x040004FE RID: 1278
	private static Dictionary<CellOffset, Vector3> animOffset;
}
