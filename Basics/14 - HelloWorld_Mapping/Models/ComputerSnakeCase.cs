
namespace HelloWorld.Models
{
    public partial class ComputerSnakeCase
    {
        public int computer_id { get; set; }
        public string? motherboard { get; set; }
        public int cpu_cores { get; set; }
        public bool has_wifi { get; set; }
        public bool has_lte { get; set; }
        public DateTime? release_date { get; set; }
        public decimal price { get; set; }
        public string? video_card { get; set; }
    }
}