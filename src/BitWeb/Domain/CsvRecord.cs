namespace BitWeb.Domain;
public class CsvRecord
{
    public long Unix { get; set; }
    public DateTime Date { get; set; }
    public string Symbol { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal VolumeBtc { get; set; }
    public decimal VolumeUsd { get; set; }
}
