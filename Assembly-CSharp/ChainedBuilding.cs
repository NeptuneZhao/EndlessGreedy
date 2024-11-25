using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007AB RID: 1963
public class ChainedBuilding : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>
{
	// Token: 0x060035BA RID: 13754 RVA: 0x0012459C File Offset: 0x0012279C
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.unlinked;
		StatusItem statusItem = new StatusItem("NotLinkedToHeadStatusItem", BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.NAME, BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.TOOLTIP, "status_item_not_linked", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		statusItem.resolveTooltipCallback = delegate(string tooltip, object obj)
		{
			ChainedBuilding.StatesInstance statesInstance = (ChainedBuilding.StatesInstance)obj;
			return tooltip.Replace("{headBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.headBuildingTag.Name.ToUpper() + ".NAME")).Replace("{linkBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.linkBuildingTag.Name.ToUpper() + ".NAME"));
		};
		this.root.OnSignal(this.doRelink, this.DEBUG_relink);
		this.unlinked.ParamTransition<bool>(this.isConnectedToHead, this.linked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsTrue).ToggleStatusItem(statusItem, (ChainedBuilding.StatesInstance smi) => smi);
		this.linked.ParamTransition<bool>(this.isConnectedToHead, this.unlinked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsFalse);
		this.DEBUG_relink.Enter(delegate(ChainedBuilding.StatesInstance smi)
		{
			smi.DEBUG_Relink();
		});
	}

	// Token: 0x04002001 RID: 8193
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State unlinked;

	// Token: 0x04002002 RID: 8194
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State linked;

	// Token: 0x04002003 RID: 8195
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State DEBUG_relink;

	// Token: 0x04002004 RID: 8196
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter isConnectedToHead = new StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter();

	// Token: 0x04002005 RID: 8197
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.Signal doRelink;

	// Token: 0x02001662 RID: 5730
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F7E RID: 28542
		public Tag headBuildingTag;

		// Token: 0x04006F7F RID: 28543
		public Tag linkBuildingTag;

		// Token: 0x04006F80 RID: 28544
		public ObjectLayer objectLayer;
	}

	// Token: 0x02001663 RID: 5731
	public class StatesInstance : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.GameInstance
	{
		// Token: 0x06009209 RID: 37385 RVA: 0x00352C7C File Offset: 0x00350E7C
		public StatesInstance(IStateMachineTarget master, ChainedBuilding.Def def) : base(master, def)
		{
			BuildingDef def2 = master.GetComponent<Building>().Def;
			this.widthInCells = def2.WidthInCells;
			int cell = Grid.PosToCell(this);
			this.neighbourCheckCells = new List<int>
			{
				Grid.OffsetCell(cell, -(this.widthInCells - 1) / 2 - 1, 0),
				Grid.OffsetCell(cell, this.widthInCells / 2 + 1, 0)
			};
		}

		// Token: 0x0600920A RID: 37386 RVA: 0x00352CEC File Offset: 0x00350EEC
		public override void StartSM()
		{
			base.StartSM();
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			this.PropagateChangedEvent(this, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x0600920B RID: 37387 RVA: 0x00352D28 File Offset: 0x00350F28
		public void DEBUG_Relink()
		{
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x0600920C RID: 37388 RVA: 0x00352D58 File Offset: 0x00350F58
		protected override void OnCleanUp()
		{
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			foreach (int cell in this.neighbourCheckCells)
			{
				bool foundHead = false;
				this.CollectNeighbourToChain(cell, ref pooledHashSet, ref foundHead, this);
				this.PropogateFoundHead(foundHead, pooledHashSet);
				this.PropagateChangedEvent(this, pooledHashSet);
				pooledHashSet.Clear();
			}
			pooledHashSet.Recycle();
			base.OnCleanUp();
		}

		// Token: 0x0600920D RID: 37389 RVA: 0x00352DDC File Offset: 0x00350FDC
		public HashSet<ChainedBuilding.StatesInstance> GetLinkedBuildings(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain)
		{
			bool flag = false;
			this.CollectToChain(ref chain, ref flag, null);
			return chain;
		}

		// Token: 0x0600920E RID: 37390 RVA: 0x00352DF8 File Offset: 0x00350FF8
		private void PropogateFoundHead(bool foundHead, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.sm.isConnectedToHead.Set(foundHead, statesInstance, false);
			}
		}

		// Token: 0x0600920F RID: 37391 RVA: 0x00352E54 File Offset: 0x00351054
		private void PropagateChangedEvent(ChainedBuilding.StatesInstance changedLink, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.Trigger(-1009905786, changedLink);
			}
		}

		// Token: 0x06009210 RID: 37392 RVA: 0x00352EA8 File Offset: 0x003510A8
		private void CollectToChain(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			if (ignoredLink != null && ignoredLink == this)
			{
				return;
			}
			if (chain.Contains(this))
			{
				return;
			}
			chain.Add(this);
			if (base.HasTag(base.def.headBuildingTag))
			{
				foundHead = true;
			}
			foreach (int cell in this.neighbourCheckCells)
			{
				this.CollectNeighbourToChain(cell, ref chain, ref foundHead, null);
			}
		}

		// Token: 0x06009211 RID: 37393 RVA: 0x00352F30 File Offset: 0x00351130
		private void CollectNeighbourToChain(int cell, ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			GameObject gameObject = Grid.Objects[cell, (int)base.def.objectLayer];
			if (gameObject == null)
			{
				return;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!component.HasTag(base.def.linkBuildingTag) && !component.IsPrefabID(base.def.headBuildingTag))
			{
				return;
			}
			ChainedBuilding.StatesInstance smi = gameObject.GetSMI<ChainedBuilding.StatesInstance>();
			if (smi != null)
			{
				smi.CollectToChain(ref chain, ref foundHead, ignoredLink);
			}
		}

		// Token: 0x04006F81 RID: 28545
		private int widthInCells;

		// Token: 0x04006F82 RID: 28546
		private List<int> neighbourCheckCells;
	}
}
