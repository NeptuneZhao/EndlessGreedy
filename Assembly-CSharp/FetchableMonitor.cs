using System;

// Token: 0x02000980 RID: 2432
public class FetchableMonitor : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance>
{
	// Token: 0x0600470E RID: 18190 RVA: 0x001966FC File Offset: 0x001948FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unfetchable;
		base.serializable = StateMachine.SerializeType.Never;
		this.fetchable.Enter("RegisterFetchable", delegate(FetchableMonitor.Instance smi)
		{
			smi.RegisterFetchable();
		}).Exit("UnregisterFetchable", delegate(FetchableMonitor.Instance smi)
		{
			smi.UnregisterFetchable();
		}).EventTransition(GameHashes.ReachableChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.AssigneeChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.EntombedChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.TagsChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventHandler(GameHashes.OnStore, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.StoragePriorityChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.TagsChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateTags)).ParamTransition<bool>(this.forceUnfetchable, this.unfetchable, (FetchableMonitor.Instance smi, bool p) => !smi.IsFetchable());
		this.unfetchable.EventTransition(GameHashes.ReachableChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.AssigneeChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.EntombedChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.TagsChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).ParamTransition<bool>(this.forceUnfetchable, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback(this.IsFetchable));
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x001968FB File Offset: 0x00194AFB
	private bool IsFetchable(FetchableMonitor.Instance smi, bool param)
	{
		return this.IsFetchable(smi);
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00196904 File Offset: 0x00194B04
	private bool IsFetchable(FetchableMonitor.Instance smi)
	{
		return smi.IsFetchable();
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x0019690C File Offset: 0x00194B0C
	private void UpdateStorage(FetchableMonitor.Instance smi, object data)
	{
		Game.Instance.fetchManager.UpdateStorage(smi.pickupable.KPrefabID.PrefabID(), smi.fetchable, data as Storage);
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x00196939 File Offset: 0x00194B39
	private void UpdateTags(FetchableMonitor.Instance smi, object data)
	{
		Game.Instance.fetchManager.UpdateTags(smi.pickupable.KPrefabID.PrefabID(), smi.fetchable);
	}

	// Token: 0x04002E57 RID: 11863
	public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State fetchable;

	// Token: 0x04002E58 RID: 11864
	public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State unfetchable;

	// Token: 0x04002E59 RID: 11865
	public StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter forceUnfetchable = new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x0200192D RID: 6445
	public new class Instance : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B80 RID: 39808 RVA: 0x0036FB5F File Offset: 0x0036DD5F
		public Instance(Pickupable pickupable) : base(pickupable)
		{
			this.pickupable = pickupable;
			this.equippable = base.GetComponent<Equippable>();
		}

		// Token: 0x06009B81 RID: 39809 RVA: 0x0036FB7B File Offset: 0x0036DD7B
		public void RegisterFetchable()
		{
			this.fetchable = Game.Instance.fetchManager.Add(this.pickupable);
			Game.Instance.Trigger(-1588644844, base.gameObject);
		}

		// Token: 0x06009B82 RID: 39810 RVA: 0x0036FBB0 File Offset: 0x0036DDB0
		public void UnregisterFetchable()
		{
			Game.Instance.fetchManager.Remove(base.smi.pickupable.KPrefabID.PrefabID(), this.fetchable);
			Game.Instance.Trigger(-1491270284, base.gameObject);
		}

		// Token: 0x06009B83 RID: 39811 RVA: 0x0036FBFC File Offset: 0x0036DDFC
		public void SetForceUnfetchable(bool is_unfetchable)
		{
			base.sm.forceUnfetchable.Set(is_unfetchable, base.smi, false);
		}

		// Token: 0x06009B84 RID: 39812 RVA: 0x0036FC18 File Offset: 0x0036DE18
		public bool IsFetchable()
		{
			return !base.sm.forceUnfetchable.Get(this) && !this.pickupable.IsEntombed && this.pickupable.IsReachable() && (!(this.equippable != null) || !this.equippable.isEquipped) && !this.pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !this.pickupable.KPrefabID.HasTag(GameTags.Creatures.ReservedByCreature) && (!this.pickupable.KPrefabID.HasTag(GameTags.Creature) || this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Deliverable));
		}

		// Token: 0x040078A1 RID: 30881
		public Pickupable pickupable;

		// Token: 0x040078A2 RID: 30882
		private Equippable equippable;

		// Token: 0x040078A3 RID: 30883
		public HandleVector<int>.Handle fetchable;
	}
}
