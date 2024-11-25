using System;

// Token: 0x020008A8 RID: 2216
public class Expectation
{
	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x001566C1 File Offset: 0x001548C1
	// (set) Token: 0x06003DF6 RID: 15862 RVA: 0x001566C9 File Offset: 0x001548C9
	public string id { get; protected set; }

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x001566D2 File Offset: 0x001548D2
	// (set) Token: 0x06003DF8 RID: 15864 RVA: 0x001566DA File Offset: 0x001548DA
	public string name { get; protected set; }

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x001566E3 File Offset: 0x001548E3
	// (set) Token: 0x06003DFA RID: 15866 RVA: 0x001566EB File Offset: 0x001548EB
	public string description { get; protected set; }

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06003DFB RID: 15867 RVA: 0x001566F4 File Offset: 0x001548F4
	// (set) Token: 0x06003DFC RID: 15868 RVA: 0x001566FC File Offset: 0x001548FC
	public Action<MinionResume> OnApply { get; protected set; }

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06003DFD RID: 15869 RVA: 0x00156705 File Offset: 0x00154905
	// (set) Token: 0x06003DFE RID: 15870 RVA: 0x0015670D File Offset: 0x0015490D
	public Action<MinionResume> OnRemove { get; protected set; }

	// Token: 0x06003DFF RID: 15871 RVA: 0x00156716 File Offset: 0x00154916
	public Expectation(string id, string name, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.OnApply = OnApply;
		this.OnRemove = OnRemove;
	}
}
