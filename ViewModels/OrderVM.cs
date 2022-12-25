namespace ApplicationDev.ViewModels
{
    public class OrderVM
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Paid { get; set; }
        public string Note { get; set; }
    }
}