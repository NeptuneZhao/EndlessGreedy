using System;
using UnityEngine;

// Token: 0x020008FA RID: 2298
public class CarePackageInfo : ITelepadDeliverable
{
	// Token: 0x06004211 RID: 16913 RVA: 0x00177B69 File Offset: 0x00175D69
	public CarePackageInfo(string ID, float amount, Func<bool> requirement)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x00177B86 File Offset: 0x00175D86
	public CarePackageInfo(string ID, float amount, Func<bool> requirement, string facadeID)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
		this.facadeID = facadeID;
	}

	// Token: 0x06004213 RID: 16915 RVA: 0x00177BAC File Offset: 0x00175DAC
	public GameObject Deliver(Vector3 location)
	{
		location += Vector3.right / 2f;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(CarePackageConfig.ID), location);
		gameObject.SetActive(true);
		gameObject.GetComponent<CarePackage>().SetInfo(this);
		return gameObject;
	}

	// Token: 0x04002BC8 RID: 11208
	public readonly string id;

	// Token: 0x04002BC9 RID: 11209
	public readonly float quantity;

	// Token: 0x04002BCA RID: 11210
	public readonly Func<bool> requirement;

	// Token: 0x04002BCB RID: 11211
	public readonly string facadeID;
}
