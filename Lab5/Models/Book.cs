namespace Lab5.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public User BorrowedBy { get; set; }  // Track who borrowed the book
        public int? BorrowedById { get; set; }  // User ID for easier tracking
        public bool IsBorrowing { get; set; }  // Control when the borrow form is shown
        public int SelectedUserId { get; set; } // Selected user to borrow the book
    }
}
