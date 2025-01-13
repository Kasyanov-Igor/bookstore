using bookstore.Services.Interfaces;
using bookstore.Models.Entities;
using bookstore.Services;

ADatabaseConnection connection = new SqliteConnection();

IBookService iBook = new BookService(connection);
IFactoryMapper factoryMapper = new FactoryMapper();
IUserService iUser = new UserService(connection, factoryMapper);

User? currentUser = null;

Console.WriteLine("Welcome to the Bookstore!");
Console.WriteLine("Please log in to continue."); ///< Login/Authentication

bool authenticated = false;

while (!authenticated)
{
	Console.Write("Enter your login: ");
	string? login = Console.ReadLine();

	Console.Write("Enter your password: ");
	string? password = Console.ReadLine();

	User user = new User { Login = login, Password = password };
	User? foundUser = iUser.FindUser(user);

	if (foundUser != null)
	{
		if (foundUser.Password == password)
		{
			Console.WriteLine("Login successful.");
			currentUser = foundUser;
			authenticated = true;
			Console.WriteLine("Press any Key to enter the 'Book Store'.");
			Console.ReadKey();
		}
		else
		{
			Console.WriteLine("Incorrect password. Try again.");
		}
	}
	else
	{
		Console.WriteLine("User not found. Creating a new account...");
		if (iUser.AddUser(user))
		{
			Console.WriteLine("User created successfully.");
			Console.WriteLine("Press any Key to enter the 'Book Store'.");
			Console.ReadKey();
			currentUser = user;
			authenticated = true;
		}
		else
		{
			Console.WriteLine("Failed to create user. Try again.");
		}
	}
}

bool exit = false;

while (!exit)
{
	Console.Clear();
	Console.WriteLine("Welcome to the Book Store!");
	Console.WriteLine("1. Add a new book");
	Console.WriteLine("2. Delete a book");
	Console.WriteLine("3. Buy a book");
	Console.WriteLine("4. Search books by Author");
	Console.WriteLine("5. Search books by Genre");
	Console.WriteLine("6. Look at the Bestsellers. (beta version 1.0)");
	Console.WriteLine("7. Exit");
	Console.Write("Enter your choice: ");

	switch (Console.ReadLine())
	{
		case "1":

			iBook.AddBook(iBook.CreateBook());

			break;
		case "2":

			Console.WriteLine("What is the name of the book?");
			string? nameBook = Console.ReadLine();

			if (nameBook != null)
			{
				iBook.DeleteBook(iBook.SearchByName(nameBook));
			}

			break;
		case "3":

			Console.WriteLine("What is the name of the book?");
			nameBook = Console.ReadLine();

			iUser.BuyBook(currentUser, iBook.SearchByName(nameBook));

			break;
		case "4":

			List<Book> booksByAuthor = iBook.SearchByAuthor();

			if (booksByAuthor.Any())
			{
				Console.WriteLine("Books by the specified author:");
				foreach (Book book in booksByAuthor)
				{
					Console.WriteLine($"{book.Name}, Genre: {book.BookGenre.Name}");
				}
			}
			else
			{
				Console.WriteLine("No books found.");
			}
			break;

		case "5":

			Console.Clear();

			Console.WriteLine("What is the genre?");

			string genreName = Console.ReadLine().ToLower();

			List<Book> booksByGenre = iBook.SearchByGenre(genreName);

			if (booksByGenre.Any())
			{
				Console.WriteLine("Books in the specified genre:");
				foreach (Book book in booksByGenre)
				{
					Console.WriteLine($"{book.Name}, Author: {book.BookAuthor.FirstName} {book.BookAuthor.LastName}");
				}
			}
			else
			{
				Console.WriteLine("No books found.");
			}
			break;

		case "6":

			Console.Write("Enter the time range (day, month, year): ");
			string time = Console.ReadLine().ToLower();

			List<Book> bestsellers = iBook.GetBestsellers(time);
			if (bestsellers.Any())
			{
				Console.WriteLine("Top 10 Bestsellers:");
				foreach (Book book in bestsellers)
				{
					Console.WriteLine($"{book.Name}, Sales: {book.SalesCount}, Last Sold: {book.LastSoldDate}");
				}
			}
			else
			{
				Console.WriteLine("No bestsellers found.");
			}
			break;

		case "7":

			exit = true;
			break;
		default:
			Console.WriteLine("Invalid choice. Please try again.");
			break;
	}
	if (!exit)
	{
		Console.WriteLine("\nPress any key to return to the main menu...");
		Console.ReadKey();
	}
}

