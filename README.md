# FlexDotnetCMS
A powerful, flexible, easy to use and Fully Featured ASP .NET CMS

To view the complete developer guide click here: https://github.com/MacdonaldRobinson/FlexDotnetCMS/raw/master/WebApplication/Admin/media/docs/FlexDotnetCMSGuide.docx

## Admin Tools
- Database Compare Tool: Allows you to compare 2 environments to see what has changed i.e New Pages, New Fields, Different Content  etc. NOTE: This will only work if the 2 environments have the same database schema.
- Database Backup and Restore Tool: Allows you to backup the current database to a folder and displays a list of all backed up files, you can then restore from a backup file. NOTE: This only works if both the web application and the database are on the same server.
- You can view the email logs from the CMS ( provided the emails were sent using the system its self )
- You can view the error logs from the CMS 

## SEO
- All pages automaticly get a search engine friendly URL, you have the ability to change the uri segment for the page from the CMS
- Ability to change the SEO for every page: Page title, Meta Description, Meta Keywords, Link title ( Url Segment for the page )
- Ability to create 301 or 302 redirects. You can create your own url redirect rules, you can have it so that /oldpage is redirected to /about/contact/

## Caching
- You have 3 levels of caching: Memory Caching, File System Caching and Redis Caching
- You have control over caching of pages. If you have set all pages to cache, the first time a visitor visits the page, it will generate a file system cache for that page and the next time the request will load from cache, drasticly speeding up performance. 
- If the page is cached then it bypasses the database connection all together making the load time extreamly fast. 
- If you have the page cached and there is an error connecting to the database, the system will attempt to load that page from cache if it exists.
- You have the ability to clear the cache of an individual page.
- Ability to clear all cache

## Roles and Permissions
- Ability to Manage Users and Roles
- Ability to limit access to a page or a page type ( role based ). Only people that belong to the role can access these pages in the CMS
- Ability to automatically propigate role limitations to all child pages.
- You can enforce the role limitations in the frontend as well, so you need to be logged in with appropriate credentials inorder to view these pages in the frontend.

## Editing
- FrontEnd Editing: Every field can be edited from the frontend of the website
- Visual Layout Editor ( Experimental ): Once logged into the cms and browsing the frontend of the website, you can toggle the visual layout editor, which allows you to create, delete, reorder the rows, columns and fields
- Every time you save a page in the CMS it will automatically create a history version which you can then reload and publish LIVE again.
- The HTML structure of the page and the Content of the page are clearly seperated and both the Content and the HTML structure for the page can be edited directly in the CMS, there is no need to use a 3rd party editor or to use FTP, this speeds up the time it takes to make changes on the website.  
-- Since the HTML structure and the content are clearly seperated you do not need to worry about having the client change any of the HTML structure.
- Ability to create custom fields with their own unique layouts. You can create new Fields in the CMS directly, no need to hardcode anything in the code behind. The CMS comes with several field types already prebuilt, so you do not even need to know programming inorder to add new fields, however if you do know programming ( Razor Engine Code ) you can program it to do anything you like, you have complete freedome over the HTML that is generated.
- Ability to modify the layout of all pages with a specific page type or unlink from a page type and create a unique layout for every page. Every page in the CMS has a type, so you can either have it so that it would use the style of the page type, or have it so that it has its own unique layout.
- When you edit content and save it you can see how it looks right away in the browser panel instead of having to open a new window and browse to the page
- You can resize the browser panel and see how the site looks in different sizes i.e test responsive layout directly through the cms

## Multi Language
- You can create as many languages in the CMS as you like.
- Ability to add new languages to each page with ease: You can simply browse to a page and then switch the language and save and publish and your done, you have now created a new language version of the page.

## Publishing 
- Ability to automatically publish a page live at a specified date time
- Ability to automatically take down a page at a specified date time
- Ability to save as draft, you can come back and make changes to the draft page later, you can even send links to the draft version of the page inorder to get approval, and then push the page LIVE once you are done. You can also set the draft version to automaticly get pushed live at a specified date time.

## Additional Functionality  / Features
- Built in Blog functionality, including moderated blog posts
- Ability to Add / Edit / Delete Glossary terms.
- Ability to Create, Edit, Delete/Recover, Permanently Delete, Hide/Show, Duplicate and Publish/unpublish pages 
- Ability to search for a page based on any field value in the backend for quick access
- Several prebuilt field types are included which should sute most use cases.
- Code completion examples are provided to get started with programming in the Layouts tab using C# and the Razor Code syntax.
