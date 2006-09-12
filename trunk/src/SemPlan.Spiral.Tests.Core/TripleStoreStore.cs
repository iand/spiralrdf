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


namespace SemPlan.Spiral.Tests.Core {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;


	/// <summary>
	/// An instrumented version of TripleStore that can verify arguments to method calls. 
	/// </summary>
  /// <remarks>
  /// $Id: TripleStoreStore.cs,v 1.4 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public class TripleStoreStore : TripleStore {
    private MethodCallStore itsMethodCalls;

    public TripleStoreStore() {
      itsMethodCalls = new MethodCallStore();
    }

    public void Dispose() { 
      // NOOP
    }
    

  
    public int NodeCount { 
      get {
        return 0;
      }
    }

    public int ResourceCount { 
      get {
        return 0;
      }
    }

    public int StatementCount { 
      get {
        return 0;
      }
    }

    public IList Resources { 
      get {
        return new ArrayList();    
      }
    }
    
    public IList ReferencedResources { 
      get {
        return new ArrayList();    
      }
    }

    public ResourceMap ResourceMap{ 
      get { return this; }
    }

    
    
    public bool IsEmpty() {
      return true;    
    }

    public void Clear() {
      //NOOP    
    }

    public void Add(Statement statement) {
      //NOOP    
    }

    public bool Contains(Statement statement) {
      return false;    
    }

    public void Add(ResourceStatement statement) {
      //NOOP    
    }

    public bool Contains(ResourceStatement resourceStatement) {
      return false;    
    }

    public void Remove(ResourceStatement resourceStatement) {
      //NOOP    
    }

  
    public StatementHandler GetStatementHandler() {
      return new StatementHandler(Add);          
    }

    public void Write(RdfWriter writer) {
      //NOOP    
    }

    public IEnumerator GetStatementEnumerator() {
      ArrayList emptyList = new ArrayList();
      return emptyList.GetEnumerator();      
    }
  
    public void Add(TripleStore other) {
      //NOOP    
    }

    public IEnumerator Solve(Query query) {
      itsMethodCalls.RecordMethodCall("Solve", query);
      ArrayList emptyList = new ArrayList();
      return emptyList.GetEnumerator();      
    }

    public bool WasSolveCalledWith(Query query) {
      return itsMethodCalls.WasMethodCalledWith("Solve", query);
    }

    public void Evaluate(Rule rule) {
      //NOOP    
    }


    public Resource GetResourceDenotedBy(GraphMember theMember) {
      return new Resource();
    }


    public IDictionary GetResourcesDenotedBy(ICollection nodeList) {
      return new Hashtable();
    }

    public bool HasResourceDenotedBy(GraphMember theMember) {
      return false;
    }

    public bool HasNodeDenoting(Resource theResource) {
      return false;    
    }
    
    public IList GetNodesDenoting(Resource theResource) {
      return new ArrayList();
    }

    public GraphMember GetBestDenotingNode(Resource theResource) {
      return new BlankNode();      
    }

    public void AddDenotation(GraphMember member, Resource theResource) {
      // NOOP
    }

    public object Clone() {
      return new TripleStoreStore();
    }


    public void Add(ResourceDescription description) {

    }

    public void Merge(ResourceStatementCollection statements, ResourceMap map) {
    
    }

    /// <returns>A bounded description generated according to the specified strategy.</returns>
    public virtual ResourceDescription GetDescriptionOf(Resource theResource, BoundingStrategy strategy) {
      itsMethodCalls.RecordMethodCall("GetDescriptionOf", theResource, strategy);
      return strategy.GetDescriptionOf(  theResource, this );
    }        

    public bool WasGetDescriptionOfCalledWith(Resource theResource, BoundingStrategy strategy) {
      return itsMethodCalls.WasMethodCalledWith("GetDescriptionOf", theResource, strategy);
    }

    /// <returns>A bounded description generated according to the specified strategy.</returns>
    public virtual ResourceDescription GetDescriptionOf(Node theNode, BoundingStrategy strategy) {
      itsMethodCalls.RecordMethodCall("GetDescriptionOf", theNode, strategy);
      return GetDescriptionOf(  GetResourceDenotedBy(theNode), strategy );
    }        

    public bool WasGetDescriptionOfCalledWith(Node theNode, BoundingStrategy strategy) {
      return itsMethodCalls.WasMethodCalledWith("GetDescriptionOf", theNode, strategy);
    }

  }
}
