# Module 2 - Working with SharePoint objects

## **Topics**:

1. [The SharePoint Object Hierarchy](#the-sharepoint-object-hierarchy)
2. [The SPFarm Class](#the-spfarm-class)
3. [Working with Services](#working-with-services)

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