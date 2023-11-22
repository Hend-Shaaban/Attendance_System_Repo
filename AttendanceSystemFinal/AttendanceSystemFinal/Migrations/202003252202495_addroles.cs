namespace AttendanceSystemFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addroles : DbMigration
    {
        public override void Up()
        {
            Sql("insert into AspNetRoles values(1,'Admin')");
            Sql("insert into AspNetRoles values(2,'Secureity')");
            Sql("insert into AspNetRoles values(3,'Student')");
            Sql("insert into Departments values('SD')");
            Sql("insert into Departments values('Mobile')");
            Sql("insert into Departments values('SA')");
            Sql("insert into Departments values('OS')");
            Sql("insert into Departments values('EL')");
            Sql("insert into Students values('Aya Gamal',1,'aya1@Student.com')");
            Sql("insert into Students values('Ahmed Gamal',1,'ahmed2@Student.com')");
            Sql("insert into Students values('Aya Fathy',2,'aya3@Student.com')");
            Sql("insert into Students values('Hnd Mohamed',3,'hnd4@Student.com')");
            Sql("insert into Students values('Sara Ahmed',2,'sara5@Student.com')");
            Sql("insert into Students values('Mohamed Ahmed',3,'mohamed6@Student.com')");
        }
        
        public override void Down()
        {
        }
    }
}
