using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class ReactionMonitor : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>
{
	// Token: 0x060019B2 RID: 6578 RVA: 0x0008956C File Offset: 0x0008776C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.DestinationReached, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		}).EventHandler(GameHashes.NavigationFailed, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		});
		this.idle.Enter("ClearReactable", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Set(null, smi, false);
		}).TagTransition(GameTags.Dead, this.dead, false);
		this.reacting.Enter("Reactable.Begin", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).Begin(smi.gameObject);
		}).Enter(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(-909573545, null);
		}).Enter("Reactable.AddChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
			}
		}).Update("Reactable.Update", delegate(ReactionMonitor.Instance smi, float dt)
		{
			this.reactable.Get(smi).Update(dt);
		}, UpdateRate.SIM_200ms, false).Exit(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(824899998, null);
		}).Exit("Reactable.End", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).End();
		}).Exit("Reactable.RemoveChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
			}
		}).EventTransition(GameHashes.NavigationFailed, this.idle, null).TagTransition(GameTags.Dying, this.dead, false).TagTransition(GameTags.Dead, this.dead, false);
		this.dead.DoNothing();
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x0008970D File Offset: 0x0008790D
	private static bool ShouldReact(ReactionMonitor.Instance smi)
	{
		return smi.ImmediateReactable != null;
	}

	// Token: 0x04000EA9 RID: 3753
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State idle;

	// Token: 0x04000EAA RID: 3754
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State reacting;

	// Token: 0x04000EAB RID: 3755
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State dead;

	// Token: 0x04000EAC RID: 3756
	public StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.ObjectParameter<Reactable> reactable;

	// Token: 0x02001271 RID: 4721
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006375 RID: 25461
		public ObjectLayer ReactionLayer;
	}

	// Token: 0x02001272 RID: 4722
	public new class Instance : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.GameInstance
	{
		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06008330 RID: 33584 RVA: 0x0031E64A File Offset: 0x0031C84A
		// (set) Token: 0x06008331 RID: 33585 RVA: 0x0031E652 File Offset: 0x0031C852
		public Reactable ImmediateReactable { get; private set; }

		// Token: 0x06008332 RID: 33586 RVA: 0x0031E65B File Offset: 0x0031C85B
		public Instance(IStateMachineTarget master, ReactionMonitor.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.lastReactTimes = new Dictionary<HashedString, float>();
			this.oneshotReactables = new List<Reactable>();
		}

		// Token: 0x06008333 RID: 33587 RVA: 0x0031E692 File Offset: 0x0031C892
		public bool CanReact(Emote e)
		{
			return this.animController != null && e.IsValidForController(this.animController);
		}

		// Token: 0x06008334 RID: 33588 RVA: 0x0031E6B0 File Offset: 0x0031C8B0
		public bool TryReact(Reactable reactable, float clockTime, Navigator.ActiveTransition transition = null)
		{
			if (reactable == null)
			{
				return false;
			}
			float num;
			if ((this.lastReactTimes.TryGetValue(reactable.id, out num) && num == this.lastReaction) || clockTime - num < reactable.localCooldown)
			{
				return false;
			}
			if (!reactable.CanBegin(base.gameObject, transition))
			{
				return false;
			}
			this.lastReactTimes[reactable.id] = clockTime;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.smi.GoTo(base.sm.reacting);
			return true;
		}

		// Token: 0x06008335 RID: 33589 RVA: 0x0031E740 File Offset: 0x0031C940
		public void PollForReactables(Navigator.ActiveTransition transition)
		{
			if (this.IsReacting())
			{
				return;
			}
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (reactable.IsExpired())
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.smi.gameObject));
			ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(int)base.def.ReactionLayer];
			ListPool<ScenePartitionerEntry, ReactionMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ReactionMonitor>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, layer, pooledList);
			float num = float.NaN;
			float time = GameClock.Instance.GetTime();
			for (int j = 0; j < pooledList.Count; j++)
			{
				Reactable reactable2 = pooledList[j].obj as Reactable;
				if (this.TryReact(reactable2, time, transition))
				{
					num = time;
					break;
				}
			}
			this.lastReaction = num;
			pooledList.Recycle();
		}

		// Token: 0x06008336 RID: 33590 RVA: 0x0031E845 File Offset: 0x0031CA45
		public void ClearLastReaction()
		{
			this.lastReaction = float.NaN;
		}

		// Token: 0x06008337 RID: 33591 RVA: 0x0031E854 File Offset: 0x0031CA54
		public void StopReaction()
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				if (base.sm.reactable.Get(base.smi) == this.oneshotReactables[i])
				{
					this.oneshotReactables[i].Cleanup();
					this.oneshotReactables.RemoveAt(i);
					break;
				}
			}
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x06008338 RID: 33592 RVA: 0x0031E8D2 File Offset: 0x0031CAD2
		public bool IsReacting()
		{
			return base.smi.IsInsideState(base.sm.reacting);
		}

		// Token: 0x06008339 RID: 33593 RVA: 0x0031E8EC File Offset: 0x0031CAEC
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, HashedString reactionId, Emote emote, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			if (!this.CanReact(emote))
			{
				return null;
			}
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(target, reactionId, choreType, globalCooldown, localCooldown, lifeSpan, maxInitialDelay);
			selfEmoteReactable.SetEmote(emote);
			int num = 0;
			while (emotePreconditions != null && num < emotePreconditions.Count)
			{
				selfEmoteReactable.AddPrecondition(emotePreconditions[num]);
				num++;
			}
			if (isOneShot)
			{
				this.AddOneshotReactable(selfEmoteReactable);
			}
			return selfEmoteReactable;
		}

		// Token: 0x0600833A RID: 33594 RVA: 0x0031E950 File Offset: 0x0031CB50
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, string reactionId, string emoteAnim, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float maxTriggerTime = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			Emote emote = new Emote(null, reactionId, new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			}, emoteAnim);
			return this.AddSelfEmoteReactable(target, reactionId, emote, isOneShot, choreType, globalCooldown, localCooldown, maxTriggerTime, maxInitialDelay, emotePreconditions);
		}

		// Token: 0x0600833B RID: 33595 RVA: 0x0031E9A0 File Offset: 0x0031CBA0
		public void AddOneshotReactable(SelfEmoteReactable reactable)
		{
			if (reactable == null)
			{
				return;
			}
			this.oneshotReactables.Add(reactable);
		}

		// Token: 0x0600833C RID: 33596 RVA: 0x0031E9B4 File Offset: 0x0031CBB4
		public void CancelOneShotReactable(SelfEmoteReactable cancel_target)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (cancel_target == reactable)
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600833D RID: 33597 RVA: 0x0031EA00 File Offset: 0x0031CC00
		public void CancelOneShotReactables(Emote reactionEmote)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				EmoteReactable emoteReactable = this.oneshotReactables[i] as EmoteReactable;
				if (emoteReactable != null && emoteReactable.emote == reactionEmote)
				{
					emoteReactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
		}

		// Token: 0x04006377 RID: 25463
		private KBatchedAnimController animController;

		// Token: 0x04006378 RID: 25464
		private float lastReaction = float.NaN;

		// Token: 0x04006379 RID: 25465
		private Dictionary<HashedString, float> lastReactTimes;

		// Token: 0x0400637A RID: 25466
		private List<Reactable> oneshotReactables;
	}
}
