using System;
using System.Collections.Generic;

class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }
    public bool IsAvailable { get; set; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
        IsAvailable = true; // Setam cartile adaugate ca fiind disponibile initial
    }
}

class User
{
    public string Name { get; }
    public string Email { get; }
    public string Password { get; }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}

class Library
{
    private List<Book> books; // Lista de carti
    private List<User> users; // Lista de utilizatori

    public Library()
    {
        books = new List<Book>();
        users = new List<User>();
    }

    public void RegisterUser(User user)
    {
        users.Add(user);
    }

    public bool AuthenticateUser(string email, string password)
    {
        User user = users.Find(u => u.Email == email && u.Password == password);
        return user != null;
    }

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public List<Book> SearchBook(string keyword)
    {
        return books.FindAll(book => book.Title.Contains(keyword) || book.Author.Contains(keyword));
    }

    public bool BorrowBook(string username, string title)
    {
        User user = users.Find(u => u.Name == username);
        Book book = books.Find(b => b.Title == title && b.IsAvailable);

        if (user != null && book != null)
        {
            book.IsAvailable = false;
            return true;
        }

        return false;
    }

    public bool ReturnBook(string username, string title)
    {
        User user = users.Find(u => u.Name == username);
        Book book = books.Find(b => b.Title == title && !b.IsAvailable);

        if (user != null && book != null)
        {
            book.IsAvailable = true;
            return true;
        }

        return false;
    }

    public List<Book> GetAvailableBooks()
    {
        return books.FindAll(book => book.IsAvailable);
    }

    public List<Book> GetBorrowedBooks()
    {
        return books.FindAll(book => !book.IsAvailable);
    }

    public List<User> GetRegisteredUsers()
    {
        return users;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();

        User user1 = new User("John", "john@example.com", "password1");
        User user2 = new User("Jane", "jane@example.com", "password2");

        library.RegisterUser(user1);
        library.RegisterUser(user2);

        Book book1 = new Book("Book 1", "Author 1", 2000);
        Book book2 = new Book("Book 2", "Author 2", 2005);
        Book book3 = new Book("Book 3", "Author 3", 2010);

        library.AddBook(book1);
        library.AddBook(book2);
        library.AddBook(book3);

        Console.WriteLine("Welcome to the Library!");

        bool isAuthenticated = false;
        User authenticatedUser = null;

        while (!isAuthenticated)
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();
            isAuthenticated = library.AuthenticateUser(email, password);

            if (isAuthenticated)
            {
                authenticatedUser = library.GetRegisteredUsers().Find(u => u.Email == email);
                Console.WriteLine("Authentication successful. Welcome, " + authenticatedUser.Name + "!");
            }
            else
            {
                Console.WriteLine("Invalid credentials. Please try again.");
            }

            Console.WriteLine();
        }

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Search for a book");
            Console.WriteLine("2. Borrow a book");
            Console.WriteLine("3. Return a book");
            Console.WriteLine("4. View available books");
            Console.WriteLine("5. View borrowed books");
            Console.WriteLine("0. Exit");

            Console.Write("Your option: ");
            string option = Console.ReadLine();

            Console.WriteLine();

            switch (option)
            {
                case "1":
                    Console.Write("Enter a keyword to search: ");
                    string keyword = Console.ReadLine();
                    List<Book> searchResults = library.SearchBook(keyword);

                    if (searchResults.Count > 0)
                    {
                        Console.WriteLine("Search results:");
                        foreach (Book book in searchResults)
                        {
                            Console.WriteLine("Title: " + book.Title);
                            Console.WriteLine("Author: " + book.Author);
                            Console.WriteLine("Year: " + book.Year);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found for the given keyword.");
                    }

                    break;

                case "2":
                    Console.Write("Enter your username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter the title of the book you want to borrow: ");
                    string borrowTitle = Console.ReadLine();

                    bool borrowSuccess = library.BorrowBook(username, borrowTitle);

                    if (borrowSuccess)
                    {
                        Console.WriteLine("Book borrowed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Book not available or user not found. Please check the details and try again.");
                    }

                    break;

                case "3":
                    Console.Write("Enter your username: ");
                    string returnUsername = Console.ReadLine();

                    Console.Write("Enter the title of the book you want to return: ");
                    string returnTitle = Console.ReadLine();

                    bool returnSuccess = library.ReturnBook(returnUsername, returnTitle);

                    if (returnSuccess)
                    {
                        Console.WriteLine("Book returned successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Book not borrowed by the user or book not found. Please check the details and try again.");
                    }

                    break;

                case "4":
                    List<Book> availableBooks = library.GetAvailableBooks();

                    if (availableBooks.Count > 0)
                    {
                        Console.WriteLine("Available books:");
                        foreach (Book book in availableBooks)
                        {
                            Console.WriteLine("Title: " + book.Title);
                            Console.WriteLine("Author: " + book.Author);
                            Console.WriteLine("Year: " + book.Year);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books available at the moment.");
                    }

                    break;

                case "5":
                    List<Book> borrowedBooks = library.GetBorrowedBooks();

                    if (borrowedBooks.Count > 0)
                    {
                        Console.WriteLine("Borrowed books:");
                        foreach (Book book in borrowedBooks)
                        {
                            Console.WriteLine("Title: " + book.Title);
                            Console.WriteLine("Author: " + book.Author);
                            Console.WriteLine("Year: " + book.Year);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books currently borrowed.");
                    }

                    break;

                case "0":
                    exit = true;
                    Console.WriteLine("Thank you for using the Library. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
