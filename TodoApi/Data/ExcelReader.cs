using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TodoApi.Models;

namespace TodoApi.Data
{
    public static class ExcelReader
    {
        public static IEnumerable<Settlement> ReadFromExcel()
        {
            var result = new List<Settlement>();
            using (var reader = new StreamReader("..//TodoApi//Data//DataResults.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                result.AddRange(csv.GetRecords<Settlement>());
            }

            return result;
        }
    }
}
