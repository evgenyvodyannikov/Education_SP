# Module 5 - Working with Server-Side Code

## **Topics**:

1. [Introduction to Web Parts](#introduction-to-web-parts)
2. [Understanding the Web Part Life Cycle](#understanding-the-web-part-life-cycle)
3. [Visual Web Parts](#visual-web-parts)
4. [The .webpart File](#the-webpart-file)
5. [Introduction to Event Receivers](#introduction-to-event-receivers)

## **Introduction to Web Parts**

SharePoint enables you to create **Web Parts** that you can include on SharePoint pages. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216547615-8d71f56c-5c87-495e-bfbd-42d2738c41d6.png">
</p>

Web Parts provide flexibility to users who can add a Web Part to any page. You can make Web Parts that display data relevant to the current page, site, or other contextual component, or you can make Web Parts that display the same content wherever they are deployed. 

In SharePoint 2013, much of what you can achieve by using a Web Part can also be achieved by using an app part. In most cases, you should choose to use an app part in preference to a Web Part; however, there are still some circumstances in which choosing a Web Part would be the appropriate choice. 

If you do not have an app infrastructure configured for your SharePoint farm, you may choose to develop a Web Part instead of an app part. Another reason you may choose to develop a Web Part is to take **advantage of Web Part connections**. Web Parts can expose data to other Web Parts on the page, and Web Parts can then consume exposed data. You cannot consume or expose data to other Web Parts by using an app part, so if you need to use this functionality, developing a Web Part would be an appropriate choice. 

To develop a Web Part, you normally create a class that inherits from the WebPart class (in the `System.Web.UI.WebControls.WebParts` namespace). You then override methods in that class to control how your Web Part is rendered. You then compile the assembly and deploy it to the server. You use a **.webpart** Web Part definition file to add the Web Part to a Web Part Gallery, from which users, with the appropriate permissions, can add the Web Part to a page. 

## **Understanding the Web Part Life Cycle**

Web Parts have a life cycle that integrates with the page life cycle for a SharePoint page, or any ASP .NET webpage. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216548704-d814b11f-ddfa-4d1c-bb67-1020072c397e.png">
</p>

When you develop a Web Part, you typically create a class that inherits from the WebPart class (although there are other options, they are not commonly used). In your class, you can **override life cycle events**. It is important that you understand the order in which life cycle events occur to enable you to develop an effective Web Part. 

The Web Part life cycle events that you can override are

- **OnInit**. The OnInit method is used to initialize the Web Part. You should override this if you need to customize this process. 

- **OnLoad**. The OnLoad method is also used to initialize the control, but it should not be used to load data.

- **CreateChildControls**. The CreateChildControls method is the most commonly overridden method in the Web Part life cycle. This method is responsible for creating controls that appear on the Web Part. You should use this method to add controls to the pages control collection. 

- **EnsureChildControls**. The EnsureChildControls method ensures that CreateChildControls has executed. You do not normally need to override this method, but you should call this method before you attempt to access a control to prevent null reference exceptions from uninitialized controls. You should not call the CreateChildControls method directly because this may result in duplicate controls if the method has been called previously (for example, by SharePoint).

- **SaveViewState**. The SaveViewState method saves the view states for the Web Part and the controls on the Web Part. 

- **OnPreRender**. The OnPreRender method handles tasks that must be completed before the page is rendered. For example, to load data in a data-bound control. You should override this if you need to customize this process.

- **Page.PreRenderComplete**. The Page.PreRenderComplete event fires after the OnPreRender method completes for all of the controls on the Web Part. You should handle this event if you need to customize any of the controls after they load data but before they are rendered. 

- **Render**. The Render method renders the Web Part by writing HTML to the output stream. 

- **RenderContents**. The RenderContents method renders the contents of the Web Part by writing to the HTML output stream. This is the second most commonly overridden method. You should override this method if you want to **customize how your controls are rendered**, or to write HTML directly to the output stream. When you use the RenderContents method, you do not output the outer HTML tags and styling, because these are output by the Render method. 

- **OnUnload**. The OnUnload method runs after the request completes, and the rendered Web Part has been output to the HTML stream. You should override the OnUnload method if you created any objects that need explicit disposal, such as if you need to close a file or database connection.

If you are writing code to handle a page postback, you may need to handle a control postback event; for example, you may need to handle a button click event. 

These events are processed after the **CreateChildControls** method, and before the **OnPreRender** method. 

If you populate a control with data in the **CreateChildControls** method, it may overwrite changes that are included in the postback, and any data that you process in your control postback method may be inaccurate. If you need to load data, you should load the data in the **OnPreRender** method. This ensures that you do not overwrite data included in a postback.

## **Visual Web Parts**

Visual Web Parts are very similar to normal Web Parts; they inherit from the WebPart class and expose the same life cycle events. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216550144-f9382779-44a1-48cc-b5c4-ad7815c9a98d.png">
</p>

However, there are two major differences. 

- You define the user interface for the Web Part in a **custom control (ascx)** file.

- You can choose to add logic to the **Page_Load** method in the control code-behind file to control the Web Part when the custom control loads; typically, you will continue to use the **OnPreRender** method to load data into your control. 

By creating a custom control to define the user interface, you benefit from using the Visual Studio page designer to enable you to easily add controls to the page and lay out the controls according to your needs. This is significantly easier than adding controls and defining layout manually by using the **CreateChildControls** method. 

## **The .webpart File**

Typically, you will deploy a Web Part as a pre-compiled assembly, either as part of a farm solution, or a sandboxed solution. 

You must create a XML Web Part definition file that defines the assembly and type that contain your Web Part code, as well as metadata such as the name, description, and group of the Web Part. 

When you create a Web Part by using Visual Studio, Visual Studio generates this file, and includes it in a Feature automatically; however, it is important that you are aware of the structure and purpose of the Web Part definition file in case you need to customize it. 

When you add a Web Part to SharePoint, SharePoint stores the Web Part in a Web Part Gallery. When a user with the necessary permission edits a page, they can add Web Parts that appear in the Web Part Gallery. The Web Part Gallery actually contains **.webpart** files. When a Web Part is added to the page, SharePoint parses the .webpart file and loads the Web Part from the assembly identified in the file.

The following code example shows a .webpart file.
```XML
	<webParts>
	    <webPart xmlns="http://schemas.microsoft.om/WebPart/v3">
	        <metaData>
	            <!-- Add the fully qualifies type name and strong named assembly name -->
	            <type name="ContosoWebPartNamespace.ContosoWebPart", "WebPartAssembly Strong Name">
	            <!-- Add an error message to display if the Web Part does not import correctly -->
	            <importErrorMessage>Error importing Web Part.</importErrorMessage>
	        </metadata>
	        <data>
	            <properties>
	                <!-- Add properties that define the Web Part metadata -->
	                <property name="Title" type="string">Contoso Web Part</property>
	                <property name="Description" type="string">This is the Contoso Web Part</property>
	            </properties>
	        </data>
	    </webPart>
	</webParts>
```

## **Introduction to Event Receivers**

When you develop any solution, you will often need to respond to actions performed outside the scope of your solution. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216551472-01addee4-d745-49c6-b35c-a37b761329ef.png">
</p>

In traditional Windows Forms application development, a **common example is responding to a user clicking a button**. Developing solutions for SharePoint is no different; you will often want to respond to actions performed outside the scope of your solution. 

Event receivers in SharePoint enable you to respond to these actions. For example, you can develop an event receiver that SharePoint will call each time an item is added to a list. SharePoint enables you to respond to a variety of events raised by SharePoint components, including:

- Site collections
- Sites
- Lists
- Content types
- Features

The events that you can handle for each of these components, **referred to as event hosts**, differ—generally, they include an event when it is changed or deleted, and when child items are added, changed, or deleted. 

The event **hosts are hierarchical**, and **provide scope** for your event receivers. For example, you can handle an item being added by using a list as an event host. This will restrict your event handler to run only when an item is added to that particular list. 

Alternatively, you can register the same event receiver by using the site collection as the even host, causing the event receiver to run every time an item is added to any list in the site collection. 

**Note:** Feature receivers are a type of event receiver.

**Event handler synchronization**

An event may be handled either **synchronously** or **asynchronously**. This has important implications for server performance, and functional implications. 

**Synchronous events** run in the same thread as the code that raised the event. Frequently, this has a direct impact on SharePoint performance for users, for example, if the event was raised by a user browsing a SharePoint site, a synchronous event will run in the IIS process thread. A major **advantage of a synchronous event is that it can change an event**, for example by cancelling the event. If you use a synchronous event, the event process will always complete before the web request completes.

**Asynchronous events** run in a different thread to the main IIS process thread, and can run in parallel on multiple other threads. This has the advantage that **they do not block the IIS process thread**, and the request returns to the browser as fast as possible. The event handler may continue to run on the server after the web request that triggered the event completes. A significant implication of this is that asynchronous events should not change the event, because the event **may have already occurred**.

**Before and after events**

SharePoint exposes two broad types of events, **before events** and **after events**. 

Before events are events that end in “**ing**”; for example, **ItemAdding**. These events are triggered **before an action** is committed to the database and provide the opportunity for you to cancel the event, or run other necessary code before the change is persisted. 

After events are events that end in “**ed**”; for example, **ItemAdded**. These events are triggered after an action is committed to the database. You cannot cancel an event in an after event (although you could use code to reverse the change). 

**Before events always run synchronously**, this is necessary in case the event receiver makes changes to the event, such as by blocking the event. 

**After events typically run asynchronously**; although this is normally appropriate, you can choose to run an after event synchronously according to your requirements. You specify the event synchronization when you deploy your event receiver. 

**Remote event receivers**

In addition to deploying event receivers on your SharePoint server, you can also use **remote event receivers**. You can create a remove event receiver by specifying a **URL for a remote event handler**, or by using an app.
