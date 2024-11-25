using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000811 RID: 2065
public class IncubationMonitor : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>
{
	// Token: 0x06003925 RID: 14629 RVA: 0x001377BC File Offset: 0x001359BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubating;
		this.root.Enter(delegate(IncubationMonitor.Instance smi)
		{
			smi.OnOperationalChanged(null);
		}).Enter(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Add(smi);
		}).Exit(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Remove(smi);
		});
		this.incubating.PlayAnim("idle", KAnim.PlayMode.Loop).Transition(this.hatching_pre, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.IsReadyToHatch), UpdateRate.SIM_1000ms).TagTransition(GameTags.Entombed, this.entombed, false).ParamTransition<bool>(this.isSuppressed, this.suppressed, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsTrue).ToggleEffect((IncubationMonitor.Instance smi) => smi.incubatingEffect);
		this.entombed.TagTransition(GameTags.Entombed, this.incubating, true);
		this.suppressed.ToggleEffect((IncubationMonitor.Instance smi) => this.suppressedEffect).ParamTransition<bool>(this.isSuppressed, this.incubating, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsFalse).TagTransition(GameTags.Entombed, this.entombed, false).Transition(this.not_viable, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.NoLongerViable), UpdateRate.SIM_1000ms);
		this.hatching_pre.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DropSelfFromStorage)).PlayAnim("hatching_pre").OnAnimQueueComplete(this.hatching_pst);
		this.hatching_pst.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnBaby)).PlayAnim("hatching_pst").OnAnimQueueComplete(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.not_viable.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnGenericEgg)).GoTo(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.suppressedEffect = new Effect("IncubationSuppressed", CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.suppressedEffect.Add(new AttributeModifier(Db.Get().Amounts.Viability.deltaAttribute.Id, -0.016666668f, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, false, false, true));
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x00137A35 File Offset: 0x00135C35
	private static bool IsReadyToHatch(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.incubation.value >= smi.incubation.GetMax();
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x00137A68 File Offset: 0x00135C68
	private static void SpawnBaby(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.spawnedCreature), position);
		gameObject.SetActive(true);
		gameObject.GetSMI<AnimInterruptMonitor.Instance>().Play("hatching_pst", KAnim.PlayMode.Once);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		Db.Get().Amounts.Wildness.Copy(gameObject, smi.gameObject);
		if (smi.incubator != null)
		{
			smi.incubator.StoreBaby(gameObject);
		}
		IncubationMonitor.SpawnShell(smi);
		SaveLoader.Instance.saveManager.Unregister(smi.GetComponent<SaveLoadRoot>());
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x00137B5D File Offset: 0x00135D5D
	private static bool NoLongerViable(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.viability.value <= smi.viability.GetMin();
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x00137B90 File Offset: 0x00135D90
	private static GameObject SpawnShell(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EggShell"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x00137BE4 File Offset: 0x00135DE4
	private static GameObject SpawnEggInnards(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RawEgg"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x00137C38 File Offset: 0x00135E38
	private static void SpawnGenericEgg(IncubationMonitor.Instance smi)
	{
		IncubationMonitor.SpawnShell(smi);
		GameObject gameObject = IncubationMonitor.SpawnEggInnards(smi);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x00137CA1 File Offset: 0x00135EA1
	private static void DeleteSelf(IncubationMonitor.Instance smi)
	{
		smi.gameObject.DeleteObject();
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x00137CB0 File Offset: 0x00135EB0
	private static void DropSelfFromStorage(IncubationMonitor.Instance smi)
	{
		if (!smi.sm.inIncubator.Get(smi))
		{
			Storage storage = smi.GetStorage();
			if (storage)
			{
				storage.Drop(smi.gameObject, true);
			}
			smi.gameObject.AddTag(GameTags.StoredPrivate);
		}
	}

	// Token: 0x0400225D RID: 8797
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter incubatorIsActive;

	// Token: 0x0400225E RID: 8798
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter inIncubator;

	// Token: 0x0400225F RID: 8799
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter isSuppressed;

	// Token: 0x04002260 RID: 8800
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State incubating;

	// Token: 0x04002261 RID: 8801
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State entombed;

	// Token: 0x04002262 RID: 8802
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State suppressed;

	// Token: 0x04002263 RID: 8803
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pre;

	// Token: 0x04002264 RID: 8804
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pst;

	// Token: 0x04002265 RID: 8805
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State not_viable;

	// Token: 0x04002266 RID: 8806
	private Effect suppressedEffect;

	// Token: 0x02001712 RID: 5906
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009482 RID: 38018 RVA: 0x0035CAEC File Offset: 0x0035ACEC
		public override void Configure(GameObject prefab)
		{
			List<string> initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
			initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
			initialAmounts.Add(Db.Get().Amounts.Incubation.Id);
			initialAmounts.Add(Db.Get().Amounts.Viability.Id);
		}

		// Token: 0x0400719D RID: 29085
		public float baseIncubationRate;

		// Token: 0x0400719E RID: 29086
		public Tag spawnedCreature;
	}

	// Token: 0x02001713 RID: 5907
	public new class Instance : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.GameInstance
	{
		// Token: 0x06009484 RID: 38020 RVA: 0x0035CB5C File Offset: 0x0035AD5C
		public Instance(IStateMachineTarget master, IncubationMonitor.Def def) : base(master, def)
		{
			this.incubation = Db.Get().Amounts.Incubation.Lookup(base.gameObject);
			Action<object> handler = new Action<object>(this.OnStore);
			master.Subscribe(856640610, handler);
			master.Subscribe(1309017699, handler);
			Action<object> handler2 = new Action<object>(this.OnOperationalChanged);
			master.Subscribe(1628751838, handler2);
			master.Subscribe(960378201, handler2);
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
			this.viability = Db.Get().Amounts.Viability.Lookup(base.gameObject);
			this.viability.value = this.viability.GetMax();
			float value = def.baseIncubationRate;
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				value = 33.333332f;
			}
			AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_INCUBATION_RATE.NAME, false, false, true);
			this.incubatingEffect = new Effect("Incubating", CREATURES.MODIFIERS.INCUBATING.NAME, CREATURES.MODIFIERS.INCUBATING.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
			this.incubatingEffect.Add(modifier);
		}

		// Token: 0x06009485 RID: 38021 RVA: 0x0035CCDA File Offset: 0x0035AEDA
		public Storage GetStorage()
		{
			if (!(base.transform.parent != null))
			{
				return null;
			}
			return base.transform.parent.GetComponent<Storage>();
		}

		// Token: 0x06009486 RID: 38022 RVA: 0x0035CD04 File Offset: 0x0035AF04
		public void OnStore(object data)
		{
			Storage storage = data as Storage;
			bool stored = storage || (data != null && (bool)data);
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06009487 RID: 38023 RVA: 0x0035CD4C File Offset: 0x0035AF4C
		public void OnOperationalChanged(object data = null)
		{
			bool stored = base.gameObject.HasTag(GameTags.Stored);
			Storage storage = this.GetStorage();
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06009488 RID: 38024 RVA: 0x0035CD8C File Offset: 0x0035AF8C
		private void UpdateIncubationState(bool stored, EggIncubator incubator)
		{
			this.incubator = incubator;
			base.smi.sm.inIncubator.Set(incubator != null, base.smi, false);
			bool value = stored && !incubator;
			base.smi.sm.isSuppressed.Set(value, base.smi, false);
			Operational operational = incubator ? incubator.GetComponent<Operational>() : null;
			bool value2 = incubator && (operational == null || operational.IsOperational);
			base.smi.sm.incubatorIsActive.Set(value2, base.smi, false);
		}

		// Token: 0x06009489 RID: 38025 RVA: 0x0035CE40 File Offset: 0x0035B040
		public void ApplySongBuff()
		{
			base.GetComponent<Effects>().Add("EggSong", true);
		}

		// Token: 0x0600948A RID: 38026 RVA: 0x0035CE54 File Offset: 0x0035B054
		public bool HasSongBuff()
		{
			return base.GetComponent<Effects>().HasEffect("EggSong");
		}

		// Token: 0x0400719F RID: 29087
		public AmountInstance incubation;

		// Token: 0x040071A0 RID: 29088
		public AmountInstance wildness;

		// Token: 0x040071A1 RID: 29089
		public AmountInstance viability;

		// Token: 0x040071A2 RID: 29090
		public EggIncubator incubator;

		// Token: 0x040071A3 RID: 29091
		public Effect incubatingEffect;
	}
}
