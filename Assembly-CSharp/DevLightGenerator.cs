using System;
using STRINGS;

// Token: 0x020006BA RID: 1722
public class DevLightGenerator : Light2D, IMultiSliderControl
{
	// Token: 0x06002B66 RID: 11110 RVA: 0x000F3D67 File Offset: 0x000F1F67
	public DevLightGenerator()
	{
		this.sliderControls = new ISliderControl[]
		{
			new DevLightGenerator.LuxController(this),
			new DevLightGenerator.RangeController(this),
			new DevLightGenerator.FalloffController(this)
		};
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000F3D96 File Offset: 0x000F1F96
	string IMultiSliderControl.SidescreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000F3D9D File Offset: 0x000F1F9D
	ISliderControl[] IMultiSliderControl.sliderControls
	{
		get
		{
			return this.sliderControls;
		}
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x000F3DA5 File Offset: 0x000F1FA5
	bool IMultiSliderControl.SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x040018E8 RID: 6376
	protected ISliderControl[] sliderControls;

	// Token: 0x020014B6 RID: 5302
	protected class LuxController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06008BD3 RID: 35795 RVA: 0x003382F8 File Offset: 0x003364F8
		public LuxController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06008BD4 RID: 35796 RVA: 0x00338307 File Offset: 0x00336507
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06008BD5 RID: 35797 RVA: 0x0033830E File Offset: 0x0033650E
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.LIGHT.LUX;
			}
		}

		// Token: 0x06008BD6 RID: 35798 RVA: 0x0033831A File Offset: 0x0033651A
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x06008BD7 RID: 35799 RVA: 0x00338321 File Offset: 0x00336521
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x06008BD8 RID: 35800 RVA: 0x00338328 File Offset: 0x00336528
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.target.Lux);
		}

		// Token: 0x06008BD9 RID: 35801 RVA: 0x00338349 File Offset: 0x00336549
		public string GetSliderTooltipKey(int index)
		{
			return "<unused>";
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x00338350 File Offset: 0x00336550
		public float GetSliderValue(int index)
		{
			return (float)this.target.Lux;
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x0033835E File Offset: 0x0033655E
		public void SetSliderValue(float value, int index)
		{
			this.target.Lux = (int)value;
			this.target.FullRefresh();
		}

		// Token: 0x06008BDC RID: 35804 RVA: 0x00338378 File Offset: 0x00336578
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04006AB6 RID: 27318
		protected Light2D target;
	}

	// Token: 0x020014B7 RID: 5303
	protected class RangeController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06008BDD RID: 35805 RVA: 0x0033837B File Offset: 0x0033657B
		public RangeController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06008BDE RID: 35806 RVA: 0x0033838A File Offset: 0x0033658A
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06008BDF RID: 35807 RVA: 0x00338391 File Offset: 0x00336591
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.TILES;
			}
		}

		// Token: 0x06008BE0 RID: 35808 RVA: 0x0033839D File Offset: 0x0033659D
		public float GetSliderMax(int index)
		{
			return 20f;
		}

		// Token: 0x06008BE1 RID: 35809 RVA: 0x003383A4 File Offset: 0x003365A4
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06008BE2 RID: 35810 RVA: 0x003383AB File Offset: 0x003365AB
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.target.Range);
		}

		// Token: 0x06008BE3 RID: 35811 RVA: 0x003383CC File Offset: 0x003365CC
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x06008BE4 RID: 35812 RVA: 0x003383D3 File Offset: 0x003365D3
		public float GetSliderValue(int index)
		{
			return this.target.Range;
		}

		// Token: 0x06008BE5 RID: 35813 RVA: 0x003383E1 File Offset: 0x003365E1
		public void SetSliderValue(float value, int index)
		{
			this.target.Range = (float)((int)value);
			this.target.FullRefresh();
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x003383FC File Offset: 0x003365FC
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04006AB7 RID: 27319
		protected Light2D target;
	}

	// Token: 0x020014B8 RID: 5304
	protected class FalloffController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06008BE7 RID: 35815 RVA: 0x003383FF File Offset: 0x003365FF
		public FalloffController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06008BE8 RID: 35816 RVA: 0x0033840E File Offset: 0x0033660E
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06008BE9 RID: 35817 RVA: 0x00338415 File Offset: 0x00336615
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.PERCENT;
			}
		}

		// Token: 0x06008BEA RID: 35818 RVA: 0x00338421 File Offset: 0x00336621
		public float GetSliderMax(int index)
		{
			return 100f;
		}

		// Token: 0x06008BEB RID: 35819 RVA: 0x00338428 File Offset: 0x00336628
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06008BEC RID: 35820 RVA: 0x0033842F File Offset: 0x0033662F
		public string GetSliderTooltip(int index)
		{
			return string.Format("{0}", this.target.FalloffRate * 100f);
		}

		// Token: 0x06008BED RID: 35821 RVA: 0x00338451 File Offset: 0x00336651
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x06008BEE RID: 35822 RVA: 0x00338458 File Offset: 0x00336658
		public float GetSliderValue(int index)
		{
			return this.target.FalloffRate * 100f;
		}

		// Token: 0x06008BEF RID: 35823 RVA: 0x0033846C File Offset: 0x0033666C
		public void SetSliderValue(float value, int index)
		{
			this.target.FalloffRate = value / 100f;
			this.target.FullRefresh();
		}

		// Token: 0x06008BF0 RID: 35824 RVA: 0x0033848B File Offset: 0x0033668B
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04006AB8 RID: 27320
		protected Light2D target;
	}
}
