using Lab5.Models;

using System.Globalization;
using System.IO;
using System.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class LibraryService : ILibraryService
{
    private List<Book> books;
    private List<User> users;
    private List<Book> borrowedBooks;
    private readonly string booksFilePath = "C:\\Users\\Josh\\Documents\\Lab5\\Data\\Books.csv";
    private readonly string usersFilePath = "C:\\Users\\Josh\\Documents\\Lab5\\Data\\Users.csv";

    public LibraryService()
    {
        books = ReadBooksFromCsv();
        users = ReadUsersFromCsv();
        borrowedBooks = new List<Book>(); // Initialize borrowedBooks list
    }

    // Book methods
    public List<Book> GetBooks() => books;

    public List<Book> GetAvailableBooks() => books.Where(b => b.BorrowedBy == null).ToList();

    public List<Book> GetBorrowedBooks() => books.Where(b => b.BorrowedBy != null).ToList();

    public bool BorrowBook(int bookId, int userId)
    {
        var book = books.FirstOrDefault(b => b.Id == bookId);
        var user = users.FirstOrDefault(u => u.Id == userId);

        if (book != null && user != null && book.BorrowedBy == null)
        {
            book.BorrowedBy = user;
            WriteBooksToCsv();  // Save the updated list of books
            return true;
        }

        return false;
    }

    public bool ReturnBook(int bookId)
    {
        var book = books.FirstOrDefault(b => b.Id == bookId);
        if (book != null && book.BorrowedBy != null)
        {
            book.BorrowedBy = null;
            WriteBooksToCsv();  // Save the updated list of books
            return true;
        }

        return false;
    }

    public void AddBook(Book book)
    {
        books.Add(book);
        WriteBooksToCsv(); // Save to CSV after adding
    }

    public void EditBook(int id, Book updatedBook)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.ISBN = updatedBook.ISBN;
            WriteBooksToCsv();  // Save to CSV after editing
        }
    }

    public void DeleteBook(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            books.Remove(book);
            borrowedBooks.Remove(book); // Also remove from borrowed list if present
            WriteBooksToCsv();  // Save to CSV after deleting
        }
    }

    // User methods
    public List<User> GetUsers() => users;

    public void AddUser(User user)
    {
        users.Add(user);
        WriteUsersToCsv();  // Save to CSV after adding
    }

    public void EditUser(int id, User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            WriteUsersToCsv();  // Save to CSV after editing
        }
    }

    public void DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            users.Remove(user);
            WriteUsersToCsv();  // Save to CSV after deleting
        }
    }

    // CSV Read and Write Methods
    private List<Book> ReadBooksFromCsv()
    {
        var booksList = new List<Book>();

        if (File.Exists(booksFilePath))
        {
            using (var reader = new StreamReader(booksFilePath))
            {
                // Skip header line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = ParseCsvLine(line);  // Use a custom parser to handle commas inside quotes

                    if (values.Length == 4)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(values[0]),
                            Title = values[1],
                            Author = values[2],
                            ISBN = values[3]
                        };
                        booksList.Add(book);
                    }
                }
            }
        }

        return booksList;
    }

    // CSV line parser to correctly handle quoted fields
    private string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        var currentValue = new StringBuilder();
        bool insideQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == ',' && !insideQuotes)
            {
                // A comma outside quotes indicates a new field
                values.Add(currentValue.ToString());
                currentValue.Clear();
            }
            else if (c == '"')
            {
                // Toggle insideQuotes when encountering a double quote
                insideQuotes = !insideQuotes;
            }
            else
            {
                // Add character to the current field value
                currentValue.Append(c);
            }
        }

        // Add the last field value
        if (currentValue.Length > 0)
        {
            values.Add(currentValue.ToString());
        }

        return values.ToArray();
    }

    private List<User> ReadUsersFromCsv()
    {
        var usersList = new List<User>();

        if (File.Exists(usersFilePath))
        {
            using (var reader = new StreamReader(usersFilePath))
            {
                // Skip header line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = ParseCsvLine(line);  // Handle commas inside quoted fields

                    if (values.Length == 3)
                    {
                        var user = new User
                        {
                            Id = int.Parse(values[0]),
                            Name = values[1],
                            Email = values[2]
                        };
                        usersList.Add(user);
                    }
                }
            }
        }

        return usersList;
    }

    private void WriteBooksToCsv()
    {
        using (var writer = new StreamWriter(booksFilePath))
        {
            // Write header line
            writer.WriteLine("Id,Title,Author,ISBN");

            foreach (var book in books)
            {
                writer.WriteLine($"{book.Id},{book.Title},{book.Author},{book.ISBN}");
            }
        }
    }

    private void WriteUsersToCsv()
    {
        using (var writer = new StreamWriter(usersFilePath))
        {
            // Write header line
            writer.WriteLine("Id,Name,Email");

            foreach (var user in users)
            {
                writer.WriteLine($"{user.Id},{user.Name},{user.Email}");
            }
        }
    }
}



