using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02000F6E RID: 3950
	public class ModifierGroup<T> : Resource
	{
		// Token: 0x06007930 RID: 31024 RVA: 0x002FEDA8 File Offset: 0x002FCFA8
		public IEnumerator<T> GetEnumerator()
		{
			return this.modifiers.GetEnumerator();
		}

		// Token: 0x170008B0 RID: 2224
		public T this[int idx]
		{
			get
			{
				return this.modifiers[idx];
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06007932 RID: 31026 RVA: 0x002FEDC8 File Offset: 0x002FCFC8
		public int Count
		{
			get
			{
				return this.modifiers.Count;
			}
		}

		// Token: 0x06007933 RID: 31027 RVA: 0x002FEDD5 File Offset: 0x002FCFD5
		public ModifierGroup(string id, string name) : base(id, name)
		{
		}

		// Token: 0x06007934 RID: 31028 RVA: 0x002FEDEA File Offset: 0x002FCFEA
		public void Add(T modifier)
		{
			this.modifiers.Add(modifier);
		}

		// Token: 0x04005A8F RID: 23183
		public List<T> modifiers = new List<T>();
	}
}
