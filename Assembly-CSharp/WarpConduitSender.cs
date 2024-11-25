using System;
using UnityEngine;

// Token: 0x02000796 RID: 1942
public class WarpConduitSender : StateMachineComponent<WarpConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x06003519 RID: 13593 RVA: 0x00121664 File Offset: 0x0011F864
	private bool IsSending()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x001216B8 File Offset: 0x0011F8B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = base.GetComponents<Storage>();
		this.gasStorage = components[0];
		this.liquidStorage = components[1];
		this.solidStorage = components[2];
		this.gasPort = new WarpConduitSender.ConduitPort(base.gameObject, this.gasPortInfo, 1, this.gasStorage);
		this.liquidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.liquidPortInfo, 2, this.liquidStorage);
		this.solidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.solidPortInfo, 3, this.solidStorage);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
		this.FindPartner();
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
		base.smi.StartSM();
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x00121809 File Offset: 0x0011FA09
	public void OnActivatedChanged(object data)
	{
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x00121834 File Offset: 0x0011FA34
	private void FindPartner()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitReceiver");
		foreach (WarpConduitReceiver component in UnityEngine.Object.FindObjectsOfType<WarpConduitReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				this.receiver = component;
				break;
			}
		}
		if (this.receiver == null)
		{
			global::Debug.LogWarning("No warp conduit receiver found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.receiver.SetStorage(this.gasStorage, this.liquidStorage, this.solidStorage);
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x001218BC File Offset: 0x0011FABC
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.inputCell, this.liquidPort.networkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasPort.inputCell, this.gasPort.networkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.inputCell, this.solidPort.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x0012194D File Offset: 0x0011FB4D
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0012197C File Offset: 0x0011FB7C
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001F86 RID: 8070
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001F87 RID: 8071
	public Storage gasStorage;

	// Token: 0x04001F88 RID: 8072
	public Storage liquidStorage;

	// Token: 0x04001F89 RID: 8073
	public Storage solidStorage;

	// Token: 0x04001F8A RID: 8074
	public WarpConduitReceiver receiver;

	// Token: 0x04001F8B RID: 8075
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04001F8C RID: 8076
	private WarpConduitSender.ConduitPort liquidPort;

	// Token: 0x04001F8D RID: 8077
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04001F8E RID: 8078
	private WarpConduitSender.ConduitPort gasPort;

	// Token: 0x04001F8F RID: 8079
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04001F90 RID: 8080
	private WarpConduitSender.ConduitPort solidPort;

	// Token: 0x0200164E RID: 5710
	private class ConduitPort
	{
		// Token: 0x060091C9 RID: 37321 RVA: 0x00351B5C File Offset: 0x0034FD5C
		public ConduitPort(GameObject parent, ConduitPortInfo info, int number, Storage targetStorage)
		{
			this.portInfo = info;
			this.inputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
				conduitConsumer.conduitType = this.portInfo.conduitType;
				conduitConsumer.useSecondaryInput = true;
				conduitConsumer.storage = targetStorage;
				conduitConsumer.capacityKG = targetStorage.capacityKg;
				conduitConsumer.alwaysConsume = false;
				this.conduitConsumer = conduitConsumer;
				this.conduitConsumer.keepZeroMassObject = false;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell, parent);
				networkManager.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			else
			{
				this.solidConsumer = parent.AddComponent<SolidConduitConsumer>();
				this.solidConsumer.useSecondaryInput = true;
				this.solidConsumer.storage = targetStorage;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, this.inputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			string meter_animation = "airlock_" + number.ToString();
			string text = "airlock_target_" + number.ToString();
			this.pre = "airlock_" + number.ToString() + "_pre";
			this.loop = "airlock_" + number.ToString() + "_loop";
			this.pst = "airlock_" + number.ToString() + "_pst";
			this.airlock = new MeterController(parent.GetComponent<KBatchedAnimController>(), text, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				text
			});
		}

		// Token: 0x060091CA RID: 37322 RVA: 0x00351D20 File Offset: 0x0034FF20
		public bool IsOn()
		{
			if (this.solidConsumer != null)
			{
				return this.solidConsumer.IsConsuming;
			}
			return this.conduitConsumer != null && (this.conduitConsumer.IsConnected && this.conduitConsumer.IsSatisfied) && this.conduitConsumer.consumedLastTick;
		}

		// Token: 0x060091CB RID: 37323 RVA: 0x00351D80 File Offset: 0x0034FF80
		public void Update()
		{
			bool flag = this.IsOn();
			if (flag != this.open)
			{
				this.open = flag;
				if (this.open)
				{
					this.airlock.meterController.Play(this.pre, KAnim.PlayMode.Once, 1f, 0f);
					this.airlock.meterController.Queue(this.loop, KAnim.PlayMode.Loop, 1f, 0f);
					return;
				}
				this.airlock.meterController.Play(this.pst, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x04006F48 RID: 28488
		public ConduitPortInfo portInfo;

		// Token: 0x04006F49 RID: 28489
		public int inputCell;

		// Token: 0x04006F4A RID: 28490
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04006F4B RID: 28491
		private ConduitConsumer conduitConsumer;

		// Token: 0x04006F4C RID: 28492
		public SolidConduitConsumer solidConsumer;

		// Token: 0x04006F4D RID: 28493
		public MeterController airlock;

		// Token: 0x04006F4E RID: 28494
		private bool open;

		// Token: 0x04006F4F RID: 28495
		private string pre;

		// Token: 0x04006F50 RID: 28496
		private string loop;

		// Token: 0x04006F51 RID: 28497
		private string pst;
	}

	// Token: 0x0200164F RID: 5711
	public class StatesInstance : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameInstance
	{
		// Token: 0x060091CC RID: 37324 RVA: 0x00351E22 File Offset: 0x00350022
		public StatesInstance(WarpConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x02001650 RID: 5712
	public class States : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender>
	{
		// Token: 0x060091CD RID: 37325 RVA: 0x00351E2C File Offset: 0x0035002C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitSender.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}).EventTransition(GameHashes.OperationalChanged, this.on, (WarpConduitSender.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.waiting).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (!smi.master.IsSending())
				{
					smi.GoTo(this.on.waiting);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
			this.on.waiting.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (smi.master.IsSending())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x04006F52 RID: 28498
		public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State off;

		// Token: 0x04006F53 RID: 28499
		public WarpConduitSender.States.onStates on;

		// Token: 0x02002555 RID: 9557
		public class onStates : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State
		{
			// Token: 0x0400A663 RID: 42595
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State working;

			// Token: 0x0400A664 RID: 42596
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State waiting;
		}
	}
}
