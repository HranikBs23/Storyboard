namespace Notes.API.Models.Entities
{
    public class Note
    {
        public Guid id { get; set; }
        public string Ttile { get; set; }
        public string Description { get; set; }
        public bool IsVisible {get; set; }
    }
}
