using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class CritterCondoInteractMontior : GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>
{
	// Token: 0x06001F21 RID: 7969 RVA: 0x000AE570 File Offset: 0x000AC770
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.lookingForCondo;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.ParamTransition<float>(this.remainingSecondsForEffect, this.satisfied, (CritterCondoInteractMontior.Instance smi, float val) => val > 0f);
		this.lookingForCondo.PreBrainUpdate(new Action<CritterCondoInteractMontior.Instance>(CritterCondoInteractMontior.FindCondoTarget)).ToggleBehaviour(GameTags.Creatures.Behaviour_InteractWithCritterCondo, (CritterCondoInteractMontior.Instance smi) => !smi.targetCondo.IsNullOrStopped() && !smi.targetCondo.IsReserved(), delegate(CritterCondoInteractMontior.Instance smi)
		{
			smi.GoTo(this.satisfied);
		});
		this.satisfied.Enter(delegate(CritterCondoInteractMontior.Instance smi)
		{
			this.remainingSecondsForEffect.Set(600f, smi, false);
		}).ScheduleGoTo((CritterCondoInteractMontior.Instance smi) => this.remainingSecondsForEffect.Get(smi), this.lookingForCondo);
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x000AE640 File Offset: 0x000AC840
	private static void FindCondoTarget(CritterCondoInteractMontior.Instance smi)
	{
		using (ListPool<CritterCondo.Instance, CritterCondoInteractMontior>.PooledList pooledList = PoolsFor<CritterCondoInteractMontior>.AllocateList<CritterCondo.Instance>())
		{
			if (!smi.def.requireCavity)
			{
				Vector3 position = smi.gameObject.transform.GetPosition();
				using (List<CritterCondo.Instance>.Enumerator enumerator = Components.CritterCondos.GetItems(smi.GetMyWorldId()).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CritterCondo.Instance instance = enumerator.Current;
						if (!instance.IsNullOrDestroyed() && !(instance.def.condoTag != smi.def.condoPrefabTag) && (instance.transform.GetPosition() - position).sqrMagnitude <= 256f && instance.CanBeReserved())
						{
							pooledList.Add(instance);
						}
					}
					goto IL_152;
				}
			}
			int cell = Grid.PosToCell(smi.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				foreach (KPrefabID kprefabID in cavityForCell.buildings)
				{
					if (!kprefabID.IsNullOrDestroyed())
					{
						CritterCondo.Instance smi2 = kprefabID.GetSMI<CritterCondo.Instance>();
						if (smi2 != null && kprefabID.HasTag(smi.def.condoPrefabTag) && smi2.CanBeReserved())
						{
							pooledList.Add(smi2);
						}
					}
				}
			}
			IL_152:
			Navigator component = smi.GetComponent<Navigator>();
			int num = -1;
			foreach (CritterCondo.Instance instance2 in pooledList)
			{
				int interactStartCell = instance2.GetInteractStartCell();
				int navigationCost = component.GetNavigationCost(interactStartCell);
				if (navigationCost != -1 && (navigationCost < num || num == -1))
				{
					num = navigationCost;
					smi.targetCondo = instance2;
				}
			}
		}
	}

	// Token: 0x0400118B RID: 4491
	public GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.State lookingForCondo;

	// Token: 0x0400118C RID: 4492
	public GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.State satisfied;

	// Token: 0x0400118D RID: 4493
	private StateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.FloatParameter remainingSecondsForEffect;

	// Token: 0x02001328 RID: 4904
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065BE RID: 26046
		public bool requireCavity = true;

		// Token: 0x040065BF RID: 26047
		public Tag condoPrefabTag = "CritterCondo";
	}

	// Token: 0x02001329 RID: 4905
	public new class Instance : GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.GameInstance
	{
		// Token: 0x06008619 RID: 34329 RVA: 0x003285CA File Offset: 0x003267CA
		public Instance(IStateMachineTarget master, CritterCondoInteractMontior.Def def) : base(master, def)
		{
		}

		// Token: 0x040065C0 RID: 26048
		public CritterCondo.Instance targetCondo;
	}
}
