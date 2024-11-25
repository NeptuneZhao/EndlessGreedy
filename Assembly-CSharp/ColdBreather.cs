using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009EE RID: 2542
[SkipSaveFileSerialization]
public class ColdBreather : StateMachineComponent<ColdBreather.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060049A3 RID: 18851 RVA: 0x001A584C File Offset: 0x001A3A4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(ColdBreather.OnSimEmittedCallback), this, "ColdBreather");
		base.smi.StartSM();
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x001A5886 File Offset: 0x001A3A86
	protected override void OnPrefabInit()
	{
		this.elementConsumer.EnableConsumption(false);
		base.Subscribe<ColdBreather>(1309017699, ColdBreather.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x001A58AC File Offset: 0x001A3AAC
	private void OnReplanted(object data = null)
	{
		ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return;
		}
		ElementConsumer component2 = base.GetComponent<ElementConsumer>();
		if (component.Replanted)
		{
			component2.consumptionRate = this.consumptionRate;
		}
		else
		{
			component2.consumptionRate = this.consumptionRate * 0.25f;
		}
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.emitRads = 480f;
			this.radiationEmitter.Refresh();
		}
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x001A5924 File Offset: 0x001A3B24
	protected override void OnCleanUp()
	{
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "coldbreather");
		this.simEmitCBHandle.Clear();
		if (this.storage)
		{
			this.storage.DropAll(true, false, default(Vector3), true, null);
		}
		base.OnCleanUp();
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x001A5982 File Offset: 0x001A3B82
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060049A8 RID: 18856 RVA: 0x001A599A File Offset: 0x001A3B9A
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.COLDBREATHER, UI.GAMEOBJECTEFFECTS.TOOLTIPS.COLDBREATHER, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x060049A9 RID: 18857 RVA: 0x001A59C2 File Offset: 0x001A3BC2
	private void SetEmitting(bool emitting)
	{
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(emitting);
		}
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x001A59E0 File Offset: 0x001A3BE0
	private void Exhale()
	{
		if (this.lastEmitTag != Tag.Invalid)
		{
			return;
		}
		this.gases.Clear();
		this.storage.Find(GameTags.Gas, this.gases);
		if (this.nextGasEmitIndex >= this.gases.Count)
		{
			this.nextGasEmitIndex = 0;
		}
		while (this.nextGasEmitIndex < this.gases.Count)
		{
			int num = this.nextGasEmitIndex;
			this.nextGasEmitIndex = num + 1;
			int index = num;
			PrimaryElement component = this.gases[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && this.simEmitCBHandle.IsValid())
			{
				float temperature = Mathf.Max(component.Element.lowTemp + 5f, component.Temperature + this.deltaEmitTemperature);
				int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffsetCell);
				ushort idx = component.Element.idx;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				SimMessages.EmitMass(gameCell, idx, component.Mass, temperature, component.DiseaseIdx, component.DiseaseCount, this.simEmitCBHandle.index);
				this.lastEmitTag = component.Element.tag;
				return;
			}
		}
	}

	// Token: 0x060049AB RID: 18859 RVA: 0x001A5B43 File Offset: 0x001A3D43
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((ColdBreather)data).OnSimEmitted(info);
	}

	// Token: 0x060049AC RID: 18860 RVA: 0x001A5B54 File Offset: 0x001A3D54
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded == 1 && this.storage && this.lastEmitTag.IsValid)
		{
			this.storage.ConsumeIgnoringDisease(this.lastEmitTag, info.mass);
		}
		this.lastEmitTag = Tag.Invalid;
	}

	// Token: 0x04003030 RID: 12336
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003031 RID: 12337
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04003032 RID: 12338
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003033 RID: 12339
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04003034 RID: 12340
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04003035 RID: 12341
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04003036 RID: 12342
	private const float EXHALE_PERIOD = 1f;

	// Token: 0x04003037 RID: 12343
	public float consumptionRate;

	// Token: 0x04003038 RID: 12344
	public float deltaEmitTemperature = -5f;

	// Token: 0x04003039 RID: 12345
	public Vector3 emitOffsetCell = new Vector3(0f, 0f);

	// Token: 0x0400303A RID: 12346
	private List<GameObject> gases = new List<GameObject>();

	// Token: 0x0400303B RID: 12347
	private Tag lastEmitTag;

	// Token: 0x0400303C RID: 12348
	private int nextGasEmitIndex;

	// Token: 0x0400303D RID: 12349
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x0400303E RID: 12350
	private static readonly EventSystem.IntraObjectHandler<ColdBreather> OnReplantedDelegate = new EventSystem.IntraObjectHandler<ColdBreather>(delegate(ColdBreather component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x020019F4 RID: 6644
	public class StatesInstance : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.GameInstance
	{
		// Token: 0x06009E83 RID: 40579 RVA: 0x003784C5 File Offset: 0x003766C5
		public StatesInstance(ColdBreather master) : base(master)
		{
		}
	}

	// Token: 0x020019F5 RID: 6645
	public class States : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather>
	{
		// Token: 0x06009E84 RID: 40580 RVA: 0x003784D0 File Offset: 0x003766D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			this.statusItemCooling = new StatusItem("cooling", CREATURES.STATUSITEMS.COOLING.NAME, CREATURES.STATUSITEMS.COOLING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(ColdBreather.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update(delegate(ColdBreather.StatesInstance smi, float dt)
			{
				smi.master.Exhale();
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (ColdBreather.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).ToggleMainStatusItem(this.statusItemCooling, null).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
				smi.master.SetEmitting(true);
			}).Exit(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
				smi.master.SetEmitting(false);
			});
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (ColdBreather.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.SetEmitting(false);
			});
		}

		// Token: 0x04007AE9 RID: 31465
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State grow;

		// Token: 0x04007AEA RID: 31466
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State blocked_from_growing;

		// Token: 0x04007AEB RID: 31467
		public ColdBreather.States.AliveStates alive;

		// Token: 0x04007AEC RID: 31468
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State dead;

		// Token: 0x04007AED RID: 31469
		private StatusItem statusItemCooling;

		// Token: 0x020025D4 RID: 9684
		public class AliveStates : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.PlantAliveSubState
		{
			// Token: 0x0400A867 RID: 43111
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State mature;

			// Token: 0x0400A868 RID: 43112
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State wilting;
		}
	}
}
