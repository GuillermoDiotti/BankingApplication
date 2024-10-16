using Microsoft.EntityFrameworkCore;
using Repository;

namespace LogicaTest.Context;

public class SqlContextFactory
{
    public SqlContext CreateMemoryContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
        optionsBuilder.UseInMemoryDatabase("TestDB");
        
        return new SqlContext(optionsBuilder.Options);
    }
}