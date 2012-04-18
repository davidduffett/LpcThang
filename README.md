# Lpc Thang

ASP.NET MVC Helper methods for implementing LivePerson Chat.

## Introduction

Implementing LivePerson Chat (Lpc) in an ASP.NET MVC website involves the following steps:

1. Initialise a global JavaScript variable `lpUnit` to the business unit that corresponds with the chat routing you have setup in the LPC Admin Console.
2. Include a script reference to `mtagconfig.js` which will be provided to you by LivePerson.
3. Include a script containing custom variables, including such things as the section of the site, customer or order details.
4. Optionally include a script for ecommerce conversions.

The aim of Lpc Thang is to simplify the process of including these scripts in your MVC application by providing easy to use Helper methods and action filter attributes.

## Implementation

1. First of all, install LpcThang from Nuget:

	PM> Install-Package LpcThang

2. In your `_Layout.cshtml` file:

		@using LpcThang;
		<head>
			...
		</head>
		<body>
			...
			<script src="/Scripts/mtagconfig.hs"></script>
			@LivePersonChat.Render()
		</body>
	
3. In your code, you can at any stage of the request add page variables, where relevant:

		[HttpGet]
		public ActionResult Index()
		{
			LivePersonChat.AddPageVariable("Category", "Electronics");
			return View();
		}

	This will output the following:

		<script type="text/javascript">
			lpAddVars("page","Category","Electronics");
		</script>
	
4. For the common page variable `Section`, you can decorate either an entire controller, or an action method with the `LivePersonChatSection` attribute:

		[LivePersonChatSection("Checkout")]
		public class CheckoutController
		{
			...
		}

	On any Checkout action the following will be outputted:

		<script type="text/javascript">
			lpAddVars("page","Section","Checkout");
		</script>
