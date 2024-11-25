using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200074A RID: 1866
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PixelPack")]
public class PixelPack : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060031AA RID: 12714 RVA: 0x001112F9 File Offset: 0x0010F4F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PixelPack>(-905833192, PixelPack.OnCopySettingsDelegate);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00111314 File Offset: 0x0010F514
	private void OnCopySettings(object data)
	{
		PixelPack component = ((GameObject)data).GetComponent<PixelPack>();
		if (component != null)
		{
			for (int i = 0; i < component.colorSettings.Count; i++)
			{
				this.colorSettings[i] = component.colorSettings[i];
			}
		}
		this.UpdateColors();
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x0011136C File Offset: 0x0010F56C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.Subscribe<PixelPack>(-801688580, PixelPack.OnLogicValueChangedDelegate);
		base.Subscribe<PixelPack>(-592767678, PixelPack.OnOperationalChangedDelegate);
		if (this.colorSettings == null)
		{
			PixelPack.ColorPair item = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item2 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item3 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item4 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			this.colorSettings = new List<PixelPack.ColorPair>();
			this.colorSettings.Add(item);
			this.colorSettings.Add(item2);
			this.colorSettings.Add(item3);
			this.colorSettings.Add(item4);
		}
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x00111488 File Offset: 0x0010F688
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == PixelPack.PORT_ID)
		{
			this.logicValue = logicValueChanged.newValue;
			this.UpdateColors();
		}
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x001114C0 File Offset: 0x0010F6C0
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateColors();
			this.animController.Play(PixelPack.ON_ANIMS, KAnim.PlayMode.Once);
		}
		else
		{
			this.animController.Play(PixelPack.OFF_ANIMS, KAnim.PlayMode.Once);
		}
		this.operational.SetActive(this.operational.IsOperational, false);
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x0011151C File Offset: 0x0010F71C
	public void UpdateColors()
	{
		if (this.operational.IsOperational)
		{
			LogicPorts component = base.GetComponent<LogicPorts>();
			if (component != null)
			{
				LogicWire.BitDepth connectedWireBitDepth = component.GetConnectedWireBitDepth(PixelPack.PORT_ID);
				if (connectedWireBitDepth == LogicWire.BitDepth.FourBit)
				{
					this.animController.SetSymbolTint(PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(1, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(2, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(3, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
					return;
				}
				if (connectedWireBitDepth == LogicWire.BitDepth.OneBit)
				{
					this.animController.SetSymbolTint(PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
				}
			}
		}
	}

	// Token: 0x04001D34 RID: 7476
	protected KBatchedAnimController animController;

	// Token: 0x04001D35 RID: 7477
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001D36 RID: 7478
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001D37 RID: 7479
	public static readonly HashedString PORT_ID = new HashedString("PixelPackInput");

	// Token: 0x04001D38 RID: 7480
	public static readonly HashedString SYMBOL_ONE_NAME = "screen1";

	// Token: 0x04001D39 RID: 7481
	public static readonly HashedString SYMBOL_TWO_NAME = "screen2";

	// Token: 0x04001D3A RID: 7482
	public static readonly HashedString SYMBOL_THREE_NAME = "screen3";

	// Token: 0x04001D3B RID: 7483
	public static readonly HashedString SYMBOL_FOUR_NAME = "screen4";

	// Token: 0x04001D3C RID: 7484
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001D3D RID: 7485
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001D3E RID: 7486
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001D3F RID: 7487
	public int logicValue;

	// Token: 0x04001D40 RID: 7488
	[Serialize]
	public List<PixelPack.ColorPair> colorSettings;

	// Token: 0x04001D41 RID: 7489
	private Color defaultActive = new Color(0.34509805f, 0.84705883f, 0.32941177f);

	// Token: 0x04001D42 RID: 7490
	private Color defaultStandby = new Color(0.972549f, 0.47058824f, 0.34509805f);

	// Token: 0x04001D43 RID: 7491
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x04001D44 RID: 7492
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x020015B8 RID: 5560
	public struct ColorPair
	{
		// Token: 0x04006D9C RID: 28060
		public Color activeColor;

		// Token: 0x04006D9D RID: 28061
		public Color standbyColor;
	}
}
