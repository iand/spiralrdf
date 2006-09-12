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
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.Configuration;
  using System.Text;
  
  public class BatchedWriteStrategy : WriteStrategy {
    private int itsWriteCacheSize;
    private ArrayList itsWriteCache;
    private DatabaseTripleStore itsStore;
    protected MySqlConnection itsConn;
    private bool itsVerbose;

    public BatchedWriteStrategy(DatabaseTripleStore store) {
      itsWriteCache = new ArrayList();
      itsWriteCacheSize = 0;
      itsStore = store;
    }
  
    public int WriteCacheSize {
      get {
        return itsWriteCacheSize;
      }
      
      set {
        itsWriteCacheSize = value;
      }
    }
  
    public MySqlConnection Connection {
      get {
        return itsConn;
      }
      set {
        itsConn = value;
      }
    }
  
    public bool Verbose {
      get {
        return itsVerbose;
      }
      set {
        itsVerbose = value;
      }
    }

  
    public void Add(Statement statement) {
      itsWriteCache.Add( statement );
      
      if ( itsWriteCache.Count > itsWriteCacheSize ) {
        EnsureAllWritesAreComplete();
      }
    }
    
    public void EnsureAllWritesAreComplete() {
      if (itsWriteCache.Count == 0) return;

      ArrayList nodeList = new ArrayList();

      foreach (Statement statement in itsWriteCache) {
        nodeList.Add( statement.GetSubject() );
        nodeList.Add( statement.GetPredicate() );
        nodeList.Add( statement.GetObject() );
      }
      
      
      IDictionary resourcesIndexedByNode = CreateAndGetResourcesDenotedBy( nodeList );


      
      StringBuilder insertNewStatements = new StringBuilder("INSERT IGNORE INTO Statements (subjectHash, predicateHash, objectHash, graphId) VALUES ");
  
      foreach (Statement statement in itsWriteCache) {
        
        insertNewStatements.Append("(");
        if ( resourcesIndexedByNode.Contains( statement.GetSubject() ) ) {
          insertNewStatements.Append( resourcesIndexedByNode[ statement.GetSubject() ].GetHashCode() );
        }
        else {
          insertNewStatements.Append( itsStore.GetResourceDenotedBy(statement.GetSubject()).GetHashCode() );
        }
        insertNewStatements.Append(",");
  
        if ( resourcesIndexedByNode.Contains( statement.GetPredicate() ) ) {
          insertNewStatements.Append( resourcesIndexedByNode[ statement.GetPredicate() ].GetHashCode() );
        }
        else {
          insertNewStatements.Append( itsStore.GetResourceDenotedBy(statement.GetPredicate()).GetHashCode() );
        }
        insertNewStatements.Append(",");
        
        if ( resourcesIndexedByNode.Contains( statement.GetObject() ) ) {
          insertNewStatements.Append( resourcesIndexedByNode[ statement.GetObject() ].GetHashCode() );
        }
        else {
          insertNewStatements.Append(  itsStore.GetResourceDenotedBy(statement.GetObject()).GetHashCode() );
        }
        insertNewStatements.Append(",");
        insertNewStatements.Append( itsStore.GetHashCode() );
        insertNewStatements.Append("),");
      }
      

      MySqlCommand cmd = new MySqlCommand( insertNewStatements.ToString(), itsConn );
      insertNewStatements.Remove(insertNewStatements.Length - 1, 1); // remove trailing comma
//~ Console.WriteLine(insertNewStatements);
      cmd.CommandText = insertNewStatements.ToString();
      cmd.ExecuteNonQuery();

      itsWriteCache.Clear();    
    }
    
    
    public IDictionary CreateAndGetResourcesDenotedBy( ICollection nodeList ) {
      if (nodeList.Count == 0) return new Hashtable();

      Hashtable uniqueNodes = new Hashtable();
      const string YES = "y";
      foreach ( Node node in nodeList) {
        uniqueNodes[ node ] = YES;
      }
      

      IDictionary resourcesIndexedByNode = itsStore.GetResourcesDenotedBy(uniqueNodes.Keys);

      if ( resourcesIndexedByNode.Keys.Count == uniqueNodes.Keys.Count) {
        return resourcesIndexedByNode;
      }

      // Some new nodes
      StringBuilder insertNewResources = new StringBuilder("INSERT IGNORE INTO Resources (graphId, resourceHash) VALUES ");
      StringBuilder insertNewUriRefs = new StringBuilder("INSERT IGNORE INTO UriRefs (hash, uri) VALUES ");
      StringBuilder insertNewPlainLiterals = new StringBuilder("INSERT IGNORE INTO PlainLiterals (hash, languageHash, value) VALUES ");
      StringBuilder insertNewLanguages = new StringBuilder("INSERT IGNORE INTO Languages (hash, value) VALUES ");
      StringBuilder insertNewTypedLiterals = new StringBuilder(" INSERT IGNORE INTO TypedLiterals (hash, datatypeHash, value) VALUES ");
      StringBuilder insertNewDataTypes = new StringBuilder("INSERT IGNORE INTO Datatypes (hash, value) VALUES ");
      StringBuilder insertNewResourceNodes = new StringBuilder("INSERT IGNORE INTO ResourceNodes (graphId, resourceHash, nodeHash, nodeType) VALUES");

      bool hasNewUriRefs = false;
      bool hasNewPlainLiterals = false;
      bool hasNewLanguages = false;
      bool hasNewTypedLiterals = false;
        
      foreach (Node node in uniqueNodes.Keys) {
        if ( resourcesIndexedByNode.Contains(node) ) continue;

        Resource resource = new Resource();
        resourcesIndexedByNode[ node ] = resource;

        insertNewResources.Append( "(" ).Append( itsStore.GetHashCode() ).Append(",").Append( resource.GetHashCode() ).Append("),");

        char nodeType = 'b';
        if ( node is UriRef ) {
          insertNewUriRefs.Append("(").Append( node.GetHashCode() ).Append(",'").Append( itsStore.EscapeString(node.GetLabel()) ).Append("'),"); 
          hasNewUriRefs = true;
          nodeType = 'u';
        }
        else if ( node is PlainLiteral ) {
          if (((PlainLiteral)node).GetLanguage() != null) {
            hasNewLanguages = true;
            insertNewLanguages.Append("(").Append( ((PlainLiteral)node).GetLanguage().GetHashCode() ).Append(",'").Append( itsStore.EscapeString( ((PlainLiteral)node).GetLanguage() ) ).Append("'),");
            insertNewPlainLiterals.Append("(").Append( node.GetHashCode() ).Append(",").Append( ((PlainLiteral)node).GetLanguage().GetHashCode() ).Append(",'").Append( itsStore.EscapeString(node.GetLabel()) ).Append("'),"); 
          }
          else {
            insertNewPlainLiterals.Append("(").Append( node.GetHashCode() ).Append(",null,'").Append( itsStore.EscapeString( node.GetLabel() ) ).Append("'),"); 
          }
          hasNewPlainLiterals = true;
          nodeType = 'p';
        }
        else if ( node is TypedLiteral ) {
          insertNewDataTypes.Append("(").Append( ((TypedLiteral)node).GetDataType().GetHashCode() ).Append(",'").Append( itsStore.EscapeString(((TypedLiteral)node).GetDataType() )).Append("'),");
          insertNewTypedLiterals.Append("(").Append( node.GetHashCode() ).Append(",").Append( ((TypedLiteral)node).GetDataType().GetHashCode() ).Append(",'").Append( itsStore.EscapeString(node.GetLabel()) ).Append("'),"); 
          hasNewTypedLiterals = true;
          nodeType = 't';
        }
        insertNewResourceNodes.Append("(").Append(itsStore.GetHashCode()).Append(",").Append( resource.GetHashCode() ).Append(",").Append( node.GetHashCode() ).Append(",'").Append( nodeType).Append("'),");
      }

      insertNewResources.Remove(insertNewResources.Length - 1, 1); // remove trailing comma
      if (Verbose) Console.WriteLine("WOULD EXECUTE:");
      if (Verbose) Console.WriteLine(insertNewResources);

      MySqlCommand cmd = new MySqlCommand( insertNewResources.ToString(), itsConn );
      cmd.ExecuteNonQuery();
      
      if (hasNewUriRefs) {
        insertNewUriRefs.Remove(insertNewUriRefs.Length - 1, 1); // remove trailing comma
        if (Verbose) Console.WriteLine(insertNewUriRefs);
        cmd.CommandText = insertNewUriRefs.ToString();
        cmd.ExecuteNonQuery();
      }

      if (hasNewLanguages) {
        insertNewLanguages.Remove(insertNewLanguages.Length - 1, 1); // remove trailing comma
        if (Verbose) Console.WriteLine(insertNewLanguages);
        cmd.CommandText = insertNewLanguages.ToString();
        cmd.ExecuteNonQuery();
      }

      if (hasNewPlainLiterals) {
        insertNewPlainLiterals.Remove(insertNewPlainLiterals.Length - 1, 1); // remove trailing comma
        if (Verbose) Console.WriteLine(insertNewPlainLiterals);
        cmd.CommandText = insertNewPlainLiterals.ToString();
        cmd.ExecuteNonQuery();
      }

      if (hasNewTypedLiterals) {
        insertNewDataTypes.Remove(insertNewDataTypes.Length - 1, 1); // remove trailing comma
        if (Verbose) Console.WriteLine(insertNewDataTypes);
        cmd.CommandText = insertNewDataTypes.ToString();
        cmd.ExecuteNonQuery();

        insertNewTypedLiterals.Remove(insertNewTypedLiterals.Length - 1, 1); // remove trailing comma
        cmd.CommandText = insertNewTypedLiterals.ToString();
        cmd.ExecuteNonQuery();
      }


      insertNewResourceNodes.Remove(insertNewResourceNodes.Length - 1, 1); // remove trailing comma
      if (Verbose) Console.WriteLine(insertNewResourceNodes);
      if (Verbose) Console.WriteLine("------------------");
      cmd.CommandText = insertNewResourceNodes.ToString();
      cmd.ExecuteNonQuery();
      


      return resourcesIndexedByNode;

    }

  }

}
  