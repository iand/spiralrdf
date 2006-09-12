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
  using SemPlan.Spiral.Utility;
  using System;
  using System.Collections;
  using System.IO;
  using System.Data;
  using System.Configuration;
  using System.Text;

  public class DatabaseTripleStore : TripleStore, ICloneable {
    private StatementHandler itsStatementHandler;
    protected MySqlConnection itsConn;
    public bool trace = false;
    private int itsHashCode;
    private bool itsVerbose;
    
    private MySqlCommand itsAddStatementCommand;
        
    private BatchedWriteStrategy itsWriteStrategy;
    
    public DatabaseTripleStore() : this( "urn:uuid:" + Guid.NewGuid().ToString() ) { }
    
    public DatabaseTripleStore(string graphUri) {
      itsHashCode = graphUri.GetHashCode();
      
      itsStatementHandler = new StatementHandler(Add);
    
      //~ string connectionString = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=true", 
                                                              //~ ConfigurationSettings.AppSettings["server"], 
                                                              //~ ConfigurationSettings.AppSettings["uid"], 
                                                              //~ ConfigurationSettings.AppSettings["pwd"], 
                                                              //~ ConfigurationSettings.AppSettings["database"]);
                                                              
      string connectionString = ConfigurationSettings.AppSettings["connectionString"];                                                              
      //~ string connectionString = String.Format("Server={0};Uid={1};Pwd={2};Database={3}; pooling=true; resetpooledconnections=false", 
                                                              //~ ConfigurationSettings.AppSettings["server"], 
                                                              //~ ConfigurationSettings.AppSettings["uid"], 
                                                              //~ ConfigurationSettings.AppSettings["pwd"], 
                                                              //~ ConfigurationSettings.AppSettings["database"]);
      itsConn = new MySqlConnection(connectionString);
      itsConn.Open();
      
      PrepareStatements();
      
      EnsureGraphExistsInDb();
      itsWriteStrategy = new BatchedWriteStrategy(this);
      itsWriteStrategy.Connection = itsConn;
    }      

    ~DatabaseTripleStore() {
      Dispose();
    }

    public void Dispose() {
        try {
          //Clear();
          if ( null != itsConn ) {
            itsConn.Close();
            itsConn = null;
          }
        } finally {  }
    }
    
    private void PrepareStatements() {
      itsAddStatementCommand = new MySqlCommand();
      itsAddStatementCommand.Connection = itsConn;
      itsAddStatementCommand.CommandText = "INSERT IGNORE INTO Statements (subjectHash, predicateHash, objectHash, graphId) VALUES ( ?subj, ?pred, ?obj, ?graph);";
      itsAddStatementCommand.Prepare();
      itsAddStatementCommand.Parameters.Add("?subj", MySqlDbType.Int32);
      itsAddStatementCommand.Parameters.Add("?pred", MySqlDbType.Int32);
      itsAddStatementCommand.Parameters.Add("?obj", MySqlDbType.Int32);
      itsAddStatementCommand.Parameters.Add("?graph", MySqlDbType.Int32);
      
    }
  
    public override int GetHashCode() {
      return itsHashCode;
    }

    public bool IsAvailable {
      get { return (itsConn.State == ConnectionState.Open); }
    }

    internal MySqlConnection Connection {
      get {
        return itsConn;
      }
    }

    public int WriteCacheSize {
      get {
        return itsWriteStrategy.WriteCacheSize;
      }
      
      set {
        itsWriteStrategy.WriteCacheSize = value;
      }
    }

    public int NodeCount {
      get {
        MySqlCommand cmd = 
          new MySqlCommand("SELECT COUNT(*) FROM ResourceNodes WHERE graphId = " + itsHashCode, itsConn);
        return Convert.ToInt32(cmd.ExecuteScalar());
      }
    
    }

    public int ResourceCount {
      get {
        MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Resources WHERE graphId = " + itsHashCode, itsConn);
        return Convert.ToInt32(cmd.ExecuteScalar());
      }
    }
    
    
    public ResourceMap ResourceMap{ 
      get { return this; }
    }
    

    public bool Verbose {
      get {
        return itsVerbose;
      }
      set {
        itsVerbose = value;
        itsWriteStrategy.Verbose = value;
      }
    }

    public void Clear() {
      MySqlCommand cmd = new MySqlCommand("DELETE FROM Statements WHERE graphId = " + itsHashCode, itsConn);
      cmd.ExecuteNonQuery();
      cmd.CommandText = "DELETE FROM ResourceNodes WHERE graphId = " + itsHashCode;
      cmd.ExecuteNonQuery();
      cmd.CommandText = "DELETE FROM Resources WHERE graphId = " + itsHashCode;
      cmd.ExecuteNonQuery();
      cmd.CommandText = "DELETE FROM Graphs WHERE graphId = " + itsHashCode;
      cmd.ExecuteNonQuery();
    }

    private void EnsureGraphExistsInDb() {
      MySqlCommand cmd = new MySqlCommand("INSERT IGNORE INTO Graphs (graphId, description) VALUES (" + 
        itsHashCode + ", '" + itsHashCode + "')", itsConn);
      cmd.ExecuteNonQuery();
    }
  
    public void Add(Statement statement) {
      itsWriteStrategy.Add( statement );      
    }
    
    internal void FlushWriteCache() {
      itsWriteStrategy.EnsureAllWritesAreComplete();
    }
  
    public void Add(ResourceStatement statement) {
      AddStatement(statement.GetSubject().GetHashCode(), statement.GetPredicate().GetHashCode(), statement.GetObject().GetHashCode());
    }

    private void AddStatement(int subjectResourceHash, int predicateResourceHash, int objectResourceHash) {
      //~ MySqlCommand cmd = new MySqlCommand("INSERT IGNORE INTO Statements (subjectHash, predicateHash, objectHash, graphId) VALUES (" + 
        //~ subjectResourceHash + ", " + predicateResourceHash + ", " + objectResourceHash + ", " + itsHashCode + ")", itsConn);
      //~ cmd.ExecuteNonQuery();

      itsAddStatementCommand.Parameters["?subj"].Value = subjectResourceHash;
      itsAddStatementCommand.Parameters["?pred"].Value = predicateResourceHash;
      itsAddStatementCommand.Parameters["?obj"].Value = objectResourceHash;
      itsAddStatementCommand.Parameters["?graph"].Value = itsHashCode;
      itsAddStatementCommand.ExecuteNonQuery();
    }
  
    public IDictionary GetResourcesDenotedBy( ICollection nodeList ) {
      if (nodeList.Count == 0) return new Hashtable();

      Hashtable resourcesIndexedByNode = new Hashtable();
      Hashtable nodesIndexedByHashCode = new Hashtable();
      string sql = "SELECT nodeHash, resourceHash FROM ResourceNodes WHERE graphId=" + itsHashCode + " AND nodeHash IN (";

      bool doneFirst = false;      
      foreach (Node node in nodeList) {
        if ( doneFirst ) {
          sql = sql + ",";
        }
        else {
          doneFirst = true;
        }
        sql = sql + node.GetHashCode().ToString();
        nodesIndexedByHashCode[ node.GetHashCode() ]  = node;
      }
      
      sql = sql + ");";
      
      MySqlCommand cmd = new MySqlCommand( sql, itsConn );
      MySqlDataReader dataReader = cmd.ExecuteReader();

      const int NODE_HASH_COL = 0;
      const int RESOURCE_HASH_COL = 1;
      while (dataReader.Read()) {
        if ( nodesIndexedByHashCode.ContainsKey( dataReader.GetInt32( NODE_HASH_COL ) ) ) {
          resourcesIndexedByNode[ nodesIndexedByHashCode[ dataReader.GetInt32( NODE_HASH_COL ) ]  ] = new Resource(dataReader.GetInt32( RESOURCE_HASH_COL));
        }
      }
      dataReader.Close();

      return resourcesIndexedByNode;
    }
  
    public virtual Resource GetResourceDenotedBy(GraphMember theMember) {
      ArrayList list = new ArrayList();
      list.Add( theMember );
      IDictionary resourcesIndexedByNode = itsWriteStrategy.CreateAndGetResourcesDenotedBy( list );
      
      return (Resource)resourcesIndexedByNode[ theMember ];
    }
  
    private void EnsureDatatypeExistsInDb(TypedLiteral node) {
      int hash = node.GetDataType().GetHashCode();
      MySqlCommand cmd = new MySqlCommand("INSERT IGNORE INTO Datatypes (hash, value) VALUES (" + 
        hash + ", '" + node.GetDataType() + "')", itsConn);
      cmd.ExecuteNonQuery();
    }

    private void EnsureLanguageExistsInDb(PlainLiteral node) {
      int hash = node.GetLanguage().GetHashCode();
      MySqlCommand cmd = new MySqlCommand("INSERT IGNORE INTO Languages (hash, value) VALUES (" + 
        hash + ", '" + node.GetLanguage() + "')", itsConn);
      cmd.ExecuteNonQuery();
    }

    public bool IsEmpty() {
      return (StatementCount == 0);
    }

    public bool HasResourceDenotedBy(GraphMember theMember) {
      MySqlCommand cmd = new MySqlCommand("SELECT EXISTS (SELECT * FROM ResourceNodes WHERE graphId = " 
      + itsHashCode + " AND nodeHash = " + theMember.GetHashCode() + ")", itsConn);
      return (Convert.ToInt32(cmd.ExecuteScalar()) > 0);
    }

    public void Write(RdfWriter writer) {
      writer.StartOutput();
      IEnumerator statementEnum = GetStatementEnumerator();
      while (statementEnum.MoveNext()) {
        ResourceStatement statement = (ResourceStatement)statementEnum.Current;
        writer.StartSubject();
          GetBestDenotingNode(statement.GetSubject()).Write(writer);
          writer.StartPredicate();
            GetBestDenotingNode(statement.GetPredicate()).Write(writer);
            writer.StartObject();
              GetBestDenotingNode(statement.GetObject()).Write(writer);
            writer.EndObject();  
          writer.EndPredicate();  
        writer.EndSubject();  
      }
      writer.EndOutput();
    }

    public override string ToString() {
      StringWriter stringWriter = new StringWriter();
      NTripleWriter writer = new NTripleWriter(stringWriter);
      Write(writer);
      return stringWriter.ToString();
    }

    public StatementHandler GetStatementHandler() {
      return itsStatementHandler;    
    }

    public Object Clone() {
      DatabaseTripleStore dbts = new DatabaseTripleStore();
      dbts.Add(this);
      return dbts;
    }

    public void Add(TripleStore other) {
      if (this == other) return;
      Merge( other, other );
    }


    public void Merge(ResourceStatementCollection statements, ResourceMap map) {
      IEnumerator statementEnumerator = statements.GetStatementEnumerator();
      while (statementEnumerator.MoveNext()) {
        ResourceStatement resStatement = (ResourceStatement)statementEnumerator.Current;
        Node theSubject = (Node)map.GetBestDenotingNode(resStatement.GetSubject());
        Arc thePredicate = (Arc)map.GetBestDenotingNode(resStatement.GetPredicate());
        Node theObject = (Node)map.GetBestDenotingNode(resStatement.GetObject());
        Statement statement = new Statement(theSubject, thePredicate, theObject);
        Add(statement);
      }
    }

    public void Dump() {
    }

    public IEnumerator GetStatementEnumerator() {
      ArrayList statements = new ArrayList();
      MySqlCommand cmd = new MySqlCommand("SELECT s.subjectHash, s.predicateHash, s.objectHash FROM Statements s WHERE s.graphId = " + itsHashCode, itsConn);
      MySqlDataReader dataReader = cmd.ExecuteReader();
      while (dataReader.Read()) {
        Resource theSubject = new Resource(dataReader.GetInt32(0));
        Resource thePredicate = new Resource(dataReader.GetInt32(1));
        Resource theObject = new Resource(dataReader.GetInt32(2));
        statements.Add(new ResourceStatement(theSubject, thePredicate, theObject));
      }
      dataReader.Close();
      return statements.GetEnumerator();
    }

    public bool Contains(Statement statement) { 
      string sql = "SELECT EXISTS (SELECT * FROM Statements s " +
      "JOIN ResourceNodes rn1 ON s.subjectHash = rn1.resourceHash AND s.graphId = rn1.graphId " +
      "JOIN ResourceNodes rn2 ON s.predicateHash = rn2.resourceHash AND s.graphId = rn2.graphId " +
      "JOIN ResourceNodes rn3 ON s.objectHash = rn3.resourceHash AND s.graphId = rn3.graphId " +
      "WHERE s.graphId = " + itsHashCode + " AND rn1.nodeHash = " + statement.GetSubject().GetHashCode() + 
      " AND rn2.nodeHash = " + statement.GetPredicate().GetHashCode() + " AND rn3.nodeHash = " + statement.GetObject().GetHashCode() + ")";
      MySqlCommand cmd = new MySqlCommand(sql, itsConn);
      return (Convert.ToInt32(cmd.ExecuteScalar()) > 0);
    }

    public bool Contains(ResourceStatement statement) {
      MySqlCommand cmd = new MySqlCommand("SELECT EXISTS (SELECT * FROM Statements s " +
      "WHERE s.graphId = " + itsHashCode + " AND s.subjectHash = " + statement.GetSubject().GetHashCode() + 
      " AND s.predicateHash = " + statement.GetPredicate().GetHashCode() + " AND s.objectHash = " + statement.GetObject().GetHashCode() + ")", itsConn);
      return (Convert.ToInt32(cmd.ExecuteScalar()) > 0);
    }

    public bool HasNodeDenoting(Resource theResource) {
      MySqlCommand cmd = new MySqlCommand("SELECT EXISTS (SELECT rs.* FROM ResourceNodes rs " +
      "WHERE s.graphId = " + itsHashCode + " AND rs.resourceHash = " + theResource.GetHashCode() + ")", itsConn);
      return (Convert.ToInt32(cmd.ExecuteScalar()) > 0);
    }
    
    public GraphMember GetBestDenotingNode(Resource theResource) {
      IList nodes = GetNodesDenoting(theResource, true);
      if (nodes.Count > 0)
        return (GraphMember)nodes[0];
      else
        return null;
    }
  
    public IList GetNodesDenoting(Resource theResource) {
      return GetNodesDenoting(theResource, false);
    }

    private IList GetNodesDenoting(Resource theResource, bool onlyTheBest) {
      ArrayList theNodes = new ArrayList();
      MySqlCommand cmd = new MySqlCommand(
        "SELECT rn.nodeHash, rn.nodeType, u.uri, pl.value, l.value, tl.value, t.value " +
        "FROM resourcenodes rn " +
        "LEFT OUTER JOIN UriRefs u ON rn.nodeHash=u.hash AND rn.nodeType='u' " +
        "LEFT OUTER JOIN Plainliterals pl ON rn.nodeHash = pl.hash AND rn.nodeType='p' " +
        "LEFT OUTER JOIN Languages l ON pl.languageHash = l.hash " +
        "LEFT OUTER JOIN TypedLiterals tl ON rn.nodehash = tl.hash AND rn.nodeType = 't' " +
        "LEFT OUTER JOIN DataTypes t ON tl.datatypeHash = t.hash " +
        "WHERE rn.graphId = " + itsHashCode + " AND rn.resourceHash = " + theResource.GetHashCode()  + (onlyTheBest == true ? " LIMIT 1" : ""), itsConn);
      MySqlDataReader dataReader = cmd.ExecuteReader();
      int nodeHash;
      char nodeType;
      while (dataReader.Read()) {
        nodeHash = dataReader.GetInt32(0);
        nodeType = dataReader.GetChar(1);
        GraphMember node = null;
        switch (nodeType) {
          case 'u':
            node = new UriRef(dataReader.GetString(2));
            break;
          case 'b':
            node = new BlankNode(nodeHash);
            break;
          case 'p':
            node = new PlainLiteral(dataReader.GetString(3), dataReader.GetString(4));
            break;
          case 't':
            node = new TypedLiteral(dataReader.GetString(5), dataReader.GetString(6));
            break;
        }
       theNodes.Add(node);
      } 
      dataReader.Close();
      return theNodes;
    }

    public void Remove(ResourceStatement resourceStatement) {
      MySqlCommand cmd = new MySqlCommand("DELETE FROM Statements WHERE " +
        "subjectHash =  " + resourceStatement.GetSubject().GetHashCode() + " AND " +
        "predicateHash =  " + resourceStatement.GetPredicate().GetHashCode() + " AND " +
        "objectHash =  " + resourceStatement.GetObject().GetHashCode() + " AND " +
        "graphId =  " + itsHashCode, itsConn);
      cmd.ExecuteNonQuery();
    }
    
    public IList Resources {
      get {
        return ReferencedResources;
      }
    } 

    public IList ReferencedResources {
      get {
        ArrayList resources = new ArrayList();
        MySqlCommand cmd = new MySqlCommand("SELECT resourceHash FROM Resources WHERE graphId = " + itsHashCode, itsConn);
        MySqlDataReader dataReader = cmd.ExecuteReader();
        while (dataReader.Read()) {
          resources.Add(new Resource(dataReader.GetInt32(0)));
        }
        dataReader.Close();
        return resources;
      }
    }

    public int StatementCount {
      get {
        MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Statements WHERE graphId = " + itsHashCode, itsConn);
        return Convert.ToInt32(cmd.ExecuteScalar());
      }
    }

    public IEnumerator Solve(Query query) {
      FlushWriteCache();
      return new DatabaseQuerySolver(query, this);
    }
  
    public MySqlDataReader ExecuteSqlQuery(string sqlQuery) {
      MySqlCommand cmd = new MySqlCommand(sqlQuery, itsConn);
      return cmd.ExecuteReader();
    }

    // TODO: replace implementation with database specific rule processor
    public void Evaluate(SemPlan.Spiral.Core.Rule rule) {
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      ruleProcessor.Process( rule, this );
    }

    public void AddDenotation(GraphMember member, Resource theResource) {
      MySqlCommand cmd = new MySqlCommand("INSERT IGNORE INTO ResourceNodes (graphId, resourceHash, nodeHash) VALUES (" + 
        itsHashCode + ", " + theResource.GetHashCode() + ", " + member.GetHashCode() + ")", itsConn);
      cmd.ExecuteNonQuery();
    }

    public string EscapeString( string input ) {
      return input.Replace( "'", "\\'" );
    }
    public void Add(ResourceDescription description) {
      // TODO
      throw new NotImplementedException();
    }

    /// <returns>A bounded description generated according to the specified strategy.</returns>
    public virtual ResourceDescription GetDescriptionOf(Resource theResource, BoundingStrategy strategy) {
      return strategy.GetDescriptionOf(  theResource, this );
    }        

    /// <returns>A bounded description generated according to the specified strategy.</returns>
    public virtual ResourceDescription GetDescriptionOf(Node theNode, BoundingStrategy strategy) {
      return GetDescriptionOf(  GetResourceDenotedBy(theNode), strategy );
    }        


  }

}
  