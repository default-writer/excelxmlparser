using ExcelDna.Integration;
using System.Collections.Generic;

namespace XmlParser
{
    public class ExcelData
    {
        public void Write(List<KeyValuePair<string, string>> parsed)
        {
            dynamic xlApp = ExcelDnaUtil.Application;

            var ws = xlApp.ActiveSheet;

            if (ws == null)
                return;

            ws.Cells.Clear();

            var data = new object[parsed.Count, 2];
            for (var i = 0; i < parsed.Count; i++)
            {
                data[i, 0] = parsed[i].Key;
                data[i, 1] = parsed[i].Value;
            }

            var startCell = ws.Cells[1, 1];
            var endCell = ws.Cells[data.GetLength(0), data.GetLength(1)];

            var range = ws.Range[startCell, endCell];

            range.Value2 = data;

            ws.Range[startCell, endCell].Columns.AutoFit();
        }

        internal List<KeyValuePair<string, string>> Read()
        {
            var result = new List<KeyValuePair<string, string>>();

            //
            dynamic xlApp = ExcelDnaUtil.Application;

            var ws = xlApp.ActiveSheet;

            if (ws == null)
                return result;

            var startCell = ws.Cells[1, 1];

            var endCell = startCell;
            int count = 1;
            while (ws.Cells[count, 1].Value != null)
            {
                endCell = ws.Cells[count, 2];
                ++count;
            }

            var range = ws.Range[startCell, endCell];
            var data = (object[,])range.Value2;

            //var data = new object[i, 2];
            var start = data.GetLowerBound(1);
            var end = data.GetUpperBound(1);
            for (var i = data.GetLowerBound(0); i <= data.GetUpperBound(0); i++)
            {
                result.Add(new KeyValuePair<string, string>($"{data[i, start]}", $"{data[i, end]}"));
            }

            ws.Cells.Clear();

            return result;
        }
    }
}
