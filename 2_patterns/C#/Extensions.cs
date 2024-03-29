using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

/*
references :
LINQ to Objects is missing a few desirable features
https://www.nuget.org/packages/morelinq/
https://www.nuget.org/packages/MoreLinq.Portable
https://github.com/morelinq/MoreLINQ

https://www.extensionmethod.net/csharp
*/

public static class Extensions    
{

	public static string ToStrinX(this object value)
	{
		string retvalue = "";

		if (value != null)
			retvalue = value.ToString();

		return retvalue;


	}
	
	public static int ToInt(this object value)
	{
		int number = 0;

		if (value != null)
			int.TryParse(value.ToString(), out number);

		return number;
	}
	
        public static int ToInt(this char c)
        { // https://stackoverflow.com/a/239109
            return (int)(c - '0');
        }

	public static long ToLong(this object value)
	{
		long number = 0;

		if (value != null)
			long.TryParse(value.ToString(), out number);

		return number;
	}


	public static float ToFloat(this object value)
	{
		float result = 0;

		if (value != null)
			float.TryParse(value.ToString(), out result);

		return result;
	}

	public static double ToDouble(this object value)
	{
		double result = 0;

		if (value != null)
			double.TryParse(value.ToString(), out result);

		return result;
	}

	public static decimal ToDecimal(this object value)
	{
		decimal number = 0;

		if (value != null)
			Decimal.TryParse(value.ToString(), out number);

		return number;
	}

	public static DateTime ToDateTime(this object value)
	{
		DateTime date= new DateTime();

		if (value != null)
			DateTime.TryParse(value.ToString(), out date);

		return date;
	}

	public static bool ToBool(this object value)
	{
		bool result=false;
		if (value != null)
			bool.TryParse(value.ToString(), out result);

		return result;
	}

    public static string ReplaceIgnoreCase(this string str, string from, string to)
    {
		//https://stackoverflow.com/a/41275230
		//https://docs.microsoft.com/en-us/dotnet/standard/base-types/substitutions-in-regular-expressions
        return Regex.Replace(str, Regex.Escape(from), to.Replace("$", "$$"), RegexOptions.IgnoreCase);
    }
	
	public static string MakeSafeFilename(this string filename)
	{
	  Regex pattern = new Regex("[" + string.Join(",", Path.GetInvalidFileNameChars()) + "]");

	  return pattern.Replace(filename, "_");
	}

	public static string ShorterExact(this string source, int maxLength)
	{
	  if ((!string.IsNullOrEmpty(source)) && (source.Length > maxLength))
		  return source.Substring(0, maxLength);
	  else
		  return source;
	}
	
	public static bool StartsWithLetter(this string source)
	{
		if (!string.IsNullOrEmpty(source))
			return char.IsLetter(source[0]);
		else
			return false;
	}
	
	public static string Greek2Greeklish(this string source)
	{
	  var originalChar = new List<char> { 'ς', 'α', 'β', 'γ', 'δ', 'ε', 'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'π', 'ρ', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω', 'ά', 'έ', 'ή', 'ί', 'ό', 'ύ', 'ώ' };
	  var replaceWith = new List<char> { 's', 'a', 'b', 'g', 'd', 'e', 'z', 'h', '8', 'i', 'k', 'l', 'm', 'n', '3', 'o', 'p', 'r', 's', 't', 'u', 'f', 'x', 'c', 'w', 'a', 'e', 'h', 'i', 'o', 'u', 'w' };
	  originalChar.ForEach(x => source = source.Replace(x, replaceWith[originalChar.IndexOf(x)]));

	  return source;
	}
	
	public static string ToRows2CSV(this DataTable dataTable,string delimeter)
	{
		IEnumerable<string> items = dataTable.AsEnumerable().Select(row => row.Field<string>(0)); ;

		if (items == null)
			return "";

		return string.Join(delimeter, items);

		//if you construct a WHERE clause
		//v = .ToRows2CSV("','");, afterwards you need to adjust it v = string.Format("{0}{1}{0}", "'", v);
	}
	
        public static DataTable ToDatatable(this List<string> list)
        {
            DataTable dtTable = new DataTable();
            dtTable.Columns.Add("Name", typeof(string));

            foreach (string row in list)
                dtTable.Rows.Add(row);

            return dtTable;
        }
	
        public static string FormatSize(this Int64 bytes)
        {
            string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
	
        #region " GenericToDataTable "
		
        //public static DataTable ConvertTo<T>(this IList<T> lst)
        //{ //https://stackoverflow.com/a/45138154
        //    DataTable tbl = CreateTable<T>();
        //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        //    foreach (T item in lst)
        //    {
        //        DataRow row = tbl.NewRow();
        //        foreach (PropertyDescriptor prop in properties)
        //        {
        //            if (!prop.PropertyType.IsGenericType || !(prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //            {
        //                row[prop.Name] = prop.GetValue(item);
        //            }
        //        }
        //        tbl.Rows.Add(row);
        //    }
        //    return tbl;
        //}

        //private static DataTable CreateTable<T>()
        //{   
        //    Type expr_0A = typeof(T);
        //    DataTable tbl = new DataTable(expr_0A.Name);
        //    foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(expr_0A))
        //    {
        //        if (!prop.PropertyType.IsGenericType || !(prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //        {
        //            tbl.Columns.Add(prop.Name, prop.PropertyType);
        //        }
        //    }
        //    return tbl;
        //}

        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> source)
        {   // https://stackoverflow.com/a/6784997
            DataTable dt = new DataTable();

            var props = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in props)
            {
                DataColumn dc = dt.Columns.Add(prop.Name, prop.PropertyType);
                dc.Caption = prop.DisplayName;
                dc.ReadOnly = prop.IsReadOnly;
            }

            foreach (T item in source)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor prop in props)
                    dr[prop.Name] = prop.GetValue(item);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable ConvertToDataTableX<T>(this IList<T> data)
        {   // the ConvertTo+CreateTable should deprecated
            var properties = TypeDescriptor.GetProperties(typeof(T));

            var dt = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in data)
            {
                var row = dt.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion
		
	public static T ToEnum<T>(this object value)
	{
		return (T)((object)Enum.Parse(typeof(T), value.ToString(), true));
	}

	public static bool IsNull<T>(this T obj)
	{
		return object.Equals(obj, null);
	}

	public static bool IsEmpty(this object value)
	{
		if (value == null)
			return true;

		return string.IsNullOrEmpty(value.ToString());
	}
	
        public static string Serialize<T>(T obj)
        {
            XmlSerializer xs = null;
            StringWriter sw = null;
            try
            {
                xs = new XmlSerializer(typeof(T));
                sw = new StringWriter();
                xs.Serialize(sw, obj);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public static T Deserialize<T>(string XML)
        {
            XmlSerializer xs = null;
            StringReader sr = null;
            try
            {
                xs = new XmlSerializer(typeof(T));
                sr = new StringReader(XML);
                return (T)xs.Deserialize(sr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
	
        public static string JSONSerialize<T>(T obj)
        { //https://stackoverflow.com/a/35452673
            string retVal = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(ms, obj);
                var byteArray = ms.ToArray();
                retVal = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            }
            return retVal;
        }
	
	public static IEnumerable<T> ToGenericList<T>(this DataTable dataTable)
	{ // https://stackoverflow.com/a/59214115
		var properties = typeof(T).GetProperties().Where(x => x.CanWrite).ToList();

		var result = new List<T>();

		// loop on rows
		foreach (DataRow row in dataTable.Rows)
		{
			// create an instance of T generic type.
			var item = Activator.CreateInstance<T>();

			// loop on properties and columns that matches properties
			foreach (var prop in properties)
				foreach (DataColumn column in dataTable.Columns)                    
					if (prop.Name == column.ColumnName)
					{
						// Get the value from the datatable cell
						object value = row[column.ColumnName];

						// Set the value into the object
						prop.SetValue(item, value);
						break;
					}


			result.Add(item);
		}

		return result;
	}
	
	public static T DeepCopy<T>(this T source)
	{ //https://stackoverflow.com/a/40017705 w/ Newtonsoft.Json.dll, you can achieve the same w/o.
		return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
	}

	public static IList<T> Clone<T>(this IList<T> listToClone) where T: ICloneable
	{ //https://stackoverflow.com/a/222640
		return listToClone.Select(item => (T)item.Clone()).ToList();
		/*
			The class must implementing  : ICloneable
			public object Clone()
			{ //https://docs.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone
				return this.MemberwiseClone();
			}

			then use x.ToList().Clone(); will return an IList<T>
			
			more https://www.pipiscrew.com/2020/09/linq-clone-common-linq-mistakes/
		*/
	}
	
        public static T Convert<T>(this string input)
        {   //https://stackoverflow.com/a/26135533
            //System.Int32, System.Boolean, System.DateTime
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                //Cast ConvertFromString(string text) : object to (T)
                return (T)converter.ConvertFromString(input);
            }
            return default(T);
        }
	
        public static string ToJson(this object obj)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return serializer.Serialize(obj);
        }
	
        public static string RemoveDiacritics(this string text)
        {   //http://www.levibotelho.com/development/c-remove-diacritics-accents-from-a-string/
	    //Doesnt work inside LINQ
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
	
        public static string LeaveOnlyDigits(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
		
            Regex r = new Regex(@"[^\d]+");
            return  r.Replace(s, "");
        }
	
	    public static string LeaveOnlyAlphaNumeric(this string filename)
	    { //AlphaNumeric space and dash
	     return Regex.Replace(filename, @"[^\p{IsGreek}a-zA-Z0-9 -]", string.Empty);
	    }
	
        public static string ExRemoveAccents(this string text)
        {
            if (text.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder(text.Normalize(NormalizationForm.FormD));
            for (int i = 0; i < stringBuilder.Length; i++)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(stringBuilder[i]) == UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Remove(i, 1);
                }
            }
            return stringBuilder.ToString();
        }
	
        public static IEnumerable<string> TakeEvery(this string s, int count)
        {   // src - https://stackoverflow.com/a/4926466
            int index = 0;
            while (index < s.Length)
            {
                if (s.Length - index >= count)
                {
                    yield return s.Substring(index, count);
                }
                else
                {
                    yield return s.Substring(index, s.Length - index);
                }
                index += count;
            }
        }
	
        public static List<List<T>> SplitPer<T>(this List<T> collection, int size)
        {   // https://codereview.stackexchange.com/a/90198 + https://codereview.stackexchange.com/a/90531
            var chunks = new List<List<T>>();
            var chunkCount = collection.Count() / size;

            if (collection.Count % size > 0)
                chunkCount++;

            for (var i = 0; i < chunkCount; i++)
                chunks.Add(collection.Skip(i * size).Take(size).ToList());

            return chunks;
        }

        public static int[] FindAllIndex<T>(this T[] array, Predicate<T> match)
        {//https://stackoverflow.com/a/15295523
            return array.Select((value, index) => match(value) ? index : -1)
                    .Where(index => index != -1).ToArray();
        }

        public static string Replace(this Match match, string source, string replacement)
        { //https://stackoverflow.com/a/44955641
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
			
            /* use as - for (int i = 0; i < m.Count; i++)
				inputFormat = m[i].Replace(inputFormat, "{" + i.ToString() + "}"); */
        }
	
        /// <summary>
        ///    EPPlus - Extracts the first ExcelWorksheet by ExcelPackage to DataTable.
        /// </summary>
        public static DataTable ToDataTable(this ExcelPackage package)
        {   // https://stackoverflow.com/a/37795129
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
		
        /// <summary>
        ///    EPPlus - Extracts a DataTable from the ExcelWorksheet.
        /// </summary>
        public static DataTable ToDataTable(this ExcelWorksheet worksheet, bool hasHeaderRow = true)
        { // https://github.com/eraydin/EPPlus.Core.Extensions/blob/f220b5d6083ea5fc5c798aa9d54f50ff3b6a023e/src/EPPlus.Core.Extensions/ExcelWorksheetExtensions.cs
            ExcelAddress dataBounds = worksheet.GetDataBounds(hasHeaderRow);

            var dataTable = new DataTable(worksheet.Name);

            if (dataBounds == null)
            {
                return dataTable;
            }

            IEnumerable<DataColumn> columns = worksheet.AsExcelTable(hasHeaderRow).Columns.Select(x => new DataColumn(!hasHeaderRow ? "Column" + x.Id : x.Name));

            dataTable.Columns.AddRange(columns.ToArray());

            for (int rowIndex = dataBounds.Start.Row; rowIndex <= dataBounds.End.Row; ++rowIndex)
            {
                ExcelRangeBase[] inputRow = worksheet.Cells[rowIndex, dataBounds.Start.Column, rowIndex, dataBounds.End.Column].ToArray();
                DataRow row = dataTable.Rows.Add();

                for (var j = 0; j < inputRow.Length; ++j)
                {
                    row[j] = inputRow[j].Value;
                }
            }

            return dataTable;
        }
}
