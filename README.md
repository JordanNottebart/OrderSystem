#ORDERSYSTEM


##Introduction

###Purpose
The purpose of this project is to develop an order management system for a company 
that can effectively track and manage product inventory. The system should prioritize user-
friendliness and seamless integration with the company's existing processes.

###Key features
1. Order Management:
	- Ability to receive, process, and track orders. 
2. Inventory Management:
	- Capability to track and update product inventory.
3. Customer Data Management:
	- Ability to store and manage customer information.

###Technologies used
- Front-end technologies
	1. JavaScript
	2. CSS


- Back-end technologies
	1. ASP.NET Core
	2. C#
	3. MVC
	4. AJAX
	5. Entity Framework Core
	6. SQL Server
	7. Dependency Injection
	8. REST APIs
	9. API Documentation (OpenAPI)

###Libraries used
- Front-end
	1. Bootstrap
	2. Datatables CSS
	3. Google Fonts (Material Icons)
	4. Select2 CSS


- Back-end
	1. jQuery
	2. Bootstrap JavaScipt
	3. Datatables JavaScript Plugin
	4. Font Awesome
	5. Select2 JavaScript Plugin


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
4. In the optionsBuilder.UseSqlServer(); replace the current string to your SQL Server connection string. 
