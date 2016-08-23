// ReSharper disable ArgumentsStyleLiteral
namespace PhoneVerification.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CreateNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Numbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        VerificationCode = c.String(),
                        Verified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Numbers");
        }
    }
}
