using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F42 RID: 3906
	[DebuggerDisplay("{Attribute.Id}")]
	public class AttributeInstance : ModifierInstance<Attribute>
	{
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600781B RID: 30747 RVA: 0x002F8107 File Offset: 0x002F6307
		public string Id
		{
			get
			{
				return this.Attribute.Id;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x0600781C RID: 30748 RVA: 0x002F8114 File Offset: 0x002F6314
		public string Name
		{
			get
			{
				return this.Attribute.Name;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x0600781D RID: 30749 RVA: 0x002F8121 File Offset: 0x002F6321
		public string Description
		{
			get
			{
				return this.Attribute.Description;
			}
		}

		// Token: 0x0600781E RID: 30750 RVA: 0x002F812E File Offset: 0x002F632E
		public float GetBaseValue()
		{
			return this.Attribute.BaseValue;
		}

		// Token: 0x0600781F RID: 30751 RVA: 0x002F813C File Offset: 0x002F633C
		public float GetTotalDisplayValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06007820 RID: 30752 RVA: 0x002F81B0 File Offset: 0x002F63B0
		public float GetTotalValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06007821 RID: 30753 RVA: 0x002F822C File Offset: 0x002F642C
		public static float GetTotalDisplayValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06007822 RID: 30754 RVA: 0x002F8290 File Offset: 0x002F6490
		public static float GetTotalValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06007823 RID: 30755 RVA: 0x002F82FC File Offset: 0x002F64FC
		public float GetModifierContribution(AttributeModifier testModifier)
		{
			if (!testModifier.IsMultiplier)
			{
				return testModifier.Value;
			}
			float num = this.Attribute.BaseValue;
			for (int num2 = 0; num2 != this.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num2];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
			}
			return num * testModifier.Value;
		}

		// Token: 0x06007824 RID: 30756 RVA: 0x002F8360 File Offset: 0x002F6560
		public AttributeInstance(GameObject game_object, Attribute attribute) : base(game_object, attribute)
		{
			DebugUtil.Assert(attribute != null);
			this.Attribute = attribute;
		}

		// Token: 0x06007825 RID: 30757 RVA: 0x002F837A File Offset: 0x002F657A
		public void Add(AttributeModifier modifier)
		{
			this.Modifiers.Add(modifier);
			if (this.OnDirty != null)
			{
				this.OnDirty();
			}
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x002F839C File Offset: 0x002F659C
		public void Remove(AttributeModifier modifier)
		{
			int i = 0;
			while (i < this.Modifiers.Count)
			{
				if (this.Modifiers[i] == modifier)
				{
					this.Modifiers.RemoveAt(i);
					if (this.OnDirty != null)
					{
						this.OnDirty();
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x002F83EE File Offset: 0x002F65EE
		public void ClearModifiers()
		{
			if (this.Modifiers.Count > 0)
			{
				this.Modifiers.Clear();
				if (this.OnDirty != null)
				{
					this.OnDirty();
				}
			}
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x002F841C File Offset: 0x002F661C
		public string GetDescription()
		{
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, this.Name, this.GetFormattedValue());
		}

		// Token: 0x06007829 RID: 30761 RVA: 0x002F8439 File Offset: 0x002F6639
		public string GetFormattedValue()
		{
			return this.Attribute.formatter.GetFormattedAttribute(this);
		}

		// Token: 0x0600782A RID: 30762 RVA: 0x002F844C File Offset: 0x002F664C
		public string GetAttributeValueTooltip()
		{
			return this.Attribute.GetTooltip(this);
		}

		// Token: 0x040059D6 RID: 22998
		public Attribute Attribute;

		// Token: 0x040059D7 RID: 22999
		public System.Action OnDirty;

		// Token: 0x040059D8 RID: 23000
		public ArrayRef<AttributeModifier> Modifiers;

		// Token: 0x040059D9 RID: 23001
		public bool hide;
	}
}
