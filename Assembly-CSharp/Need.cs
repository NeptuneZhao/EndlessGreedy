using System;
using Klei.AI;

// Token: 0x020009B8 RID: 2488
public abstract class Need : KMonoBehaviour
{
	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x0600485D RID: 18525 RVA: 0x0019ECE7 File Offset: 0x0019CEE7
	// (set) Token: 0x0600485E RID: 18526 RVA: 0x0019ECEF File Offset: 0x0019CEEF
	public string Name { get; protected set; }

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x0600485F RID: 18527 RVA: 0x0019ECF8 File Offset: 0x0019CEF8
	// (set) Token: 0x06004860 RID: 18528 RVA: 0x0019ED00 File Offset: 0x0019CF00
	public string ExpectationTooltip { get; protected set; }

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06004861 RID: 18529 RVA: 0x0019ED09 File Offset: 0x0019CF09
	// (set) Token: 0x06004862 RID: 18530 RVA: 0x0019ED11 File Offset: 0x0019CF11
	public string Tooltip { get; protected set; }

	// Token: 0x06004863 RID: 18531 RVA: 0x0019ED1A File Offset: 0x0019CF1A
	public Klei.AI.Attribute GetExpectationAttribute()
	{
		return this.expectationAttribute.Attribute;
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x0019ED27 File Offset: 0x0019CF27
	protected void SetModifier(Need.ModifierType modifier)
	{
		if (this.currentStressModifier != modifier)
		{
			if (this.currentStressModifier != null)
			{
				this.UnapplyModifier(this.currentStressModifier);
			}
			if (modifier != null)
			{
				this.ApplyModifier(modifier);
			}
			this.currentStressModifier = modifier;
		}
	}

	// Token: 0x06004865 RID: 18533 RVA: 0x0019ED58 File Offset: 0x0019CF58
	private void ApplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Add(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().AddStatusItem(modifier.statusItem, null);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().AddThought(modifier.thought);
		}
	}

	// Token: 0x06004866 RID: 18534 RVA: 0x0019EDB4 File Offset: 0x0019CFB4
	private void UnapplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Remove(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(modifier.statusItem, false);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().RemoveThought(modifier.thought);
		}
	}

	// Token: 0x04002F76 RID: 12150
	protected AttributeInstance expectationAttribute;

	// Token: 0x04002F77 RID: 12151
	protected Need.ModifierType stressBonus;

	// Token: 0x04002F78 RID: 12152
	protected Need.ModifierType stressNeutral;

	// Token: 0x04002F79 RID: 12153
	protected Need.ModifierType stressPenalty;

	// Token: 0x04002F7A RID: 12154
	protected Need.ModifierType currentStressModifier;

	// Token: 0x020019C0 RID: 6592
	protected class ModifierType
	{
		// Token: 0x04007A9C RID: 31388
		public AttributeModifier modifier;

		// Token: 0x04007A9D RID: 31389
		public StatusItem statusItem;

		// Token: 0x04007A9E RID: 31390
		public Thought thought;
	}
}
