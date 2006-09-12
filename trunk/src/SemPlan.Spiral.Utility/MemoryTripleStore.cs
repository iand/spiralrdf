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

namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.IO;

  // This class is being extracted from the enclosing class
  public class MemoryTripleStore : TripleStore {
    private Hashtable itsNamedResources;
    private Hashtable itsResourceDenotations;
    private Hashtable itsResourceStatements;

    private StatementHandler itsStatementHandler;

    private bool itsVerbose;

    public MemoryTripleStore() {
      itsNamedResources = new Hashtable(3000);
      itsResourceDenotations = new Hashtable(3000);
      itsStatementHandler = new StatementHandler(Add);
      itsResourceStatements = new Hashtable(1000);
    }

    public void Dispose() {
      itsNamedResources.Clear();
      itsNamedResources = null;

      itsResourceDenotations.Clear();
      itsResourceDenotations = null;

      itsResourceStatements.Clear();
      itsResourceStatements = null;
    }
    
    public bool Verbose {
      get {
        return itsVerbose;
      }
      
      set {
        itsVerbose = value;
      }
   }
  
    public int NodeCount {
      get {
        return itsNamedResources.Count;
      }
    }

    public int ResourceCount {
      get {
        return itsResourceDenotations.Count;
      }
    }

    public int StatementCount {
      get {
        return itsResourceStatements.Count;
      }
    }

    //~ public IList Nodes {
      //~ get {
        //~ ArrayList nodeList = new ArrayList();
        //~ nodeList.AddRange(  itsNamedResources.Keys );
        //~ return nodeList;
      //~ }
    //~ } 

    public ResourceMap ResourceMap{ 
      get { return this; }
    }

    public IList Resources {
      get {
        return ReferencedResources;
      }
    } 

    public IList ReferencedResources {
      get {
        ArrayList resourceList = new ArrayList();
        resourceList.AddRange(  itsResourceDenotations.Keys );
        return resourceList;
      }
    } 


    public void Clear() {
      itsNamedResources = new Hashtable();
      itsResourceDenotations = new Hashtable();
      itsResourceStatements = new Hashtable();
      itsStatementHandler = new StatementHandler(Add);
    }

    public void AddDenotation(GraphMember member, Resource theResource) {
      itsNamedResources[ member ] = theResource;

      if ( HasNodeDenoting( theResource )) {
        ((ArrayList)itsResourceDenotations[ theResource ]).Add( member ); 
      }
      else {
        ArrayList nodeList = new ArrayList();
        nodeList.Add( member );
        itsResourceDenotations[ theResource ] = nodeList; 
      }
    }

    public void AddDenotations(IList members, Resource theResource) {
      foreach (GraphMember member in members) {
        itsNamedResources[ member ] = theResource;
      }

      if ( HasNodeDenoting( theResource )) {
        ((ArrayList)itsResourceDenotations[ theResource ]).AddRange( members ); 
      }
      else {
        ArrayList nodeList = new ArrayList();
        nodeList.AddRange( members );
        itsResourceDenotations[ theResource ] = nodeList; 
      }
    }

    public void SetDenotations(IList members, Resource theResource) {
      foreach (GraphMember member in members) {
        itsNamedResources[ member ] = theResource;
      }

      ArrayList nodeList = new ArrayList();
      nodeList.AddRange( members );
      itsResourceDenotations[ theResource ] = nodeList; 
    }
    
    public bool HasResourceDenotedBy(GraphMember theMember) {
      return itsNamedResources.ContainsKey(theMember);
    }

    public bool HasNodeDenoting(Resource theResource) {
      return itsResourceDenotations.ContainsKey(theResource);
    }

    public bool HasDenotation(GraphMember theMember, Resource theResource) {
      return itsNamedResources[theMember].Equals( theResource );
    }

    public IList GetNodesDenoting(Resource theResource) {
      ArrayList nodeList = new ArrayList();
      if ( HasNodeDenoting( theResource )) {
        nodeList.AddRange((ArrayList)itsResourceDenotations[ theResource ]); 
      }
      return nodeList;
    }

    public GraphMember GetBestDenotingNode(Resource theResource) {
      Node theNode = null;
      
      if ( HasNodeDenoting( theResource )) {
        foreach (Node node in (ArrayList)itsResourceDenotations[ theResource ]) {
          
          if (node is BlankNode) {
            if (theNode == null) {              
              theNode = node;
            }
          }
          else {
            return node;
          }
        }
      }

      if ( theNode == null) {
        BlankNode newNode = new BlankNode();
        AddDenotation( newNode, theResource );
        return newNode;
      }
      else {
        return theNode;
      }
    }

    /// <returns>A resource denoted by the subject if the subject is known, otherwise a new resource</returns>
    public virtual Resource GetResourceDenotedBy(GraphMember theMember) {
      if (itsNamedResources.ContainsKey(theMember) ) {
        return (Resource)itsNamedResources[theMember];
      }
      else {
        Resource resource = MakeNewResourceForNode( theMember);
        AddDenotation( theMember, resource );
        return resource;
      }
    }


    public virtual IDictionary GetResourcesDenotedBy(ICollection nodeList ) {
      Hashtable resourcesIndexedByNode = new Hashtable();

      foreach (GraphMember node in nodeList) {
        if (itsNamedResources.ContainsKey(node) ) {
          resourcesIndexedByNode[node] = itsNamedResources[node];
        }
      }
      
      return resourcesIndexedByNode;
    }


    public Resource MakeNewResourceForNode(GraphMember theMember) {
      if (theMember.IsSelfDenoting()) {
        return new Resource( theMember );
      }
      else {
        return new Resource();
      }
    }

    public GraphMember MakeNameForResource( Resource theResource) {
      if (theResource.Value is GraphMember) {
        return (GraphMember)theResource.Value;
      }
      else {
        return new BlankNode();
      }
    }


    public void Add(Statement statement) {
      Resource theSubject;
      if ( HasResourceDenotedBy( statement.GetSubject() ) ) {
        theSubject = GetResourceDenotedBy( statement.GetSubject() );
      }
      else {
        theSubject = MakeNewResourceForNode( statement.GetSubject() );
        AddDenotation( statement.GetSubject(), theSubject );
      }

      Resource thePredicate;
      if ( HasResourceDenotedBy( statement.GetPredicate() ) ) {
        thePredicate = GetResourceDenotedBy( statement.GetPredicate() );
      }
      else {
        thePredicate = MakeNewResourceForNode( statement.GetPredicate() );
        AddDenotation( statement.GetPredicate(), thePredicate );
      }

      Resource theObject;
      if ( HasResourceDenotedBy( statement.GetObject() ) ) {
        theObject = GetResourceDenotedBy( statement.GetObject() );
      }
      else {
        theObject = MakeNewResourceForNode( statement.GetObject() );
        AddDenotation( statement.GetObject(), theObject );
      }

      ResourceStatement resourceStatement = new ResourceStatement( theSubject,  thePredicate, theObject );
      Add( resourceStatement );
    }

    public void Add(Resource theSubject, Arc thePredicateArc, Resource theObject) {
      Resource thePredicate;
      if ( HasResourceDenotedBy( thePredicateArc ) ) {
        thePredicate = GetResourceDenotedBy( thePredicateArc );
      }
      else {
        thePredicate = MakeNewResourceForNode( thePredicateArc );
        AddDenotation( thePredicateArc, thePredicate );
      }

      Add( new ResourceStatement( theSubject, thePredicate, theObject ) );
    }

    public void Add(ResourceStatement resourceStatement) {
      if ( ! itsResourceStatements.ContainsKey( resourceStatement ) ) {
        if (! HasNodeDenoting( resourceStatement.GetSubject() ) ) {
          AddDenotation( MakeNameForResource(resourceStatement.GetSubject()), resourceStatement.GetSubject() );
        }
        if (! HasNodeDenoting( resourceStatement.GetPredicate() ) ) {
          AddDenotation( MakeNameForResource(resourceStatement.GetPredicate()), resourceStatement.GetPredicate() );
        }
        if (! HasNodeDenoting( resourceStatement.GetObject() ) ) {
          AddDenotation( MakeNameForResource(resourceStatement.GetObject()),  resourceStatement.GetObject() );
        }

        itsResourceStatements[ resourceStatement ] = resourceStatement;
      }
    }
    
    public void Remove(ResourceStatement resourceStatement) {
      itsResourceStatements.Remove( resourceStatement );
    }
    
    public IEnumerator GetStatementEnumerator() {
      return itsResourceStatements.Keys.GetEnumerator();
    }


    public bool IsEmpty() {
      return (itsResourceStatements.Count == 0);
    }

    public void Dump() {
      Console.WriteLine( "Triples:");
      Console.WriteLine( ToString() );
      Console.WriteLine("  itsNamedResources:");
      foreach (Node node in itsNamedResources.Keys) {
        Console.WriteLine("    Node {0} maps to Resource {1}", node, itsNamedResources[node]);
      }
    }

    public override string ToString() {
      StringWriter stringWriter = new StringWriter();
      NTripleWriter writer = new NTripleWriter(stringWriter);
      Write(writer);
      return stringWriter.ToString();
    }
    
    public object Clone() {
      MemoryTripleStore theClone = new MemoryTripleStore();
      theClone.itsNamedResources = (Hashtable)itsNamedResources.Clone();
      theClone.itsResourceDenotations = (Hashtable)itsResourceDenotations.Clone();
      theClone.itsResourceStatements = (Hashtable)itsResourceStatements.Clone();
      return theClone;
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

    public StatementHandler GetStatementHandler() {
      return itsStatementHandler;    
    }

    public bool Contains(Statement statement) {
      if ( ! HasResourceDenotedBy( statement.GetSubject()) ) {
        return false;
      }

      if ( ! HasResourceDenotedBy(  statement.GetPredicate()) ) {
        return false;
      }

      if ( ! HasResourceDenotedBy( statement.GetObject() ) ) {
        return false;
      }

      Resource theSubject = GetResourceDenotedBy(  statement.GetSubject() );
      Resource thePredicate = GetResourceDenotedBy(  statement.GetPredicate() );
      Resource theObject = GetResourceDenotedBy( statement.GetObject()  );

      ResourceStatement testStatement = new ResourceStatement( theSubject, thePredicate, theObject);

      return Contains( testStatement );
    }
    
    public bool Contains(ResourceStatement resourceStatement) {
      return itsResourceStatements.ContainsKey( resourceStatement );
    }
  

    public void Merge(ResourceStatementCollection statements, ResourceMap map)  {
      if (this == statements) return;
      
      Hashtable equivalentResources = new Hashtable();
      
      foreach (Resource resource in statements.ReferencedResources) {
        Resource internalResource = resource;

        foreach (GraphMember member in map.GetNodesDenoting( resource ) ) {
          if ( HasResourceDenotedBy( member ) ) {
            internalResource = GetResourceDenotedBy( member );
            equivalentResources[ resource ] = internalResource;
            break;
          }
          
          // Must be a new resource
          // Remember this for when we're Adding statements
        // TODO: move this outside the loop
          equivalentResources[ resource ] = resource;
        }

        foreach (GraphMember member in map.GetNodesDenoting( resource ) ) {
          AddDenotation( member, internalResource );
        }
      }

      IEnumerator statementEnumerator = statements.GetStatementEnumerator();
      while (statementEnumerator.MoveNext()) {
        ResourceStatement statement = (ResourceStatement)statementEnumerator.Current;
        ResourceStatement internalStatement = new ResourceStatement( (Resource)equivalentResources[ statement.GetSubject() ] ,
                                                                                                                    (Resource)equivalentResources[ statement.GetPredicate() ] , 
                                                                                                                    (Resource)equivalentResources[ statement.GetObject() ]  );
        Add( internalStatement );
      }
      
    }

    public void Add(TripleStore other) {
      if (this == other) return;
      Merge( other, other );
    }
  
  
    public IEnumerator Solve(Query query) {
      return new BacktrackingQuerySolver( query, this);
    }
  
    public void Evaluate(Rule rule) {
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      ruleProcessor.Process( rule, this );
    }


    public void Add(ResourceDescription description) {
      Merge( description, description.ResourceMap );
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
  