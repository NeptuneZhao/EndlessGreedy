using System;
using UnityEngine;

// Token: 0x02000A58 RID: 2648
[AddComponentMenu("KMonoBehaviour/scripts/Reservable")]
public class Reservable : KMonoBehaviour
{
	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06004CDB RID: 19675 RVA: 0x001B7489 File Offset: 0x001B5689
	public GameObject ReservedBy
	{
		get
		{
			return this.reservedBy;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06004CDC RID: 19676 RVA: 0x001B7491 File Offset: 0x001B5691
	public bool isReserved
	{
		get
		{
			return !(this.reservedBy == null);
		}
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x001B74A2 File Offset: 0x001B56A2
	public bool Reserve(GameObject reserver)
	{
		if (this.reservedBy == null)
		{
			this.reservedBy = reserver;
			return true;
		}
		return false;
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x001B74BC File Offset: 0x001B56BC
	public void ClearReservation(GameObject reserver)
	{
		if (this.reservedBy == reserver)
		{
			this.reservedBy = null;
		}
	}

	// Token: 0x04003317 RID: 13079
	private GameObject reservedBy;
}
