using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class LogicBroadcastReceiver : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x06002DEB RID: 11755 RVA: 0x00101FA0 File Offset: 0x001001A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.SetChannel(this.channel.Get());
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, this.IsSpaceVisible());
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject));
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x00102044 File Offset: 0x00100244
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
		bool flag2 = this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, flag2);
		if (flag2 && !this.syncToChannelComplete)
		{
			this.SyncWithBroadcast();
		}
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x0010211A File Offset: 0x0010031A
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x06002DEE RID: 11758 RVA: 0x00102148 File Offset: 0x00100348
	private bool CheckChannelValid()
	{
		return this.channel.Get() != null && this.channel.Get().GetComponent<LogicPorts>().inputPorts != null;
	}

	// Token: 0x06002DEF RID: 11759 RVA: 0x00102177 File Offset: 0x00100377
	public LogicBroadcaster GetChannel()
	{
		return this.channel.Get();
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x00102184 File Offset: 0x00100384
	public void SetChannel(LogicBroadcaster broadcaster)
	{
		this.ClearChannel();
		if (broadcaster == null)
		{
			return;
		}
		this.channel.Set(broadcaster);
		this.syncToChannelComplete = false;
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-801688580, new Action<object>(this.OnChannelLogicEvent)));
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-592767678, new Action<object>(this.OnChannelLogicEvent)));
		this.SyncWithBroadcast();
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x0010221C File Offset: 0x0010041C
	private void ClearChannel()
	{
		if (this.CheckChannelValid())
		{
			for (int i = 0; i < this.channelEventListeners.Count; i++)
			{
				this.channel.Get().gameObject.Unsubscribe(this.channelEventListeners[i]);
			}
		}
		this.channelEventListeners.Clear();
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x00102273 File Offset: 0x00100473
	private void OnChannelLogicEvent(object data)
	{
		if (!this.channel.Get().GetComponent<Operational>().IsOperational)
		{
			return;
		}
		this.SyncWithBroadcast();
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x00102294 File Offset: 0x00100494
	private void SyncWithBroadcast()
	{
		if (!this.CheckChannelValid())
		{
			return;
		}
		bool flag = LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.UpdateRangeStatus(flag);
		if (!flag)
		{
			return;
		}
		base.GetComponent<LogicPorts>().SendSignal(this.PORT_ID, this.channel.Get().GetCurrentValue());
		this.syncToChannelComplete = true;
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x001022FE File Offset: 0x001004FE
	public static bool CheckRange(GameObject broadcaster, GameObject receiver)
	{
		return AxialUtil.GetDistance(broadcaster.GetMyWorldLocation(), receiver.GetMyWorldLocation()) <= LogicBroadcaster.RANGE;
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x0010231C File Offset: 0x0010051C
	private void UpdateRangeStatus(bool inRange)
	{
		if (!inRange && this.rangeStatusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.rangeStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.BroadcasterOutOfRange, null);
			return;
		}
		if (this.rangeStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.rangeStatusItem, false);
			this.rangeStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x00102394 File Offset: 0x00100594
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			if (!this.wasOperational)
			{
				this.wasOperational = true;
				this.animController.Queue("on_pre", KAnim.PlayMode.Once, 1f, 0f);
				this.animController.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else if (this.wasOperational)
		{
			this.wasOperational = false;
			this.animController.Queue("on_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.animController.Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06002DF7 RID: 11767 RVA: 0x00102450 File Offset: 0x00100650
	protected override void OnCleanUp()
	{
		this.ClearChannel();
		base.OnCleanUp();
	}

	// Token: 0x04001AC8 RID: 6856
	[Serialize]
	private Ref<LogicBroadcaster> channel = new Ref<LogicBroadcaster>();

	// Token: 0x04001AC9 RID: 6857
	public string PORT_ID = "";

	// Token: 0x04001ACA RID: 6858
	private List<int> channelEventListeners = new List<int>();

	// Token: 0x04001ACB RID: 6859
	private bool syncToChannelComplete;

	// Token: 0x04001ACC RID: 6860
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x04001ACD RID: 6861
	public static readonly Operational.Flag validChannelInRange = new Operational.Flag("validChannelInRange", Operational.Flag.Type.Requirement);

	// Token: 0x04001ACE RID: 6862
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001ACF RID: 6863
	private bool wasOperational;

	// Token: 0x04001AD0 RID: 6864
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x04001AD1 RID: 6865
	private Guid rangeStatusItem = Guid.Empty;

	// Token: 0x04001AD2 RID: 6866
	private Guid spaceNotVisibleStatusItem = Guid.Empty;
}
