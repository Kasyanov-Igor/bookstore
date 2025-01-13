using bookstore.Services;
using bookstore.Models.Entities;
using bookstore.Services.Interfaces;

namespace bookstore.Tests
{
	public class bookstoreTest
	{
		[Fact]
		public void CheckBookAddition()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IBookService bookService = new BookService(connection);

			Book testAddook = new Book()
			{
				Name = "c# basics",
				BookAuthor = new BookAuthor { FirstName = "Roman", LastName = "Popov" },
				BookPublisher = new BookPublisher { Name = "publisher" },
				BookGenre = new BookGenre { Name = "pr" }
			};
			bookService.AddBook(testAddook);

			Assert.True(connection.Books.Contains(testAddook));
		}

		[Fact]
		public void SearchByNameTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IBookService bookService = new BookService(connection);

			Book testAddook = new Book()
			{
				Name = "c# basics",
				BookAuthor = new BookAuthor { FirstName = "Roman", LastName = "Popov" },
				BookPublisher = new BookPublisher { Name = "publisher" },
				BookGenre = new BookGenre { Name = "pr" }
			};
			bookService.AddBook(testAddook);

			string nameTest = "c# basics";

			Assert.True(bookService.SearchByName(nameTest).Name == testAddook.Name);
		}

		[Fact]
		public void DeleteBookTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IBookService bookService = new BookService(connection);

			Book testAddook = new Book()
			{
				Name = "c# basics",
				BookAuthor = new BookAuthor { FirstName = "Roman", LastName = "Popov" },
				BookPublisher = new BookPublisher { Name = "publisher" },
				BookGenre = new BookGenre { Name = "pr" }
			};
			bookService.AddBook(testAddook);

			bookService.DeleteBook(testAddook);

			Assert.False(connection.Books.Contains(testAddook));
		}

		[Fact]
		public void AddBookAuthorTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IBookAuthorService authorService = new BookAuthorService(connection);

			BookAuthor bookAuthor = new BookAuthor() { FirstName = "Igor", LastName = "Kasyanov" };

			authorService.AddBookAuthor(bookAuthor);

			Assert.True(connection.BookAuthors.Contains(bookAuthor));
		}

		[Fact]
		public void AddBookGenreTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IBookGenreService genreService = new BookGenreService(connection);

			BookGenre bookGenre = new BookGenre();

			genreService.AddBookGenre(bookGenre);

			Assert.True(connection.BookGenres.Contains(bookGenre));
		}

		[Fact]
		public void AddBookPublisherTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IBookPublisherService publisherService = new BookPublisherService(connection);

			BookPublisher bookPublisher = new BookPublisher() { Name = "A.D" };

			publisherService.AddBookPublisher(bookPublisher);

			Assert.True(connection.BookPublisher.Contains(bookPublisher));
		}

		[Fact]
		public void AddUserTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IFactoryMapper factoryMapper = new FactoryMapper();

			IUserService userService = new UserService(connection, factoryMapper);

			User user = new User { Login = "Igor", Password = "12345" };

			userService.AddUser(user);

            Assert.True(connection.Users.Contains(user));
		}

		[Fact]
		public void FindUserTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IFactoryMapper factoryMapper = new FactoryMapper();

			IUserService userService = new UserService(connection, factoryMapper);

			User user = new User { Login = "Igor", Password = "123" };

			userService.AddUser(user);

			Assert.True(userService.FindUser(user) == connection.Users.Where(usr => usr.Login == "Igor" && usr.Password == "123").FirstOrDefault());
		}

		[Fact]
		public void DeleteUserTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IFactoryMapper factoryMapper = new FactoryMapper();

			IUserService userService = new UserService(connection, factoryMapper);

			User user = new User { Login = "Igor", Password = "123" };

			userService.AddUser(user);

			userService.DeleteUser(user);

			Assert.False(connection.Users.Contains(user));
		}


		[Fact]
		public void BuyBookTest()
		{
			ADatabaseConnection connection = new SqliteConnection();

			IFactoryMapper factoryMapper = new FactoryMapper();

			IUserService userService = new UserService(connection, factoryMapper);

			User user = new User { Login = "Igor", Password = "123" };

			Book testAddook = new Book()
			{
				Name = "c# basics",
				BookAuthor = new BookAuthor { FirstName = "Roman", LastName = "Popov" },
				BookPublisher = new BookPublisher { Name = "publisher" },
				BookGenre = new BookGenre { Name = "pr" },
				CostPrice = 900

			};

			userService.BuyBook(user, testAddook);

			Assert.False(connection.PurchaseHistory.Contains(new PurchaseHistory() { Book = testAddook, User = user }));
		}
	}
}