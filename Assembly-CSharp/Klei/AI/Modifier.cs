using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02000F6D RID: 3949
	public class Modifier : Resource
	{
		// Token: 0x0600792C RID: 31020 RVA: 0x002FECC4 File Offset: 0x002FCEC4
		public Modifier(string id, string name, string description) : base(id, name)
		{
			this.description = description;
		}

		// Token: 0x0600792D RID: 31021 RVA: 0x002FECE0 File Offset: 0x002FCEE0
		public void Add(AttributeModifier modifier)
		{
			if (modifier.AttributeId != "")
			{
				this.SelfModifiers.Add(modifier);
			}
		}

		// Token: 0x0600792E RID: 31022 RVA: 0x002FED00 File Offset: 0x002FCF00
		public virtual void AddTo(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x0600792F RID: 31023 RVA: 0x002FED54 File Offset: 0x002FCF54
		public virtual void RemoveFrom(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x04005A8D RID: 23181
		public string description;

		// Token: 0x04005A8E RID: 23182
		public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
	}
}
