# Module 3 - Working with Lists and Libraries

## **Topics**:

1. [Understanding the List and Library Class Hierarchy](#understanding-the-list-and-library-class-hierarchy)

### **Understanding the List and Library Class Hierarchy**

The SharePoint object model provides classes that you can use to work with lists, libraries, and files in your code.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215979168-cad4ee6d-81e2-4042-bb69-42d8af1eb418.png">
</p>

The **SPList** class represents a SharePoint list or library. Every list or library in a SharePoint deployment is represented by an **SPList** instance. Document libraries are essentially a specialized type of list, and consequently the **SPDocumentLibrary** class inherits from the **SPList** class. The SPList class includes collections of fields, list items, and folders.

The **SPListItem** class represents an individual item in a SharePoint list. A list item can include a value for each field defined in the parent list. If the list that contains the item is a document library, the SPListItem instance will have an associated file, which is represented by the SPFile class.

The **SPFolder** class represents a folder. This can be a folder within a list or library, or it can be a virtual directory on the SharePoint site. Both the SPList class and the SPWeb class expose collections of folders. The SPFolder class includes a collection of SPFile objects.