# Module 8 - Client-Side Sharepoint Development

## **Topics**:

1. [Overview of the Managed CSOM](#overview-of-the-managed-csom)
2. [Using Client Context, Loading Objects, Executing Queries](#using-client-context-loading-objects-executing-queries)

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
