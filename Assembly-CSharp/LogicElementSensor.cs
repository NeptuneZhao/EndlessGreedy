using System;
using KSerialization;

// Token: 0x02000707 RID: 1799
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicElementSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x06002E72 RID: 11890 RVA: 0x00103D7E File Offset: 0x00101F7E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Filterable>().onFilterChanged += this.OnElementSelected;
	}

	// Token: 0x06002E73 RID: 11891 RVA: 0x00103DA0 File Offset: 0x00101FA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		base.Subscribe<LogicElementSensor>(-592767678, LogicElementSensor.OnOperationalChangedDelegate);
	}

	// Token: 0x06002E74 RID: 11892 RVA: 0x00103DF0 File Offset: 0x00101FF0
	public void Sim200ms(float dt)
	{
		int i = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			this.samples[this.sampleIdx] = (Grid.ElementIdx[i] == this.desiredElementIdx);
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		bool flag = true;
		bool[] array = this.samples;
		for (int j = 0; j < array.Length; j++)
		{
			flag = (array[j] && flag);
		}
		if (base.IsSwitchedOn != flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x06002E75 RID: 11893 RVA: 0x00103E6F File Offset: 0x0010206F
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x00103E80 File Offset: 0x00102080
	private void UpdateLogicCircuit()
	{
		bool flag = this.switchedOn && base.GetComponent<Operational>().IsOperational;
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x00103EBC File Offset: 0x001020BC
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x00103F44 File Offset: 0x00102144
	private void OnElementSelected(Tag element_tag)
	{
		if (!element_tag.IsValid)
		{
			return;
		}
		Element element = ElementLoader.GetElement(element_tag);
		bool on = true;
		if (element != null)
		{
			this.desiredElementIdx = ElementLoader.GetElementIndex(element.id);
			on = (element.id == SimHashes.Void || element.id == SimHashes.Vacuum);
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x00103FB3 File Offset: 0x001021B3
	private void OnOperationalChanged(object data)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x00103FC4 File Offset: 0x001021C4
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001B16 RID: 6934
	private bool wasOn;

	// Token: 0x04001B17 RID: 6935
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04001B18 RID: 6936
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001B19 RID: 6937
	private bool[] samples = new bool[8];

	// Token: 0x04001B1A RID: 6938
	private int sampleIdx;

	// Token: 0x04001B1B RID: 6939
	private ushort desiredElementIdx = ushort.MaxValue;

	// Token: 0x04001B1C RID: 6940
	private static readonly EventSystem.IntraObjectHandler<LogicElementSensor> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicElementSensor>(delegate(LogicElementSensor component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
