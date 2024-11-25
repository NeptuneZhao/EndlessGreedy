using System;

// Token: 0x0200057C RID: 1404
public class KnockKnock : Activatable
{
	// Token: 0x060020A0 RID: 8352 RVA: 0x000B6587 File Offset: 0x000B4787
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x000B6596 File Offset: 0x000B4796
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!this.doorAnswered)
		{
			this.workTimeRemaining += dt;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x000B65B6 File Offset: 0x000B47B6
	public void AnswerDoor()
	{
		this.doorAnswered = true;
		this.workTimeRemaining = 1f;
	}

	// Token: 0x04001255 RID: 4693
	private bool doorAnswered;
}
