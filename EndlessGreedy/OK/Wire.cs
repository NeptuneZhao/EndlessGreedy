using HarmonyLib;

[HarmonyPatch(typeof(Wire))]
internal class SuperWire
{
	[HarmonyPostfix]
	[HarmonyPatch("GetMaxWattageAsFloat")]
	public static void Postfix(ref float __result)
	{
		__result = 5e5f;
	}
}