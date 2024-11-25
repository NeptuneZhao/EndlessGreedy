﻿using System;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;

// Token: 0x020009C1 RID: 2497
[SkipSaveFileSerialization]
public class QualityOfLifeNeed : Need, ISim4000ms
{
	// Token: 0x0600487F RID: 18559 RVA: 0x0019F1AC File Offset: 0x0019D3AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.expectationAttribute = attributes.Add(Db.Get().Attributes.QualityOfLifeExpectation);
		base.Name = DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME;
		base.ExpectationTooltip = string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_TOOLTIP, Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue());
		this.stressBonus = new Need.ModifierType
		{
			modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0f, DUPLICANTS.NEEDS.QUALITYOFLIFE.GOOD_MODIFIER, false, false, false)
		};
		this.stressNeutral = new Need.ModifierType
		{
			modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.008333334f, DUPLICANTS.NEEDS.QUALITYOFLIFE.NEUTRAL_MODIFIER, false, false, true)
		};
		this.stressPenalty = new Need.ModifierType
		{
			modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0f, DUPLICANTS.NEEDS.QUALITYOFLIFE.BAD_MODIFIER, false, false, false),
			statusItem = Db.Get().DuplicantStatusItems.PoorQualityOfLife
		};
		this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(base.gameObject);
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x0019F324 File Offset: 0x0019D524
	public void Sim4000ms(float dt)
	{
		if (this.skipUpdate)
		{
			return;
		}
		float num = 0.004166667f;
		float b = 0.041666668f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Morale);
		if (currentQualitySetting.id == "Disabled")
		{
			base.SetModifier(null);
			return;
		}
		if (currentQualitySetting.id == "Easy")
		{
			num = 0.0033333334f;
			b = 0.016666668f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			num = 0.008333334f;
			b = 0.05f;
		}
		else if (currentQualitySetting.id == "VeryHard")
		{
			num = 0.016666668f;
			b = 0.083333336f;
		}
		float totalValue = this.qolAttribute.GetTotalValue();
		float totalValue2 = this.expectationAttribute.GetTotalValue();
		float num2 = totalValue2 - totalValue;
		if (totalValue < totalValue2)
		{
			this.stressPenalty.modifier.SetValue(Mathf.Min(num2 * num, b));
			base.SetModifier(this.stressPenalty);
			return;
		}
		if (totalValue > totalValue2)
		{
			this.stressBonus.modifier.SetValue(Mathf.Max(-num2 * -0.016666668f, -0.033333335f));
			base.SetModifier(this.stressBonus);
			return;
		}
		base.SetModifier(this.stressNeutral);
	}

	// Token: 0x04002F7E RID: 12158
	private AttributeInstance qolAttribute;

	// Token: 0x04002F7F RID: 12159
	public bool skipUpdate;
}
