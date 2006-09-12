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
  using System.Collections;
  using System.Text;
	/// <summary>
	/// Represents something that can compare two bindings for ordering
	/// </summary>
  /// <remarks>
  /// $Id: BindingsComparer.cs,v 1.2 2006/02/16 15:25:24 ian Exp $
  ///</remarks>

  public class BindingsComparer : IComparer {
    private Expression itsOrderExpression;
    
    public BindingsComparer( Expression orderExpression ) {
      itsOrderExpression = orderExpression;
    }

    public int Compare( object x, object y) {
      if (null == x) throw new ArgumentException("x must not be null");
      if (null == y) throw new ArgumentException("y must not be null");
      if (! (x is Bindings ) ) throw new ArgumentException("x must be a Bindings");
      if (! (y is Bindings ) ) throw new ArgumentException("y must be a Bindings");


      object xValue = itsOrderExpression.Evaluate( (Bindings)x );
      object yValue = itsOrderExpression.Evaluate( (Bindings)y );

      if ( null == xValue  && null != yValue ) return -1;
      if ( null != xValue  && null == yValue ) return 1;
      if ( null == xValue  && null == yValue ) return 0;
      
      return String.Compare( xValue.ToString(), yValue.ToString() );
    }

  }

}