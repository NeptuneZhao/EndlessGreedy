using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000746 RID: 1862
[AddComponentMenu("KMonoBehaviour/scripts/ParkSign")]
public class ParkSign : KMonoBehaviour
{
	// Token: 0x06003199 RID: 12697 RVA: 0x00110F3E File Offset: 0x0010F13E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ParkSign>(-832141045, ParkSign.TriggerRoomEffectsDelegate);
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x00110F58 File Offset: 0x0010F158
	private void TriggerRoomEffects(object data)
	{
		GameObject gameObject = (GameObject)data;
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.gameObject.GetComponent<KPrefabID>(), gameObject.GetComponent<Effects>());
		}
	}

	// Token: 0x04001D2C RID: 7468
	private static readonly EventSystem.IntraObjectHandler<ParkSign> TriggerRoomEffectsDelegate = new EventSystem.IntraObjectHandler<ParkSign>(delegate(ParkSign component, object data)
	{
		component.TriggerRoomEffects(data);
	});
}
