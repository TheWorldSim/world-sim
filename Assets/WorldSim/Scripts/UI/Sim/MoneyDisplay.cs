using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
	private TextMeshProUGUI cost;
	private Button toggle_button;
	private bool show_total_cost = false;
	private RegionOfInterest roi;

	void Awake()
    {
		cost = transform.Find("Cost").GetComponent<TextMeshProUGUI>();
		toggle_button = transform.Find("ToggleButton").GetComponent<Button>();
		toggle_button.onClick.AddListener(handle_toggle_click);
	}

	private void Start()
	{
		roi = GameManagerWorldSim.instance.selected_region_of_interest;

		GameManagerWorldSim.date_time.on_changed_sim_date_time += handle_changed_sim_date_time;
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
	}

	private void handle_changed_sim_date_time(ChangedDateTime change)
	{
		if (show_total_cost) display_total_cost();
		else display_cost_per_kWh();
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		roi = region;
	}

	private void display_total_cost()
	{
		var mdollars_per_year = roi.currency.conversion_from_USD * GameManagerWorldSim.power.total_cost_mdollar_per_year;
		cost.text = roi.currency.symbol + mdollars_per_year.ToString("0") + "m per year";
	}

	private void display_cost_per_kWh()
	{
		var GJ_per_year = GameManagerWorldSim.power.total_demand_base_fulfilled_GJ_per_year;
		if (GJ_per_year == 0)
		{
			cost.text = "infinite " + roi.currency.symbol + " per kWh";
			return;
		}

		var mdollars_per_year = GameManagerWorldSim.power.total_cost_mdollar_per_year;
		var dollar_per_GJ = (mdollars_per_year / GJ_per_year) * 1e6f;

		var dollar_per_kWh = dollar_per_GJ * DataConversionsManager.GJ_per_kWh;
		var currency_cents_per_kWh = dollar_per_kWh * 100 * roi.currency.conversion_from_USD;

		//Debug.Log("GJ " + GJ_per_year + " mdollars_per_year " + mdollars_per_year);

		cost.text = currency_cents_per_kWh.ToString("0.00") + roi.currency.symbol_cents + " per kWh";
	}

	private void handle_toggle_click()
	{
		show_total_cost = !show_total_cost;
	}

	//private void handle_money_change(object obj, MoneyChange money_change)
	//{
	//	cost.text = "M$" + Mathf.RoundToInt((float)money_change.current_dollars / 1e6f);
	//}
}
