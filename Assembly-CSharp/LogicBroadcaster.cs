using System;
using KSerialization;

// Token: 0x02000701 RID: 1793
public class LogicBroadcaster : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06002DFA RID: 11770 RVA: 0x001024BF File Offset: 0x001006BF
	// (set) Token: 0x06002DFB RID: 11771 RVA: 0x001024C7 File Offset: 0x001006C7
	public int BroadCastChannelID
	{
		get
		{
			return this.broadcastChannelID;
		}
		private set
		{
			this.broadcastChannelID = value;
		}
	}

	// Token: 0x06002DFC RID: 11772 RVA: 0x001024D0 File Offset: 0x001006D0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.LogicBroadcasters.Add(this);
	}

	// Token: 0x06002DFD RID: 11773 RVA: 0x001024E3 File Offset: 0x001006E3
	protected override void OnCleanUp()
	{
		Components.LogicBroadcasters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x001024F8 File Offset: 0x001006F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicBroadcaster>(-801688580, LogicBroadcaster.OnLogicValueChangedDelegate);
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, this.IsSpaceVisible());
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x06002DFF RID: 11775 RVA: 0x00102565 File Offset: 0x00100765
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x06002E00 RID: 11776 RVA: 0x00102593 File Offset: 0x00100793
	public int GetCurrentValue()
	{
		return base.GetComponent<LogicPorts>().GetInputValue(this.PORT_ID);
	}

	// Token: 0x06002E01 RID: 11777 RVA: 0x001025AB File Offset: 0x001007AB
	private void OnLogicValueChanged(object data)
	{
	}

	// Token: 0x06002E02 RID: 11778 RVA: 0x001025B0 File Offset: 0x001007B0
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
				return;
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x0010263C File Offset: 0x0010083C
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

	// Token: 0x04001AD3 RID: 6867
	public static int RANGE = 5;

	// Token: 0x04001AD4 RID: 6868
	private static int INVALID_CHANNEL_ID = -1;

	// Token: 0x04001AD5 RID: 6869
	public string PORT_ID = "";

	// Token: 0x04001AD6 RID: 6870
	private bool wasOperational;

	// Token: 0x04001AD7 RID: 6871
	[Serialize]
	private int broadcastChannelID = LogicBroadcaster.INVALID_CHANNEL_ID;

	// Token: 0x04001AD8 RID: 6872
	private static readonly EventSystem.IntraObjectHandler<LogicBroadcaster> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicBroadcaster>(delegate(LogicBroadcaster component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001AD9 RID: 6873
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x04001ADA RID: 6874
	private Guid spaceNotVisibleStatusItem = Guid.Empty;

	// Token: 0x04001ADB RID: 6875
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001ADC RID: 6876
	[MyCmpGet]
	private KBatchedAnimController animController;
}
