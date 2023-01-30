# Module 1 - First view to SharePoint
## **Topics**:
1. [Workloads](#workloads)
2. [The SharePoint 2013 Technology Stack](#the-sharepoint-2013-technology-stack)
3. [The SharePoint Page Rendering Process](#the-sharepoint-page-rendering-process)
4. [Declarative components](#declarative-components)
5. [Web Parts](#web-parts)
6. [Application Pages](#application-pages)
7. [Timer Jobs](#timer-jobs)
8. [Event Recievers](#event-receivers)
9. [Workflow](#workflow)
10. [SharePoint Features](#sharepoint-features)

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

### **Timer Jobs**

SharePoint timer jobs are used to run **background tasks** that do not require user interaction on a scheduled basis. SharePoint 2013 uses built-in timer jobs extensively for a wide range of tasks, such as scheduled approval and publishing, processing health and diagnostic data, upgrading site collections, and applying expiration policies.

Custom timer jobs can be useful when you want to perform long-running or resource-intensive tasks without user interaction and without returning any information to a user. For example, you might use a timer job to aggregate and process list or site data across site collection boundaries. 

Timer jobs run in a separate process—the SharePoint timer service, or owstimer.exe—rather than the IIS worker process, **w3wp.exe**. This means that timer jobs should not affect the performance and stability of individual IIS application pools. It also means that you can run long-running processes without the risk of the process being interrupted by an application pool recycle.

### **Event Receivers**

You can use event receivers to create code that performs some action in response to an event in SharePoint. SharePoint 2013 exposes a wide variety of events. You can create event receivers for the following categories of events in SharePoint.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215468705-5fd4ba6f-308f-4b13-82b2-dbcaff4d37c3.png">
</p>

**There are two types of events in SharePoint:**

- Before events. A before event is fired immediately before an action happens, which allows the event receiver to cancel the action if required. 
- After events. An after event is fired immediately after an action happens, which allows the event receiver to perform any additional actions required in response to the action. 

The following code shows a simple example of an event receiver class that handles the ItemDeleting event:

```C#
//Creating an Event Receiver
	public class ContosoEventReceiver : SPItemEventReceiver
	{
	   public override void ItemDeleting(SPItemEventProperties properties)
	   {
	      properties.ErrorMessage = "You do not want to delete this item";
	      properties.Status = SPEventReceiverStatus.CancelWithError;
	   }
	}
```

### **Workflow**

By using workflows, you can **model and automate** business processes. Generally speaking, you create workflows when you want to capture user input from multiple people and perform actions in response to that user input. For example, suppose you wanted to automate a document publishing process. Your document might go through the following stages on the way to publication:

- Authoring. A user creates a first draft of the document.

- Technical review. Another user reviews the document for accuracy. If the technical reviewer is satisfied, the document proceeds to legal review. If not, the document goes back to the author for changes.

- Legal review. A member of the legal team reviews the document for compliance with regulations and local statute. If the legal reviewer is satisfied, the document proceeds to copy edit. If not, the document goes back to the author for changes.

- Copy edit. An editor checks the document for spelling, grammar, and style. If the editor requires major changes, the document goes back to the author for incorporation. If only minor changes are required, the document proceeds to publishing.

- Publishing. The document is submitted for printing and distribution.

Continuing with this scenario, let us assume that you want to automate this publishing process. In this case, you can create a workflow that assigns a task to a user at each stage. When the user marks his or her task as complete and provides any required information to the workflow, the workflow logic proceeds to another stage. In this example, the workflow would use branching logic to determine who should see the document next.

One of the key aspects of SharePoint workflow—and any other WF45 workflow—is that the **workflow is dormant (inactive) while waiting for user input**. In other words, the workflow does not actively wait for user input by polling for changes in field values or task completion statuses. Instead, WF45 will awaken, or rehydrate, the workflow when it has the information it requires to proceed. This enables workflows to become useful tools for managing long-running business processes.

### **SharePoint Features**

A SharePoint Feature is a framework for packaging and deploying declarative components. Features were introduced in SharePoint 2007 and have been a major component of SharePoint development ever since. The SharePoint development tools in Visual Studio 2012 automate much of the Feature development process and provide a Feature designer to help you assemble the Feature components. However, it is important for developers to understand what Features are and how they work. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/215471953-fc0d52ab-a99c-49f6-a988-b51535ddb659.png">
</p>

Features are contained within a folder. The **name of the folder is important**, because you will refer to the feature **by the name of this folder** when you install or uninstall the feature.

The root of this folder always contains a file named Feature.xml. This is known as the feature manifest, which describes the contents of the Feature package and indicates what SharePoint should do with the contents of the Feature when it is installed and activated. The feature folder also typically contains subfolders and files that contain the contents and resources that make up the Feature.

The following code shows a simple example of a feature manifest file:

```XML
A Feature Manifest File
	<Feature xmlns="http://schemas.microsoft.com/sharepoint"
	               Id="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
	               Title="Contoso Project Resources"
	               Description="Contains site columns, content types, and lists for Contoso project sites."
	               Scope="Web">
	   <ElementManifests>
	      <ElementManifest Location="SiteColumns\Elements.xml" />
	      <ElementManifest Location="ContentTypes\Elements.xml" />
	      <ElementFile Location="Pages\ContosoProject.aspx" />
	      <ElementFile Location="Images\ProjectIcon.png" />
	   </ElementManifests>
	</Feature>
```

For now, there are three key aspects of note in this feature manifest example:

- **Scope**. Features are always scoped to a particular level in the SharePoint hierarchy. The Scope attribute can take the values Farm, WebApplication, Site (a site collection), or Web (an individual sub-site). 

- **Element manifests**. Features use ElementManifest elements to indicate the location of an element manifest within the folder structure of the Feature.

- **Element files**. Features use ElementFile elements to indicate the location of files within the folder structure of the feature. Unlike element manifests, which SharePoint must parse to be able to provision the custom components described within, an element file is simply a file—such as an ASPX page or an image—that must be deployed by the Feature. 

Features are typically deployed as part of a farm solution, a sandboxed solution, or a SharePoint app, which manages the feature installation and activation process. If you deploy a Feature within a SharePoint app or a sandboxed solution, the Feature is never copied to the server file system; instead, it remains in the content database from where it is accessed when required.
