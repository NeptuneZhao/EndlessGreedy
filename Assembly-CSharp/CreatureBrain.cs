using System;

// Token: 0x02000421 RID: 1057
public class CreatureBrain : Brain
{
	// Token: 0x06001690 RID: 5776 RVA: 0x00078FA8 File Offset: 0x000771A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			component.SetAbilities(new CreaturePathFinderAbilities(component));
		}
	}

	// Token: 0x04000C9C RID: 3228
	public string symbolPrefix;

	// Token: 0x04000C9D RID: 3229
	public Tag species;
}
