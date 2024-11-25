using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000717 RID: 1815
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonReader")]
public class LogicRibbonReader : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06002F9D RID: 12189 RVA: 0x00108EC0 File Offset: 0x001070C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonReader>(-801688580, LogicRibbonReader.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002F9E RID: 12190 RVA: 0x00108F1C File Offset: 0x0010711C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonReader>(-905833192, LogicRibbonReader.OnCopySettingsDelegate);
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x00108F38 File Offset: 0x00107138
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonReader.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x00108F78 File Offset: 0x00107178
	private void OnCopySettings(object data)
	{
		LogicRibbonReader component = ((GameObject)data).GetComponent<LogicRibbonReader>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x00108FA8 File Offset: 0x001071A8
	private void UpdateLogicCircuit()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
		int portCell = component.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component2 = gameObject.GetComponent<LogicWire>();
			if (component2 != null)
			{
				bitDepth = component2.MaxBitDepth;
			}
		}
		if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
		{
			int num = this.currentValue >> this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, num);
		}
		else
		{
			int num = this.currentValue & 1 << this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, (num > 0) ? 1 : 0);
		}
		this.UpdateVisuals();
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x00109051 File Offset: 0x00107251
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x00109059 File Offset: 0x00107259
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x00109068 File Offset: 0x00107268
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x00109070 File Offset: 0x00107270
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x00109078 File Offset: 0x00107278
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_TITLE";
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x0010907F File Offset: 0x0010727F
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_DESCRIPTION;
		}
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x0010908B File Offset: 0x0010728B
	public bool SideScreenDisplayWriterDescription()
	{
		return false;
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x0010908E File Offset: 0x0010728E
	public bool SideScreenDisplayReaderDescription()
	{
		return true;
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x00109094 File Offset: 0x00107294
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x001090E0 File Offset: 0x001072E0
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonReader.INPUT_PORT_ID);
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x0010910C File Offset: 0x0010730C
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonReader.OUTPUT_PORT_ID);
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x00109138 File Offset: 0x00107338
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06002FAE RID: 12206 RVA: 0x00109178 File Offset: 0x00107378
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x001091B8 File Offset: 0x001073B8
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		this.GetInputValue();
		int num = 0;
		if (inputNetwork)
		{
			num += 4;
			this.kbac.SetSymbolTint(this.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num++;
			this.kbac.SetSymbolTint(this.OUTPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetOutputValue()) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001BFA RID: 7162
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonReaderInput");

	// Token: 0x04001BFB RID: 7163
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonReaderOutput");

	// Token: 0x04001BFC RID: 7164
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BFD RID: 7165
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001BFE RID: 7166
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001BFF RID: 7167
	private KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04001C00 RID: 7168
	private KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04001C01 RID: 7169
	private KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04001C02 RID: 7170
	private KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04001C03 RID: 7171
	private KAnimHashedString OUTPUT_SYMBOL = "output_light_bloom";

	// Token: 0x04001C04 RID: 7172
	private KBatchedAnimController kbac;

	// Token: 0x04001C05 RID: 7173
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04001C06 RID: 7174
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04001C07 RID: 7175
	private LogicPorts ports;

	// Token: 0x04001C08 RID: 7176
	public int bitDepth = 4;

	// Token: 0x04001C09 RID: 7177
	[Serialize]
	public int selectedBit;

	// Token: 0x04001C0A RID: 7178
	[Serialize]
	private int currentValue;
}
