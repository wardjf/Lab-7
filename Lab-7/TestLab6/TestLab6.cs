using Lab5.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

[TestClass]
public class MockLibraryServiceTests
{
    private MockLibraryService mockLibraryService;

    [TestInitialize]
    public void TestInitialize()
    {
        mockLibraryService = new MockLibraryService();
        
        File.WriteAllText("C:\\Users\\Josh\\Documents\\GitHub\\Lab-7\\Lab-7\\TestLab6\\MockData\\MockBooks.csv", string.Empty);
        File.WriteAllText("C:\\Users\\Josh\\Documents\\GitHub\\Lab-7\\Lab-7\\TestLab6\\MockData\\MockUsers.csv", string.Empty);

        var sampleBooks = new List<Book>
        {
            new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565" },
            new Book { Id = 2, Title = "1984", Author = "George Orwell", ISBN = "9780451524935" },
            new Book { Id = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061120084" },
            new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "9780141439518" },
            new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", ISBN = "9780316769488" }
        };

        var sampleUsers = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" },
            new User { Id = 3, Name = "George Johnson", Email = "george.johnson@example.com" },
            new User { Id = 4, Name = "Amy Brown", Email = "amy.brown@example.com" }
        };

        using (var writer = new StreamWriter("C:\\Users\\Josh\\Documents\\GitHub\\Lab-7\\Lab-7\\TestLab6\\MockData\\MockBooks.csv"))
        {
            writer.WriteLine("ID,Title,Author,ISBN"); 
            foreach (var book in sampleBooks)
            {
                writer.WriteLine($"{book.Id},{book.Title},{book.Author},{book.ISBN}");
            }
        }

        using (var writer = new StreamWriter("C:\\Users\\Josh\\Documents\\GitHub\\Lab-7\\Lab-7\\TestLab6\\MockData\\MockUsers.csv"))
        {
            writer.WriteLine("ID,Name,Email"); 
            foreach (var user in sampleUsers)
            {
                writer.WriteLine($"{user.Id},{user.Name},{user.Email}");
            }
        }
    }

    [TestMethod]
    public void GetBooks_ShouldReturnAllBooks()
    {
        var books = mockLibraryService.GetBooks();
        Assert.IsTrue(books.Count > 0);
    }

    [TestMethod]
    public void GetAvailableBooks_ShouldReturnOnlyAvailableBooks()
    {
        var availableBooks = mockLibraryService.GetAvailableBooks();
        foreach (var book in availableBooks)
        {
            Assert.IsNull(book.BorrowedBy);
        }
    }

    [TestMethod]
    public void BorrowBook_ShouldUpdateBorrowedByField()
    {
        var user = mockLibraryService.FindUser(1);  // John Doe
        var book = mockLibraryService.FindBook(5);  // Mock Book 5
        var result = mockLibraryService.BorrowBook(book.Id, user.Id);
        Assert.IsTrue(result);
        Assert.AreEqual(user, book.BorrowedBy);
    }

    [TestMethod]
    public void ReturnBook_ShouldRemoveBorrowedByField()
    {
        var book = mockLibraryService.FindBook(5);  
        mockLibraryService.ReturnBook(book.Id);
        Assert.IsNull(book.BorrowedBy);
    }

    [TestMethod]
    public void DeleteBook_ShouldRemoveBookFromList()
    {
        var book = mockLibraryService.FindBook(5);  
        mockLibraryService.DeleteBook(book.Id);
        var deletedBook = mockLibraryService.FindBook(5);
        Assert.IsNull(deletedBook);
    }

    [TestMethod]
    public void GetUsers_ShouldReturnAllUsers()
    {
        var users = mockLibraryService.GetUsers();
        Assert.IsTrue(users.Count > 0);
    }

    [TestMethod]
    public void AddUser_ShouldAddUserToList()
    {
        var newUser = new User { Id = 51, Name = "Test User", Email = "testuser@example.com" };
        mockLibraryService.AddUser(newUser);
        var addedUser = mockLibraryService.FindUser(51);
        Assert.IsNotNull(addedUser);
        Assert.AreEqual(newUser.Name, addedUser.Name);
    }

    [TestMethod]
    public void DeleteUser_ShouldRemoveUserFromList()
    {
        var user = mockLibraryService.FindUser(1); 
        mockLibraryService.DeleteUser(user.Id);
        var deletedUser = mockLibraryService.FindUser(1);
        Assert.IsNull(deletedUser);
    }

    [TestMethod]
    public void BorrowBook_ShouldReturnFalseIfBookAlreadyBorrowed()
    {
      
        var user = mockLibraryService.FindUser(1);  
        var book = mockLibraryService.FindBook(5);  
        Assert.IsNotNull(user);  
        Assert.IsNotNull(book); 

        var firstBorrowResult = mockLibraryService.BorrowBook(book.Id, user.Id);
        Assert.IsTrue(firstBorrowResult);  

        var secondBorrowResult = mockLibraryService.BorrowBook(book.Id, user.Id);
        Assert.IsFalse(secondBorrowResult);  
    }

}


