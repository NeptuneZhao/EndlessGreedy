using System;
using System.Diagnostics;
using System.IO;
using Klei;
using Newtonsoft.Json;

namespace KMod
{
	// Token: 0x02000EE1 RID: 3809
	[JsonObject(MemberSerialization.Fields)]
	[DebuggerDisplay("{title}")]
	public struct Label
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06007683 RID: 30339 RVA: 0x002E8CAD File Offset: 0x002E6EAD
		[JsonIgnore]
		private string distribution_platform_name
		{
			get
			{
				return this.distribution_platform.ToString();
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06007684 RID: 30340 RVA: 0x002E8CC0 File Offset: 0x002E6EC0
		[JsonIgnore]
		public string install_path
		{
			get
			{
				return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name, this.id));
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06007685 RID: 30341 RVA: 0x002E8CDD File Offset: 0x002E6EDD
		[JsonIgnore]
		public string defaultStaticID
		{
			get
			{
				return this.id + "." + this.distribution_platform.ToString();
			}
		}

		// Token: 0x06007686 RID: 30342 RVA: 0x002E8D00 File Offset: 0x002E6F00
		public override string ToString()
		{
			return this.title;
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x002E8D08 File Offset: 0x002E6F08
		public bool Match(Label rhs)
		{
			return this.id == rhs.id && this.distribution_platform == rhs.distribution_platform;
		}

		// Token: 0x04005676 RID: 22134
		public Label.DistributionPlatform distribution_platform;

		// Token: 0x04005677 RID: 22135
		public string id;

		// Token: 0x04005678 RID: 22136
		public string title;

		// Token: 0x04005679 RID: 22137
		public long version;

		// Token: 0x02001F8C RID: 8076
		public enum DistributionPlatform
		{
			// Token: 0x04008EEE RID: 36590
			Local,
			// Token: 0x04008EEF RID: 36591
			Steam,
			// Token: 0x04008EF0 RID: 36592
			Epic,
			// Token: 0x04008EF1 RID: 36593
			Rail,
			// Token: 0x04008EF2 RID: 36594
			Dev
		}
	}
}
