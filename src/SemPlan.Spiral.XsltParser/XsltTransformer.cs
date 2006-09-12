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
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;

namespace SemPlan.Spiral.XsltParser
{

public class XsltTransformer  {
	
	XsltArgumentList paramList;
	XslTransform xslTransform;
	StreamWriter writer;
	
	public XsltTransformer() {
		xslTransform = new XslTransform();
	}
	
	public XsltTransformer(string stylesheetFilename) {
		xslTransform = new XslTransform();
		xslTransform.Load(stylesheetFilename, null);
	}
	
	public MemoryStream TransformContentWithBaseUri(XPathDocument documentToTransform, string baseUri) 
	{	
		MemoryStream stream = new MemoryStream(2000);
		try
		{
			paramList = new XsltArgumentList();
			paramList.AddParam("base", "", baseUri);
			writer = new StreamWriter(stream);
			xslTransform.Transform(documentToTransform, paramList, writer, null);
		} 
		catch (Exception e) 
		{
			Console.Write(e.Message);
		}
		stream.Position = 0;
		return stream;
	}
		
	public MemoryStream TransformContent(XPathDocument documentToTransform) 
	{	
		MemoryStream stream = new MemoryStream(2000);
		try
		{
			writer = new StreamWriter(stream);
			xslTransform.Transform(documentToTransform, null, writer, null);
		} 
		catch (Exception e) 
		{
			Console.Write(e.Message);
		}
		stream.Position = 0;
		return stream;
	}
		
}
}
