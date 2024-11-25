using System;

namespace TemplateClasses
{
	// Token: 0x02000E28 RID: 3624
	[Serializable]
	public class Cell
	{
		// Token: 0x0600738F RID: 29583 RVA: 0x002C4479 File Offset: 0x002C2679
		public Cell()
		{
		}

		// Token: 0x06007390 RID: 29584 RVA: 0x002C4484 File Offset: 0x002C2684
		public Cell(int loc_x, int loc_y, SimHashes _element, float _temperature, float _mass, string _diseaseName, int _diseaseCount, bool _preventFoWReveal = false)
		{
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.element = _element;
			this.temperature = _temperature;
			this.mass = _mass;
			this.diseaseName = _diseaseName;
			this.diseaseCount = _diseaseCount;
			this.preventFoWReveal = _preventFoWReveal;
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06007391 RID: 29585 RVA: 0x002C44D4 File Offset: 0x002C26D4
		// (set) Token: 0x06007392 RID: 29586 RVA: 0x002C44DC File Offset: 0x002C26DC
		public SimHashes element { get; set; }

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06007393 RID: 29587 RVA: 0x002C44E5 File Offset: 0x002C26E5
		// (set) Token: 0x06007394 RID: 29588 RVA: 0x002C44ED File Offset: 0x002C26ED
		public float mass { get; set; }

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06007395 RID: 29589 RVA: 0x002C44F6 File Offset: 0x002C26F6
		// (set) Token: 0x06007396 RID: 29590 RVA: 0x002C44FE File Offset: 0x002C26FE
		public float temperature { get; set; }

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06007397 RID: 29591 RVA: 0x002C4507 File Offset: 0x002C2707
		// (set) Token: 0x06007398 RID: 29592 RVA: 0x002C450F File Offset: 0x002C270F
		public string diseaseName { get; set; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06007399 RID: 29593 RVA: 0x002C4518 File Offset: 0x002C2718
		// (set) Token: 0x0600739A RID: 29594 RVA: 0x002C4520 File Offset: 0x002C2720
		public int diseaseCount { get; set; }

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600739B RID: 29595 RVA: 0x002C4529 File Offset: 0x002C2729
		// (set) Token: 0x0600739C RID: 29596 RVA: 0x002C4531 File Offset: 0x002C2731
		public int location_x { get; set; }

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600739D RID: 29597 RVA: 0x002C453A File Offset: 0x002C273A
		// (set) Token: 0x0600739E RID: 29598 RVA: 0x002C4542 File Offset: 0x002C2742
		public int location_y { get; set; }

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600739F RID: 29599 RVA: 0x002C454B File Offset: 0x002C274B
		// (set) Token: 0x060073A0 RID: 29600 RVA: 0x002C4553 File Offset: 0x002C2753
		public bool preventFoWReveal { get; set; }
	}
}
