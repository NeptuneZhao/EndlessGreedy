using System;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class SelfEmoteReactable : EmoteReactable
{
	// Token: 0x060019BB RID: 6587 RVA: 0x000897BC File Offset: 0x000879BC
	public SelfEmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000897DC File Offset: 0x000879DC
	public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != this.gameObject)
		{
			return false;
		}
		Navigator component = reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x00089814 File Offset: 0x00087A14
	public void PairEmote(EmoteChore emoteChore)
	{
		this.chore = emoteChore;
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x00089820 File Offset: 0x00087A20
	protected override void InternalEnd()
	{
		if (this.chore != null && this.chore.driver != null)
		{
			this.chore.PairReactable(null);
			this.chore.Cancel("Reactable ended");
			this.chore = null;
		}
		base.InternalEnd();
	}

	// Token: 0x04000EAD RID: 3757
	private EmoteChore chore;
}
