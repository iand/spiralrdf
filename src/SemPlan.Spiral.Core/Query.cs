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
	/// Represents a query on a TripleStore
	/// </summary>
  /// <remarks> 
  /// $Id: Query.cs,v 1.11 2006/02/16 15:25:24 ian Exp $
  ///</remarks>
  public class Query {
    private ArrayList itsVariables;
    private ArrayList itsDescribeTerms;
    private bool itsIsDistinct;
    private bool itsSelectAll;
    private PatternGroup itsPatternGroup;
    private string itsBase;
    private SortOrder itsOrderDirection;
    private Expression itsOrderBy;
    private Uri itsBaseUri;
    private int itsResultLimit;
    private int itsResultOffset;
    private ResultFormType itsResultForm;
    
    private QueryGroup itsQueryGroup;
    
    public enum SortOrder {
      Ascending, Descending
    }

    
    public enum ResultFormType {
      Select, Construct, Ask, Describe
    }
    
    public Query() {
      itsPatternGroup = new PatternGroup();
      itsVariables = new ArrayList();
      itsDescribeTerms = new ArrayList();
      itsIsDistinct = false;
      itsSelectAll = true;
      itsBase = String.Empty;
      itsOrderDirection = SortOrder.Ascending;
      itsOrderBy = new NullOrdering();
      itsResultLimit = Int32.MaxValue;
      itsResultForm = ResultFormType.Select;
      itsQueryGroup = null;
    }

    public QueryGroup QueryGroup {
    
      get {
        return itsQueryGroup;
      }

      set {
        itsQueryGroup = value;
      }

    }
    
    public QueryGroup ResolveQueryGroup(TripleStore triples) {
      if ( null == itsQueryGroup) {
        return itsPatternGroup.Resolve( triples );
      }
      else {
        return itsQueryGroup.Resolve( triples );
      }
      
    }

    public virtual void AddPattern( Pattern  pattern ) {
      itsPatternGroup.AddPattern( pattern );
    }
    
    public virtual void AddConstraint( Constraint constraint ) {
      itsPatternGroup.AddConstraint( constraint );
    }

    [Obsolete("Use PatternGroup.OptionalGroup.AddPattern")]
    public void AddOptionalPattern( Pattern  pattern ) {
      ((PatternGroup)itsPatternGroup.OptionalGroup).AddPattern( pattern );
    }

    public void AddVariable( Variable  variable ) {
      itsVariables.Add( variable );
      itsSelectAll = false;
    }
    
    public void AddDescribeTerm( PatternTerm term ) {
      itsDescribeTerms.Add( term );
      itsSelectAll = false;
    }


    [Obsolete("Use AddVariable and IsDistinct")]
    public void AddDistinct( Variable  variable ) {
      IsDistinct = true;
      AddVariable( variable );
    }

    public bool IsDistinct {
      get {
        return itsIsDistinct;
      }
      set {
        itsIsDistinct = value;
      }
    }

    public bool SelectAll {
      get {
        return itsSelectAll;
      }
      set {
        itsSelectAll = value;
      }
    }

    public PatternGroup PatternGroup {
      get {
        return itsPatternGroup;
      }
      set {
        itsPatternGroup = value;
      }
    }

    [Obsolete("Use PatternGroup.OptionalGroup.Patterns")]
    public IList OptionalPatterns{ 
      get {
        return itsPatternGroup.OptionalGroup.Patterns;
      }
    }

    public IList Variables{ 
      get {
        ArrayList listCopy = new ArrayList();
        if (SelectAll) {
          if ( null == itsQueryGroup) {
            listCopy.AddRange( PatternGroup.GetMentionedVariables() );
          }
          else {
            listCopy.AddRange( itsQueryGroup.GetMentionedVariables() );
          }
        }
        else {
          listCopy.AddRange( itsVariables );
        }
        return listCopy;
      }
    }

    public IList DescribeTerms{ 
      get {
        ArrayList listCopy = new ArrayList();
        listCopy.AddRange( itsDescribeTerms );
        return listCopy;
      }
    }


    public virtual bool HasPatterns {
      get { return PatternGroup.HasPatterns; }
    }


    [Obsolete("Use Variables")]
    public IList DistinctVariables{ 
      get {
        return Variables;
      }
    }

    [Obsolete("Use PatternGroup.Patterns")]
    public virtual IList Patterns { 
      get {
        return itsPatternGroup.Patterns;
      }
    }

    [Obsolete("Use PatternGroup.Constraints")]
    public virtual IList Constraints { 
      get {
        return itsPatternGroup.Constraints;
      }
    }

    public virtual string Base { 
      get {
        return itsBase;
      }
      set {
        itsBase = value;
        itsBaseUri = new Uri( itsBase );
      }
    }

    public virtual SortOrder OrderDirection {
      get {
        return itsOrderDirection;
      }
      set {
        itsOrderDirection = value;
      }
    }

    public virtual Expression OrderBy {
      get {
        return itsOrderBy;
      }
      set {
        itsOrderBy = value;
      }
    }

    public virtual bool IsOrdered {
      get {
        return !( itsOrderBy is NullOrdering );
      }
    }

    public virtual int ResultLimit {
      get { return itsResultLimit; }
      set { itsResultLimit = value; }
    }

    public virtual int ResultOffset {
      get { return itsResultOffset; }
      set { itsResultOffset = value; }
    }

    public virtual ResultFormType ResultForm {
      get { return itsResultForm;  }
      set { itsResultForm = value;  }
    }


    public override string ToString() {
      StringBuilder buffer = new StringBuilder();
      buffer.Append("A Query which ");
      if ( itsSelectAll ) {
        buffer.Append("selects all variables");
      }
      else {
        buffer.Append("does not select all variables");
      }

      if ( itsIsDistinct ) {
        buffer.Append(", is distinct");
      }
      else {
        buffer.Append(", is not distinct");
      }

      buffer.Append(", sorts ");
      buffer.Append( OrderDirection );
      buffer.Append( " by " );
      buffer.Append( OrderBy );

      if (Base.Length > 0) {
        buffer.Append(", has base URI of <");
        buffer.Append( Base );
        buffer.Append(">");
      }
      else {
        buffer.Append(", has no base URI");
      }

      buffer.Append(" and has " + itsPatternGroup.ToString() );
      
      return buffer.ToString();
    }   


    /// <summary>Determines whether two Query instances are equal.</summary>
  	/// <returns>True if the two Queries are equal, False otherwise</returns>
    public override bool Equals(object other) {
      if (null == other) return false;
      if (this == other) return true;
      if (! GetType().Equals(other.GetType() ) ) return false;
      
      Query specific = (Query)other;     
      
      if ( ResultLimit != specific.ResultLimit) return false;
      if ( ResultOffset != specific.ResultOffset) return false;
      if ( itsSelectAll != specific.itsSelectAll) return false;
      if ( itsVariables.Count != specific.itsVariables.Count) return false;
      if (! itsPatternGroup.Equals( specific.itsPatternGroup ) ) return false;
      if (! Base.Equals( specific.Base ) ) return false;
      if ( OrderDirection != specific.OrderDirection) return false;
      if (! OrderBy.Equals( specific.OrderBy ) ) return false;
      if ( ResultForm != specific.ResultForm) return false;

      if ( null == itsQueryGroup && null != specific.itsQueryGroup) return false;
      if ( null != itsQueryGroup && null == specific.itsQueryGroup) return false;
      if ( null != itsQueryGroup && null != specific.itsQueryGroup) {
        if (! itsQueryGroup.Equals( specific.itsQueryGroup ) ) {
          return false;
        }
      }

      
      IList otherVariables = specific.itsVariables;
      foreach (Variable variable in itsVariables) {
        if (! otherVariables.Contains( variable ) ) {
          return false;
        }
      }

      IList otherDescribeTerms = specific.itsDescribeTerms;
      foreach (PatternTerm term in itsDescribeTerms) {
        if (! otherDescribeTerms.Contains( term ) ) {
          return false;
        }
      }


      return true;
    }


    public override int GetHashCode() {
      int hashcode = ResultLimit + ResultOffset;

      hashcode = hashcode << 1;
      hashcode = hashcode ^ ( itsSelectAll ? 3 : 13);
      hashcode = hashcode ^ itsPatternGroup.GetHashCode();
      hashcode = hashcode ^ ( OrderDirection == SortOrder.Ascending ? 3 : 13 );
      hashcode = hashcode >> 1;
      hashcode = hashcode ^ ( ResultForm == ResultFormType.Select ? 3 : 13 );
      hashcode = hashcode << 1;
      hashcode = hashcode ^ ( ResultForm == ResultFormType.Describe ? 3 : 13 );
      hashcode = hashcode << 1;
      hashcode = hashcode ^ ( ResultForm == ResultFormType.Ask ? 3 : 13 );
      hashcode = hashcode << 1;
      hashcode = hashcode ^ ( ResultForm == ResultFormType.Construct ? 3 : 13 );
      hashcode = hashcode >> 1;
      hashcode = hashcode ^ Base.GetHashCode();
      hashcode = hashcode << 1;
      hashcode = hashcode ^ OrderBy.GetHashCode();

      if ( itsQueryGroup != null ) {
        hashcode = hashcode << 1;
        hashcode = hashcode ^ itsQueryGroup.GetHashCode();
      }

      foreach (Variable variable in itsVariables) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ variable.GetHashCode();
      }

      foreach (PatternTerm term in itsDescribeTerms) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ term.GetHashCode();
      }

      return hashcode;
    }


    public UriRef CreateUriRef(string text) {
      Uri uri;
      if (null != itsBaseUri) {
        uri = new Uri( itsBaseUri, text );
      }
      else {
        uri = new Uri( text );
      }
      return new UriRef( uri.ToString() );
    }


    /*************************************************************************/
    protected class NullOrdering : Expression {
      public object Evaluate(Bindings bindings) { return "unordered"; }
      
      public override bool Equals(object other) {
        if (null == other) return false;
        if (this == other) return false;
        return (GetType().Equals(other.GetType() ) );
      }
      
      public override int GetHashCode() { return 5; }
    }
 
 

  }



}