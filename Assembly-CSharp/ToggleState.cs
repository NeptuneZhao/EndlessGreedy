using System;
using UnityEngine;

// Token: 0x02000DF9 RID: 3577
[Serializable]
public struct ToggleState
{
	// Token: 0x04004E4F RID: 20047
	public string Name;

	// Token: 0x04004E50 RID: 20048
	public string on_click_override_sound_path;

	// Token: 0x04004E51 RID: 20049
	public string on_release_override_sound_path;

	// Token: 0x04004E52 RID: 20050
	public string sound_parameter_name;

	// Token: 0x04004E53 RID: 20051
	public float sound_parameter_value;

	// Token: 0x04004E54 RID: 20052
	public bool has_sound_parameter;

	// Token: 0x04004E55 RID: 20053
	public Sprite sprite;

	// Token: 0x04004E56 RID: 20054
	public Color color;

	// Token: 0x04004E57 RID: 20055
	public Color color_on_hover;

	// Token: 0x04004E58 RID: 20056
	public bool use_color_on_hover;

	// Token: 0x04004E59 RID: 20057
	public bool use_rect_margins;

	// Token: 0x04004E5A RID: 20058
	public Vector2 rect_margins;

	// Token: 0x04004E5B RID: 20059
	public StatePresentationSetting[] additional_display_settings;
}
