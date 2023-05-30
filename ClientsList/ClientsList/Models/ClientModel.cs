namespace ClientsList.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsModelValid() =>
            !IsFieldEmpty(Name) && !IsFieldEmpty(Email) &&
            !IsFieldEmpty(Phone) && !IsFieldEmpty(Address);
        private bool IsFieldEmpty(in string field) => string.IsNullOrWhiteSpace(field);
    }
}