using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
public interface IGameObjectEffectDescriptor
{
	// Token: 0x060041DF RID: 16863
	List<Descriptor> GetDescriptors(GameObject go);
}
