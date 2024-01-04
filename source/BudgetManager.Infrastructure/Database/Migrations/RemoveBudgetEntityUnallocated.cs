namespace BudgetManager.Infrastructure.Database.Migrations;

using MongoDB.Bson;
using MongoDB.Extensions.Migration;

public class RemoveBudgetEntityUnallocated : IMigration
{
    public int Version => 1;

    public void Up(BsonDocument document)
    {
        document.Remove("Unallocated");
    }

    public void Down(BsonDocument document)
    {
    }
}
