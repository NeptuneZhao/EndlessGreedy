using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class StressShockChore : Chore<StressShockChore.StatesInstance>
{
	// Token: 0x0600177A RID: 6010 RVA: 0x0007F4B8 File Offset: 0x0007D6B8
	private static bool CheckBlocked(int sourceCell, int destinationCell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		Grid.CollectCellsInLine(sourceCell, destinationCell, hashSet);
		bool result = false;
		foreach (int i in hashSet)
		{
			if (Grid.Solid[i])
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x0007F524 File Offset: 0x0007D724
	public static void AddBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(true);
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x0007F52D File Offset: 0x0007D72D
	public static void RemoveBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(false);
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x0007F538 File Offset: 0x0007D738
	public StressShockChore(ChoreType chore_type, IStateMachineTarget target, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.StressShock, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressShockChore.StatesInstance(this, target.gameObject, notification);
	}

	// Token: 0x02001206 RID: 4614
	public class StatesInstance : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.GameInstance
	{
		// Token: 0x060081EF RID: 33263 RVA: 0x0031A458 File Offset: 0x00318658
		public StatesInstance(StressShockChore master, GameObject shocker, Notification notification) : base(master)
		{
			base.sm.shocker.Set(shocker, base.smi, false);
			this.notification = notification;
		}

		// Token: 0x060081F0 RID: 33264 RVA: 0x0031A4D0 File Offset: 0x003186D0
		public void SetDrainModifierActiveState(bool draining)
		{
			if (draining)
			{
				this.batteryMonitor.AddOrUpdateModifier(this.powerDrainModifier, true);
				return;
			}
			this.batteryMonitor.RemoveModifier(this.powerDrainModifier.id, true);
		}

		// Token: 0x060081F1 RID: 33265 RVA: 0x0031A504 File Offset: 0x00318704
		public void FindDestination()
		{
			int value = this.FindIdleCell();
			base.sm.targetMoveLocation.Set(value, base.smi, false);
			this.GoTo(base.sm.shocking.runAroundShockingStuff);
		}

		// Token: 0x060081F2 RID: 33266 RVA: 0x0031A548 File Offset: 0x00318748
		private int FindIdleCell()
		{
			Navigator component = base.smi.master.GetComponent<Navigator>();
			MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)component.GetCurrentAbilities();
			minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
			IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(90, 180));
			component.RunQuery(idleCellQuery);
			minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
			return idleCellQuery.GetResultCell();
		}

		// Token: 0x060081F3 RID: 33267 RVA: 0x0031A5A8 File Offset: 0x003187A8
		public void PickShockTarget(StressShockChore.StatesInstance smi)
		{
			int num = Grid.PosToCell(smi.master.gameObject);
			int worldId = (int)Grid.WorldIdx[num];
			List<GameObject> list = new List<GameObject>();
			float num2 = UnityEngine.Random.Range(0f, 2f);
			foreach (Health health in Components.Health.GetWorldItems(worldId, false))
			{
				if (!health.IsNullOrDestroyed() && !(health.gameObject == smi.master.gameObject))
				{
					int num3 = Grid.PosToCell(health);
					float num4 = Vector2.Distance(Grid.CellToPos2D(num), Grid.CellToPos2D(num3));
					if (num4 <= (float)STRESS.SHOCKER.SHOCK_RADIUS && num4 > num2 && !StressShockChore.CheckBlocked(num, num3))
					{
						list.Add(health.gameObject);
					}
				}
			}
			Vector2I vector2I = Grid.CellToXY(num);
			List<ScenePartitionerEntry> list2 = new List<ScenePartitionerEntry>();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x - STRESS.SHOCKER.SHOCK_RADIUS, vector2I.y - STRESS.SHOCKER.SHOCK_RADIUS, STRESS.SHOCKER.SHOCK_RADIUS * 2, STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.objectLayers[42], list2);
			foreach (ScenePartitionerEntry scenePartitionerEntry in list2)
			{
				if (!StressShockChore.CheckBlocked(num, Grid.PosToCell(new Vector2((float)scenePartitionerEntry.x, (float)scenePartitionerEntry.y))) && scenePartitionerEntry.obj as GameObject != null)
				{
					list.Add(scenePartitionerEntry.obj as GameObject);
				}
			}
			if (list.Count == 0)
			{
				Vector2I vector2I2 = Grid.CellToXY(num);
				List<ScenePartitionerEntry> list3 = new List<ScenePartitionerEntry>();
				GameScenePartitioner.Instance.GatherEntries(vector2I2.x - STRESS.SHOCKER.SHOCK_RADIUS, vector2I2.y - STRESS.SHOCKER.SHOCK_RADIUS, STRESS.SHOCKER.SHOCK_RADIUS * 2, STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.completeBuildings, list3);
				foreach (ScenePartitionerEntry scenePartitionerEntry2 in list3)
				{
					if (!StressShockChore.CheckBlocked(num, Grid.PosToCell(new Vector2((float)scenePartitionerEntry2.x, (float)scenePartitionerEntry2.y))))
					{
						BuildingComplete buildingComplete = scenePartitionerEntry2.obj as BuildingComplete;
						if (buildingComplete != null)
						{
							list.Add(buildingComplete.gameObject);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			GameObject random = list.GetRandom<GameObject>();
			GameObject gameObject = random;
			float num5 = float.MaxValue;
			foreach (GameObject gameObject2 in list)
			{
				if (list.Count <= 1 || !(gameObject2 == base.sm.previousTarget.Get(smi)))
				{
					float num6 = Vector2.Distance(base.transform.position, gameObject2.transform.position);
					if (num6 < num5)
					{
						num5 = num6;
						gameObject = gameObject2;
					}
				}
			}
			if (random != null && gameObject != null && UnityEngine.Random.Range(0, 100) > 50)
			{
				base.sm.beamTarget.Set(gameObject, smi, false);
				return;
			}
			base.sm.beamTarget.Set(gameObject, smi, false);
		}

		// Token: 0x060081F4 RID: 33268 RVA: 0x0031A940 File Offset: 0x00318B40
		public void MakeBeam()
		{
			GameObject gameObject = new GameObject("shockFX");
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			base.sm.beamFX.Set(kbatchedAnimController, base.smi, false);
			kbatchedAnimController.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_fx_kanim")
			});
			gameObject.SetActive(true);
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("snapTo_hat", out flag).GetColumn(3);
			vector -= Vector3.up / 4f;
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
			gameObject.transform.position = vector;
			gameObject.transform.parent = base.transform;
			kbatchedAnimController.Play("lightning_beam_comp", KAnim.PlayMode.Loop, 1f, 0f);
			GameObject gameObject2 = new GameObject("impactFX");
			gameObject2.SetActive(false);
			KBatchedAnimController kbatchedAnimController2 = gameObject2.AddComponent<KBatchedAnimController>();
			base.sm.impactFX.Set(kbatchedAnimController2, base.smi, false);
			kbatchedAnimController2.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_impact_fx_kanim")
			});
			gameObject2.SetActive(true);
			kbatchedAnimController2.Play("stress_beam_impact_fx", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x060081F5 RID: 33269 RVA: 0x0031AAA4 File Offset: 0x00318CA4
		public void ClearBeam(int beamIdx)
		{
			base.sm.previousTarget.Set(base.sm.beamTarget.Get(base.smi), base.smi, false);
			base.sm.beamTarget.Set(null, base.smi, false);
			if (base.sm.beamFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.beamFX.Get(base.smi).gameObject);
				base.sm.beamFX.Set(null, base.smi, false);
			}
			if (base.sm.impactFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.impactFX.Get(base.smi).gameObject);
				base.sm.impactFX.Set(null, base.smi, false);
			}
		}

		// Token: 0x060081F6 RID: 33270 RVA: 0x0031ABA8 File Offset: 0x00318DA8
		public void AimBeam(Vector3 targetPosition, int beamIdx)
		{
			Vector3 v = Vector3.Normalize(targetPosition - base.smi.sm.beamFX.Get(base.smi).transform.position);
			float rotation = MathUtil.AngleSigned(Vector3.up, v, Vector3.forward) + 90f;
			base.smi.sm.beamFX.Get(base.smi).Rotation = rotation;
			float animWidth = Vector2.Distance(base.smi.sm.beamTarget.Get(base.smi).transform.position, base.smi.sm.beamFX.Get(base.smi).transform.position) / 2.5f;
			base.smi.sm.beamFX.Get(base.smi).animWidth = animWidth;
			base.smi.sm.impactFX.Get(base.smi).transform.position = targetPosition;
		}

		// Token: 0x060081F7 RID: 33271 RVA: 0x0031ACC8 File Offset: 0x00318EC8
		public void ShowBeam(bool show)
		{
			base.smi.sm.impactFX.Get(base.smi).enabled = show;
			base.smi.sm.beamFX.Get(base.smi).enabled = show;
		}

		// Token: 0x0400621D RID: 25117
		public Notification notification;

		// Token: 0x0400621E RID: 25118
		[MySmiReq]
		public BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x0400621F RID: 25119
		public BionicBatteryMonitor.WattageModifier powerDrainModifier = new BionicBatteryMonitor.WattageModifier("StressShockChore", string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, DUPLICANTS.TRAITS.STRESSSHOCKER.DRAIN_ATTRIBUTE, "<b>+</b>" + GameUtil.GetFormattedWattage(STRESS.SHOCKER.POWER_CONSUMPTION_RATE, GameUtil.WattageFormatterUnit.Automatic, true)), STRESS.SHOCKER.POWER_CONSUMPTION_RATE, STRESS.SHOCKER.POWER_CONSUMPTION_RATE);
	}

	// Token: 0x02001207 RID: 4615
	public class States : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore>
	{
		// Token: 0x060081F8 RID: 33272 RVA: 0x0031AD18 File Offset: 0x00318F18
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.shocking.findDestination;
			base.serializable = StateMachine.SerializeType.Never;
			base.Target(this.shocker);
			this.shocking.DefaultState(this.shocking.findDestination).ToggleAnims("anim_loco_stressshocker_kanim", 0f).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.MakeBeam();
			}).Exit(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ClearBeam(0);
			});
			this.shocking.findDestination.Enter("FindDestination", delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(false);
				smi.FindDestination();
			});
			this.shocking.runAroundShockingStuff.MoveTo((StressShockChore.StatesInstance smi) => smi.sm.targetMoveLocation.Get(smi), this.shocking.findDestination, this.delay, false).ParamTransition<float>(this.powerConsumed, this.complete, (StressShockChore.StatesInstance smi, float p) => p >= STRESS.SHOCKER.MAX_POWER_USE).Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(true);
			}).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.PickShockTarget(smi);
				float num = dt * STRESS.SHOCKER.POWER_CONSUMPTION_RATE;
				smi.sm.powerConsumed.Delta(-num, smi);
				smi.batteryMonitor.ConsumePower(-num);
				if (smi.sm.beamTarget.Get(smi) != null)
				{
					Health component = smi.sm.beamTarget.Get(smi).GetComponent<Health>();
					if (component != null)
					{
						component.Damage(dt * STRESS.SHOCKER.DAMAGE_RATE);
						return;
					}
					if (smi.sm.beamTarget.Get(smi).HasTag(GameTags.Wires))
					{
						BuildingHP component2 = smi.sm.beamTarget.Get(smi).GetComponent<BuildingHP>();
						if (component2 != null)
						{
							component2.DoDamage(Mathf.RoundToInt(dt * STRESS.SHOCKER.DAMAGE_RATE));
						}
					}
				}
			}, UpdateRate.SIM_200ms, false).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				if (smi.sm.beamTarget.Get(smi) != null)
				{
					Vector3 vector = smi.sm.beamTarget.Get(smi).transform.position + Vector3.up / 2f;
					if (!StressShockChore.CheckBlocked(Grid.PosToCell(smi.sm.beamFX.Get(smi).transform.position), Grid.PosToCell(vector)))
					{
						smi.AimBeam(vector, 0);
					}
				}
			}, UpdateRate.RENDER_EVERY_TICK, false);
			this.delay.ScheduleGoTo(0.5f, this.shocking);
			this.complete.Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x04006220 RID: 25120
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.TargetParameter shocker;

		// Token: 0x04006221 RID: 25121
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController[]> cosmeticBeamFXs;

		// Token: 0x04006222 RID: 25122
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> beamFX;

		// Token: 0x04006223 RID: 25123
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> impactFX;

		// Token: 0x04006224 RID: 25124
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> beamTarget;

		// Token: 0x04006225 RID: 25125
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> previousTarget;

		// Token: 0x04006226 RID: 25126
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.IntParameter targetMoveLocation;

		// Token: 0x04006227 RID: 25127
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.FloatParameter powerConsumed;

		// Token: 0x04006228 RID: 25128
		public StressShockChore.States.ShockStates shocking;

		// Token: 0x04006229 RID: 25129
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State delay;

		// Token: 0x0400622A RID: 25130
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State complete;

		// Token: 0x020023EE RID: 9198
		public class ShockStates : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State
		{
			// Token: 0x0400A06A RID: 41066
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State findDestination;

			// Token: 0x0400A06B RID: 41067
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State runAroundShockingStuff;
		}
	}
}
