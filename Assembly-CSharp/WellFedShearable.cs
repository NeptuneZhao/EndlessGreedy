using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class WellFedShearable : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>
{
	// Token: 0x060039C4 RID: 14788 RVA: 0x0013AA28 File Offset: 0x00138C28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Enter(delegate(WellFedShearable.Instance smi)
		{
			if (smi.def.hideSymbols != null)
			{
				foreach (KAnimHashedString symbol in smi.def.hideSymbols)
				{
					smi.animController.SetSymbolVisiblity(symbol, false);
				}
			}
		}).Update(new Action<WellFedShearable.Instance, float>(WellFedShearable.UpdateScales), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.CaloriesConsumed, delegate(WellFedShearable.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.growing.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Transition(this.fullyGrown, new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.fullyGrown.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (WellFedShearable.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).EventTransition(GameHashes.Molt, this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown))).Transition(this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x0013AB9E File Offset: 0x00138D9E
	private static bool AreScalesFullyGrown(WellFedShearable.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x0013ABBC File Offset: 0x00138DBC
	private static void UpdateScales(WellFedShearable.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			for (int i = 0; i < smi.def.scaleGrowthSymbols.Length; i++)
			{
				bool is_visible = i <= num - 1;
				smi.animController.SetSymbolVisiblity(smi.def.scaleGrowthSymbols[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x040022C1 RID: 8897
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State growing;

	// Token: 0x040022C2 RID: 8898
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State fullyGrown;

	// Token: 0x02001744 RID: 5956
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009516 RID: 38166 RVA: 0x0035ECE7 File Offset: 0x0035CEE7
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x06009517 RID: 38167 RVA: 0x0035ED10 File Offset: 0x0035CF10
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04007234 RID: 29236
		public string effectId;

		// Token: 0x04007235 RID: 29237
		public float caloriesPerCycle;

		// Token: 0x04007236 RID: 29238
		public float growthDurationCycles;

		// Token: 0x04007237 RID: 29239
		public int levelCount;

		// Token: 0x04007238 RID: 29240
		public Tag itemDroppedOnShear;

		// Token: 0x04007239 RID: 29241
		public float dropMass;

		// Token: 0x0400723A RID: 29242
		public Tag requiredDiet = null;

		// Token: 0x0400723B RID: 29243
		public KAnimHashedString[] scaleGrowthSymbols = WellFedShearable.Def.SCALE_SYMBOL_NAMES;

		// Token: 0x0400723C RID: 29244
		public KAnimHashedString[] hideSymbols;

		// Token: 0x0400723D RID: 29245
		public static KAnimHashedString[] SCALE_SYMBOL_NAMES = new KAnimHashedString[]
		{
			"scale_0",
			"scale_1",
			"scale_2",
			"scale_3",
			"scale_4"
		};
	}

	// Token: 0x02001745 RID: 5957
	public new class Instance : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.GameInstance, IShearable
	{
		// Token: 0x0600951A RID: 38170 RVA: 0x0035EE70 File Offset: 0x0035D070
		public Instance(IStateMachineTarget master, WellFedShearable.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
		}

		// Token: 0x0600951B RID: 38171 RVA: 0x0035EEC2 File Offset: 0x0035D0C2
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x0600951C RID: 38172 RVA: 0x0035EED8 File Offset: 0x0035D0D8
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			if (base.def.requiredDiet != null && caloriesConsumedEvent.tag != base.def.requiredDiet)
			{
				return;
			}
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x0600951D RID: 38173 RVA: 0x0035EF84 File Offset: 0x0035D184
		public void Shear()
		{
			PrimaryElement component = base.smi.GetComponent<PrimaryElement>();
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.itemDroppedOnShear), null, null);
			gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)), Grid.SceneLayer.Ore));
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			component2.Temperature = component.Temperature;
			component2.Mass = base.def.dropMass;
			component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
			gameObject.SetActive(true);
			Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, UnityEngine.Random.value * 2f + 2f);
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, initial_velocity);
			this.scaleGrowth.value = 0f;
			WellFedShearable.UpdateScales(this, 0f);
		}

		// Token: 0x0400723E RID: 29246
		[MyCmpGet]
		private Effects effects;

		// Token: 0x0400723F RID: 29247
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04007240 RID: 29248
		public AmountInstance scaleGrowth;

		// Token: 0x04007241 RID: 29249
		public int currentScaleLevel = -1;
	}
}
