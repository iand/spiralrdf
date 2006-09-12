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
	/// Represents a triple pattern for a query
	/// </summary>
  /// <remarks>
  /// $Id: Pattern.cs,v 1.6 2006/02/13 23:42:49 ian Exp $
  ///</remarks>

  public class Pattern : Atom {
    private PatternTerm itsSubject;
    private PatternTerm itsPredicate;
    private PatternTerm itsObject;

    public Pattern(PatternTerm theSubject, PatternTerm  thePredicate, PatternTerm theObject) {
      if ( null == theSubject ) throw new ArgumentNullException("theSubject");
      if ( null == thePredicate ) throw new ArgumentNullException("thePredicate");
      if ( null == theObject ) throw new ArgumentNullException("theObject");
      itsSubject = theSubject;
      itsPredicate = thePredicate;
      itsObject = theObject;
    }
  
    public PatternTerm GetSubject() {
      return itsSubject;  
    }

    public PatternTerm GetPredicate() {
      return itsPredicate;  
    }

    public PatternTerm GetObject() {
      return itsObject;  
    }

    public override string ToString() {
      return GetSubject().ToString() + " " + GetPredicate().ToString() + " " + GetObject().ToString() + " .";
    }

    public override bool Equals(object other) {
      if (null == other) return false;
      if (this == other) return true;
      
      if (other is Pattern) {
        Pattern otherSpecific = (Pattern)other;
        
        return ( GetSubject() is BlankNode || GetSubject().Equals(otherSpecific.GetSubject()) )
        
        && ( GetPredicate() is BlankNode || GetPredicate().Equals(otherSpecific.GetPredicate()) )
        
        && (GetObject() is BlankNode || GetObject().Equals(otherSpecific.GetObject()) );
      }
      return false;
    }

    public override int GetHashCode() {
      return ToString().GetHashCode();
    }


    public Pattern Substitute( Bindings bindings ) {
      PatternTerm substituteSubject = itsSubject;
      PatternTerm substitutePredicate = itsPredicate;
      PatternTerm substituteObject = itsObject;
     
      if ( bindings.Contains( itsSubject ) ) {
        substituteSubject = (PatternTerm)bindings.Get( itsSubject );
      }        

      if ( bindings.Contains( itsPredicate ) ) {
        substitutePredicate = (PatternTerm)bindings.Get( itsPredicate );
      }        
      if ( bindings.Contains( itsObject ) ) {
        substituteObject = (PatternTerm)bindings.Get( itsObject );
      }        
      return new Pattern( substituteSubject, substitutePredicate, substituteObject);
    }
  

    public PatternTerm MakeTerm( PatternTerm member, ResourceMap map ) {
      if (  member is Variable ) {
        //~ itsVariables[ ((Variable)member).Name ] = member;
        return (Variable)member; 
      }
      else {
        if ( map.HasResourceDenotedBy( (GraphMember)member) ) {
          return map.GetResourceDenotedBy( (GraphMember)member );
        }
        else {
          return null;
        }
      }
    }

    
    public Pattern Resolve(ResourceMap map) {
      PatternTerm theSubject = MakeTerm( GetSubject(), map );
      if ( theSubject == null ) {
        throw new UnknownGraphMemberException( GetSubject() );
      }
      

      PatternTerm  thePredicate = MakeTerm( GetPredicate(), map );
      if ( thePredicate == null ) {
        throw new UnknownGraphMemberException( GetSubject() );
      }

      PatternTerm  theObject = MakeTerm( GetObject(), map );
      if ( theObject == null ) {
        throw new UnknownGraphMemberException( GetSubject() );
      }

      return new Pattern( theSubject, thePredicate, theObject );

    }    
  
    public Bindings Unify(Atom other, Bindings bindings, ResourceMap map) {
      if ( null == bindings) {
        return null;
      }
      else if (Equals(other) ) {
        return bindings;
      }
      else {
        return null;
      }
    
    }


  }
}