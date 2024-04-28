using System.ComponentModel.DataAnnotations;

namespace Cafe.DTOs
{
    public class GetProductDTO : ImageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }



        public string subCategoryName { get; set; }
        public int subCategoryId { get; set; }
    }
}
