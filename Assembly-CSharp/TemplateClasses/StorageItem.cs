using System;

namespace TemplateClasses
{
	// Token: 0x02000E29 RID: 3625
	[Serializable]
	public class StorageItem
	{
		// Token: 0x060073A1 RID: 29601 RVA: 0x002C455C File Offset: 0x002C275C
		public StorageItem()
		{
			this.rottable = new Rottable();
		}

		// Token: 0x060073A2 RID: 29602 RVA: 0x002C4570 File Offset: 0x002C2770
		public StorageItem(string _id, float _units, float _temp, SimHashes _element, string _disease, int _disease_count, bool _isOre)
		{
			this.rottable = new Rottable();
			this.id = _id;
			this.element = _element;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.isOre = _isOre;
			this.temperature = _temp;
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x060073A3 RID: 29603 RVA: 0x002C45C3 File Offset: 0x002C27C3
		// (set) Token: 0x060073A4 RID: 29604 RVA: 0x002C45CB File Offset: 0x002C27CB
		public string id { get; set; }

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x060073A5 RID: 29605 RVA: 0x002C45D4 File Offset: 0x002C27D4
		// (set) Token: 0x060073A6 RID: 29606 RVA: 0x002C45DC File Offset: 0x002C27DC
		public SimHashes element { get; set; }

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060073A7 RID: 29607 RVA: 0x002C45E5 File Offset: 0x002C27E5
		// (set) Token: 0x060073A8 RID: 29608 RVA: 0x002C45ED File Offset: 0x002C27ED
		public float units { get; set; }

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060073A9 RID: 29609 RVA: 0x002C45F6 File Offset: 0x002C27F6
		// (set) Token: 0x060073AA RID: 29610 RVA: 0x002C45FE File Offset: 0x002C27FE
		public bool isOre { get; set; }

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060073AB RID: 29611 RVA: 0x002C4607 File Offset: 0x002C2807
		// (set) Token: 0x060073AC RID: 29612 RVA: 0x002C460F File Offset: 0x002C280F
		public float temperature { get; set; }

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060073AD RID: 29613 RVA: 0x002C4618 File Offset: 0x002C2818
		// (set) Token: 0x060073AE RID: 29614 RVA: 0x002C4620 File Offset: 0x002C2820
		public string diseaseName { get; set; }

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x060073AF RID: 29615 RVA: 0x002C4629 File Offset: 0x002C2829
		// (set) Token: 0x060073B0 RID: 29616 RVA: 0x002C4631 File Offset: 0x002C2831
		public int diseaseCount { get; set; }

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x060073B1 RID: 29617 RVA: 0x002C463A File Offset: 0x002C283A
		// (set) Token: 0x060073B2 RID: 29618 RVA: 0x002C4642 File Offset: 0x002C2842
		public Rottable rottable { get; set; }

		// Token: 0x060073B3 RID: 29619 RVA: 0x002C464C File Offset: 0x002C284C
		public StorageItem Clone()
		{
			return new StorageItem(this.id, this.units, this.temperature, this.element, this.diseaseName, this.diseaseCount, this.isOre)
			{
				rottable = 
				{
					rotAmount = this.rottable.rotAmount
				}
			};
		}
	}
}
