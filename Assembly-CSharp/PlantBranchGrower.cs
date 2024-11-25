using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020009F6 RID: 2550
public class PlantBranchGrower : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>
{
	// Token: 0x060049CE RID: 18894 RVA: 0x001A6228 File Offset: 0x001A4428
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.wilt;
		this.worldgen.Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.WorldGenUpdate), UpdateRate.RENDER_EVERY_TICK, false);
		this.wilt.TagTransition(GameTags.Wilting, this.maturing, true);
		this.maturing.TagTransition(GameTags.Wilting, this.wilt, false).EnterTransition(this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature)).EventTransition(GameHashes.Grow, this.growingBranches, null);
		this.growingBranches.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.fullyGrown, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.AllBranchesCreated)).ToggleStatusItem((PlantBranchGrower.Instance smi) => smi.def.growingBranchesStatusItem, null).Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.GrowBranchUpdate), UpdateRate.SIM_4000ms, false);
		this.fullyGrown.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.NotAllBranchesCreated));
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x001A6398 File Offset: 0x001A4598
	public static bool NotAllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount < smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x001A63A8 File Offset: 0x001A45A8
	public static bool AllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount >= smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x001A63BB File Offset: 0x001A45BB
	public static bool IsMature(PlantBranchGrower.Instance smi)
	{
		return smi.IsGrown;
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x001A63C3 File Offset: 0x001A45C3
	public static void GrowBranchUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		smi.SpawnRandomBranch(0f);
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x001A63D4 File Offset: 0x001A45D4
	public static void WorldGenUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		float growth_percentage = UnityEngine.Random.Range(0f, 1f);
		if (!smi.SpawnRandomBranch(growth_percentage))
		{
			smi.GoTo(smi.sm.defaultState);
		}
	}

	// Token: 0x0400306C RID: 12396
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State worldgen;

	// Token: 0x0400306D RID: 12397
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State wilt;

	// Token: 0x0400306E RID: 12398
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State maturing;

	// Token: 0x0400306F RID: 12399
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State growingBranches;

	// Token: 0x04003070 RID: 12400
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State fullyGrown;

	// Token: 0x02001A06 RID: 6662
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007B0E RID: 31502
		public string BRANCH_PREFAB_NAME;

		// Token: 0x04007B0F RID: 31503
		public int MAX_BRANCH_COUNT = -1;

		// Token: 0x04007B10 RID: 31504
		public CellOffset[] BRANCH_OFFSETS;

		// Token: 0x04007B11 RID: 31505
		public bool harvestOnDrown;

		// Token: 0x04007B12 RID: 31506
		public bool propagateHarvestDesignation = true;

		// Token: 0x04007B13 RID: 31507
		public Func<int, bool> additionalBranchGrowRequirements;

		// Token: 0x04007B14 RID: 31508
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested;

		// Token: 0x04007B15 RID: 31509
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned;

		// Token: 0x04007B16 RID: 31510
		public StatusItem growingBranchesStatusItem = Db.Get().MiscStatusItems.GrowingBranches;

		// Token: 0x04007B17 RID: 31511
		public Action<PlantBranchGrower.Instance> onEarlySpawn;
	}

	// Token: 0x02001A07 RID: 6663
	public new class Instance : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.GameInstance
	{
		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06009EC8 RID: 40648 RVA: 0x0037A071 File Offset: 0x00378271
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06009EC9 RID: 40649 RVA: 0x0037A08E File Offset: 0x0037828E
		public bool IsGrown
		{
			get
			{
				return this.growing == null || this.growing.PercentGrown() >= 1f;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06009ECA RID: 40650 RVA: 0x0037A0AF File Offset: 0x003782AF
		public int MaxBranchesAllowedAtOnce
		{
			get
			{
				if (base.def.MAX_BRANCH_COUNT >= 0)
				{
					return Mathf.Min(base.def.MAX_BRANCH_COUNT, base.def.BRANCH_OFFSETS.Length);
				}
				return base.def.BRANCH_OFFSETS.Length;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06009ECB RID: 40651 RVA: 0x0037A0EC File Offset: 0x003782EC
		public int CurrentBranchCount
		{
			get
			{
				int num = 0;
				if (this.branches != null)
				{
					int i = 0;
					while (i < this.branches.Length)
					{
						num += ((this.GetBranch(i++) != null) ? 1 : 0);
					}
				}
				return num;
			}
		}

		// Token: 0x06009ECC RID: 40652 RVA: 0x0037A130 File Offset: 0x00378330
		public GameObject GetBranch(int idx)
		{
			if (this.branches != null && this.branches[idx] != null)
			{
				KPrefabID kprefabID = this.branches[idx].Get();
				if (kprefabID != null)
				{
					return kprefabID.gameObject;
				}
			}
			return null;
		}

		// Token: 0x06009ECD RID: 40653 RVA: 0x0037A16E File Offset: 0x0037836E
		protected override void OnCleanUp()
		{
			this.SetTrunkOccupyingCellsAsPlant(false);
			base.OnCleanUp();
		}

		// Token: 0x06009ECE RID: 40654 RVA: 0x0037A180 File Offset: 0x00378380
		public Instance(IStateMachineTarget master, PlantBranchGrower.Def def) : base(master, def)
		{
			this.growing = base.GetComponent<IManageGrowingStates>();
			this.growing = ((this.growing != null) ? this.growing : base.gameObject.GetSMI<IManageGrowingStates>());
			this.SetTrunkOccupyingCellsAsPlant(true);
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			base.Subscribe(144050788, new Action<object>(this.OnUpdateRoom));
		}

		// Token: 0x06009ECF RID: 40655 RVA: 0x0037A1F8 File Offset: 0x003783F8
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranchGrower.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.DefineBranchArray();
			base.Subscribe(-216549700, new Action<object>(this.OnUprooted));
			base.Subscribe(-266953818, delegate(object obj)
			{
				this.UpdateAutoHarvestValue(null);
			});
			if (base.def.harvestOnDrown)
			{
				base.Subscribe(-750750377, new Action<object>(this.OnUprooted));
			}
		}

		// Token: 0x06009ED0 RID: 40656 RVA: 0x0037A27C File Offset: 0x0037847C
		private void OnUpdateRoom(object data)
		{
			if (this.branches == null)
			{
				return;
			}
			this.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(144050788, data);
			});
		}

		// Token: 0x06009ED1 RID: 40657 RVA: 0x0037A2B4 File Offset: 0x003784B4
		private void SetTrunkOccupyingCellsAsPlant(bool doSet)
		{
			CellOffset[] occupiedCellsOffsets = base.GetComponent<OccupyArea>().OccupiedCellsOffsets;
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (doSet)
				{
					Grid.Objects[cell2, 5] = base.gameObject;
				}
				else if (Grid.Objects[cell2, 5] == base.gameObject)
				{
					Grid.Objects[cell2, 5] = null;
				}
			}
		}

		// Token: 0x06009ED2 RID: 40658 RVA: 0x0037A334 File Offset: 0x00378534
		private void OnNewGameSpawn(object data)
		{
			this.DefineBranchArray();
			float percentage = 1f;
			if ((double)UnityEngine.Random.value < 0.1)
			{
				percentage = UnityEngine.Random.Range(0.75f, 0.99f);
			}
			else
			{
				this.GoTo(base.sm.worldgen);
			}
			this.growing.OverrideMaturityLevel(percentage);
		}

		// Token: 0x06009ED3 RID: 40659 RVA: 0x0037A390 File Offset: 0x00378590
		public void ManuallyDefineBranchArray(KPrefabID[] _branches)
		{
			this.DefineBranchArray();
			for (int i = 0; i < Mathf.Min(this.branches.Length, _branches.Length); i++)
			{
				KPrefabID kprefabID = _branches[i];
				if (kprefabID != null)
				{
					if (this.branches[i] == null)
					{
						this.branches[i] = new Ref<KPrefabID>();
					}
					this.branches[i].Set(kprefabID);
				}
				else
				{
					this.branches[i] = null;
				}
			}
		}

		// Token: 0x06009ED4 RID: 40660 RVA: 0x0037A3FB File Offset: 0x003785FB
		private void DefineBranchArray()
		{
			if (this.branches == null)
			{
				this.branches = new Ref<KPrefabID>[base.def.BRANCH_OFFSETS.Length];
			}
		}

		// Token: 0x06009ED5 RID: 40661 RVA: 0x0037A420 File Offset: 0x00378620
		public void ActionPerBranch(Action<GameObject> action)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && action != null)
				{
					action(branch.gameObject);
				}
			}
		}

		// Token: 0x06009ED6 RID: 40662 RVA: 0x0037A460 File Offset: 0x00378660
		public GameObject[] GetExistingBranches()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					list.Add(branch.gameObject);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06009ED7 RID: 40663 RVA: 0x0037A4AC File Offset: 0x003786AC
		public void OnBranchRemoved(GameObject _branch)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && branch == _branch)
				{
					this.branches[i] = null;
				}
			}
			base.gameObject.Trigger(-1586842875, null);
		}

		// Token: 0x06009ED8 RID: 40664 RVA: 0x0037A500 File Offset: 0x00378700
		public void OnBrancHarvested(PlantBranch.Instance branch)
		{
			Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested = base.def.onBranchHarvested;
			if (onBranchHarvested == null)
			{
				return;
			}
			onBranchHarvested(branch, this);
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x0037A51C File Offset: 0x0037871C
		private void OnUprooted(object data = null)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					branch.Trigger(-216549700, null);
				}
			}
		}

		// Token: 0x06009EDA RID: 40666 RVA: 0x0037A55C File Offset: 0x0037875C
		public List<int> GetAvailableSpawnPositions()
		{
			PlantBranchGrower.Instance.spawn_choices.Clear();
			int cell = Grid.PosToCell(this);
			for (int i = 0; i < base.def.BRANCH_OFFSETS.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, base.def.BRANCH_OFFSETS[i]);
				if (this.GetBranch(i) == null && this.CanBranchGrowInCell(cell2))
				{
					PlantBranchGrower.Instance.spawn_choices.Add(i);
				}
			}
			return PlantBranchGrower.Instance.spawn_choices;
		}

		// Token: 0x06009EDB RID: 40667 RVA: 0x0037A5D4 File Offset: 0x003787D4
		public void RefreshBranchZPositionOffset(GameObject _branch)
		{
			if (this.branches != null)
			{
				for (int i = 0; i < this.branches.Length; i++)
				{
					GameObject branch = this.GetBranch(i);
					if (branch != null && branch == _branch)
					{
						Vector3 position = branch.transform.position;
						position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.8f / (float)this.branches.Length * (float)i;
						branch.transform.SetPosition(position);
					}
				}
			}
		}

		// Token: 0x06009EDC RID: 40668 RVA: 0x0037A650 File Offset: 0x00378850
		public bool SpawnRandomBranch(float growth_percentage = 0f)
		{
			if (this.IsUprooted)
			{
				return false;
			}
			if (this.CurrentBranchCount >= this.MaxBranchesAllowedAtOnce)
			{
				return false;
			}
			List<int> availableSpawnPositions = this.GetAvailableSpawnPositions();
			availableSpawnPositions.Shuffle<int>();
			if (availableSpawnPositions.Count > 0)
			{
				int idx = availableSpawnPositions[0];
				PlantBranch.Instance instance = this.SpawnBranchAtIndex(idx);
				IManageGrowingStates manageGrowingStates = instance.GetComponent<IManageGrowingStates>();
				manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : instance.gameObject.GetSMI<IManageGrowingStates>());
				if (manageGrowingStates != null)
				{
					manageGrowingStates.OverrideMaturityLevel(growth_percentage);
				}
				instance.StartSM();
				base.gameObject.Trigger(-1586842875, instance);
				Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned = base.def.onBranchSpawned;
				if (onBranchSpawned != null)
				{
					onBranchSpawned(instance, this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06009EDD RID: 40669 RVA: 0x0037A6F4 File Offset: 0x003788F4
		private PlantBranch.Instance SpawnBranchAtIndex(int idx)
		{
			if (idx < 0 || idx >= this.branches.Length)
			{
				return null;
			}
			GameObject branch = this.GetBranch(idx);
			if (branch != null)
			{
				return branch.GetSMI<PlantBranch.Instance>();
			}
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), base.def.BRANCH_OFFSETS[idx]), Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			gameObject.SetActive(true);
			PlantBranch.Instance smi = gameObject.GetSMI<PlantBranch.Instance>();
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = smi.GetComponent<MutantPlant>();
				if (component2 != null)
				{
					component.CopyMutationsTo(component2);
					PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
					PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
					PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
				}
			}
			this.UpdateAutoHarvestValue(smi);
			smi.SetTrunk(this);
			this.branches[idx] = new Ref<KPrefabID>();
			this.branches[idx].Set(smi.GetComponent<KPrefabID>());
			return smi;
		}

		// Token: 0x06009EDE RID: 40670 RVA: 0x0037A7F8 File Offset: 0x003789F8
		private bool CanBranchGrowInCell(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			if (Grid.Solid[cell])
			{
				return false;
			}
			if (Grid.Objects[cell, 1] != null)
			{
				return false;
			}
			if (Grid.Objects[cell, 5] != null)
			{
				return false;
			}
			if (Grid.Foundation[cell])
			{
				return false;
			}
			int cell2 = Grid.CellAbove(cell);
			return Grid.IsValidCell(cell2) && !Grid.IsSubstantialLiquid(cell2, 0.35f) && (base.def.additionalBranchGrowRequirements == null || base.def.additionalBranchGrowRequirements(cell));
		}

		// Token: 0x06009EDF RID: 40671 RVA: 0x0037A89C File Offset: 0x00378A9C
		public void UpdateAutoHarvestValue(PlantBranch.Instance specificBranch = null)
		{
			HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
			if (component != null && this.branches != null)
			{
				if (specificBranch != null)
				{
					HarvestDesignatable component2 = specificBranch.GetComponent<HarvestDesignatable>();
					if (component2 != null)
					{
						component2.SetHarvestWhenReady(component.HarvestWhenReady);
					}
					return;
				}
				if (base.def.propagateHarvestDesignation)
				{
					for (int i = 0; i < this.branches.Length; i++)
					{
						GameObject branch = this.GetBranch(i);
						if (branch != null)
						{
							HarvestDesignatable component3 = branch.GetComponent<HarvestDesignatable>();
							if (component3 != null)
							{
								component3.SetHarvestWhenReady(component.HarvestWhenReady);
							}
						}
					}
				}
			}
		}

		// Token: 0x04007B18 RID: 31512
		private IManageGrowingStates growing;

		// Token: 0x04007B19 RID: 31513
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x04007B1A RID: 31514
		[Serialize]
		private Ref<KPrefabID>[] branches;

		// Token: 0x04007B1B RID: 31515
		private static List<int> spawn_choices = new List<int>();
	}
}
