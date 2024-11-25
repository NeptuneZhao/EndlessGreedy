using System;

namespace Klei.Actions
{
	// Token: 0x02000F8A RID: 3978
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ActionTypeAttribute : Attribute
	{
		// Token: 0x060079DD RID: 31197 RVA: 0x0030162C File Offset: 0x002FF82C
		public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
		{
			this.TypeName = typeName;
			this.GroupName = groupName;
			this.GenerateConfig = generateConfig;
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x0030164C File Offset: 0x002FF84C
		public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			bool flag = object.Equals(lhs, null);
			bool flag2 = object.Equals(rhs, null);
			if (flag || flag2)
			{
				return flag == flag2;
			}
			return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x00301699 File Offset: 0x002FF899
		public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x003016A5 File Offset: 0x002FF8A5
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x003016AE File Offset: 0x002FF8AE
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04005AEA RID: 23274
		public readonly string TypeName;

		// Token: 0x04005AEB RID: 23275
		public readonly string GroupName;

		// Token: 0x04005AEC RID: 23276
		public readonly bool GenerateConfig;
	}
}
