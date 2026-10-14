namespace NZWalks.API.Models.Domain
{
    public class Difficulty
    {
        //Guid -- Globally Unique Identifier
        //to create an identifier that is very unlikely to be duplicated
        public Guid Id { get; set;}
        public string Name { get; set; }
    }
}
