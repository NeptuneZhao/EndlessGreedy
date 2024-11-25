using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F72 RID: 3954
	[AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
	public class PrefabAttributeModifiers : KMonoBehaviour
	{
		// Token: 0x06007953 RID: 31059 RVA: 0x002FF3A5 File Offset: 0x002FD5A5
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		// Token: 0x06007954 RID: 31060 RVA: 0x002FF3AD File Offset: 0x002FD5AD
		public void AddAttributeDescriptor(AttributeModifier modifier)
		{
			this.descriptors.Add(modifier);
		}

		// Token: 0x06007955 RID: 31061 RVA: 0x002FF3BB File Offset: 0x002FD5BB
		public void RemovePrefabAttribute(AttributeModifier modifier)
		{
			this.descriptors.Remove(modifier);
		}

		// Token: 0x04005A98 RID: 23192
		public List<AttributeModifier> descriptors = new List<AttributeModifier>();
	}
}
