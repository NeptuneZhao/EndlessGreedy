using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000837 RID: 2103
[Serializable]
public class TemplateContainer
{
	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06003A7D RID: 14973 RVA: 0x0014021A File Offset: 0x0013E41A
	// (set) Token: 0x06003A7E RID: 14974 RVA: 0x00140222 File Offset: 0x0013E422
	public string name { get; set; }

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06003A7F RID: 14975 RVA: 0x0014022B File Offset: 0x0013E42B
	// (set) Token: 0x06003A80 RID: 14976 RVA: 0x00140233 File Offset: 0x0013E433
	public int priority { get; set; }

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06003A81 RID: 14977 RVA: 0x0014023C File Offset: 0x0013E43C
	// (set) Token: 0x06003A82 RID: 14978 RVA: 0x00140244 File Offset: 0x0013E444
	public TemplateContainer.Info info { get; set; }

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06003A83 RID: 14979 RVA: 0x0014024D File Offset: 0x0013E44D
	// (set) Token: 0x06003A84 RID: 14980 RVA: 0x00140255 File Offset: 0x0013E455
	public List<Cell> cells { get; set; }

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06003A85 RID: 14981 RVA: 0x0014025E File Offset: 0x0013E45E
	// (set) Token: 0x06003A86 RID: 14982 RVA: 0x00140266 File Offset: 0x0013E466
	public List<Prefab> buildings { get; set; }

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06003A87 RID: 14983 RVA: 0x0014026F File Offset: 0x0013E46F
	// (set) Token: 0x06003A88 RID: 14984 RVA: 0x00140277 File Offset: 0x0013E477
	public List<Prefab> pickupables { get; set; }

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06003A89 RID: 14985 RVA: 0x00140280 File Offset: 0x0013E480
	// (set) Token: 0x06003A8A RID: 14986 RVA: 0x00140288 File Offset: 0x0013E488
	public List<Prefab> elementalOres { get; set; }

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06003A8B RID: 14987 RVA: 0x00140291 File Offset: 0x0013E491
	// (set) Token: 0x06003A8C RID: 14988 RVA: 0x00140299 File Offset: 0x0013E499
	public List<Prefab> otherEntities { get; set; }

	// Token: 0x06003A8D RID: 14989 RVA: 0x001402A4 File Offset: 0x0013E4A4
	public void Init(List<Cell> _cells, List<Prefab> _buildings, List<Prefab> _pickupables, List<Prefab> _elementalOres, List<Prefab> _otherEntities)
	{
		if (_cells != null && _cells.Count > 0)
		{
			this.cells = _cells;
		}
		if (_buildings != null && _buildings.Count > 0)
		{
			this.buildings = _buildings;
		}
		if (_pickupables != null && _pickupables.Count > 0)
		{
			this.pickupables = _pickupables;
		}
		if (_elementalOres != null && _elementalOres.Count > 0)
		{
			this.elementalOres = _elementalOres;
		}
		if (_otherEntities != null && _otherEntities.Count > 0)
		{
			this.otherEntities = _otherEntities;
		}
		this.info = new TemplateContainer.Info();
		this.RefreshInfo();
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x00140327 File Offset: 0x0013E527
	public RectInt GetTemplateBounds(int padding = 0)
	{
		return this.GetTemplateBounds(Vector2I.zero, padding);
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00140335 File Offset: 0x0013E535
	public RectInt GetTemplateBounds(Vector2 position, int padding = 0)
	{
		return this.GetTemplateBounds(new Vector2I((int)position.x, (int)position.y), padding);
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00140354 File Offset: 0x0013E554
	public RectInt GetTemplateBounds(Vector2I position, int padding = 0)
	{
		if ((this.info.min - new Vector2f(0, 0)).sqrMagnitude <= 1E-06f)
		{
			this.RefreshInfo();
		}
		return this.info.GetBounds(position, padding);
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0014039C File Offset: 0x0013E59C
	public void RefreshInfo()
	{
		if (this.cells == null)
		{
			return;
		}
		int num = 1;
		int num2 = -1;
		int num3 = 1;
		int num4 = -1;
		foreach (Cell cell in this.cells)
		{
			if (cell.location_x < num)
			{
				num = cell.location_x;
			}
			if (cell.location_x > num2)
			{
				num2 = cell.location_x;
			}
			if (cell.location_y < num3)
			{
				num3 = cell.location_y;
			}
			if (cell.location_y > num4)
			{
				num4 = cell.location_y;
			}
		}
		this.info.size = new Vector2((float)(1 + (num2 - num)), (float)(1 + (num4 - num3)));
		this.info.min = new Vector2((float)num, (float)num3);
		this.info.area = this.cells.Count;
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x00140494 File Offset: 0x0013E694
	public void SaveToYaml(string save_name)
	{
		string text = TemplateCache.RewriteTemplatePath(save_name);
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(text));
		}
		YamlIO.Save<TemplateContainer>(this, text + ".yaml", null);
	}

	// Token: 0x02001756 RID: 5974
	[Serializable]
	public class Info
	{
		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06009558 RID: 38232 RVA: 0x0035F6D2 File Offset: 0x0035D8D2
		// (set) Token: 0x06009559 RID: 38233 RVA: 0x0035F6DA File Offset: 0x0035D8DA
		public Vector2f size { get; set; }

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x0600955A RID: 38234 RVA: 0x0035F6E3 File Offset: 0x0035D8E3
		// (set) Token: 0x0600955B RID: 38235 RVA: 0x0035F6EB File Offset: 0x0035D8EB
		public Vector2f min { get; set; }

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x0600955C RID: 38236 RVA: 0x0035F6F4 File Offset: 0x0035D8F4
		// (set) Token: 0x0600955D RID: 38237 RVA: 0x0035F6FC File Offset: 0x0035D8FC
		public int area { get; set; }

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x0600955E RID: 38238 RVA: 0x0035F705 File Offset: 0x0035D905
		// (set) Token: 0x0600955F RID: 38239 RVA: 0x0035F70D File Offset: 0x0035D90D
		public Tag[] tags { get; set; }

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06009560 RID: 38240 RVA: 0x0035F716 File Offset: 0x0035D916
		// (set) Token: 0x06009561 RID: 38241 RVA: 0x0035F71E File Offset: 0x0035D91E
		public Tag[] discover_tags { get; set; }

		// Token: 0x06009562 RID: 38242 RVA: 0x0035F728 File Offset: 0x0035D928
		public RectInt GetBounds(Vector2I position, int padding)
		{
			return new RectInt(position.x + (int)this.min.x - padding, position.y + (int)this.min.y - padding, (int)this.size.x + padding * 2, (int)this.size.y + padding * 2);
		}
	}
}
