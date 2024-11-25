using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000712 RID: 1810
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicMemory")]
public class LogicMemory : KMonoBehaviour
{
	// Token: 0x06002F51 RID: 12113 RVA: 0x00108258 File Offset: 0x00106458
	protected override void OnSpawn()
	{
		if (LogicMemory.infoStatusItem == null)
		{
			LogicMemory.infoStatusItem = new StatusItem("StoredValue", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicMemory.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicMemory.ResolveInfoStatusItemString);
		}
		base.Subscribe<LogicMemory>(-801688580, LogicMemory.OnLogicValueChangedDelegate);
	}

	// Token: 0x06002F52 RID: 12114 RVA: 0x001082BC File Offset: 0x001064BC
	public void OnLogicValueChanged(object data)
	{
		if (this.ports == null || base.gameObject == null || this == null)
		{
			return;
		}
		if (((LogicValueChanged)data).portID != LogicMemory.READ_PORT_ID)
		{
			int inputValue = this.ports.GetInputValue(LogicMemory.SET_PORT_ID);
			int inputValue2 = this.ports.GetInputValue(LogicMemory.RESET_PORT_ID);
			int num = this.value;
			if (LogicCircuitNetwork.IsBitActive(0, inputValue2))
			{
				num = 0;
			}
			else if (LogicCircuitNetwork.IsBitActive(0, inputValue))
			{
				num = 1;
			}
			if (num != this.value)
			{
				this.value = num;
				this.ports.SendSignal(LogicMemory.READ_PORT_ID, this.value);
				KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.Play(LogicCircuitNetwork.IsBitActive(0, this.value) ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
		}
	}

	// Token: 0x06002F53 RID: 12115 RVA: 0x001083B0 File Offset: 0x001065B0
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		int outputValue = ((LogicMemory)data).ports.GetOutputValue(LogicMemory.READ_PORT_ID);
		return string.Format(BUILDINGS.PREFABS.LOGICMEMORY.STATUS_ITEM_VALUE, outputValue);
	}

	// Token: 0x04001BD6 RID: 7126
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001BD7 RID: 7127
	[Serialize]
	private int value;

	// Token: 0x04001BD8 RID: 7128
	private static StatusItem infoStatusItem;

	// Token: 0x04001BD9 RID: 7129
	public static readonly HashedString READ_PORT_ID = new HashedString("LogicMemoryRead");

	// Token: 0x04001BDA RID: 7130
	public static readonly HashedString SET_PORT_ID = new HashedString("LogicMemorySet");

	// Token: 0x04001BDB RID: 7131
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicMemoryReset");

	// Token: 0x04001BDC RID: 7132
	private static readonly EventSystem.IntraObjectHandler<LogicMemory> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicMemory>(delegate(LogicMemory component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
