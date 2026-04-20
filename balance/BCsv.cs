using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace TreasureHunter.balance;

public class BCsv
{
		public Dictionary<UpgradeType, Dictionary<int, BValue>> balanced(string path)
	{
		var result = new Dictionary<UpgradeType, Dictionary<int, BValue>>();
		using var parser = new TextFieldParser(path);
		parser.TextFieldType = FieldType.Delimited;
		parser.SetDelimiters(",");
		parser.HasFieldsEnclosedInQuotes = true;
		var headers = parser.ReadFields();
		if (headers == null) return result;
		foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
		{
			result[type] = new Dictionary<int, BValue>();
			var col = Array.FindIndex(headers, h => h.Trim() == type.ToString());
			if (col < 0) continue;
			using var dataParser = new TextFieldParser(path);
			dataParser.TextFieldType = FieldType.Delimited;
			dataParser.SetDelimiters(",");
			dataParser.HasFieldsEnclosedInQuotes = true;
			dataParser.ReadFields();
			while (!dataParser.EndOfData)
			{
				var parts = dataParser.ReadFields();
				if (parts == null || parts.Length <= col + 1) continue;
				if (string.IsNullOrWhiteSpace(parts[0])) continue;
				if (!int.TryParse(parts[0], out var level)) continue;
				var value = new decimal();
				value = ParseDecimal(type == UpgradeType.TREASURE_TIER ? parts[col - 1] : parts[col]);
				var cost = ParseDecimal(parts[col + 1]);

					result[type][level] = new BValue(value, cost);
			}
		}

		return result;
	}

	private decimal ParseDecimal(string s)
	{
		if (string.IsNullOrWhiteSpace(s)) return 0;
		var normalized = s.Replace(',', '.');
		if (decimal.TryParse(normalized, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
			return d;
		if (double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var dbl))
			return (decimal)dbl;
		return 0;
	}
}
