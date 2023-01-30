# Module 1 - First view to SharePoint
## **Topics**:
1. [Workloads](#workloads)
2. [The SharePoint 2013 Technology Stack](#the-sharepoint-2013-technology-stack)
3. [The SharePoint Page Rendering Process](#the-sharepoint-page-rendering-process)
4. [Declarative components](#declarative-components)
5. [Web Parts](#web-parts)
6. [Application Pages](#application-pages)

### **Workloads**:

SharePoint 2013 provides functionality across several broad functional areas, commonly known as **workloads**.

- **Portals** and **collaboration**. SharePoint provides many core capabilities, such as the ability to create **sites**, **workspaces**, **libraries**, and **lists**, which enable teams to work together effectively regardless of their physical location. 
- **Search**. SharePoint provides a comprehensive and highly extensible search platform that enables users to find information from a variety of content sources across an organization. 
- **Enterprise Content Management**. SharePoint provides a range of functionality to support business processes, such as **workflow**, **records management**.
- **Web Content Management**. SharePoint provides comprehensive **Content Management System (CMS)** features that can be used to manage and maintain a corporate web presence. 
- **Social and communities**. SharePoint provides functionality that enables users to connect with one another, share news and information, and build communities around common interests.
- **Business Connectivity Services**. SharePoint enables you to integrate data from external systems into solutions for SharePoint sites and Microsoft Office 2013 applications. 
- **Business Intelligence**. SharePoint provides a range of capabilities for providing insight into business data, from built-in tools such as Excel Services and PerformancePoint Services, to integration with SQL Server 2012 Analysis Services and SQL Server 2012 Reporting Services.

### **The SharePoint 2013 Technology Stack**:

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

### **The SharePoint Page Rendering Process**:

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

### **Declarative components**:

With SharePoint, you can define and configure a wide range of custom components using **CAML** *(Collaborative Application Markup Language (CAML) is an XML-based language that is used in Microsoft SharePoint Foundation to define the fields and views that are used in sites and lists)*.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215461592-09c7921e-84be-4d22-b3e1-971d8087e972.png">
</p>

For example, you can use CAML to create and configure the following elements
- Field definitions.
- Content type definitions.
- List templates. Use these to enable users to create new instances of specified list definitions. 
- List instances. Use these to provision an instance of a list template.
- Content type bindings.
- Modules. Use these to deploy files, together with any associated metadata, to a SharePoint site.
- Event registrations.
- Custom actions.
- Workflow definitions.

The following code example shows how to use CAML to define a "use by" date site column:
```XML
Defining a Site Column Declaratively
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	   <Field
	          ID="{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}"
	          Name="UseBy"
	          DisplayName="Use By"
	          Type="DateTime"
	          Format="DateOnly"
	          Required="FALSE"
	          Group="Contoso Site Columns">
	   </Field>
	</Elements>
```

Declarative components are always defined within an **Elements** element, and the files containing these components are referred to as element manifests. Element manifests are covered in greater detail in the "SharePoint Features" topic in the next lesson.

**Where can you use declarative components?**

Declarative components are easier to read than their imperative equivalents. Because declarative components do not execute any code, there are fewer limitations on where and how you deploy your customizations. 

Declarative components are the most flexible of all approaches to building custom SharePoint functionality. You can deploy declarative components in SharePoint Features within solutions or apps, and you can use declarative components with both SharePoint Online and on-premises SharePoint 2013 deployments. 

### **Web Parts**

A Web Part is a **type of ASP.NET control**, and SharePoint Web Parts build on the ASP.NET Web Part framework. This framework is designed to enable users to modify the content and behavior of webpages by adding **custom controls** (the Web Parts) to **specific regions on the page (Web Part zones)**. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215464881-37202f53-a865-4e74-9a87-e13f304ae6c9.png">
</p>

In most cases, your Web Parts will inherit from the **System.Web.UI.WebControls**.**WebParts.WebPart** class. This class provides various life cycle events that integrate with the ASP.NET page life cycle of the page that contains the Web Part. 

For example, the WebPart class exposes **Init**, **Load**, **PreRender**, and **Unload** events that you can handle like you would in an ASPX code-behind file. The WebPart class also includes various abstract methods, such as **CreateChildControls** and **RenderContents**, which you can override to implement the functionality of your Web Part.

The following code shows the basic structure of a regular SharePoint Web Part:

```C#
//Creating a SharePoint Web Part
	public class ContosoWebPart : WebPart
	{
	   Label lblGreeting;
	   protected override void CreateChildControls()
	   {
	      //Create your controls within this method.
	      lblGreeting = new Label();
	      lblGreeting.Text = "Hello, World!"
	      this.Controls.Add(lblGreeting);
	   }
	   protected override void RenderContents(HtmlTextWriter writer)
	   {
	      // Define your rendering logic within this method.
	      writer.Write("<div id='contosowebpart'>");
	      lblGreeting.RenderControl(writer);
	      writer.Write("</div>");
	   }
	}
```

In addition to deploying the assembly that contains your Web Part code, *you must also deploy a Web Parts control description file to the Web Part Gallery*. The Web Parts control description file is a CAML-based XML file with a *.webpart file name extension* that specifies how the Web Part should appear in the Web Part Gallery when deployed. 

**When should you create Web Parts?**

In SharePoint 2013, you should use apps instead, at least wherever possible. Apps provide a more consistent installation, deployment, and life cycle management experience.
However, there are still scenarios in which you might want to create Web Parts. This is most likely when you want to take advantage of Web Part connections; for example, to provide data to other Web Parts, consume data from other Web Parts, or provide filters to other Web Parts.

### **Application Pages**

SharePoint uses application pages extensively to expose site management and administrative capabilities. Application pages are used to expose functionality that is not specific to sites or content.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215466831-aac61d40-5283-41d7-9153-ede201048265.png">
</p>


Creating a custom application page is one of the easier approaches to SharePoint development. You simply create an **ASP.NET forms (ASPX) page** and deploy it to the **15\TEMPLATE\LAYOUTS** folder (or a subfolder within) on each SharePoint WFE server. 

Your page is then available on **every site through the _layouts virtual directory**. For example, if you created and deployed an application page named MyApplicationPage.aspx:
- Users of a Finance site collection could access your page at http://finance.contoso.com/_layouts/MyApplicationPage.aspx.
- Users of a Sales site collection could access your page at http://sales.contoso.com/_layouts/MyApplicationPage.aspx.
- Users of a regional sales sub-site could access your page at http://sales.contoso.com/regions/EMEA/_layouts/MyApplicationPage.aspx.

In terms of capabilities, **application pages run with full trust and have full access to the server-side object model**. As such, there is little that you cannot do with a custom application page. Typically, application pages are used when you want to **expose the same functionality to every site** in a SharePoint server farm.

The **Microsoft.SharePoint.WebControls** namespace includes two base classes that you can use as the foundation for your custom application pages:
- LayoutsPageBase. Use this class when you want to create an application page that only privileged users can access.
- UnsecuredLayoutsPageBase. Use this class when you want to create an application page that anyone (even unauthenticated users) can access.



