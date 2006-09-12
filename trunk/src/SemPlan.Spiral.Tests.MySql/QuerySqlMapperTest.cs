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

namespace SemPlan.Spiral.Tests.MySql {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using SemPlan.Spiral.MySql;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  using SemPlan.Spiral.Tests.Core;
  
	/// <summary>
	/// Programmer tests for DatabaseQuerySolver class
	/// </summary>
  /// <remarks>
  /// $Id: QuerySqlMapperTest.cs,v 1.2 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
	[TestFixture]
  public class QuerySqlMapperTest {

    [Test]
    public void SingleStatementSingleVariableAsObject() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void SingleStatementSingleVariableAsSubject() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


    [Test]
    public void SingleStatementSingleVariableAsPredicate() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new Variable("v"), new UriRef("ex:o") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.predicateHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


    [Test]
    public void SingleStatementSingleVariableWithArbitraryName() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("hula") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_hula.resourceHash rh_hula, rn_hula.nodeHash nh_hula, rn_hula.nodeType nt_hula, COALESCE(u_hula.uri, pl_hula.value, l_hula.value) val_hula, COALESCE(tl_hula.value, t_hula.value) sub_hula " +
          "FROM Statements s1 JOIN ResourceNodes rn_hula ON rn_hula.resourceHash=s1.objectHash AND rn_hula.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_hula ON rn_hula.nodeHash=u_hula.hash AND rn_hula.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_hula ON rn_hula.nodeHash=pl_hula.hash AND rn_hula.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_hula ON pl_hula.languageHash=l_hula.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_hula ON rn_hula.nodehash=tl_hula.hash AND rn_hula.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_hula ON tl_hula.datatypeHash=t_hula.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


    [Test]
    public void SingleStatementTwoVariables() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("s"), new UriRef("ex:p"), new Variable("o") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_s.resourceHash rh_s, rn_s.nodeHash nh_s, rn_s.nodeType nt_s, u_s.uri val_s, NULL sub_s, rn_o.resourceHash rh_o, rn_o.nodeHash nh_o, rn_o.nodeType nt_o, COALESCE(u_o.uri, pl_o.value, l_o.value) val_o, COALESCE(tl_o.value, t_o.value) sub_o " +
          "FROM Statements s1 JOIN ResourceNodes rn_s ON rn_s.resourceHash=s1.subjectHash AND rn_s.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_s ON rn_s.nodeHash=u_s.hash AND rn_s.nodeType='u' " +
          "JOIN ResourceNodes rn_o ON rn_o.resourceHash=s1.objectHash AND rn_o.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_o ON rn_o.nodeHash=u_o.hash AND rn_o.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_o ON rn_o.nodeHash=pl_o.hash AND rn_o.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_o ON pl_o.languageHash=l_o.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_o ON rn_o.nodehash=tl_o.hash AND rn_o.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_o ON tl_o.datatypeHash=t_o.hash " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

 
     [Test]
    public void TwoStatementsSingleVariableAsSubject() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p2"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p2"), new UriRef("ex:o") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 " +
          "JOIN Statements s2 ON s2.subjectHash=s1.subjectHash" +
          " AND s2.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p2") ).GetHashCode() +
          " AND s2.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s2.graphId=" + statements.GetHashCode() +
          " JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


     [Test]
    public void TwoStatementsSingleVariableAsPredicate() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p2"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new Variable("v"), new UriRef("ex:o") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 " +
          "JOIN Statements s2" +
          " ON s2.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s2.predicateHash=s1.subjectHash" +
          " AND s2.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s2.graphId=" + statements.GetHashCode() +
          " JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


     [Test]
    public void TwoStatementsSingleVariableAsObject() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p2"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p2"), new Variable("v") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 " +
          "JOIN Statements s2" +
          " ON s2.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s2.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p2") ).GetHashCode() +
          " AND s2.objectHash=s1.subjectHash" +
          " AND s2.graphId=" + statements.GetHashCode() + 
          " JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


    [Test]
    public void Distinct() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      Query query = builder.GetQuery();
      query.IsDistinct = true;
      
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT DISTINCT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


    [Test]
    public void NoPatterns() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      Query query = builder.GetQuery();
      
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      Assert.AreEqual( false, mapper.IsFeasible);
    }
    
    
    [Test]
    public void OrderBySimpleVariableExpressionAscending() {
       MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      Query query = builder.GetQuery();
      query.OrderBy = new VariableExpression( new Variable("v" ) );
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode() +
          " ORDER BY val_v";
  
  

      Assert.AreEqual(  expected, mapper.Sql );
    }
    

    [Test]
    public void OrderBySimpleVariableExpressionDescending() {
       MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      Query query = builder.GetQuery();
      query.OrderBy = new VariableExpression( new Variable("v" ) );
      query.OrderDirection = Query.SortOrder.Descending;
      
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode() +
          " ORDER BY val_v DESC";
  
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void SingleStatementPlusOptionalStatementSharedSubject() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p2"), new UriRef("ex:o2") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddOptional( new Pattern( new Variable("v"),  new UriRef("ex:p2"), new UriRef("ex:o2") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 " +
          "LEFT OUTER JOIN Statements s2" +
          " ON s2.subjectHash=s1.subjectHash" +
          " AND s2.predicateHash=" +  statements.GetResourceDenotedBy( new UriRef("ex:p2") ).GetHashCode()  + 
          " AND s2.objectHash=" +  statements.GetResourceDenotedBy( new UriRef("ex:o2") ).GetHashCode()  + 
          " AND s2.graphId=" + statements.GetHashCode() +
          " JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void SingleStatementPlusUnmatchableOptionalStatementSharedSubject() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddOptional( new Pattern( new Variable("v"),  new UriRef("ex:p2"), new UriRef("ex:o2") ) );
      Query query = builder.GetQuery();

      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void SingleStatementPlusMultipleOptionalStatementsInOneGroup() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p2"), new UriRef("ex:o2") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddOptional( new Pattern( new Variable("v"),  new Variable("p"), new UriRef("ex:o2") ) );
      builder.AddOptional( new Pattern( new Variable("v"),  new Variable("p"), new UriRef("ex:o") ) );
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v, rn_p.resourceHash rh_p, rn_p.nodeHash nh_p, rn_p.nodeType nt_p, u_p.uri val_p, NULL sub_p " +
          "FROM Statements s1 " +
          "LEFT OUTER JOIN Statements s2" +
          " ON s2.subjectHash=s1.subjectHash" +
          " AND s2.objectHash=" +  statements.GetResourceDenotedBy( new UriRef("ex:o2") ).GetHashCode()  + 
          " AND s2.graphId=" + statements.GetHashCode() +
          " JOIN Statements s3" +
          " ON s3.subjectHash=s2.subjectHash" +
          " AND s3.predicateHash=s2.predicateHash" + 
          " AND s3.objectHash=" +  statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode()  + 
          " AND s3.graphId=" + statements.GetHashCode() +
          " JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "JOIN ResourceNodes rn_p ON rn_p.resourceHash=s2.predicateHash AND rn_p.graphId=s2.graphId " +
          "LEFT OUTER JOIN UriRefs u_p ON rn_p.nodeHash=u_p.hash AND rn_p.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void SingleStatementPlusUnmatchableOptionalStatementWithVariable() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("v"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      builder.AddOptional( new Pattern( new Variable("v"),  new UriRef("ex:p2"), new Variable("o") ) );
      Query query = builder.GetQuery();

      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, u_v.uri val_v, NULL sub_v, NULL rh_o, NULL nh_o, NULL nt_o, NULL val_o, NULL sub_o " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.subjectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "WHERE s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.objectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:o") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode();
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void ConstraintSimpleVariableExpressionIsLiteral() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      builder.AddConstraint( new Constraint( new IsLiteral( new VariableExpression( new Variable("v") ) ) ) ); 
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode() +
          " AND (nt_v='p' OR nt_v='t')";
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void ConstraintSimpleVariableExpressionIsIri() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      builder.AddConstraint( new Constraint( new IsIri( new VariableExpression( new Variable("v") ) ) ) ); 
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode() +
          " AND nt_v='u'";
  

      Assert.AreEqual(  expected, mapper.Sql );
    }


    [Test]
    public void ConstraintSimpleVariableExpressionIsBlank() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      builder.AddConstraint( new Constraint( new IsBlank( new VariableExpression( new Variable("v") ) ) ) ); 
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode() +
          " AND nt_v='b'";
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

    [Test]
    public void ConstraintBound() {
      MemoryTripleStore statements = new MemoryTripleStore();
      statements.Add( new Statement( new UriRef("ex:s"), new UriRef("ex:p"), new UriRef("ex:o") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("ex:s"), new UriRef("ex:p"), new Variable("v") ) );
      builder.AddConstraint( new Constraint( new Bound( new Variable("v") ) ) ); 
      Query query = builder.GetQuery();
  
      QuerySqlMapper mapper =  new QuerySqlMapper(query, statements);

      string expected = 
          "SELECT rn_v.resourceHash rh_v, rn_v.nodeHash nh_v, rn_v.nodeType nt_v, COALESCE(u_v.uri, pl_v.value, l_v.value) val_v, COALESCE(tl_v.value, t_v.value) sub_v " +
          "FROM Statements s1 JOIN ResourceNodes rn_v ON rn_v.resourceHash=s1.objectHash AND rn_v.graphId=s1.graphId " +
          "LEFT OUTER JOIN UriRefs u_v ON rn_v.nodeHash=u_v.hash AND rn_v.nodeType='u' " +
          "LEFT OUTER JOIN PlainLiterals pl_v ON rn_v.nodeHash=pl_v.hash AND rn_v.nodeType='p' " +
          "LEFT OUTER JOIN Languages l_v ON pl_v.languageHash=l_v.hash " +
          "LEFT OUTER JOIN TypedLiterals tl_v ON rn_v.nodehash=tl_v.hash AND rn_v.nodeType='t' " +
          "LEFT OUTER JOIN DataTypes t_v ON tl_v.datatypeHash=t_v.hash " +
          "WHERE s1.subjectHash=" + statements.GetResourceDenotedBy( new UriRef("ex:s") ).GetHashCode() +
          " AND s1.predicateHash=" + statements.GetResourceDenotedBy( new UriRef("ex:p") ).GetHashCode() +
          " AND s1.graphId=" + statements.GetHashCode() +
          " AND s1.objectHash IS NOT NULL";
  

      Assert.AreEqual(  expected, mapper.Sql );
    }

  }  
}