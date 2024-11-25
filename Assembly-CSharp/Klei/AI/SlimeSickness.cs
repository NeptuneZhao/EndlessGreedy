using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F52 RID: 3922
	public class SlimeSickness : Sickness
	{
		// Token: 0x060078A3 RID: 30883 RVA: 0x002FBA04 File Offset: 0x002F9C04
		public SlimeSickness() : base("SlimeSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 2220f, "SlimeSicknessRecovery")
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier("BreathDelta", DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * -1.25f, DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true),
				new AttributeModifier("Athletics", -3f, DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_sick_kanim"
			}, Db.Get().Expressions.Sick));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Sick, 50f));
			base.AddSicknessComponent(new SlimeSickness.SlimeLungComponent());
		}

		// Token: 0x04005A1E RID: 23070
		private const float COUGH_FREQUENCY = 20f;

		// Token: 0x04005A1F RID: 23071
		private const float COUGH_MASS = 0.1f;

		// Token: 0x04005A20 RID: 23072
		private const int DISEASE_AMOUNT = 1000;

		// Token: 0x04005A21 RID: 23073
		public const string ID = "SlimeSickness";

		// Token: 0x04005A22 RID: 23074
		public const string RECOVERY_ID = "SlimeSicknessRecovery";

		// Token: 0x02002340 RID: 9024
		public class SlimeLungComponent : Sickness.SicknessComponent
		{
			// Token: 0x0600B608 RID: 46600 RVA: 0x003C91F0 File Offset: 0x003C73F0
			public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
			{
				SlimeSickness.SlimeLungComponent.StatesInstance statesInstance = new SlimeSickness.SlimeLungComponent.StatesInstance(diseaseInstance);
				statesInstance.StartSM();
				return statesInstance;
			}

			// Token: 0x0600B609 RID: 46601 RVA: 0x003C91FE File Offset: 0x003C73FE
			public override void OnCure(GameObject go, object instance_data)
			{
				((SlimeSickness.SlimeLungComponent.StatesInstance)instance_data).StopSM("Cured");
			}

			// Token: 0x0600B60A RID: 46602 RVA: 0x003C9210 File Offset: 0x003C7410
			public override List<Descriptor> GetSymptoms()
			{
				return new List<Descriptor>
				{
					new Descriptor(DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM, DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM_TOOLTIP, Descriptor.DescriptorType.SymptomAidable, false)
				};
			}

			// Token: 0x02003512 RID: 13586
			public class StatesInstance : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.GameInstance
			{
				// Token: 0x0600DED7 RID: 57047 RVA: 0x00431907 File Offset: 0x0042FB07
				public StatesInstance(SicknessInstance master) : base(master)
				{
				}

				// Token: 0x0600DED8 RID: 57048 RVA: 0x00431910 File Offset: 0x0042FB10
				public Reactable GetReactable()
				{
					Emote cough = Db.Get().Emotes.Minion.Cough;
					SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "SlimeLungCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
					selfEmoteReactable.SetEmote(cough);
					selfEmoteReactable.RegisterEmoteStepCallbacks("react", null, new Action<GameObject>(this.FinishedCoughing));
					return selfEmoteReactable;
				}

				// Token: 0x0600DED9 RID: 57049 RVA: 0x00431998 File Offset: 0x0042FB98
				private void ProduceSlime(GameObject cougher)
				{
					AmountInstance amountInstance = Db.Get().Amounts.Temperature.Lookup(cougher);
					int gameCell = Grid.PosToCell(cougher);
					string id = Db.Get().Diseases.SlimeGerms.Id;
					Equippable equippable = base.master.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						equippable.GetComponent<Storage>().AddGasChunk(SimHashes.ContaminatedOxygen, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex(id), 1000, false, true);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.Cough, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex(id), 1000, true, -1);
					}
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, base.master.modifier.Name, 1000), cougher.transform, 1.5f, false);
				}

				// Token: 0x0600DEDA RID: 57050 RVA: 0x00431AB6 File Offset: 0x0042FCB6
				private void FinishedCoughing(GameObject cougher)
				{
					this.ProduceSlime(cougher);
					base.sm.coughFinished.Trigger(this);
				}

				// Token: 0x0400D755 RID: 55125
				public float lastCoughTime;
			}

			// Token: 0x02003513 RID: 13587
			public class States : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance>
			{
				// Token: 0x0600DEDB RID: 57051 RVA: 0x00431AD0 File Offset: 0x0042FCD0
				public override void InitializeStates(out StateMachine.BaseState default_state)
				{
					default_state = this.breathing;
					this.breathing.DefaultState(this.breathing.normal).TagTransition(GameTags.NoOxygen, this.notbreathing, false);
					this.breathing.normal.Enter("SetCoughTime", delegate(SlimeSickness.SlimeLungComponent.StatesInstance smi)
					{
						if (smi.lastCoughTime < Time.time)
						{
							smi.lastCoughTime = Time.time;
						}
					}).Update("Cough", delegate(SlimeSickness.SlimeLungComponent.StatesInstance smi, float dt)
					{
						if (!smi.master.IsDoctored && Time.time - smi.lastCoughTime > 20f)
						{
							smi.GoTo(this.breathing.cough);
						}
					}, UpdateRate.SIM_4000ms, false);
					this.breathing.cough.ToggleReactable((SlimeSickness.SlimeLungComponent.StatesInstance smi) => smi.GetReactable()).OnSignal(this.coughFinished, this.breathing.normal);
					this.notbreathing.TagTransition(new Tag[]
					{
						GameTags.NoOxygen
					}, this.breathing, true);
				}

				// Token: 0x0400D756 RID: 55126
				public StateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.Signal coughFinished;

				// Token: 0x0400D757 RID: 55127
				public SlimeSickness.SlimeLungComponent.States.BreathingStates breathing;

				// Token: 0x0400D758 RID: 55128
				public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State notbreathing;

				// Token: 0x02003854 RID: 14420
				public class BreathingStates : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State
				{
					// Token: 0x0400DF98 RID: 57240
					public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State normal;

					// Token: 0x0400DF99 RID: 57241
					public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State cough;
				}
			}
		}
	}
}
