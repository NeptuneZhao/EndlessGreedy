using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public class ClimbableTreeMonitor : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>
{
	// Token: 0x06001EF8 RID: 7928 RVA: 0x000AD600 File Offset: 0x000AB800
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree, (ClimbableTreeMonitor.Instance smi) => smi.UpdateHasClimbable(), delegate(ClimbableTreeMonitor.Instance smi)
		{
			smi.OnClimbComplete();
		});
	}

	// Token: 0x04001179 RID: 4473
	private const int MAX_NAV_COST = 2147483647;

	// Token: 0x02001310 RID: 4880
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400656F RID: 25967
		public float searchMinInterval = 60f;

		// Token: 0x04006570 RID: 25968
		public float searchMaxInterval = 120f;
	}

	// Token: 0x02001311 RID: 4881
	public new class Instance : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.GameInstance
	{
		// Token: 0x060085B5 RID: 34229 RVA: 0x00326DB6 File Offset: 0x00324FB6
		public Instance(IStateMachineTarget master, ClimbableTreeMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x060085B6 RID: 34230 RVA: 0x00326DC6 File Offset: 0x00324FC6
		private void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x060085B7 RID: 34231 RVA: 0x00326DF4 File Offset: 0x00324FF4
		public bool UpdateHasClimbable()
		{
			if (this.climbTarget == null)
			{
				if (Time.time < this.nextSearchTime)
				{
					return false;
				}
				this.FindClimbableTree();
				this.RefreshSearchTime();
			}
			return this.climbTarget != null;
		}

		// Token: 0x060085B8 RID: 34232 RVA: 0x00326E2C File Offset: 0x0032502C
		private void FindClimbableTree()
		{
			this.climbTarget = null;
			ListPool<KMonoBehaviour, ClimbableTreeMonitor>.PooledList pooledList = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
			Vector3 position = base.master.transform.GetPosition();
			Extents extents = new Extents(Grid.PosToCell(position), 10);
			Navigator component = base.GetComponent<Navigator>();
			IEnumerable<object> first = GameScenePartitioner.Instance.AsyncSafeEnumerate(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.plants);
			IEnumerable<object> second = GameScenePartitioner.Instance.AsyncSafeEnumerate(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.completeBuildings);
			foreach (object obj in first.Concat(second))
			{
				KMonoBehaviour kmonoBehaviour = obj as KMonoBehaviour;
				if (!kmonoBehaviour.HasTag(GameTags.Creatures.ReservedByCreature))
				{
					int cell = Grid.PosToCell(kmonoBehaviour);
					if (component.CanReach(cell))
					{
						ForestTreeSeedMonitor component2 = kmonoBehaviour.GetComponent<ForestTreeSeedMonitor>();
						StorageLocker component3 = kmonoBehaviour.GetComponent<StorageLocker>();
						if (component2 != null)
						{
							if (!component2.ExtraSeedAvailable)
							{
								continue;
							}
						}
						else
						{
							if (!(component3 != null))
							{
								continue;
							}
							Storage component4 = component3.GetComponent<Storage>();
							if (!component4.allowItemRemoval || component4.IsEmpty())
							{
								continue;
							}
						}
						pooledList.Add(kmonoBehaviour);
					}
				}
			}
			if (pooledList.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, pooledList.Count);
				KMonoBehaviour kmonoBehaviour2 = pooledList[index];
				this.climbTarget = kmonoBehaviour2.gameObject;
			}
			pooledList.Recycle();
		}

		// Token: 0x060085B9 RID: 34233 RVA: 0x00326FBC File Offset: 0x003251BC
		public void OnClimbComplete()
		{
			this.climbTarget = null;
		}

		// Token: 0x04006571 RID: 25969
		public GameObject climbTarget;

		// Token: 0x04006572 RID: 25970
		public float nextSearchTime;
	}
}
