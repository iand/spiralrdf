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

namespace SemPlan.Spiral.Core {
  using System;
	/// <summary>
	/// Represents a factory that creates statements
	/// </summary>
  /// <remarks>
  /// $Id: StatementFactory.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class StatementFactory  {
    public virtual Statement MakeStatement(UriRef subject, UriRef predicate, UriRef value) {
      return new Statement( subject, predicate, value);
    }
    
    public virtual Statement MakeStatement(BlankNode subject, UriRef predicate, UriRef value) {
      return new Statement( subject, predicate, value);
    }
    
    public virtual Statement MakeStatement(UriRef subject, UriRef predicate, BlankNode value) {
      return new Statement( subject, predicate, value);
    }
    
    public virtual Statement MakeStatement(BlankNode subject, UriRef predicate, BlankNode value) {
      return new Statement( subject, predicate, value);
    }

    public virtual Statement MakeStatement(UriRef subject, UriRef predicate, PlainLiteral value) {
      return new Statement( subject, predicate, value);
    }
    
    public virtual Statement MakeStatement(BlankNode subject, UriRef predicate, PlainLiteral value) {
      return new Statement( subject, predicate, value);
    }

    public virtual Statement MakeStatement(UriRef subject, UriRef predicate, TypedLiteral value) {
      return new Statement( subject, predicate, value);
    }
    
    public virtual Statement MakeStatement(BlankNode subject, UriRef predicate, TypedLiteral value) {
      return new Statement( subject, predicate, value);
    }

  }
}