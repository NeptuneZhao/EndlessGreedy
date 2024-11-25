using System;
using UnityEngine;

// Token: 0x020004F0 RID: 1264
public class PlantMutationSoundEvent : SoundEvent
{
	// Token: 0x06001C13 RID: 7187 RVA: 0x00093C1C File Offset: 0x00091E1C
	public PlantMutationSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x00093C2C File Offset: 0x00091E2C
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		MutantPlant component = behaviour.controller.gameObject.GetComponent<MutantPlant>();
		Vector3 position = behaviour.position;
		if (component != null)
		{
			for (int i = 0; i < component.GetSoundEvents().Count; i++)
			{
				SoundEvent.PlayOneShot(component.GetSoundEvents()[i], position, 1f);
			}
		}
	}
}
