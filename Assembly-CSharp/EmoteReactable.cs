using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class EmoteReactable : Reactable
{
	// Token: 0x06001993 RID: 6547 RVA: 0x00088CB8 File Offset: 0x00086EB8
	public EmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, range_width, range_height, true, globalCooldown, localCooldown, lifeSpan, max_initial_delay, ObjectLayer.NumLayers)
	{
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x00088CE4 File Offset: 0x00086EE4
	public EmoteReactable SetEmote(Emote emote)
	{
		this.emote = emote;
		return this;
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00088CF0 File Offset: 0x00086EF0
	public EmoteReactable RegisterEmoteStepCallbacks(HashedString stepName, Action<GameObject> startedCb, Action<GameObject> finishedCb)
	{
		if (this.callbackHandles == null)
		{
			this.callbackHandles = new HandleVector<EmoteStep.Callbacks>.Handle[this.emote.StepCount];
		}
		int stepIndex = this.emote.GetStepIndex(stepName);
		this.callbackHandles[stepIndex] = this.emote[stepIndex].RegisterCallbacks(startedCb, finishedCb);
		return this;
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x00088D48 File Offset: 0x00086F48
	public EmoteReactable SetExpression(Expression expression)
	{
		this.expression = expression;
		return this;
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x00088D52 File Offset: 0x00086F52
	public EmoteReactable SetThought(Thought thought)
	{
		this.thought = thought;
		return this;
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x00088D5C File Offset: 0x00086F5C
	public EmoteReactable SetOverideAnimSet(string animSet)
	{
		this.overrideAnimSet = Assets.GetAnim(animSet);
		return this;
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x00088D70 File Offset: 0x00086F70
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null || new_reactor == null)
		{
			return false;
		}
		Navigator component = new_reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving() && (-257 & 1 << (int)component.CurrentNavType) != 0 && this.gameObject != new_reactor;
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x00088DD4 File Offset: 0x00086FD4
	public override void Update(float dt)
	{
		if (this.emote == null || !this.emote.IsValidStep(this.currentStep))
		{
			return;
		}
		if (this.gameObject != null && this.reactor != null)
		{
			Facing component = this.reactor.GetComponent<Facing>();
			if (component != null)
			{
				component.Face(this.gameObject.transform.GetPosition());
			}
		}
		float timeout = this.emote[this.currentStep].timeout;
		if (timeout > 0f && timeout < this.elapsed)
		{
			this.NextStep(null);
			return;
		}
		this.elapsed += dt;
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x00088E88 File Offset: 0x00087088
	protected override void InternalBegin()
	{
		this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
		this.emote.ApplyAnimOverrides(this.kbac, this.overrideAnimSet);
		if (this.expression != null)
		{
			this.reactor.GetComponent<FaceGraph>().AddExpression(this.expression);
		}
		if (this.thought != null)
		{
			this.reactor.GetSMI<ThoughtGraph.Instance>().AddThought(this.thought);
		}
		this.NextStep(null);
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x00088F08 File Offset: 0x00087108
	protected override void InternalEnd()
	{
		if (this.kbac != null)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			this.emote.RemoveAnimOverrides(this.kbac, this.overrideAnimSet);
			this.kbac = null;
		}
		if (this.reactor != null)
		{
			if (this.expression != null)
			{
				this.reactor.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			if (this.thought != null)
			{
				this.reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(this.thought);
			}
		}
		this.currentStep = -1;
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x00088FAC File Offset: 0x000871AC
	protected override void InternalCleanup()
	{
		if (this.emote == null || this.callbackHandles == null)
		{
			return;
		}
		int num = 0;
		while (this.emote.IsValidStep(num))
		{
			this.emote[num].UnregisterCallbacks(this.callbackHandles[num]);
			num++;
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x00089000 File Offset: 0x00087200
	private void NextStep(HashedString finishedAnim)
	{
		if (this.emote.IsValidStep(this.currentStep) && this.emote[this.currentStep].timeout <= 0f)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			if (this.callbackHandles != null)
			{
				this.emote[this.currentStep].OnStepFinished(this.callbackHandles[this.currentStep], this.reactor);
			}
		}
		this.currentStep++;
		if (!this.emote.IsValidStep(this.currentStep) || this.kbac == null)
		{
			base.End();
			return;
		}
		EmoteStep emoteStep = this.emote[this.currentStep];
		if (emoteStep.anim != HashedString.Invalid)
		{
			this.kbac.Play(emoteStep.anim, emoteStep.mode, 1f, 0f);
			if (this.kbac.IsStopped())
			{
				emoteStep.timeout = 0.25f;
			}
		}
		if (emoteStep.timeout <= 0f)
		{
			this.kbac.onAnimComplete += this.NextStep;
		}
		else
		{
			this.elapsed = 0f;
		}
		if (this.callbackHandles != null)
		{
			emoteStep.OnStepStarted(this.callbackHandles[this.currentStep], this.reactor);
		}
	}

	// Token: 0x04000E8E RID: 3726
	private KBatchedAnimController kbac;

	// Token: 0x04000E8F RID: 3727
	public Expression expression;

	// Token: 0x04000E90 RID: 3728
	public Thought thought;

	// Token: 0x04000E91 RID: 3729
	public Emote emote;

	// Token: 0x04000E92 RID: 3730
	private HandleVector<EmoteStep.Callbacks>.Handle[] callbackHandles;

	// Token: 0x04000E93 RID: 3731
	protected KAnimFile overrideAnimSet;

	// Token: 0x04000E94 RID: 3732
	private int currentStep = -1;

	// Token: 0x04000E95 RID: 3733
	private float elapsed;
}
