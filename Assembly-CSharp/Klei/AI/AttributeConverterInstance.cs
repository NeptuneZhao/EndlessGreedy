using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F40 RID: 3904
	public class AttributeConverterInstance : ModifierInstance<AttributeConverter>
	{
		// Token: 0x06007813 RID: 30739 RVA: 0x002F7F2D File Offset: 0x002F612D
		public AttributeConverterInstance(GameObject game_object, AttributeConverter converter, AttributeInstance attribute_instance) : base(game_object, converter)
		{
			this.converter = converter;
			this.attributeInstance = attribute_instance;
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x002F7F45 File Offset: 0x002F6145
		public float Evaluate()
		{
			return this.converter.multiplier * this.attributeInstance.GetTotalValue() + this.converter.baseValue;
		}

		// Token: 0x06007815 RID: 30741 RVA: 0x002F7F6A File Offset: 0x002F616A
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			return this.converter.DescriptionFromAttribute(this.Evaluate(), go);
		}

		// Token: 0x040059D3 RID: 22995
		public AttributeConverter converter;

		// Token: 0x040059D4 RID: 22996
		public AttributeInstance attributeInstance;
	}
}
