# FlexDotnetCMS
A powerful, flexible, easy to use and Fully Featured ASP .NET CMS

To view the complete developer guide click here: https://github.com/MacdonaldRobinson/FlexDotnetCMS/raw/master/WebApplication/Admin/media/docs/FlexDotnetCMSGuide.docx

## Demo Site ##
- **URL**: http://flexdotnetcms.somee.com/
- **Base Headless Api Url**: http://flexdotnetcms.somee.com/?format=json

- **CMS Login URL**: http://flexdotnetcms.somee.com/admin/
	- **Username:** test_dev
	- **Password:** test_password

## Automatic Single Page App / Universal App with SSR ( Server Side Rendering )
- Automatic Single Page App functionality with SSR via the Single Page App library that I built ( https://github.com/MacdonaldRobinson/jquery-spa )
- Since the pages are rendred Server Side, you do not lose any of the functionality offered by the CMS like frontend editing / dynamic nav generation based on the site tree in the CMS.
- Since the pages are rendered Server Side on the first request, you dont run into any SEO issues as you would in a classic SPA ( This is similar to how NuxtJS works )
- You can even use other frontend libraries like VueJS as a javascript include to enhance form submissions etc and it works well with the built in SPA library.

## Automatic Image Optimization
- The Framework automaticly optimizes ALL image requestes using the Image Processor Library ( https://imageprocessor.org/imageprocessor-web/imageprocessingmodule/quality/ )

## Automatic Google Site Map Generator
- I built a Google Site Map Generator that is dynamicly updated based on the changes in the CMS, the site map generator is automaticly called from the robots.txt file, so you dont even need to submit your site map.

## Automatic RSS and JSON Generator
- I built a RSS generator and a JSON generator which are dynamicly generated based on changes in the CMS
  - You can call the RSS generator by simply adding "?format=rss" at the end of any URL
  - You can call the JSON generator by simply adding "?format=json" at the end of any URL ( This doubles as the Headless CMS API )
  
## Decoupled CMS
- The CMS is only used to house content
- You can easily pull CMS content into your cshtml files, please scroll down to the section labeled ( Example showing how you can pull content from the CMS from a cshtml file )
  
## Headless CMS
- Comes with a API that allows you to load content via AJAX which means you can use the CMS as a Headless CMS ( Simply add "?format=json" at the end of the URL of any page an that will return a JSON representation of all the fields, incluiding the custom fields that you have created in the CMS ) 

## Static File Generator
- The CMS is also a Static File Generator which builds static pages and stores them in the Cache folder, and then on every other request it loads the page via cache making the performance extremely fast ( Please view the Cache section below to see how caching works )
  - Please note that these static files are dynamicly generated on the first request of the page, and are then re-generated based on changes made in the CMS, so there is no need of a build  and deployment process

## Works side by side with other SPA Javascript Frameworks like React / Vue / Angular
- If you prefer to not use the built in SPA library, you could use any other javascript framework and place it in the "Frontend" folder, the only thing you need to do is to make sure that you are loading the index page for all virtual page requests. 
  - You can do this from the CMS by changing the code in the Layouts section for every template to load the index file {IncludeFile:'/Frontend/PATH_TO_INDEX_FILE'}
- You can then use the CMS has a Headless CMS and load content via the API
- This gives you the benefit of having only one host hosting both your frontend and backend systems.  
  
## Editing
- FrontEnd Editing: Every field can be edited from the frontend of the website
- Visual Layout Editor ( Experimental ): Once logged into the cms and browsing the frontend of the website, you can toggle the visual layout editor, which allows you to create, delete, reorder the rows, columns and fields
- Every time you save a page in the CMS it will automatically create a history version which you can then reload and publish LIVE again.
- The HTML structure of the page and the Content of the page are clearly seperated and both the Content and the HTML structure for the page can be edited directly in the CMS, there is no need to use a 3rd party editor or to use FTP, this speeds up the time it takes to make changes on the website.  
  - Since the HTML structure and the content are clearly seperated you do not need to worry about having the client change any of the HTML structure.
- Ability to create custom fields with their own unique layouts. You can create new Fields in the CMS directly, no need to hardcode anything in the code behind. The CMS comes with several field types already prebuilt, so you do not even need to know programming inorder to add new fields, however if you do know programming ( Razor Engine Code ) you can program it to do anything you like, you have complete freedom over the HTML that is generated.
- Ability to modify the layout of all pages with a specific page type or unlink from a page type and create a unique layout for every page. Every page in the CMS has a type, so you can either have it so that it would use the style of the page type, or have it so that it has its own unique layout.
- When you edit content and save it you can see how it looks right away in the browser panel instead of having to open a new window and browse to the page
- You can resize the browser panel and see how the site looks in different sizes i.e test responsive layout directly through the cms
- You can create draft versions for pages which you can reload and edit at anytime
  - You will also get a visual indicator in the site tree besides pages that have a draft version.
  - The following indicators exist: "Will Publish", "Will Expire", "UnPublished", "HasDraft" along with a clock besides them if they are set to auto publish / auto expire

## Programming
- Programming in the CMS is done using C# and using the Razor Syntax
- Code is compiled and run on the fly using Razor Engine ( http://antaris.github.io/RazorEngine/ )
- You have access to the entire framework code, so you can pretty much do anything
- There are several Database Mappers and Helpers that are included with the framework.
- You can write Razor Code in any of the layout views in the CMS
- You can use short codes in any of the layout tabs and fields
- You can automatically execute some razor code when a page is published in the CMS by putting the code in the "On Publish Execute Code" Accordian tab under the Layouts tab for an individual page, or if you want to have the same code execute for all pages of the same type you can put this in the media type layouts section.

**Instead of programming directly in the CMS you can use an include ShortCode "{IncludeFile:'[Path-To-File]'}" which allows you to load a cshtml file, which will be executed at runtime, without any build process, kinda makes your workflow similar to PHP, simply make your change and refresh the page.**

*You will get full intellisence and code completion support when using Visual Studio*


## Multi Language
- You can create as many languages in the CMS as you like.
- Ability to add new languages to each page with ease: You can simply browse to a page and then switch the language and save and publish and your done, you have now created a new language version of the page.

## Publishing 
- Ability to automatically publish a page live at a specified date time
- Ability to automatically take down a page at a specified date time
- Ability to save as draft, you can come back and make changes to the draft page later, you can even send links to the draft version of the page inorder to get approval, and then push the page LIVE once you are done. 
  - You can also set the draft version to automaticly get pushed live at a specified date time.

## SEO
- All pages automaticly get a search engine friendly URL, you have the ability to change the uri segment for the page from the CMS
- Ability to change the SEO for every page: Page title, Meta Description, Meta Keywords, Meta Robots, Link title ( Url Segment for the page )
- Ability to create 301 or 302 redirects. You can create your own url redirect rules, you can have it so that /oldpage is redirected to /about/contact/

## Caching
- You have 3 levels of caching: Memory Caching, File System Caching and Redis Caching
- You have control over caching of pages. If you have set all pages to cache, the first time a visitor visits the page, it will generate a cache for that page and the next time the request will load from cache, drasticly speeding up performance. 
- If the page is cached then it bypasses the database connection all together making the load time extreamly fast. 
- If you have the page cached and there is an error connecting to the database, the system will attempt to load that page from cache if it exists.
- You have the ability to clear the cache of an individual page.
- Ability to clear all cache

## Roles and Permissions
- Ability to Manage Users and Roles
- Ability to limit access to a page or a page type ( role based ). Only people that belong to the role can access these pages in the CMS
- Ability to automatically propigate role limitations to all child pages.
- You can enforce the role limitations in the frontend as well, so you need to be logged in with appropriate credentials inorder to view these pages in the frontend.

## Admin Tools
- Database Compare Tool: Allows you to compare 2 environments to see what has changed i.e New Pages, New Fields, Different Content  etc.
  - NOTE: This will only work if the 2 environments have the same database schema.
- Database Backup and Restore Tool: Allows you to backup the current database to a folder and displays a list of all backed up files, you can then restore from a backup file. 
  - NOTE: This only works if both the web application and the database are on the same server.
- You can view the email logs from the CMS ( provided the emails were sent using the system its self )
- You can view the error logs from the CMS 

## Additional Functionality  / Features
- Built in Blog functionality, including moderated blog posts
- Built in File Manager with the following functionality:
  - Ability to create, rename and delete folders
  - Ability to create, rename, preview and open files in a new window
  - Ability to move files and folders around by dragging and dropping files and folders onto other folders
- Ability to Add / Edit / Delete Glossary terms.
- Ability to Create, Edit, Delete/Recover, Permanently Delete, Hide/Show, Duplicate and Publish/unpublish pages 
- Ability to search for a page based on any field value in the backend for quick access
- Several prebuilt field types are included which should sute most use cases.
- Code completion examples are provided to get started with programming in the Layouts tab using C# and the Razor Code syntax.

## Example showing how you can pull content from the CMS from a cshtml file
You will get full intellisence and code completion support when using Visual Studio

```html
@using FrameworkLibrary
@{
	var currentPage = Model.MediaDetail as MediaDetail;
	var referencesField = currentPage.LoadField("References");	
  
	if (referencesField != null && !string.IsNullOrEmpty(referencesField.FieldValue))
	{
		var content = currentPage.RenderField("References");
		<p>@content</p>
	}
  
	var resourcesField = (MediaDetailField)currentPage.LoadField("Resources");

	if (resourcesField != null && resourcesField.FieldAssociations.Any())
	{  
		<div>
			<h2>Resources</h2>
			<div class="resources__cards">
			@foreach (var fieldsAssociation in resourcesField.FieldAssociations)
			{
				var sectionTitle = fieldsAssociation.MediaDetail.SectionTitle;
				var briefDescription = fieldsAssociation.MediaDetail.RenderField("BriefDescription", false);
				var websiteAddress = fieldsAssociation.MediaDetail.RenderField("WebsiteAddress", false);
				
					<a href="@websiteAddress">
						<div></div>
						<div>
							<div>
								<h4>@sectionTitle</h4>
							</div>
							@Raw(briefDescription)
						</div>
						<div>
							<span>View Resource</span>
						</div>
					</a>
			}
			</div>
		</div>	  
	}  
}

```
