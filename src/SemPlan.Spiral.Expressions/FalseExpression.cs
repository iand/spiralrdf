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

namespace SemPlan.Spiral.Expressions {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;

	/// <summary>
	/// Represents a constraint on query results
	/// </summary>
  /// <remarks> 
  /// $Id: FalseExpression.cs,v 1.2 2006/02/13 23:43:22 ian Exp $
  ///</remarks>
  public class FalseExpression : Expression {
    public object Argument {
      get { return false; }
    }
    
		public object Evaluate( Bindings bindings ) {
      return false;
    }
    
    public Expression Substitute( Hashtable bindings ) {
      return this;
    }
    
    public override bool Equals(object other) {
      if (null == other) return false;
      if (this == other) return true;
      
      return (GetType().Equals( other.GetType() ) );
    }

    public override int GetHashCode() {
      return 1;
    }
  }
}