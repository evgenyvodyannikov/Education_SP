# Module 3 - Working with Lists and Libraries

## **Topics**:

1. [Understanding the List and Library Class Hierarchy](#understanding-the-list-and-library-class-hierarchy)
2. [Retrieving List and Library Objects](#retrieving-list-and-library-objects)
3. [Creating and Deleting List and Library Objects](#creating-and-deleting-list-and-library-objects)

### **Understanding the List and Library Class Hierarchy**

The SharePoint object model provides classes that you can use to work with lists, libraries, and files in your code.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215979168-cad4ee6d-81e2-4042-bb69-42d8af1eb418.png">
</p>

The **SPList** class represents a SharePoint list or library. Every list or library in a SharePoint deployment is represented by an **SPList** instance. Document libraries are essentially a specialized type of list, and consequently the **SPDocumentLibrary** class inherits from the **SPList** class. The SPList class includes collections of fields, list items, and folders.

The **SPListItem** class represents an individual item in a SharePoint list. A list item can include a value for each field defined in the parent list. If the list that contains the item is a document library, the SPListItem instance will have an associated file, which is represented by the SPFile class.

The **SPFolder** class represents a folder. This can be a folder within a list or library, or it can be a virtual directory on the SharePoint site. Both the SPList class and the SPWeb class expose collections of folders. The SPFolder class includes a collection of SPFile objects.

### **Retrieving List and Library Objects**

The SPList class **does not include a constructor**. When you need to work with a list or library in code, you **retrieve the SPList** instance from the **parent SPWeb** object. You can do this in various ways:

- Use the **SPWeb.Lists** property. The Lists property exposes a collection object of type **SPListCollection**. This is an enumerable collection of all the lists and libraries in the site.

- Use the **SPWeb.GetList method**. The GetList method enables you to retrieve a specific list or library by providing the site-relative URL of the list or library.

- Use the **SPWeb.GetListsOfType method**. The GetListsOfType method returns an **SPListCollection** object that contains all the lists or libraries of the specified base type. The method requires an argument of type **SPBaseType**. This is an enumeration that defines the fundamental types of SharePoint lists and libraries, such as **DocumentLibrary**, **GenericList**, and **Survey**.

- Use the **SPWeb.GetListFromWebPartPageUrl method**. The GetListFromWebPartPageUrl method enables you to retrieve the list or library that is associated with the first Web Part on a specific Web Part page.

The **SPListCollection** class also provides methods for retrieving lists:

- The **SPListCollection.GetList method** enables you to retrieve a list from the collection by name. The method **throws an exception** if the list does not exist within the collection.

- The **TryGetList method** also enables you to attempt to retrieve a list from a collection by name. However, if the list does not exist, the method **returns null** rather than throwing an exception.

Regardless of whether you are retrieving a list or a library, you always start by instantiating an SPList instance. If you are retrieving a library, and you require the additional functionality offered by the SPDocumentLibrary class, you can then cast the SPList instance to SPDocumentLibrary.

The following code example shows how to enumerate and interact with the lists and libraries in a SharePoint site:

```C#
	var web = SPContext.Current.Web;
	foreach(SPList list in web.Lists)
	{
	   // Prevent users from creating folders in the list.
	   list.EnableFolderCreation = false;
	   // Set the email address for the list, and enable email notifications.
	   list.EmailAddress = string.Format("{0}@sharepoint.contoso.com", list.RootFolder.Name);
	   list.EnableAssignToEmail = true;
	   // Persist list changes to the database.
	   list.Update();
	   // Check whether the list is a document library.
	   if (list is SPDocumentLibrary)
	   {
	      // Cast the list to use SPDocumentLibrary properties and methods.
	      var library = (SPDocumentLibrary)list;
	      // If the library is not a gallery…
	      if (!library.IsCatalog)
	      {
	         // Get a list of checked out files.
	         IList<SPCheckedOutFile> checkedOutFiles = library.CheckedOutFiles;
	         // Get the URL of the document template for the library.
	         string docTemplate = library.DocumentTemplateUrl;
	      }
	   }
	}
```

In most cases, the SPList class provides all the functionality you need to work with list items and documents.

As illustrated by the code example, the SPDocumentLibrary class includes some additional properties and methods that you may find useful in certain circumstances. 

For example, the **CheckedOutFiles** property returns a list of all the files that are currently checked out in the library, and the **DocumentTemplateUrl** property enables you to get or set the template that is applied when a user creates a new document in the library.

### **Creating and Deleting List and Library Objects**

You can use the SharePoint object model to create and delete lists and libraries programmatically.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215981635-1d65cb2e-c469-41b3-bf8d-bf7148c137f4.png">
</p>

**Note:** Document libraries are a subset of SharePoint lists in which the list template is of type **SPListTemplateType.DocumentLibrary**. Where this topic refers to lists, the concepts apply to all types of SharePoint lists, including document libraries.

**List definitions, list templates, and list instances**

Before you create your own lists in code, you need to understand some of the terminology used when describing SharePoint list functionality. When you create a new list on a SharePoint site, you are creating a list instance. The behavior and capabilities of the list instance are defined by a list definition or a list template.

The terms list definition and list template are often used interchangeably. The core concepts of list definitions and list templates are as follows:

- A **list definition** defines an **XML-based schema** for a list. This schema is deployed to the server file system, either within a site definition or within a Feature. The schema defines every aspect of the list, including the **fields** it should contain, the **views** and **forms** it provides, and the **content types** that it references. A list definition must specify one of the fundamental list types defined by the **SPBaseType** enumeration.

- A **list template file** points to a list definition. The list template file specifies how the list appears in the SharePoint user interface. For example, the list template file specifies the **display name** and **description** for the list and **whether it should be added** to the Quick Launch navigation menu by default. The list template file is what makes the list definition available for use, both in the UI and through the object model. List template files are deployed in Features.

- When a user has customized a list instance, either in SharePoint Designer or through the browser, he or she **can save the customized list as a custom list template**. The user can then create new lists from the saved list template.

List templates can come from three places: **standard templates** as defined by the SPListTemplateType enumeration, **custom templates** based on a list definition, or a **customized and saved list template**. In the SharePoint object model, list templates are represented by the **SPListTemplate** class, regardless of whether the list template points to a list definition or a saved template. 

You can retrieve list template objects in two ways:

- To retrieve all the list templates from a specific web, use the **SPWeb.ListTemplates** property. This returns an enumerable collection of all available list templates, regardless of whether they point to a list definition or a saved template.

- To retrieve all the available custom list templates, use the **SPSite.GetCustomListTemplates** method. This returns an enumerable collection of all the list templates that were saved by users in the site collection.

**Creating list instances**

To create a list instance programmatically, you call the **Add method** on the **SPListCollection** object exposed by the **SPWeb.Lists** property. The Add method includes seven overloads that enable you to create lists or libraries in various different ways. 

All overloads require you to specify a **title** and a **description** for the new list. However, you must also indicate the template or definition on which you want to base your new list. 

You can do this in several ways. For example:

- You can provide an **SPListTemplate** instance that represents a list definition or a custom list template.

- You can specify a **member of the SPListTemplateType** enumeration. This enumeration defines all the built-in list templates (which point to list definitions) provided by SharePoint Foundation.

- You can **specify the feature** that contains the list definition, together with an integer template type to indicate the associated list template.

Specifying a member of the SPListTemplateType enumeration is the most straightforward approach when you want to create a list from a SharePoint Foundation list template. 

If you want to create a list from a custom list template, or a list template defined by one of the SharePoint Server workloads, you typically retrieve an SPListTemplate instance and pass it to the SPListCollection.Add method.

The SPListCollection.Add method returns a **GUID** value that uniquely identifies the list instance that you have created. If you need to perform additional actions on the new list, you can use this GUID value to retrieve the SPList instance that represents the list.

The following code example shows how to add lists to a SharePoint site:
```C#
	var web = SPContext.Current.Web;
	// Create a list using a SharePoint Foundation list template.
	var contactsListID = web.Lists.Add("Project Contacts", "Use this list to store contact details for project stakeholders",SPListTemplateType.Contacts);
	// Create a list from a custom template.
	SPListTemplate template = web.ListTemplates["ContosoInventory"];
	var inventoryListID = web.Lists.Add("Project Inventory", "Use this list to monitor inventory levels for research activities", template);
	// Use the GUID to construct the list if you need to perform additional actions.
	SPList list = web.Lists[contactsListID];
	list.OnQuickLaunch = true;
	list.Update();
```

**Deleting list instances**

You can delete a list instance in two ways:

- You can call the **SPList.Delete method** on the list instance you want to delete.

- You can call the **SPListCollection.Delete method**, and specify the **GUID** of the list instance you want to delete.

The following code example illustrates how to delete a list:

```C#
	var web = SPContext.Current.Web;
	SPList list = web.Lists["Project Inventory"];
	list.Delete();
```

In most scenarios, calling the **SPList.Delete** method is the easiest way to delete a list. The **SPListCollection.Delete method** may be preferable if you can provide a **GUID** identifier for the list you want to delete without first instantiating an SPList object.

You can also recycle a list by calling the **SPList.Recycle** method, which returns the GUID for the list. When using this method, the list is placed in the recycle bin of the user account that runs the code.
