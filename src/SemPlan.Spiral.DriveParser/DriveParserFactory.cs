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


namespace SemPlan.Spiral.DriveParser {
  using System;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
	/// <summary>
	/// Represents a factory for making RDF parsers
	/// </summary>
  /// <remarks>
  /// $Id: DriveParserFactory.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class DriveParserFactory : ParserFactory {
    private Dereferencer itsDereferencer;
    
    public DriveParserFactory() {
      itsDereferencer = new SimpleDereferencer();
    }
    
    public Parser MakeParser(ResourceFactory resourceFactory, StatementFactory statementFactory) {
      DriveParser parser = new DriveParser(resourceFactory, statementFactory);
      parser.SetDereferencer( itsDereferencer );
      return parser;
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
