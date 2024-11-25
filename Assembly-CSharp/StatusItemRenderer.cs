using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class StatusItemRenderer
{
	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06002363 RID: 9059 RVA: 0x000C5884 File Offset: 0x000C3A84
	// (set) Token: 0x06002364 RID: 9060 RVA: 0x000C588C File Offset: 0x000C3A8C
	public int layer { get; private set; }

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06002365 RID: 9061 RVA: 0x000C5895 File Offset: 0x000C3A95
	// (set) Token: 0x06002366 RID: 9062 RVA: 0x000C589D File Offset: 0x000C3A9D
	public int selectedHandle { get; private set; }

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06002367 RID: 9063 RVA: 0x000C58A6 File Offset: 0x000C3AA6
	// (set) Token: 0x06002368 RID: 9064 RVA: 0x000C58AE File Offset: 0x000C3AAE
	public int highlightHandle { get; private set; }

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06002369 RID: 9065 RVA: 0x000C58B7 File Offset: 0x000C3AB7
	// (set) Token: 0x0600236A RID: 9066 RVA: 0x000C58BF File Offset: 0x000C3ABF
	public Color32 backgroundColor { get; private set; }

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x0600236B RID: 9067 RVA: 0x000C58C8 File Offset: 0x000C3AC8
	// (set) Token: 0x0600236C RID: 9068 RVA: 0x000C58D0 File Offset: 0x000C3AD0
	public Color32 selectedColor { get; private set; }

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x0600236D RID: 9069 RVA: 0x000C58D9 File Offset: 0x000C3AD9
	// (set) Token: 0x0600236E RID: 9070 RVA: 0x000C58E1 File Offset: 0x000C3AE1
	public Color32 neutralColor { get; private set; }

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x0600236F RID: 9071 RVA: 0x000C58EA File Offset: 0x000C3AEA
	// (set) Token: 0x06002370 RID: 9072 RVA: 0x000C58F2 File Offset: 0x000C3AF2
	public Sprite arrowSprite { get; private set; }

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06002371 RID: 9073 RVA: 0x000C58FB File Offset: 0x000C3AFB
	// (set) Token: 0x06002372 RID: 9074 RVA: 0x000C5903 File Offset: 0x000C3B03
	public Sprite backgroundSprite { get; private set; }

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06002373 RID: 9075 RVA: 0x000C590C File Offset: 0x000C3B0C
	// (set) Token: 0x06002374 RID: 9076 RVA: 0x000C5914 File Offset: 0x000C3B14
	public float scale { get; private set; }

	// Token: 0x06002375 RID: 9077 RVA: 0x000C5920 File Offset: 0x000C3B20
	public StatusItemRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		this.entries = new StatusItemRenderer.Entry[100];
		this.shader = Shader.Find("Klei/StatusItem");
		for (int i = 0; i < this.entries.Length; i++)
		{
			StatusItemRenderer.Entry entry = default(StatusItemRenderer.Entry);
			entry.Init(this.shader);
			this.entries[i] = entry;
		}
		this.backgroundColor = new Color32(244, 74, 71, byte.MaxValue);
		this.selectedColor = new Color32(225, 181, 180, byte.MaxValue);
		this.neutralColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		this.arrowSprite = Assets.GetSprite("StatusBubbleTop");
		this.backgroundSprite = Assets.GetSprite("StatusBubble");
		this.scale = 1f;
		Game.Instance.Subscribe(2095258329, new Action<object>(this.OnHighlightObject));
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x000C5A54 File Offset: 0x000C3C54
	public int GetIdx(Transform transform)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			int num2 = this.entryCount;
			this.entryCount = num2 + 1;
			num = num2;
			this.handleTable[instanceID] = num;
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.handle = instanceID;
			entry.transform = transform;
			entry.buildingPos = transform.GetPosition();
			entry.building = transform.GetComponent<Building>();
			entry.isBuilding = (entry.building != null);
			entry.selectable = transform.GetComponent<KSelectable>();
			this.entries[num] = entry;
		}
		return num;
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000C5B04 File Offset: 0x000C3D04
	public void Add(Transform transform, StatusItem status_item)
	{
		if (this.entryCount == this.entries.Length)
		{
			StatusItemRenderer.Entry[] array = new StatusItemRenderer.Entry[this.entries.Length * 2];
			for (int i = 0; i < this.entries.Length; i++)
			{
				array[i] = this.entries[i];
			}
			for (int j = this.entries.Length; j < array.Length; j++)
			{
				array[j].Init(this.shader);
			}
			this.entries = array;
		}
		int idx = this.GetIdx(transform);
		StatusItemRenderer.Entry entry = this.entries[idx];
		entry.Add(status_item);
		this.entries[idx] = entry;
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000C5BB4 File Offset: 0x000C3DB4
	public void Remove(Transform transform, StatusItem status_item)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			return;
		}
		StatusItemRenderer.Entry entry = this.entries[num];
		if (entry.statusItems.Count == 0)
		{
			return;
		}
		entry.Remove(status_item);
		this.entries[num] = entry;
		if (entry.statusItems.Count == 0)
		{
			this.ClearIdx(num);
		}
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x000C5C20 File Offset: 0x000C3E20
	private void ClearIdx(int idx)
	{
		StatusItemRenderer.Entry entry = this.entries[idx];
		this.handleTable.Remove(entry.handle);
		if (idx != this.entryCount - 1)
		{
			entry.Replace(this.entries[this.entryCount - 1]);
			this.entries[idx] = entry;
			this.handleTable[entry.handle] = idx;
		}
		entry = this.entries[this.entryCount - 1];
		entry.Clear();
		this.entries[this.entryCount - 1] = entry;
		this.entryCount--;
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x000C5CCD File Offset: 0x000C3ECD
	private HashedString GetMode()
	{
		if (OverlayScreen.Instance != null)
		{
			return OverlayScreen.Instance.mode;
		}
		return OverlayModes.None.ID;
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x000C5CEC File Offset: 0x000C3EEC
	public void MarkAllDirty()
	{
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].MarkDirty();
		}
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x000C5D1C File Offset: 0x000C3F1C
	public void RenderEveryTick()
	{
		if (DebugHandler.HideUI)
		{
			return;
		}
		this.scale = 1f + Mathf.Sin(Time.unscaledTime * 8f) * 0.1f;
		Shader.SetGlobalVector("_StatusItemParameters", new Vector4(this.scale, 0f, 0f, 0f));
		Vector3 camera_tr = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 camera_bl = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		this.visibleEntries.Clear();
		Camera worldCamera = GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera;
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].Render(this, camera_bl, camera_tr, this.GetMode(), worldCamera);
		}
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000C5E20 File Offset: 0x000C4020
	public void GetIntersections(Vector2 pos, List<InterfaceTool.Intersection> intersections)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, intersections, this.scale);
		}
	}

	// Token: 0x0600237E RID: 9086 RVA: 0x000C5E7C File Offset: 0x000C407C
	public void GetIntersections(Vector2 pos, List<KSelectable> selectables)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, selectables, this.scale);
		}
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000C5ED8 File Offset: 0x000C40D8
	public void SetOffset(Transform transform, Vector3 offset)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(transform.GetInstanceID(), out num))
		{
			this.entries[num].offset = offset;
		}
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000C5F10 File Offset: 0x000C4110
	private void OnSelectObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.selectedHandle, out num))
		{
			this.entries[num].MarkDirty();
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.selectedHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.selectedHandle, out num))
			{
				this.entries[num].MarkDirty();
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x000C5F94 File Offset: 0x000C4194
	private void OnHighlightObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.highlightHandle, out num))
		{
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.MarkDirty();
			this.entries[num] = entry;
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.highlightHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.highlightHandle, out num))
			{
				StatusItemRenderer.Entry entry2 = this.entries[num];
				entry2.MarkDirty();
				this.entries[num] = entry2;
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000C6038 File Offset: 0x000C4238
	public void Destroy()
	{
		Game.Instance.Unsubscribe(-1503271301, new Action<object>(this.OnSelectObject));
		Game.Instance.Unsubscribe(-1201923725, new Action<object>(this.OnHighlightObject));
		foreach (StatusItemRenderer.Entry entry in this.entries)
		{
			entry.Clear();
			entry.FreeResources();
		}
	}

	// Token: 0x04001424 RID: 5156
	private StatusItemRenderer.Entry[] entries;

	// Token: 0x04001425 RID: 5157
	private int entryCount;

	// Token: 0x04001426 RID: 5158
	private Dictionary<int, int> handleTable = new Dictionary<int, int>();

	// Token: 0x04001430 RID: 5168
	private Shader shader;

	// Token: 0x04001431 RID: 5169
	public List<StatusItemRenderer.Entry> visibleEntries = new List<StatusItemRenderer.Entry>();

	// Token: 0x020013BB RID: 5051
	public struct Entry
	{
		// Token: 0x06008823 RID: 34851 RVA: 0x0032D823 File Offset: 0x0032BA23
		public void Init(Shader shader)
		{
			this.statusItems = new List<StatusItem>();
			this.mesh = new Mesh();
			this.mesh.name = "StatusItemRenderer";
			this.dirty = true;
			this.material = new Material(shader);
		}

		// Token: 0x06008824 RID: 34852 RVA: 0x0032D860 File Offset: 0x0032BA60
		public void Render(StatusItemRenderer renderer, Vector3 camera_bl, Vector3 camera_tr, HashedString overlay, Camera camera)
		{
			if (this.transform == null)
			{
				string text = "Error cleaning up status items:";
				foreach (StatusItem statusItem in this.statusItems)
				{
					text += statusItem.Id;
				}
				global::Debug.LogWarning(text);
				return;
			}
			Vector3 vector = this.isBuilding ? this.buildingPos : this.transform.GetPosition();
			if (this.isBuilding)
			{
				vector.x += (float)((this.building.Def.WidthInCells - 1) % 2) / 2f;
			}
			if (vector.x < camera_bl.x || vector.x > camera_tr.x || vector.y < camera_bl.y || vector.y > camera_tr.y)
			{
				return;
			}
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && (!Grid.IsVisible(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId))
			{
				return;
			}
			if (!this.selectable.IsSelectable)
			{
				return;
			}
			renderer.visibleEntries.Add(this);
			if (this.dirty)
			{
				int num2 = 0;
				StatusItemRenderer.Entry.spritesListedToRender.Clear();
				StatusItemRenderer.Entry.statusItemsToRender_Index.Clear();
				int num3 = -1;
				foreach (StatusItem statusItem2 in this.statusItems)
				{
					num3++;
					if (statusItem2.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem2.render_overlay != overlay))
					{
						Sprite sprite = statusItem2.sprite.sprite;
						if (!statusItem2.unique)
						{
							if (StatusItemRenderer.Entry.spritesListedToRender.Contains(sprite) || StatusItemRenderer.Entry.spritesListedToRender.Count >= StatusItemRenderer.Entry.spritesListedToRender.Capacity)
							{
								continue;
							}
							StatusItemRenderer.Entry.spritesListedToRender.Add(sprite);
						}
						StatusItemRenderer.Entry.statusItemsToRender_Index.Add(num3);
						num2++;
					}
				}
				this.hasVisibleStatusItems = (num2 != 0);
				StatusItemRenderer.Entry.MeshBuilder meshBuilder = new StatusItemRenderer.Entry.MeshBuilder(num2 + 6, this.material);
				float num4 = 0.25f;
				float z = -5f;
				Vector2 b = new Vector2(0.05f, -0.05f);
				float num5 = 0.02f;
				Color32 c = new Color32(0, 0, 0, byte.MaxValue);
				Color32 c2 = new Color32(0, 0, 0, 75);
				Color32 c3 = renderer.neutralColor;
				if (renderer.selectedHandle == this.handle || renderer.highlightHandle == this.handle)
				{
					c3 = renderer.selectedColor;
				}
				else
				{
					for (int i = 0; i < this.statusItems.Count; i++)
					{
						if (this.statusItems[i].notificationType != NotificationType.Neutral)
						{
							c3 = renderer.backgroundColor;
							break;
						}
					}
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f) + b, new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f) + b, new Vector2(num4 * (float)num2, num4), z, renderer.backgroundSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num4 * (float)num2 + num5, num4 + num5), z, renderer.backgroundSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num4 * (float)num2, num4), z, renderer.backgroundSprite, c3);
				for (int j = 0; j < StatusItemRenderer.Entry.statusItemsToRender_Index.Count; j++)
				{
					StatusItem statusItem3 = this.statusItems[StatusItemRenderer.Entry.statusItemsToRender_Index[j]];
					float x = (float)j * num4 * 2f - num4 * (float)(num2 - 1);
					if (statusItem3.sprite == null)
					{
						DebugUtil.DevLogError(string.Concat(new string[]
						{
							"Status Item ",
							statusItem3.Id,
							" has null sprite for icon '",
							statusItem3.iconName,
							"', you need to run Collect Sprites or manually add the sprite to the TintedSprites list in the GameAssets prefab."
						}));
						statusItem3.iconName = "status_item_exclamation";
						statusItem3.sprite = Assets.GetTintedSprite("status_item_exclamation");
					}
					Sprite sprite2 = statusItem3.sprite.sprite;
					meshBuilder.AddQuad(new Vector2(x, 0f), new Vector2(num4, num4), z, sprite2, c);
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f + num5), new Vector2(0.05f + num5, 0.05f + num5), z, renderer.arrowSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0.29f), new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c3);
				meshBuilder.End(this.mesh);
				this.dirty = false;
			}
			if (this.hasVisibleStatusItems && GameScreenManager.Instance != null)
			{
				Graphics.DrawMesh(this.mesh, vector + this.offset, Quaternion.identity, this.material, renderer.layer, camera, 0, null, false, false);
			}
		}

		// Token: 0x06008825 RID: 34853 RVA: 0x0032DDE8 File Offset: 0x0032BFE8
		public void Add(StatusItem status_item)
		{
			this.statusItems.Add(status_item);
			this.dirty = true;
		}

		// Token: 0x06008826 RID: 34854 RVA: 0x0032DDFD File Offset: 0x0032BFFD
		public void Remove(StatusItem status_item)
		{
			this.statusItems.Remove(status_item);
			this.dirty = true;
		}

		// Token: 0x06008827 RID: 34855 RVA: 0x0032DE14 File Offset: 0x0032C014
		public void Replace(StatusItemRenderer.Entry entry)
		{
			this.handle = entry.handle;
			this.transform = entry.transform;
			this.building = this.transform.GetComponent<Building>();
			this.buildingPos = this.transform.GetPosition();
			this.isBuilding = (this.building != null);
			this.selectable = this.transform.GetComponent<KSelectable>();
			this.offset = entry.offset;
			this.dirty = true;
			this.statusItems.Clear();
			this.statusItems.AddRange(entry.statusItems);
		}

		// Token: 0x06008828 RID: 34856 RVA: 0x0032DEB0 File Offset: 0x0032C0B0
		private bool Intersects(Vector2 pos, float scale)
		{
			if (this.transform == null)
			{
				return false;
			}
			Bounds bounds = this.mesh.bounds;
			Vector3 vector = this.buildingPos + this.offset + bounds.center;
			Vector2 a = new Vector2(vector.x, vector.y);
			Vector3 size = bounds.size;
			Vector2 b = new Vector2(size.x * scale * 0.5f, size.y * scale * 0.5f);
			Vector2 vector2 = a - b;
			Vector2 vector3 = a + b;
			return pos.x >= vector2.x && pos.x <= vector3.x && pos.y >= vector2.y && pos.y <= vector3.y;
		}

		// Token: 0x06008829 RID: 34857 RVA: 0x0032DF88 File Offset: 0x0032C188
		public void GetIntersection(Vector2 pos, List<InterfaceTool.Intersection> intersections, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable)
			{
				intersections.Add(new InterfaceTool.Intersection
				{
					component = this.selectable,
					distance = -100f
				});
			}
		}

		// Token: 0x0600882A RID: 34858 RVA: 0x0032DFD4 File Offset: 0x0032C1D4
		public void GetIntersection(Vector2 pos, List<KSelectable> selectables, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable && !selectables.Contains(this.selectable))
			{
				selectables.Add(this.selectable);
			}
		}

		// Token: 0x0600882B RID: 34859 RVA: 0x0032E007 File Offset: 0x0032C207
		public void Clear()
		{
			this.statusItems.Clear();
			this.offset = Vector3.zero;
			this.dirty = false;
		}

		// Token: 0x0600882C RID: 34860 RVA: 0x0032E026 File Offset: 0x0032C226
		public void FreeResources()
		{
			if (this.mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mesh);
				this.mesh = null;
			}
			if (this.material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
			}
		}

		// Token: 0x0600882D RID: 34861 RVA: 0x0032E061 File Offset: 0x0032C261
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x040067B0 RID: 26544
		public int handle;

		// Token: 0x040067B1 RID: 26545
		public Transform transform;

		// Token: 0x040067B2 RID: 26546
		public Building building;

		// Token: 0x040067B3 RID: 26547
		public Vector3 buildingPos;

		// Token: 0x040067B4 RID: 26548
		public KSelectable selectable;

		// Token: 0x040067B5 RID: 26549
		public List<StatusItem> statusItems;

		// Token: 0x040067B6 RID: 26550
		public Mesh mesh;

		// Token: 0x040067B7 RID: 26551
		public bool dirty;

		// Token: 0x040067B8 RID: 26552
		public int layer;

		// Token: 0x040067B9 RID: 26553
		public Material material;

		// Token: 0x040067BA RID: 26554
		public Vector3 offset;

		// Token: 0x040067BB RID: 26555
		public bool hasVisibleStatusItems;

		// Token: 0x040067BC RID: 26556
		public bool isBuilding;

		// Token: 0x040067BD RID: 26557
		private const int STATUS_ICONS_LIMIT = 12;

		// Token: 0x040067BE RID: 26558
		public static List<Sprite> spritesListedToRender = new List<Sprite>(12);

		// Token: 0x040067BF RID: 26559
		public static List<int> statusItemsToRender_Index = new List<int>(12);

		// Token: 0x020024A5 RID: 9381
		private struct MeshBuilder
		{
			// Token: 0x0600BA89 RID: 47753 RVA: 0x003D3BB4 File Offset: 0x003D1DB4
			public MeshBuilder(int quad_count, Material material)
			{
				this.vertices = new Vector3[4 * quad_count];
				this.uvs = new Vector2[4 * quad_count];
				this.uv2s = new Vector2[4 * quad_count];
				this.colors = new Color32[4 * quad_count];
				this.triangles = new int[6 * quad_count];
				this.material = material;
				this.quadIdx = 0;
			}

			// Token: 0x0600BA8A RID: 47754 RVA: 0x003D3C18 File Offset: 0x003D1E18
			public void AddQuad(Vector2 center, Vector2 half_size, float z, Sprite sprite, Color color)
			{
				if (this.quadIdx == StatusItemRenderer.Entry.MeshBuilder.textureIds.Length)
				{
					return;
				}
				Rect rect = sprite.rect;
				Rect textureRect = sprite.textureRect;
				float num = textureRect.width / rect.width;
				float num2 = textureRect.height / rect.height;
				int num3 = 4 * this.quadIdx;
				this.vertices[num3] = new Vector3((center.x - half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[1 + num3] = new Vector3((center.x - half_size.x) * num, (center.y + half_size.y) * num2, z);
				this.vertices[2 + num3] = new Vector3((center.x + half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[3 + num3] = new Vector3((center.x + half_size.x) * num, (center.y + half_size.y) * num2, z);
				float num4 = textureRect.x / (float)sprite.texture.width;
				float num5 = textureRect.y / (float)sprite.texture.height;
				float num6 = textureRect.width / (float)sprite.texture.width;
				float num7 = textureRect.height / (float)sprite.texture.height;
				this.uvs[num3] = new Vector2(num4, num5);
				this.uvs[1 + num3] = new Vector2(num4, num5 + num7);
				this.uvs[2 + num3] = new Vector2(num4 + num6, num5);
				this.uvs[3 + num3] = new Vector2(num4 + num6, num5 + num7);
				this.colors[num3] = color;
				this.colors[1 + num3] = color;
				this.colors[2 + num3] = color;
				this.colors[3 + num3] = color;
				float x = (float)this.quadIdx + 0.5f;
				this.uv2s[num3] = new Vector2(x, 0f);
				this.uv2s[1 + num3] = new Vector2(x, 0f);
				this.uv2s[2 + num3] = new Vector2(x, 0f);
				this.uv2s[3 + num3] = new Vector2(x, 0f);
				int num8 = 6 * this.quadIdx;
				this.triangles[num8] = num3;
				this.triangles[1 + num8] = num3 + 1;
				this.triangles[2 + num8] = num3 + 2;
				this.triangles[3 + num8] = num3 + 2;
				this.triangles[4 + num8] = num3 + 1;
				this.triangles[5 + num8] = num3 + 3;
				this.material.SetTexture(StatusItemRenderer.Entry.MeshBuilder.textureIds[this.quadIdx], sprite.texture);
				this.quadIdx++;
			}

			// Token: 0x0600BA8B RID: 47755 RVA: 0x003D3F64 File Offset: 0x003D2164
			public void End(Mesh mesh)
			{
				mesh.Clear();
				mesh.vertices = this.vertices;
				mesh.uv = this.uvs;
				mesh.uv2 = this.uv2s;
				mesh.colors32 = this.colors;
				mesh.SetTriangles(this.triangles, 0);
				mesh.RecalculateBounds();
			}

			// Token: 0x0400A271 RID: 41585
			private Vector3[] vertices;

			// Token: 0x0400A272 RID: 41586
			private Vector2[] uvs;

			// Token: 0x0400A273 RID: 41587
			private Vector2[] uv2s;

			// Token: 0x0400A274 RID: 41588
			private int[] triangles;

			// Token: 0x0400A275 RID: 41589
			private Color32[] colors;

			// Token: 0x0400A276 RID: 41590
			private int quadIdx;

			// Token: 0x0400A277 RID: 41591
			private Material material;

			// Token: 0x0400A278 RID: 41592
			private static int[] textureIds = new int[]
			{
				Shader.PropertyToID("_Tex0"),
				Shader.PropertyToID("_Tex1"),
				Shader.PropertyToID("_Tex2"),
				Shader.PropertyToID("_Tex3"),
				Shader.PropertyToID("_Tex4"),
				Shader.PropertyToID("_Tex5"),
				Shader.PropertyToID("_Tex6"),
				Shader.PropertyToID("_Tex7"),
				Shader.PropertyToID("_Tex8"),
				Shader.PropertyToID("_Tex9"),
				Shader.PropertyToID("_Tex10")
			};
		}
	}
}
