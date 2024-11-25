using System;
using UnityEngine;

// Token: 0x0200078C RID: 1932
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTubeBridge")]
public class TravelTubeBridge : KMonoBehaviour, ITravelTubePiece
{
	// Token: 0x1700039B RID: 923
	// (get) Token: 0x060034A0 RID: 13472 RVA: 0x0011EE86 File Offset: 0x0011D086
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0011EE94 File Offset: 0x0011D094
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
		base.Subscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
		base.Subscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0011EEE8 File Offset: 0x0011D0E8
	protected override void OnCleanUp()
	{
		base.Unsubscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate, false);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0011EF39 File Offset: 0x0011D139
	private void OnBuildingBroken(object data)
	{
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0011EF3B File Offset: 0x0011D13B
	private void OnBuildingFullyRepaired(object data)
	{
	}

	// Token: 0x04001F18 RID: 7960
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001F19 RID: 7961
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
