#region Copyright (c) 2004 Ian Davis & James Carlyle
/*----------------------------------------------------------------------------------
Copyright © 2004 Ian Davis & James Carlyle

This software is provided 'as-is', without any express or implied warranty. In no 
event will the author(s) be held liable for any damages arising from the use of this 
software.
 
Permission is granted to anyone to use this software for any purpose, including 
commercial applications, and to alter it and redistribute it freely, subject to the 
following restrictions:

1. The origin of this software must not be misrepresented; you must not claim that 
   you wrote the original software. If you use this software in a product, an 
   acknowledgment (see the following) in the product documentation is required.

   Portions Copyright © 2004 Ian Davis & James Carlyle

2. Altered source versions must be plainly marked as such, and must not be 
   misrepresented as being the original software.

3. This notice may not be removed or altered from any source distribution.
----------------------------------------------------------------------------------*/
#endregion
using SemPlan.Spiral.Core;
using SemPlan.Spiral.Utility;
using SemPlan.Spiral.XsltParser;
using System;
using System.Collections;
using System.IO;
using System.Xml;


/// <summary>
/// Dumps out some RDF as RDF/XML
/// </summary>
/// <remarks>
/// $Id: Query.cs,v 1.1 2005/10/03 15:52:53 ian Exp $
///</remarks>
public class QueryExample {
    
	static void Main(string[] args) {
	  ParserFactory parserFactory = new XsltParserFactory();
    TripleStoreFactory tripleStoreFactory = new MemoryTripleStoreFactory();

    TripleStore model = tripleStoreFactory.MakeTripleStore();
    
    // Parse the bloggers RDF file
    FileStream fileStream = new FileStream("./bloggers.rdf", FileMode.Open);

    Parser parser = parserFactory.MakeParser( tripleStoreFactory.MakeResourceFactory(), new StatementFactory());
    parser.NewStatement += model.GetStatementHandler();
    parser.Parse(  fileStream, "" );
    parser.NewStatement -= model.GetStatementHandler();


    /* Define the query. Equivalent to:
            ?blogger rdf:type foaf:Agent .
            ?blogger foaf:name ?name .
        */
    Query queryListOfBloggers = new Query();
    queryListOfBloggers.AddPattern( new Pattern( new Variable("blogger"), new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://xmlns.com/foaf/0.1/Agent") ) );
    queryListOfBloggers.AddPattern( new Pattern( new Variable("blogger"), new UriRef("http://xmlns.com/foaf/0.1/name"), new Variable("name") ) );

    IEnumerator solutions = model.Solve( queryListOfBloggers );
    while ( solutions.MoveNext() ) {
      QuerySolution solution = (QuerySolution)solutions.Current;
      Console.WriteLine( solution["name"] );
    }    
  
	
	}



}
