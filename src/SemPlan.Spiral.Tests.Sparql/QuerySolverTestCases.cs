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

namespace SemPlan.Spiral.Tests.Sparql {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Sparql;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Tests of QuerySolvers expressed in Sparql syntax
	/// </summary>
  /// <remarks>
  /// These are based on the tests from the ARQ package
  /// $Id: QuerySolverTestCases.cs,v 1.1 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
	[TestFixture]
  public class QuerySolverTestCases {
  
    //~ private TripleStore GetModel0() {
      //~ MemoryTripleStore store = new MemoryTripleStore();
      //~ store.Add( new Statement(new UriRef("urn:/*not_a_comment*/") , new UriRef("http://localhost/p1"), new PlainLiteral("RDQL//RDQL") ) );
      //~ return store;
    //~ }

    //~ private void AssertQuerySolution(string query, TripleStore store, params string[] expectedResults) {
    
    //~ }

 /*

[ mf:name "test-S-01.rq" ;
  mf:action [ qt:query <test-S-01.rq> ; qt:data <modelA.nt> ] ;
  mf:result <result-S-01.n3> ]

[ mf:name "test-S-02.rq" ;
  mf:action [ qt:query <test-S-02.rq> ; qt:data <modelA.nt> ] ;
  mf:result <result-S-02.n3> ]

[ mf:name "test-S-03.rq" ;
  mf:action [ qt:query <test-S-03.rq> ; qt:data <modelA.nt> ] ;
  mf:result <result-S-03.n3> ]

[ mf:name "test-S-04.rq" ;
  mf:action [ qt:query <test-S-04.rq> ; qt:data <modelA.nt> ] ;
  mf:result <result-S-04.n3> ]

[ mf:name "test-S-05.rq" ;
  mf:action [ qt:query <test-S-05.rq> ; qt:data <modelA.nt> ] ;
  mf:result <result-S-05.n3> ]


    
[ mf:name "test-0-01.rq" ;
  mf:action [ qt:query <test-0-01.rq> ; qt:data <model0.nt> ] ;
  mf:result <result-0-01.n3> ]

[ mf:name "test-0-02.rq" ;
  mf:action [ qt:query <test-0-02.rq> ; qt:data <model0.nt> ] ;
  mf:result <result-0-02.n3> ]

[ mf:name "test-0-03.rq" ;
  mf:action [ qt:query <test-0-03.rq> ; qt:data <model0.nt> ] ;
  mf:result <result-0-03.n3> ]

[ mf:name "test-0-04.rq" ;
  mf:action [ qt:query <test-0-04.rq> ; qt:data <model0.nt> ] ;
  mf:result <result-0-04.n3> ]


[ mf:name "test-1-01.rq" ;
  mf:action [ qt:query <test-1-01.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-01.n3> ]

[ mf:name "test-1-02.rq" ;
  mf:action [ qt:query <test-1-02.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-02.n3> ]

[ mf:name "test-1-03.rq" ;
  mf:action [ qt:query <test-1-03.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-03.n3> ]

[ mf:name "test-1-04.rq" ;
  mf:action [ qt:query <test-1-04.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-04.n3> ]

[ mf:name "test-1-05.rq" ;
  mf:action [ qt:query <test-1-05.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-05.n3> ]

[ mf:name "test-1-06.rq" ;
  mf:action [ qt:query <test-1-06.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-06.n3> ]

[ mf:name "test-1-07.rq" ;
  mf:action [ qt:query <test-1-07.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-07.n3> ]

[ mf:name "test-1-08.rq" ;
  mf:action [ qt:query <test-1-08.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-08.n3> ]


## Removed : volates qname rules in SPARQL
## [ mf:name "test-1-09.rq" ;
##   mf:action [ qt:query <test-1-09.rq> ; qt:data <model1.nt> ] ;
##   mf:result <result-1-09.n3> ]

[ mf:name "test-1-10.rq" ;
  mf:action [ qt:query <test-1-10.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-1-10.n3> ]


[ mf:name "test-2-01.rq" ;
  mf:action [ qt:query <test-2-01.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-01.n3> ]

[ mf:name "test-2-02.rq" ;
  mf:action [ qt:query <test-2-02.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-02.n3> ]

[ mf:name "test-2-03.rq" ;
  mf:action [ qt:query <test-2-03.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-03.n3> ]

[ mf:name "test-2-04.rq" ;
  mf:action [ qt:query <test-2-04.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-04.n3> ]

[ mf:name "test-2-05.rq" ;
  mf:action [ qt:query <test-2-05.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-05.n3> ]

[ mf:name "test-2-06.rq" ;
  mf:action [ qt:query <test-2-06.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-06.n3> ]

[ mf:name "test-2-07.rq" ;
  mf:action [ qt:query <test-2-07.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-07.n3> ]

[ mf:name "test-2-08.rq" ;
  mf:action [ qt:query <test-2-08.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-08.n3> ]

[ mf:name "test-2-09.rq" ;
  mf:action [ qt:query <test-2-09.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-09.n3> ]

[ mf:name "test-2-10.rq" ;
  mf:action [ qt:query <test-2-10.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-2-10.n3> ]


## Testing URIs with eq : insert a str()
[ mf:name "test-3-01.rq" ;
  mf:action [ qt:query <test-3-01.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-3-01.n3> ]

[ mf:name "test-3-02.rq" ;
  mf:action [ qt:query <test-3-02.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-3-02.n3> ]

[ mf:name "test-3-03.rq" ;
  mf:action [ qt:query <test-3-03.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-3-03.n3> ]

[ mf:name "test-3-04.rq" ;
  mf:action [ qt:query <test-3-04.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-3-04.n3> ]

[ mf:name "test-3-05.rq" ;
  mf:action [ qt:query <test-3-05.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-3-05.n3> ]

[ mf:name "test-3-06.rq" ;
  mf:action [ qt:query <test-3-06.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-3-06.n3> ]

##

[ mf:name "test-3-07.rq" ;
  mf:action [ qt:query <test-3-07.rq> ; qt:data <model5.nt> ] ;
  mf:result <result-3-07.n3> ]

[ mf:name "test-4-01.rq" ;
  mf:action [ qt:query <test-4-01.rq> ; qt:data <model2.nt> ] ;
  mf:result <result-4-01.n3> ]

[ mf:name "test-4-02.rq" ;
  mf:action [ qt:query <test-4-02.rq> ; qt:data <model2.nt> ] ;
  mf:result <result-4-02.n3> ]

[ mf:name "test-4-03.rq" ;
  mf:action [ qt:query <test-4-03.rq> ; qt:data <model2.nt> ] ;
  mf:result <result-4-03.n3> ]

[ mf:name "test-4-04.rq" ;
  mf:action [ qt:query <test-4-04.rq> ; qt:data <model2.nt> ] ;
  mf:result <result-4-04.n3> ]

[ mf:name "test-4-05.rq" ;
  mf:action [ qt:query <test-4-05.rq> ; qt:data <model3.nt> ] ;
  mf:result <result-4-05.n3> ]


# Tests bNodes
## [ mf:name "test-4-06.rq" ;
##   mf:action [ qt:query <test-4-06.rq> ; qt:data <model3.nt> ] ;
##   mf:result <result-4-06.n3> ]

## String compare of a var with a URI value
[ mf:name "test-4-07.rq" ;
  mf:action [ qt:query <test-4-07.rq> ; qt:data <model3.nt> ] ;
  mf:result <result-4-07.n3> ]

## --

[ mf:name "test-5-01.rq" ;
  mf:action [ qt:query <test-5-01.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-5-01.n3> ]

[ mf:name "test-5-02.rq" ;
  mf:action [ qt:query <test-5-02.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-5-02.n3> ]

[ mf:name "test-5-03.rq" ;
  mf:action [ qt:query <test-5-03.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-5-03.n3> ]

[ mf:name "test-5-04.rq" ;
  mf:action [ qt:query <test-5-04.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-5-04.n3> ]

## --

[ mf:name "test-6-01.rq" ;
  mf:action [ qt:query <test-6-01.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-6-01.n3> ]

[ mf:name "test-6-02.rq" ;
  mf:action [ qt:query <test-6-02.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-6-02.n3> ]

[ mf:name "test-6-03.rq" ;
  mf:action [ qt:query <test-6-03.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-6-03.n3> ]

[ mf:name "test-6-04.rq" ;
  mf:action [ qt:query <test-6-04.rq> ; qt:data <model4.nt> ] ;
  mf:result <result-6-04.n3> ]

## --

[ mf:name "test-7-01.rq" ;
  mf:action [ qt:query <test-7-01.rq> ; qt:data <model6.nt> ] ;
  mf:result <result-7-01.n3> ]

[ mf:name "test-7-02.rq" ;
  mf:action [ qt:query <test-7-02.rq> ; qt:data <model6.nt> ] ;
  mf:result <result-7-02.n3> ]

[ mf:name "test-7-03.rq" ;
  mf:action [ qt:query <test-7-03.rq> ; qt:data <model7.nt> ] ;
  mf:result <result-7-03.n3> ]

[ mf:name "test-7-04.rq" ;
  mf:action [ qt:query <test-7-04.rq> ; qt:data <model7.nt> ] ;
  mf:result <result-7-04.n3> ]


## Testing URIs with =~ (needs str added)
## [ mf:name "test-8-01.rq" ;
##   mf:action [ qt:query <test-8-01.rq> ; qt:data <model1.nt> ] ;
##   mf:result <result-8-01.n3> ]
## 
## [ mf:name "test-8-02.rq" ;
##   mf:action [ qt:query <test-8-02.rq> ; qt:data <model1.nt> ] ;
##   mf:result <result-8-02.n3> ]
## 
## [ mf:name "test-8-03.rq" ;
##   mf:action [ qt:query <test-8-03.rq> ; qt:data <model1.nt> ] ;
##   mf:result <result-8-03.n3> ]
## 
## [ mf:name "test-8-04.rq" ;
##   mf:action [ qt:query <test-8-04.rq> ; qt:data <model1.nt> ] ;
##   mf:result <result-8-04.n3> ]
## 
## [ mf:name "test-8-05.rq" ;
##   mf:action [ qt:query <test-8-05.rq> ; qt:data <model1.nt> ] ;
##   mf:result <result-8-05.n3> ]


[ mf:name "test-9-01.rq" ;
  mf:action [ qt:query <test-9-01.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-9-01.n3> ]

[ mf:name "test-9-02.rq" ;
  mf:action [ qt:query <test-9-02.rq> ; qt:data <model1.nt> ] ;
  mf:result <result-9-02.n3> ]


## Testing URIs with eq
## [ mf:name "test-A-01.rq" ;
##   mf:action [ qt:query <test-A-01.rq> ; qt:data <model5.nt> ] ;
##   mf:result <result-A-01.n3> ]

## [ mf:name "test-A-02.rq" ;
##   mf:action [ qt:query <test-A-02.rq> ; qt:data <model5.nt> ] ;
##   mf:result <result-A-02.n3> ]


[ mf:name "test-B-01.rq" ;
  mf:action [ qt:query <test-B-01.rq> ; qt:data <model8.n3> ] ;
  mf:result <result-B-01.n3> ]

[ mf:name "test-B-02.rq" ;
  mf:action [ qt:query <test-B-02.rq> ; qt:data <model8.n3> ] ;
  mf:result <result-B-02.n3> ]

[ mf:name "test-B-03.rq" ;
  mf:action [ qt:query <test-B-03.rq> ; qt:data <model8.n3> ] ;
  mf:result <result-B-03.n3> ]

[ mf:name "test-B-04.rq" ;
  mf:action [ qt:query <test-B-04.rq> ; qt:data <model8.n3> ] ;
  mf:result <result-B-04.n3> ]

[ mf:name "test-B-05.rq" ;
  mf:action [ qt:query <test-B-05.rq> ; qt:data <model8.n3> ] ;
  mf:result <result-B-05.n3> ]


## Unknown type ; datatype test
##[ mf:name "test-B-06.rq" ;
##   mf:action [ qt:query <test-B-06.rq> ; qt:data <model8.n3> ] ;
##   mf:result <result-B-06.n3> ]


## Pointless
## [ mf:name "test-B-07.rq" ;
##   mf:action [ qt:query <test-B-07.rq> ; qt:data <model9.n3> ] ;
##   mf:result <result-B-07.n3> ]


[ mf:name "test-B-08.rq" ;
  mf:action [ qt:query <test-B-08.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-08.n3> ]

[ mf:name "test-B-09.rq" ;
  mf:action [ qt:query <test-B-09.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-09.n3> ]

[ mf:name "test-B-10.rq" ;
  mf:action [ qt:query <test-B-10.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-10.n3> ]

[ mf:name "test-B-11.rq" ;
  mf:action [ qt:query <test-B-11.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-11.n3> ]

[ mf:name "test-B-12.rq" ;
  mf:action [ qt:query <test-B-12.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-12.n3> ]

[ mf:name "test-B-13.rq" ;
  mf:action [ qt:query <test-B-13.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-13.n3> ]


## Duplicate of test 13 - deleted
## [ mf:name "test-B-14.rq" ; mf:action <test-B-14.rq> ; mf:result <result-B-14.n3> ]

[ mf:name "test-B-15.rq" ;
  mf:action [ qt:query <test-B-15.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-15.n3> ]

[ mf:name "test-B-16.rq" ;
  mf:action [ qt:query <test-B-16.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-16.n3> ]


[ mf:name "test-B-17.rq" ;
  mf:action [ qt:query <test-B-17.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-17.n3> ]


## Keywork "true"

[ mf:name "test-B-18.rq" ;
  mf:action [ qt:query <test-B-18.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-18.n3> ]

[ mf:name "test-B-19.rq" ;
  mf:action [ qt:query <test-B-19.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-19.n3> ]

[ mf:name "test-B-20.rq" ;
  mf:action [ qt:query <test-B-20.rq> ; qt:data <model9.n3> ] ;
  mf:result <result-B-20.n3> ]
*/

  
  
  }
  
}