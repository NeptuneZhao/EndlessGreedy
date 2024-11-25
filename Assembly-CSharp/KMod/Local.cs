using System;
using System.IO;
using Klei;
using STRINGS;

namespace KMod
{
	// Token: 0x02000EDB RID: 3803
	public class Local : IDistributionPlatform
	{
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x0600765D RID: 30301 RVA: 0x002E809A File Offset: 0x002E629A
		// (set) Token: 0x0600765E RID: 30302 RVA: 0x002E80A2 File Offset: 0x002E62A2
		public string folder { get; private set; }

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x0600765F RID: 30303 RVA: 0x002E80AB File Offset: 0x002E62AB
		// (set) Token: 0x06007660 RID: 30304 RVA: 0x002E80B3 File Offset: 0x002E62B3
		public Label.DistributionPlatform distribution_platform { get; private set; }

		// Token: 0x06007661 RID: 30305 RVA: 0x002E80BC File Offset: 0x002E62BC
		public string GetDirectory()
		{
			return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.folder));
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x002E80D4 File Offset: 0x002E62D4
		private void Subscribe(string directoryName, long timestamp, IFileSource file_source, bool isDevMod)
		{
			Label label = new Label
			{
				id = directoryName,
				distribution_platform = this.distribution_platform,
				version = (long)directoryName.GetHashCode(),
				title = directoryName
			};
			KModHeader header = KModUtil.GetHeader(file_source, label.defaultStaticID, directoryName, directoryName, isDevMod);
			label.title = header.title;
			Mod mod = new Mod(label, header.staticID, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, delegate()
			{
				App.OpenWebURL("file://" + file_source.GetRoot());
			});
			if (file_source.GetType() == typeof(Directory))
			{
				mod.status = Mod.Status.Installed;
			}
			Global.Instance.modManager.Subscribe(mod, this);
		}

		// Token: 0x06007663 RID: 30307 RVA: 0x002E81A8 File Offset: 0x002E63A8
		public Local(string folder, Label.DistributionPlatform distribution_platform, bool isDevFolder)
		{
			this.folder = folder;
			this.distribution_platform = distribution_platform;
			DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string name = directoryInfo2.Name;
				this.Subscribe(name, directoryInfo2.LastWriteTime.ToFileTime(), new Directory(directoryInfo2.FullName), isDevFolder);
			}
		}
	}
}
