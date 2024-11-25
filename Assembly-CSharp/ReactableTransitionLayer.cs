using System;

// Token: 0x020005EE RID: 1518
public class ReactableTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x060024BD RID: 9405 RVA: 0x000CCAC1 File Offset: 0x000CACC1
	public ReactableTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x000CCACA File Offset: 0x000CACCA
	protected override bool IsOverrideComplete()
	{
		return !this.reactionMonitor.IsReacting() && base.IsOverrideComplete();
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x000CCAE4 File Offset: 0x000CACE4
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.reactionMonitor == null)
		{
			this.reactionMonitor = navigator.GetSMI<ReactionMonitor.Instance>();
		}
		this.reactionMonitor.PollForReactables(transition);
		if (this.reactionMonitor.IsReacting())
		{
			base.BeginTransition(navigator, transition);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.end;
		}
	}

	// Token: 0x040014CE RID: 5326
	private ReactionMonitor.Instance reactionMonitor;
}
