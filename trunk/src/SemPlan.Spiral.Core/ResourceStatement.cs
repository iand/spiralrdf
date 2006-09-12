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
  using SemPlan.Spiral.Utility;
  using System;
  using System.Collections;
  using System.IO;

  public class ResourceStatement : Atom {
    private Resource itsSubject;
    private Resource itsPredicate;
    private Resource itsObject;
    private int itsHashCode;
    
    public ResourceStatement(Resource theSubject, Resource thePredicate, Resource theObject) {
      itsSubject = theSubject;
      itsPredicate = thePredicate;
      itsObject = theObject;

      itsHashCode = itsSubject.GetHashCode();
      itsHashCode = itsHashCode >> 1;
      itsHashCode = itsHashCode ^ itsPredicate.GetHashCode();
      itsHashCode = itsHashCode >> 1;
      itsHashCode = itsHashCode ^ itsObject.GetHashCode();

    }
  
    public Resource GetSubject() {
      return itsSubject;  
    }

    public Resource GetPredicate() {
      return itsPredicate;  
    }

    public Resource GetObject() {
      return itsObject;  
    }
    
    public override string ToString() {
      return GetSubject().ToString() + " " + GetPredicate().ToString() + " " + GetObject().ToString() + " .";
    }

    public override bool Equals(object other) {
      if (this == other) return true;
      
      if (other is ResourceStatement) {
        if (GetHashCode() != other.GetHashCode() ) {
          return false;
        }

        ResourceStatement otherSpecific = (ResourceStatement)other;
        return (
          GetSubject().GetHashCode() == otherSpecific.GetSubject().GetHashCode() 
          && GetPredicate().GetHashCode() == otherSpecific.GetPredicate().GetHashCode() 
          && GetObject().GetHashCode() == otherSpecific.GetObject().GetHashCode() 
           );
      }

      return false;
    }

    public override int GetHashCode() {
      return itsHashCode;
    }

    public Bindings Unify(Atom y, Bindings bindings, ResourceMap map) {
      if ( null == bindings) {
        return null;
      }
      else if (Equals(y) ) {
        return bindings;
      }
      else if (y is Pattern) {
        return  GetObject().Unify( ((Pattern)y).GetObject(),        
            GetSubject().Unify( ((Pattern)y).GetSubject(),        
              GetPredicate().Unify( ((Pattern)y).GetPredicate(), bindings, map ) , map
            ) , map
          );     
      }
      else if (y is ResourceStatement) {
        return GetObject().Unify( ((ResourceStatement)y).GetObject(),        
            GetSubject().Unify(  ((ResourceStatement)y).GetSubject(),        
              GetPredicate().Unify(  ((ResourceStatement)y).GetPredicate(), bindings, map ), map
            ) , map
          );     
      }
      else {
        return null;
      }
    
    }

  }
}