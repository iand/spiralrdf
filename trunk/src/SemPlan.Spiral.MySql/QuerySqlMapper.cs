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

using MySql.Data.MySqlClient;

namespace SemPlan.Spiral.MySql {
  using System;
  using System.Text;
  using System.Collections;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using System.Data;

  /// <summary>
	/// Represents a mapping from a query to SQL
	/// </summary>
  /// <remarks>
  /// $Id: QuerySqlMapper.cs,v 1.2 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public class QuerySqlMapper : QueryGroupVisitor {
    private Query itsQuery;
    private ResourceMap itsResourceMap;

    private ArrayList itsSelectTerms;
    private ArrayList itsWhereTerms;
    private ArrayList itsJoins;
    private ArrayList itsVariableList;
    private int itsStatementTableCount;
    private int itsOptionalNestingDepth;
    
    private bool itsIsMappable;
    private bool itsIsFeasible;
    private string itsSql;
    private IDictionary itsResourcesIndexedByNode;
    private Hashtable itsSelectedVariables;
    private Hashtable itsVariableFirstMentions;
    private Hashtable itsVariableTables;
    private Hashtable itsNonLiteralVariables; // Stores whether a variable is used in subject or object of pattern which restricts possible node types

    public QuerySqlMapper(Query query, ResourceMap resourceMap) {
      itsQuery = query;
      itsResourceMap = resourceMap;

      itsIsMappable = true;
      itsIsFeasible = true;
      
      itsSelectTerms = new ArrayList();
      itsWhereTerms = new ArrayList();
      itsJoins = new ArrayList();
      itsStatementTableCount = 0;
      itsVariableFirstMentions = new Hashtable();
      itsVariableTables = new Hashtable();
      itsSelectedVariables = new Hashtable();
      itsNonLiteralVariables = new Hashtable();
      itsVariableList = new ArrayList();

      itsSql = "";
      
      QueryGroup group = itsQuery.QueryGroup;

      StringBuilder sql = new StringBuilder();
      

      
      if (! CollectResourcesForTerms(group) ) {
        NoSolutionIsPossible();
      }


      if ( itsIsFeasible ) {
        sql.Append( GenerateSelect( group ) );
      }
      
      itsSql = sql.ToString();
      
    }
    

    private string GenerateSelect(QueryGroup group) {
      group.Accept( this );

      if ( itsJoins.Count == 0 && itsWhereTerms.Count == 0) {
        itsIsFeasible = false;
        itsIsMappable = false;
        return string.Empty;
      }

      foreach (Variable variable in itsQuery.Variables) {
        if ( itsVariableFirstMentions.Contains( variable.Name ) ) {
          if (! itsSelectedVariables.Contains( variable.Name ) ) {
            itsVariableList.Add( variable );
            itsSelectedVariables[ variable.Name ] = variable;
            
            itsSelectTerms.Add( "rn_" + variable.Name + ".resourceHash rh_" + variable.Name);
            itsSelectTerms.Add( "rn_" + variable.Name + ".nodeHash nh_" + variable.Name );
            itsSelectTerms.Add( "rn_" + variable.Name + ".nodeType nt_" + variable.Name );
  
            itsJoins.Add("JOIN ResourceNodes rn_" + variable.Name + " ON rn_" + variable.Name + ".resourceHash=" + itsVariableFirstMentions[ variable.Name ] + " AND rn_" + variable.Name + ".graphId=" + itsVariableTables[ variable.Name ] + ".graphId");
            itsJoins.Add("LEFT OUTER JOIN UriRefs u_" + variable.Name + " ON rn_" + variable.Name + ".nodeHash=u_" + variable.Name + ".hash AND rn_" + variable.Name + ".nodeType='u'");
            
            if (! itsNonLiteralVariables.Contains( variable.Name ) ) {
              itsSelectTerms.Add( "COALESCE(u_" + variable.Name + ".uri, pl_" + variable.Name + ".value, l_" + variable.Name + ".value) val_" + variable.Name );
              itsSelectTerms.Add( "COALESCE(tl_" + variable.Name + ".value, t_" + variable.Name + ".value) sub_" + variable.Name );
            
              itsJoins.Add("LEFT OUTER JOIN PlainLiterals pl_" + variable.Name + " ON rn_" + variable.Name + ".nodeHash=pl_" + variable.Name + ".hash AND rn_" + variable.Name + ".nodeType='p'");
              itsJoins.Add("LEFT OUTER JOIN Languages l_" + variable.Name + " ON pl_" + variable.Name + ".languageHash=l_" + variable.Name + ".hash");
              itsJoins.Add("LEFT OUTER JOIN TypedLiterals tl_" + variable.Name + " ON rn_" + variable.Name + ".nodehash=tl_" + variable.Name + ".hash AND rn_" + variable.Name + ".nodeType='t'");
              itsJoins.Add("LEFT OUTER JOIN DataTypes t_" + variable.Name + " ON tl_" + variable.Name + ".datatypeHash=t_" + variable.Name + ".hash");
            }
            else {
              itsSelectTerms.Add( "u_" + variable.Name + ".uri val_" + variable.Name );
              itsSelectTerms.Add( "NULL sub_" + variable.Name );
            }
          }
        }
        else {
            itsSelectTerms.Add( "NULL rh_" + variable.Name );
            itsSelectTerms.Add( "NULL nh_" + variable.Name );
            itsSelectTerms.Add( "NULL nt_" + variable.Name );
            itsSelectTerms.Add( "NULL val_" + variable.Name );
            itsSelectTerms.Add( "NULL sub_" + variable.Name );
        }
      }
      

      return BuildSql();
    }    
    
    public string Sql {
      get {
        return itsSql;
      }
    }

    public bool IsMappable {
      get { return itsIsMappable; }
    }

    public bool IsFeasible {
      get { return itsIsFeasible; }
    }

    public void visit(QueryGroupAnd group) {
      foreach (QueryGroup subgroup in group.Groups) {
        subgroup.Accept( this );
      }
    }
    
    public void visit(QueryGroupConstraints group) {
      foreach (SemPlan.Spiral.Core.Constraint constraint in group.Constraints) {
        if ( constraint.Expression is IsLiteral && ((IsLiteral)constraint.Expression).SubExpression is VariableExpression) {
          string variableName = ((VariableExpression)((IsLiteral)constraint.Expression).SubExpression).Variable.Name;
          itsWhereTerms.Add("(nt_" + variableName + "='p' OR nt_" + variableName + "='t')"); 
        }
        else if ( constraint.Expression is IsIri && ((IsIri)constraint.Expression).SubExpression is VariableExpression) {
          string variableName = ((VariableExpression)((IsIri)constraint.Expression).SubExpression).Variable.Name;
          itsWhereTerms.Add("nt_" + variableName + "='u'"); 
        }
        else if ( constraint.Expression is IsBlank && ((IsBlank)constraint.Expression).SubExpression is VariableExpression) {
          string variableName = ((VariableExpression)((IsBlank)constraint.Expression).SubExpression).Variable.Name;
          itsWhereTerms.Add("nt_" + variableName + "='b'"); 
        }
        else if ( constraint.Expression is Bound) {
          string variableName = ((Bound)constraint.Expression).Variable.Name;
          if ( itsVariableFirstMentions.Contains( variableName ) ) {
            itsWhereTerms.Add(itsVariableFirstMentions[ variableName ] + " IS NOT NULL"); 
          }
          else {
            MarkAsNotFeasible(" Bound constraint applied to variable '" + variableName + "' before it is first mentioned");
          }
        }
        else {
          itsIsMappable = false;
        }
      }
    }
    
    public void visit(QueryGroupOptional group) {
      ++itsOptionalNestingDepth;
      group.Group.Accept( this );
      --itsOptionalNestingDepth;
    }
    
    public void visit(QueryGroupOr group) {
      foreach (QueryGroup subgroup in group.Groups) {
        subgroup.Accept( this );
      }
    }
    
    public void visit(QueryGroupPatterns group) {
      ArrayList groupJoins = new ArrayList();
      Hashtable variableMentionsWithinThisGroup = new Hashtable();
    
      bool allPatternsAreMatchable = true;
      foreach (Pattern pattern in group.Patterns) {
        if ( ! (pattern.GetSubject() is Variable) && ! itsResourcesIndexedByNode.Contains( pattern.GetSubject() ) ) {
          allPatternsAreMatchable = false;
        }
        else if ( ! (pattern.GetPredicate() is Variable) && ! itsResourcesIndexedByNode.Contains( pattern.GetPredicate() ) ) {
          allPatternsAreMatchable = false;
        }
        else if ( ! (pattern.GetObject() is Variable) && ! itsResourcesIndexedByNode.Contains( pattern.GetObject() ) ) {
          allPatternsAreMatchable = false;
        }

      }

      if ( allPatternsAreMatchable) {
        foreach (Pattern pattern in group.Patterns) {
          ++itsStatementTableCount;       
          string tableName = "s" + itsStatementTableCount;
          
          ArrayList constraints  = new ArrayList();
          bool referencesExternalVariables = false;
          if (ProcessPatternTerm( pattern.GetSubject(), tableName, "subjectHash", constraints, variableMentionsWithinThisGroup) ) {
            referencesExternalVariables =  true;
          }
          if ( ProcessPatternTerm( pattern.GetPredicate(), tableName, "predicateHash", constraints, variableMentionsWithinThisGroup) ) {
            referencesExternalVariables =  true;
          }
          if ( ProcessPatternTerm( pattern.GetObject(), tableName, "objectHash", constraints, variableMentionsWithinThisGroup) ) {
            referencesExternalVariables =  true;
          }
          
          constraints.Add( tableName + ".graphId=" + itsResourceMap.GetHashCode() );
  
          if (itsStatementTableCount > 1) {
            StringBuilder join = new StringBuilder();
            if ( referencesExternalVariables && itsOptionalNestingDepth > 0) {
              join.Append("LEFT OUTER ");        
            }
            join.Append("JOIN Statements " + tableName + " ON ");        
    
            bool doneFirst = false;
            foreach (string constraint in constraints) {
              if ( constraint != null && constraint.Length > 0) {
                if ( doneFirst ) {
                  join.Append(" AND ");
                }
                join.Append( constraint );
                doneFirst = true;
              }
            }
            groupJoins.Add( join.ToString() );
          }
          else {
            foreach (string constraint in constraints) {
              if ( constraint != null && constraint.Length > 0) {
                itsWhereTerms.Add( constraint );
              }
            }
          }        
        }
        itsJoins.AddRange( groupJoins );
      }
      else {
        if ( itsOptionalNestingDepth == 0 ) {
          NoSolutionIsPossible();
        }
      }


    }

    private bool ProcessPatternTerm(PatternTerm term, string tableName, string columnName, ArrayList constraints, Hashtable localVariableMentions) {
      bool referencesExternalVariables = false;
      if (term is Variable) {
        Variable variable = (Variable)term;
        if ( itsVariableFirstMentions.Contains( variable.Name ) ) {
          if ( localVariableMentions.Contains( variable.Name ) ) {
            constraints.Add(  tableName + "." + columnName + "=" + localVariableMentions[ variable.Name ] );
          }
          else {
            referencesExternalVariables = true;
            constraints.Add( tableName + "." + columnName + "=" + itsVariableFirstMentions[ variable.Name ] );
            localVariableMentions[ variable.Name ] = tableName + "." + columnName;
          }
        }
        else {
          itsVariableFirstMentions[ variable.Name ] = tableName + "." + columnName;
          localVariableMentions[ variable.Name ] = tableName + "." + columnName;
          itsVariableTables[ variable.Name ] = tableName;
        }
        
        if ( columnName.Equals("subjectHash") || columnName.Equals("predicateHash") ) {
          itsNonLiteralVariables[ variable.Name ] = variable;
        }
        
      }
      else {
        constraints.Add( tableName + "." + columnName + "=" + itsResourcesIndexedByNode[ term ].GetHashCode() );
      }
  
      return referencesExternalVariables;
    
    }


    private string BuildSql() {
      StringBuilder sql = new StringBuilder("SELECT ");
 
      if (itsQuery.IsDistinct) {
        sql.Append("DISTINCT ");
      }
 
      bool doneFirst = false;
      foreach (string term in itsSelectTerms) {
        if ( doneFirst ) {
          sql.Append(", ");
        }
        sql.Append( term );
        doneFirst = true;
      }

      sql.Append(" FROM Statements s1 ");

      foreach (string term in itsJoins) {
        sql.Append( term ).Append(" ");
      }

      sql.Append("WHERE ");

      doneFirst = false;
      foreach (string term in itsWhereTerms) {
        if ( doneFirst ) {
          sql.Append(" AND ");
        }
        sql.Append( term );
        doneFirst = true;
      }

      if ( itsQuery.IsOrdered ) {
        if ( itsQuery.OrderBy is VariableExpression ) {
          sql.Append(" ORDER BY val_" + ((VariableExpression)itsQuery.OrderBy).Variable.Name);
          if ( itsQuery.OrderDirection.Equals( Query.SortOrder.Descending ) ) {
            sql.Append(" DESC");
          }
        }
        else {
          itsIsMappable = false;
        }
      }

      return sql.ToString();
    }

    public ICollection Variables {
      get { return itsVariableList; }
    }

    private bool CollectResourcesForTerms(QueryGroup group) {
      TermCollector collector = new TermCollector( group );
    
    
      ICollection requiredTerms = collector.RequiredTerms;
      ICollection optionalTerms = collector.OptionalTerms;
      
      IDictionary requiredResourcesIndexedByNode = itsResourceMap.GetResourcesDenotedBy( requiredTerms );
      
      if ( requiredResourcesIndexedByNode.Keys.Count != requiredTerms.Count) {
        return false;
      }
      
      IDictionary optionalResourcesIndexedByNode = itsResourceMap.GetResourcesDenotedBy( optionalTerms );
      itsResourcesIndexedByNode = requiredResourcesIndexedByNode;
      foreach ( Node node in optionalResourcesIndexedByNode.Keys) {
        itsResourcesIndexedByNode[ node ] = optionalResourcesIndexedByNode[node];
      }

      return true;
    }

    public void NoSolutionIsPossible() {
      itsIsFeasible = false;
      itsIsMappable = false;
      itsSql = "";
    }

    public void MarkAsNotFeasible(string reason) {
      itsIsFeasible = false;
    }

  }
}