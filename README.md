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
- Uses API Keys to prevent CSRF attacks (Not implemented)

##Code Style##
- Every public or protected property, member, type, interface or method should use [Pascal Case](http://msdn.microsoft.com/en-us/library/x2dbyw72(v=vs.71).aspx)
- Every private property, member, type, interface or method should use [Camel Case](http://msdn.microsoft.com/en-us/library/x2dbyw72(v=vs.71).aspx)
- Comments for properties, members, methods, types or interfaces should use [XML Comments](http://msdn.microsoft.com/en-us/magazine/dd722812.aspx)
	-   	```
		   	///<summary>This is an integer used for etc.</summary>
	 		public int MyInt;```
	-		```
			///<summary>This is an integer used for etc</summary> 
		    private int myInt;
		    ```
