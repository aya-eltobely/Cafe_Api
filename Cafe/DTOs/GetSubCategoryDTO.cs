namespace Cafe.DTOs
{
    public class GetSubCategoryDTO : ImageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
