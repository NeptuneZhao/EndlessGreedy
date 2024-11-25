using System;
using System.Collections.Generic;

// Token: 0x02000983 RID: 2435
public class ExposureType
{
	// Token: 0x04002E64 RID: 11876
	public string germ_id;

	// Token: 0x04002E65 RID: 11877
	public string sickness_id;

	// Token: 0x04002E66 RID: 11878
	public string infection_effect;

	// Token: 0x04002E67 RID: 11879
	public int exposure_threshold;

	// Token: 0x04002E68 RID: 11880
	public bool infect_immediately;

	// Token: 0x04002E69 RID: 11881
	public List<string> required_traits;

	// Token: 0x04002E6A RID: 11882
	public List<string> excluded_traits;

	// Token: 0x04002E6B RID: 11883
	public List<string> excluded_effects;

	// Token: 0x04002E6C RID: 11884
	public int base_resistance;
}
