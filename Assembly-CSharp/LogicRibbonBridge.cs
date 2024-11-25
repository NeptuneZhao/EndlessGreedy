using System;

// Token: 0x02000716 RID: 1814
public class LogicRibbonBridge : KMonoBehaviour
{
	// Token: 0x06002F9B RID: 12187 RVA: 0x00108E0C File Offset: 0x0010700C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		switch (base.GetComponent<Rotatable>().GetOrientation())
		{
		case Orientation.Neutral:
			component.Play("0", KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Orientation.R90:
			component.Play("90", KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Orientation.R180:
			component.Play("180", KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Orientation.R270:
			component.Play("270", KAnim.PlayMode.Once, 1f, 0f);
			return;
		default:
			return;
		}
	}
}
