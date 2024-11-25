using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009A9 RID: 2473
public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
	// Token: 0x060047F0 RID: 18416 RVA: 0x0019C1E4 File Offset: 0x0019A3E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.safe;
		this.root.EventHandler(GameHashes.SafeFromThreats, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnSafe(d);
		}).EventHandler(GameHashes.Attacked, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnAttacked(d);
		}).EventHandler(GameHashes.ObjectDestroyed, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		this.safe.Enter(delegate(ThreatMonitor.Instance smi)
		{
			smi.revengeThreat.Clear();
		}).Enter(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats)).EventHandler(GameHashes.FactionChanged, new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats));
		this.safe.passive.DoNothing();
		this.safe.seeking.PreBrainUpdate(delegate(ThreatMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		});
		this.threatened.duplicant.Transition(this.safe, GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Not(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback(ThreatMonitor.DupeHasValidTarget)), UpdateRate.SIM_200ms);
		this.threatened.duplicant.ShouldFight.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), this.safe).Update("DupeUpdateTarget", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget), UpdateRate.SIM_200ms, false);
		this.threatened.duplicant.ShoudFlee.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), this.safe);
		this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (ThreatMonitor.Instance smi) => !smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).ToggleBehaviour(GameTags.Creatures.Attack, (ThreatMonitor.Instance smi) => smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).Update("CritterCalmUpdate", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterCalmUpdate), UpdateRate.SIM_200ms, false).PreBrainUpdate(new Action<ThreatMonitor.Instance>(ThreatMonitor.CritterUpdateThreats));
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x0019C440 File Offset: 0x0019A640
	private static void SeekThreats(ThreatMonitor.Instance smi)
	{
		Faction faction = FactionManager.Instance.GetFaction(smi.alignment.Alignment);
		if (smi.IAmADuplicant || faction.CanAttack)
		{
			smi.GoTo(smi.sm.safe.seeking);
			return;
		}
		smi.GoTo(smi.sm.safe.passive);
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x0019C4A0 File Offset: 0x0019A6A0
	private static bool DupeHasValidTarget(ThreatMonitor.Instance smi)
	{
		bool result = false;
		if (smi.MainThreat != null && smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
		{
			IApproachable component = smi.MainThreat.GetComponent<RangedAttackable>();
			if (component != null)
			{
				result = (smi.navigator.GetNavigationCost(component) != -1);
			}
		}
		return result;
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x0019C4F2 File Offset: 0x0019A6F2
	private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
	{
		if (!ThreatMonitor.DupeHasValidTarget(smi))
		{
			smi.Trigger(2144432245, null);
		}
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x0019C508 File Offset: 0x0019A708
	private static void CritterCalmUpdate(ThreatMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (smi.revengeThreat.target != null && smi.revengeThreat.Calm(dt, smi.alignment))
		{
			smi.Trigger(-21431934, null);
		}
	}

	// Token: 0x060047F5 RID: 18421 RVA: 0x0019C546 File Offset: 0x0019A746
	private static void CritterUpdateThreats(ThreatMonitor.Instance smi)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.CheckForThreats() && !ThreatMonitor.IsInSafeState(smi))
		{
			smi.GoTo(smi.sm.safe);
		}
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x0019C572 File Offset: 0x0019A772
	private static bool IsInSafeState(ThreatMonitor.Instance smi)
	{
		return smi.GetCurrentState() == smi.sm.safe.passive || smi.GetCurrentState() == smi.sm.safe.seeking;
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x0019C5A6 File Offset: 0x0019A7A6
	private Chore CreateAttackChore(ThreatMonitor.Instance smi)
	{
		return new AttackChore(smi.master, smi.MainThreat);
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x0019C5B9 File Offset: 0x0019A7B9
	private Chore CreateFleeChore(ThreatMonitor.Instance smi)
	{
		return new FleeChore(smi.master, smi.MainThreat);
	}

	// Token: 0x04002F1E RID: 12062
	public ThreatMonitor.SafeStates safe;

	// Token: 0x04002F1F RID: 12063
	public ThreatMonitor.ThreatenedStates threatened;

	// Token: 0x02001999 RID: 6553
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007A02 RID: 31234
		public Health.HealthState fleethresholdState = Health.HealthState.Injured;

		// Token: 0x04007A03 RID: 31235
		public Tag[] friendlyCreatureTags;

		// Token: 0x04007A04 RID: 31236
		public int maxSearchEntities = 50;

		// Token: 0x04007A05 RID: 31237
		public int maxSearchDistance = 20;

		// Token: 0x04007A06 RID: 31238
		public CellOffset[] offsets = OffsetGroups.Use;
	}

	// Token: 0x0200199A RID: 6554
	public class SafeStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04007A07 RID: 31239
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State passive;

		// Token: 0x04007A08 RID: 31240
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State seeking;
	}

	// Token: 0x0200199B RID: 6555
	public class ThreatenedStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04007A09 RID: 31241
		public ThreatMonitor.ThreatenedDuplicantStates duplicant;

		// Token: 0x04007A0A RID: 31242
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
	}

	// Token: 0x0200199C RID: 6556
	public class ThreatenedDuplicantStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04007A0B RID: 31243
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;

		// Token: 0x04007A0C RID: 31244
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
	}

	// Token: 0x0200199D RID: 6557
	public struct Grudge
	{
		// Token: 0x06009D52 RID: 40274 RVA: 0x0037488C File Offset: 0x00372A8C
		public void Reset(FactionAlignment revengeTarget)
		{
			this.target = revengeTarget;
			float num = 10f;
			this.grudgeTime = num;
		}

		// Token: 0x06009D53 RID: 40275 RVA: 0x003748B0 File Offset: 0x00372AB0
		public bool Calm(float dt, FactionAlignment self)
		{
			if (this.grudgeTime <= 0f)
			{
				return true;
			}
			this.grudgeTime = Mathf.Max(0f, this.grudgeTime - dt);
			if (this.grudgeTime == 0f)
			{
				if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
				}
				this.Clear();
				return true;
			}
			return false;
		}

		// Token: 0x06009D54 RID: 40276 RVA: 0x00374943 File Offset: 0x00372B43
		public void Clear()
		{
			this.grudgeTime = 0f;
			this.target = null;
		}

		// Token: 0x06009D55 RID: 40277 RVA: 0x00374958 File Offset: 0x00372B58
		public bool IsValidRevengeTarget(bool isDuplicant)
		{
			return this.target != null && this.target.IsAlignmentActive() && (this.target.health == null || !this.target.health.IsDefeated()) && (!isDuplicant || !this.target.IsPlayerTargeted());
		}

		// Token: 0x04007A0D RID: 31245
		public FactionAlignment target;

		// Token: 0x04007A0E RID: 31246
		public float grudgeTime;
	}

	// Token: 0x0200199E RID: 6558
	public new class Instance : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
	{
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06009D56 RID: 40278 RVA: 0x003749BA File Offset: 0x00372BBA
		public GameObject MainThreat
		{
			get
			{
				return this.mainThreat;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06009D57 RID: 40279 RVA: 0x003749C2 File Offset: 0x00372BC2
		public bool IAmADuplicant
		{
			get
			{
				return this.alignment.Alignment == FactionManager.FactionID.Duplicant;
			}
		}

		// Token: 0x06009D58 RID: 40280 RVA: 0x003749D4 File Offset: 0x00372BD4
		public Instance(IStateMachineTarget master, ThreatMonitor.Def def) : base(master, def)
		{
			this.alignment = master.GetComponent<FactionAlignment>();
			this.navigator = master.GetComponent<Navigator>();
			this.choreDriver = master.GetComponent<ChoreDriver>();
			this.health = master.GetComponent<Health>();
			this.choreConsumer = master.GetComponent<ChoreConsumer>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x06009D59 RID: 40281 RVA: 0x00374A42 File Offset: 0x00372C42
		public void ClearMainThreat()
		{
			this.SetMainThreat(null);
		}

		// Token: 0x06009D5A RID: 40282 RVA: 0x00374A4C File Offset: 0x00372C4C
		public void SetMainThreat(GameObject threat)
		{
			if (threat == this.mainThreat)
			{
				return;
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
				if (threat == null)
				{
					base.Trigger(2144432245, null);
				}
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
			this.mainThreat = threat;
			if (this.mainThreat != null)
			{
				this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x06009D5B RID: 40283 RVA: 0x00374B34 File Offset: 0x00372D34
		public bool HasThreat()
		{
			return this.MainThreat != null;
		}

		// Token: 0x06009D5C RID: 40284 RVA: 0x00374B42 File Offset: 0x00372D42
		public void OnSafe(object data)
		{
			if (this.revengeThreat.target != null)
			{
				if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
				{
					this.revengeThreat.Clear();
				}
				this.ClearMainThreat();
			}
		}

		// Token: 0x06009D5D RID: 40285 RVA: 0x00374B80 File Offset: 0x00372D80
		public void OnAttacked(object data)
		{
			FactionAlignment factionAlignment = (FactionAlignment)data;
			this.revengeThreat.Reset(factionAlignment);
			if (this.mainThreat == null)
			{
				this.SetMainThreat(factionAlignment.gameObject);
				this.GoToThreatened();
			}
			else if (!this.WillFight())
			{
				this.GoToThreatened();
			}
			if (factionAlignment.GetComponent<Bee>())
			{
				Chore chore = (this.choreDriver != null) ? this.choreDriver.GetCurrentChore() : null;
				if (chore != null && chore.gameObject.GetComponent<HiveWorkableEmpty>() != null)
				{
					chore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
				}
			}
		}

		// Token: 0x06009D5E RID: 40286 RVA: 0x00374C24 File Offset: 0x00372E24
		public bool WillFight()
		{
			if (this.choreConsumer != null)
			{
				if (!this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
				if (!this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
			}
			return this.health.State < base.smi.def.fleethresholdState;
		}

		// Token: 0x06009D5F RID: 40287 RVA: 0x00374CA0 File Offset: 0x00372EA0
		private void GotoThreatResponse()
		{
			Chore currentChore = base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
			if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShouldFight);
				return;
			}
			if (currentChore != null && currentChore.target != null && currentChore.target != base.master && currentChore.target.GetComponent<Pickupable>() != null)
			{
				return;
			}
			base.smi.GoTo(base.smi.sm.threatened.duplicant.ShoudFlee);
		}

		// Token: 0x06009D60 RID: 40288 RVA: 0x00374D55 File Offset: 0x00372F55
		public void GoToThreatened()
		{
			if (this.IAmADuplicant)
			{
				this.GotoThreatResponse();
				return;
			}
			base.smi.GoTo(base.sm.threatened.creature);
		}

		// Token: 0x06009D61 RID: 40289 RVA: 0x00374D81 File Offset: 0x00372F81
		public void Cleanup(object data)
		{
			if (this.mainThreat)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x06009D62 RID: 40290 RVA: 0x00374DBC File Offset: 0x00372FBC
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (base.smi.CheckForThreats())
			{
				this.GoToThreatened();
				return;
			}
			if (!ThreatMonitor.IsInSafeState(base.smi))
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.safe);
			}
		}

		// Token: 0x06009D63 RID: 40291 RVA: 0x00374E18 File Offset: 0x00373018
		public bool CheckForThreats()
		{
			if (base.isMasterNull)
			{
				return false;
			}
			GameObject x;
			if (this.revengeThreat.IsValidRevengeTarget(this.IAmADuplicant))
			{
				x = this.revengeThreat.target.gameObject;
			}
			else if (this.IAmADuplicant)
			{
				x = this.FindThreatDuplicant();
			}
			else
			{
				x = this.FindThreatOther();
			}
			this.SetMainThreat(x);
			return x != null;
		}

		// Token: 0x06009D64 RID: 40292 RVA: 0x00374E7C File Offset: 0x0037307C
		private GameObject FindThreatDuplicant()
		{
			this.threats.Clear();
			if (this.WillFight())
			{
				foreach (object obj in Components.PlayerTargeted)
				{
					FactionAlignment factionAlignment = (FactionAlignment)obj;
					if (!factionAlignment.IsNullOrDestroyed() && factionAlignment.IsPlayerTargeted() && !factionAlignment.health.IsDefeated() && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
					{
						this.threats.Add(factionAlignment);
					}
				}
			}
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x06009D65 RID: 40293 RVA: 0x00374F40 File Offset: 0x00373140
		private GameObject FindThreatOther()
		{
			this.threats.Clear();
			this.GatherThreats();
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x06009D66 RID: 40294 RVA: 0x00374F60 File Offset: 0x00373160
		private void GatherThreats()
		{
			ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.def.maxSearchDistance);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.attackableEntitiesLayer, pooledList);
			int count = pooledList.Count;
			int num = Mathf.Min(count, base.def.maxSearchEntities);
			for (int i = 0; i < num; i++)
			{
				if (this.currentUpdateIndex >= count)
				{
					this.currentUpdateIndex = 0;
				}
				ScenePartitionerEntry scenePartitionerEntry = pooledList[this.currentUpdateIndex];
				this.currentUpdateIndex++;
				FactionAlignment factionAlignment = scenePartitionerEntry.obj as FactionAlignment;
				if (!(factionAlignment.transform == null) && !(factionAlignment == this.alignment) && (base.def.friendlyCreatureTags == null || !factionAlignment.kprefabID.HasAnyTags(base.def.friendlyCreatureTags)) && factionAlignment.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
				{
					this.threats.Add(factionAlignment);
				}
			}
			pooledList.Recycle();
		}

		// Token: 0x06009D67 RID: 40295 RVA: 0x003750BC File Offset: 0x003732BC
		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int i = threats.Count - 1; i >= 0; i--)
			{
				FactionAlignment factionAlignment = threats[i];
				float num3 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num3 < num2)
				{
					num2 = num3;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}

		// Token: 0x04007A0F RID: 31247
		public FactionAlignment alignment;

		// Token: 0x04007A10 RID: 31248
		public Navigator navigator;

		// Token: 0x04007A11 RID: 31249
		public ChoreDriver choreDriver;

		// Token: 0x04007A12 RID: 31250
		private Health health;

		// Token: 0x04007A13 RID: 31251
		private ChoreConsumer choreConsumer;

		// Token: 0x04007A14 RID: 31252
		public ThreatMonitor.Grudge revengeThreat;

		// Token: 0x04007A15 RID: 31253
		public int currentUpdateIndex;

		// Token: 0x04007A16 RID: 31254
		private GameObject mainThreat;

		// Token: 0x04007A17 RID: 31255
		private List<FactionAlignment> threats = new List<FactionAlignment>();

		// Token: 0x04007A18 RID: 31256
		private Action<object> refreshThreatDelegate;
	}
}
