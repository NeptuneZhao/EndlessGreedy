using System;
using Database;

// Token: 0x02000B7C RID: 2940
public class EntityModifierSet : ModifierSet
{
	// Token: 0x0600585F RID: 22623 RVA: 0x001FE278 File Offset: 0x001FC478
	public override void Initialize()
	{
		base.Initialize();
		this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
		this.ChoreGroups = new ChoreGroups(this.Root);
		base.LoadTraits();
	}

	// Token: 0x040039FE RID: 14846
	public DuplicantStatusItems DuplicantStatusItems;

	// Token: 0x040039FF RID: 14847
	public ChoreGroups ChoreGroups;
}
