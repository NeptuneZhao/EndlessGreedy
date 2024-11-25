using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class WallDamageSoundEvent : SoundEvent
{
	// Token: 0x06001CDD RID: 7389 RVA: 0x00098938 File Offset: 0x00096B38
	public WallDamageSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, false, min_interval, false)
	{
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x00098948 File Offset: 0x00096B48
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = default(Vector3);
		AggressiveChore.StatesInstance smi = behaviour.controller.gameObject.GetSMI<AggressiveChore.StatesInstance>();
		if (smi != null)
		{
			this.tile = smi.sm.wallCellToBreak;
			int audioCategory = WallDamageSoundEvent.GetAudioCategory(this.tile);
			vector = Grid.CellToPos(this.tile);
			vector.z = 0f;
			GameObject gameObject = behaviour.controller.gameObject;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
			{
				EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
				instance.setParameterByName("material_ID", (float)audioCategory, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x00098A24 File Offset: 0x00096C24
	private static int GetAudioCategory(int tile)
	{
		Element element = Grid.Element[tile];
		if (Grid.Foundation[tile])
		{
			return 12;
		}
		if (element.id == SimHashes.Dirt)
		{
			return 0;
		}
		if (element.id == SimHashes.CrushedIce || element.id == SimHashes.Ice || element.id == SimHashes.DirtyIce)
		{
			return 1;
		}
		if (element.id == SimHashes.OxyRock)
		{
			return 3;
		}
		if (element.HasTag(GameTags.Metal))
		{
			return 5;
		}
		if (element.HasTag(GameTags.RefinedMetal))
		{
			return 6;
		}
		if (element.id == SimHashes.Sand)
		{
			return 8;
		}
		if (element.id == SimHashes.Algae)
		{
			return 10;
		}
		return 7;
	}

	// Token: 0x04001048 RID: 4168
	public int tile;
}
