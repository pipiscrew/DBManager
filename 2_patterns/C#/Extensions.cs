using System;
using System.Collections.Generic;
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
}
