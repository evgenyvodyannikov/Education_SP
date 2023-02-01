# Module 3 - Working with Lists and Libraries

## **Topics**:

1. [Understanding the List and Library Class Hierarchy](#understanding-the-list-and-library-class-hierarchy)
2. [Retrieving List and Library Objects](#retrieving-list-and-library-objects)
3. [Creating and Deleting List and Library Objects](#creating-and-deleting-list-and-library-objects)
4. [Working with List Items](#working-with-list-items)
5. [Working with Fields](#working-with-fields)
6. [Working with Files](#working-with-files)
7. [Approaches to Querying List Data](#approaches-to-querying-list-data)

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

### **Working with List Items**

Items in a SharePoint list are represented by the **SPListItem** class. This applies regardless of the type of list: for example, an **SPListItem** instance can represent an event in a **calendar**, a **document** in a **document library**, a **contact** in a contacts list, and so on. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215985766-4a8a67d1-7c5f-48db-a64a-863a43b090de.png">
</p>

The list items in a SharePoint list are stored in an enumerable collection of type **SPListItemCollection**, which you can access through the **SPList.Items** property.

**Adding items to a list**

The process for adding a new item to a SharePoint list consists of three high-level steps:

1. Call the **SPListItemCollection.Add method**. This creates a new list item and returns an SPListItem instance.
2. Specify **field values**, and other properties as required, on the SPListItem instance.
3. Call the **Update** method on the SPListItem instance to write the changes to the content database.

The following code example shows how to add a new item to a SharePoint list:

```C#
	var web = SPContext.Current.Web;
	var list = web.Lists["Project Contacts"];
	// Step 1: Call the SPListItemCollection.Add method.
	SPListItem contact = list.Items.Add();
	// Step 2: Specify field values.
	contact["Last Name"] = "Kretowicz";
	contact["First Name"] = "Marcin";
	contact["Company"] = "Contoso Pharmaceuticals";
	contact["Job Title"] = "Sales Director";
	contact["Business Phone"] = "425-555-8910";
	contact["Email Address"] = "marcin@contoso.com";
	// Step 3: Call the Update method.
	contact.Update();
```

**Retrieving and modifying list items**

The SPListItem class includes two identifier properties:

- The **SPListItem.ID** property returns an integer identifier for the list item. SharePoint lists assign incrementally increasing integer identifiers to new list items.

- The **SPListItem.UniqueId** property returns a **GUID** value that uniquely identifies the list item.

The SPList class provides methods that you can use to retrieve a list item using either the integer ID or the unique ID of the item:

- The **SPList.GetItemById** method returns the list item with the specified integer identifier.
- The **SPList.GetItemByUniqueId** method returns the list item with the specified GUID identifier.

You can also retrieve list items by using an indexer with an **SPListItemCollection** instance (for example, `myList.Items[identifier]`). 

The indexer allows you to specify a GUID value or an integer value. However, you should use this approach with extreme caution. The integer value you provide to the indexer represents the position of the list item in the collection. This is not necessarily the same as the integer identifier of the list item. 

As such, best practice is to use one of the SPList methods, rather than the SPListItemCollection indexer, to retrieve specific list items.

SharePoint list items often include many fields, and these fields can contain large amounts of data. As a result, retrieving a list item is **computationally expensive**. The **SPList.GetItemByIdSelectedFields** method provides a more efficient way to retrieve a list item, by specifying which field values you want to retrieve. This method returns an **SPListItem** instance in the same way as the **SPList.GetItemById** method, but the returned SPListItem instance only contains field values for the fields you specify when you call the method.

The following code example shows how to use the **SPList.GetItemByIdSelectedFields** method:

```C#
	var web = SPContext.Current.Web;
	var list = web.Lists["Project Contacts"];
	// This identifier value is typically retrieved by using a query class or LINQ to SharePoint.
	int itemID = 1;
	// Retrieve a list item containing only field values for Job Title and Email Address.
	SPListItem contact = list.GetItemByIdSelectedFields(itemID, "Job Title", "Email Address");
	// Update the selected field values.
	contact["Job Title"] = "Vice President, Sales";
	contact["Email Address"] = "vpsales@contoso.com";
	contact.Update();
```

After you retrieve a list item, you update field values and call the Update method in the same way as if you had created a new list item.

**Deleting list items**

You can delete a list item in three ways:

- You can call the **SPListItem.Delete** method on the list item you want to delete.
- You can call the **SPListItemCollection.Delete** method, and specify the **index** of the list item you want to delete.
- You can call the **SPListItemCollection.DeleteItemById** method, and specify the integer identifier of the list item you want to delete.

### **Working with Fields**

Up to this point, you have seen how to set simple string-based field values when you work with list items. However, in practice, SharePoint lists include fields of many different types, such as Text, DateTime, Lookup, Currency, and Geolocation. 

These field types are defined by the **SPFieldType** enumeration, and you can check the type of a field by retrieving the **SPField.Type** property. 

There are approximately thirty built-in field types, and you can also define custom field types if your project demands it.

When you set a field value on a list item, you must provide an object of the appropriate type. For simple field types, this is straightforward. 

For more complex field types, SharePoint provides **field value classes**. To set the value of a complex field type, you should provide an instance of the appropriate field value class.

**Field classes and field value classes**

The SharePoint object model includes several classes to represent different field types. These classes all inherit from the SPField class, and the class names all begin with SPField. For example:

- The **SPFieldBoolean** class represents a Boolean field.
- The **SPFieldCalculated** class represents a calculated field.
- The **SPFieldChoice** class represents a choice field.

For more complex field types, where the value cannot be represented by a simple .NET type, the object model includes a field value class that corresponds to the field class. For example:

- The **SPFieldUrl** class represents a field that contains URL values, and the **SPFieldUrlValue** class represents the associated field value.
- The **SPFieldLookup** class represents a lookup field, and the **SPFieldLookupValue** class represents the associated field value.
- The **SPFieldGeolocation** class represents a geographical location field, and the **SPFieldGeolocationValue** class represents the associated field value.

In many cases, field classes can parse values provided in various formats into an appropriate field value class instance. For example, you can set the value of an SPFieldGeolocation field by providing a string value in **Well-Known Text (WKT)** format. However, your code will typically be more robust (stable) if you construct an appropriate field value instance when you need to set the value of a list item field.

**Setting complex field values**

The following code example shows how to set values for complex field types:

```C#
	// Retrieve an arbitrary list item.
	var list = SPContext.Current.Web.Lists["Offices"];
	SPListItem item = list.GetItemById(1);
	// Set the value of a geolocation field.
	Double latitude = 51.4198;
	Double longitude = -2.6147;
	var geoValue = new SPFieldGeolocationValue(latitude, longitude);
	item["location"] = geoVal;
	// Set the value of a URL field.
	var urlValue = new SPFieldUrlValue();
	urlValue.Url = "http://bristol.contoso.com";
	urlValue.Description = "Bristol Office";
	item["website"] = urlValue;
	// Set the value of a multiple choice field.
	var multiValue = new SPFieldMultiChoiceValue();
	multiValue.Add("Parking");
	multiValue.Add("Meeting Rooms");
	multiValue.Add("Cafe");
	item["facilities"] = multiValue;
	// Call the Update method when you have finished making changes.
	item.Update();
```

**Retrieving complex field values**

When you use an indexer to retrieve the value of a list item field, the indexer returns a **System.Object**. You must cast the returned object to an appropriate type before you can get the field values in a meaningful format.

The following code example shows how to retrieve field values for complex field types:

```C#
	// Retrieve an arbitrary list item.
	var list = SPContext.Current.Web.Lists["Offices"];
	SPListItem item = list.GetItemById(1);
	// Retrieve the value of a geolocation field.
	var geoValue = item["location"] as SPFieldGeolocationValue;
	Double latitude = geoValue.Latitude;
	Double longitude = geoValue.Longitude;
	// Retrieve the value of a URL field.
	var urlValue = item["website"] as SPFieldUrlValue;
	string url = urlValue.Url;
	string description = urlValue.Description;
	// Retrieve the values in a multiple choice field.
	var selectedValues = new List<String>();
	var multiValue = item["facilities"] as SPFieldMultiChoiceValue;
	for (int i = 0; i < multiValue.Count; i++)
	{
	   selectedValues.Add(multiValue[i]);
	}
```

When you use an indexer with an **SPListItemCollection**, the indexer returns an object with an underlying type of String. In some cases, you cannot cast this string to the appropriate field value class. In these cases, you must use the **SPField.GetFieldValue** method to convert the string value into the appropriate field value type. 

For example, you cannot convert the string representation of a user field value into an **SPFieldUserValue** instance.

The following example shows how to use the SPField.GetFieldValue method:

```C#
	// Retrieve an arbitrary list item.
	var list = SPContext.Current.Web.Lists["Appraisals"];
	SPListItem item = list.GetItemById(1);
	// Retrieve the value of a user field and get the display name of the user.
	var field = item.Fields["Employee"] as SPFieldUser;
	var fieldValue = field.GetFieldValue(item["Employee"].ToString()) as SPFieldUserValue;
	string displayName = fieldValue.User.Name;
```

### **Working with Files**

In the SharePoint object model, files on a SharePoint site are represented by the **SPFile** class. 

If the file is a document in a document library, there is a one-to-one relationship between the **SPFile** object and the **SPListItem** object:

- You can retrieve the SPFile instance from the **SPListItem.File** property.
- You can retrieve the SPListItem instance from the **SPFile.Item** property.

The SPListItem class and the SPFile class represent different aspects of a document in a document library. You use the SPListItem class if you want to work with the **metadata of the document**, for example to update field values. 

You use the SPFile class if you want to work with the **document itself**, for example to check the document in or out, view version history, or to access the contents of a file as a stream or a byte array.

**Retrieving files**

Although you can retrieve SPFile instances from individual list items, this is not the best approach unless you specifically want to retrieve the SPListItem instance as well as the SPFile instance. 

The primary mechanism for accessing files is through the **SPWeb** class or the **SPFolder** class. Both of these classes include a Files property, which exposes an enumerable collection of **SPFile** objects. The SPWeb class also includes various methods that you can use to retrieve specific files or folders.

The following code example shows how to retrieve files programmatically:

```C#
	var web = SPContext.Current.Web;
	// Get the contents of a file as a string.
	string contents = web.GetFileAsString("Documents/Text.txt");
	// Get the contents of a file as a byte array.
	SPFile image = web.GetFile("Documents/Image.bmp");
	byte[] imageBytes = image.OpenBinary();
	// Get the contents of a file as a stream and save to the local file system.
	SPFile video = web.GetFile("Documents/Video.mp4");
	int bufferSize = 10 * 1024;
	using (Stream stream = video.OpenBinaryStream())
	{
	   using (FileStream fs = new FileStream(@"C:\LocalPath\Video.mp4", FileMode.Create, FileAccess.Write))
	   {
	      byte[] buffer = new byte[bufferSize];
	      int byteCount = 0;
	      while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
	      {
	         fs.Write(buffer, 0, byteCount);
	      }
	   }
	}
```

**Checking files in and out**

Check-outs and check-ins are a mechanism that enables users to prevent other users from modifying a document while they make changes. 

If a user checks out a document in a document library, the document is locked for editing by that user. No other users can make changes to the document while the check-out is in place. When the user checks in the document, the lock is removed and changes made by the user are visible to everyone.

You can use the SPFile class to check documents in and out programmatically. For example, you might want to develop a "bulk check-in" solution, so that users who have uploaded multiple documents to a library do not have to check in each file individually.

To check out a file, you call the **SPFile.CheckOut method**. If required, you can also specify a check-out type by providing a member of the **SPFile.SPCheckOutType** enumeration as an argument to the CheckOut method. The **SPFile.SPCheckOutType** enumeration defines the following values:

- None. This indicates that the document is not checked out.

- Offline. This indicates that the document is checked out for editing on a local computer. This is equivalent to selecting the Use my local drafts folder option when you check out a file through the browser UI.

- Online. This indicates that the document is checked out for editing on the server. This is the default value when you check out a document.

To check in a file, you call the **SPFile.CheckIn** method and provide a comment. If required, you can also use the **SPCheckinType** enumeration to indicate whether you are checking in a major version or a minor version, or whether you want to overwrite the current version.

The following code example shows how to check files in and out:

```C#
	var web = SPContext.Current.Web;
	// Retrieve the file.
	var file = web.GetFile("Documents/Invoice.docx");
	// Check the file out.
	if (file.CheckOutType == SPFile.SPCheckOutType.None)
	{
	   file.CheckOut();
	   // Update the file as required….
	   // Check the file back in.
	   file.CheckIn("Invoice updated.");
	}
```

**Adding and updating files**

To add a file to a SharePoint site, you call the **Add** method on an **SPFileCollection** object. Both the SPWeb class and the SPFolder class include a Files property that exposes an SPFileCollection object.

The **SPFileCollection.Add** method includes 19 overloads that enable you to add files in a multitude of different ways. In each case, you must provide a **site-relative or server-relative URL** to indicate the location at which you want to add the file. You typically provide the contents of the file as a byte array or a stream. Other overload variations enable you to specify file properties in various different ways.

The following code example shows how to add a file to a SharePoint site:

```C#
	var web = SPContext.Current.Web;
	// Get a byte array from another file, either from the local file system or from elsewhere on SharePoint.
	SPFile source = web.GetFile("Documents/Image.bmp");
	byte[] imageBytes = source.OpenBinary();
	// Add the file to a library.
	SPFile file = web.Files.Add(@"Images/NewImage.bmp", fileBytes);
	// Check in the file after adding it.
	file.CheckIn("New image added.");
```

You can also use the Add method to update existing files. Several Add method overloads include a Boolean parameter that you can use to indicate whether you want to overwrite existing files with the same name. This enables you to add a file as a new version of an existing file.

### **Approaches to Querying List Data**

In many scenarios, you will want to retrieve one or more list items that match certain criteria. 

For example, you might want to retrieve all list items that were modified after a particular date, or all list items that were modified by a particular user, or all list items that contain specific field values. You can approach these tasks in various ways.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215994257-032607f3-1d5f-4b87-a28c-49531377d376.png">
</p>


**Why you should not enumerate list items**

If you are new to SharePoint development, you might be tempted to retrieve list items by enumerating a list item collection and testing each list item against your selection criteria, as shown by the following example:

```C#
	var web = SPContext.Current.Web;
	var list = web.Lists["Inventory"];
	foreach (SPListItem item in list.Items)
	{
	   // Do something with the list item.
	}
```

This approach is considered **bad practice**. When you enumerate a list item collection, the enumerator must instantiate each list item in the collection. Instantiating a list item is a **computationally expensive process**, particularly if the list item contains large amounts of field data.

Rather than enumerating list item collections, SharePoint provides two optimized approaches that you can use to retrieve list items: **query classes**, and **LINQ to SharePoint**.

**Using query classes**

SharePoint provides two query classes for efficient retrieval of list data:

- **SPQuery**. Use the SPQuery class to retrieve list items that match specific criteria from a single list.

- **SPSiteDataQuery**. Use the SPSiteDataQuery class to retrieve and aggregate list item data from multiple lists in a site collection.

Both of these classes require you to create a query by using Collaborative Application Markup Language (CAML).

**Using LINQ to SharePoint**

You can use **Language Integrated Query (LINQ)** syntax to retrieve data from SharePoint lists. For server-side code, this functionality is provided by the **LINQ to SharePoint Provider**, which converts LINQ queries into CAML queries.
Before you can use LINQ to SharePoint in your code, you must generate entity classes to represent the lists you want to work with. SharePoint includes a command-line tool, **SPMetal.exe**, which you can use to generate entity classes for the lists in a specified site collection.
