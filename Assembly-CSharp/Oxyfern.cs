using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009F4 RID: 2548
public class Oxyfern : StateMachineComponent<Oxyfern.StatesInstance>
{
	// Token: 0x060049C4 RID: 18884 RVA: 0x001A6117 File Offset: 0x001A4317
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060049C5 RID: 18885 RVA: 0x001A612F File Offset: 0x001A432F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049C6 RID: 18886 RVA: 0x001A6142 File Offset: 0x001A4342
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (Tutorial.Instance.oxygenGenerators.Contains(base.gameObject))
		{
			Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		}
	}

	// Token: 0x060049C7 RID: 18887 RVA: 0x001A6177 File Offset: 0x001A4377
	protected override void OnPrefabInit()
	{
		base.Subscribe<Oxyfern>(1309017699, Oxyfern.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x060049C8 RID: 18888 RVA: 0x001A6190 File Offset: 0x001A4390
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
		if (this.receptacleMonitor.Replanted)
		{
			Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
		}
	}

	// Token: 0x060049C9 RID: 18889 RVA: 0x001A61BA File Offset: 0x001A43BA
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.00062500004f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.00015625001f;
	}

	// Token: 0x04003066 RID: 12390
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003067 RID: 12391
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04003068 RID: 12392
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x04003069 RID: 12393
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x0400306A RID: 12394
	private static readonly EventSystem.IntraObjectHandler<Oxyfern> OnReplantedDelegate = new EventSystem.IntraObjectHandler<Oxyfern>(delegate(Oxyfern component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x02001A01 RID: 6657
	public class StatesInstance : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.GameInstance
	{
		// Token: 0x06009EAE RID: 40622 RVA: 0x0037999B File Offset: 0x00377B9B
		public StatesInstance(Oxyfern master) : base(master)
		{
		}
	}

	// Token: 0x02001A02 RID: 6658
	public class States : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern>
	{
		// Token: 0x06009EAF RID: 40623 RVA: 0x003799A4 File Offset: 0x00377BA4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(Oxyfern.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_pst", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (Oxyfern.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			});
			this.alive.wilting.PlayAnim("wilt3").EventTransition(GameHashes.WiltRecover, this.alive.mature, (Oxyfern.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04007B04 RID: 31492
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State grow;

		// Token: 0x04007B05 RID: 31493
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State blocked_from_growing;

		// Token: 0x04007B06 RID: 31494
		public Oxyfern.States.AliveStates alive;

		// Token: 0x04007B07 RID: 31495
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State dead;

		// Token: 0x020025E3 RID: 9699
		public class AliveStates : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.PlantAliveSubState
		{
			// Token: 0x0400A8B1 RID: 43185
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State mature;

			// Token: 0x0400A8B2 RID: 43186
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State wilting;
		}
	}
}
