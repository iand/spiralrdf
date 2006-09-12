#region Copyright (c) 2004 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2004 Ian Davis and James Carlyle

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in 
the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
of the Software, and to permit persons to whom the Software is furnished to do 
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
------------------------------------------------------------------------------*/
#endregion


using System;

namespace SemPlan.Spiral.XsltParser
{
	/// <summary>
	/// Represents available datatypes for typed literal nodes	
	/// </summary>
	public abstract class DataType
	{
		public DataType()
		{
		}
		
		public static Object getLiteralObject(Type dataType, string lexicalValue) {
			if (dataType == typeof(StringDataType))
				return lexicalValue;
			else if (dataType == typeof(BooleanDataType))
				return Boolean.Parse(lexicalValue);
			else if (dataType == typeof(StringDataType))
				return Decimal.Parse(lexicalValue);
			else return null;
		}
		
		public static Type parseDataType(string dataType) {
			if (dataType == @"http://www.w3.org/2001/XMLSchema#string")
				return typeof(StringDataType);
			else if (dataType == @"http://www.w3.org/2001/XMLSchema#boolean")
				return typeof(BooleanDataType);
			else if (dataType == @"http://www.w3.org/2001/XMLSchema#decimal")
				return typeof(DecimalDataType);
			else return null;
		}
	}
	
	public class StringDataType : DataType
	{
		new public static string ToString()
		{
			return @"http://www.w3.org/2001/XMLSchema#string";
		}
	}

	public class BooleanDataType : DataType
	{
		new public static string ToString()
		{
			return @"http://www.w3.org/2001/XMLSchema#boolean";
		}
	}

	public class DecimalDataType : DataType
	{
		new public static string ToString()
		{
			return @"http://www.w3.org/2001/XMLSchema#decimal";
		}
	}
}
