using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F3E RID: 3902
	public class Attribute : Resource
	{
		// Token: 0x06007807 RID: 30727 RVA: 0x002F7C70 File Offset: 0x002F5E70
		public Attribute(string id, bool is_trainable, Attribute.Display show_in_ui, bool is_profession, float base_value = 0f, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null, string[] overrideDLCIDs = null) : base(id, null, null)
		{
			string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
			this.Name = Strings.Get(new StringKey(str + ".NAME"));
			this.ProfessionName = Strings.Get(new StringKey(str + ".NAME"));
			this.Description = Strings.Get(new StringKey(str + ".DESC"));
			this.IsTrainable = is_trainable;
			this.IsProfession = is_profession;
			this.ShowInUI = show_in_ui;
			this.BaseValue = base_value;
			this.formatter = Attribute.defaultFormatter;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (overrideDLCIDs != null)
			{
				this.DLCIds = overrideDLCIDs;
			}
		}

		// Token: 0x06007808 RID: 30728 RVA: 0x002F7D5C File Offset: 0x002F5F5C
		public Attribute(string id, string name, string profession_name, string attribute_description, float base_value, Attribute.Display show_in_ui, bool is_trainable, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null) : base(id, name)
		{
			this.Description = attribute_description;
			this.ProfessionName = profession_name;
			this.BaseValue = base_value;
			this.ShowInUI = show_in_ui;
			this.IsTrainable = is_trainable;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (this.ProfessionName == "")
			{
				this.ProfessionName = null;
			}
		}

		// Token: 0x06007809 RID: 30729 RVA: 0x002F7DDF File Offset: 0x002F5FDF
		public void SetFormatter(IAttributeFormatter formatter)
		{
			this.formatter = formatter;
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x002F7DE8 File Offset: 0x002F5FE8
		public AttributeInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600780B RID: 30731 RVA: 0x002F7DF8 File Offset: 0x002F5FF8
		public AttributeInstance Lookup(GameObject go)
		{
			Attributes attributes = go.GetAttributes();
			if (attributes != null)
			{
				return attributes.Get(this);
			}
			return null;
		}

		// Token: 0x0600780C RID: 30732 RVA: 0x002F7E18 File Offset: 0x002F6018
		public string GetDescription(AttributeInstance instance)
		{
			return instance.GetDescription();
		}

		// Token: 0x0600780D RID: 30733 RVA: 0x002F7E20 File Offset: 0x002F6020
		public string GetTooltip(AttributeInstance instance)
		{
			return this.formatter.GetTooltip(this, instance);
		}

		// Token: 0x040059C1 RID: 22977
		private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);

		// Token: 0x040059C2 RID: 22978
		public string Description;

		// Token: 0x040059C3 RID: 22979
		public float BaseValue;

		// Token: 0x040059C4 RID: 22980
		public Attribute.Display ShowInUI;

		// Token: 0x040059C5 RID: 22981
		public bool IsTrainable;

		// Token: 0x040059C6 RID: 22982
		public bool IsProfession;

		// Token: 0x040059C7 RID: 22983
		public string ProfessionName;

		// Token: 0x040059C8 RID: 22984
		public List<AttributeConverter> converters = new List<AttributeConverter>();

		// Token: 0x040059C9 RID: 22985
		public string uiSprite;

		// Token: 0x040059CA RID: 22986
		public string thoughtSprite;

		// Token: 0x040059CB RID: 22987
		public string uiFullColourSprite;

		// Token: 0x040059CC RID: 22988
		public string[] DLCIds = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x040059CD RID: 22989
		public IAttributeFormatter formatter;

		// Token: 0x02002335 RID: 9013
		public enum Display
		{
			// Token: 0x04009E08 RID: 40456
			Normal,
			// Token: 0x04009E09 RID: 40457
			Skill,
			// Token: 0x04009E0A RID: 40458
			Expectation,
			// Token: 0x04009E0B RID: 40459
			General,
			// Token: 0x04009E0C RID: 40460
			Details,
			// Token: 0x04009E0D RID: 40461
			Never
		}
	}
}
