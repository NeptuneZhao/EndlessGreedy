using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F41 RID: 3905
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeConverters")]
	public class AttributeConverters : KMonoBehaviour
	{
		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06007816 RID: 30742 RVA: 0x002F7F7E File Offset: 0x002F617E
		public int Count
		{
			get
			{
				return this.converters.Count;
			}
		}

		// Token: 0x06007817 RID: 30743 RVA: 0x002F7F8C File Offset: 0x002F618C
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				foreach (AttributeConverter converter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance item = new AttributeConverterInstance(base.gameObject, converter, attributeInstance);
					this.converters.Add(item);
				}
			}
		}

		// Token: 0x06007818 RID: 30744 RVA: 0x002F8030 File Offset: 0x002F6230
		public AttributeConverterInstance Get(AttributeConverter converter)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter == converter)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x06007819 RID: 30745 RVA: 0x002F808C File Offset: 0x002F628C
		public AttributeConverterInstance GetConverter(string id)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter.Id == id)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x040059D5 RID: 22997
		public List<AttributeConverterInstance> converters = new List<AttributeConverterInstance>();
	}
}
