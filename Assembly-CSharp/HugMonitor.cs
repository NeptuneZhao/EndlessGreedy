using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class HugMonitor : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
	// Token: 0x06001F38 RID: 7992 RVA: 0x000AF01C File Offset: 0x000AD21C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<HugMonitor.Instance, float>(this.UpdateHugEggCooldownTimer), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (HugMonitor.Instance smi) => smi.UpdateHasTarget(), delegate(HugMonitor.Instance smi)
		{
			smi.hugTarget = null;
		});
		this.normal.DefaultState(this.normal.idle).ParamTransition<float>(this.hugFrenzyTimer, this.hugFrenzy, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsGTZero);
		this.normal.idle.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		this.normal.hugReady.ToggleReactable(new Func<HugMonitor.Instance, Reactable>(this.GetHugReactable));
		GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State state = this.normal.hugReady.passiveHug.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (HugMonitor.Instance smi) => true, delegate(HugMonitor.Instance smi)
		{
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi, false);
			smi.GoTo(this.normal.hugReady.passiveHug);
		});
		this.hugFrenzy.ParamTransition<float>(this.hugFrenzyTimer, this.normal, (HugMonitor.Instance smi, float p) => p <= 0f && !smi.IsHugging()).Update(new Action<HugMonitor.Instance, float>(this.UpdateHugFrenzyTimer), UpdateRate.SIM_1000ms, false).ToggleEffect((HugMonitor.Instance smi) => smi.frenzyEffect).ToggleLoopingSound(HugMonitor.soundPath, null, true, true, true).Enter(delegate(HugMonitor.Instance smi)
		{
			smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
			smi.hugParticleFx.transform.SetParent(smi.master.transform);
			smi.hugParticleFx.SetActive(true);
		}).Exit(delegate(HugMonitor.Instance smi)
		{
			Util.KDestroyGameObject(smi.hugParticleFx);
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi, false);
		});
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x000AF2A0 File Offset: 0x000AD4A0
	private Reactable GetHugReactable(HugMonitor.Instance smi)
	{
		return new HugMinionReactable(smi.gameObject);
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x000AF2AD File Offset: 0x000AD4AD
	private void UpdateWantsHugCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.wantsHugCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x000AF2C8 File Offset: 0x000AD4C8
	private void UpdateHugEggCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugEggCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x000AF2E3 File Offset: 0x000AD4E3
	private void UpdateHugFrenzyTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugFrenzyTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x04001196 RID: 4502
	private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX", false);

	// Token: 0x04001197 RID: 4503
	private static Effect hugEffect;

	// Token: 0x04001198 RID: 4504
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugFrenzyTimer;

	// Token: 0x04001199 RID: 4505
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter wantsHugCooldownTimer;

	// Token: 0x0400119A RID: 4506
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugEggCooldownTimer;

	// Token: 0x0400119B RID: 4507
	public HugMonitor.NormalStates normal;

	// Token: 0x0400119C RID: 4508
	public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State hugFrenzy;

	// Token: 0x0200133B RID: 4923
	public class HUGTUNING
	{
		// Token: 0x040065F7 RID: 26103
		public const float HUG_EGG_TIME = 15f;

		// Token: 0x040065F8 RID: 26104
		public const float HUG_DUPE_WAIT = 60f;

		// Token: 0x040065F9 RID: 26105
		public const float FRENZY_EGGS_PER_CYCLE = 6f;

		// Token: 0x040065FA RID: 26106
		public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;

		// Token: 0x040065FB RID: 26107
		public const float HUG_FRENZY_DURATION = 120f;
	}

	// Token: 0x0200133C RID: 4924
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065FC RID: 26108
		public float hugsPerCycle = 2f;

		// Token: 0x040065FD RID: 26109
		public float scanningInterval = 30f;

		// Token: 0x040065FE RID: 26110
		public float hugFrenzyDuration = 120f;

		// Token: 0x040065FF RID: 26111
		public float hugFrenzyCooldown = 480f;

		// Token: 0x04006600 RID: 26112
		public float hugFrenzyCooldownFailed = 120f;

		// Token: 0x04006601 RID: 26113
		public float scanningIntervalFrenzy = 15f;

		// Token: 0x04006602 RID: 26114
		public int maxSearchCost = 30;
	}

	// Token: 0x0200133D RID: 4925
	public class HugReadyStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04006603 RID: 26115
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State passiveHug;

		// Token: 0x04006604 RID: 26116
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State seekingHug;
	}

	// Token: 0x0200133E RID: 4926
	public class NormalStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04006605 RID: 26117
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State idle;

		// Token: 0x04006606 RID: 26118
		public HugMonitor.HugReadyStates hugReady;
	}

	// Token: 0x0200133F RID: 4927
	public new class Instance : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.GameInstance
	{
		// Token: 0x06008650 RID: 34384 RVA: 0x00328CF4 File Offset: 0x00326EF4
		public Instance(IStateMachineTarget master, HugMonitor.Def def) : base(master, def)
		{
			this.frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
			this.RefreshSearchTime();
			if (HugMonitor.hugEffect == null)
			{
				HugMonitor.hugEffect = Db.Get().effects.Get("EggHug");
			}
			base.smi.sm.wantsHugCooldownTimer.Set(UnityEngine.Random.Range(base.smi.def.hugFrenzyCooldownFailed, base.smi.def.hugFrenzyCooldown), base.smi, false);
		}

		// Token: 0x06008651 RID: 34385 RVA: 0x00328D8C File Offset: 0x00326F8C
		private void RefreshSearchTime()
		{
			if (this.hugTarget == null)
			{
				base.smi.sm.hugEggCooldownTimer.Set(this.GetScanningInterval(), base.smi, false);
				return;
			}
			base.smi.sm.hugEggCooldownTimer.Set(this.GetHugInterval(), base.smi, false);
		}

		// Token: 0x06008652 RID: 34386 RVA: 0x00328DEE File Offset: 0x00326FEE
		private float GetScanningInterval()
		{
			if (!this.IsHuggingFrenzy())
			{
				return base.def.scanningInterval;
			}
			return base.def.scanningIntervalFrenzy;
		}

		// Token: 0x06008653 RID: 34387 RVA: 0x00328E0F File Offset: 0x0032700F
		private float GetHugInterval()
		{
			if (this.IsHuggingFrenzy())
			{
				return 0f;
			}
			return 600f / base.def.hugsPerCycle;
		}

		// Token: 0x06008654 RID: 34388 RVA: 0x00328E30 File Offset: 0x00327030
		public bool IsHuggingFrenzy()
		{
			return base.smi.GetCurrentState() == base.smi.sm.hugFrenzy;
		}

		// Token: 0x06008655 RID: 34389 RVA: 0x00328E4F File Offset: 0x0032704F
		public bool IsHugging()
		{
			return base.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;
		}

		// Token: 0x06008656 RID: 34390 RVA: 0x00328E64 File Offset: 0x00327064
		public bool UpdateHasTarget()
		{
			if (this.hugTarget == null)
			{
				if (base.smi.sm.hugEggCooldownTimer.Get(base.smi) > 0f)
				{
					return false;
				}
				this.FindEgg();
				this.RefreshSearchTime();
			}
			return this.hugTarget != null;
		}

		// Token: 0x06008657 RID: 34391 RVA: 0x00328EBC File Offset: 0x003270BC
		public void EnterHuggingFrenzy()
		{
			base.smi.sm.hugFrenzyTimer.Set(base.smi.def.hugFrenzyDuration, base.smi, false);
			base.smi.sm.hugEggCooldownTimer.Set(0f, base.smi, false);
		}

		// Token: 0x06008658 RID: 34392 RVA: 0x00328F18 File Offset: 0x00327118
		private void FindEgg()
		{
			int cell = Grid.PosToCell(base.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			int num = base.def.maxSearchCost;
			this.hugTarget = null;
			if (cavityForCell != null)
			{
				foreach (KPrefabID kprefabID in cavityForCell.eggs)
				{
					if (!kprefabID.HasTag(GameTags.Creatures.ReservedByCreature) && !kprefabID.GetComponent<Effects>().HasEffect(HugMonitor.hugEffect))
					{
						int num2 = Grid.PosToCell(kprefabID);
						if (kprefabID.HasTag(GameTags.Stored))
						{
							GameObject gameObject;
							KPrefabID kprefabID2;
							if (!Grid.ObjectLayers[1].TryGetValue(num2, out gameObject) || !gameObject.TryGetComponent<KPrefabID>(out kprefabID2) || !kprefabID2.IsPrefabID("EggIncubator"))
							{
								continue;
							}
							num2 = Grid.PosToCell(gameObject);
							kprefabID = kprefabID2;
						}
						int navigationCost = this.navigator.GetNavigationCost(num2);
						if (navigationCost != -1 && navigationCost < num)
						{
							this.hugTarget = kprefabID;
							num = navigationCost;
						}
					}
				}
			}
		}

		// Token: 0x04006607 RID: 26119
		public GameObject hugParticleFx;

		// Token: 0x04006608 RID: 26120
		public Vector3 hugParticleOffset;

		// Token: 0x04006609 RID: 26121
		public Effect frenzyEffect;

		// Token: 0x0400660A RID: 26122
		public KPrefabID hugTarget;

		// Token: 0x0400660B RID: 26123
		[MyCmpGet]
		private Navigator navigator;
	}
}
