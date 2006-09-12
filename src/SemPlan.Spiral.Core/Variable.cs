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
	/// <summary>
	/// Represents a variable in a pattern
	/// </summary>
  /// <remarks>
  /// $Id: Variable.cs,v 1.4 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class Variable : PatternTerm {
    private string itsName;
    public Variable(string name) {
      itsName = name;
    }
  
    public string Name {
      get {
        return itsName;
      }
    }
  
    public override string ToString() {
      return "?" + itsName;
    }

    public string GetLabel() {
      return "?" + itsName;
    }

    public override bool Equals(Object other) {
      if ( this == other) return true;
      if (other is Variable) {
        return (itsName.Equals(((Variable)other).itsName));
      }
      
      return false;
    }

    public override int GetHashCode() {
      return itsName.GetHashCode();
    }


    public virtual Bindings Unify(Atom other, Bindings bindings, ResourceMap map) {
      if ( bindings.Contains( this )  ) {
        return ((Atom)bindings.Get(this)).Unify( other, bindings, map);
      }
      else if ( bindings.Contains( other )  ) {
        return Unify( (Atom)bindings.Get(other), bindings, map);
      }
      //~ else if ( VariableOccursInExpression( var, other) ) {
        //~ return null;
      //~ }
      else {
        Bindings cloned = (Bindings)bindings.Clone();
        cloned.Bind( this, other);
        if ( other is Resource) {
          cloned.BindNode( this, map.GetBestDenotingNode( (Resource)other ) );
        }
        return cloned;
      }
    }
  }
}