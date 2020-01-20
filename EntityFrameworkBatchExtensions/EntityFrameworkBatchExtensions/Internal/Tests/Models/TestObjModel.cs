using System;

namespace EntityFrameworkBatchExtensions.Internal.Tests.Models
{
    public class TestObjModel
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Completed { get; set; }
    }
}