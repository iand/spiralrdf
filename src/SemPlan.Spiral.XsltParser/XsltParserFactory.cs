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


namespace SemPlan.Spiral.XsltParser {
using System;
using SemPlan.Spiral.Core;
using SemPlan.Spiral.Utility;
/// <summary>
/// Represents a factory for making RDF parsers
/// </summary>
/// <remarks>
/// $Id: XsltParserFactory.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
///</remarks>
public class XsltParserFactory : ParserFactory {
  private XsltTransformer itsXsltTransformer;
  private XsltTransformer itsValidatorXsltTransformer;
  private Dereferencer itsDereferencer;
  private const string DefaultXsltStylesheetName = @"./RdfXmlToTriples.xsl";
  private const string DefaultValidatorXsltStylesheetName = @"./RdfXmlValidator.xsl";
 
  public XsltParserFactory(string xsltStylesheetName, string validatorXsltStylesheetName, Dereferencer dereferencer) {
    itsXsltTransformer = new XsltTransformer(xsltStylesheetName);
    itsValidatorXsltTransformer = new XsltTransformer(validatorXsltStylesheetName);
    itsDereferencer  = dereferencer;
  }
  
  public XsltParserFactory() : this(DefaultXsltStylesheetName, DefaultValidatorXsltStylesheetName, new SimpleDereferencer()) {}
  
	public Parser MakeParser(SemPlan.Spiral.Core.ResourceFactory resourceFactory, SemPlan.Spiral.Core.StatementFactory statementFactory) {
    return new XsltParser(resourceFactory, statementFactory, itsXsltTransformer, itsValidatorXsltTransformer, itsDereferencer);
	}
  
    /// <summary>
    /// Set the Dereferencer to be used to dereference URIs
    /// </summary>
    public void SetDereferencer(Dereferencer dereferencer) {
      itsDereferencer = dereferencer;
    }  

    public Dereferencer GetDereferencer() {
      return itsDereferencer;
    }  

  }
}
