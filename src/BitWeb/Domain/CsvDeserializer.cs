using System.Globalization;

namespace BitWeb.Domain;

public static class CsvDeserializer
{

    public static CsvRecord DeserializeLine(string csvLine)
    {
        var fields = csvLine.Split(',');

        if (fields.Length != 9)
        {
            throw new FormatException("Invalid CSV format.");
        }

        return new CsvRecord
        {
            Unix = long.Parse(fields[0]),
            Date = DateTime.ParseExact(fields[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            Symbol = fields[2],
            Open = decimal.Parse(fields[3]),
            High = decimal.Parse(fields[4]),
            Low = decimal.Parse(fields[5]),
            Close = decimal.Parse(fields[6]),
            VolumeBtc = decimal.Parse(fields[7]),
            VolumeUsd = decimal.Parse(fields[8])
        };
    }

    public static IEnumerable<CsvRecord> DeserializeCsv(IEnumerable<string> csvLines)
    {
        // Skip the header line
        bool isFirstLine = true;
        foreach (var line in csvLines)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                continue; // Skip header
            }

            var fields = line.Split(',');
            if (fields.Length != 9)
            {
                throw new FormatException("Invalid CSV format.");
            }

            yield return new CsvRecord
            {
                Unix = long.Parse(fields[0]),
                Date = DateTime.ParseExact(fields[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Symbol = fields[2],
                Open = decimal.Parse(fields[3]),
                High = decimal.Parse(fields[4]),
                Low = decimal.Parse(fields[5]),
                Close = decimal.Parse(fields[6]),
                VolumeBtc = decimal.Parse(fields[7]),
                VolumeUsd = decimal.Parse(fields[8])
            };
        }
    }
}
