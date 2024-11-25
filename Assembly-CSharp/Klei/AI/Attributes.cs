using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F46 RID: 3910
	public class Attributes
	{
		// Token: 0x06007856 RID: 30806 RVA: 0x002F8D70 File Offset: 0x002F6F70
		public IEnumerator<AttributeInstance> GetEnumerator()
		{
			return this.AttributeTable.GetEnumerator();
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06007857 RID: 30807 RVA: 0x002F8D82 File Offset: 0x002F6F82
		public int Count
		{
			get
			{
				return this.AttributeTable.Count;
			}
		}

		// Token: 0x06007858 RID: 30808 RVA: 0x002F8D8F File Offset: 0x002F6F8F
		public Attributes(GameObject game_object)
		{
			this.gameObject = game_object;
		}

		// Token: 0x06007859 RID: 30809 RVA: 0x002F8DAC File Offset: 0x002F6FAC
		public AttributeInstance Add(Attribute attribute)
		{
			AttributeInstance attributeInstance = this.Get(attribute.Id);
			if (attributeInstance == null)
			{
				attributeInstance = new AttributeInstance(this.gameObject, attribute);
				this.AttributeTable.Add(attributeInstance);
			}
			return attributeInstance;
		}

		// Token: 0x0600785A RID: 30810 RVA: 0x002F8DE4 File Offset: 0x002F6FE4
		public void Add(AttributeModifier modifier)
		{
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Add(modifier);
			}
		}

		// Token: 0x0600785B RID: 30811 RVA: 0x002F8E08 File Offset: 0x002F7008
		public void Remove(AttributeModifier modifier)
		{
			if (modifier == null)
			{
				return;
			}
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Remove(modifier);
			}
		}

		// Token: 0x0600785C RID: 30812 RVA: 0x002F8E30 File Offset: 0x002F7030
		public float GetValuePercent(string attribute_id)
		{
			float result = 1f;
			AttributeInstance attributeInstance = this.Get(attribute_id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue() / attributeInstance.GetBaseValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + attribute_id);
			}
			return result;
		}

		// Token: 0x0600785D RID: 30813 RVA: 0x002F8E70 File Offset: 0x002F7070
		public AttributeInstance Get(string attribute_id)
		{
			for (int i = 0; i < this.AttributeTable.Count; i++)
			{
				if (this.AttributeTable[i].Id == attribute_id)
				{
					return this.AttributeTable[i];
				}
			}
			return null;
		}

		// Token: 0x0600785E RID: 30814 RVA: 0x002F8EBA File Offset: 0x002F70BA
		public AttributeInstance Get(Attribute attribute)
		{
			return this.Get(attribute.Id);
		}

		// Token: 0x0600785F RID: 30815 RVA: 0x002F8EC8 File Offset: 0x002F70C8
		public float GetValue(string id)
		{
			float result = 0f;
			AttributeInstance attributeInstance = this.Get(id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + id);
			}
			return result;
		}

		// Token: 0x06007860 RID: 30816 RVA: 0x002F8F00 File Offset: 0x002F7100
		public AttributeInstance GetProfession()
		{
			AttributeInstance attributeInstance = null;
			foreach (AttributeInstance attributeInstance2 in this)
			{
				if (attributeInstance2.modifier.IsProfession)
				{
					if (attributeInstance == null)
					{
						attributeInstance = attributeInstance2;
					}
					else if (attributeInstance.GetTotalValue() < attributeInstance2.GetTotalValue())
					{
						attributeInstance = attributeInstance2;
					}
				}
			}
			return attributeInstance;
		}

		// Token: 0x06007861 RID: 30817 RVA: 0x002F8F68 File Offset: 0x002F7168
		public string GetProfessionString(bool longform = true)
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, 0, DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_NAME);
			}
			return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, (int)profession.GetTotalValue(), profession.modifier.ProfessionName);
		}

		// Token: 0x06007862 RID: 30818 RVA: 0x002F8FDC File Offset: 0x002F71DC
		public string GetProfessionDescriptionString()
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_DESC;
			}
			return string.Format(DUPLICANTS.ATTRIBUTES.PROFESSION_DESC, profession.modifier.Name);
		}

		// Token: 0x040059E9 RID: 23017
		public List<AttributeInstance> AttributeTable = new List<AttributeInstance>();

		// Token: 0x040059EA RID: 23018
		public GameObject gameObject;
	}
}
