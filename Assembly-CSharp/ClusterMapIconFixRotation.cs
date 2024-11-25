using System;

// Token: 0x02000AF3 RID: 2803
public class ClusterMapIconFixRotation : KMonoBehaviour
{
	// Token: 0x0600539E RID: 21406 RVA: 0x001DF620 File Offset: 0x001DD820
	private void Update()
	{
		if (base.transform.parent != null)
		{
			float z = base.transform.parent.rotation.eulerAngles.z;
			this.rotation = -z;
			this.animController.Rotation = this.rotation;
		}
	}

	// Token: 0x0400370D RID: 14093
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x0400370E RID: 14094
	private float rotation;
}
