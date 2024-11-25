using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200097A RID: 2426
public class DecorMonitor : GameStateMachine<DecorMonitor, DecorMonitor.Instance>
{
	// Token: 0x060046FE RID: 18174 RVA: 0x00196190 File Offset: 0x00194390
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleAttributeModifier("DecorSmoother", (DecorMonitor.Instance smi) => smi.GetDecorModifier(), (DecorMonitor.Instance smi) => true).Update("DecorSensing", delegate(DecorMonitor.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.NewDay, (DecorMonitor.Instance smi) => GameClock.Instance, delegate(DecorMonitor.Instance smi)
		{
			smi.OnNewDay();
		});
	}

	// Token: 0x04002E45 RID: 11845
	public static float MAXIMUM_DECOR_VALUE = 120f;

	// Token: 0x02001922 RID: 6434
	public new class Instance : GameStateMachine<DecorMonitor, DecorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B4F RID: 39759 RVA: 0x0036F3DC File Offset: 0x0036D5DC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.cycleTotalDecor = 2250f;
			this.amount = Db.Get().Amounts.Decor.Lookup(base.gameObject);
			this.modifier = new AttributeModifier(Db.Get().Amounts.Decor.deltaAttribute.Id, 1f, DUPLICANTS.NEEDS.DECOR.OBSERVED_DECOR, false, false, false);
		}

		// Token: 0x06009B50 RID: 39760 RVA: 0x0036F50D File Offset: 0x0036D70D
		public AttributeModifier GetDecorModifier()
		{
			return this.modifier;
		}

		// Token: 0x06009B51 RID: 39761 RVA: 0x0036F518 File Offset: 0x0036D718
		public void Update(float dt)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell))
			{
				return;
			}
			float decorAtCell = GameUtil.GetDecorAtCell(cell);
			this.cycleTotalDecor += decorAtCell * dt;
			float value = 0f;
			float num = 4.1666665f;
			if (Mathf.Abs(decorAtCell - this.amount.value) > 0.5f)
			{
				if (decorAtCell > this.amount.value)
				{
					value = 3f * num;
				}
				else if (decorAtCell < this.amount.value)
				{
					value = -num;
				}
			}
			else
			{
				this.amount.value = decorAtCell;
			}
			this.modifier.SetValue(value);
		}

		// Token: 0x06009B52 RID: 39762 RVA: 0x0036F5BC File Offset: 0x0036D7BC
		public void OnNewDay()
		{
			this.yesterdaysTotalDecor = this.cycleTotalDecor;
			this.cycleTotalDecor = 0f;
			float totalValue = base.gameObject.GetAttributes().Add(Db.Get().Attributes.DecorExpectation).GetTotalValue();
			float num = this.yesterdaysTotalDecor / 600f;
			num += totalValue;
			Effects component = base.gameObject.GetComponent<Effects>();
			foreach (KeyValuePair<float, string> keyValuePair in this.effectLookup)
			{
				if (num < keyValuePair.Key)
				{
					component.Add(keyValuePair.Value, true);
					break;
				}
			}
		}

		// Token: 0x06009B53 RID: 39763 RVA: 0x0036F680 File Offset: 0x0036D880
		public float GetTodaysAverageDecor()
		{
			return this.cycleTotalDecor / (GameClock.Instance.GetCurrentCycleAsPercentage() * 600f);
		}

		// Token: 0x06009B54 RID: 39764 RVA: 0x0036F699 File Offset: 0x0036D899
		public float GetYesterdaysAverageDecor()
		{
			return this.yesterdaysTotalDecor / 600f;
		}

		// Token: 0x04007876 RID: 30838
		[Serialize]
		private float cycleTotalDecor;

		// Token: 0x04007877 RID: 30839
		[Serialize]
		private float yesterdaysTotalDecor;

		// Token: 0x04007878 RID: 30840
		private AmountInstance amount;

		// Token: 0x04007879 RID: 30841
		private AttributeModifier modifier;

		// Token: 0x0400787A RID: 30842
		private List<KeyValuePair<float, string>> effectLookup = new List<KeyValuePair<float, string>>
		{
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * -0.25f, "DecorMinus1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0f, "Decor0"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.25f, "Decor1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.5f, "Decor2"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.75f, "Decor3"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE, "Decor4"),
			new KeyValuePair<float, string>(float.MaxValue, "Decor5")
		};
	}
}
