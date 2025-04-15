using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyMusicCollection.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MusicCollectionBDcontext>
{
    public MusicCollectionBDcontext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MusicCollectionBDcontext>();


        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MusicCollectionDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        return new MusicCollectionBDcontext(optionsBuilder.Options);
    }
}
