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

using MySql.Data.MySqlClient;

namespace SemPlan.Spiral.MySql {
  using System;
  using System.Text;
  using System.Collections;
  using SemPlan.Spiral.Core;
  using System.Data;

  /// <summary>
	/// Represents a query solver for a DatabaseTripleStore
	/// </summary>
  /// <remarks>
  /// $Id: DatabaseQuerySolver.cs,v 1.10 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public class DatabaseQuerySolver : QuerySolver, IEnumerator {
    StringBuilder itsTablesClause;
    StringBuilder itsWhereClause;
    StringBuilder itsSelectClause;
    Hashtable itsVariables; 
    Hashtable itsResources; 
    Query itsQuery;
    DatabaseTripleStore itsTriples;
    //~ MySqlDataReader itsDataReader;
    ICollection itsSelectedVariables;
    IEnumerator itsSolutions;
    private bool itsExplain;
    private IDictionary itsResourcesIndexedByNode;
    private bool itsSolutionIsPossible;
    private string itsQuerySql;
    
    public DatabaseQuerySolver(Query query, DatabaseTripleStore triples) {
      itsQuery = query;
      itsTriples = triples;
      AssumeSolutionIsPossible();
      
      QuerySqlMapper mapper = new QuerySqlMapper( query, triples );
      if ( mapper.IsFeasible ) {
        if (mapper.IsMappable) {

          itsQuerySql = mapper.Sql;
          itsSelectedVariables = mapper.Variables;      
    
          ExecuteQuery();      
        }
        else {
          itsSolutions = new ArrayList().GetEnumerator();
        }
      }
      else {
        itsSolutions = new ArrayList().GetEnumerator();
      }
    }
    
    public bool Explain {
      get {
        return itsExplain;
      }
      
      set {
        itsExplain = value;
      }
    }

    public string Sql {
      get {
        return itsQuerySql;
      }
    }
    
    public bool MoveNext( ) {
      if (itsSolutions != null)
        return itsSolutions.MoveNext();
      else 
        return false;
    }

    public object Current {
      get {
        return itsSolutions.Current;
      }
    }
    
    public bool SolutionIsPossible {
      get { return itsSolutionIsPossible; }
      set { itsSolutionIsPossible = value; }
    }
    
    public void AssumeSolutionIsPossible() {
      SolutionIsPossible = true;
    }
    
    public void NoSolutionIsPossible() {
      itsSolutions = new ArrayList().GetEnumerator();
      SolutionIsPossible = false;
    }
    
    public void Reset() {
      throw new NotSupportedException("Reset not supported");
    }
  
    public void ExecuteQuery() {
      ArrayList solutions = new ArrayList();
      if ( SolutionIsPossible ) {
        MySqlDataReader dataReader = null;
        try {
      
          dataReader = itsTriples.ExecuteSqlQuery(itsQuerySql);
          while (dataReader.Read()) {
            QuerySolution solution = new QuerySolution();
            int col = 0;
            foreach (Variable var in itsSelectedVariables) {
            
              object resourceHash = dataReader.GetValue(col);
              if ( !resourceHash.Equals(DBNull.Value) ) {
                Resource resource = new Resource( Convert.ToInt32(resourceHash));
                solution[var.Name] = resource;
                
                int nodeHash = dataReader.GetInt32(col + 1);
                char nodeType = dataReader.GetChar(col + 2);
                string lexicalValue = dataReader.GetString(col + 3);
                object rawSubValue = dataReader.GetValue(col + 4);
                string subValue = null;
                if ( ! rawSubValue.Equals(DBNull.Value) ) {
                  subValue = (string)rawSubValue;
                }
                
                GraphMember node = null;
                switch (nodeType) {
                  case 'u':
                    node = new UriRef(lexicalValue);
                    break;
                  case 'b':
                    node = new BlankNode(nodeHash);
                    break;
                  case 'p':
                    node = new PlainLiteral(lexicalValue, subValue);
                    break;
                  case 't':
                    node = new TypedLiteral(lexicalValue, subValue);
                    break;
                }
                
                solution.SetNode( var.Name, node );
              }
              col += 5;
            }
            solutions.Add(solution);
          }
        }
        catch (MySqlException e ) {
          throw new ApplicationException( "Error executing '" + itsQuerySql + "' for query " + itsQuery, e);
        }
        finally {
          if ( null != dataReader) {
            dataReader.Close();
          }
        }
      }
      
      itsSolutions = solutions.GetEnumerator();
    
    }
 
 }
  
}