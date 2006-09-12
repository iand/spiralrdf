#region Copyright (c) 2006 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2006 Ian Davis and James Carlyle

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

namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO; 
  using System.Text;
  using System.Xml;
  using System.Collections;

	/// <summary>
	/// Represents something that can select a parser from a given set of criteria
	/// </summary>
  /// <remarks> 
  /// $Id: ParserSelecter.cs,v 1.1 2006/01/10 12:26:53 ian Exp $
  ///</remarks>
  public class ParserSelecter {
    private Hashtable itsParserFactories;
    private ResourceFactory itsResourceFactory;
    private StatementFactory itsStatementFactory;
  
    public ParserSelecter(ResourceFactory resourceFactory, StatementFactory statementFactory) {
      itsParserFactories = new Hashtable();
      itsStatementFactory = statementFactory;
      itsResourceFactory = resourceFactory;
    }

    public void RegisterUrlPattern( string urlPattern, ParserFactory factory) {
      itsParserFactories[urlPattern] = factory;
    }

    public Parser GetParserForUrl( string url ) {
      ParserFactory factory = (ParserFactory)itsParserFactories[url];
      
      return factory.MakeParser(itsResourceFactory, itsStatementFactory);
    }

  }

}