#ORDERSYSTEM

#Table of Contents
- Introduction
	- Purpose
	- Key Features
	- Technologies used
	- Libraries used
- Installation and Configuration
	- Setting Up The Database
	- Configuring The Application
- Usage
	- Dashboard
	- Customers
	- Products
	- Adding Orders
	- Editing Orders
- Known bugs
	- Creating/Editing Orders
	- Overall Performance
- API
- Responsive
- Future Features

##Introduction

###Purpose
The purpose of this project is to develop an order management system for a company 
that can effectively track and manage product inventory. The system should prioritize being easy to use and seamless integration with the company's existing processes.

###Key features
1. Order Management:
	- Ability to receive, process, and track orders. 
2. Inventory Management:
	- Capability to track and update product inventory.
3. Customer Data Management:
	- Ability to store and manage customer information.

###Technologies used
- Front-end technologies
	- JavaScript
	- CSS

- Back-end technologies
	- ASP.NET Core
	- C#
	- MVC
	- AJAX
	- Entity Framework Core
	- SQL Server
	- Dependency Injection
	- REST APIs
	- API Documentation (OpenAPI)

###Libraries used
- Front-end
	- Bootstrap
	- Datatables CSS
	- Google Fonts (Material Icons)
	- Select2 CSS

- Back-end
	- jQuery
	- Bootstrap JavaScipt
	- Datatables JavaScript Plugin
	- Font Awesome
	- Select2 JavaScript Plugin


##Installation and Configuration

###Setting Up The Database
1. Locate the .sql file in the project.
2. Open SQL Server Management Studio (SSMS) and connect to your SQL Server instance.
3. Open the .sql file in a text editor, copy all the SQL statements, and paste them into the query window in SSMS.
4. Change the location where you want to save the database for the .mdf and the .ldf.
5. Execute the entire script by clicking the "Execute" button.
6. Once the script execution completes successfully, you can verify that the database objects were created by expanding the "Tables" node.

###Configuring The Application
1. Open the JN.Ordersystem Solution in Visual Studio 2022.
2. Navigate to JN.OrderSystem.DAL.
3. Open DataContext.cs
4. In the optionsBuilder.UseSqlServer(); replace the current string with your SQL Server connection string.


##Usage

###Dashboard
Upon opening the application, you will see the Dashboard.
- Graphs:
	- It currently displays 2 random graphs as pictures, which can be further developed into fully functional graphs in the future.

- Latest Order Needing Attention (Top right):
	- If there are orders in the database, it will display the most recent order that doesn't have the status of "Shipped".
	- It will show the order's ID, date, customer details, total items, sales total, and status.
	- You can click on the order ID or customer details to view more information about the order or customer, respectively.
	- If all orders have been processed or there are no orders needing attention, it will display a text stating that all orders have been processed.

- Most Profitable Customer (Middle right):
	- If there are orders in the database, it will display the customer who made the highest total purchase amount across all orders.
	- The total sales amount is calculated by adding the purchase amount of all the orders made by that specific customer.
	- It will show the customer's name and ID, which can be clicked to view more information about the customer.
	- Underneath, it will display the total sales amount for that customer.
	- If there are no orders in the database, it will display a text stating that there are currently no orders.

- Products with Low Stock (Bottom right):
	- If there are products in the database that currently have less than 20 units in stock, they will be displayed.
	- Each product will have its id, name and the current units in stock shown.
	- You can click on a product to view more details about that specific product.
	- At the top of the list, there is a "Resupply" button.
	- Clicking on the "Resupply" button will simulate a resupply from the suppliers by adding 50 units to the units in stock for all the low-stock products.
	- After clicking the "Resupply" button, an alert will appear indicating whether the resupply was successful or not.
	
###Customers
Let's start by adding our first customer. First, navigate to the customers page from the sidebar. Here we will see a list but with no entries in it. Click on the "New Customer" button, to add a customer to the database.
This will redirect us to the Create Customer page. Fill in the fields. All fields are required.
- Last name (string with max. 50 characters)
- First name (string with max. 50 characters)
- Address (string with max. 70 characters)
- Postal Code (string with max. 10 characters)
- City (string with max. 50 characters)	
- Email (string with max. 100 characters)
- Phone number (string with max. 10 characters)
	- Note: this is a string, otherwise the first 0 in some phone numbers aren't saved.
- Click on create to save the customer to the database.
- After clicking on create, we will be redirected back to the list of customers. 

If everyting went fine, we should see our first customer in the list. Notice that we have a couple of actions, next to the customer details. These actions are:
- Edit
	- To edit the info of the customer.
- Details
	- To see more details of the customer.
- Delete
	- To instantly delete the customer.

###Products
Next, we can create a product. Navigate to the products page using the sidebar. Here we see another empty list. Click on the "New Product" button, to add a product to the database.
This will redirect us to the Create Product page. Again, fill in the fields. All fields are required.
- Product Name (string with max. 50 characters)
- Description (string with max. a large amount of characters)
- Price (decimal)
	- Note: It is currently not possible to use a "," in the number. So just use whole numbers for now.
- Units In Stock (whole number)
- Click on create to save the product to the database.
- After clicking on create, we will be redirected back to the list of products. 

If everyting went fine, we should see our first product in the list. Again, notice that we have a couple of actions, next to the product details. These actions are:
- Edit
	- To edit the info of the product.
- Details
	- To see more details of the product.
- Delete
	- To instantly delete the product.

###Adding Orders
Once we have atleast one customer and one product in the database, we can finally add an order. Navigate to the orders page using the sidebar. 
Here we see another empty list. Click on the "New Order" button, to start creating an order. Again, fill in the fields. All fields are required.
- Order Date
	- This is already set to the current date, but this can be changed.
- Customer
	- Choose a customer from the list
		- We can search for a customer using the ID or just the name.
- Products
	- Choose a product from the list
		- Again, we can search for a product using the ID or just the name.
- Quantity
	- Choose the quantity of the product that you want to add to the cart.
- When done choosing, we can add the product to the cart by clicking the "Add Product To Cart" button.
- We can add or remove multiple products to/from the cart.
- When done adding, we can click on the "Create" button to finalize the order.
- After clicking on create, we will be redirected back to the list of orders. 

If everyting went fine, we should see our first order in the list. Notice how, we now also have another button other than the standard actions:
- Confirm/Process button
	- When we click on the "Confirm" button, it will do 2 things:
		- First, it will check if there was a quantity chosen for a product that exceeds the number of units in stock for that specific product.
			- If it passes the check, the status will update to "Pending".
			- If the check fails, an alert will be displayed indicating the product for which the quantity exceeds the available units in stock.
		- Second, when the status changes to "Pending", the button will change to "Process".
	- When we now click on the "Process" button, it will do a couple of things:
		- First, it checks again if there was a quantity chosen for a product that exceeds the number of units in stock for that specific product.
			- If it passes the check, the status will update to "Shipped"
			- If the check fails, an alert with the same error will be shown.
		- Second, when the status changes to "Shipped", the button itself will be removed.
		- Third, the "Edit" button will also be removed.
- Edit
	- To edit the order.
	- If the status is "Shipped", this button will be removed.
- Details
	- To see more details of the order.
		- In the details, we can also confirm/process the order.
- Delete
	- To instantly delete the order and with it, all the corresponding order details.

###Editing Orders
Currently, it is only possible to edit:
- The order date
- The customer
- The current amount of items
	- So if there were only 2 separate products chosen, we can only edit for these 2 products.


## Known Bugs

###Creating/Editing Orders
1. When creating or editing orders, if a customer is selected and a product is added to the cart, 
then later change the customer without adding anything new to the cart and click on 'Create', the order will still be placed under the initially selected customer.

###Overall Performance
1. When constantly switching pages from the sidebar, the memory increases and doesn't go back down.

No further bugs have been identified at this time. 


##API
This project also contains an API, with the standard CRUD features.


##Responsive
This application is only usable for PC and for an iPad with dimensions 810x1080.

##Future Features
- Make use of the suppliers.
- Add or remove products in the Edit page for the Orders.
- Better validation e.g.:
	- Check for valid customer email
	- Check for letters in customer phone number
	- ...
- Make use of an admin account
- Make the application entirely responsive.
- Use commas for the price of the product.
- ...