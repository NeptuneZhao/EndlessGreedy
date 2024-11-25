using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class HugMinionReactable : Reactable
{
	// Token: 0x06000412 RID: 1042 RVA: 0x00020EA8 File Offset: 0x0001F0A8
	public HugMinionReactable(GameObject gameObject) : base(gameObject, "HugMinionReactable", Db.Get().ChoreTypes.Hug, 1, 1, true, 1f, 0f, float.PositiveInfinity, 0f, ObjectLayer.Minion)
	{
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00020EF0 File Offset: 0x0001F0F0
	public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null)
		{
			return false;
		}
		Navigator component = newReactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00020F2A File Offset: 0x0001F12A
	public override void Update(float dt)
	{
		this.gameObject.GetComponent<Facing>().SetFacing(this.reactor.GetComponent<Facing>().GetFacing());
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00020F4C File Offset: 0x0001F14C
	protected override void InternalBegin()
	{
		KAnimControllerBase component = this.reactor.GetComponent<KAnimControllerBase>();
		component.AddAnimOverrides(Assets.GetAnim("anim_react_pip_kanim"), 0f);
		component.Play("hug_dupe_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_loop", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_pst", KAnim.PlayMode.Once, 1f, 0f);
		component.onAnimComplete += this.Finish;
		this.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(new HashedString[]
		{
			"hug_dupe_pre",
			"hug_dupe_loop",
			"hug_dupe_pst"
		});
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0002102C File Offset: 0x0001F22C
	private void Finish(HashedString anim)
	{
		if (anim == "hug_dupe_pst")
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KAnimControllerBase>().onAnimComplete -= this.Finish;
				this.ApplyEffects();
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"HugMinionReactable finishing without adding a Hugged effect."
				});
			}
			base.End();
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00021098 File Offset: 0x0001F298
	private void ApplyEffects()
	{
		this.reactor.GetComponent<Effects>().Add("Hugged", true);
		HugMonitor.Instance smi = this.gameObject.GetSMI<HugMonitor.Instance>();
		if (smi != null)
		{
			smi.EnterHuggingFrenzy();
		}
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x000210D1 File Offset: 0x0001F2D1
	protected override void InternalEnd()
	{
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x000210D3 File Offset: 0x0001F2D3
	protected override void InternalCleanup()
	{
	}
}
