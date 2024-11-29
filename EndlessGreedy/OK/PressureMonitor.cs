using HarmonyLib;

[HarmonyPatch(typeof(PressureMonitor.Instance))]
internal class SuperPressureMonitor
{
	/// <summary>
	/// 不让鼓膜破裂
	/// </summary>
	/// <param name="__result"></param>
	[HarmonyPostfix, HarmonyPatch(nameof(PressureMonitor.Instance.IsInHighPressure))]
	public static void Postfix(ref bool __result)
	{
		__result = false;
	}
}