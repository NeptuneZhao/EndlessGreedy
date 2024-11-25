using System;
using Database;

// Token: 0x02000DC1 RID: 3521
public class SkillListable : IListableOption
{
	// Token: 0x06006FA6 RID: 28582 RVA: 0x002A0BC4 File Offset: 0x0029EDC4
	public SkillListable(string name)
	{
		this.skillName = name;
		Skill skill = Db.Get().Skills.TryGet(this.skillName);
		if (skill != null)
		{
			this.name = skill.Name;
			this.skillHat = skill.hat;
		}
	}

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06006FA7 RID: 28583 RVA: 0x002A0C14 File Offset: 0x0029EE14
	// (set) Token: 0x06006FA8 RID: 28584 RVA: 0x002A0C1C File Offset: 0x0029EE1C
	public string skillName { get; private set; }

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06006FA9 RID: 28585 RVA: 0x002A0C25 File Offset: 0x0029EE25
	// (set) Token: 0x06006FAA RID: 28586 RVA: 0x002A0C2D File Offset: 0x0029EE2D
	public string skillHat { get; private set; }

	// Token: 0x06006FAB RID: 28587 RVA: 0x002A0C36 File Offset: 0x0029EE36
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x04004C5F RID: 19551
	public LocString name;
}
