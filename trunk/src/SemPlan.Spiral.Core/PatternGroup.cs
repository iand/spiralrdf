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
	/// Represents group of query patterns
	/// </summary>
  /// <remarks> 
  /// $Id: PatternGroup.cs,v 1.4 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class PatternGroup  {
    protected ArrayList itsPatterns;
    protected ArrayList itsAlternateGroups;
    protected PatternGroup itsOptionalPatterns;
    protected ArrayList itsConstraints;
    protected QueryGroupPatterns itsQueryPartPatterns;
    protected QueryGroupOr itsQueryPartAlternate;
    protected QueryGroupPatterns itsQueryPartOptionalPatterns;
    protected QueryGroupOptional itsQueryPartOptional;
    protected QueryGroupConstraints itsQueryPartConstraints;
    
    public PatternGroup() {
      itsPatterns = new ArrayList();
      itsAlternateGroups = new ArrayList();
      itsConstraints = new ArrayList();
      itsQueryPartAlternate = new QueryGroupOr();
      itsQueryPartPatterns = new QueryGroupPatterns();
      itsQueryPartOptionalPatterns = new QueryGroupPatterns();
      itsQueryPartOptional = new QueryGroupOptional( itsQueryPartOptionalPatterns );
      itsQueryPartConstraints = new QueryGroupConstraints(  );
    }
        
    public virtual void AddPattern( Pattern  pattern ) {
      itsPatterns.Add( pattern );
      itsQueryPartPatterns.Add( pattern );
    }
    
    public virtual void AddAlternateGroup( PatternGroup group ) {
      itsAlternateGroups.Add( group );
    }

    public virtual void AddConstraint( Constraint constraint ) {
      itsConstraints.Add( constraint );
      itsQueryPartConstraints.Add( constraint );
    }


    public virtual IList Patterns{ 
      get {
        ArrayList patternsCopy = new ArrayList();
        patternsCopy.AddRange( itsPatterns );
        return patternsCopy;
      }
    }

    public virtual IList AlternateGroups { 
      get {
        ArrayList patternsCopy = new ArrayList();
        patternsCopy.AddRange( itsAlternateGroups );
        return patternsCopy;
      }
    }

    public virtual PatternGroup OptionalGroup { 
      get {
        if ( null == itsOptionalPatterns) {
          itsOptionalPatterns = new PatternGroup();
        }
        return itsOptionalPatterns;
      }
      set {
        itsOptionalPatterns = value;
      }
    }

    public virtual IList Constraints { 
      get {
        ArrayList constraintsCopy = new ArrayList();
        constraintsCopy.AddRange( itsConstraints );
        return constraintsCopy;
      }
    }

    public virtual bool HasOptionalPatterns { 
      get {
        if ( null != itsOptionalPatterns) {
          if( itsOptionalPatterns.HasPatterns ) {
            return true;
          }
        }
        return false;
      }
    }

    public virtual bool HasPatterns { 
      get {
        if (itsPatterns.Count > 0) return true;
        
        if ( null != itsOptionalPatterns) {
          if( itsOptionalPatterns.HasPatterns ) {
            return true;
          }
        }
        
        foreach (PatternGroup group in itsAlternateGroups) {
          if (group.HasPatterns) return true;
        }

        return false;
      }
    }

    public override string ToString() {
      StringBuilder buffer = new StringBuilder();
      
      buffer.Append("{");
    
      if ( itsPatterns.Count > 0 ) {
        if ( itsAlternateGroups.Count > 0 ) {
          buffer.Append("{");
          buffer.Append( Environment.NewLine );
        }
        buffer.Append( Environment.NewLine );
        foreach (Pattern pattern in itsPatterns) {
          buffer.Append( "  " );
          buffer.Append( pattern );
          buffer.Append( Environment.NewLine );
        }
        if ( itsAlternateGroups.Count > 0 ) {
          buffer.Append("}");
          buffer.Append( Environment.NewLine );
        }
      }
      else {
        buffer.Append( " " );
      }
      
      if ( itsAlternateGroups.Count > 0 ) {
        foreach (PatternGroup group in itsAlternateGroups) {
          buffer.Append(" UNION ");
          buffer.Append( Environment.NewLine );
          buffer.Append( group );
          buffer.Append( Environment.NewLine );
        }
      }
      
      if ( null != itsOptionalPatterns ) {
        buffer.Append(" OPTIONAL ");
        buffer.Append( Environment.NewLine );
        buffer.Append( itsOptionalPatterns );
        buffer.Append( Environment.NewLine );
      }
      
      if ( itsConstraints.Count > 0 ) {
        foreach (Constraint constraint in itsConstraints) {
          buffer.Append(" FILTER ");
          buffer.Append( constraint );
          buffer.Append( Environment.NewLine );
        }
      }
      buffer.Append( "}" );

      return buffer.ToString();
    }   


    /// <summary>Determines whether two PatternGroup instances are equal.</summary>
  	/// <returns>True if the two PatternGroups are equal, False otherwise</returns>
    public override bool Equals(object other) {
      if (null == other) return false;
      if (this == other) return true;
      
      
      PatternGroup specific = (PatternGroup)other;     
      
      if ( itsPatterns.Count != specific.itsPatterns.Count) return false;
      if ( itsAlternateGroups.Count != specific.itsAlternateGroups.Count) return false;
      if ( itsConstraints.Count != specific.itsConstraints.Count) return false;
      if ( null == itsOptionalPatterns ) {
        if ( null != specific.itsOptionalPatterns ) return false;
      }
      else {
        if (! itsOptionalPatterns.Equals( specific.itsOptionalPatterns) ) return false;
      }
      
      IList otherPatterns = specific.itsPatterns;
      foreach (Pattern pattern in itsPatterns) {
        if (! otherPatterns.Contains( pattern ) ) {
          return false;
        }
      }
      
      IList otherAlternateGroups = specific.itsAlternateGroups;
      foreach (PatternGroup group in itsAlternateGroups) {
        if (! otherAlternateGroups.Contains( group ) ) {
          return false;
        }
      }
      
      IList otherConstraints = specific.itsConstraints;
      foreach (Constraint constraint in itsConstraints) {
        if (! otherConstraints.Contains( constraint ) ) {
          return false;
        }
      }

      return true;
    }


    public override int GetHashCode() {
      int hashcode = 12345678;

      hashcode = hashcode >> 1;
      if ( null != itsOptionalPatterns) {
        hashcode = hashcode ^ itsOptionalPatterns.GetHashCode();
      }

      foreach (Pattern pattern in itsPatterns) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ pattern.GetHashCode();
      }

      foreach (PatternGroup group in itsAlternateGroups) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ group.GetHashCode();
      }
      
      foreach (Constraint constraint in itsConstraints) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ constraint.GetHashCode();
      }

      return hashcode;
    }

    public IList GetMentionedVariables() {
      ArrayList variables = new ArrayList();
      
      foreach (Pattern pattern in Patterns) {
        if (pattern.GetSubject() is Variable) {
          variables.Add( pattern.GetSubject() );
        }
        if (pattern.GetPredicate() is Variable) {
          variables.Add( pattern.GetPredicate() );
        }
        if (pattern.GetObject() is Variable) {
          variables.Add( pattern.GetObject() );
        }
      }
      

      if ( null != itsOptionalPatterns ) {
        variables.AddRange( itsOptionalPatterns.GetMentionedVariables() );
      }

      foreach (PatternGroup group in itsAlternateGroups) {
        variables.AddRange( group.GetMentionedVariables() );
      }

      return variables;
    }    
    
    public QueryGroup Resolve(TripleStore triples) {
      QueryGroupAnd groupAnd = new QueryGroupAnd();
      
      QueryGroupPatterns groupRequired = new QueryGroupPatterns();
      foreach (Pattern pattern in itsPatterns) {
        groupRequired.Add( pattern.Resolve( triples ) );
      }
      groupAnd.Add( groupRequired );

      if ( null != itsOptionalPatterns) {
        try {
          QueryGroupPatterns groupOptional = new QueryGroupPatterns();
          foreach (Pattern pattern in itsOptionalPatterns.Patterns) {
            groupOptional.Add( pattern.Resolve( triples ) );
          }
          groupAnd.Add( new QueryGroupOptional( groupOptional ) );
        }
        catch (UnknownGraphMemberException) {
          // NOOP
        }        
      }

      QueryGroupConstraints groupConstraints = new QueryGroupConstraints();
      foreach (Constraint constraint in itsConstraints) {
        groupConstraints.Add( constraint );
      }
      groupAnd.Add( groupConstraints );

      QueryGroupOr alternateGroups = new QueryGroupOr();
      alternateGroups.Add( groupAnd );
      
      
      foreach (PatternGroup group in itsAlternateGroups) {
        try {
          QueryGroupPatterns groupAlternate = new QueryGroupPatterns();
          foreach (Pattern pattern in group.Patterns) {
            groupAlternate .Add( pattern.Resolve( triples ) );
          }
          alternateGroups.Add( groupAlternate  );
        }
        catch (UnknownGraphMemberException) {
          // NOOP
        }        
      }
      
      return alternateGroups;
    }

    public Pattern First() {
      return (Pattern)itsPatterns[0];
    }

    public PatternGroup Rest() {
      PatternGroup rest = new PatternGroup();
      rest.itsPatterns.AddRange( itsPatterns.GetRange(1, itsPatterns.Count - 1) );
      rest.OptionalGroup = itsOptionalPatterns;
      rest.itsAlternateGroups.AddRange( itsAlternateGroups );
      rest.itsConstraints.AddRange(itsConstraints);
      
      return rest;
    }

 }
}