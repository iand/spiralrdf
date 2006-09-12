#region Copyright (c) 2005 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2005 Ian Davis and James Carlyle

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
  using System.Globalization;
  
	/// <summary>
	/// Represents a constraint on query results
	/// </summary>
  /// <remarks> 
  /// $Id: Constraint.cs,v 1.5 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public class Constraint {
    private Expression itsExpression;
    private const string XSD_BOOLEAN = "http://www.w3.org/2001/XMLSchema#boolean";
    private const string XSD_STRING = "http://www.w3.org/2001/XMLSchema#string";
    private const string XSD_INTEGER = "http://www.w3.org/2001/XMLSchema#integer";
    private const string XSD_DECIMAL = "http://www.w3.org/2001/XMLSchema#decimal";
    private const string XSD_DOUBLE = "http://www.w3.org/2001/XMLSchema#double";
    private const string XSD_FLOAT = "http://www.w3.org/2001/XMLSchema#float";
    private static CaseInsensitiveComparer theComparer;

    static Constraint() {
      CultureInfo usCulture = new CultureInfo("en-US");
      theComparer  = new CaseInsensitiveComparer( usCulture );
    }

    public Constraint() {
    }
    public Constraint(Expression expression) {
      itsExpression = expression;
    }
    
    public Expression Expression {
      get { return itsExpression; }
    }
    
		public virtual bool SatisfiedBy( Bindings bindings ) {
      return EffectiveBooleanValue( itsExpression.Evaluate( bindings ) );
    }
    
    public bool EffectiveBooleanValue( object value) {
      if (value.Equals(true)) return true;
      if (value.Equals(false)) return false;
      
      if ( value is TypedLiteral) {
        TypedLiteral specific = (TypedLiteral)value;
        if ( specific.GetDataType().Equals( XSD_BOOLEAN )) {
          return ( 0 == theComparer.Compare("true", specific.GetLabel() ) );
        }
        else if ( specific.GetDataType().Equals( XSD_STRING )) {
          return ( 0 != specific.GetLabel().Length );
        }
        else if ( specific.GetDataType().Equals( XSD_INTEGER )) {
          return ( 0 != Convert.ToInt32( specific.GetLabel() ) );
        }
        else if ( specific.GetDataType().Equals( XSD_DECIMAL )) {
          return ( 0 != Convert.ToDecimal( specific.GetLabel() ) );
        }
        else if ( specific.GetDataType().Equals( XSD_DOUBLE )) {
          return ( 0.0 != Convert.ToDouble( specific.GetLabel() ) );
        }
        else if ( specific.GetDataType().Equals( XSD_FLOAT )) {
          return ( 0.0 != Convert.ToSingle( specific.GetLabel() ) );
        }
      }
      else if ( value is PlainLiteral) {
        return ( 0 != ((PlainLiteral)value).GetLabel().Length );
      }
      return false;
    }
    
    public override bool Equals(object other) {
      if (null == other) return false;
      if (this == other) return true;
      
      if (GetType().Equals( other.GetType() ) ) {
        return ( itsExpression.Equals( ((Constraint)other).itsExpression) );
      }
      
      return false;
    }

    public override string ToString() {
      return itsExpression.ToString();
    }

    public override int GetHashCode() {
      return itsExpression.GetHashCode();
    }
  }
}