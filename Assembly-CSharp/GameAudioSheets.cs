using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class GameAudioSheets : AudioSheets
{
	// Token: 0x06001CBF RID: 7359 RVA: 0x00097A1A File Offset: 0x00095C1A
	public static GameAudioSheets Get()
	{
		if (GameAudioSheets._Instance == null)
		{
			GameAudioSheets._Instance = Resources.Load<GameAudioSheets>("GameAudioSheets");
		}
		return GameAudioSheets._Instance;
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x00097A40 File Offset: 0x00095C40
	public override void Initialize()
	{
		this.validFileNames.Add("game_triggered");
		foreach (KAnimFile kanimFile in Assets.instance.AnimAssets)
		{
			if (!(kanimFile == null))
			{
				this.validFileNames.Add(kanimFile.name);
			}
		}
		base.Initialize();
		foreach (AudioSheet audioSheet in this.sheets)
		{
			foreach (AudioSheet.SoundInfo soundInfo in audioSheet.soundInfos)
			{
				if (soundInfo.Type == "MouthFlapSoundEvent" || soundInfo.Type == "VoiceSoundEvent")
				{
					HashSet<HashedString> hashSet = null;
					if (!this.animsNotAllowedToPlaySpeech.TryGetValue(soundInfo.File, out hashSet))
					{
						hashSet = new HashSet<HashedString>();
						this.animsNotAllowedToPlaySpeech[soundInfo.File] = hashSet;
					}
					hashSet.Add(soundInfo.Anim);
				}
			}
		}
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x00097BA8 File Offset: 0x00095DA8
	protected override AnimEvent CreateSoundOfType(string type, string file_name, string sound_name, int frame, float min_interval, string dlcId)
	{
		SoundEvent soundEvent = null;
		bool shouldCameraScalePosition = true;
		if (sound_name.Contains(":disable_camera_position_scaling"))
		{
			sound_name = sound_name.Replace(":disable_camera_position_scaling", "");
			shouldCameraScalePosition = false;
		}
		if (type == "FloorSoundEvent")
		{
			soundEvent = new FloorSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "SoundEvent" || type == "LoopingSoundEvent")
		{
			bool is_looping = type == "LoopingSoundEvent";
			string[] array = sound_name.Split(':', StringSplitOptions.None);
			sound_name = array[0];
			soundEvent = new SoundEvent(file_name, sound_name, frame, true, is_looping, min_interval, false);
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i] == "IGNORE_PAUSE")
				{
					soundEvent.ignorePause = true;
				}
				else
				{
					global::Debug.LogWarning(sound_name + " has unknown parameter " + array[i]);
				}
			}
		}
		else if (type == "LadderSoundEvent")
		{
			soundEvent = new LadderSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "LaserSoundEvent")
		{
			soundEvent = new LaserSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "HatchDrillSoundEvent")
		{
			soundEvent = new HatchDrillSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "CreatureChewSoundEvent")
		{
			soundEvent = new CreatureChewSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "BuildingDamageSoundEvent")
		{
			soundEvent = new BuildingDamageSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "WallDamageSoundEvent")
		{
			soundEvent = new WallDamageSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "RemoteSoundEvent")
		{
			soundEvent = new RemoteSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "VoiceSoundEvent" || type == "LoopingVoiceSoundEvent")
		{
			soundEvent = new VoiceSoundEvent(file_name, sound_name, frame, type == "LoopingVoiceSoundEvent");
		}
		else if (type == "MouthFlapSoundEvent")
		{
			soundEvent = new MouthFlapSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "MainMenuSoundEvent")
		{
			soundEvent = new MainMenuSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "ClusterMapSoundEvent")
		{
			soundEvent = new ClusterMapSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "ClusterMapLoopingSoundEvent")
		{
			soundEvent = new ClusterMapSoundEvent(file_name, sound_name, frame, true);
		}
		else if (type == "UIAnimationSoundEvent")
		{
			soundEvent = new UIAnimationSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "UIAnimationVoiceSoundEvent")
		{
			soundEvent = new UIAnimationVoiceSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "UIAnimationLoopingSoundEvent")
		{
			soundEvent = new UIAnimationSoundEvent(file_name, sound_name, frame, true);
		}
		else if (type == "CreatureVariationSoundEvent")
		{
			soundEvent = new CreatureVariationSoundEvent(file_name, sound_name, frame, true, type == "LoopingSoundEvent", min_interval, false);
		}
		else if (type == "CountedSoundEvent")
		{
			soundEvent = new CountedSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
		}
		else if (type == "SculptingSoundEvent")
		{
			soundEvent = new SculptingSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
		}
		else if (type == "PhonoboxSoundEvent")
		{
			soundEvent = new PhonoboxSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "PlantMutationSoundEvent")
		{
			soundEvent = new PlantMutationSoundEvent(file_name, sound_name, frame, min_interval);
		}
		if (soundEvent != null)
		{
			soundEvent.shouldCameraScalePosition = shouldCameraScalePosition;
		}
		return soundEvent;
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x00097EF0 File Offset: 0x000960F0
	public bool IsAnimAllowedToPlaySpeech(KAnim.Anim anim)
	{
		HashSet<HashedString> hashSet = null;
		return !this.animsNotAllowedToPlaySpeech.TryGetValue(anim.animFile.name, out hashSet) || !hashSet.Contains(anim.hash);
	}

	// Token: 0x04001036 RID: 4150
	private static GameAudioSheets _Instance;

	// Token: 0x04001037 RID: 4151
	private HashSet<HashedString> validFileNames = new HashSet<HashedString>();

	// Token: 0x04001038 RID: 4152
	private Dictionary<HashedString, HashSet<HashedString>> animsNotAllowedToPlaySpeech = new Dictionary<HashedString, HashSet<HashedString>>();

	// Token: 0x020012D2 RID: 4818
	private class SingleAudioSheetLoader : AsyncLoader
	{
		// Token: 0x060084EA RID: 34026 RVA: 0x003250C0 File Offset: 0x003232C0
		public override void Run()
		{
			this.sheet.soundInfos = new ResourceLoader<AudioSheet.SoundInfo>(this.text, this.name).resources.ToArray();
		}

		// Token: 0x040064AF RID: 25775
		public AudioSheet sheet;

		// Token: 0x040064B0 RID: 25776
		public string text;

		// Token: 0x040064B1 RID: 25777
		public string name;
	}

	// Token: 0x020012D3 RID: 4819
	private class GameAudioSheetLoader : GlobalAsyncLoader<GameAudioSheets.GameAudioSheetLoader>
	{
		// Token: 0x060084EC RID: 34028 RVA: 0x003250F0 File Offset: 0x003232F0
		public override void CollectLoaders(List<AsyncLoader> loaders)
		{
			foreach (AudioSheet audioSheet in GameAudioSheets.Get().sheets)
			{
				loaders.Add(new GameAudioSheets.SingleAudioSheetLoader
				{
					sheet = audioSheet,
					text = audioSheet.asset.text,
					name = audioSheet.asset.name
				});
			}
		}

		// Token: 0x060084ED RID: 34029 RVA: 0x00325174 File Offset: 0x00323374
		public override void Run()
		{
		}
	}
}
