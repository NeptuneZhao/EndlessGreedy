using System;

// Token: 0x0200050D RID: 1293
public class ElementsAudio
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06001CBB RID: 7355 RVA: 0x000979AE File Offset: 0x00095BAE
	public static ElementsAudio Instance
	{
		get
		{
			if (ElementsAudio._instance == null)
			{
				ElementsAudio._instance = new ElementsAudio();
			}
			return ElementsAudio._instance;
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000979C6 File Offset: 0x00095BC6
	public void LoadData(ElementsAudio.ElementAudioConfig[] elements_audio_configs)
	{
		this.elementAudioConfigs = elements_audio_configs;
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000979D0 File Offset: 0x00095BD0
	public ElementsAudio.ElementAudioConfig GetConfigForElement(SimHashes id)
	{
		if (this.elementAudioConfigs != null)
		{
			for (int i = 0; i < this.elementAudioConfigs.Length; i++)
			{
				if (this.elementAudioConfigs[i].elementID == id)
				{
					return this.elementAudioConfigs[i];
				}
			}
		}
		return null;
	}

	// Token: 0x04001034 RID: 4148
	private static ElementsAudio _instance;

	// Token: 0x04001035 RID: 4149
	private ElementsAudio.ElementAudioConfig[] elementAudioConfigs;

	// Token: 0x020012D1 RID: 4817
	public class ElementAudioConfig : Resource
	{
		// Token: 0x040064A7 RID: 25767
		public SimHashes elementID;

		// Token: 0x040064A8 RID: 25768
		public AmbienceType ambienceType = AmbienceType.None;

		// Token: 0x040064A9 RID: 25769
		public SolidAmbienceType solidAmbienceType = SolidAmbienceType.None;

		// Token: 0x040064AA RID: 25770
		public string miningSound = "";

		// Token: 0x040064AB RID: 25771
		public string miningBreakSound = "";

		// Token: 0x040064AC RID: 25772
		public string oreBumpSound = "";

		// Token: 0x040064AD RID: 25773
		public string floorEventAudioCategory = "";

		// Token: 0x040064AE RID: 25774
		public string creatureChewSound = "";
	}
}
