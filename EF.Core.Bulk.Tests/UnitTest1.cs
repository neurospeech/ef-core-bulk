using EF.Core.Bulk.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EF.Core.Bulk.Tests
{
    public class UnitTest1: BaseTest
    {
        public UnitTest1(ITestOutputHelper writer) : base(writer)
        {

        }

        [Fact]
        public async Task BulkInsert()
        {
            using (var db = this.CreateContext()) {

                var googleProducts = db.Products.Where(p => p.Name.StartsWith("google"));

                int count = await db.InsertAsync(googleProducts.Select(p => new Product {
                    Name = p.Name + " V2"
                }));

                Assert.Equal(2, count);

                count = await db.Products.CountAsync();

                Assert.Equal(6, count);

                count = await googleProducts.Select(p => new Product {
                    Archived = true
                }).UpdateAsync();

                Assert.Equal(4, count);

                count = await db.Products.Where(x => x.Archived == true).CountAsync();

                Assert.Equal(4, count);

                count = await db.DeleteAsync(db.Products.Where(x => x.Archived == true));

                Assert.Equal(4, count);

                count = await db.Products.Where(x => x.Archived == true).CountAsync();

                Assert.Equal(0, count);

                count = await db.Products.CountAsync();

                Assert.Equal(2, count);
            }
        }
    }
}
