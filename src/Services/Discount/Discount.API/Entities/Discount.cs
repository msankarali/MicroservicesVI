using Dapper.Contrib.Extensions;

namespace Discount.API.Entities
{
    [Table("discount")]
    public class Discount
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        private int _rate;
        public int Rate
        {
            get { return _rate; }
            set
            {
                if (value < 0)
                {
                    _rate = 0;
                }
                else if (value > 100)
                {
                    _rate = 100;
                }
                else
                {
                    _rate = value;
                }
            }
        }

        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}