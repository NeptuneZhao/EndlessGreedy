using System;

namespace Database
{
	// Token: 0x02000ECC RID: 3788
	public class SkillPerk : Resource
	{
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06007629 RID: 30249 RVA: 0x002E47FE File Offset: 0x002E29FE
		// (set) Token: 0x0600762A RID: 30250 RVA: 0x002E4806 File Offset: 0x002E2A06
		public Action<MinionResume> OnApply { get; protected set; }

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600762B RID: 30251 RVA: 0x002E480F File Offset: 0x002E2A0F
		// (set) Token: 0x0600762C RID: 30252 RVA: 0x002E4817 File Offset: 0x002E2A17
		public Action<MinionResume> OnRemove { get; protected set; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600762D RID: 30253 RVA: 0x002E4820 File Offset: 0x002E2A20
		// (set) Token: 0x0600762E RID: 30254 RVA: 0x002E4828 File Offset: 0x002E2A28
		public Action<MinionResume> OnMinionsChanged { get; protected set; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x0600762F RID: 30255 RVA: 0x002E4831 File Offset: 0x002E2A31
		// (set) Token: 0x06007630 RID: 30256 RVA: 0x002E4839 File Offset: 0x002E2A39
		public bool affectAll { get; protected set; }

		// Token: 0x06007631 RID: 30257 RVA: 0x002E4842 File Offset: 0x002E2A42
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x002E486B File Offset: 0x002E2A6B
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, string[] requiredDlcIds = null, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
			this.requiredDlcIds = requiredDlcIds;
		}

		// Token: 0x040055C4 RID: 21956
		public string[] requiredDlcIds;
	}
}
