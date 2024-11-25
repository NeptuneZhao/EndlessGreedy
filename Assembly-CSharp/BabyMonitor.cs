﻿using System;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007EF RID: 2031
public class BabyMonitor : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>
{
	// Token: 0x0600382E RID: 14382 RVA: 0x00132FC8 File Offset: 0x001311C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.baby;
		this.root.Enter(new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State.Callback(BabyMonitor.AddBabyEffect));
		this.baby.Transition(this.spawnadult, new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.Transition.ConditionCallback(BabyMonitor.IsReadyToSpawnAdult), UpdateRate.SIM_4000ms);
		this.spawnadult.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, (BabyMonitor.Instance smi) => true, null);
		this.babyEffect = new Effect("IsABaby", CREATURES.MODIFIERS.BABY.NAME, CREATURES.MODIFIERS.BABY.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.9f, CREATURES.MODIFIERS.BABY.NAME, true, false, true));
		this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, CREATURES.MODIFIERS.BABY.NAME, false, false, true));
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x001330EE File Offset: 0x001312EE
	private static void AddBabyEffect(BabyMonitor.Instance smi)
	{
		smi.Get<Effects>().Add(smi.sm.babyEffect, false);
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x00133108 File Offset: 0x00131308
	private static bool IsReadyToSpawnAdult(BabyMonitor.Instance smi)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(smi.gameObject);
		float num = smi.def.adultThreshold;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 0.005f;
		}
		return amountInstance.value > num;
	}

	// Token: 0x040021BF RID: 8639
	public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State baby;

	// Token: 0x040021C0 RID: 8640
	public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State spawnadult;

	// Token: 0x040021C1 RID: 8641
	public Effect babyEffect;

	// Token: 0x020016C1 RID: 5825
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040070C0 RID: 28864
		public Tag adultPrefab;

		// Token: 0x040070C1 RID: 28865
		public string onGrowDropID;

		// Token: 0x040070C2 RID: 28866
		public bool forceAdultNavType;

		// Token: 0x040070C3 RID: 28867
		public float adultThreshold = 5f;

		// Token: 0x040070C4 RID: 28868
		public Action<GameObject> configureAdultOnMaturation;
	}

	// Token: 0x020016C2 RID: 5826
	public new class Instance : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.GameInstance
	{
		// Token: 0x06009375 RID: 37749 RVA: 0x0035940E File Offset: 0x0035760E
		public Instance(IStateMachineTarget master, BabyMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009376 RID: 37750 RVA: 0x00359418 File Offset: 0x00357618
		public void SpawnAdult()
		{
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.smi.def.adultPrefab), position);
			gameObject.SetActive(true);
			if (!base.smi.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			}
			if (base.smi.def.onGrowDropID != null)
			{
				Util.KInstantiate(Assets.GetPrefab(base.smi.def.onGrowDropID), position).SetActive(true);
			}
			foreach (AmountInstance amountInstance in base.gameObject.GetAmounts())
			{
				AmountInstance amountInstance2 = amountInstance.amount.Lookup(gameObject);
				if (amountInstance2 != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					amountInstance2.value = num * amountInstance2.GetMax();
				}
			}
			EffectInstance effectInstance = base.gameObject.GetComponent<Effects>().Get("AteFromFeeder");
			if (effectInstance != null)
			{
				gameObject.GetComponent<Effects>().Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			if (!base.smi.def.forceAdultNavType)
			{
				Navigator component = base.smi.GetComponent<Navigator>();
				gameObject.GetComponent<Navigator>().SetCurrentNavType(component.CurrentNavType);
			}
			gameObject.Trigger(-2027483228, base.gameObject);
			KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component2)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			}
			base.smi.gameObject.Trigger(663420073, gameObject);
			base.smi.gameObject.DeleteObject();
			if (base.def.configureAdultOnMaturation != null)
			{
				base.def.configureAdultOnMaturation(gameObject);
			}
		}
	}
}
