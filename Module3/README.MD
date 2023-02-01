# Module 3 - Working with Lists and Libraries

## **Topics**:

1. [Understanding the List and Library Class Hierarchy](#understanding-the-list-and-library-class-hierarchy)
2. [Retrieving List and Library Objects](#retrieving-list-and-library-objects)

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
