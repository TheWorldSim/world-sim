using UnityEngine;

public class DataConversionsManager : MonoBehaviour
{
	public static float days_per_year = (365 * 3 + 366) / 4;
	public static float hours_per_year = days_per_year * 24; // == 8766
	public static float s_per_year = hours_per_year * 3600; // == 31,557,600

	public static float GJ_per_MWh = 3.6f;  // 3,600,000,000 J per MWh and 1,000,000,000 J per GJ
	public static float GJ_per_kWh = GJ_per_MWh / 1000;  // 3,600,000 J per kWh and 1,000,000,000 J per GJ

	public static float GBP_per_USD_2020_02_13 = 0.77f;
}
