using System;
using System.Collections.Generic;
using Klei;
using ProcGen;

// Token: 0x02000836 RID: 2102
public static class TemplateCache
{
	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06003A74 RID: 14964 RVA: 0x00140118 File Offset: 0x0013E318
	// (set) Token: 0x06003A75 RID: 14965 RVA: 0x0014011F File Offset: 0x0013E31F
	public static bool Initted { get; private set; }

	// Token: 0x06003A76 RID: 14966 RVA: 0x00140127 File Offset: 0x0013E327
	public static void Init()
	{
		if (TemplateCache.Initted)
		{
			return;
		}
		TemplateCache.templates = new Dictionary<string, TemplateContainer>();
		TemplateCache.Initted = true;
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x00140141 File Offset: 0x0013E341
	public static void Clear()
	{
		TemplateCache.templates = null;
		TemplateCache.Initted = false;
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x00140150 File Offset: 0x0013E350
	public static string RewriteTemplatePath(string scopePath)
	{
		string dlcId;
		string str;
		SettingsCache.GetDlcIdAndPath(scopePath, out dlcId, out str);
		return SettingsCache.GetAbsoluteContentPath(dlcId, "templates/" + str);
	}

	// Token: 0x06003A79 RID: 14969 RVA: 0x00140178 File Offset: 0x0013E378
	public static string RewriteTemplateYaml(string scopePath)
	{
		return TemplateCache.RewriteTemplatePath(scopePath) + ".yaml";
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x0014018C File Offset: 0x0013E38C
	public static TemplateContainer GetTemplate(string templatePath)
	{
		if (!TemplateCache.templates.ContainsKey(templatePath))
		{
			TemplateCache.templates.Add(templatePath, null);
		}
		if (TemplateCache.templates[templatePath] == null)
		{
			string text = TemplateCache.RewriteTemplateYaml(templatePath);
			TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(text, null, null);
			if (templateContainer == null)
			{
				Debug.LogWarning("Missing template [" + text + "]");
			}
			templateContainer.name = templatePath;
			TemplateCache.templates[templatePath] = templateContainer;
		}
		return TemplateCache.templates[templatePath];
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x00140205 File Offset: 0x0013E405
	public static bool TemplateExists(string templatePath)
	{
		return FileSystem.FileExists(TemplateCache.RewriteTemplateYaml(templatePath));
	}

	// Token: 0x0400232E RID: 9006
	private const string defaultAssetFolder = "bases";

	// Token: 0x0400232F RID: 9007
	private static Dictionary<string, TemplateContainer> templates;
}
