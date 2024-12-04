using System.ComponentModel.DataAnnotations;

namespace ToDoApi2.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Expiry Date is required")]
        public DateTime ExpiryDate { get; set; }
        public bool IsComplete { get; set; }

        [Required(ErrorMessage = "Completion percentage is required")]
        [Range(0, 100, ErrorMessage = "Completion percentage must be between 0 and 100")]
        public int CompletionPercentage { get; set; }
    }
}