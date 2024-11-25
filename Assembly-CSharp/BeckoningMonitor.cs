using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007F0 RID: 2032
public class BeckoningMonitor : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
	// Token: 0x06003832 RID: 14386 RVA: 0x00133160 File Offset: 0x00131360
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeckoningMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (BeckoningMonitor.Instance smi) => smi.IsReadyToBeckon(), null).Update(delegate(BeckoningMonitor.Instance smi, float dt)
		{
			smi.UpdateBlockedStatusItem();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x020016C4 RID: 5828
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600937A RID: 37754 RVA: 0x0035966B File Offset: 0x0035786B
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
		}

		// Token: 0x040070C7 RID: 28871
		public float caloriesPerCycle;

		// Token: 0x040070C8 RID: 28872
		public string effectId = "MooWellFed";
	}

	// Token: 0x020016C5 RID: 5829
	public new class Instance : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameInstance
	{
		// Token: 0x0600937C RID: 37756 RVA: 0x003596A4 File Offset: 0x003578A4
		public Instance(IStateMachineTarget master, BeckoningMonitor.Def def) : base(master, def)
		{
			this.beckoning = Db.Get().Amounts.Beckoning.Lookup(base.gameObject);
		}

		// Token: 0x0600937D RID: 37757 RVA: 0x003596D0 File Offset: 0x003578D0
		private bool IsSpaceVisible()
		{
			int num = Grid.PosToCell(this);
			return Grid.IsValidCell(num) && Grid.ExposedToSunlight[num] > 0;
		}

		// Token: 0x0600937E RID: 37758 RVA: 0x003596FC File Offset: 0x003578FC
		private bool IsBeckoningAvailable()
		{
			return base.smi.beckoning.value >= base.smi.beckoning.GetMax();
		}

		// Token: 0x0600937F RID: 37759 RVA: 0x00359723 File Offset: 0x00357923
		public bool IsReadyToBeckon()
		{
			return this.IsBeckoningAvailable() && this.IsSpaceVisible();
		}

		// Token: 0x06009380 RID: 37760 RVA: 0x00359738 File Offset: 0x00357938
		public void UpdateBlockedStatusItem()
		{
			bool flag = this.IsSpaceVisible();
			if (!flag && this.IsBeckoningAvailable() && this.beckoningBlockedHandle == Guid.Empty)
			{
				this.beckoningBlockedHandle = this.kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked, null);
				return;
			}
			if (flag)
			{
				this.beckoningBlockedHandle = this.kselectable.RemoveStatusItem(this.beckoningBlockedHandle, false);
			}
		}

		// Token: 0x06009381 RID: 37761 RVA: 0x003597A8 File Offset: 0x003579A8
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x040070C9 RID: 28873
		private AmountInstance beckoning;

		// Token: 0x040070CA RID: 28874
		[MyCmpGet]
		private Effects effects;

		// Token: 0x040070CB RID: 28875
		[MyCmpGet]
		public KSelectable kselectable;

		// Token: 0x040070CC RID: 28876
		private Guid beckoningBlockedHandle;
	}
}
