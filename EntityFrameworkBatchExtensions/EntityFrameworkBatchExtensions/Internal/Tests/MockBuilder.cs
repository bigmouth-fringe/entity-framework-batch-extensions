using System;
using System.Collections.Generic;
using EntityFrameworkBatchExtensions.Internal.Tests.Models;

namespace EntityFrameworkBatchExtensions.Internal.Tests
{
    internal static class MockBuilder
    {
        public static List<TestObjModel> BuildObjs()
        {
            return new List<TestObjModel> {
                new TestObjModel {
                    Id = 123,
                    Title = "TestTitle",
                    Description = "TestDesc",
                    CreationDate = DateTime.Now,
                    Completed = true
                },
                new TestObjModel {
                    Id = 123,
                    Title = "TestTitle",
                    Description = "TestDesc",
                    CreationDate = DateTime.Now,
                    Completed = true
                },
                new TestObjModel {
                    Id = 123,
                    Title = "TestTitle",
                    Description = "TestDesc",
                    CreationDate = DateTime.Now,
                    Completed = true
                }
            };
        }
    }
}