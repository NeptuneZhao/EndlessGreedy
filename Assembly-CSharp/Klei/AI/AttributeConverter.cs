using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F3F RID: 3903
	public class AttributeConverter : Resource
	{
		// Token: 0x0600780F RID: 30735 RVA: 0x002F7E3D File Offset: 0x002F603D
		public AttributeConverter(string id, string name, string description, float multiplier, float base_value, Attribute attribute, IAttributeFormatter formatter = null) : base(id, name)
		{
			this.description = description;
			this.multiplier = multiplier;
			this.baseValue = base_value;
			this.attribute = attribute;
			this.formatter = formatter;
		}

		// Token: 0x06007810 RID: 30736 RVA: 0x002F7E6E File Offset: 0x002F606E
		public AttributeConverterInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x06007811 RID: 30737 RVA: 0x002F7E7C File Offset: 0x002F607C
		public AttributeConverterInstance Lookup(GameObject go)
		{
			AttributeConverters component = go.GetComponent<AttributeConverters>();
			if (component != null)
			{
				return component.Get(this);
			}
			return null;
		}

		// Token: 0x06007812 RID: 30738 RVA: 0x002F7EA4 File Offset: 0x002F60A4
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			string text;
			if (this.formatter != null)
			{
				text = this.formatter.GetFormattedValue(value, this.formatter.DeltaTimeSlice);
			}
			else if (this.attribute.formatter != null)
			{
				text = this.attribute.formatter.GetFormattedValue(value, this.attribute.formatter.DeltaTimeSlice);
			}
			else
			{
				text = GameUtil.GetFormattedSimple(value, GameUtil.TimeSlice.None, null);
			}
			if (text != null)
			{
				text = GameUtil.AddPositiveSign(text, value > 0f);
				return string.Format(this.description, text);
			}
			return null;
		}

		// Token: 0x040059CE RID: 22990
		public string description;

		// Token: 0x040059CF RID: 22991
		public float multiplier;

		// Token: 0x040059D0 RID: 22992
		public float baseValue;

		// Token: 0x040059D1 RID: 22993
		public Attribute attribute;

		// Token: 0x040059D2 RID: 22994
		public IAttributeFormatter formatter;
	}
}
