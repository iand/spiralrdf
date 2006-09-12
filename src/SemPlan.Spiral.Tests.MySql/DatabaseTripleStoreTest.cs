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

namespace SemPlan.Spiral.Tests.MySql {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.MySql;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  using System.IO;
  using System.Xml;
  
	/// <summary>
	/// Programmer tests for DatabaseTripleStore class
	/// </summary>
  /// <remarks>
  /// $Id: DatabaseTripleStoreTest.cs,v 1.6 2006/02/07 01:11:14 ian Exp $
  ///</remarks>
	[TestFixture] [Category("RequiresMySql")]
  public class DatabaseTripleStoreTest : TripleStoreTest {
    ArrayList itsTripleStores;
    public override TripleStore MakeNewTripleStore() {
      DatabaseTripleStore store = new  DatabaseTripleStore();
      itsTripleStores.Add( store );
      return store;
    }

    public override IEnumerator GetSolutions(Query query, TripleStore tripleStore, bool explain) {
      return tripleStore.Solve( query );
    }

    [SetUp]
    public void SetUp() {
      itsTripleStores = new ArrayList();
    }

    [TearDown]
    public void TearDown() {
      foreach (TripleStore store in itsTripleStores) {
        store.Clear();
      }
    }
    
    [Test]
    public void connectToTestDatabase() {
      DatabaseTripleStore store = (DatabaseTripleStore)MakeNewTripleStore();
      Assert.IsTrue(store.IsAvailable);
    }
    
    [Test]
    public void hasResourceDenotedBy() {
      DatabaseTripleStore store = (DatabaseTripleStore)MakeNewTripleStore();
      Node subject = new UriRef("abc");
      Arc predicate = new UriRef("def");
      Node obj = new UriRef("ghi");
      Node dummy = new UriRef("xxxxxxx");
      Statement s = new Statement(subject, predicate, obj);
      store.Add(s);
      Assert.IsTrue(store.HasResourceDenotedBy(subject), "store has subject");
      Assert.IsTrue(store.HasResourceDenotedBy(predicate), "store has predicate");
      Assert.IsTrue(store.HasResourceDenotedBy(obj), "store has object");
      Assert.IsFalse(store.HasResourceDenotedBy(dummy), "store has dummy");
      store.Clear();
    }

 }  
  
}