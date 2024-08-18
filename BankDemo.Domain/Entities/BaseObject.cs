namespace BankDemo.Domain.Entities
{
    public abstract class BaseObject
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
    }
}
