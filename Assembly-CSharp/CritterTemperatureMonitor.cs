using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007FB RID: 2043
public class CritterTemperatureMonitor : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>
{
	// Token: 0x06003875 RID: 14453 RVA: 0x00134340 File Offset: 0x00132540
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.uncomfortableEffect = new Effect("EffectCritterTemperatureUncomfortable", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.uncomfortableEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, false, false, true));
		this.deadlyEffect = new Effect("EffectCritterTemperatureDeadly", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.deadlyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -2f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, false, false, true));
		this.root.Enter(new StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback(CritterTemperatureMonitor.RefreshInternalTemperature)).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			StateMachine.BaseState targetState = smi.GetTargetState();
			if (smi.GetCurrentState() != targetState)
			{
				smi.GoTo(targetState);
			}
		}, UpdateRate.SIM_200ms, false).Update(new Action<CritterTemperatureMonitor.Instance, float>(CritterTemperatureMonitor.UpdateInternalTemperature), UpdateRate.SIM_1000ms, false);
		this.hot.TagTransition(GameTags.Dead, this.dead, false).ToggleCreatureThought(Db.Get().Thoughts.Hot, null);
		this.cold.TagTransition(GameTags.Dead, this.dead, false).ToggleCreatureThought(Db.Get().Thoughts.Cold, null);
		this.hot.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.hot.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.cold.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.cold.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.dead.DoNothing();
	}

	// Token: 0x06003876 RID: 14454 RVA: 0x0013464E File Offset: 0x0013284E
	public static void UpdateInternalTemperature(CritterTemperatureMonitor.Instance smi, float dt)
	{
		CritterTemperatureMonitor.RefreshInternalTemperature(smi);
		if (smi.OnUpdate_GetTemperatureInternal != null)
		{
			smi.OnUpdate_GetTemperatureInternal(dt, smi.GetTemperatureInternal());
		}
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x00134670 File Offset: 0x00132870
	public static void RefreshInternalTemperature(CritterTemperatureMonitor.Instance smi)
	{
		if (smi.temperature != null)
		{
			smi.temperature.SetValue(smi.GetTemperatureInternal());
		}
	}

	// Token: 0x040021E8 RID: 8680
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State comfortable;

	// Token: 0x040021E9 RID: 8681
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State dead;

	// Token: 0x040021EA RID: 8682
	public CritterTemperatureMonitor.TemperatureStates hot;

	// Token: 0x040021EB RID: 8683
	public CritterTemperatureMonitor.TemperatureStates cold;

	// Token: 0x040021EC RID: 8684
	public Effect uncomfortableEffect;

	// Token: 0x040021ED RID: 8685
	public Effect deadlyEffect;

	// Token: 0x020016DB RID: 5851
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060093BE RID: 37822 RVA: 0x00359DD5 File Offset: 0x00357FD5
		public float GetIdealTemperature()
		{
			return (this.temperatureHotUncomfortable + this.temperatureColdUncomfortable) / 2f;
		}

		// Token: 0x040070FC RID: 28924
		public float temperatureHotDeadly = float.MaxValue;

		// Token: 0x040070FD RID: 28925
		public float temperatureHotUncomfortable = float.MaxValue;

		// Token: 0x040070FE RID: 28926
		public float temperatureColdDeadly = float.MinValue;

		// Token: 0x040070FF RID: 28927
		public float temperatureColdUncomfortable = float.MinValue;

		// Token: 0x04007100 RID: 28928
		public float secondsUntilDamageStarts = 1f;

		// Token: 0x04007101 RID: 28929
		public float damagePerSecond = 0.25f;

		// Token: 0x04007102 RID: 28930
		public bool isBammoth;
	}

	// Token: 0x020016DC RID: 5852
	public class TemperatureStates : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State
	{
		// Token: 0x04007103 RID: 28931
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State uncomfortable;

		// Token: 0x04007104 RID: 28932
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State deadly;
	}

	// Token: 0x020016DD RID: 5853
	public new class Instance : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.GameInstance
	{
		// Token: 0x060093C1 RID: 37825 RVA: 0x00359E4C File Offset: 0x0035804C
		public Instance(IStateMachineTarget master, CritterTemperatureMonitor.Def def) : base(master, def)
		{
			this.health = master.GetComponent<Health>();
			this.occupyArea = master.GetComponent<OccupyArea>();
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.CritterTemperature.Lookup(base.gameObject);
			this.pickupable = master.GetComponent<Pickupable>();
		}

		// Token: 0x060093C2 RID: 37826 RVA: 0x00359EB1 File Offset: 0x003580B1
		public void ResetDamageCooldown()
		{
			this.secondsUntilDamage = base.def.secondsUntilDamageStarts;
		}

		// Token: 0x060093C3 RID: 37827 RVA: 0x00359EC4 File Offset: 0x003580C4
		public void TryDamage(float deltaSeconds)
		{
			if (this.secondsUntilDamage <= 0f)
			{
				this.health.Damage(base.def.damagePerSecond);
				this.secondsUntilDamage = 1f;
				return;
			}
			this.secondsUntilDamage -= deltaSeconds;
		}

		// Token: 0x060093C4 RID: 37828 RVA: 0x00359F04 File Offset: 0x00358104
		public StateMachine.BaseState GetTargetState()
		{
			bool flag = this.IsEntirelyInVaccum();
			float temperatureExternal = this.GetTemperatureExternal();
			float temperatureInternal = this.GetTemperatureInternal();
			StateMachine.BaseState result;
			if (this.pickupable.KPrefabID.HasTag(GameTags.Dead))
			{
				result = base.sm.dead;
			}
			else if (!flag && temperatureExternal > base.def.temperatureHotDeadly)
			{
				result = base.sm.hot.deadly;
			}
			else if (!flag && temperatureExternal < base.def.temperatureColdDeadly)
			{
				result = base.sm.cold.deadly;
			}
			else if (temperatureInternal > base.def.temperatureHotUncomfortable)
			{
				result = base.sm.hot.uncomfortable;
			}
			else if (temperatureInternal < base.def.temperatureColdUncomfortable)
			{
				result = base.sm.cold.uncomfortable;
			}
			else
			{
				result = base.sm.comfortable;
			}
			return result;
		}

		// Token: 0x060093C5 RID: 37829 RVA: 0x00359FE8 File Offset: 0x003581E8
		public bool IsEntirelyInVaccum()
		{
			int cachedCell = this.pickupable.cachedCell;
			bool result;
			if (this.occupyArea != null)
			{
				result = true;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					if (!base.def.isBammoth || this.occupyArea.OccupiedCellsOffsets[i].x == 0)
					{
						int num = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
						if (!Grid.IsValidCell(num) || !Grid.Element[num].IsVacuum)
						{
							result = false;
							break;
						}
					}
				}
			}
			else
			{
				result = (!Grid.IsValidCell(cachedCell) || Grid.Element[cachedCell].IsVacuum);
			}
			return result;
		}

		// Token: 0x060093C6 RID: 37830 RVA: 0x0035A09B File Offset: 0x0035829B
		public float GetTemperatureInternal()
		{
			return this.primaryElement.Temperature;
		}

		// Token: 0x060093C7 RID: 37831 RVA: 0x0035A0A8 File Offset: 0x003582A8
		public float GetTemperatureExternal()
		{
			int cachedCell = this.pickupable.cachedCell;
			if (this.occupyArea != null)
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					if (!base.def.isBammoth || this.occupyArea.OccupiedCellsOffsets[i].x == 0)
					{
						int num3 = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
						if (Grid.IsValidCell(num3))
						{
							bool flag = Grid.Element[num3].id == SimHashes.Vacuum || Grid.Element[num3].id == SimHashes.Void;
							num2++;
							num += (flag ? this.GetTemperatureInternal() : Grid.Temperature[num3]);
						}
					}
				}
				return num / (float)Mathf.Max(1, num2);
			}
			if (Grid.Element[cachedCell].id != SimHashes.Vacuum && Grid.Element[cachedCell].id != SimHashes.Void)
			{
				return Grid.Temperature[cachedCell];
			}
			return this.GetTemperatureInternal();
		}

		// Token: 0x04007105 RID: 28933
		public AmountInstance temperature;

		// Token: 0x04007106 RID: 28934
		public Health health;

		// Token: 0x04007107 RID: 28935
		public OccupyArea occupyArea;

		// Token: 0x04007108 RID: 28936
		public PrimaryElement primaryElement;

		// Token: 0x04007109 RID: 28937
		public Pickupable pickupable;

		// Token: 0x0400710A RID: 28938
		public float secondsUntilDamage;

		// Token: 0x0400710B RID: 28939
		public Action<float, float> OnUpdate_GetTemperatureInternal;
	}
}
