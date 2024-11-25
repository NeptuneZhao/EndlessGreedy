using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F3 RID: 2547
public class OilEater : StateMachineComponent<OilEater.StatesInstance>
{
	// Token: 0x060049C1 RID: 18881 RVA: 0x001A6049 File Offset: 0x001A4249
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049C2 RID: 18882 RVA: 0x001A605C File Offset: 0x001A425C
	public void Exhaust(float dt)
	{
		if (base.smi.master.wiltCondition.IsWilting())
		{
			return;
		}
		this.emittedMass += dt * this.emitRate;
		if (this.emittedMass >= this.minEmitMass)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffset);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementEmitted, this.emittedMass, component.Temperature, byte.MaxValue, 0, true, -1);
			this.emittedMass = 0f;
		}
	}

	// Token: 0x0400305D RID: 12381
	private const SimHashes srcElement = SimHashes.CrudeOil;

	// Token: 0x0400305E RID: 12382
	private const SimHashes emitElement = SimHashes.CarbonDioxide;

	// Token: 0x0400305F RID: 12383
	public float emitRate = 1f;

	// Token: 0x04003060 RID: 12384
	public float minEmitMass;

	// Token: 0x04003061 RID: 12385
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04003062 RID: 12386
	[Serialize]
	private float emittedMass;

	// Token: 0x04003063 RID: 12387
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003064 RID: 12388
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003065 RID: 12389
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x020019FF RID: 6655
	public class StatesInstance : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.GameInstance
	{
		// Token: 0x06009EA7 RID: 40615 RVA: 0x003796E9 File Offset: 0x003778E9
		public StatesInstance(OilEater master) : base(master)
		{
		}
	}

	// Token: 0x02001A00 RID: 6656
	public class States : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater>
	{
		// Token: 0x06009EA8 RID: 40616 RVA: 0x003796F4 File Offset: 0x003778F4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(OilEater.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, delegate(object data)
				{
					GameObject gameObject = (GameObject)data;
					CreatureHelpers.DeselectCreature(gameObject);
					Util.KDestroyGameObject(gameObject);
				}, smi.master.gameObject);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(OilEater.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update("Alive", delegate(OilEater.StatesInstance smi, float dt)
			{
				smi.master.Exhaust(dt);
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (OilEater.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop);
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (OilEater.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04007B00 RID: 31488
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State grow;

		// Token: 0x04007B01 RID: 31489
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State blocked_from_growing;

		// Token: 0x04007B02 RID: 31490
		public OilEater.States.AliveStates alive;

		// Token: 0x04007B03 RID: 31491
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State dead;

		// Token: 0x020025E1 RID: 9697
		public class AliveStates : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.PlantAliveSubState
		{
			// Token: 0x0400A8A9 RID: 43177
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State mature;

			// Token: 0x0400A8AA RID: 43178
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State wilting;
		}
	}
}
