using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ImageData { get; set; }

        /////////////////////////////////////////// 


        
        public virtual SubCategory SubCategory   { get; set; }


        
        public virtual Product Product   { get; set; }

    }
}
