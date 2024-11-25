using System;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02000F45 RID: 3909
	[DebuggerDisplay("{AttributeId}")]
	public class AttributeModifier
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06007843 RID: 30787 RVA: 0x002F8ABB File Offset: 0x002F6CBB
		// (set) Token: 0x06007844 RID: 30788 RVA: 0x002F8AC3 File Offset: 0x002F6CC3
		public string AttributeId { get; private set; }

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06007845 RID: 30789 RVA: 0x002F8ACC File Offset: 0x002F6CCC
		// (set) Token: 0x06007846 RID: 30790 RVA: 0x002F8AD4 File Offset: 0x002F6CD4
		public float Value { get; private set; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06007847 RID: 30791 RVA: 0x002F8ADD File Offset: 0x002F6CDD
		// (set) Token: 0x06007848 RID: 30792 RVA: 0x002F8AE5 File Offset: 0x002F6CE5
		public bool IsMultiplier { get; private set; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06007849 RID: 30793 RVA: 0x002F8AEE File Offset: 0x002F6CEE
		// (set) Token: 0x0600784A RID: 30794 RVA: 0x002F8AF6 File Offset: 0x002F6CF6
		public GameUtil.TimeSlice? OverrideTimeSlice { get; set; }

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x0600784B RID: 30795 RVA: 0x002F8AFF File Offset: 0x002F6CFF
		// (set) Token: 0x0600784C RID: 30796 RVA: 0x002F8B07 File Offset: 0x002F6D07
		public bool UIOnly { get; private set; }

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x0600784D RID: 30797 RVA: 0x002F8B10 File Offset: 0x002F6D10
		// (set) Token: 0x0600784E RID: 30798 RVA: 0x002F8B18 File Offset: 0x002F6D18
		public bool IsReadonly { get; private set; }

		// Token: 0x0600784F RID: 30799 RVA: 0x002F8B24 File Offset: 0x002F6D24
		public AttributeModifier(string attribute_id, float value, string description = null, bool is_multiplier = false, bool uiOnly = false, bool is_readonly = true)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.Description = ((description == null) ? attribute_id : description);
			this.DescriptionCB = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.IsReadonly = is_readonly;
			this.OverrideTimeSlice = null;
		}

		// Token: 0x06007850 RID: 30800 RVA: 0x002F8B80 File Offset: 0x002F6D80
		public AttributeModifier(string attribute_id, float value, Func<string> description_cb, bool is_multiplier = false, bool uiOnly = false)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.DescriptionCB = description_cb;
			this.Description = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.OverrideTimeSlice = null;
			if (description_cb == null)
			{
				global::Debug.LogWarning("AttributeModifier being constructed without a description callback: " + attribute_id);
			}
		}

		// Token: 0x06007851 RID: 30801 RVA: 0x002F8BE1 File Offset: 0x002F6DE1
		public void SetValue(float value)
		{
			this.Value = value;
		}

		// Token: 0x06007852 RID: 30802 RVA: 0x002F8BEC File Offset: 0x002F6DEC
		public string GetName()
		{
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
			{
				return attribute.Name;
			}
			return "";
		}

		// Token: 0x06007853 RID: 30803 RVA: 0x002F8C27 File Offset: 0x002F6E27
		public string GetDescription()
		{
			if (this.DescriptionCB == null)
			{
				return this.Description;
			}
			return this.DescriptionCB();
		}

		// Token: 0x06007854 RID: 30804 RVA: 0x002F8C44 File Offset: 0x002F6E44
		public string GetFormattedString()
		{
			IAttributeFormatter attributeFormatter = null;
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (!this.IsMultiplier)
			{
				if (attribute != null)
				{
					attributeFormatter = attribute.formatter;
				}
				else
				{
					attribute = Db.Get().BuildingAttributes.TryGet(this.AttributeId);
					if (attribute != null)
					{
						attributeFormatter = attribute.formatter;
					}
					else
					{
						attribute = Db.Get().PlantAttributes.TryGet(this.AttributeId);
						if (attribute != null)
						{
							attributeFormatter = attribute.formatter;
						}
					}
				}
			}
			string text = "";
			if (attributeFormatter != null)
			{
				text = attributeFormatter.GetFormattedModifier(this);
			}
			else if (this.IsMultiplier)
			{
				text += GameUtil.GetFormattedPercent(this.Value * 100f, GameUtil.TimeSlice.None);
			}
			else
			{
				text += GameUtil.GetFormattedSimple(this.Value, GameUtil.TimeSlice.None, null);
			}
			if (text != null && text.Length > 0 && text[0] != '-')
			{
				GameUtil.TimeSlice? overrideTimeSlice = this.OverrideTimeSlice;
				GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None;
				if (!(overrideTimeSlice.GetValueOrDefault() == timeSlice & overrideTimeSlice != null))
				{
					text = GameUtil.AddPositiveSign(text, this.Value > 0f);
				}
			}
			return text;
		}

		// Token: 0x06007855 RID: 30805 RVA: 0x002F8D54 File Offset: 0x002F6F54
		public AttributeModifier Clone()
		{
			return new AttributeModifier(this.AttributeId, this.Value, this.Description, false, false, true);
		}

		// Token: 0x040059E7 RID: 23015
		public string Description;

		// Token: 0x040059E8 RID: 23016
		public Func<string> DescriptionCB;
	}
}
