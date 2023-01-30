# Module 1 - First view to SharePoint
## **Lessons**:
1. Introducing the SharePoint Developer Landscape
   
#### **Workloads**:

SharePoint 2013 provides functionality across several broad functional areas, commonly known as **workloads**.

- **Portals** and **collaboration**. SharePoint provides many core capabilities, such as the ability to create **sites**, **workspaces**, **libraries**, and **lists**, which enable teams to work together effectively regardless of their physical location. 
- **Search**. SharePoint provides a comprehensive and highly extensible search platform that enables users to find information from a variety of content sources across an organization. 
- **Enterprise Content Management**. SharePoint provides a range of functionality to support business processes, such as **workflow**, **records management**.
- **Web Content Management**. SharePoint provides comprehensive **Content Management System (CMS)** features that can be used to manage and maintain a corporate web presence. 
- **Social and communities**. SharePoint provides functionality that enables users to connect with one another, share news and information, and build communities around common interests.
- **Business Connectivity Services**. SharePoint enables you to integrate data from external systems into solutions for SharePoint sites and Microsoft Office 2013 applications. 
- **Business Intelligence**. SharePoint provides a range of capabilities for providing insight into business data, from built-in tools such as Excel Services and PerformancePoint Services, to integration with SQL Server 2012 Analysis Services and SQL Server 2012 Reporting Services.

#### **The SharePoint 2013 Technology Stack**:
SharePoint Server 2013 builds on a variety of products and technologies. Understanding how these products and technologies fit together can be helpful when you design and build solutions for the SharePoint platform.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215442902-c3bf2c89-0ece-4647-acdf-f09accf5b427.png">
</p>

- **SharePoint, ASP.NET, and IIS** 
     
     SharePoint Server 2013 is an ASP.NET web application. SharePoint requires Internet Information Services (IIS) and ASP.NET 4 to run in integrated mode, where IIS and ASP.NET are essentially a single platform from the perspective of web applications. SharePoint adds custom modules and handlers to the IIS/ASP.NET integrated pipeline to implement the SharePoint page rendering process.

- **.NET Framework 4.5**
     
     SharePoint 2013 depends on various features of .NET Framework 4.5. In particular, the SharePoint claims infrastructure relies on Windows Identity Foundation to manage claims-based Windows identities. SharePoint 2013 workflows rely on Windows Workflow Foundation (WF45). 

- **Office Web Apps Server 2013**

     In SharePoint 2013, Office Web Apps Server is a separate server product that must run on separate hardware. This enables architects and administrators to scale an Office Web Apps server farm and a SharePoint server farm independently.

- **Workflow Manager 1.0**
     
     The SharePoint workflow infrastructure is now external to the SharePoint logical architecture. SharePoint workflows now run on an independent Workflow Manager infrastructure. You can install Workflow Manager on the same hardware as SharePoint Server 2013, which is useful for developer workstations and smaller SharePoint deployments. As deployments grow larger, architects and administrators will often scale out Workflow Manager to run on separate hardware.

- **SharePoint Foundation 2013**
     
     SharePoint Foundation 2013 represents the core set of SharePoint functionality. This includes key functional components such as the SharePoint service application architecture, the page rendering components, and the claims infrastructure, together with the ability to perform core tasks such as creating sites, managing content, and managing users.

-  **SharePoint Server 2013**
     
     SharePoint Server 2013 builds on SharePoint Foundation 2013 to provide more advanced functionality in each of the key workload areas.

#### **The SharePoint Page Rendering Process**:

In SharePoint, the parsing and page rendering process underpins almost all aspects of functionality and user experience. It is essential to gain a solid understanding of this process before you begin developing customizations for the SharePoint platform.

In terms of parsing and rendering, there are two distinct types of pages in SharePoint: **application pages** and **content pages**.

- **Application pages**
    
     An application page is an ASPX page that resides on the file system of SharePoint Web Front End (WFE) servers. These pages are deployed to the SharePoint file structure, which is often referred to as the SharePoint root, the SharePoint hive, or the 15 hive.
     
     Application pages are parsed and rendered by the ASP.NET parser as standard ASPX pages. Application pages are used to encapsulate functionality rather than content. For example, the administration pages and dialog boxes that are used to manage a SharePoint site are all application pages. 

- **Content pages**
     
     A content page is an ASPX page that is **dynamically** constructed when the page is requested. Content pages are formed by combining a page template from the file system with page content from the database. The page template effectively consists of placeholders, such as Web Part zones, into which content and controls are injected when the page is constructed. 

- **Ghosting and unghosting**

     Uncustomized content pages are sometimes referred to as ghosted pages. This reflects the fact that the page does not really exist as a single entity—it's a combination of a physical page template and a set of content from the content database.

     Administrators and power users can use SharePoint Designer to customize content pages within a site collection. *If this customization includes changes to the physical page template, the page becomes unghosted.* This means that the page can no longer be created dynamically by merging a physical page template from the site definition with content from the database. Instead, the entire ASPX page is stored in the content database and retrieved when requested.

     *Customized (or unghosted) content pages are parsed and rendered by the SharePoint safe mode parser*, rather than the ASP.NET parser. The safe mode parser includes various restrictions to prevent malicious users from using customized pages to launch attacks. In particular, the safe mode parser blocks the processing of any pages that contain inline script.
