using System.ComponentModel.DataAnnotations.Schema;

namespace BankDemo.Domain.Entities.Account
{
    public class Currency : BaseObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortSign { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
    }
}
