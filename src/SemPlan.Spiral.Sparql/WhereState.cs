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

namespace SemPlan.Spiral.Sparql {
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using System;
  using System.Collections;
  using System.Text.RegularExpressions;

	/// <summary>
	/// Represents a parser of the Sparql query syntax
	/// </summary>
  /// <remarks> 
  /// $Id: WhereState.cs,v 1.4 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  internal class WhereState : State {
    private PatternCollector itsPatternCollector;
    private ArrayList itsPatternCollectorStack;

    public WhereState() {
      itsPatternCollectorStack = new ArrayList();
    }
  
    public TermGroup CurrentGroup {
      get {
        return itsPatternCollector.CurrentTermGroup;
      }
    }    
  
    public override State Handle(QueryParser parser, QueryTokenizer tokenizer, Query query) { 
      if (Explain) Console.WriteLine("Entering WHERE state");
      StartPatternGroup();
      query.PatternGroup = itsPatternCollector.PatternGroup;

      do {
        if (Explain) Console.WriteLine("  tokenizer.TokenText is " + tokenizer.TokenText  + " (" + tokenizer.Type + ")");

        if ( tokenizer.Type == QueryTokenizer.TokenType.BeginGroup ) {
          itsPatternCollector.EndTermGroup( query);
          itsPatternCollector.IncrementNestingDepth();
        }
        else if ( tokenizer.Type == QueryTokenizer.TokenType.EndGroup ) {
          if ( itsPatternCollector.HasNestedGroups ) {
            itsPatternCollector.EndTermGroup( query);
            itsPatternCollector.DecrementNestingDepth();
          }
          else {
            EndPatternGroup(query);
            if ( itsPatternCollectorStack.Count == 0) {
              return new SolutionModifierState();
            }
            else {
              itsPatternCollector.StartTermGroup();
            }
          }
        }
        else if ( tokenizer.Type == QueryTokenizer.TokenType.KeywordOptional ) {
          if ( tokenizer.MoveNext() ) {
            if (tokenizer.Type ==  QueryTokenizer.TokenType.BeginGroup) {
              itsPatternCollector.EndTermGroup( query);
              StartOptionalPatternGroup();
            }
            else {
              throw new SparqlException("Error parsing OPTIONAL declaration, expecting { but encountered '" + tokenizer.TokenText + "'", tokenizer);
            }
          }
          else {
            throw new SparqlException("Error parsing OPTIONAL declaration, expecting { but encountered end of query", tokenizer);
          }
        }
        else if ( tokenizer.Type == QueryTokenizer.TokenType.KeywordUnion ) {
          if ( tokenizer.MoveNext() ) {
            if (tokenizer.Type ==  QueryTokenizer.TokenType.BeginGroup) {
              itsPatternCollector.EndTermGroup( query);
              StartAlternatePatternGroup();
            }
            else {
              throw new SparqlException("Error parsing UNION declaration, expecting { but encountered '" + tokenizer.TokenText + "'", tokenizer);
            }
          }
          else {
            throw new SparqlException("Error parsing UNION declaration, expecting { but encountered end of query", tokenizer);
          }
        }
        else if ( tokenizer.Type == QueryTokenizer.TokenType.KeywordGraph ) {
          //TODO
        }
        else {        
          itsPatternCollector.Handle(parser, tokenizer, query);        
        } 

      } while ( tokenizer.MoveNext() ) ;

      return this;
    }      

        
      private void StartPatternGroup() {
        if (Explain) Console.WriteLine("Starting a new pattern group");
        
        PatternGroup group = new PatternGroup();
        
        itsPatternCollector = new PatternCollector(group);
        itsPatternCollector.Explain = Explain;
        itsPatternCollectorStack.Add( itsPatternCollector );
        
        itsPatternCollector.StartTermGroup();
        
      }
  
      private void StartOptionalPatternGroup() {
        if (Explain) Console.WriteLine("Starting a new optional pattern group");


        PatternGroup newGroup = new PatternGroup();
        itsPatternCollector.PatternGroup.OptionalGroup = newGroup;

        PatternCollector collector = new PatternCollector( newGroup );
        collector.Explain = Explain;
        itsPatternCollectorStack.Add( collector );
        itsPatternCollector = collector;
        
        itsPatternCollector.StartTermGroup();
      }
  
      private void StartAlternatePatternGroup() {
        if (Explain) Console.WriteLine("Starting a new alternate pattern group");


        PatternGroup newGroup = new PatternGroup();
        itsPatternCollector.PatternGroup.AddAlternateGroup(newGroup);

        PatternCollector collector = new PatternCollector( newGroup );
        collector.Explain = Explain;
        itsPatternCollectorStack.Add( collector );
        itsPatternCollector = collector;
        
        itsPatternCollector.StartTermGroup();
      }


      private void EndPatternGroup(Query query) {
        if (Explain) Console.WriteLine("Ending a pattern group");
        itsPatternCollector.ReportRemainingPatterns( query );
        
        if ( itsPatternCollectorStack.Count > 0) {
          itsPatternCollectorStack.RemoveAt( itsPatternCollectorStack.Count - 1 );
        }
  
        if ( itsPatternCollectorStack.Count > 0) {
          itsPatternCollector = (PatternCollector)itsPatternCollectorStack[ itsPatternCollectorStack.Count - 1];
        }
        
        
      }
  
      private class PatternCollector {
        private ArrayList itsTermGroupStack;
        private TermGroup itsCurrentGroup;
        private PatternGroup itsPatternGroup;
        private bool itsExplain;
        private int itsNestingDepth;
        
        public PatternCollector(PatternGroup patternGroup) {
          itsTermGroupStack = new ArrayList();
          itsPatternGroup = patternGroup;
        }
      
        public bool Explain {
          get {
            return itsExplain;
          }
          set {
            itsExplain = value;
          }
        }    
      
        public TermGroup CurrentTermGroup {
          get {
            return itsCurrentGroup;
          }
        }    
      
        public PatternGroup PatternGroup {
          get {
            return itsPatternGroup;
          }
        }    


        public void Handle(QueryParser parser, QueryTokenizer tokenizer, Query query) { 

          if ( tokenizer.Type == QueryTokenizer.TokenType.StatementTerminator) {
    
            if ( CurrentTermGroup.stackCount > 2 ) {
              ReportPattern(query);
              SucceedingPatternTermsAreNotRequired();
            }
            
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.ValueDelimiter ) {
            ReportPatternRetainSubjectAndPredicate( query );
            SucceedingPatternTermsAreNotRequired();
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.PredicateDelimiter) {
            ReportPatternRetainSubject( query );
            SucceedingPatternTermsAreNotRequired();
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.LiteralLanguage ) {
            PatternTerm member = CurrentTermGroup.popGraphMember();
            CurrentTermGroup.pushGraphMember( new PlainLiteral( member.GetLabel(), tokenizer.TokenText) );
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.LiteralDatatype ) {
            PatternTerm member = CurrentTermGroup.popGraphMember();
            CurrentTermGroup.pushGraphMember( new TypedLiteral( member.GetLabel(), tokenizer.TokenText) );
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.KeywordFilter ) {
            EndTermGroup( query);
            ParseFilter( tokenizer, PatternGroup );
            StartTermGroup( );
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.BeginGeneratedBlankNode ) {
            BlankNode node = new BlankNode();
            CurrentTermGroup.pushGraphMember( node );
            StartBlankNodeGroup();
            CurrentTermGroup.pushGraphMember( node );
          }
          else if ( tokenizer.Type == QueryTokenizer.TokenType.EndGeneratedBlankNode ) {
            EndTermGroup( query);
          }
          else {
            PatternTerm subjectMember = parser.ParseTokenToMember( query );
            CurrentTermGroup.pushGraphMember( subjectMember );
            SucceedingPatternTermsAreRequired();
          }
        }
        
        public void StartTermGroup() {
          if (Explain) Console.WriteLine("Starting a new term group");
          itsCurrentGroup = new TermGroup();
          itsTermGroupStack.Add( itsCurrentGroup );
        }
  
        public void EndTermGroup(Query query ) {
          if (Explain) Console.WriteLine("Ending an term group");

          if ( itsCurrentGroup.stackCount > 2 ) {
            ReportPattern( query );
          }
    
          if ( itsTermGroupStack.Count > 0) {
            itsTermGroupStack.RemoveAt( itsTermGroupStack.Count - 1 );
          }
    
          if ( itsTermGroupStack.Count > 0) {
            itsCurrentGroup = (TermGroup)itsTermGroupStack[ itsTermGroupStack.Count - 1];
          }
          
        }

        public void ReportPatternRetainSubjectAndPredicate(Query query) {
          PatternTerm valueMember = CurrentTermGroup.popGraphMember();
          PatternTerm predicateMember = CurrentTermGroup.popGraphMember();
          PatternTerm subjectMember = CurrentTermGroup.popGraphMember();      
    
          Pattern pattern = new Pattern( subjectMember, predicateMember, valueMember); 
          PatternGroup.AddPattern( pattern );
    
          CurrentTermGroup.pushGraphMember( subjectMember );
          CurrentTermGroup.pushGraphMember( predicateMember );
        }    
    
        public void ReportPatternRetainSubject(Query query) {
          PatternTerm valueMember = CurrentTermGroup.popGraphMember();
          PatternTerm predicateMember = CurrentTermGroup.popGraphMember();
          PatternTerm subjectMember = CurrentTermGroup.popGraphMember();      
    
          Pattern pattern = new Pattern( subjectMember, predicateMember, valueMember); 
          PatternGroup.AddPattern( pattern );
    
          CurrentTermGroup.pushGraphMember( subjectMember );
        }    
    
        public void ReportPattern(Query query) {
          if (Explain) Console.WriteLine("Reporting a new pattern");
          PatternTerm valueMember = CurrentTermGroup.popGraphMember();
          PatternTerm predicateMember = CurrentTermGroup.popGraphMember();
          PatternTerm subjectMember = CurrentTermGroup.popGraphMember();      
    
          Pattern pattern = new Pattern( subjectMember, predicateMember, valueMember); 
          if (Explain) Console.WriteLine("Pattern is " + pattern);
          PatternGroup.AddPattern( pattern );
        }
        
        public void ReportRemainingPatterns(Query query) {
          if (Explain) Console.WriteLine("Reporting remaining patterns");
          if (Explain) Console.WriteLine(CurrentTermGroup.stackCount + " terms still on stack");
          while ( itsTermGroupStack.Count > 0) {
            EndTermGroup( query);
          }
        }


        public void StartBlankNodeGroup() {
          itsCurrentGroup = new TermGroup();
          itsTermGroupStack.Add( itsCurrentGroup );
        }

        private void SucceedingPatternTermsAreNotRequired() {
          CurrentTermGroup.RequiresPatternTerm = false;
        }

        private void SucceedingPatternTermsAreRequired() {
          CurrentTermGroup.RequiresPatternTerm = true;
        }

        private void  ParseFilter( QueryTokenizer tokenizer, PatternGroup group ) {
          while (tokenizer.MoveNext() ) {
            switch (tokenizer.Type) {
              case QueryTokenizer.TokenType.BeginGroup:
                break;
              case QueryTokenizer.TokenType.EndGroup:
                return;
              case QueryTokenizer.TokenType.Variable:
                Constraint constraint = new Constraint( new VariableExpression( new Variable( tokenizer.TokenText ) ) );
                group.AddConstraint( constraint );
                break;
            }
          }
        }  

        public void IncrementNestingDepth() {
          ++itsNestingDepth;
        }        

        public void DecrementNestingDepth() {
          --itsNestingDepth;
        }   

        public bool HasNestedGroups {
          get { return (itsNestingDepth > 0); }
        }               

      }
  
  
    }


    /******************************************************************************************/
    public class TermGroup {
      private ArrayList itsGraphMemberStack;
      private bool itsRequiresPatternTerm;
      
      public TermGroup() {
        itsGraphMemberStack = new ArrayList();
        itsRequiresPatternTerm = false;
      }
  
      public void pushGraphMember( PatternTerm member ) {
        itsGraphMemberStack.Add( member );
      }

      public PatternTerm popGraphMember() {
        PatternTerm member = (PatternTerm)itsGraphMemberStack[ itsGraphMemberStack.Count - 1];
        itsGraphMemberStack.RemoveAt( itsGraphMemberStack.Count - 1 );
        return member;
      }

      public int stackCount {
        get { return itsGraphMemberStack.Count; }
      }
      
      public bool RequiresPatternTerm {
        get { return itsRequiresPatternTerm; } 
        set { itsRequiresPatternTerm = value; } 
      }
    
    }
}
