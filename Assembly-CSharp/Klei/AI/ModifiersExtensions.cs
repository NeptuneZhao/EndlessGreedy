using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F71 RID: 3953
	public static class ModifiersExtensions
	{
		// Token: 0x0600794D RID: 31053 RVA: 0x002FF2F6 File Offset: 0x002FD4F6
		public static Attributes GetAttributes(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetAttributes();
		}

		// Token: 0x0600794E RID: 31054 RVA: 0x002FF304 File Offset: 0x002FD504
		public static Attributes GetAttributes(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.attributes;
			}
			return null;
		}

		// Token: 0x0600794F RID: 31055 RVA: 0x002FF329 File Offset: 0x002FD529
		public static Amounts GetAmounts(this KMonoBehaviour cmp)
		{
			if (cmp is Modifiers)
			{
				return ((Modifiers)cmp).amounts;
			}
			return cmp.gameObject.GetAmounts();
		}

		// Token: 0x06007950 RID: 31056 RVA: 0x002FF34C File Offset: 0x002FD54C
		public static Amounts GetAmounts(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.amounts;
			}
			return null;
		}

		// Token: 0x06007951 RID: 31057 RVA: 0x002FF371 File Offset: 0x002FD571
		public static Sicknesses GetSicknesses(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetSicknesses();
		}

		// Token: 0x06007952 RID: 31058 RVA: 0x002FF380 File Offset: 0x002FD580
		public static Sicknesses GetSicknesses(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.sicknesses;
			}
			return null;
		}
	}
}
