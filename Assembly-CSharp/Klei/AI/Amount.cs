using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F3B RID: 3899
	[DebuggerDisplay("{Id}")]
	public class Amount : Resource
	{
		// Token: 0x060077E6 RID: 30694 RVA: 0x002F7838 File Offset: 0x002F5A38
		public Amount(string id, string name, string description, Attribute min_attribute, Attribute max_attribute, Attribute delta_attribute, bool show_max, Units units, float visual_delta_threshold, bool show_in_ui, string uiSprite = null, string thoughtSprite = null)
		{
			this.Id = id;
			this.Name = name;
			this.description = description;
			this.minAttribute = min_attribute;
			this.maxAttribute = max_attribute;
			this.deltaAttribute = delta_attribute;
			this.showMax = show_max;
			this.units = units;
			this.visualDeltaThreshold = visual_delta_threshold;
			this.showInUI = show_in_ui;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
		}

		// Token: 0x060077E7 RID: 30695 RVA: 0x002F78A8 File Offset: 0x002F5AA8
		public void SetDisplayer(IAmountDisplayer displayer)
		{
			this.displayer = displayer;
			this.minAttribute.SetFormatter(displayer.Formatter);
			this.maxAttribute.SetFormatter(displayer.Formatter);
			this.deltaAttribute.SetFormatter(displayer.Formatter);
		}

		// Token: 0x060077E8 RID: 30696 RVA: 0x002F78E4 File Offset: 0x002F5AE4
		public AmountInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x060077E9 RID: 30697 RVA: 0x002F78F4 File Offset: 0x002F5AF4
		public AmountInstance Lookup(GameObject go)
		{
			Amounts amounts = go.GetAmounts();
			if (amounts != null)
			{
				return amounts.Get(this);
			}
			return null;
		}

		// Token: 0x060077EA RID: 30698 RVA: 0x002F7914 File Offset: 0x002F5B14
		public void Copy(GameObject to, GameObject from)
		{
			AmountInstance amountInstance = this.Lookup(to);
			AmountInstance amountInstance2 = this.Lookup(from);
			amountInstance.value = amountInstance2.value;
		}

		// Token: 0x060077EB RID: 30699 RVA: 0x002F793B File Offset: 0x002F5B3B
		public string GetValueString(AmountInstance instance)
		{
			return this.displayer.GetValueString(this, instance);
		}

		// Token: 0x060077EC RID: 30700 RVA: 0x002F794A File Offset: 0x002F5B4A
		public string GetDescription(AmountInstance instance)
		{
			return this.displayer.GetDescription(this, instance);
		}

		// Token: 0x060077ED RID: 30701 RVA: 0x002F7959 File Offset: 0x002F5B59
		public string GetTooltip(AmountInstance instance)
		{
			return this.displayer.GetTooltip(this, instance);
		}

		// Token: 0x060077EE RID: 30702 RVA: 0x002F7968 File Offset: 0x002F5B68
		public void DebugSetValue(AmountInstance instance, float value)
		{
			if (this.debugSetValue != null)
			{
				this.debugSetValue(instance, value);
				return;
			}
			instance.SetValue(value);
		}

		// Token: 0x040059AA RID: 22954
		public string description;

		// Token: 0x040059AB RID: 22955
		public bool showMax;

		// Token: 0x040059AC RID: 22956
		public Units units;

		// Token: 0x040059AD RID: 22957
		public float visualDeltaThreshold;

		// Token: 0x040059AE RID: 22958
		public Attribute minAttribute;

		// Token: 0x040059AF RID: 22959
		public Attribute maxAttribute;

		// Token: 0x040059B0 RID: 22960
		public Attribute deltaAttribute;

		// Token: 0x040059B1 RID: 22961
		public Action<AmountInstance, float> debugSetValue;

		// Token: 0x040059B2 RID: 22962
		public bool showInUI;

		// Token: 0x040059B3 RID: 22963
		public string uiSprite;

		// Token: 0x040059B4 RID: 22964
		public string thoughtSprite;

		// Token: 0x040059B5 RID: 22965
		public IAmountDisplayer displayer;
	}
}
