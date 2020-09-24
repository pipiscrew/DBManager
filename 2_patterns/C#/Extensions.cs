using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

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

	public static string ToRows2CSV(this DataTable dataTable,string delimeter)
	{
		IEnumerable<string> items = dataTable.AsEnumerable().Select(row => row.Field<string>(0)); ;

		if (items == null)
			return "";

		return string.Join(delimeter, items);

		//if you construct a WHERE clause
		//v = .ToRows2CSV("','");, afterwards you need to adjust it v = string.Format("{0}{1}{0}", "'", v);
	}
	

        #region " GenericToDataTable "
        //https://stackoverflow.com/a/45138154
		
        public static DataTable ConvertTo<T>(this IList<T> lst)
        {
            DataTable tbl = CreateTable<T>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (!prop.PropertyType.IsGenericType || !(prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        row[prop.Name] = prop.GetValue(item);
                    }
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }

        private static DataTable CreateTable<T>()
        {
            Type expr_0A = typeof(T);
            DataTable tbl = new DataTable(expr_0A.Name);
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(expr_0A))
            {
                if (!prop.PropertyType.IsGenericType || !(prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    tbl.Columns.Add(prop.Name, prop.PropertyType);
                }
            }
            return tbl;
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
