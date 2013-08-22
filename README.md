# TheFlow
An open source question and answer site inspired by [Stackoverflow](http://www.stackoverflow.com).

## Features and Goals ##
- Licensed under the Apache 2.0 License
- Runs on ASP.Net
- Uses the MVC (Model-view-controller) pattern
- OpenID for login
	- Using [DotNetOpenAuth](https://github.com/DotNetOpenAuth/DotNetOpenAuth)
- Forms Authentication Cookies for login sessions
- RESTful back-end API
	- Using [MVC Web Api](http://www.asp.net/web-api)
	- API Keys to help prevent Cross-Site Request Forgery
- Simple, clean and mobile friendly front-end website
	- Using [Zurb Foundation 4](http://foundation.zurb.com/)
- SSL

##Open ID##
- Easy to implement with [DotNetOpenAuth](https://github.com/DotNetOpenAuth/DotNetOpenAuth)
- Does not transmit user credentials.

##Front-End (TheFlow.Site) ##
- Smooth and mobile friendly using [Zurb Foundation 4](http://foundation.zurb.com/)
- Authentication using [DotNetOpenAuth](https://github.com/DotNetOpenAuth/DotNetOpenAuth)
	- With Forms Authentication
- SSL enabled
- Date formatting with [moment.js](http://momentjs.com/)

##Back-End (TheFlow.API) ##
- RESTful
- Uses API Keys to prevent CSRF (Cross-Site Request Forgery) attacks (Not implemented)

##Code Style##
- Every **public** or protected property, member, type, interface or method should use [Pascal Case](http://msdn.microsoft.com/en-us/library/x2dbyw72(v=vs.71).aspx)
- Every **private** property, member, type, interface or method should use [Camel Case](http://msdn.microsoft.com/en-us/library/x2dbyw72(v=vs.71).aspx)
- Comments for properties, members, methods, types or interfaces should use [XML Comments](http://msdn.microsoft.com/en-us/magazine/dd722812.aspx)
	-   	```
		   	///<summary>This is an integer used for etc.</summary> [newline]
	 		public int MyInt;```
	-		```
			///<summary>This is an integer used for etc</summary> [newline]
		    private int myInt;
		    ```
- Acronyms (HTTP, GUID, etc.) in member, method, property, class or interface names should start with a capital letter, but all of the other letters of the acronym should be lower case.
	- **Not** `UserID` but `UserId`
	- **Not** `HTTPContext` but `HttpContext`
	- **Not** `SomeJSONData` but `SomeJsonData`

##Data Storage Scheme##

The Database system is based on Entity Framework Code First, therefore no database is actually included in the project. When you first start the site it will probably give you an error about the database. This is because none of the migrations (incremental changes) have been applied to the database. To solve this go to the `Package Manager Console`, set the default project to `TheFlow.API` and type `Update-Database` then press `Enter`. This will apply changes to the database based on the code files in `TheFlow.API\Migrations`, therefore updating the database to a version with no conflicts.

To make changes to the tables, edit the entity classes in `TheFlow.API\Migrations` and then type `Add-Migration [TheNameForYourChanges]` in the `Package Manager Console`, a new migration file will appear in `TheFlow.API\Migrations` detailing how to update the database. Then run `Update-Database` again to update the database.

**ALL** queries to the database should go through the 'DbContext' class, this provides the easiest access to the database. And **All** queries to the database should use `LINQ`, it provides the easiest and most secure option for queries by preventing `Code Injection`.

###Entities###
These are the entities used by the Entity Framework ORM to provide statically typed access to the DB

- `Post`, An entity that is used to hold basic information about a general post (`Author`, `Body`, `Up/Down votes`, etc). 
	- `Answer`, An entity that is used for answers to a question (Holds a reference to the `Question`).
	- `Question`, An entity that is used for questions (`Title`, `Number of Views` and the `Accepted Answer`).
- `Comment`, An entity that is used for comments on a `Post` (`Author`, `Body`, `Id`, etc).
- `User`, An entity that is used to hold information about a user (`OpenId`, `Email`, `Display Name`, `Reputation`, etc).
- `Star`, An entity that is used to mark a question that a user likes (`User`, `Question`, `Id`).
- `Edit`, An entity that is used to define an edit to a `Post` (`New Body`, `Original Post`, etc).
