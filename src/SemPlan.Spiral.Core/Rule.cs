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
	/// Represents an inference rule. If all antecedent atoms are true then all consequent atoms must also be true.
	/// </summary>
  /// <remarks>
  /// $Id: Rule.cs,v 1.3 2006/02/07 01:11:14 ian Exp $
  ///</remarks>
  public class Rule {
    private ArrayList itsAntecedents;
    private ArrayList itsConsequents;
    
    public Rule() {
      itsAntecedents = new ArrayList();
      itsConsequents = new ArrayList();
    }
    
    public void AddAntecedent( Pattern atom ) {
      itsAntecedents.Add( atom );
    }
    
    public void AddConsequent( Pattern atom ) {
      itsConsequents.Add( atom );
    }

    public IList Antecedents { 
      get {
        ArrayList patternsCopy = new ArrayList();
        patternsCopy.AddRange( itsAntecedents );
        return patternsCopy;
      }
    }
   
    public IList Consequents { 
      get {
        ArrayList patternsCopy = new ArrayList();
        patternsCopy.AddRange( itsConsequents );
        return patternsCopy;
      }
    }

  }
}