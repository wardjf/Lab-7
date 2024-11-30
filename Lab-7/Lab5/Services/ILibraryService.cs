using System.Collections.Generic;
using Lab5.Models;

public interface ILibraryService
{
    // Book methods
    List<Book> GetBooks();
    List<Book> GetAvailableBooks();
    List<Book> GetBorrowedBooks();  // Get the list of borrowed books
    bool BorrowBook(int bookId, int userId);  // Borrow a book
    bool ReturnBook(int bookId);  // Return a book
    void AddBook(Book book);  // Add a new book
    void EditBook(int id, Book updatedBook);  // Edit an existing book
    void DeleteBook(int id);  // Delete a book

    // User methods
    List<User> GetUsers();  // Get the list of users
    void AddUser(User user);  // Add a new user
    void EditUser(int id, User updatedUser);  // Edit an existing user
    void DeleteUser(int id);  // Delete a user
}




