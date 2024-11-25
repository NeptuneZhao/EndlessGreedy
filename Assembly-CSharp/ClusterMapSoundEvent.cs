using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
public class ClusterMapSoundEvent : SoundEvent
{
	// Token: 0x06001AD1 RID: 6865 RVA: 0x0008D133 File Offset: 0x0008B333
	public ClusterMapSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x0008D148 File Offset: 0x0008B348
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		if (ClusterMapScreen.Instance.IsActive())
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x0008D160 File Offset: 0x0008B360
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (Cluster Map Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, true, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
			instance.setParameterByName(ClusterMapSoundEvent.X_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().x / (float)Screen.width, false);
			instance.setParameterByName(ClusterMapSoundEvent.Y_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().y / (float)Screen.height, false);
			instance.setParameterByName(ClusterMapSoundEvent.ZOOM_PARAMETER, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x0008D260 File Offset: 0x0008B460
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}

	// Token: 0x04000F35 RID: 3893
	private static string X_POSITION_PARAMETER = "Starmap_Position_X";

	// Token: 0x04000F36 RID: 3894
	private static string Y_POSITION_PARAMETER = "Starmap_Position_Y";

	// Token: 0x04000F37 RID: 3895
	private static string ZOOM_PARAMETER = "Starmap_Zoom_Percentage";
}
