using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000581 RID: 1409
public abstract class LoopingSoundParameterUpdater
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x060020BD RID: 8381 RVA: 0x000B71DE File Offset: 0x000B53DE
	// (set) Token: 0x060020BE RID: 8382 RVA: 0x000B71E6 File Offset: 0x000B53E6
	public HashedString parameter { get; private set; }

	// Token: 0x060020BF RID: 8383 RVA: 0x000B71EF File Offset: 0x000B53EF
	public LoopingSoundParameterUpdater(HashedString parameter)
	{
		this.parameter = parameter;
	}

	// Token: 0x060020C0 RID: 8384
	public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x060020C1 RID: 8385
	public abstract void Update(float dt);

	// Token: 0x060020C2 RID: 8386
	public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x02001379 RID: 4985
	public struct Sound
	{
		// Token: 0x040066BF RID: 26303
		public EventInstance ev;

		// Token: 0x040066C0 RID: 26304
		public HashedString path;

		// Token: 0x040066C1 RID: 26305
		public Transform transform;

		// Token: 0x040066C2 RID: 26306
		public SoundDescription description;

		// Token: 0x040066C3 RID: 26307
		public bool objectIsSelectedAndVisible;
	}
}
