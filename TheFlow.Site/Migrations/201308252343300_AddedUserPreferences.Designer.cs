// <auto-generated />
namespace TheFlow.API.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    public sealed partial class AddedUserPreferences : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddedUserPreferences));
        
        string IMigrationMetadata.Id
        {
            get { return "201308252343300_AddedUserPreferences"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}