using System.ComponentModel.DataAnnotations;

namespace Examples.Web.Infrastructure.DataAnnotations
{
    public class LongRange : RangeAttribute
    {
        public LongRange(long minimum, long maximum)
            : base(typeof(long), minimum.ToString(), maximum.ToString())
        {
        }
    }
}