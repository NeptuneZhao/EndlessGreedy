using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020008A9 RID: 2217
public class AttributeModifierExpectation : Expectation
{
	// Token: 0x06003E00 RID: 15872 RVA: 0x00156744 File Offset: 0x00154944
	public AttributeModifierExpectation(string id, string name, string description, AttributeModifier modifier, Sprite icon) : base(id, name, description, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Add(modifier);
	}, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier);
	})
	{
		this.modifier = modifier;
		this.icon = icon;
	}

	// Token: 0x04002610 RID: 9744
	public AttributeModifier modifier;

	// Token: 0x04002611 RID: 9745
	public Sprite icon;
}
