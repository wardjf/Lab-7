using Lab5.Models;
using Lab5;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Lab5Tests
{
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
            var book = new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" };

            // Add book to library and the CSV
            libraryService.AddBook(book);

            var books = libraryService.GetBooks();

            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("Book A", books.First().Title);
        }

        [TestMethod]
        public void AddBook_DuplicateID_ShouldThrowException()
        {
            var book1 = new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" };
            libraryService.AddBook(book1);

            var book2 = new Book { Id = 1, Title = "Book B", Author = "Author B", ISBN = "12345" };

            // Should throw exception due to duplicate ISBN
            Assert.ThrowsException<InvalidOperationException>(() => libraryService.AddBook(book2));
        }

        [TestMethod]
        public void RemoveBook_ValidId_ShouldDecreaseBookCount()
        {
            var book = new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" };
            libraryService.AddBook(book);

            // Delete the book and verify count reduces
            libraryService.DeleteBook(1);

            var books = libraryService.GetBooks();

            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void RemoveBook_InvalidId_ShouldNotChangeBookCount()
        {
            var book = new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" };
            libraryService.AddBook(book);

            // Try removing a book with an invalid ID
            Assert.ThrowsException<KeyNotFoundException>(() => libraryService.DeleteBook(999));

            var books = libraryService.GetBooks();
            Assert.AreEqual(1, books.Count);  // Ensure book count remains the same
        }

        [TestMethod]
        public void FindBook_ValidId_ShouldReturnCorrectBook()
        {
            var book = new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" };
            libraryService.AddBook(book);

            // Find book by ID
            var foundBook = libraryService.FindBook(1);

            Assert.AreEqual("Book A", foundBook.Title);
        }

        [TestMethod]
        public void FindBook_InvalidId_ShouldReturnNull()
        {
            var result = libraryService.FindBook(999);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllBooks_EmptyLibrary_ShouldReturnEmptyList()
        {
            var books = libraryService.GetBooks();

            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void GetAllBooks_WithBooks_ShouldReturnAllBooks()
        {
            libraryService.AddBook(new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" });
            libraryService.AddBook(new Book { Id = 2, Title = "Book B", Author = "Author B", ISBN = "67890" });

            var books = libraryService.GetBooks();

            Assert.AreEqual(2, books.Count);
        }

        [TestMethod]
        public void UpdateBook_ValidId_ShouldModifyBookDetails()
        {
            var book = new Book { Id = 1, Title = "Book A", Author = "Author A", ISBN = "12345" };
            libraryService.AddBook(book);

            var updatedBook = new Book { Id = 1, Title = "Updated Title", Author = "Updated Author", ISBN = "12345" };
            libraryService.EditBook(1, updatedBook);

            var foundBook = libraryService.FindBook(1);

            Assert.AreEqual("Updated Title", foundBook.Title);
            Assert.AreEqual("Updated Author", foundBook.Author);
        }

        [TestMethod]
        public void UpdateBook_InvalidId_ShouldThrowException()
        {
            var updatedBook = new Book { Id = 1, Title = "Updated Title", Author = "Updated Author", ISBN = "12345" };

            // Try updating a non-existing book
            Assert.ThrowsException<KeyNotFoundException>(() => libraryService.EditBook(999, updatedBook));
        }
    }
}

