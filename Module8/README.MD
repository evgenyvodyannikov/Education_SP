# Module 8 - Client-Side Sharepoint Development

## **Topics**:

1. [Overview of the Managed CSOM](#overview-of-the-managed-csom)
2. [Using Client Context, Loading Objects, Executing Queries](#using-client-context-loading-objects-executing-queries)
3. [Reading SharePoint Data](#reading-sharepoint-data)
4. [Changing SharePoint Data](#changing-sharepoint-data)

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