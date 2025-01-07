
public class HistoryDataPoint<V>
{
	public ChangedDateTime change { get; private set; }
	public V value { get; private set; }

	public HistoryDataPoint(ChangedDateTime change, V value)
	{
		this.change = change;
		this.value = value;
	}

	public override string ToString()
	{
		return change.ToString() + "   value: " + value;
	}
}
