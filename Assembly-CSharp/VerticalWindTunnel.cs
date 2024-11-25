using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B52 RID: 2898
[SerializationConfig(MemberSerialization.OptIn)]
public class VerticalWindTunnel : StateMachineComponent<VerticalWindTunnel.StatesInstance>, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x0600569B RID: 22171 RVA: 0x001EF22C File Offset: 0x001ED42C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		this.bottomConsumer = components[0];
		this.bottomConsumer.EnableConsumption(false);
		this.bottomConsumer.OnElementConsumed += delegate(Sim.ConsumedMassInfo info)
		{
			this.OnElementConsumed(false, info);
		};
		this.topConsumer = components[1];
		this.topConsumer.EnableConsumption(false);
		this.topConsumer.OnElementConsumed += delegate(Sim.ConsumedMassInfo info)
		{
			this.OnElementConsumed(true, info);
		};
		this.operational = base.GetComponent<Operational>();
	}

	// Token: 0x0600569C RID: 22172 RVA: 0x001EF2AC File Offset: 0x001ED4AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.invalidIntake = this.HasInvalidIntake();
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.WindTunnelIntake, this.invalidIntake, this);
		this.operational.SetFlag(VerticalWindTunnel.validIntakeFlag, !this.invalidIntake);
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new VerticalWindTunnelWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("VerticalWindTunnelWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			VerticalWindTunnelWorkable verticalWindTunnelWorkable = go.AddOrGet<VerticalWindTunnelWorkable>();
			int player_index = i;
			VerticalWindTunnelWorkable verticalWindTunnelWorkable2 = verticalWindTunnelWorkable;
			verticalWindTunnelWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(verticalWindTunnelWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			verticalWindTunnelWorkable.overrideAnim = this.overrideAnims[i];
			verticalWindTunnelWorkable.preAnims = this.workPreAnims[i];
			verticalWindTunnelWorkable.loopAnim = this.workAnims[i];
			verticalWindTunnelWorkable.pstAnims = this.workPstAnims[i];
			this.workables[i] = verticalWindTunnelWorkable;
			this.workables[i].windTunnel = this;
		}
		base.smi.StartSM();
	}

	// Token: 0x0600569D RID: 22173 RVA: 0x001EF458 File Offset: 0x001ED658
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x0600569E RID: 22174 RVA: 0x001EF4AC File Offset: 0x001ED6AC
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<VerticalWindTunnelWorkable> workChore = new WorkChore<VerticalWindTunnelWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x0600569F RID: 22175 RVA: 0x001EF514 File Offset: 0x001ED714
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x060056A0 RID: 22176 RVA: 0x001EF530 File Offset: 0x001ED730
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x060056A1 RID: 22177 RVA: 0x001EF590 File Offset: 0x001ED790
	public void Sim200ms(float dt)
	{
		bool flag = this.HasInvalidIntake();
		if (flag != this.invalidIntake)
		{
			this.invalidIntake = flag;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.WindTunnelIntake, this.invalidIntake, this);
			this.operational.SetFlag(VerticalWindTunnel.validIntakeFlag, !this.invalidIntake);
		}
	}

	// Token: 0x060056A2 RID: 22178 RVA: 0x001EF5F0 File Offset: 0x001ED7F0
	private float GetIntakeRatio(int fromCell, int radius)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = -radius; i < radius; i++)
		{
			for (int j = -radius; j < radius; j++)
			{
				int cell = Grid.OffsetCell(fromCell, j, i);
				if (!Grid.IsSolidCell(cell))
				{
					if (Grid.IsGas(cell))
					{
						num2 += 1f;
					}
					num += 1f;
				}
			}
		}
		return num2 / num;
	}

	// Token: 0x060056A3 RID: 22179 RVA: 0x001EF654 File Offset: 0x001ED854
	private bool HasInvalidIntake()
	{
		Vector3 position = base.transform.GetPosition();
		int cell = Grid.XYToCell((int)position.x, (int)position.y);
		int fromCell = Grid.OffsetCell(cell, (int)this.topConsumer.sampleCellOffset.x, (int)this.topConsumer.sampleCellOffset.y);
		int fromCell2 = Grid.OffsetCell(cell, (int)this.bottomConsumer.sampleCellOffset.x, (int)this.bottomConsumer.sampleCellOffset.y);
		this.avgGasAccumTop += this.GetIntakeRatio(fromCell, (int)this.topConsumer.consumptionRadius);
		this.avgGasAccumBottom += this.GetIntakeRatio(fromCell2, (int)this.bottomConsumer.consumptionRadius);
		int num = 5;
		this.avgGasCounter = (this.avgGasCounter + 1) % num;
		if (this.avgGasCounter == 0)
		{
			double num2 = (double)(this.avgGasAccumTop / (float)num);
			float num3 = this.avgGasAccumBottom / (float)num;
			this.avgGasAccumBottom = 0f;
			this.avgGasAccumTop = 0f;
			return num2 < 0.5 || (double)num3 < 0.5;
		}
		return this.invalidIntake;
	}

	// Token: 0x060056A4 RID: 22180 RVA: 0x001EF778 File Offset: 0x001ED978
	public void SetGasWalls(bool set)
	{
		Building component = base.GetComponent<Building>();
		Sim.Cell.Properties properties = (Sim.Cell.Properties)3;
		Vector3 position = base.transform.GetPosition();
		for (int i = 0; i < component.Def.HeightInCells; i++)
		{
			int gameCell = Grid.XYToCell(Mathf.FloorToInt(position.x) - 2, Mathf.FloorToInt(position.y) + i);
			int gameCell2 = Grid.XYToCell(Mathf.FloorToInt(position.x) + 2, Mathf.FloorToInt(position.y) + i);
			if (set)
			{
				SimMessages.SetCellProperties(gameCell, (byte)properties);
				SimMessages.SetCellProperties(gameCell2, (byte)properties);
			}
			else
			{
				SimMessages.ClearCellProperties(gameCell, (byte)properties);
				SimMessages.ClearCellProperties(gameCell2, (byte)properties);
			}
		}
	}

	// Token: 0x060056A5 RID: 22181 RVA: 0x001EF81C File Offset: 0x001EDA1C
	private void OnElementConsumed(bool isTop, Sim.ConsumedMassInfo info)
	{
		Building component = base.GetComponent<Building>();
		Vector3 position = base.transform.GetPosition();
		CellOffset offset = isTop ? new CellOffset(0, component.Def.HeightInCells + 1) : new CellOffset(0, 0);
		SimMessages.AddRemoveSubstance(Grid.OffsetCell(Grid.XYToCell((int)position.x, (int)position.y), offset), info.removedElemIdx, CellEventLogger.Instance.ElementEmitted, info.mass, info.temperature, info.diseaseIdx, info.diseaseCount, true, -1);
	}

	// Token: 0x060056A6 RID: 22182 RVA: 0x001EF8A4 File Offset: 0x001EDAA4
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.players.Add(player);
		}
		else
		{
			this.players.Remove(player);
		}
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x060056A7 RID: 22183 RVA: 0x001EF8FC File Offset: 0x001EDAFC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(BUILDINGS.PREFABS.VERTICALWINDTUNNEL.DISPLACEMENTEFFECT.Replace("{amount}", GameUtil.GetFormattedMass(this.displacementAmount_DescriptorOnly, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.VERTICALWINDTUNNEL.DISPLACEMENTEFFECT_TOOLTIP.Replace("{amount}", GameUtil.GetFormattedMass(this.displacementAmount_DescriptorOnly, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x040038A4 RID: 14500
	public string specificEffect;

	// Token: 0x040038A5 RID: 14501
	public string trackingEffect;

	// Token: 0x040038A6 RID: 14502
	public int basePriority;

	// Token: 0x040038A7 RID: 14503
	public float displacementAmount_DescriptorOnly;

	// Token: 0x040038A8 RID: 14504
	public static readonly Operational.Flag validIntakeFlag = new Operational.Flag("valid_intake", Operational.Flag.Type.Requirement);

	// Token: 0x040038A9 RID: 14505
	private bool invalidIntake;

	// Token: 0x040038AA RID: 14506
	private float avgGasAccumTop;

	// Token: 0x040038AB RID: 14507
	private float avgGasAccumBottom;

	// Token: 0x040038AC RID: 14508
	private int avgGasCounter;

	// Token: 0x040038AD RID: 14509
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040038AE RID: 14510
	private VerticalWindTunnelWorkable[] workables;

	// Token: 0x040038AF RID: 14511
	private Chore[] chores;

	// Token: 0x040038B0 RID: 14512
	private ElementConsumer bottomConsumer;

	// Token: 0x040038B1 RID: 14513
	private ElementConsumer topConsumer;

	// Token: 0x040038B2 RID: 14514
	private Operational operational;

	// Token: 0x040038B3 RID: 14515
	public HashSet<int> players = new HashSet<int>();

	// Token: 0x040038B4 RID: 14516
	public HashedString[] overrideAnims = new HashedString[]
	{
		"anim_interacts_windtunnel_center_kanim",
		"anim_interacts_windtunnel_left_kanim",
		"anim_interacts_windtunnel_right_kanim"
	};

	// Token: 0x040038B5 RID: 14517
	public string[][] workPreAnims = new string[][]
	{
		new string[]
		{
			"weak_working_front_pre",
			"weak_working_back_pre"
		},
		new string[]
		{
			"medium_working_front_pre",
			"medium_working_back_pre"
		},
		new string[]
		{
			"strong_working_front_pre",
			"strong_working_back_pre"
		}
	};

	// Token: 0x040038B6 RID: 14518
	public string[] workAnims = new string[]
	{
		"weak_working_loop",
		"medium_working_loop",
		"strong_working_loop"
	};

	// Token: 0x040038B7 RID: 14519
	public string[][] workPstAnims = new string[][]
	{
		new string[]
		{
			"weak_working_back_pst",
			"weak_working_front_pst"
		},
		new string[]
		{
			"medium_working_back_pst",
			"medium_working_front_pst"
		},
		new string[]
		{
			"strong_working_back_pst",
			"strong_working_front_pst"
		}
	};

	// Token: 0x02001BA4 RID: 7076
	public class States : GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel>
	{
		// Token: 0x0600A3FD RID: 41981 RVA: 0x0038B0E8 File Offset: 0x003892E8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.PlayAnim("off").ParamTransition<int>(this.playerCount, this.operational.pre, (VerticalWindTunnel.StatesInstance smi, int p) => p > 0);
			this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter(delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.SetActive(false);
			}).ParamTransition<int>(this.playerCount, this.operational.post, (VerticalWindTunnel.StatesInstance smi, int p) => p == 0).Enter("GasWalls", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.SetGasWalls(true);
			}).Exit("GasWalls", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.SetGasWalls(false);
			});
			this.operational.post.PlayAnim("working_pst").QueueAnim("off_pre", false, null).OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x04008049 RID: 32841
		public StateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.IntParameter playerCount;

		// Token: 0x0400804A RID: 32842
		public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State unoperational;

		// Token: 0x0400804B RID: 32843
		public VerticalWindTunnel.States.OperationalStates operational;

		// Token: 0x02002629 RID: 9769
		public class OperationalStates : GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State
		{
			// Token: 0x0400A9CB RID: 43467
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State stopped;

			// Token: 0x0400A9CC RID: 43468
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State pre;

			// Token: 0x0400A9CD RID: 43469
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State playing;

			// Token: 0x0400A9CE RID: 43470
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State post;
		}
	}

	// Token: 0x02001BA5 RID: 7077
	public class StatesInstance : GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.GameInstance
	{
		// Token: 0x0600A3FF RID: 41983 RVA: 0x0038B34E File Offset: 0x0038954E
		public StatesInstance(VerticalWindTunnel smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x0600A400 RID: 41984 RVA: 0x0038B368 File Offset: 0x00389568
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x0400804C RID: 32844
		private Operational operational;
	}
}
