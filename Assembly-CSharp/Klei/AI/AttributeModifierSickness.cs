using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F57 RID: 3927
	public class AttributeModifierSickness : Sickness.SicknessComponent
	{
		// Token: 0x060078AB RID: 30891 RVA: 0x002FC480 File Offset: 0x002FA680
		public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
		{
			this.attributeModifiers = attribute_modifiers;
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x002FC490 File Offset: 0x002FA690
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			Attributes attributes = go.GetAttributes();
			for (int i = 0; i < this.attributeModifiers.Length; i++)
			{
				AttributeModifier modifier = this.attributeModifiers[i];
				attributes.Add(modifier);
			}
			return null;
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x002FC4C8 File Offset: 0x002FA6C8
		public override void OnCure(GameObject go, object instance_data)
		{
			Attributes attributes = go.GetAttributes();
			for (int i = 0; i < this.attributeModifiers.Length; i++)
			{
				AttributeModifier modifier = this.attributeModifiers[i];
				attributes.Remove(modifier);
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060078AE RID: 30894 RVA: 0x002FC4FF File Offset: 0x002FA6FF
		public AttributeModifier[] Modifers
		{
			get
			{
				return this.attributeModifiers;
			}
		}

		// Token: 0x060078AF RID: 30895 RVA: 0x002FC508 File Offset: 0x002FA708
		public override List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x04005A2A RID: 23082
		private AttributeModifier[] attributeModifiers;
	}
}
