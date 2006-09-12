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

using SemPlan.Spiral.Core;
using SemPlan.Spiral.Utility;
using SemPlan.Spiral.MySql;
using System;
using System.Collections;

public class Shootout {

	static void Main(string[] args) {
    if (args.Length < 2) {
      Console.WriteLine("Usage: Shootout.exe <type> <number of subjects>");
      Console.WriteLine("  type can be db or mem");
      return;
    }
  
    TripleStoreDriver driver;
    
    if (args[0].Equals("mem")) {
      driver = new MemoryTripleStoreDriver();
    }
    else if (args[0].Equals("db")) {
      driver = new DatabaseTripleStoreDriver();
    }
    else {
      Console.WriteLine("Unknown triple store type: " + args[0]);
      return;
    }

    Shootout.theNodesCnt = Convert.ToInt32( args[1] );
    
    Shootout shootout = new Shootout( driver );
    shootout.RunSuite();
    

  }

  private RdfDbTestDriver theTestDriver;

  public Shootout(RdfDbTestDriver testDriver) {
    theTestDriver = testDriver;
  }

  
  public static int theNodesCnt = 0;

	public void Query() {


		long startTime = DateTime.Now.Ticks;
		long cnt = 0;
    long queryCount = 0;
    
    Random random = new Random();
		object repo = null;
		try {
			repo = theTestDriver.GetRepository();

			for (int i = 0; i < (theNodesCnt / 100); i++) {
				long val = (long) (random.Next(theNodesCnt) );
				object it = theTestDriver.GetStatements(repo, theTestDriver.CreatePrd(repo, "node" + val), null, null);
        ++queryCount;
        
				// check type before each query since query result could be a literal
				while (theTestDriver.MoreStatements(it)) {
					object s1 = theTestDriver.NextStatement(it);
					if (!theTestDriver.IsSub(theTestDriver.GetObj(s1))) continue;
					cnt++;
					object it2 = theTestDriver.GetStatements(repo, theTestDriver.GetObj(s1), null, null);
        ++queryCount;

					while (theTestDriver.MoreStatements(it2)) {
						object s2 = theTestDriver.NextStatement(it2);
						if (!theTestDriver.IsSub(theTestDriver.GetObj(s2))) continue;
						cnt++;
						object it3 = theTestDriver.GetStatements(repo, theTestDriver.GetObj(s1), null, null);
          ++queryCount;

						while (theTestDriver.MoreStatements(it3)) {
							object s3 = theTestDriver.NextStatement(it3);
							if (!theTestDriver.IsSub(theTestDriver.GetObj(s3))) continue;
							cnt++;
						}
						theTestDriver.CloseStmtIt(it3);
					}
					theTestDriver.CloseStmtIt(it2);
				}
				theTestDriver.CloseStmtIt(it);
			}
		} catch (Exception e) {
      Console.WriteLine("Unexpected exception: " + e );
      Console.WriteLine( e.StackTrace );
		} finally {
			if (repo != null) theTestDriver.CloseRepository(repo);
		}

		Console.WriteLine("#statements traversed: " + cnt);
		Console.WriteLine("#queries executed: " + queryCount);
		Console.WriteLine("\tQuery Run-time: " + (DateTime.Now.Ticks - startTime) / 10000000.0 + "s");
	}

    public bool weakAPI = true;
    
    public void AddData() {
    	long startTime = 0;
      Random random = new Random();
    	
    	try {
    		object repo = theTestDriver.GetRepository();
		
		    object[] preds = new object[10];
		    preds[0] = theTestDriver.CreatePrd(repo, "pred" + 0);
		    preds[1] = theTestDriver.CreatePrd(repo, "pred" + 1);
		    preds[2] = theTestDriver.CreatePrd(repo, "pred" + 2);
		    preds[3] = theTestDriver.CreatePrd(repo, "pred" + 3);
		    preds[4] = theTestDriver.CreatePrd(repo, "pred" + 4);
		    preds[5] = theTestDriver.CreatePrd(repo, "pred" + 5);
		    preds[6] = theTestDriver.CreatePrd(repo, "pred" + 6);
		    preds[7] = theTestDriver.CreatePrd(repo, "pred" + 7);
		    preds[8] = theTestDriver.CreatePrd(repo, "pred" + 8);
		    preds[9] = theTestDriver.CreatePrd(repo, "pred" + 9);
		    
		    startTime = DateTime.Now.Ticks;

		    theTestDriver.StartTransaction(repo);
		
		    for (long i=0;i<theNodesCnt; i++) {
		        
		        long val = (long) i;
		        object mySubject = theTestDriver.CreatePrd(repo, "node" + val);
		        for (int j=0;j<10;j++) {
		            int val2 = (int) (random.Next(theNodesCnt/100) );
		            theTestDriver.AddStatement(repo, mySubject, preds[random.Next(10)], theTestDriver.CreatePrd(repo, "node" + val2));
		        }
		        
		        if (weakAPI) {
			        if (i % (theNodesCnt / 10) == 0) {
			            Console.WriteLine("Added " + (i * 10)+ " triples in " + (DateTime.Now.Ticks - startTime)/10000000.0 + "s");
			            theTestDriver.CommitTransaction(repo);
			            theTestDriver.StartTransaction(repo);
			        }
		        }
		    }
		    //System.err.println("Add  Run-time: " + (System.currentTimeMillis() - startTime) / 1000 + "s");
		    
		    //myRepository.addGraph(myGraph);
		    theTestDriver.CommitTransaction(repo);
		} catch (Exception e) {
      Console.WriteLine("Unexpected exception: " + e );
      Console.WriteLine(e.StackTrace);
		}

		Console.WriteLine("\tAdd Run-time: " + (DateTime.Now.Ticks - startTime) / 10000000.0 + "s");
    }

    public void CountStatements() {
    	try {
    		object repo = theTestDriver.GetRepository();
    		object it = theTestDriver.GetStatements(repo, null, null, null);
	        int cnt = 0;
	        while (theTestDriver.MoreStatements(it)) {
	        	theTestDriver.NextStatement(it);
	        	//repo.getGraph().remove(it.next());
	            cnt ++;
	        }
	        theTestDriver.CloseStmtIt(it);
	        Console.WriteLine(cnt);
    	} catch (Exception e) {
        Console.WriteLine("Unexpected exception: " + e);
        Console.WriteLine(e.StackTrace);
    	}
    }

    public void removeStatements() {
    	try {
    		object repo = theTestDriver.GetRepository();

    		object it = theTestDriver.GetStatements(repo, null, null, null);
	        while (theTestDriver.MoreStatements(it)) {
	        	theTestDriver.RemoveStatement(repo, theTestDriver.NextStatement(it));
		        theTestDriver.CloseStmtIt(it);
		    	it = theTestDriver.GetStatements(repo, null, null, null);
	        }
	        theTestDriver.CloseStmtIt(it);
    	} catch (Exception e) {
        Console.WriteLine("Unexpected exception: " + e);
        Console.WriteLine(e.StackTrace);
    	}
    }

	public void RunSuite() {
		Console.WriteLine("0. Adding Data");
		AddData();
		Console.WriteLine("1. Querying Data");
    Query();
		Console.WriteLine("2. Done");
	}


}

/// Ported from http://simile.mit.edu/repository/shootout/trunk/shootout/src/edu/mit/simile/shootout/common/RdfDbTestDriver.java
public abstract class RdfDbTestDriver {
  public abstract /*=RepositoryType=*/object/**/ GetRepository();

	public abstract object CreatePrd(/*=RepositoryType=*/object/**/ repo, string uri);

	public abstract void RemoveStatement(/*=RepositoryType=*/object/**/ repo, /*=StmtType=*/object/**/ s);
	public abstract void AddStatement(/*=RepositoryType=*/object/**/ repo, object s, object p, object o);
	public abstract /*=StmtItType=*/object/**/ GetStatements(/*=RepositoryType=*/object/**/ repo, object s, object p, object o);

	public abstract object GetSub(/*=StmtType=*/object/**/ s);
	public abstract object GetObj(/*=StmtType=*/object/**/ s);
	public abstract object GetPrd(/*=StmtType=*/object/**/ s);
	public abstract bool IsSub(object obj);
	public abstract bool IsObj(object obj);
	public abstract bool IsPrd(object obj);
	public abstract bool MoreStatements(/*=StmtItType=*/object/**/ it);
	public abstract /*=StmtType=*/object/**/ NextStatement(/*=StmtItType=*/object/**/ it);
	public abstract void CloseStmtIt(/*=StmtItType=*/object/**/ it);

	public virtual void StartTransaction(/*=RepositoryType=*/object/**/ repo) {}
	public virtual void CommitTransaction(/*=RepositoryType=*/object/**/ repo) {}
	public virtual void CloseRepository(object repo) {}
}

public class TripleStoreDriver : RdfDbTestDriver {
  protected TripleStoreFactory itsTripleStoreFactory;
  protected TripleStore itsTripleStore;
  
  public TripleStoreDriver( TripleStoreFactory tripleStoreFactory ) {
    itsTripleStoreFactory = tripleStoreFactory;
    itsTripleStore = itsTripleStoreFactory.MakeTripleStore();
  }
  
  public override /*=RepositoryType=*/object/**/ GetRepository() {
    return itsTripleStore;
  }

	public override object CreatePrd(/*=RepositoryType=*/object/**/ repo, string uri) {
    return new UriRef( uri );
  }

	public override void RemoveStatement(/*=RepositoryType=*/object/**/ repo, /*=StmtType=*/object/**/ s) {
    // TODO
  }
	public override void AddStatement(/*=RepositoryType=*/object/**/ repo, object s, object p, object o) {
    Statement stmt = new Statement( (Node)s, (Arc)p, (Node)o);
    ((TripleStore)repo).Add( stmt );
  }
  
	public override /*=StmtItType=*/object/**/ GetStatements(/*=RepositoryType=*/object/**/ repo, object s, object p, object o) {

    if ( s == null ) {
      s = new Variable("s");
    }

    if ( p == null ) {
      p = new Variable("p");
    }


    if ( o == null ) {
      o = new Variable("o");
    }
  
    Query query =new Query();
    query.AddPattern( new Pattern( (GraphMember)s, (GraphMember)p, (GraphMember)o) );
    return ((TripleStore)repo).Solve( query );
  }

	public override object GetSub(/*=StmtType=*/object/**/ s) {
    return itsTripleStore.GetBestDenotingNode( ((QuerySolution)s)["s"] );
  }
  
	public override object GetObj(/*=StmtType=*/object/**/ s) {
    return itsTripleStore.GetBestDenotingNode( ((QuerySolution)s)["o"] );
  }
  
	public override object GetPrd(/*=StmtType=*/object/**/ s) {
    return itsTripleStore.GetBestDenotingNode( ((QuerySolution)s)["p"] );
  }
  
	public override bool IsSub(object obj) {
    return (obj is UriRef || obj is BlankNode);
  }
  
	public override bool IsObj(object obj) {
    return true;
  }
  
	public override bool IsPrd(object obj) {
    return obj is UriRef;
  }
  
	public override bool MoreStatements(/*=StmtItType=*/object/**/ it) {
    return ((IEnumerator)it).MoveNext();
  } 
  
	public override /*=StmtType=*/object/**/ NextStatement(/*=StmtItType=*/object/**/ it) {
    return ((IEnumerator)it).Current;
  }
  
	public override void CloseStmtIt(/*=StmtItType=*/object/**/ it) {
  
  }

	public override void StartTransaction(/*=RepositoryType=*/object/**/ repo) {
  
  }
	
  public override void CommitTransaction(/*=RepositoryType=*/object/**/ repo) {
  
  }
	
  public override void CloseRepository(object repo) {
    itsTripleStore.Clear();
  }



}


public class MemoryTripleStoreDriver :  TripleStoreDriver {
  public MemoryTripleStoreDriver() : base( new MemoryTripleStoreFactory() ){
  }
}

public class DatabaseTripleStoreDriver :  TripleStoreDriver {
  public DatabaseTripleStoreDriver() : base( new DatabaseTripleStoreFactory() ){
    ((DatabaseTripleStore)itsTripleStore).WriteCacheSize = 100;
//    ((DatabaseTripleStore)itsTripleStore).Verbose = true;
  }
}