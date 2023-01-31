# Module 2 - Working with SharePoint objects

## **Topics**:

1. [The SharePoint Object Hierarchy](#the-sharepoint-object-hierarchy)

### **The SharePoint Object Hierarchy**:

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