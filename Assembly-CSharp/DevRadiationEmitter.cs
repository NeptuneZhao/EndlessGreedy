using System;
using STRINGS;

// Token: 0x020006BC RID: 1724
public class DevRadiationEmitter : KMonoBehaviour, ISingleSliderControl, ISliderControl
{
	// Token: 0x06002B6E RID: 11118 RVA: 0x000F3EE5 File Offset: 0x000F20E5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(true);
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06002B6F RID: 11119 RVA: 0x000F3F07 File Offset: 0x000F2107
	public string SliderTitleKey
	{
		get
		{
			return BUILDINGS.PREFABS.DEVRADIATIONGENERATOR.NAME;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06002B70 RID: 11120 RVA: 0x000F3F13 File Offset: 0x000F2113
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.RADIATION.RADS;
		}
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x000F3F1F File Offset: 0x000F211F
	public float GetSliderMax(int index)
	{
		return 5000f;
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x000F3F26 File Offset: 0x000F2126
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x000F3F2D File Offset: 0x000F212D
	public string GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x000F3F34 File Offset: 0x000F2134
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x000F3F3B File Offset: 0x000F213B
	public float GetSliderValue(int index)
	{
		return this.radiationEmitter.emitRads;
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x000F3F48 File Offset: 0x000F2148
	public void SetSliderValue(float value, int index)
	{
		this.radiationEmitter.emitRads = value;
		this.radiationEmitter.Refresh();
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x000F3F61 File Offset: 0x000F2161
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x040018EB RID: 6379
	[MyCmpReq]
	private RadiationEmitter radiationEmitter;
}
