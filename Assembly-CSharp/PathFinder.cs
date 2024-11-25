using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x0200048B RID: 1163
public class PathFinder
{
	// Token: 0x06001922 RID: 6434 RVA: 0x00086C28 File Offset: 0x00084E28
	public static void Initialize()
	{
		NavType[] array = new NavType[11];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NavType)i;
		}
		PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, array);
		for (int j = 0; j < Grid.CellCount; j++)
		{
			if (Grid.Visible[j] > 0 || Grid.Spawnable[j] > 0)
			{
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				GameUtil.FloodFillConditional(j, PathFinder.allowPathfindingFloodFillCb, pooledList, null);
				Grid.AllowPathfinding[j] = true;
				pooledList.Recycle();
			}
		}
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(PathFinder.OnReveal));
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x00086CCF File Offset: 0x00084ECF
	private static void OnReveal(int cell)
	{
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x00086CD1 File Offset: 0x00084ED1
	public static void UpdatePath(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x00086CE0 File Offset: 0x00084EE0
	public static bool ValidatePath(NavGrid nav_grid, PathFinderAbilities abilities, ref PathFinder.Path path)
	{
		if (!path.IsValid())
		{
			return false;
		}
		for (int i = 0; i < path.nodes.Count; i++)
		{
			PathFinder.Path.Node node = path.nodes[i];
			if (i < path.nodes.Count - 1)
			{
				PathFinder.Path.Node node2 = path.nodes[i + 1];
				int num = node.cell * nav_grid.maxLinksPerCell;
				bool flag = false;
				NavGrid.Link link = nav_grid.Links[num];
				while (link.link != PathFinder.InvalidHandle)
				{
					if (link.link == node2.cell && node2.navType == link.endNavType && node.navType == link.startNavType)
					{
						PathFinder.PotentialPath potentialPath = new PathFinder.PotentialPath(node.cell, node.navType, PathFinder.PotentialPath.Flags.None);
						flag = abilities.TraversePath(ref potentialPath, node.cell, node.navType, 0, (int)link.transitionId, false);
						if (flag)
						{
							break;
						}
					}
					num++;
					link = nav_grid.Links[num];
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x00086DF4 File Offset: 0x00084FF4
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query)
	{
		int invalidCell = PathFinder.InvalidCell;
		NavType nav_type = NavType.NumNavTypes;
		query.ClearResult();
		if (!Grid.IsValidCell(potential_path.cell))
		{
			return;
		}
		PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref nav_type);
		if (invalidCell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, nav_type, out flag);
			query.SetResult(invalidCell, cell.cost, nav_type);
		}
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x00086E58 File Offset: 0x00085058
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query);
		if (query.GetResultCell() != PathFinder.InvalidCell)
		{
			PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
			return;
		}
		path.Clear();
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x00086E8C File Offset: 0x0008508C
	private static void BuildResultPath(int path_cell, NavType path_nav_type, ref PathFinder.Path path)
	{
		if (path_cell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out flag);
			path.Clear();
			path.cost = cell.cost;
			while (path_cell != PathFinder.InvalidCell)
			{
				path.AddNode(new PathFinder.Path.Node
				{
					cell = path_cell,
					navType = cell.navType,
					transitionId = cell.transitionId
				});
				path_cell = cell.parent;
				if (path_cell != PathFinder.InvalidCell)
				{
					cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out flag);
				}
			}
			if (path.nodes != null)
			{
				for (int i = 0; i < path.nodes.Count / 2; i++)
				{
					PathFinder.Path.Node value = path.nodes[i];
					path.nodes[i] = path.nodes[path.nodes.Count - i - 1];
					path.nodes[path.nodes.Count - i - 1] = value;
				}
			}
		}
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x00086F98 File Offset: 0x00085198
	private static void FindPaths(NavGrid nav_grid, ref PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, PathFinder.PotentialList potentials, ref int result_cell, ref NavType result_nav_type)
	{
		potentials.Clear();
		PathFinder.PathGrid.ResetUpdate();
		PathFinder.PathGrid.BeginUpdate(potential_path.cell, false);
		bool flag;
		PathFinder.Cell cell = PathFinder.PathGrid.GetCell(potential_path, out flag);
		PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, 0, potentials, PathFinder.PathGrid, ref cell);
		int num = int.MaxValue;
		while (potentials.Count > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
			cell = PathFinder.PathGrid.GetCell(keyValuePair.Value, out flag);
			if (cell.cost == keyValuePair.Key)
			{
				if (cell.navType != NavType.Tube && query.IsMatch(keyValuePair.Value.cell, cell.parent, cell.cost) && cell.cost < num)
				{
					result_cell = keyValuePair.Value.cell;
					num = cell.cost;
					result_nav_type = cell.navType;
					break;
				}
				PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell.cost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell.parent, cell.parentNavType);
			}
		}
		PathFinder.PathGrid.EndUpdate(true);
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000870D2 File Offset: 0x000852D2
	public static void AddPotential(PathFinder.PotentialPath potential_path, int parent_cell, NavType parent_nav_type, int cost, byte transition_id, PathFinder.PotentialList potentials, PathGrid path_grid, ref PathFinder.Cell cell_data)
	{
		cell_data.cost = cost;
		cell_data.parent = parent_cell;
		cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
		cell_data.transitionId = transition_id;
		potentials.Add(cost, potential_path);
		path_grid.SetCell(potential_path, ref cell_data);
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0008710E File Offset: 0x0008530E
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void BeginDetailSample(string region_name)
	{
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00087110 File Offset: 0x00085310
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void EndDetailSample(string region_name)
	{
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x00087114 File Offset: 0x00085314
	public static bool IsSubmerged(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num = Grid.CellAbove(cell);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsLiquid) || (Grid.Element[cell].IsLiquid && Grid.IsValidCell(num) && Grid.Solid[num]);
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x00087170 File Offset: 0x00085370
	public static void AddPotentials(PathFinder.PotentialScratchPad potential_scratch_pad, PathFinder.PotentialPath potential, int cost, ref PathFinderAbilities abilities, PathFinderQuery query, int max_links_per_cell, NavGrid.Link[] links, PathFinder.PotentialList potentials, PathGrid path_grid, int parent_cell, NavType parent_nav_type)
	{
		if (!Grid.IsValidCell(potential.cell))
		{
			return;
		}
		int num = 0;
		NavGrid.Link[] linksWithCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
		int num2 = potential.cell * max_links_per_cell;
		NavGrid.Link link = links[num2];
		for (int link2 = link.link; link2 != PathFinder.InvalidHandle; link2 = link.link)
		{
			if (link.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link.startNavType))
			{
				linksWithCorrectNavType[num++] = link;
			}
			num2++;
			link = links[num2];
		}
		int num3 = 0;
		PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
		for (int i = 0; i < num; i++)
		{
			NavGrid.Link link3 = linksWithCorrectNavType[i];
			int link4 = link3.link;
			bool flag = false;
			PathFinder.Cell cell = path_grid.GetCell(link4, link3.endNavType, out flag);
			if (flag)
			{
				int num4 = cost + (int)link3.cost;
				bool flag2 = cell.cost == -1;
				bool flag3 = num4 < cell.cost;
				if (flag2 || flag3)
				{
					linksInCellRange[num3++] = new PathFinder.PotentialScratchPad.PathGridCellData
					{
						pathGridCell = cell,
						link = link3
					};
				}
			}
		}
		for (int j = 0; j < num3; j++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[j];
			int link5 = pathGridCellData.link.link;
			pathGridCellData.isSubmerged = PathFinder.IsSubmerged(link5);
			linksInCellRange[j] = pathGridCellData;
		}
		for (int k = 0; k < num3; k++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData2 = linksInCellRange[k];
			NavGrid.Link link6 = pathGridCellData2.link;
			int link7 = link6.link;
			PathFinder.Cell pathGridCell = pathGridCellData2.pathGridCell;
			int num5 = cost + (int)link6.cost;
			PathFinder.PotentialPath potentialPath = potential;
			potentialPath.cell = link7;
			potentialPath.navType = link6.endNavType;
			if (pathGridCellData2.isSubmerged)
			{
				int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potentialPath, link6);
				num5 += submergedPathCostPenalty;
			}
			PathFinder.PotentialPath.Flags flags = potentialPath.flags;
			bool flag4 = abilities.TraversePath(ref potentialPath, potential.cell, potential.navType, num5, (int)link6.transitionId, pathGridCellData2.isSubmerged);
			PathFinder.PotentialPath.Flags flags2 = potentialPath.flags;
			if (flag4)
			{
				PathFinder.AddPotential(potentialPath, potential.cell, potential.navType, num5, link6.transitionId, potentials, path_grid, ref pathGridCell);
			}
		}
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000873BB File Offset: 0x000855BB
	public static void DestroyStatics()
	{
		PathFinder.PathGrid.OnCleanUp();
		PathFinder.PathGrid = null;
		PathFinder.Temp.Potentials.Clear();
	}

	// Token: 0x04000E14 RID: 3604
	public static int InvalidHandle = -1;

	// Token: 0x04000E15 RID: 3605
	public static int InvalidIdx = -1;

	// Token: 0x04000E16 RID: 3606
	public static int InvalidCell = -1;

	// Token: 0x04000E17 RID: 3607
	public static PathGrid PathGrid;

	// Token: 0x04000E18 RID: 3608
	private static readonly Func<int, bool> allowPathfindingFloodFillCb = delegate(int cell)
	{
		if (Grid.Solid[cell])
		{
			return false;
		}
		if (Grid.AllowPathfinding[cell])
		{
			return false;
		}
		Grid.AllowPathfinding[cell] = true;
		return true;
	};

	// Token: 0x02001264 RID: 4708
	public struct Cell
	{
		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06008300 RID: 33536 RVA: 0x0031E29C File Offset: 0x0031C49C
		public NavType navType
		{
			get
			{
				return (NavType)(this.navTypes & 15);
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06008301 RID: 33537 RVA: 0x0031E2A8 File Offset: 0x0031C4A8
		public NavType parentNavType
		{
			get
			{
				return (NavType)(this.navTypes >> 4);
			}
		}

		// Token: 0x06008302 RID: 33538 RVA: 0x0031E2B4 File Offset: 0x0031C4B4
		public void SetNavTypes(NavType type, NavType parent_type)
		{
			this.navTypes = (byte)(type | parent_type << 4);
		}

		// Token: 0x04006346 RID: 25414
		public int cost;

		// Token: 0x04006347 RID: 25415
		public int parent;

		// Token: 0x04006348 RID: 25416
		public short queryId;

		// Token: 0x04006349 RID: 25417
		private byte navTypes;

		// Token: 0x0400634A RID: 25418
		public byte transitionId;
	}

	// Token: 0x02001265 RID: 4709
	public struct PotentialPath
	{
		// Token: 0x06008303 RID: 33539 RVA: 0x0031E2D1 File Offset: 0x0031C4D1
		public PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
		{
			this.cell = cell;
			this.navType = nav_type;
			this.flags = flags;
		}

		// Token: 0x06008304 RID: 33540 RVA: 0x0031E2E8 File Offset: 0x0031C4E8
		public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags |= new_flags;
		}

		// Token: 0x06008305 RID: 33541 RVA: 0x0031E2F8 File Offset: 0x0031C4F8
		public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags &= ~new_flags;
		}

		// Token: 0x06008306 RID: 33542 RVA: 0x0031E30A File Offset: 0x0031C50A
		public bool HasFlag(PathFinder.PotentialPath.Flags flag)
		{
			return this.HasAnyFlag(flag);
		}

		// Token: 0x06008307 RID: 33543 RVA: 0x0031E313 File Offset: 0x0031C513
		public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask)
		{
			return (this.flags & mask) > PathFinder.PotentialPath.Flags.None;
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06008308 RID: 33544 RVA: 0x0031E320 File Offset: 0x0031C520
		// (set) Token: 0x06008309 RID: 33545 RVA: 0x0031E328 File Offset: 0x0031C528
		public PathFinder.PotentialPath.Flags flags { readonly get; private set; }

		// Token: 0x0400634B RID: 25419
		public int cell;

		// Token: 0x0400634C RID: 25420
		public NavType navType;

		// Token: 0x02002400 RID: 9216
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x0400A0C1 RID: 41153
			None = 0,
			// Token: 0x0400A0C2 RID: 41154
			HasAtmoSuit = 1,
			// Token: 0x0400A0C3 RID: 41155
			HasJetPack = 2,
			// Token: 0x0400A0C4 RID: 41156
			HasOxygenMask = 4,
			// Token: 0x0400A0C5 RID: 41157
			PerformSuitChecks = 8,
			// Token: 0x0400A0C6 RID: 41158
			HasLeadSuit = 16
		}
	}

	// Token: 0x02001266 RID: 4710
	public struct Path
	{
		// Token: 0x0600830A RID: 33546 RVA: 0x0031E331 File Offset: 0x0031C531
		public void AddNode(PathFinder.Path.Node node)
		{
			if (this.nodes == null)
			{
				this.nodes = new List<PathFinder.Path.Node>();
			}
			this.nodes.Add(node);
		}

		// Token: 0x0600830B RID: 33547 RVA: 0x0031E352 File Offset: 0x0031C552
		public bool IsValid()
		{
			return this.nodes != null && this.nodes.Count > 1;
		}

		// Token: 0x0600830C RID: 33548 RVA: 0x0031E36C File Offset: 0x0031C56C
		public bool HasArrived()
		{
			return this.nodes != null && this.nodes.Count > 0;
		}

		// Token: 0x0600830D RID: 33549 RVA: 0x0031E386 File Offset: 0x0031C586
		public void Clear()
		{
			this.cost = 0;
			if (this.nodes != null)
			{
				this.nodes.Clear();
			}
		}

		// Token: 0x0400634E RID: 25422
		public int cost;

		// Token: 0x0400634F RID: 25423
		public List<PathFinder.Path.Node> nodes;

		// Token: 0x02002401 RID: 9217
		public struct Node
		{
			// Token: 0x0400A0C7 RID: 41159
			public int cell;

			// Token: 0x0400A0C8 RID: 41160
			public NavType navType;

			// Token: 0x0400A0C9 RID: 41161
			public byte transitionId;
		}
	}

	// Token: 0x02001267 RID: 4711
	public class PotentialList
	{
		// Token: 0x0600830E RID: 33550 RVA: 0x0031E3A2 File Offset: 0x0031C5A2
		public KeyValuePair<int, PathFinder.PotentialPath> Next()
		{
			return this.queue.Dequeue();
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x0600830F RID: 33551 RVA: 0x0031E3AF File Offset: 0x0031C5AF
		public int Count
		{
			get
			{
				return this.queue.Count;
			}
		}

		// Token: 0x06008310 RID: 33552 RVA: 0x0031E3BC File Offset: 0x0031C5BC
		public void Add(int cost, PathFinder.PotentialPath path)
		{
			this.queue.Enqueue(cost, path);
		}

		// Token: 0x06008311 RID: 33553 RVA: 0x0031E3CB File Offset: 0x0031C5CB
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04006350 RID: 25424
		private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

		// Token: 0x02002402 RID: 9218
		public class PriorityQueue<TValue>
		{
			// Token: 0x0600B897 RID: 47255 RVA: 0x003CF69D File Offset: 0x003CD89D
			public PriorityQueue()
			{
				this._baseHeap = new List<KeyValuePair<int, TValue>>();
			}

			// Token: 0x0600B898 RID: 47256 RVA: 0x003CF6B0 File Offset: 0x003CD8B0
			public void Enqueue(int priority, TValue value)
			{
				this.Insert(priority, value);
			}

			// Token: 0x0600B899 RID: 47257 RVA: 0x003CF6BA File Offset: 0x003CD8BA
			public KeyValuePair<int, TValue> Dequeue()
			{
				KeyValuePair<int, TValue> result = this._baseHeap[0];
				this.DeleteRoot();
				return result;
			}

			// Token: 0x0600B89A RID: 47258 RVA: 0x003CF6CE File Offset: 0x003CD8CE
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.Count > 0)
				{
					return this._baseHeap[0];
				}
				throw new InvalidOperationException("Priority queue is empty");
			}

			// Token: 0x0600B89B RID: 47259 RVA: 0x003CF6F0 File Offset: 0x003CD8F0
			private void ExchangeElements(int pos1, int pos2)
			{
				KeyValuePair<int, TValue> value = this._baseHeap[pos1];
				this._baseHeap[pos1] = this._baseHeap[pos2];
				this._baseHeap[pos2] = value;
			}

			// Token: 0x0600B89C RID: 47260 RVA: 0x003CF730 File Offset: 0x003CD930
			private void Insert(int priority, TValue value)
			{
				KeyValuePair<int, TValue> item = new KeyValuePair<int, TValue>(priority, value);
				this._baseHeap.Add(item);
				this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
			}

			// Token: 0x0600B89D RID: 47261 RVA: 0x003CF768 File Offset: 0x003CD968
			private int HeapifyFromEndToBeginning(int pos)
			{
				if (pos >= this._baseHeap.Count)
				{
					return -1;
				}
				while (pos > 0)
				{
					int num = (pos - 1) / 2;
					if (this._baseHeap[num].Key - this._baseHeap[pos].Key <= 0)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
				return pos;
			}

			// Token: 0x0600B89E RID: 47262 RVA: 0x003CF7C8 File Offset: 0x003CD9C8
			private void DeleteRoot()
			{
				if (this._baseHeap.Count <= 1)
				{
					this._baseHeap.Clear();
					return;
				}
				this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
				this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
				this.HeapifyFromBeginningToEnd(0);
			}

			// Token: 0x0600B89F RID: 47263 RVA: 0x003CF834 File Offset: 0x003CDA34
			private void HeapifyFromBeginningToEnd(int pos)
			{
				int count = this._baseHeap.Count;
				if (pos >= count)
				{
					return;
				}
				for (;;)
				{
					int num = pos;
					int num2 = 2 * pos + 1;
					int num3 = 2 * pos + 2;
					if (num2 < count && this._baseHeap[num].Key - this._baseHeap[num2].Key > 0)
					{
						num = num2;
					}
					if (num3 < count && this._baseHeap[num].Key - this._baseHeap[num3].Key > 0)
					{
						num = num3;
					}
					if (num == pos)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
			}

			// Token: 0x0600B8A0 RID: 47264 RVA: 0x003CF8DC File Offset: 0x003CDADC
			public void Clear()
			{
				this._baseHeap.Clear();
			}

			// Token: 0x17000C11 RID: 3089
			// (get) Token: 0x0600B8A1 RID: 47265 RVA: 0x003CF8E9 File Offset: 0x003CDAE9
			public int Count
			{
				get
				{
					return this._baseHeap.Count;
				}
			}

			// Token: 0x0400A0CA RID: 41162
			private List<KeyValuePair<int, TValue>> _baseHeap;
		}

		// Token: 0x02002403 RID: 9219
		private class HOTQueue<TValue>
		{
			// Token: 0x0600B8A2 RID: 47266 RVA: 0x003CF8F8 File Offset: 0x003CDAF8
			public KeyValuePair<int, TValue> Dequeue()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				this.count--;
				return this.hotQueue.Dequeue();
			}

			// Token: 0x0600B8A3 RID: 47267 RVA: 0x003CF954 File Offset: 0x003CDB54
			public void Enqueue(int priority, TValue value)
			{
				if (priority <= this.hotThreshold)
				{
					this.hotQueue.Enqueue(priority, value);
				}
				else
				{
					this.coldQueue.Enqueue(priority, value);
					this.coldThreshold = Math.Max(this.coldThreshold, priority);
				}
				this.count++;
			}

			// Token: 0x0600B8A4 RID: 47268 RVA: 0x003CF9A8 File Offset: 0x003CDBA8
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				return this.hotQueue.Peek();
			}

			// Token: 0x0600B8A5 RID: 47269 RVA: 0x003CF9F3 File Offset: 0x003CDBF3
			public void Clear()
			{
				this.count = 0;
				this.hotThreshold = int.MinValue;
				this.hotQueue.Clear();
				this.coldThreshold = int.MinValue;
				this.coldQueue.Clear();
			}

			// Token: 0x17000C12 RID: 3090
			// (get) Token: 0x0600B8A6 RID: 47270 RVA: 0x003CFA28 File Offset: 0x003CDC28
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x0400A0CB RID: 41163
			private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x0400A0CC RID: 41164
			private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x0400A0CD RID: 41165
			private int hotThreshold = int.MinValue;

			// Token: 0x0400A0CE RID: 41166
			private int coldThreshold = int.MinValue;

			// Token: 0x0400A0CF RID: 41167
			private int count;
		}
	}

	// Token: 0x02001268 RID: 4712
	private class Temp
	{
		// Token: 0x04006351 RID: 25425
		public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
	}

	// Token: 0x02001269 RID: 4713
	public class PotentialScratchPad
	{
		// Token: 0x06008315 RID: 33557 RVA: 0x0031E3FF File Offset: 0x0031C5FF
		public PotentialScratchPad(int max_links_per_cell)
		{
			this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
			this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
		}

		// Token: 0x04006352 RID: 25426
		public NavGrid.Link[] linksWithCorrectNavType;

		// Token: 0x04006353 RID: 25427
		public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

		// Token: 0x02002404 RID: 9220
		public struct PathGridCellData
		{
			// Token: 0x0400A0D0 RID: 41168
			public PathFinder.Cell pathGridCell;

			// Token: 0x0400A0D1 RID: 41169
			public NavGrid.Link link;

			// Token: 0x0400A0D2 RID: 41170
			public bool isSubmerged;
		}
	}
}
