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

namespace SemPlan.Spiral.Tests.Core {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
	/// <summary>
	/// An instrumented version of Node that counts calls to methods
	/// </summary>
  /// <remarks>
  /// $Id: NodeCounter.cs,v 1.4 2006/02/13 23:44:11 ian Exp $
  ///</remarks>
  public class NodeCounter : Node {

    public int GetLabelCalled = 0;
    public string GetLabel() {
      ++GetLabelCalled;
      return "";
    }

    public int WriteCalled = 0;
    public void Write(RdfWriter writer) {
      ++WriteCalled;
    }

    public int MatchesCalled = 0;
    public bool Matches(ResourceSpecifier specifier) {
      ++MatchesCalled;
      return false;
    }

    public int EquivalentToCalled = 0;
    public bool EquivalentTo(Node other) {
      ++EquivalentToCalled;
      return false;
    }

    public int IsSelfDenotingCalled = 0;
    public bool IsSelfDenoting() {
      ++IsSelfDenotingCalled;
      return false;
    }

    public Bindings Unify(Atom other, Bindings bindings, ResourceMap map) {
      if ( null == bindings) {
        return null;
      }
      else if (Equals(other) ) {
        return bindings;
      }
      else if (other is Variable) {
        return (( Variable)other).Unify( this, bindings, map);
      }
      else {
        return null;
      }
    
    }


  }
}