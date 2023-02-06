# Module 8 - Client-Side Sharepoint Development

## **Topics**:

1. [Overview of the Managed CSOM](#overview-of-the-managed-csom)
2. [Using Client Context, Loading Objects, Executing Queries](#using-client-context-loading-objects-executing-queries)
3. [Reading SharePoint Data](#reading-sharepoint-data)
4. [Changing SharePoint Data](#changing-sharepoint-data)
5. [Handling Server-Side Errors](#handling-server-side-errors)
6. [Overview of the CSOM for JavaScript](#overview-of-the-csom-for-javascript)
7. [Using the Client Context Object](#using-the-client-context-object)
8. [Loading Objects and Running Queries](#loading-objects-and-running-queries)
9. [Reading SharePoint Data](#reading-sharepoint-data)
10. [Changing SharePoint Data](#changing-sharepoint-data)
11. [Handling Server-Side Errors](#handling-server-side-errors-1)
12. [Overview of the REST API](#overview-of-the-rest-api)
13. [SharePoint REST API URLs](#sharepoint-rest-api-urls)

## **Overview of the Managed CSOM**

The Managed **CSOM (Client-Side Object Model)** has many similarities to the JavaScript CSOM and you will notice that you take the same approach to solve the same coding problem when using each CSOM. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216968802-c26750a0-f1a8-4a79-8b40-93c8a103e3b8.png">
</p>

The code differences arise mostly because of the **differences** between **JavaScript** and a fully object-oriented .NET Framework language, such as Visual **C#**.

Consequently, if you know both JavaScript and C#, it is easy to write both browser-based code and managed code that works with SharePoint data.

**When to use the Managed CSOM**

You can use the Managed CSOM whenever you use the .NET Framework to build a client for SharePoint. For example:

- A **desktop application**. Such an application might use Windows Presentation Foundation (WPF) to implement a user interface and present SharePoint information along with data from other sources.

- A **website**. An ASP.NET web application, written in MVC or Web Forms, could use the Managed CSOM to access SharePoint data.

- A **cloud-hosted SharePoint app**. Cloud-hosted apps can include ASP.NET managed code that runs in the remote web. You can call SharePoint functions from ASP.NET code by using the Managed CSOM.

You cannot use the Managed CSOM in a SharePoint-hosted app because only client-side JavaScript code can be included in such apps.

**Components of the Managed CSOM**

The classes and functions that make up the Managed CSOM are stored in two files: `Microsoft.SharePoint.Client.dll` and `Microsoft.SharePoint.ClientRuntime.dll`. 

You can find these files in SharePoint **ISAPI** folder. To build an application that uses the Managed CSOM, you must add a reference to both of these DLLs in Visual Studio.

Just like the JavaScript CSOM, the Manage CSOM sends requests to the Client.svc WCF service. 

**Using the CSOM proxy**

The Managed CSOM uses batched requests, just like the JavaScript CSOM does. Your managed code can make one or more requests by working with the CSOM proxy object.

When you call the **ExecuteQuery()** method, all these batched requests are sent to the **Client.svc** service in a single operation. The Client.svc works with the service object model to execute the request against the SharePoint content database. Data is returned in the JSON format

## **Using Client Context, Loading Objects, Executing Queries**

Just like with the **JavaScript CSOM**, when you use the **Managed CSOM**, you must start by creating a **client context** object. 

The context object represents the user's current location within SharePoint and you can use it to gain access to the current site collection, site, and other objects.

The following code demonstrates how to create a context object by using the Managed CSOM in C# code. The context object is used to access the site collection and return its URL.

```C#
	public string getSiteCollectionUrl () {
	   string appWebUrl = Page.Request["SPAppWebUrl"];
	   using (ClientContext context = new ClientContext(appWebUrl));
	   {
	      Site siteCollection = context.Site;
	      context.Load(siteCollection, s=>s.Title, s=>s.ServerRelativeUrl);
	      context.ExecuteQuery();
	      return siteCollection.Url;
	   }
	}
```

Notice the following features of the preceding code example:

- **appWebUrl**. To use the client context object, you must pass a string URL to the SharePoint site. In a cloud-hosted app, a URL for the app web is always found in the **SPAppWebUrl** request parameter. You can use this to supply the necessary URL to the client context.

- **Load()**. Because the Manage CSOM uses batched queries and the sevice proxy, you must use the Load() method to set up the object in the client context object for retrieval from the server. In the preceding example, the site collection is loaded in this way. You can set up a batch of requests by loading multiple objects before you execute a single query to retrieve them. If you want to specify the properties of the object that the query will return, pass lambda expressions to the Load() method. In the preceding example, lambda expressions are used to request the Title and ServerRelativeUrl properties.

- **ExecuteQuery()**. Use this method to execute the batch of queries.

## **Reading SharePoint Data**

When you are working with SharePoint data, you must frequently begin by finding out what lists are present in a site and what items are in those lists. 

You can display this information in a table of data or populate the fields in an editor form. The Manage CSOM provides a range of functions that make reading data simple, for example:

- **ClientContext.Web.Lists**. This property of the SharePoint site returns a collection of all the lists and document libraries in the site.

- **ClientContext.Web.Lists.GetByTitle()**. This method on a collection of lists returns a specific list that matches the title passed.

- **List.GetItems()**. This method on a list returns a collection of items. If you use the method with no parameters, all the items in the list are returned. Alternatively, you can specify a CAML query to filter and order the information returned.

This example shows how use the **Web.Lists** property, the **Lists.GetByTitle()** method, and the **List.GetItems()** method from the Managed CSOM. The call to **GetItems()** uses a CAML query to order the results alphabetically. 

```C#
	public string investigateContent () {
	   // Get the app web URL.
	   string appWebUrl = Page.Request["SPAppWebUrl"];
	   // Set up the client context.
	   using (ClientContext context = new ClientContext(appWebUrl))
	   {
	      // Start by loading the site.
	      currentWeb = context.Web;
	      context.Load(currentWeb);
	      // Now we have the SharePoint site, load the lists in the site.
	      ListCollection allLists = currentWeb.Lists;
	      context.Load(allLists);
	      // Set up a CAML query to load items in the Suggestions list.
	      string query = "<View><Query><OrderBy><FieldRef Name='Title' /></OrderBy></Query>" +
	         "<ViewFields><FieldRef Name='ID' />< FieldRef Name='Title' /></ViewFields></View>";
	      CamlQuery camlQuery = new CamlQuery();
	      camlQuery.ViewXML = query;
	      // Load the suggestions list.
	      List suggestionsList = allLists.GetByTitle("Suggestions");
	      context.Load(suggestionsList);
	      // Get the items in the suggestions list by using the CAML query.
	      ListItemCollection items = suggestionsList.GetItems(camlQuery);
	      context.Load(items);
	      // Now we have batched all the requests, we can execute the query.
	      context.ExecuteQuery();
	      // Tell the user about the lists.
	      string message = "The lists are as follows: ";
	      foreach (List currentList in allLists) 
	      {
	         message += currentList.Title + " ";
	      }
	      // Tell the user about the items in the suggestions list.
	      message += "The suggestions are as follows: ";
	      foreach (ListItem currentItem in items)
	      {
	         message += currentItem.Title + " ";
	      }
	      return message;
	   }
	}
```

## **Changing SharePoint Data**

The Managed CSOM includes a range of classes and methods that you can call to **create**, **update**, and **delete** items in SharePoint lists. 

These include the following:

- **ListCollection.Add()**. This method creates a new list in the Lists collection for a SharePoint site.

- **ListCreationInformation**. This class enables you to set the properties of a new list. You must create a ListCreationInformation object and set its properties, and then pass the ListCreationInformation object to the **ListCollection.Add()** method.

- **List.AddItem()**. This method creates a new item in a SharePoint list.

- **ListItemCreationInformation**. This class enables you to set the properties of a new list item. Use the class as you use ListCreationInformation for new lists.

- **ListItem.Update()**. This method enables you to update list items. When you set properties of an existing list item, you must call the Update() method to save your changes. 

- **ListItem.DeleteObject()**. This method remove an item from the list.

The following code shows an example class with methods you can call to create a new list, create a new item in the Suggestions list, update an existing item, and delete an item.

```C#
	public class SharePointEditor 
	{
	   string appWebUrl;
	   public SharePointEditor() 
	   {
	      appWebUrl = Page.Request["SPAppWebUrl"];
	   }
	   public void CreateList (string name, string description) 
	   {
	      using (ClientContext context = new ClientContext(appWebUrl)
	      {
	         // Start by creating a new list creation information object.
	         ListCreationInformation listInfo = new ListCreationInformation();
	         listInfo.Title = name;
	         listInfo.Description = description;
	         listInfo.TemplateType = (int)ListTemplateType.GenericList;
	         // Create the new list item and execute the query.
	         List newList = context.Web.Lists.Add(listInfo);
	         context.ExecuteQuery();
	      }
	   }
	   public void CreateItem (string title) 
	   {
	      using (ClientContext context = new ClientContext(appWebUrl) 
	      {
	         // Get the suggestions list.
	         List suggestionsList = context.Web.Lists.GetByTitle("Suggestions");
	         context.Load(suggestionsList);
	         // Create the List Item Creation Info object.
	         ListItemCreationInformation itemInfo = new ListItemCreationInformation();
	         // Create the new item and set its properties.
	         ListItem newItem = suggestionsList.AddItem(itemInfo);
	         newItem["Title"] = title;
	         // You must call the Update method before you execute the query.
	         newItem.Update();
	         context.ExecuteQuery();
	      }
	   }
	   public void UpdateItem (int id, string title) 
	   {
	      using (ClientContext context = new ClientContext(appWebUrl) 
	      {
	         // Get the suggestions list.
	         List suggestionsList = context.Web.Lists.GetByTitle("Suggestions");
	         context.Load(suggestionsList);
	         // Get the item and set its properties.
	         ListItem item = suggestionsList.GetItemById(id);
	         item["Title"] = title;
	         // You must call the Update method before you execute the query.
	         item.Update();
	         context.ExecuteQuery();
	      }
	   }
	   public void DeleteItem (int id) 
	   {
	      using (ClientContext context = new ClientContext(appWebUrl) 
	      {
	         // Get the suggestions list.
	         List suggestionsList = context.Web.Lists.GetByTitle("Suggestions");
	         context.Load(suggestionsList);
	         // Get the item and delete it.
	         ListItem item = suggestionsList.GetItemById(id);
	         item.DeleteObject();
	         // Remember to execute the query.
	         context.ExecuteQuery();
	      }
	   }
	}
```

## **Handling Server-Side Errors**

In the Managed CSOM, server-side errors can be handled in the same way as in the JavaScript CSOM: by using an **ExeceptionHandlingScope** object and calling **StartTry()**, **StartCatch()**, and **StartFinally()** methods. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216972007-292e37d2-6d9f-4b56-8ef8-4eb9f11d8843.png">
</p>

However, C# supports **using() { } code blocks**, which you can use to ensure that the scope and its try, catch, and finally objects are disposed correctly before you call **ExecuteQuery()**.

The following code shows how to create an object that uses an exception handling scope to implement try/catch/finally code on the server.

```C#
	public class ErrorScope ()
	{
	   public void scope () {
	      // Get the App Web URL so that you can get the client context.
	      string appWebUrl = Page.Request["SPAppWebUrl"];
	      using (ClientContext context = new ClientContext(appWebUrl))
	      {
	         // Create and start the exception handling scope.
	         ExceptionHandlingScope xScope = new ExceptionHandlingScope(context);
	         using (xScope.StartScope())
	         {
	            // Define the try block.
	            using (xScope.StartTry())
	            {
	               // Place code that may generate exceptions here.
	            }
	            // Define the catch block.
	            using (xScope.StartCatch())
	            {
	               // Place code that handles exceptions here.
	            }
	            using (xScope.StartFinally())
	            {
	               // Place code that should execute regardless of errors here.
	            }
	         }
	      }
	      context.ExecuteQuery();
	   }
    }
```

## **Overview of the CSOM for JavaScript**

In earlier versions of SharePoint, most custom code ran within SharePoint server-side processes and therefore was able to call classes and methods in the server-side object model. 

In SharePoint 2010, a new client-side object model (CSOM) was introduced, which enabled developers to call core SharePoint functionality from browsers, Silverlight applications, and other clients. 

The CSOM executed requests by sending them to a Windows Communication Foundation (WCF) endpoint. In SharePoint 2013, because the new app model emphasizes client-side code, the CSOM has been greatly expanded: you can now use it to access both SharePoint core functionality and enterprise search, business connectivity services, managed metadata, social networking, enterprise content management, web content management, record management, and so on.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216982731-eb496b1e-4893-4e5b-b252-a44240531431.png">
</p>

**Components of the CSOM**

The classes and functions that make up the JavaScript CSOM are stored in two files: **sp.js** and **sp.runtime.js**. You can find these files in the SharePoint LAYOUTS folder. 

In SharePoint 2013, the functions in these JavaScript files call a WCF service named **Client.svc** that runs on all SharePoint web front-end servers. This service is found in the SharePoint ISAPI folder. The Client.svc service is also available in any SharePoint site by using the alias /_api. 

For example, if your site URL is:

http://intranet.contoso.com

then the Client.svc service can be found at:

http://intranet.contoso.com/**_api**

The Client.svc service uses the server-side object model to communicate with the SharePoint content database. 

Although you can use the JavaScript CSOM without any other script libraries, Visual Studio includes the jQuery library in all app projects by default because it eases development of server-side code. 

For example, by using the **$(document).ready()** jQuery function, you can ensure that no CSOM functions run before the Document Object Model (DOM) is fully loaded.

**Linking to script libraries**

In a page within a SharePoint app, use `<script>` tags to link to all the JavaScript libraries you need. You can either include these files in the app or link to their network locations. 

Visual Studio includes the following `<script>` tags in app projects. These link to **sp.js** and **sp.runtime.js** stored on the SharePoint server.

**Linking to the JavaScript CSOM**

```HTML
	<script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
	<script type="text/javascript" src="/_layouts/15/sp.js"></script>
```

Alternatively, you can copy the sp.runtime.js and sp.js files into the /Scripts folder in your app and modify the link to that folder. However, in most cases, because the app must connect to SharePoint to access content, it is logical to download the CSOM from SharePoint as well.

Visual Studio includes the following `<script>` tags in app projects to link to jQuery.

**Linking to jQuery**

```HTML
	<script type="text/javascript" src="../Scripts/jquery-1.7.1.min.js"></script>
```

Notice that jQuery is included in the app Scripts folder by default. You might change this link in the following circumstances:

- You want to use a later version of jQuery. If you want to use a more up-to-date version of jQuery, obtain the script library from http://jquery.com and add it to the Scripts folder in your app. You must update the version number in the `<script>` tag. 

- You want to use jQuery from a Content Delivery Network (CDN). A CDN is a set of high-performance servers distributed around the world. When you request a file from a CDN, a server physically close to your location responds. This can accelerate the performance of your application, but it requires an Internet connection. If you link to jQuery in a CDN, you can remove the jQuery file from your app's Scripts folder. Two possible CDNs that you can use for jQuery are:
   - http://code.jquery.com. This CDN is managed by the jQuery Foundation.

   - http://ajax.aspnetcdn.com/ajax/jQuery. This CDM is managed by Microsoft.

In addition to links to script libraries, remember that you must link to your own JavaScript files, such as the **app.js** file that apps use by default.

**Using the CSOM proxy**

When you make one or more requests to SharePoint by using the JavaScript CSOM, you work with a JavaScript object that acts as a proxy for the **Client.svc** WCF service. 

These requests are grouped into a batch and sent in a single operation to the WCF service in the form of XML. The Client.svc works with the service object model to execute the request against the SharePoint content database. 

When there is data to return, such as a list of items in a list, the data is sent in the JavaScript Object Notation (JSON). You can work with this form of data very easily in JavaScript to present it to users. For example, it is easy to loop through collections. 

## **Using the Client Context Object**

When you access SharePoint through the JavaScript object model, you must start by creating a client **context object**, much as you would when your write server-side SharePoint code. 

The context object represents the user's current location within SharePoint, and you can use it to gain access to the current site collection, site, and other objects. 

The following code demonstrates how to create a context object by using the JavaScript CSOM. The context object is used to access the site collection and return its URL.

```JavaScript
	"use strict";
	var Contoso = window.Contoso || {}
	Contoso.SiteCollection = function () {
	   var siteCollection,
	         context,
	         getSiteCollection = function () {
	            context = new SP.ClientContext.get_current();
	            siteCollection = context.get_site();
	            context.load(siteCollection);
	            context.executeQueryAsync(onSuccess, onFailure);
	         },
	         onSuccess = function () {
	            alert("Site Collection URL: " + siteCollection.get_url());
	         },
	         onFailure = function(sender, args) {
	            alert("Could not obtain the site collection URL");
	         }
	   return {
	      execute: getSiteCollection
	   }
	}();
	$(document).ready(function () {
	   Contoso.SiteCollection.execute();
	});
```

In the preceding code, notice that the query is executed **asynchronously** and the data returned is processed in the separate **onSuccess()** callback function. 

This pattern ensures that long-running queries do not halt a process and freeze the browser.

There are a range of other methods, called in the same way as you call **get_site()** in the preceding example, that enable you to access other information from the client context object. 

These include:

- **get_web()**. This method returns the current SharePoint site (**SPWeb**). This may be the top-level site in a site collection, or a subsite. In a SharePoint-hosted app, the current web is always the app web.

- **get_web().get_currentUser()**. This method, which you must access through the current SharePoint site, returns the identity of the user that made the request. You can use the user to access profile properties or permissions. 

- **get_web().get_siteGroups()**. This method returns any SharePoint groups that have been defined for the current site.

## **Loading Objects and Running Queries**

The JavaScript library uses a proxy class to interact with the SharePoint Client.svc service and execute batches of queries. 
This architecture means that you must take two steps to perform a data operation in SharePoint:
	
1. Load an object. When you load an object, you set it up in the client context object for retrieval from the server. You can load multiple objects to batch them together for retrieval in a single XML query. You can use two methods to load objects:

   - **Load()**. This method specifies an object or collection to retrieve, such as a SharePoint site or the items in a list. In addition to the object to retrieve, you can specify the properties of the object to retrieve. Do this by passing the names of the properties as string parameters to the Load() method. If you do not specify any property names, all the properties of the object are loaded.

   - **LoadQuery()**. This method specifies information to retrieve by defining a Language Integrated Query (LINQ) request.

2. Execute the operation. When you call the **executeQueryAsync()** method, the batched XML requests are sent to the **Client.svc** service. Because this is an asynchronous operation, the browser does not halt while it waits for a response, and you must define callback functions that run when a response is received from the server. 

The following code shows how to load an object into the client context and how to execute the operation asynchronously and display the results.

```JavaScript
	"use strict";
	var Contoso = window.Contoso || {}
	Contoso.SiteCollection = function () {
	   var siteCollection,
	         context,
	         getSiteCollection = function () {
	            context = new SP.ClientContext.get_current();
	            siteCollection = context.get_site();
	            context.load(siteCollection, "Title", "ServerRelativeUrl");
	            context.executeQueryAsync(onSuccess, onFailure);
	         },
	         onSuccess = function () {
	            alert("Site Collection URL: " + siteCollection.get_url());
	         },
	         onFailure = function(sender, args) {
	            alert("Could not obtain the site collection URL");
	         }
	   return {
	      execute: getSiteCollection
	   }
	}();
	$(document).ready(function () {
	   Contoso.SiteCollection.execute();
	});
```

## **Reading SharePoint Data**

The JavaScript CSOM makes it easy to obtain the lists and libraries in a SharePoint site, the items in a list, and a specific item in a list. 

These methods enable you to display data to the user, for example by using jQuery to update a web page element. You can also use these methods when you want to display current values for the user to edit. 

You can use the following methods to read data from SharePoint:

- **get_lists()**. This method is available from a SharePoint site object and returns all the lists in the site.

- **get_lists().getByTitle()**: You can use the getByTitle method on any collection of lists to obtain a list with a specific title.

- **getItems()**. You can use the getItems method on any list to return all the items in the list. Alternatively, by passing a Collaborative Application Markup Language (CAML) query to getItems, you can filter and order the items returned.

- **getItemById()**. If you already have the ID of the relevant item, you can use getItemByID to return the item.

This example shows how use the **get_lists()** method, the **get_lists().getByTitle()** method, and the **getItems()** method from the JavaScript CSOM. The call to getItems() uses a CAML query to order the results alphabetically. 

```JavaScript
	"use strict";
	var Contoso = window.Contoso || {}
	Contoso.ReadData = function () {
	   var site,
	         allLists,
	         suggestionsList,
	         items,
	         context,
	         getData = function () {
	            context = new SP.ClientContext.get_current();
	            site = context.get_web();
	            context.load(site);
	            // Now we have the SharePoint site, load the lists in the site.
	            allLists = site.get_lists();
	            context.load(allLists);
	            // Set up a CAML query to load items in the Suggestions list.
	            var query = "<View><Query><OrderBy><FieldRef Name='Title' /></OrderBy></Query>" +
	               "<ViewFields><FieldRef Name='ID' />< FieldRef Name='Title' /></ViewFields></View>";
	            var camlQuery = new SP.CamlQuery();
	            var camlQuery.set_viewXml(query);
	            // Load the suggestions list.
	            suggestionsList = site.get_lists().getByTitle("Suggestions");
	            context.load(suggestionsList);
	            // Get the items in the suggestions list by using the CAML query.
	            items = suggestionsList.getItems(camlQuery);
	            context.load(items);
	            context.executeQueryAsync(onSuccess, onFailure);
	         },
	         onSuccess = function () {
	            // Tell the user about the lists.
	            var message = "The lists are as follows: ";
	            var listEnumerator = allLists.getEnumerator();
	            while (listEnumerator.moveNext()) {
	               message += listEnumerator.get_current().get_title() +  " ";
	            }
	            alert(message);
	            // Tell the user about the items in the suggestions list.
	            message = "The suggestions are as follows: ";
	            var itemEnumerator = items.getEnumerator();
	            while (itemEnumerator.moveNext()) {
	               message += itemEnumerator.get_current().get_item('Title') +  " ";
	            }
	            alert(message);
	         },
	         onFailure = function(sender, args) {
	            alert("Could not obtain the data in the current site");
	         };
	   return {
	      execute: getData
	   };
	}();
	$(document).ready(function () {
	   Contoso.ReadData.execute();
	});
```

In the preceding example, notice that an enumerator object is used to loop through the lists and the items. 

## **Changing SharePoint Data**

When you create new items, update existing items, or delete items in a SharePoint list by using the JavaScript CSOM, you must still use a client context object, and load and execute a query just as you would for methods that read data. 

Use the following approaches to edit SharePoint items:

- To create a new list. Get the lists collection for the site, and then call the **add()** method on that lists collection. You must create a new **ListCreationInformation** object and pass it to the add() method. You can use the ListCreationInformation object to set properties such as a title for the list.

- To create a new item in a list. Get a specific list, and then call the **additem()** method on that list. You must create a new **ListItemCreationInformation** object and pass it to the additem() method. You can set properties on the item by calling the item.set_item() method.

- To update an existing item in a list. Get a single item in a list by using the **getItemById()** method on the list. After you have the item, use the **set_item()** method to update its fields, and then call update() to save changes before you call **context.executeQueryAsync()**.

- To delete an existing item in a list. Get a single item in a list by using the **getItemById()** method on the list. After you have the item, call **deleteObject()** before you call **context.executeQueryAsync()**.

The following example defines an object in the Contoso namespace with methods for creating, updating and deleting items in a list named Suggestions. You could call these methods, for example, when a user clicks a submit input control.

```JavaScript
	"use strict";
	var Contoso = window.Contoso || {}
	// This is an object to store the items in the Suggestions list.
	Contoso.Suggestions;
	Contoso.SuggestionsList = function () {
	   createSuggestion =  function (subject, feedback) {
	      // Get the client context and suggestions list.
	      var context = new SP.ClientContext.get_current();
	      var list = context.get_web().get_lists().getByTitle("Suggestions");
	      context.load(list);
	      // Create the new item and set fields.
	      var listItemCreationInfo = new SP.ListItemCreationInformation();
	      var newItem = list.addItem(listItemCreationInfo);
	      newItem.set_item("Subject", subject);
	      newItem.set_item("Feedback", feedback);
	      // Remember to call update before executing the query.
	      newItem.update();
	      context.executeQueryAsync(onSuccess, onError);
	   },
	   updateSuggestion = function (id, subject, feedback) {
	      // Get the client context and suggestions list.
	      var context = new SP.ClientContext.get_current();
	      var list = context.get_web().get_lists().getByTitle("Suggestions");
	      context.load(list);
	      var itemToUpdate = list.getItemById(id);
	      itemToUpdate.set_item("Subject", subject);
	      itemToUpdate.set_item("Feedback", feedback);
	      // Remember to call update before executing the query.
	      newItem.update();
	      context.executeQueryAsync(onSuccess, onError);
	   },
	   removeItem = function (id) {
	      // Get the client context and suggestions list.
	      var context = new SP.ClientContext.get_current();
	      var list = context.get_web().get_lists().getByTitle("Suggestions");
	      context.load(list);
	      // Get the item from its ID.
	      var itemToDelete = list.getItemById(id);
	      // Delete it before executing the query.
	      itemToDelete.deleteObject();
	      context.executeQueryAsync(onSuccess, onError);
	   },
	   onSuccess = function () {
	      alert("The operation completed successfully);
	   },
	   onError = function (sender, args) {
	      alert("The operation caused an unexpected error);
	   };
	   return {
	      create: createSuggestion,
	      update: updateSuggestion,
	      delete: removeSuggestion
	   };
	} ();
```

## **Handling Server-Side Errors**

In JavaScript, it is easy to handle errors that arise on the client by using the **try/catch/finally** construction. 

When you run a query on the server, you specify the name of two functions in the call to **executeQueryAsync**: The first function is called if the query succeeds, and the second function is called if the query fails. This failure function is a good place to handle problems that arise on the server. 

However, the **failure callback function** is only called when the server responds to the client. The failure callback function executes on the client and has limited access to server information. 

Remember that, when you use the CSOM, you can batch many queries into a single call to the **Client.svc** service. If an operation causes an error, the whole batch fails and you have no way to ensure that subsequent operations complete. 

You also cannot instruct the server how to respond to errors on the client.
To solve these issues, CSOM includes a mechanism with which you can send error-handling instructions to the server along with your batched query. 

The mechanism is implemented by the **ExceptionHandlingScope** object. This object enables you to define a **try/catch/finally** construction that applies while the query executes on the server.

The following code shows how to create an object that uses an exception handling scope to implement try/catch/finally code on the server.

```JavaScript
	"using strict";
	var Contoso = window.Contoso || {}
	Contoso.ErrorScope = function() {
	   var site,
	         scope = function() {
	            var context = new SP.ClientContext.get_current();
	            // Create and start the exception handling scope.
	            var e = new SP.ExceptionHandlingScope(context);
	            var s = e.startScope();
	            // Define the try block.
	            var t = e.startTry();
	            // Place code that may generate exceptions here.
	            t.dispose(); // This call to dispose marks the end of the try block.
	            // Define the catch block.
	            var c = e.startCatch();
	            // Place code that handles exceptions here.
	            c.dispose(); //This call to dispose marks the end of the catch block
	            // Define the finally block.
	            var f = e.startFinally();
	            // Place code that should execute regardless of errors here.
	            f.dispose(); // This call to dispose marks the end of the finally block.
	            // You must also dispose of the exception handling scope before you execute the query.
	            s.dispose();
	            context.executeQueryAsync(onSuccess, onFailure);
	         },
	         onSuccess = function () {
	            alert("Query was successful");
	         },
	         onFailure = function () {
	            alert("Query failed");
	         }
	   return {
	      execute: scope
	   }   
	} ();
	$(document).ready(function () {
	   Contoso.ErrorScope.execute();
	});
```

## **Overview of the REST API**

The JavaScript CSOM that you saw in the previous lesson is a set of JavaScript functions that sends calls to the **Client.svc** Windows Communication Foundation (WCF) service. 

This web service can also be used directly by formulating and sending your own calls instead of relying on the CSOM library. 

It is easy to create and send such calls because the Client.svc library is a RESTful web service. R

ESTful web services have the following characteristics:

- **Logical URLs**. The REST service itself has a URL. By specifying a URI within the service's URL, you can define the object or collection of objects to work with. For example, in SharePoint, the Client.svc is available in the **/_api** subfolder of any site. To select a list named "Discussion," you could use the URI **/_api/web/lists/getbytitle('Discussion')**.

- **Support for HTTP verbs**. To specify the operation you want to perform in a RESTful service, you use HTTP verbs. For example, to read information, you use the GET verb. To remove an item, you use the DELETE verb. By using a combination of a URL with an HTTP verb, you specify the object or objects to select and the operation to perform on those objects.

- **Support for OData**. RESTful services use the Open Data Protocol (OData) to create queries for data. For example, you can use an OData operator such as **$select** in a URL to filter the collection of objects that your operation will act on. The OData protocol also specifies that RESTful services should return data in the Atom Publishing Protocol (**AtomPub**) or JavaScript Object Notation (**JSON**) formats.

One advantage of RESTful services is that you can easily explore them with a web browser. This is because a RESTful service uses the HTTP protocol, with user-friendly URLs that you can easily enter manually, and returns XML data that the browser can display. 

This is extremely helpful during development because you can use a web browser such as Internet Explorer to query data and examine the structure of the response. 

For example, you can obtain the properties of a SharePoint website by entering the following URL in the Internet Explorer address bar:

**http://intranet.contoso.com/_api/web**

Another advantage for JavaScript developers is that the jQuery library has helpful functions that you can use to formulate REST calls quickly. Because you can request JSON data, it is also very easy to process the results.

The following code show how to use the jQuery **getJSON()** function to obtain information about the current SharePoint site from the REST API.

```JavaScript
	$(document).ready( function () {
	   $.getJSON(
	      "http://intranet.contoso.com/_api/web",
	      function (data) {
	         alert('The SharePoint site is called: ' + data.d.Title);
	      }
	   )
	});
```

## **SharePoint REST API URLs**

Before you begin programming against the SharePoint REST API, it is essential to understand **how REST API URLs are constructed**. W

ith this understanding, you can easily formulate the URLs to call any REST API method and pass parameters. 

The following examples work with a SharePoint site at the following URL:

`http://intranet.contoso.com/`

Therefore, all REST API operations must start with the following URL, which is the location of the Client.svc service:

`http://intranet.contoso.com/_api/`

**Common SharePoint objects**

The following table shows the URLs you must use to access SharePoint site collections, sites, lists, and other objects.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/217003849-f8679d4d-86e1-4c9b-a56c-492774e0ddfe.png">
</p>

**Controlling the objects returned**

Many of the URLs listed in the previous table return collections of objects. When you have large content databases, lists can contain thousands of items and sites can contain many lists. 

Therefore, it is essential to be able to control the collection of objects that a REST API call returns. 

You can do this by using OData query operators. The operators you can use include the following:

- **$select**. Use the $select operator to specify the fields that the query will return. If you do not use the $select operator, all the fields are returned expect any fields that are likely to be very large, such as attached files.

- **$order**. Use the $order operator to specify one or more fields by which to order the results.

- **$filter**. Use the $filter operator to set conditions. Only items that satisfy the conditions will be returned. For example, you could use the $filter operator to return only items whose Title starts with an "A".

- **$top**. Use the $top operator to specify a number of objects to return. 

- **$skip**. Use the $skip operator to specify a number of objects to omit from the results. **$top** and **$skip** are used to implement paging. For example, to show the second page of results, when there are ten results on each page, you would specify **$top=10** and **$skip=10**.

The following table shows some REST API URLs that use the OData operators to control the objects returned:

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/217004599-5112a392-e1c2-4744-b773-31bbd5b9e614.png">
</p>

**Note:** Try exploring your SharePoint REST API by typing URLs similar to those in the preceding tables into the Internet Explorer address bar. Substitute the URL of your own SharePoint site. By examining the AtomPub XML returned in each case, you can become proficient in formulating REST API URLs. These skills will be helpful when you write JavaScript code that call the REST API.
