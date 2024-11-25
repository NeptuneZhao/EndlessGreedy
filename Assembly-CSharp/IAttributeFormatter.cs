using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000BD6 RID: 3030
public interface IAttributeFormatter
{
	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06005C4A RID: 23626
	// (set) Token: 0x06005C4B RID: 23627
	GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x06005C4C RID: 23628
	string GetFormattedAttribute(AttributeInstance instance);

	// Token: 0x06005C4D RID: 23629
	string GetFormattedModifier(AttributeModifier modifier);

	// Token: 0x06005C4E RID: 23630
	string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice);

	// Token: 0x06005C4F RID: 23631
	string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance);

	// Token: 0x06005C50 RID: 23632
	string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters);
}
