using System;
using System.Collections.Generic;

namespace TemplateClasses
{
	// Token: 0x02000E27 RID: 3623
	[Serializable]
	public class Prefab
	{
		// Token: 0x0600736B RID: 29547 RVA: 0x002C4193 File Offset: 0x002C2393
		public Prefab()
		{
			this.type = Prefab.Type.Other;
		}

		// Token: 0x0600736C RID: 29548 RVA: 0x002C41A4 File Offset: 0x002C23A4
		public Prefab(string _id, Prefab.Type _type, int loc_x, int loc_y, SimHashes _element, float _temperature = -1f, float _units = 1f, string _disease = null, int _disease_count = 0, Orientation _rotation = Orientation.Neutral, Prefab.template_amount_value[] _amount_values = null, Prefab.template_amount_value[] _other_values = null, int _connections = 0, string facadeIdId = null)
		{
			this.id = _id;
			this.type = _type;
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.connections = _connections;
			this.element = _element;
			this.temperature = _temperature;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.facadeId = facadeIdId;
			this.rotationOrientation = _rotation;
			if (_amount_values != null && _amount_values.Length != 0)
			{
				this.amounts = _amount_values;
			}
			if (_other_values != null && _other_values.Length != 0)
			{
				this.other_values = _other_values;
			}
		}

		// Token: 0x0600736D RID: 29549 RVA: 0x002C4238 File Offset: 0x002C2438
		public Prefab Clone(Vector2I offset)
		{
			Prefab prefab = new Prefab(this.id, this.type, offset.x + this.location_x, offset.y + this.location_y, this.element, this.temperature, this.units, this.diseaseName, this.diseaseCount, this.rotationOrientation, this.amounts, this.other_values, this.connections, this.facadeId);
			if (this.rottable != null)
			{
				prefab.rottable = new Rottable();
				prefab.rottable.rotAmount = this.rottable.rotAmount;
			}
			if (this.storage != null && this.storage.Count > 0)
			{
				prefab.storage = new List<StorageItem>();
				foreach (StorageItem storageItem in this.storage)
				{
					prefab.storage.Add(storageItem.Clone());
				}
			}
			return prefab;
		}

		// Token: 0x0600736E RID: 29550 RVA: 0x002C4348 File Offset: 0x002C2548
		public void AssignStorage(StorageItem _storage)
		{
			if (this.storage == null)
			{
				this.storage = new List<StorageItem>();
			}
			this.storage.Add(_storage);
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x0600736F RID: 29551 RVA: 0x002C4369 File Offset: 0x002C2569
		// (set) Token: 0x06007370 RID: 29552 RVA: 0x002C4371 File Offset: 0x002C2571
		public string id { get; set; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06007371 RID: 29553 RVA: 0x002C437A File Offset: 0x002C257A
		// (set) Token: 0x06007372 RID: 29554 RVA: 0x002C4382 File Offset: 0x002C2582
		public int location_x { get; set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06007373 RID: 29555 RVA: 0x002C438B File Offset: 0x002C258B
		// (set) Token: 0x06007374 RID: 29556 RVA: 0x002C4393 File Offset: 0x002C2593
		public int location_y { get; set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06007375 RID: 29557 RVA: 0x002C439C File Offset: 0x002C259C
		// (set) Token: 0x06007376 RID: 29558 RVA: 0x002C43A4 File Offset: 0x002C25A4
		public SimHashes element { get; set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06007377 RID: 29559 RVA: 0x002C43AD File Offset: 0x002C25AD
		// (set) Token: 0x06007378 RID: 29560 RVA: 0x002C43B5 File Offset: 0x002C25B5
		public float temperature { get; set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06007379 RID: 29561 RVA: 0x002C43BE File Offset: 0x002C25BE
		// (set) Token: 0x0600737A RID: 29562 RVA: 0x002C43C6 File Offset: 0x002C25C6
		public float units { get; set; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600737B RID: 29563 RVA: 0x002C43CF File Offset: 0x002C25CF
		// (set) Token: 0x0600737C RID: 29564 RVA: 0x002C43D7 File Offset: 0x002C25D7
		public string diseaseName { get; set; }

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600737D RID: 29565 RVA: 0x002C43E0 File Offset: 0x002C25E0
		// (set) Token: 0x0600737E RID: 29566 RVA: 0x002C43E8 File Offset: 0x002C25E8
		public int diseaseCount { get; set; }

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x0600737F RID: 29567 RVA: 0x002C43F1 File Offset: 0x002C25F1
		// (set) Token: 0x06007380 RID: 29568 RVA: 0x002C43F9 File Offset: 0x002C25F9
		public Orientation rotationOrientation { get; set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06007381 RID: 29569 RVA: 0x002C4402 File Offset: 0x002C2602
		// (set) Token: 0x06007382 RID: 29570 RVA: 0x002C440A File Offset: 0x002C260A
		public List<StorageItem> storage { get; set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06007383 RID: 29571 RVA: 0x002C4413 File Offset: 0x002C2613
		// (set) Token: 0x06007384 RID: 29572 RVA: 0x002C441B File Offset: 0x002C261B
		public Prefab.Type type { get; set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06007385 RID: 29573 RVA: 0x002C4424 File Offset: 0x002C2624
		// (set) Token: 0x06007386 RID: 29574 RVA: 0x002C442C File Offset: 0x002C262C
		public string facadeId { get; set; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06007387 RID: 29575 RVA: 0x002C4435 File Offset: 0x002C2635
		// (set) Token: 0x06007388 RID: 29576 RVA: 0x002C443D File Offset: 0x002C263D
		public int connections { get; set; }

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06007389 RID: 29577 RVA: 0x002C4446 File Offset: 0x002C2646
		// (set) Token: 0x0600738A RID: 29578 RVA: 0x002C444E File Offset: 0x002C264E
		public Rottable rottable { get; set; }

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600738B RID: 29579 RVA: 0x002C4457 File Offset: 0x002C2657
		// (set) Token: 0x0600738C RID: 29580 RVA: 0x002C445F File Offset: 0x002C265F
		public Prefab.template_amount_value[] amounts { get; set; }

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600738D RID: 29581 RVA: 0x002C4468 File Offset: 0x002C2668
		// (set) Token: 0x0600738E RID: 29582 RVA: 0x002C4470 File Offset: 0x002C2670
		public Prefab.template_amount_value[] other_values { get; set; }

		// Token: 0x02001F4D RID: 8013
		public enum Type
		{
			// Token: 0x04008D39 RID: 36153
			Building,
			// Token: 0x04008D3A RID: 36154
			Ore,
			// Token: 0x04008D3B RID: 36155
			Pickupable,
			// Token: 0x04008D3C RID: 36156
			Other
		}

		// Token: 0x02001F4E RID: 8014
		[Serializable]
		public class template_amount_value
		{
			// Token: 0x0600ADDC RID: 44508 RVA: 0x003AAF2E File Offset: 0x003A912E
			public template_amount_value()
			{
			}

			// Token: 0x0600ADDD RID: 44509 RVA: 0x003AAF36 File Offset: 0x003A9136
			public template_amount_value(string id, float value)
			{
				this.id = id;
				this.value = value;
			}

			// Token: 0x17000BF7 RID: 3063
			// (get) Token: 0x0600ADDE RID: 44510 RVA: 0x003AAF4C File Offset: 0x003A914C
			// (set) Token: 0x0600ADDF RID: 44511 RVA: 0x003AAF54 File Offset: 0x003A9154
			public string id { get; set; }

			// Token: 0x17000BF8 RID: 3064
			// (get) Token: 0x0600ADE0 RID: 44512 RVA: 0x003AAF5D File Offset: 0x003A915D
			// (set) Token: 0x0600ADE1 RID: 44513 RVA: 0x003AAF65 File Offset: 0x003A9165
			public float value { get; set; }
		}
	}
}
