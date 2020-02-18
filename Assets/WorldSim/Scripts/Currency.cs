using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency
{
	public string symbol { get; private set; }
	public string symbol_cents { get; private set; }
	public float conversion_from_USD { get; private set; }

	public Currency(string symbol, string symbol_cents, float conversion_from_USD)
	{
		this.symbol = symbol;
		this.symbol_cents = symbol_cents;
		this.conversion_from_USD = conversion_from_USD;
	}

	// currencies
	public static Currency USD = new Currency("$", "¢", 1);
	public static Currency GBP = new Currency("£", "p", DataConversionsManager.GBP_per_USD_2020_02_13);
}
