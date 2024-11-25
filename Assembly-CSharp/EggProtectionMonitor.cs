using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000803 RID: 2051
public class EggProtectionMonitor : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>
{
	// Token: 0x060038AC RID: 14508 RVA: 0x001353B0 File Offset: 0x001335B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.find_egg;
		this.find_egg.BatchUpdate(new UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate(EggProtectionMonitor.Instance.FindEggToGuard), UpdateRate.SIM_200ms).ParamTransition<bool>(this.hasEggToGuard, this.guard.safe, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsTrue);
		this.guard.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_kanim"), smi.def.animPrefix, "_heat", 0);
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
		}).Exit(delegate(EggProtectionMonitor.Instance smi)
		{
			if (!smi.def.animPrefix.IsNullOrWhiteSpace())
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_kanim"), smi.def.animPrefix, null, 0);
			}
			else
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim("pincher_kanim").GetData(), 0);
			}
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
		}).Update("CanProtectEgg", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.CanProtectEgg();
		}, UpdateRate.SIM_1000ms, true).ParamTransition<bool>(this.hasEggToGuard, this.find_egg, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsFalse);
		this.guard.safe.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		}).Update("EggProtectionMonitor.safe", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.RefreshThreat(null);
		}, UpdateRate.SIM_200ms, true).ToggleStatusItem(CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME, CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, null);
		this.guard.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (EggProtectionMonitor.Instance smi) => smi.threatMonitor.HasThreat(), delegate(EggProtectionMonitor.Instance smi)
		{
			smi.GoTo(this.guard.safe);
		}).Update("Threatened", new Action<EggProtectionMonitor.Instance, float>(EggProtectionMonitor.CritterUpdateThreats), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x0013556F File Offset: 0x0013376F
	private static void CritterUpdateThreats(EggProtectionMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.threatMonitor.HasThreat())
		{
			smi.GoTo(smi.sm.guard.safe);
		}
	}

	// Token: 0x04002211 RID: 8721
	public StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.BoolParameter hasEggToGuard;

	// Token: 0x04002212 RID: 8722
	public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State find_egg;

	// Token: 0x04002213 RID: 8723
	public EggProtectionMonitor.GuardEggStates guard;

	// Token: 0x020016EE RID: 5870
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400712E RID: 28974
		public Tag[] allyTags;

		// Token: 0x0400712F RID: 28975
		public string animPrefix;
	}

	// Token: 0x020016EF RID: 5871
	public class GuardEggStates : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State
	{
		// Token: 0x04007130 RID: 28976
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State safe;

		// Token: 0x04007131 RID: 28977
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State threatened;
	}

	// Token: 0x020016F0 RID: 5872
	public new class Instance : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameInstance
	{
		// Token: 0x06009401 RID: 37889 RVA: 0x0035AA0D File Offset: 0x00358C0D
		public Instance(IStateMachineTarget master, EggProtectionMonitor.Def def) : base(master, def)
		{
			this.navigator = master.GetComponent<Navigator>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x06009402 RID: 37890 RVA: 0x0035AA38 File Offset: 0x00358C38
		public void CanProtectEgg()
		{
			bool flag = true;
			if (this.eggToProtect == null)
			{
				flag = false;
			}
			if (flag)
			{
				int num = 150;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(this.eggToProtect));
				if (navigationCost == -1 || navigationCost >= num)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				this.SetEggToGuard(null);
			}
		}

		// Token: 0x06009403 RID: 37891 RVA: 0x0035AA8C File Offset: 0x00358C8C
		public static void FindEggToGuard(List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry> instances, float time_delta)
		{
			ListPool<KPrefabID, EggProtectionMonitor>.PooledList pooledList = ListPool<KPrefabID, EggProtectionMonitor>.Allocate();
			pooledList.Capacity = Mathf.Max(pooledList.Capacity, Components.IncubationMonitors.Count);
			foreach (object obj in Components.IncubationMonitors)
			{
				IncubationMonitor.Instance instance = (IncubationMonitor.Instance)obj;
				pooledList.Add(instance.gameObject.GetComponent<KPrefabID>());
			}
			ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.PooledList pooledList2 = ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.Allocate();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(pooledList);
			for (int i = 0; i < pooledList.Count; i += 256)
			{
				EggProtectionMonitor.Instance.find_eggs_job.Add(new EggProtectionMonitor.Instance.FindEggsTask(i, Mathf.Min(i + 256, pooledList.Count)));
			}
			GlobalJobManager.Run(EggProtectionMonitor.Instance.find_eggs_job);
			for (int num = 0; num != EggProtectionMonitor.Instance.find_eggs_job.Count; num++)
			{
				EggProtectionMonitor.Instance.find_eggs_job.GetWorkItem(num).Finish(pooledList, pooledList2);
			}
			pooledList.Recycle();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(null);
			foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry entry in new List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry>(instances))
			{
				GameObject eggToGuard = null;
				int num2 = 100;
				foreach (EggProtectionMonitor.Instance.Egg egg in pooledList2)
				{
					int navigationCost = entry.data.navigator.GetNavigationCost(egg.cell);
					if (navigationCost != -1 && navigationCost < num2)
					{
						eggToGuard = egg.game_object;
						num2 = navigationCost;
					}
				}
				entry.data.SetEggToGuard(eggToGuard);
			}
			pooledList2.Recycle();
		}

		// Token: 0x06009404 RID: 37892 RVA: 0x0035AC70 File Offset: 0x00358E70
		public void SetEggToGuard(GameObject egg)
		{
			this.eggToProtect = egg;
			base.sm.hasEggToGuard.Set(egg != null, base.smi, false);
		}

		// Token: 0x06009405 RID: 37893 RVA: 0x0035AC98 File Offset: 0x00358E98
		public void GoToThreatened()
		{
			base.smi.GoTo(base.sm.guard.threatened);
		}

		// Token: 0x06009406 RID: 37894 RVA: 0x0035ACB8 File Offset: 0x00358EB8
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning() || this.eggToProtect == null)
			{
				return;
			}
			if (base.smi.threatMonitor.HasThreat())
			{
				this.GoToThreatened();
				return;
			}
			if (base.smi.GetCurrentState() != base.sm.guard.safe)
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.guard.safe);
			}
		}

		// Token: 0x04007132 RID: 28978
		[MySmiReq]
		public ThreatMonitor.Instance threatMonitor;

		// Token: 0x04007133 RID: 28979
		public GameObject eggToProtect;

		// Token: 0x04007134 RID: 28980
		private Navigator navigator;

		// Token: 0x04007135 RID: 28981
		private Action<object> refreshThreatDelegate;

		// Token: 0x04007136 RID: 28982
		private static WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>> find_eggs_job = new WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>>();

		// Token: 0x02002578 RID: 9592
		private struct Egg
		{
			// Token: 0x0400A6CD RID: 42701
			public GameObject game_object;

			// Token: 0x0400A6CE RID: 42702
			public int cell;
		}

		// Token: 0x02002579 RID: 9593
		private struct FindEggsTask : IWorkItem<List<KPrefabID>>
		{
			// Token: 0x0600BEE4 RID: 48868 RVA: 0x003DAA7B File Offset: 0x003D8C7B
			public FindEggsTask(int start, int end)
			{
				this.start = start;
				this.end = end;
				this.eggs = ListPool<int, EggProtectionMonitor>.Allocate();
			}

			// Token: 0x0600BEE5 RID: 48869 RVA: 0x003DAA98 File Offset: 0x003D8C98
			public void Run(List<KPrefabID> prefab_ids)
			{
				for (int num = this.start; num != this.end; num++)
				{
					if (EggProtectionMonitor.Instance.FindEggsTask.EGG_TAG.Contains(prefab_ids[num].PrefabTag))
					{
						this.eggs.Add(num);
					}
				}
			}

			// Token: 0x0600BEE6 RID: 48870 RVA: 0x003DAAE0 File Offset: 0x003D8CE0
			public void Finish(List<KPrefabID> prefab_ids, List<EggProtectionMonitor.Instance.Egg> eggs)
			{
				foreach (int index in this.eggs)
				{
					GameObject gameObject = prefab_ids[index].gameObject;
					eggs.Add(new EggProtectionMonitor.Instance.Egg
					{
						game_object = gameObject,
						cell = Grid.PosToCell(gameObject)
					});
				}
				this.eggs.Recycle();
			}

			// Token: 0x0400A6CF RID: 42703
			private static readonly List<Tag> EGG_TAG = new List<Tag>
			{
				"CrabEgg".ToTag(),
				"CrabWoodEgg".ToTag(),
				"CrabFreshWaterEgg".ToTag()
			};

			// Token: 0x0400A6D0 RID: 42704
			private ListPool<int, EggProtectionMonitor>.PooledList eggs;

			// Token: 0x0400A6D1 RID: 42705
			private int start;

			// Token: 0x0400A6D2 RID: 42706
			private int end;
		}
	}
}
