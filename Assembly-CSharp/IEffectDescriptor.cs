using System;
using System.Collections.Generic;

// Token: 0x020008F3 RID: 2291
[Obsolete("No longer used. Use IGameObjectEffectDescriptor instead", false)]
public interface IEffectDescriptor
{
	// Token: 0x060041E0 RID: 16864
	List<Descriptor> GetDescriptors(BuildingDef def);
}
