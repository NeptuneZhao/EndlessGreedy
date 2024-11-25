using System;
using Klei.AI;

// Token: 0x020003CC RID: 972
[Serializable]
public struct SpiceInstance
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600144A RID: 5194 RVA: 0x0006F7CE File Offset: 0x0006D9CE
	public AttributeModifier CalorieModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.CalorieModifier;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600144B RID: 5195 RVA: 0x0006F7EA File Offset: 0x0006D9EA
	public AttributeModifier FoodModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.FoodModifier;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600144C RID: 5196 RVA: 0x0006F806 File Offset: 0x0006DA06
	public Effect StatBonus
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].StatBonus;
		}
	}

	// Token: 0x04000B9B RID: 2971
	public Tag Id;

	// Token: 0x04000B9C RID: 2972
	public float TotalKG;
}
