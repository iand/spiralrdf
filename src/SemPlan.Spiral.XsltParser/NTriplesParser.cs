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


using System;
using System.Text.RegularExpressions;
using SemPlan.Spiral.Core;

namespace SemPlan.Spiral.XsltParser
{
	/// <summary>
	/// A parser that reads and splits a line containing an N-Triple.	
	/// </summary>
public class NTriplesParser{

	private static Regex tripleRegex = new Regex(@"^(\S+)\s+(\S+)\s+(\S.*\S)\s*\.\s*$", RegexOptions.Compiled); 
	private static Regex commentRegex = new Regex(@"^#", RegexOptions.Compiled); 
	//private static Regex literal = new Regex(@"^""(\S+)""\^\^(\S+)$", RegexOptions.Compiled);
	private static Regex literal = new Regex(@"^""(.*)""\^\^<(.+)>$", RegexOptions.Compiled);
	//private static Regex literalUntypedLang = new Regex(@"^""([^""]+)""\@(\S+)$", RegexOptions.Compiled);
	private static Regex literalUntypedLang = new Regex(@"^""(.*)""\@(.+)$", RegexOptions.Compiled);
	//private static Regex literalUntyped = new Regex(@"^""([^""]+)""$", RegexOptions.Compiled);
	private static Regex literalUntyped = new Regex(@"^""(.*)""$", RegexOptions.Compiled);
	//private static Regex anonymous = new Regex(@"^_:(\S+)$", RegexOptions.Compiled);
	private static Regex anonymous = new Regex(@"^_:(.+)$", RegexOptions.Compiled);
	//private static Regex regular = new Regex(@"^<([^>]+)>$", RegexOptions.Compiled);
	private static Regex regular = new Regex(@"^<(.+)>$", RegexOptions.Compiled);
	private static Match match;
	private SemPlan.Spiral.Core.StatementFactory statementFactory;
	private SemPlan.Spiral.Core.ResourceFactory resourceFactory;

	public NTriplesParser(){
	}
		
	public NTriplesParser(SemPlan.Spiral.Core.ResourceFactory resourceFactory, SemPlan.Spiral.Core.StatementFactory statementFactory) {
			this.resourceFactory = resourceFactory;
			this.statementFactory = statementFactory;
	}

	/// <summary>
	/// Assign the factory the parser must use to create resources
	/// </summary>
	public void setResourceFactory(SemPlan.Spiral.Core.ResourceFactory factory) {
		this.resourceFactory = factory;
	}

	/// <summary>
	/// Assign the factory the parser must use to create statements from resources
	/// </summary>
	public void setStatementFactory(SemPlan.Spiral.Core.StatementFactory factory) {
		this.statementFactory = factory;
	}
	
	public SemPlan.Spiral.Core.Statement ParseTriple (string tripleLine) {
		if (!commentRegex.IsMatch(tripleLine)) {
			match = tripleRegex.Match(tripleLine);
			if (match.Success) {
				string subjectString = match.Groups[1].Value;
				string predicateString = match.Groups[2].Value;
				string objectString = match.Groups[3].Value;
				return MakeStatement(subjectString, predicateString, objectString);
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
	
	public SemPlan.Spiral.Core.Statement MakeStatement(string subjectString, string predicateString, string objectString) {
		Object subject = parseResource(subjectString);
		Object predicate = parseResource(predicateString);
		Object value = parseResource(objectString);
		if (subject != null && predicate != null && value != null && predicate is UriRef) {
			if (subject is UriRef) {
				if (value is UriRef)
					return statementFactory.MakeStatement((UriRef)subject, (UriRef)predicate, (UriRef)value);
				else if (value is BlankNode)
					return statementFactory.MakeStatement((UriRef)subject, (UriRef)predicate, (BlankNode)value);
				else if (value is PlainLiteral)
					return statementFactory.MakeStatement((UriRef)subject, (UriRef)predicate, (PlainLiteral)value);
				else if (value is TypedLiteral)
					return statementFactory.MakeStatement((UriRef)subject, (UriRef)predicate, (TypedLiteral)value);
			} else if (subject is BlankNode) {
				if (value is UriRef)
					return statementFactory.MakeStatement((BlankNode)subject, (UriRef)predicate, (UriRef)value);
				else if (value is BlankNode)
					return statementFactory.MakeStatement((BlankNode)subject, (UriRef)predicate, (BlankNode)value);
				else if (value is PlainLiteral)
					return statementFactory.MakeStatement((BlankNode)subject, (UriRef)predicate, (PlainLiteral)value);
				else if (value is TypedLiteral)
					return statementFactory.MakeStatement((BlankNode)subject, (UriRef)predicate, (TypedLiteral)value);
			}
		}	
		return null;
	}
	

	public Object parseResource(string lexicalValue) {
		match = literal.Match(lexicalValue);
		if (match.Success) 
			return resourceFactory.MakeTypedLiteral(match.Groups[1].Value, match.Groups[2].Value);

		match = literalUntypedLang.Match(lexicalValue);
		if (match.Success) 
			return resourceFactory.MakePlainLiteral(match.Groups[1].Value, match.Groups[2].Value);

		match = literalUntyped.Match(lexicalValue);
		if (match.Success)
			return resourceFactory.MakePlainLiteral(match.Groups[1].Value);

		match = anonymous.Match(lexicalValue);
		if (match.Success)
			return resourceFactory.MakeBlankNode(match.Groups[1].Value);

		match = regular.Match(lexicalValue);
		if (match.Success)
			return resourceFactory.MakeUriRef(match.Groups[1].Value);

		return null;
	}
		
}
}
