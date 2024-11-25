using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000718 RID: 1816
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonWriter")]
public class LogicRibbonWriter : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06002FB2 RID: 12210 RVA: 0x001093F3 File Offset: 0x001075F3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonWriter>(-905833192, LogicRibbonWriter.OnCopySettingsDelegate);
	}

	// Token: 0x06002FB3 RID: 12211 RVA: 0x0010940C File Offset: 0x0010760C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonWriter>(-801688580, LogicRibbonWriter.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x00109468 File Offset: 0x00107668
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonWriter.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x001094A8 File Offset: 0x001076A8
	private void OnCopySettings(object data)
	{
		LogicRibbonWriter component = ((GameObject)data).GetComponent<LogicRibbonWriter>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x001094D8 File Offset: 0x001076D8
	private void UpdateLogicCircuit()
	{
		int new_value = this.currentValue << this.selectedBit;
		base.GetComponent<LogicPorts>().SendSignal(LogicRibbonWriter.OUTPUT_PORT_ID, new_value);
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x00109507 File Offset: 0x00107707
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x00109510 File Offset: 0x00107710
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x00109550 File Offset: 0x00107750
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x00109590 File Offset: 0x00107790
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002FBB RID: 12219 RVA: 0x0010959F File Offset: 0x0010779F
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x001095A7 File Offset: 0x001077A7
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06002FBD RID: 12221 RVA: 0x001095AF File Offset: 0x001077AF
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_TITLE";
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06002FBE RID: 12222 RVA: 0x001095B6 File Offset: 0x001077B6
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_DESCRIPTION;
		}
	}

	// Token: 0x06002FBF RID: 12223 RVA: 0x001095C2 File Offset: 0x001077C2
	public bool SideScreenDisplayWriterDescription()
	{
		return true;
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x001095C5 File Offset: 0x001077C5
	public bool SideScreenDisplayReaderDescription()
	{
		return false;
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x001095C8 File Offset: 0x001077C8
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x00109614 File Offset: 0x00107814
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonWriter.INPUT_PORT_ID);
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x00109640 File Offset: 0x00107840
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonWriter.OUTPUT_PORT_ID);
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x0010966C File Offset: 0x0010786C
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		int num = 0;
		if (inputNetwork)
		{
			num++;
			this.kbac.SetSymbolTint(LogicRibbonWriter.INPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetInputValue()) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num += 4;
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001C0B RID: 7179
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonWriterInput");

	// Token: 0x04001C0C RID: 7180
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonWriterOutput");

	// Token: 0x04001C0D RID: 7181
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C0E RID: 7182
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001C0F RID: 7183
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001C10 RID: 7184
	private LogicPorts ports;

	// Token: 0x04001C11 RID: 7185
	public int bitDepth = 4;

	// Token: 0x04001C12 RID: 7186
	[Serialize]
	public int selectedBit;

	// Token: 0x04001C13 RID: 7187
	[Serialize]
	private int currentValue;

	// Token: 0x04001C14 RID: 7188
	private KBatchedAnimController kbac;

	// Token: 0x04001C15 RID: 7189
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04001C16 RID: 7190
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04001C17 RID: 7191
	private static KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04001C18 RID: 7192
	private static KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04001C19 RID: 7193
	private static KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04001C1A RID: 7194
	private static KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04001C1B RID: 7195
	private static KAnimHashedString INPUT_SYMBOL = "input_light_bloom";
}
