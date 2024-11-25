using System;
using UnityEngine;

// Token: 0x02000794 RID: 1940
public class WarpConduitReceiver : StateMachineComponent<WarpConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x0600350E RID: 13582 RVA: 0x0012118C File Offset: 0x0011F38C
	private bool IsReceiving()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x001211DE File Offset: 0x0011F3DE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.smi.StartSM();
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x001211F8 File Offset: 0x0011F3F8
	private void FindPartner()
	{
		if (this.senderGasStorage != null)
		{
			return;
		}
		WarpConduitSender warpConduitSender = null;
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitSender");
		foreach (WarpConduitSender warpConduitSender2 in UnityEngine.Object.FindObjectsOfType<WarpConduitSender>())
		{
			if (warpConduitSender2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpConduitSender = warpConduitSender2;
				break;
			}
		}
		if (warpConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.SetStorage(warpConduitSender.gasStorage, warpConduitSender.liquidStorage, warpConduitSender.solidStorage);
		WarpConduitStatus.UpdateWarpConduitsOperational(warpConduitSender.gameObject, base.gameObject);
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x00121294 File Offset: 0x0011F494
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.outputCell, this.liquidPort.networkItem, true);
		if (this.gasPort.portInfo != null)
		{
			Conduit.GetNetworkManager(this.gasPort.portInfo.conduitType).RemoveFromNetworks(this.gasPort.outputCell, this.gasPort.networkItem, true);
		}
		else
		{
			global::Debug.LogWarning("Conduit Receiver gasPort portInfo is null in OnCleanUp");
		}
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.outputCell, this.solidPort.networkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x00121343 File Offset: 0x0011F543
	public void OnActivatedChanged(object data)
	{
		if (this.senderGasStorage == null)
		{
			this.FindPartner();
		}
		WarpConduitStatus.UpdateWarpConduitsOperational((this.senderGasStorage != null) ? this.senderGasStorage.gameObject : null, base.gameObject);
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x00121380 File Offset: 0x0011F580
	public void SetStorage(Storage gasStorage, Storage liquidStorage, Storage solidStorage)
	{
		this.senderGasStorage = gasStorage;
		this.senderLiquidStorage = liquidStorage;
		this.senderSolidStorage = solidStorage;
		this.gasPort.SetPortInfo(base.gameObject, this.gasPortInfo, gasStorage, 1);
		this.liquidPort.SetPortInfo(base.gameObject, this.liquidPortInfo, liquidStorage, 2);
		this.solidPort.SetPortInfo(base.gameObject, this.solidPortInfo, solidStorage, 3);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x00121477 File Offset: 0x0011F677
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x001214A8 File Offset: 0x0011F6A8
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type == this.solidPortInfo.conduitType)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001F7C RID: 8060
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04001F7D RID: 8061
	private WarpConduitReceiver.ConduitPort liquidPort;

	// Token: 0x04001F7E RID: 8062
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04001F7F RID: 8063
	private WarpConduitReceiver.ConduitPort solidPort;

	// Token: 0x04001F80 RID: 8064
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04001F81 RID: 8065
	private WarpConduitReceiver.ConduitPort gasPort;

	// Token: 0x04001F82 RID: 8066
	public Storage senderGasStorage;

	// Token: 0x04001F83 RID: 8067
	public Storage senderLiquidStorage;

	// Token: 0x04001F84 RID: 8068
	public Storage senderSolidStorage;

	// Token: 0x0200164B RID: 5707
	public struct ConduitPort
	{
		// Token: 0x060091C1 RID: 37313 RVA: 0x003516D0 File Offset: 0x0034F8D0
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage, int number)
		{
			this.portInfo = info;
			this.outputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
				conduitDispenser.conduitType = this.portInfo.conduitType;
				conduitDispenser.useSecondaryOutput = true;
				conduitDispenser.alwaysDispense = true;
				conduitDispenser.storage = senderStorage;
				this.dispenser = conduitDispenser;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.outputCell, parent);
				networkManager.AddToNetworks(this.outputCell, this.networkItem, true);
			}
			else
			{
				SolidConduitDispenser solidConduitDispenser = parent.AddComponent<SolidConduitDispenser>();
				solidConduitDispenser.storage = senderStorage;
				solidConduitDispenser.alwaysDispense = true;
				solidConduitDispenser.useSecondaryOutput = true;
				solidConduitDispenser.solidOnly = true;
				this.solidDispenser = solidConduitDispenser;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, this.outputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.outputCell, this.networkItem, true);
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

		// Token: 0x060091C2 RID: 37314 RVA: 0x00351878 File Offset: 0x0034FA78
		public bool IsOn()
		{
			if (this.solidDispenser != null)
			{
				return this.solidDispenser.IsDispensing;
			}
			return this.dispenser != null && !this.dispenser.blocked && !this.dispenser.empty;
		}

		// Token: 0x060091C3 RID: 37315 RVA: 0x003518CC File Offset: 0x0034FACC
		public void UpdatePortAnim()
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

		// Token: 0x04006F3C RID: 28476
		public ConduitPortInfo portInfo;

		// Token: 0x04006F3D RID: 28477
		public int outputCell;

		// Token: 0x04006F3E RID: 28478
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04006F3F RID: 28479
		public ConduitDispenser dispenser;

		// Token: 0x04006F40 RID: 28480
		public SolidConduitDispenser solidDispenser;

		// Token: 0x04006F41 RID: 28481
		public MeterController airlock;

		// Token: 0x04006F42 RID: 28482
		private bool open;

		// Token: 0x04006F43 RID: 28483
		private string pre;

		// Token: 0x04006F44 RID: 28484
		private string loop;

		// Token: 0x04006F45 RID: 28485
		private string pst;
	}

	// Token: 0x0200164C RID: 5708
	public class StatesInstance : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.GameInstance
	{
		// Token: 0x060091C4 RID: 37316 RVA: 0x0035196E File Offset: 0x0034FB6E
		public StatesInstance(WarpConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x0200164D RID: 5709
	public class States : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver>
	{
		// Token: 0x060091C5 RID: 37317 RVA: 0x00351978 File Offset: 0x0034FB78
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitReceiver.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}).EventTransition(GameHashes.OperationalFlagChanged, this.on, (WarpConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.idle).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}, UpdateRate.SIM_1000ms, false);
			this.on.idle.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.IsReceiving())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.IsReceiving())
				{
					smi.GoTo(this.on.idle);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
		}

		// Token: 0x04006F46 RID: 28486
		public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State off;

		// Token: 0x04006F47 RID: 28487
		public WarpConduitReceiver.States.onStates on;

		// Token: 0x02002553 RID: 9555
		public class onStates : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State
		{
			// Token: 0x0400A65B RID: 42587
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State working;

			// Token: 0x0400A65C RID: 42588
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State idle;
		}
	}
}
