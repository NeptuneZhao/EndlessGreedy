using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200086F RID: 2159
[SerializationConfig(MemberSerialization.OptIn)]
public class EightDirectionUtil
{
	// Token: 0x06003C26 RID: 15398 RVA: 0x0014DF14 File Offset: 0x0014C114
	public static int GetDirectionIndex(EightDirection direction)
	{
		return (int)direction;
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0014DF17 File Offset: 0x0014C117
	public static EightDirection AngleToDirection(int angle)
	{
		return (EightDirection)Mathf.Floor((float)angle / 45f);
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x0014DF27 File Offset: 0x0014C127
	public static Vector3 GetNormal(EightDirection direction)
	{
		return EightDirectionUtil.normals[EightDirectionUtil.GetDirectionIndex(direction)];
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x0014DF39 File Offset: 0x0014C139
	public static float GetAngle(EightDirection direction)
	{
		return (float)(45 * EightDirectionUtil.GetDirectionIndex(direction));
	}

	// Token: 0x04002482 RID: 9346
	public static readonly Vector3[] normals = new Vector3[]
	{
		Vector3.up,
		(Vector3.up + Vector3.left).normalized,
		Vector3.left,
		(Vector3.down + Vector3.left).normalized,
		Vector3.down,
		(Vector3.down + Vector3.right).normalized,
		Vector3.right,
		(Vector3.up + Vector3.right).normalized
	};
}
