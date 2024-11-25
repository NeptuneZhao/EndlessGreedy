using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
public class PlantBranch : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>
{
	// Token: 0x060049CC RID: 18892 RVA: 0x001A620E File Offset: 0x001A440E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x0400306B RID: 12395
	private StateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.TargetParameter Trunk;

	// Token: 0x02001A04 RID: 6660
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007B09 RID: 31497
		public Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback;

		// Token: 0x04007B0A RID: 31498
		public Action<PlantBranch.Instance> onEarlySpawn;
	}

	// Token: 0x02001A05 RID: 6661
	public new class Instance : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06009EB9 RID: 40633 RVA: 0x00379C94 File Offset: 0x00377E94
		public bool HasTrunk
		{
			get
			{
				return this.trunk != null && !this.trunk.IsNullOrDestroyed() && !this.trunk.isMasterNull;
			}
		}

		// Token: 0x06009EBA RID: 40634 RVA: 0x00379CBB File Offset: 0x00377EBB
		public Instance(IStateMachineTarget master, PlantBranch.Def def) : base(master, def)
		{
			this.SetOccupyGridSpace(true);
			base.Subscribe(1272413801, new Action<object>(this.OnHarvest));
		}

		// Token: 0x06009EBB RID: 40635 RVA: 0x00379CF4 File Offset: 0x00377EF4
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranch.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.trunk = this.GetTrunk();
			if (!this.HasTrunk)
			{
				global::Debug.LogWarning("Tree Branch loaded with missing trunk reference. Destroying...");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(this.trunk, this);
		}

		// Token: 0x06009EBC RID: 40636 RVA: 0x00379D6A File Offset: 0x00377F6A
		private void OnHarvest(object data)
		{
			if (this.HasTrunk)
			{
				this.trunk.OnBrancHarvested(this);
			}
		}

		// Token: 0x06009EBD RID: 40637 RVA: 0x00379D80 File Offset: 0x00377F80
		protected override void OnCleanUp()
		{
			this.UnsubscribeToTrunk();
			this.SetOccupyGridSpace(false);
			base.OnCleanUp();
		}

		// Token: 0x06009EBE RID: 40638 RVA: 0x00379D98 File Offset: 0x00377F98
		private void SetOccupyGridSpace(bool active)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!active)
			{
				if (Grid.Objects[cell, 5] == base.gameObject)
				{
					Grid.Objects[cell, 5] = null;
				}
				return;
			}
			GameObject gameObject = Grid.Objects[cell, 5];
			if (gameObject != null && gameObject != base.gameObject)
			{
				global::Debug.LogWarningFormat(base.gameObject, "PlantBranch.SetOccupyGridSpace already occupied by {0}", new object[]
				{
					gameObject
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			Grid.Objects[cell, 5] = base.gameObject;
		}

		// Token: 0x06009EBF RID: 40639 RVA: 0x00379E38 File Offset: 0x00378038
		public void SetTrunk(PlantBranchGrower.Instance trunk)
		{
			this.trunk = trunk;
			base.smi.sm.Trunk.Set(trunk.gameObject, this, false);
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(trunk, this);
		}

		// Token: 0x06009EC0 RID: 40640 RVA: 0x00379E87 File Offset: 0x00378087
		public PlantBranchGrower.Instance GetTrunk()
		{
			if (base.smi.sm.Trunk.IsNull(this))
			{
				return null;
			}
			return base.sm.Trunk.Get(this).GetSMI<PlantBranchGrower.Instance>();
		}

		// Token: 0x06009EC1 RID: 40641 RVA: 0x00379EBC File Offset: 0x003780BC
		private void SubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			if (this.trunkWiltHandle == -1)
			{
				this.trunkWiltHandle = this.trunk.gameObject.Subscribe(-724860998, new Action<object>(this.OnTrunkWilt));
			}
			if (this.trunkWiltRecoverHandle == -1)
			{
				this.trunkWiltRecoverHandle = this.trunk.gameObject.Subscribe(712767498, new Action<object>(this.OnTrunkRecover));
			}
			base.Trigger(912965142, !this.trunk.GetComponent<WiltCondition>().IsWilting());
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			PlantablePlot receptacle = this.trunk.GetComponent<ReceptacleMonitor>().GetReceptacle();
			component.SetReceptacle(receptacle);
			this.trunk.RefreshBranchZPositionOffset(base.gameObject);
			base.GetComponent<BudUprootedMonitor>().SetParentObject(this.trunk.GetComponent<KPrefabID>());
		}

		// Token: 0x06009EC2 RID: 40642 RVA: 0x00379F9C File Offset: 0x0037819C
		private void UnsubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			this.trunk.gameObject.Unsubscribe(this.trunkWiltHandle);
			this.trunk.gameObject.Unsubscribe(this.trunkWiltRecoverHandle);
			this.trunkWiltHandle = -1;
			this.trunkWiltRecoverHandle = -1;
			this.trunk.OnBranchRemoved(base.gameObject);
		}

		// Token: 0x06009EC3 RID: 40643 RVA: 0x00379FFD File Offset: 0x003781FD
		private void OnTrunkWilt(object data = null)
		{
			base.Trigger(912965142, false);
		}

		// Token: 0x06009EC4 RID: 40644 RVA: 0x0037A010 File Offset: 0x00378210
		private void OnTrunkRecover(object data = null)
		{
			base.Trigger(912965142, true);
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06009EC5 RID: 40645 RVA: 0x0037A023 File Offset: 0x00378223
		public string WiltStateString
		{
			get
			{
				return "    • " + DUPLICANTS.STATS.TRUNKHEALTH.NAME;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06009EC6 RID: 40646 RVA: 0x0037A039 File Offset: 0x00378239
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.UnhealthyRoot
				};
			}
		}

		// Token: 0x04007B0B RID: 31499
		public PlantBranchGrower.Instance trunk;

		// Token: 0x04007B0C RID: 31500
		private int trunkWiltHandle = -1;

		// Token: 0x04007B0D RID: 31501
		private int trunkWiltRecoverHandle = -1;
	}
}
