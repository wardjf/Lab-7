using Lab5.Models;

[TestClass]
public class LibraryServiceTests
{
    private LibraryService libraryService;

    [TestInitialize]
    public void SetUp()
    {
        libraryService = new LibraryService();
    }

    [TestMethod]
    public void AddBook_ValidBook_ShouldIncreaseBookCount()
    {
        var book = new Book { Title = "Book A", Author = "Author A", ISBN = "12345" };
        libraryService.AddBook(book);

        Assert.AreEqual(1, libraryService.GetAllBooks().Count);
    }

    [TestMethod]
    public void AddBook_DuplicateISBN_ShouldThrowException()
    {
        var book = new Book { Title = "Book A", Author = "Author A", ISBN = "12345" };
        libraryService.AddBook(book);

        var duplicateBook = new Book { Title = "Book B", Author = "Author B", ISBN = "12345" };

        Assert.ThrowsException<InvalidOperationException>(() => libraryService.AddBook(duplicateBook));
    }

    [TestMethod]
    public void RemoveBook_ValidISBN_ShouldDecreaseBookCount()
    {
        var book = new Book { Title = "Book A", Author = "Author A", ISBN = "12345" };
        libraryService.AddBook(book);

        libraryService.RemoveBook("12345");

        Assert.AreEqual(0, libraryService.GetAllBooks().Count);
    }

    [TestMethod]
    public void RemoveBook_InvalidISBN_ShouldThrowException()
    {
        Assert.ThrowsException<KeyNotFoundException>(() => libraryService.RemoveBook("99999"));
    }

    [TestMethod]
    public void FindBook_ValidISBN_ShouldReturnCorrectBook()
    {
        var book = new Book { Title = "Book A", Author = "Author A", ISBN = "12345" };
        libraryService.AddBook(book);

        var foundBook = libraryService.FindBook("12345");

        Assert.AreEqual("Book A", foundBook.Title);
    }

    [TestMethod]
    public void FindBook_InvalidISBN_ShouldReturnNull()
    {
        var result = libraryService.FindBook("99999");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetAllBooks_EmptyLibrary_ShouldReturnEmptyList()
    {
        var books = libraryService.GetAllBooks();

        Assert.AreEqual(0, books.Count);
    }

    [TestMethod]
    public void GetAllBooks_WithBooks_ShouldReturnAllBooks()
    {
        libraryService.AddBook(new Book { Title = "Book A", Author = "Author A", ISBN = "12345" });
        libraryService.AddBook(new Book { Title = "Book B", Author = "Author B", ISBN = "67890" });

        var books = libraryService.GetAllBooks();

        Assert.AreEqual(2, books.Count);
    }

    [TestMethod]
    public void UpdateBook_ValidISBN_ShouldModifyBookDetails()
    {
        var book = new Book { Title = "Book A", Author = "Author A", ISBN = "12345" };
        libraryService.AddBook(book);

        var updatedBook = new Book { Title = "Book A Updated", Author = "Author A", ISBN = "12345" };
        libraryService.UpdateBook(updatedBook);

        var foundBook = libraryService.FindBook("12345");

        Assert.AreEqual("Book A Updated", foundBook.Title);
    }

    [TestMethod]
    public void UpdateBook_InvalidISBN_ShouldThrowException()
    {
        var updatedBook = new Book { Title = "Book A Updated", Author = "Author A", ISBN = "12345" };

        Assert.ThrowsException<KeyNotFoundException>(() => libraryService.UpdateBook(updatedBook));
    }
}
