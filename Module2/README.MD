# Module 2 - Working with SharePoint objects

## **Topics**:

1. [The SharePoint Object Hierarchy](#the-sharepoint-object-hierarchy)
2. [The SPFarm Class](#the-spfarm-class)
3. [Working with Services](#working-with-services)
4. [Working with Web Applications](#working-with-web-applications)
5. [Site Collections and the SPSite Class](#site-collections-and-the-spsite-class)
6. [Individual Sites and the SPWeb Class](#individual-sites-and-the-spweb-class)

### **The SharePoint Object Hierarchy**

All of the SharePoint 2013 object models provide a core set of classes that represent key components of a SharePoint deployment. Just like the logical architecture of a SharePoint deployment, these classes are arranged in a hierarchical manner.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215712989-d9de2f02-741a-4821-a2ce-74cca4a59697.png">
</p>

The core classes in the server-side object model are as follows:
- **SPFarm**. The SPFarm class represents a SharePoint 2013 farm in the logical architecture sense: a **configuration database**, a **collection of services**, a **collection of service applications**, and a **collection of web applications**. 
- **SPService**. The SPService class represents a SharePoint **service instance**, such as Excel Calculation Services, the Machine Translation Service, or the User Profile Service.
- **SPServiceApplication**. The SPServiceApplication class represents a **SharePoint service application**, such as the Search Service Application, the Managed Metadata Service application, or the User Profile Service Application.
- **SPWebApplication**. The SPWebApplication class represents a SharePoint web application. A web application can contain **one or more site collections**.
- **SPSite**. The SPSite class represents a SharePoint **site collection**. A site collection can contain one or more sites.
- **SPWeb**. The SPWeb class represents an **individual SharePoint site**. A site can contain lists, libraries, and various other artifacts. Each site can also contain one or more subsites, which are also represented by SPWeb objects. 
- **SPList**. The SPList class represents a SharePoint list or library.

Together, these classes represent the hierarchy of a SharePoint deployment in its entirety. Each object exposes an enumerable collection of the objects below it in the hierarchy.

### **The SPFarm Class**

SPFarm objects are the highest-level object in the hierarchy and, as their name implies, they represent the SharePoint farm as a logical entity. The SPFarm class **does not include a constructor**. Instead, the class **provides a global static object** that represents the current farm through the Local property.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215716257-20e27591-89ba-47da-9657-e39ade82a58a.png">
</p>

The following code example shows how to get a reference to the local SharePoint farm by using the server-side object model:

```C#
//Getting a Reference to the Local SharePoint Farm 
	SPFarm farm = SPFarm.Local;
```

SPFarm objects **represent farm-level configuration settings**, and act as an overall container for all other objects in the farm. For example, the SPFarm class exposes properties such as DefaultServiceAccount and TimerService, and contains collection properties such as **Servers**, **Services**, and **Solutions**.

**Note:** To use the SPFarm class in your code, you must add a reference to the ```Microsoft.SharePoint.Administration``` namespace.

In general, you will only use SPFarm objects if you are creating administrative applications for SharePoint. You do not need to use this object to access site collections, sites, lists, or list data. However, the SPFarm class is useful when you need to retrieve farm-level configuration settings for use elsewhere in your code. 

For example, you can use the **Id** property to retrieve the **unique identifier for the local farm**, which you need when you want to configure trust relationships between SharePoint farms so you can share service applications. In other scenarios, you might want to use the **CurrentUserIsAdministrator method** to determine whether the current user is a member of the farm administrators group before performing some configuration tasks.

The following code example shows how to work with SPFarm properties and methods:
```C#
//Working with SPFarm Objects
	//Get the unique identifier for the local farm.
	Guid farmID = SPFarm.Local.Id;
	//Determine whether the current user is a farm administrator.
	bool isAdmin = SPFarm.Local.CurrentUserIsAdministrator();
```

**Note:** You can also work with SPFarm objects in Windows PowerShell. The **Get-SPFarm** cmdlet returns the local farm instance.
Because the SPFarm class represents farm-level configuration and infrastructure settings, you cannot use the SPFarm class from sandboxed solutions or from client-side code. There is no client-side equivalent to the SPFarm class. For the same reasons, you cannot use the SPFarm class when you work with SharePoint Online subscriptions.

### **Working with Services**

SharePoint 2013 provides functionality through **several farm-wide services**. Many services, such as Excel Calculation Services or the User Profile Service, underpin specific service applications. Other services, such as the Microsoft SharePoint Foundation Web Application service or the Distributed Cache service, underpin a broader range of SharePoint functionality.

Although services **are defined at the farm level**, service instances actually run on individual servers. In a scaled-out SharePoint deployment, running service instances are often distributed between servers to even out the workload of each server. 

In a highly available SharePoint deployment, instances of the same service often run on more than one server. As such, it is important to distinguish between services and service instances. 

The SharePoint server-side object model uses the following classes to represent services and service instances:
- **SPService**. This class represents a **farm-scoped service**. All farm-wide SharePoint services derive from the SPService class. 
- **SPServiceInstance**. This class represents a **server-scoped service instance**. The SPServiceInstance class associates a specific service with a specific server.

**Working with SPService and SPServiceInstance objects**

The following code example shows how to check whether at least one instance of a specified service is running in the local SharePoint farm:
```C#
    string upssTypeName = "User Profile Synchronization Service";
	bool isRunningInstance = false;
	foreach (SPService service in SPFarm.Local.Services)
	{
	   if (String.Equals(service.TypeName, upssTypeName))
	   {
	      foreach (SPServiceInstance instance in service.Instances)
	      {
	         if (instance.Status == SPObjectStatus.Online)
	         {
	            isRunningInstance = true;
	         }
	      }
	   }
	}
	if (isRunningInstance)
	{
	   // At least one instance of the User Profile Synchronization Service is running.
	}
```

The **SPWebService** class is primarily a management container for SharePoint web applications (**SPWebApplication** objects). Like all SharePoint services, the SPWebService class derives from the SPService class. However, the SPWebService class includes some useful additional functionality. F

or example, you will work with the SPWebService class if you want to:
- Apply configuration changes to multiple web applications.
- Programmatically create or delete web applications.
- Change default settings for new content databases.
- Retrieve farm-scoped features from the SharePoint environment.
- Apply Web.config modifications to one or more web applications.

The SPWebService class includes two useful static properties: **ContentService** and **AdministrationService**. The ContentService property returns the SPWebService instance that contains regular (content) web applications. The AdministrationService property returns the SPWebService instance that contains the Central Administration web application. 

The following code example shows how to enumerate the web applications in SPWebService instances:
```C#
	var contentService = SPWebService.ContentService;
	foreach (var webApp in contentService.WebApplications)
	{
	   // Perform actions on each content web application.
	}
	var adminService = SPWebService.AdministrationService;
	foreach (var webApp in adminService.WebApplications)
	{
	   // Perform actions on the Central Administration web application.
	}
```

### **Working with Web Applications**

SharePoint **web applications are containers for site collections and sites**. Web applications are also the items in the object hierarchy that map SharePoint sites to Internet Information Services (IIS) websites. 

By default, there is a **one-to-one** mapping between SharePoint web applications and IIS websites. If you use the Central Administration website to create a web application, SharePoint provisions a corresponding IIS website for you, using the URL, port number, and host header you specify. Many settings are configured at the web application level, such as resource throttling settings, authentication providers, and permission policies.

In the SharePoint object model, web applications are represented by the **SPWebApplication** class. Although you can create and delete web applications programmatically (by working with the parent SPWebService object), this is not a common requirement. Web applications are typically created by administrators, using either Windows PowerShell or the Central Administration website. As a developer, you are more likely to work with existing **SPWebApplication** objects to retrieve or modify application-scoped settings.

You can instantiate an SPWebApplication object in various ways. For example, you can:
- Enumerate or index the **WebApplications** property of the parent SPWebService object.
- Use the **SPWebApplication constructor** and specify the display name of the web application, the parent SPWebService object, and the **SPApplicationPool** object that represents the credentials under which the web application process runs.
- Retrieve the **WebApplication** property from an **SPSite** object, to get the parent web application for a specific site collection.

The following code example shows how to retrieve an SPWebApplication object and modify settings:

```C#
	// Get a reference to the parent SPWebService instance.
	var contentService = SPWebService.ContentService;
	// Use an indexer to retrieve the web application by display name.
	var webApp = contentService.WebApplications["Contoso Content"];
	// Change the maximum file size (in MB) that users can upload.
	webApp.MaximumFileSize = 75;
	// Persist the changes.
	webApp.Update();
```

In common with many SharePoint objects, **you must call the Update method** after making changes to an SPWebApplication object. The Update method serializes the SPWebApplication object and updates the configuration database. Any changes you make will not be applied to the web application if you do not call the Update method.

**Web application zones**

In most cases, a SharePoint web application has a **one-to-one relationship with an IIS website**. However, you can extend a web application to additional IIS websites. In the SharePoint logical architecture, each additional IIS website is represented by a web application zone. 

A single web application can be extended to up to five zones. Extending a SharePoint web application to additional zones effectively provides users with additional paths to the same content. Each IIS website provides access to the same set of site collections, but each can have a unique URL, port number, host header, protocol, bindings, and Web.config file. 

### **Site Collections and the SPSite Class**

As the name suggests, a site collection is a collection of individual sites. Site collections also represent the point in the object hierarchy at which control shifts from farm administrators (who work primarily on the server) to end users (who interact with SharePoint through a web browser). In the SharePoint object model, site collections are represented by the **SPSite** class.

**Note:** The shift in focus from farm administrators to end users is also reflected in the server-side object model namespace. The classes discussed up to this point all reside in the ```Microsoft.SharePoint.Administration``` namespace. 
The **SPSite** class, together with the classes beneath it in the object model hierarchy, resides in the ```Microsoft.SharePoint``` namespace.

As a SharePoint developer, you will work with site collections on a regular basis. Site collections are significant for several reasons. 

First, they provide a security boundary in SharePoint deployments. SharePoint **groups**, **users**, and various **security settings** are defined and managed at the site collection level. 

Although **authentication** is managed at the **web application** (or web application zone) level, **authorization** is managed at the **site collection** level and below. 

Many artifacts, such as site columns, content types, master pages, apps, Web Parts, and themes, are typically (if not strictly always) deployed to and scoped at the site collection level.

You can instantiate an SPSite object in several different ways. For example, you can:
- Pass a URL to the SPSite constructor.
- Enumerate or index the Sites collection of the parent SPWebApplication object.
- Retrieve the current site collection from the execution context, by using static properties of the SPContext class.

The following code example shows how to use various approaches to retrieve an SPSite instance:

```C#
	// Pass a URL to the SPSite constructor.
	var site1 = new SPSite("http://team.contoso.com");
	// Retrieve an SPSite from the parent SPWebApplication.
	var contentService = SPWebService.ContentService;
	var webApp = contentService.WebApplications["Contoso Content"];
	var site2 = webApp.Sites["team.contoso.com"];
	// Retrieve an SPSite from the current execution context.
	var site3 = SPContext.Current.Site;
	// Dispose of SPSite objects where appropriate after use.
	site1.Dispose();
	site2.Dispose();
```

Creating SPSite objects is **memory intensive**, and after you finish using an SPSite object, you must **dispose** of it.

Each of the client-side object models for SharePoint 2013 provides a way of working with site collections:
- When you use the JavaScript object model, site collections are represented by the **SP.Site object**.
- When you use the managed client object model, site collections are represented by the ```Microsoft.SharePoint.Client.Site``` class.

When you use the REST API, you can access a site collection by constructing a URL in the format ```http://{site_collection_URL}/_api/site```, for example http://team.contoso.com/_api/site.

### **Individual Sites and the SPWeb Class**

In the SharePoint server-side object model, an individual site is represented by the **SPWeb** class.

Almost all the information that end users work with on SharePoint sites is stored in **lists** of some sort. These range from simple lists, such as **tasks lists** or **announcements lists**, through calendars, to document libraries and page libraries. Every list in SharePoint belongs to an **individual site**. As such, instantiating an SPWeb object is very often the first thing you do when you want to programmatically interact with SharePoint content. 

**Note:** The names SPSite and SPWeb refer to site collections, and sites can be confusing. Whereas farm administrators and end users typically talk in terms of site collections and sites, developers often talk in terms of **sites** (meaning site collections) and **webs** (meaning individual sites). 

Every **SPSite object has a root web**, which is the **SPWeb** object that shares the URL of the SPSite object. As such, an SPSite object always contains at least one SPWeb. End users (or developers) with sufficient privileges can add additional webs to the site collection, up to a supported limit of 250,000 webs per site collection in most scenarios.

 Unlike other objects in the SharePoint hierarchy, **SPWeb objects can act as parents to other SPWeb objects**. From an end user perspective, this allows users to develop an information architecture consisting of sites, subsites, sub-subsites, and so on. In other words, you can structure your maximum of 250,000 webs per site collection into as many hierarchical levels as you want.

You can instantiate an SPWeb object in several different ways. For example, you can:

- Access the **RootWeb** property of an **SPSite** object.
- Enumerate or index the **AllWebs** collection of an **SPSite** object.
- Retrieve a specific web by calling the **SPSite.OpenWeb** method.
- Retrieve the current web from the execution context, by using static properties of the **SPContext** class.

The following code example shows how to use various approaches to retrieve an SPWeb instance:

```C#
Instantiating SPWeb Objects
	var site = new SPSite("http://team.contoso.com");
	// Get the root web for the site collection.
	var web1 = site.RootWeb;
	// Retrieve a specific web from the parent site collection.
	var web2 = site.AllWebs["finance"];
	var web3 = site.OpenWeb("finance");
	// Retrieve a web from the current execution context.
	var web4 = SPContext.Current.Web;
	// Dispose of objects where appropriate after use.
	web1.Dispose();
	web2.Dispose();
	web3.Dispose();
	site.Dispose();
```

Creating SPWeb objects is **memory-intensive**, and after you finish using an SPWeb object, you must **dispose** of it. SPWeb objects are similar to SPSite objects in this regard.

**Webs in client-side code**

Each of the client-side object models for SharePoint 2013 provides a way of working with webs:
- When you use the JavaScript object model, site collections are represented by the **SP.Web** object.
- When you use the managed client object model, site collections are represented by the **Microsoft.SharePoint.Client.Web** class.
- When you use the REST API, you can access a web by constructing a URL in the format ```http://{web_URL}/_api/web```, for example http://team.contoso.com/finance/_api/web.
