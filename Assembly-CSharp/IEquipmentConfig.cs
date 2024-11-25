using System;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public interface IEquipmentConfig
{
	// Token: 0x06003D6B RID: 15723
	EquipmentDef CreateEquipmentDef();

	// Token: 0x06003D6C RID: 15724
	void DoPostConfigure(GameObject go);

	// Token: 0x06003D6D RID: 15725
	string[] GetDlcIds();
}
