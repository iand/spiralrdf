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
	/// <summary>
	/// Represents a query on a TripleStore
	/// </summary>
  /// <remarks>
  /// $Id: BacktrackingQuerySolver.cs,v 1.3 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public class BacktrackingQuerySolver : QuerySolver, IEnumerator {
    private Hashtable itsRequestedVariables;
    private Hashtable itsDistinctSolutions;
    private Query itsQuery;
    private ArrayList itsSolutions;
    private int itsSolutionIndex;
    
    private bool itsExplain;
    public bool Explain {
      get {
        return itsExplain;
      }
      set {
        itsExplain = value;
      }
    }    
    
    public BacktrackingQuerySolver( Query query, TripleStore triples ) :this( query, triples, false) {

    }

    public BacktrackingQuerySolver( Query query, TripleStore triples, bool explain ) {
      itsExplain = explain;
      itsQuery = query;
      itsSolutions = new ArrayList();
      
      itsDistinctSolutions = new Hashtable();
      itsRequestedVariables = new Hashtable();

      if (Explain) Console.WriteLine("Collecting variables");
      CollectVariablesToBeSelected();

      try {

        ArrayList bindingsList = GetSolutions( triples, itsQuery.ResolveQueryGroup( triples ), new Bindings() );
        if ( query.IsOrdered ) {
          if (Explain) Console.WriteLine("Sorting results");
          bindingsList.Sort( new BindingsComparer( query.OrderBy ) );
          if ( query.OrderDirection == Query.SortOrder.Descending ) {
            bindingsList.Reverse();
          }
        }


        foreach ( Bindings bindings in bindingsList ) {
          QuerySolution solution = GetSolution( bindings, itsRequestedVariables, triples); 
          if ( IsDistinctSolution( solution ) ) {
            if (Explain) Console.WriteLine("Reporting solution " + solution);
            itsSolutions.Add( solution );
          }
        }
        
        
      }
      catch (UnknownGraphMemberException) { }

      itsSolutionIndex = -1;
    }

    public ArrayList GetSolutions( TripleStore store, QueryGroup group, Bindings bindings) {
      if (Explain) Console.WriteLine("Solving " + group.GetType());

      if ( group is QueryGroupOr ) {
        if (Explain) Console.WriteLine("Solving QueryGroupOr");
        ArrayList solutions = new ArrayList();
        foreach (QueryGroup subgroup in ((QueryGroupOr)group).Groups) {
          solutions.AddRange( GetSolutions( store, subgroup, bindings) );
        }
        if (Explain) Console.WriteLine("Have " + solutions.Count + " candidate solutions after or");
        
        return solutions;      
      }
      else if (group is QueryGroupAnd) {
        ArrayList solutions = new ArrayList();
        solutions.Add( bindings );
        
        foreach (QueryGroup subgroup in ((QueryGroupAnd)group).Groups) {
          ArrayList filteredSolutions = new ArrayList();
          foreach (Bindings tempBindings in solutions)  {
            filteredSolutions.AddRange( GetSolutions( store, subgroup, tempBindings ) );
          }
          solutions = filteredSolutions;
          if (Explain) Console.WriteLine("Have " + solutions.Count + " candidate solutions after " + subgroup.GetType());
        }
        return solutions;      
      }
      
      else if ( group is QueryGroupOptional) {
        if (Explain) Console.WriteLine("Solving QueryGroupOptional");
        ArrayList augmentedSolutions = GetSolutions( store, ((QueryGroupOptional)group).Group, bindings );
      
        if ( augmentedSolutions.Count == 0)  {
          augmentedSolutions.Add( bindings );
        }
        
        return augmentedSolutions;
      }
      else if ( group is QueryGroupConstraints  ) {
        if (Explain) Console.WriteLine("Solving QueryGroupConstraints");
        ArrayList solutions = new ArrayList();
        foreach ( Constraint constraint in ((QueryGroupConstraints)group).Constraints) {
          if (Explain) Console.WriteLine("Testing solution against constraint {0}", constraint);
          if (! constraint.SatisfiedBy( bindings ) ) {
            if (Explain) Console.WriteLine("Failed to satisfy constraint");
            return solutions;
          }
        }
  
        solutions.Add( bindings );
        return solutions;
      }
      else if (group is QueryGroupPatterns) {
        if (Explain) Console.WriteLine("Solving QueryGroupPatterns");
        ArrayList solutions = new ArrayList();
        
        if ( ((QueryGroupPatterns)group).Patterns.Count == 0) {
          return solutions;
        }
        
        Bindings answers = new Bindings();
        
        Pattern firstGoal = ((QueryGroupPatterns)group).First();
        Pattern newGoal = firstGoal.Substitute( bindings );
        
        IEnumerator statements = store.GetStatementEnumerator();
        while ( statements.MoveNext() ) {
          ResourceStatement statement = (ResourceStatement)statements.Current;
          
          if (Explain) Console.WriteLine("Trying for a binding for {0} with {1}", statement, newGoal);
          answers = statement.Unify(newGoal, bindings, store);
  
          if ( answers != null ) {
            if (Explain) Console.WriteLine("Got a binding for {0} with {1}", statement, newGoal);
            if (((QueryGroupPatterns)group).Patterns.Count == 1) {
              if (Explain) Console.WriteLine("Got a candidate solution {0}", answers);
              solutions.Add( answers );
            }
            else {
              solutions.AddRange( GetSolutions(store,  ((QueryGroupPatterns)group).Rest(), answers) );
            }
          }
          
        }
        
        return solutions;
      }        
      else {
        return new ArrayList();
      }
      
    }
    

    private QuerySolution GetSolution(Bindings bindings, Hashtable requiredVariables, TripleStore store) {
      QuerySolution solution = new QuerySolution();
      IDictionaryEnumerator en = bindings.GetEnumerator();

      while (en.MoveNext() ) {
        if ( requiredVariables.ContainsKey( ((Variable)en.Key).Name ) ) {
          solution[ ((Variable)en.Key).Name   ] = (Resource)en.Value;
          solution.SetNode( ((Variable)en.Key).Name , store.GetBestDenotingNode( (Resource)en.Value ) );
        }
      }
      return solution;
    }

    public ArrayList GenerateGoals(PatternGroup group, TripleStore store) {
      ArrayList goals = new ArrayList();
      
      foreach (Pattern pattern in group.Patterns) {
        Pattern goal = pattern.Resolve( store );
        if ( null != goal ) {
          goals.Add( goal );
        }
        else {
          if (Explain) Console.WriteLine("Pattern {0} cannot be matched in this triple store since it contains unknown terms", pattern);
          return new ArrayList();
        }
      }
            
      return goals;
    }


    public bool MoveNext( ) {
      ++itsSolutionIndex;
      return ( itsSolutionIndex < itsSolutions.Count );
    }
           
    public bool IsDistinctSolution(QuerySolution solution) {
      if (! itsQuery.IsDistinct) {
        return true;
      } 

      if (itsDistinctSolutions.ContainsKey( solution ) ) {
        return false;
      }
      else {
        itsDistinctSolutions[solution] = solution;
        return true;
      }
    }


    public object Current {
      get {
        return (QuerySolution)itsSolutions[itsSolutionIndex];
      }
    }
    
    public void Reset() {
      throw new NotSupportedException("Reset not supported");
    }
    

    public void CollectVariablesToBeSelected() {
      foreach (Variable variable in itsQuery.Variables ) {
        itsRequestedVariables[ variable.Name ] = variable;
      }
    }
   
   
  }  
}