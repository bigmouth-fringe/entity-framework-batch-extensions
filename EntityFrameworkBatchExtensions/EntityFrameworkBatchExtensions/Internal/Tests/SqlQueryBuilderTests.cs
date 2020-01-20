using System.Collections.Generic;
using System.Linq;
using EntityFrameworkBatchExtensions.Internal.Tests.Models;
using NUnit.Framework;

namespace EntityFrameworkBatchExtensions.Internal.Tests
{
    [TestFixture]
    internal class SqlQueryBuilderTests
    {
        private readonly string _tableName = "TestObjModels";
        private List<TestObjModel> _objs;

        [SetUp]
        public void SetUp()
        {
            _objs = MockBuilder.BuildObjs();
        }

        [Test]
        public void BuildCreateQuery_CorrectParamsPassed_CorrectQueryReturned()
        {
            // Arrange & Act
            var props = typeof(TestObjModel).GetProperties().ToList();
            var sql = SqlQueryBuilder.BuildCreateQuery(_tableName, props, _objs);
            
            // Assert
            Assert.AreNotEqual(string.Empty, sql);
        }
        
        [Test]
        public void BuildUpdateQuery_CorrectParamsPassed_CorrectQueryReturned()
        {
            // Arrange & Act
            var objIds = _objs.Select(o => o.Id).ToList();
            var props = typeof(TestObjModel).GetProperties().ToList();
            var obj = _objs.First();
            var sql = SqlQueryBuilder.BuildUpdateQuery(_tableName, objIds, props, obj);
            
            // Assert
            Assert.AreNotEqual(string.Empty, sql);
        }
        
        [Test]
        public void BuildDeleteQuery_CorrectParamsPassed_CorrectQueryReturned()
        {
            // Arrange & Act
            var objIds = _objs.Select(o => o.Id).ToList();
            var sql = SqlQueryBuilder.BuildDeleteQuery(_tableName, objIds);
            
            // Assert
            Assert.AreNotEqual(string.Empty, sql);
        }
    }
}