# Module 5 - Working with Server-Side Code

## **Topics**:

1. [Introduction to Web Parts](#introduction-to-web-parts)
2. [Understanding the Web Part Life Cycle](#understanding-the-web-part-life-cycle)
3. [Visual Web Parts](#visual-web-parts)
4. [The .webpart File](#the-webpart-file)
5. [Introduction to Event Receivers](#introduction-to-event-receivers)
6. [Developing an Event Receiver](#developing-an-event-receiver)
7. [Deploying an Event Receiver](#deploying-an-event-receiver)
8. [Introduction to Timer Jobs](#introduction-to-timer-jobs)
9. [Developing a Timer Job](#developing-a-timer-job)
10. [Deploying a Timer Job](#deploying-a-timer-job)
11. [Work Item Timer Jobs](#work-item-timer-jobs)
12. [Developing a Work Item Timer Job](#developing-a-work-item-timer-job)

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

## **Developing an Event Receiver**

To develop an event receiver, you simply create a type that derives from a base class, and then override an event method. There are different base classes for event receivers including:

- **SPWebEventReceiver**. Use the SPWebEventReceiver base class to handle site and site collection life cycle events.

- **SPListEventReceiver**. Use the SPListEventReceiver base class to handle list life cycle events, which include events such as a field being added to a list, in addition to a list being created or deleted.

- **SPItemEventReceiver**. Use the SPItemEventReceiver base class to handle item life cycle events.

- **SPEmailEventReceiver**. Use the SPEmailEventReceiver base class to handle an email message being received by the event host.

- **SPWorkflowEventReceiver**. Use the SPWorkflowEventReceiver base class to handle workflow life cycle events.

- **SPSecurityEventReceiver**. Use the SPSecurityEventReceiver base class to handle security events, for example a new group being created, or a user being added to a group.

You must select the appropriate base class based on the event you need to handle. After you select the appropriate base class, you must override base class methods with your event handler implementation.

The following code example shows an example of how to handle an item being added to a list.

```C#
	public class EventReceiver : SPItemEventReceiver
	{
	    public override void ItemAdding(SPItemEventProperties properties)
	    {
	        base.ItemAdding(properties);
	    }
	}
```

**Cancelling an event**

When you develop an event receiver for a **before event**, you may use the event receiver to cancel the event. 

Most event handler methods accept a properties parameter. You should use the **Status** property of the properties parameter to cancel the event by setting the Status property to a member of the **SPEventReceiverStatus** enumeration. 

You can choose to cancel an event with or without displaying an error. If you choose to display an error, you should also provide either an error message by setting the **ErrorMessage** property, or the URL of an error page that the user should be redirected to, by setting the **RedirectUrl** property.

The following code example shows how to cancel an event.

```C#
	public override void ItemAdding(SPItemEventProperties properties)
	{
	    // Cancel an event and do not display an error.
	    properties.Status = SPEventReceiverStatus.CancelNoError;
	    // Cancel an event and display an error message.
	    properties.Status = SPEventReceiverStatus.CancelWithError;
	    properties.ErrorMessage = "Event cancelled.";
	    // Cancel an event and redirect the user to an error page.
	    properties.Status = SPEventReceiverStatus.CancelWithRedirectUrl;
	    properties.RedirectUrl = "CustomErrorPage.aspx";
	}
```

## **Deploying an Event Receiver**

You can choose to deploy an event receiver manually, by installing the event receiver assembly in the global assembly cache, and then you can use Windows PowerShell to associate, or bind, the event receiver with an event source. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216560315-9823debc-a32a-4680-aefc-a049403ecb40.png">
</p>

However, if you deploy manually, you must deploy the assembly to every server, and the process includes inherent risks (for example, a typographical error). 

Alternatively, you can use a solution and a Feature to deploy an event receiver. When you use a Feature to deploy an event receiver, you can choose one of two options; you can either bind the event receiver with an event source declaratively, or bind the event receiver with an event source by using code.

Whichever approach you choose, you should package the assembly containing the compiled event receiver in the solution.

Regardless of your approach, you will need to provide the same information:

- **Assembly**. You must use the strong name for your handler assembly.

- **Class**. You must use the fully-qualified type name for your event receiver class.

- **Type**. You must specify a value from the **SPEventReceiverType** enumeration that identifies the event your event receiver should handle.

- **Url**. As an alternative to specifying an assembly, class, and type, you can specify a Url of a remote event receiver. If you specify a Url, you do not need to specify an assembly, class, or type; in all other circumstances, these are required. 

- **Name**. You can specify a name for your event receiver binding.

- **Synchronization**. You can specify the synchronization for your event receiver. If you omit this property, the default synchronization is used, based on the type of event.

- **SequenceNumber**. You can specify a sequence number for your event receiver. SharePoint uses the sequence number to determine the order to run event receivers. The default value is 10000, and lower numbers are run before high numbers. 

**Using code to deploy an event receiver**

To bind your event receiver to an event host programmatically, you must first obtain a reference to the event host. You then create a new instance of the **SPEventReceiverDefinition** class by adding a new item to the event host's **EventReceivers** collection property. You then set the properties of the **SPEventReceiverDefintion** object, before finally calling the Update method to persist the changes. 

The following code example shows how to bind an event receiver to an event host programmatically.

```C#
	using (SPSite site = new SPSite("http://portal.contoso.com"))
	{
	    using (SPWeb web = site.OpenWeb())
	    {
	        // Obtain a reference to the event host.
	        SPList list = web.Lists["Suppliers"];
	        // Create a new instance of the SPEventReceiverDefinition class by using the Add method of the 
	        // EventReceivers collection of the event host. 
	        SPEventReceiverDefinition eventRec = list.EventReceivers.Add();
	        // Optionally, specify a name for the event receiver binding.
	        eventRec.Name = "Event Receiver For ItemAdded";
	        // Specify the strong name for the assembly that contains the complied event receiver.
	        eventRec.Assembly = 
	          "ReceiverAssembly, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=24a5ef6a3fe2c28c";
	        // Specify the fully qualified type name for your event receiver class.
	        eventRec.Class = "ReceiverNamespace.ReceiverClass";
	        // Specify the event that should be handled.
	        eventRec.Type = SPEventReceiverType.ItemAdded;
	        // Specify whether the event should run synchronously or asynchronously. 
	        eventRec.Synchronization = SPEventReceiverSynchronization.Synchronous;
	        // Optionally, specify a sequence value for the event receiver.
	        eventRec.SequenceNumber = 50;
	        // Call the Update method to persist the changes.
	        eventRec.Update();
	    }
	}
```

**Using declarative markup to deploy an event receiver**

You can use declarative markup to associate an event receiver with an event source. To use declarative markup, you add a **Receivers element** to the **Elements** element in an element manifest file. 

When you use declarative deployment, SharePoint will bind the event receiver to the event host based on the Feature activation scope, for example by binding it to a site if the Feature has the Web scope. If you are binding to a list, you can identify the list by using the attributes of the Receivers element. 

The following code example shows how to bind an event receiver to an event host declaratively.

```XML
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	    <!-- Add one Receivers element per event host. -->
	    <!-- Set the attributes on the Receivers element to identify the event host. -->
	    <Receivers ListUrl="http://portal.contoso.com/lists/suppliers">
	        <!-- Add one Receiver element per event you want to handle on this event host. -->
	        <Receiver>
	            <!-- Optionally, specify a name for the event receiver binding. -->
	            <Name>Event Receiver For ItemAdded</Name>
	            <!-- Specify the  strong name for the assembly that contains the complied event receiver. -->
	            <Assembly> 
	                ReceiverAssembly, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=24a5ef6a3fe2c28c
	            </Assembly>
	            <!-- Specify the fully qualified type name for your event receiver class. -->
	            <Class>ReceiverNamespace.ReceiverClass</Class>
	            <!-- Specify the event that should be handled. -->
	            <Type>ItemAdded</Type>
	            <!-- Specify whether the event should run synchronously or asynchronously. -->
	            <Synchronization>Synchronous</Synchronization>
	            <!-- Optionally, specify a sequence value for the event receiver. -->
	            <SequenceNumber>50</SequenceNumber>
	        </Receiver>
	    </Receivers>
	</Elements>
```

## **Introduction to Timer Jobs**

Some solutions require you to perform processes, which are either too time consuming or too computationally expensive to run each time a page is requested. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216572438-e14ede1c-573b-4bd9-81d1-3135bddd99f0.png">
</p>

SharePoint enables you to develop **timer jobs** that run in a **separate process**, and run on a **scheduled basis**. Timer jobs enable you to develop code that runs either once, or regularly, by using a schedule.

When you define a **schedule**, you can define a time window for the timer job. If you use a **time window**, you can reduce the instantaneous load on your server farm because SharePoint can run the timer job at different times within the time window on different servers in the farm. This enables some servers to remain responsive while others are running the timer job. 

Timer jobs do not run in the IIS worker process; instead, they run in the **OWSTIMER.exe** process. This means they are not subject to the same timeouts that may apply to web requests in the IIS worker process, and can perform long running calculations and processes. 

Also, by default, the **OWSTIMER.exe** process runs under the farm service account. This provides access for code running in a timer job to both the content and configuration databases. 

Furthermore, whereas by default, the ASP .NET code runs by using an impersonated identity of the current user, which is likely to further restrict your code's access to SharePoint and the server, the timer job uses the process identity to run code, which provides you with the most complete access to SharePoint and the server. 

## **Developing a Timer Job**

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216573311-ee672843-1eaf-4c26-9870-8fa3ca750731.png">
</p>

To develop a timer job definition, complete the following steps:

1. Create a new class that inherits from the **SPJobDefinition** class (in the `Microsoft.SharePoint.Administration` namespace). 

2. Add a default constructor to the class. 

3. Add a non-default constructor that accepts four parameters:
   - A string representing the **name** of the timer job. 
   - A **SPWebApplication** representing the web application that owns the timer job.
   - A **SPServer** representing the server with which this job is associated.
   - A **SPLockType** identifying the lock type that should be used for this job (this enables you to specify whether the job can be run on more than one server at a time, and whether this needs to run for each content database, or just once.

4. Modify the non-default constructor to call the equivalent constructor on the base class.

5. Override the Execute method. You should add your custom logic to this method. 

The following code example shows how to develop a custom timer job. 
```C#
	// Define a class that inherits from the SPJobDefinition class. 
	public class ContosoTimerJob : SPJobDefinition
	{
	    // Define a default constructor.
	    public ContosoTimerJob()
	    {
	    }
	    // Define a non-default constructor
	    public ContosoTimerJob(string name, SPWebApplication webApplication, 
	        SPServer server, SPJobLockType lockType)
	        : base(name, webApplication, server, lockType) // Call the constructor of the base class.
	    {
	    }
	    public override void Execute(Guid targetInstanceId)
	    {
	        // Add custom logic here.
	    }
    }
```

## **Deploying a Timer Job**

To deploy a timer job, you must use code. The most common technique to deploy a timer job is to use a **Feature** with a **Feature receiver**. You should use the **FeatureActivated** method to install your timer job, and the **FeatureDeactivated** method to uninstall your timer job. 

You must provide a name for your timer job. You can define a string constant in your Feature receiver class to store the timer job name. You should always check to ensure that a timer job with the same name does not already exist before you install your timer job. If a timer job with the same name already exists, you should uninstall that timer job first.

To uninstall a timer job, you complete the following steps:

1. Obtain a reference to the web application with which the timer job is associated. You can obtain this by using the properties parameter passed to the **FeatureDeactivated** method. 

2. Iterate over the **JobDefinitions** collection property exposed by the web application object. For each job in the collection, check whether the **Name** property matches your timer job's name. If it does, call the Delete method on the job.

When you deploy a schedule, you must create a schedule for the job. The **schedule** determines how often the job should run, and when the job should run. 

You create a schedule by creating an instance of one of the following classes:

- SPMinuteSchedule
- SPHourlySchedule
- SPDailySchedule
- SPWeeklySchedule
- SPMonthlySchedule
- SPYearlySchedule

When you create an instance of one of these classes, you define a **begin time**, an **end time**, and an **interval**. The begin time specifies the earliest that the timer job can start running. The end time specifies the latest that the timer job can start running. 

For example, with a **SPMinuteSchedule**, you can specify the BeginSecond and EndSecond properties. If you specify a BeginSecond of 1 and an EndSecond of 5, the timer job will start running between 16:00:01, and 16:00:05 (assuming that the job was due to run at some point during the minute at 16:00). 

A timer job can continue to run after the end time is reached; this only specifies the time period during which the job may start, not when the job must finish. 

The Interval property specifies how often the job should run. In the **SPMinuteSchedule** example, an Interval of 5 would indicate that the timer job should run every **5 minutes**. An advantage of specifying a time window instead of an explicit start time is that SharePoint can run the timer job at different times, within the time window, on different servers in your server farm. 
This helps to ensure that your farm remains responsive when processor-intensive timer jobs are running. 

To install a timer job, you complete the following steps:

1. Obtain a reference to the web application with which the timer job should be associated. You can obtain this by using the properties parameter passed to the FeatureActivated method. 

2. Create a new instance of the timer job class, specifying the constant name, and the web application as parameters for the constructor. 

3. Create a schedule for the timer job.

4. Set the Schedule property of the timer job to the schedule object you created in the previous step. 

5. Call the job's Update method to persist the changes. 

The following code example shows a Feature receiver to install a timer job.

```C#
	public class TimerJobInstaller : SPFeatureReceiver
	{
	    // Define a constant with the job name.
	    const string JobName = "ContosoTimerJob";
	    // Create a method to remove the job if it is installed.
	    private void RemoveJob(SPWebApplication webApplication)
	    {
	        foreach (var job in webApplication.JobDefinitions)
	        {
	            if (job.Name.Equals(JobName))
	            {
	                job.Delete();
	            }
	        }
	    }
	    // Override the FeatureActivated method to install the job. 
	    public override void FeatureActivated(SPFeatureReceiverProperties properties)
	    {
	        // Obtain a reference to the web application.
	        SPWebApplication webApplication = ((SPSite)properties.Feature.Parent).WebApplication;
	        // Remove the timer job if it is already installed.
	        RemoveJob(webApplication);
	        // Create a new instance of the timer job.
	        ContosoTimerJob timerJob = new ContosoTimerJob(JobName, webApplication);
	        // Create a schedule for the job.
	        SPMinuteSchedule schedule = new SPMinuteSchedule();
	        schedule.BeginSecond = 0;
	        schedule.EndSecond = 5;
	        schedule.Interval = 10;
	        // Assign the schedule to the job.
	        timerJob.Schedule = schedule;
	        // Call the Update method to persist the changes.
	        timerJob.Update();
	    }
	    // Override the FeatureDeactivated method to uninstall the job. 
	    public override void FeatureDeactivated(SPFeatureReceiverProperties properties)
	    {
	        // Obtain a reference to the web application.
	        SPWebApplication webApplication = ((SPSite)properties.Feature.Parent).WebApplication;
	        // Remove the timer job.
	        RemoveJob(webApplication);
	    }
    }
```

## **Work Item Timer Jobs**

**Work item timer jobs** are a special type of timer job specifically designed to **process a set of items**. A work item timer job works by processing items in a queue. 

A work item timer job, like other timer jobs, works on a scheduled basis; however, when a work item timer job is scheduled to run, SharePoint will only execute the timer job logic if at least one work item has been added to the queue for that timer job. 

This provides a two-part split for work item timer jobs:

- The work item timer job definition, which contains the logic for processing items. 
- A queue of items to be processed.

You must develop the work item timer job definition, but you do not need to develop the queue; SharePoint provides a queue, persisted in the database, that you use. 

You must write code to add items to the queue. Your code to add items to the queue must run in the context of a site collection administrator, or use elevated privileges; for this reason, you cannot add items to a work item timer job queue by using a sandboxed solution. 

Work item timer jobs are particularly beneficial if you need to process items across multiple lists, or need to only process particular items from a list, such as items that have changed or been added to the list. 

Alternatively, you could use an event receiver to process list items that have changes or been added, but work item timer jobs provide advantages over event receivers:

- They run in the **OWSTIMER.exe** process, which grants them more security permission than an event receiver that may run in the context of the current user, or the web application service account. 

- They may run during off peak hours when the impact of processor intensive operations is less significant.

In this example, you might use a work item timer job in conjunction with an event receiver, by using the event receiver to add an added or change item to the work item timer job queue, and then processing the item in the work item timer job according to the work item timer job schedule (or at a later date if you specify a start date in the future when you add the item to the queue).

Finally, work item timer jobs enable you to process either a single item at a time, or to retrieve a set of items that are queued for processing and process them all at the same time. If you choose to implement batch processing, you can set a maximum batch size.

## **Developing a Work Item Timer Job**

To develop a work item timer job, complete the following steps:

1. Create a new class that inherits from the **SPWorkItemJobDefinition** class (in the `Microsoft.SharePoint.Administration` namespace). 

2. Add a default constructor to the class. 

3. Add a non-default constructor that accepts two parameters:
   - A string representing the **name** of the work item timer job. 
   - A **SPWebApplication** representing the web application that owns the timer job.

4. Modify the non-default constructor to call the equivalent constructor on the base class.

5. Override the **WorkItemType** method to return a **Guid** that identifies the work item timer job. This should always return the same value, and you use this when you queue an item to be processed by your work item timer job. 

6. Override the **DisplayName** property to return a display name for your work item timer job. This should always return the same value. 

7. Optionally, override the **BatchFetchLimit** property to return an int identifying the **maximum number of work items** that should be retrieved from the work item queue each time the timer job runs. 

8. Override the **ProcessWorkItem**, or **ProcessWorkItems** method. You should override the ProcessWorkItem method to process a single item at a time or the ProcessWorkItems method if you will process a batch of items. Add your custom logic to this method. After your custom logic, add code to delete the work item from the work item queue to prevent it from being processed more than once.

The following code example shows how to develop a custom work item timer job.

```C#
	public class WorkItemTimerJob : SPWorkItemJobDefinition
	{
	    public WorkItemTimerJob()
	    {
	    }
	    public WorkItemTimerJob(string name, SPWebApplication webApplication
	        : base(name, webApplication)
	    {
	    }
	    public override Guid WorkItemType()
	    {
	        return new Guid("{7B1F0F56-2A2D-4AA3-B962-FCAD66ED02C8}");
	    }
	    public override string DisplayName
	    {
	        get
	        {
	            return "Contoso Work Item Timer Job";
	        }
	    }
	    public override int BatchFetchLimit
	    {
	        get
	        {
	            return 100;
	        }
	    }
	    public override bool ProcessWorkItem(SPContentDatabase contentDatabase, SPWorkItemCollection workItems, SPWorkItem workItem, SPJobState jobState)
	    {
	        // Add code to process work item. 
	        // Delete the work item.
	        workItems.DeleteWorkItem(workItem.Id);
	    }
	}

```

To deploy a work item timer job, you use the same procedure as you use to deploy a normal timer job. 

After you have deployed a work item timer job, you can add items to the work item queue for your timer job to process. You use the **AddWorkItem** method of an **SPSite** instance to add an item to the queue. 

You must use elevated privileges, or be logged in to SharePoint as a site collection administrator to call the AddWorkItem method. When you call the AddWorkItem method, you should provide the **Guid** that you return in the WorkItemType method as the gWorkItemType parameter.
