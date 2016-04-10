# FlexDotnetCMS
A powerful, flexible and easy to use ASP .NET CMS

# Below is what you will need inorder to get started
- Visual Studio Community 2015 
- .NET 4.5.2 + ( will keep updating this as new versions come out )
- Microsoft SQL Server 2012 + ( might work with older versions )
- Microsoft SQL Server Management studio 2012 + ( Once again might work with older versions )

#Getting Started
- Download the zip file and unzip it to a folder
- Double click the .sln file, this should open the solution consisting of 3 projects
- Right click on the "WebApplication" Project and set it as the "Start Up project"
- Open up the "Web.config" file under the "WebApplication" project and search for the AppSetting "EnableInstaller" and set its value to "True"
- Create a new empty database in SQL Server
- Now you can run the project
- Follow the steps in the installer, this will update the "Web.Config" with your DB connection information and execute the .sql scripts needed to setup the database, in the final step you will be asked to "Disable The installer" click on this link and you will be done.
- You can now login to the CMS by going to [domain]/admin/
- The default username is: admin
- The default password is: password

