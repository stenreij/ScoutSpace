namespace Core.Domain
{
    public class Team
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string city { get; set; }
        public Division division { get; set; }
        public int? contactNr { get; set; }
    }
}
