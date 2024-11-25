using System;

namespace Klei.CustomSettings
{
	// Token: 0x02000F31 RID: 3889
	public class SettingLevel
	{
		// Token: 0x06007794 RID: 30612 RVA: 0x002F6C75 File Offset: 0x002F4E75
		public SettingLevel(string id, string label, string tooltip, long coordinate_value = 0L, object userdata = null)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.userdata = userdata;
			this.coordinate_value = coordinate_value;
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06007795 RID: 30613 RVA: 0x002F6CA2 File Offset: 0x002F4EA2
		// (set) Token: 0x06007796 RID: 30614 RVA: 0x002F6CAA File Offset: 0x002F4EAA
		public string id { get; private set; }

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06007797 RID: 30615 RVA: 0x002F6CB3 File Offset: 0x002F4EB3
		// (set) Token: 0x06007798 RID: 30616 RVA: 0x002F6CBB File Offset: 0x002F4EBB
		public string tooltip { get; private set; }

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06007799 RID: 30617 RVA: 0x002F6CC4 File Offset: 0x002F4EC4
		// (set) Token: 0x0600779A RID: 30618 RVA: 0x002F6CCC File Offset: 0x002F4ECC
		public string label { get; private set; }

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600779B RID: 30619 RVA: 0x002F6CD5 File Offset: 0x002F4ED5
		// (set) Token: 0x0600779C RID: 30620 RVA: 0x002F6CDD File Offset: 0x002F4EDD
		public object userdata { get; private set; }

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600779D RID: 30621 RVA: 0x002F6CE6 File Offset: 0x002F4EE6
		// (set) Token: 0x0600779E RID: 30622 RVA: 0x002F6CEE File Offset: 0x002F4EEE
		public long coordinate_value { get; private set; }
	}
}
