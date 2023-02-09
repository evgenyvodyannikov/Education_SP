# Module 12 - Managing Custom Components and Site Life Cycles

## **Topics**:

1. [Introduction to List Definitions](#introduction-to-list-definitions)
2. [Developing List Definitions](#developing-list-definitions)
3. [The Visual Studio List Designer](#the-visual-studio-list-designer)
4. [Provisioning Lists](#provisioning-lists)
5. [Introduction to Site Definitions and Web Templates](#introduction-to-site-definitions-and-web-templates)

## **Introduction to List Definitions**

SharePoint contains several list definitions you can use in your SharePoint solutions, but you will often want to develop a custom list, with fields that meet your specific requirements. Although you can add a custom list on an ad-hoc basis by using the SharePoint interface, you should usually create a SharePoint list by using a template. 

A list definition can contain content types, fields, views, forms, toolbars, and a default description. 

You create a list definition by using declarative markup and a Feature, although you may include other files; for example, you can include custom forms as part of the list definition. After you define and deploy a list definition, you can create an instance of that list either by using a Feature or by using the SharePoint object model. 

## **Developing List Definitions**

To develop a new list definition, you must create a new Feature. To create a simple list definition, your Feature must contain the following items:

- A **Feature.xml** file. The Feature.xml file must be in the root of the Feature folder; it should contain the Feature definition and details of the files, including the folder structure, that are used by the Feature. 

- An **element manifest** file. The element manifest file can have any name, providing it has an ".xml" file name extension. You must include the element manifest file in the Feature.xml file by using an ElementManifest element. The element manifest file should contain a ListTemplate element. This element defines the properties of the list definition—for example, the name, display name, and ID value—but does not contain any details of the forms, field, views, and so on that are included in the list definition.

- A subfolder that contains a file named **Schema.xml**. The name of the subfolder is determined by the value of the Name attribute of the ListTemplate element in your element manifest file. If the name of the subfolder and the value of the Name attribute do not match, your list definition will not work correctly. The Schema.xml file should contain Collaborative Application Markup Language (CAML), with a root List element, with child elements to add fields, views, content types, and so on to the list definition. The Schema.xml file should be included in the Feature.xml file by using an ElementFile element, rather than an ElementManifest element.

The following code example shows an example of an element manifest file for a list definition. 

```XML
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	    <ListTemplate
	        Name="AssistantList {Name for the list definition; the name of the sub-folder for the schema.xml file}"
	        Type="10001 {Unique identifier for the list definition, use values above 10000 to avoid clashes}"
	        BaseType="0 {Identifier of the list on which this list is based, 0 for generic list}"
	        OnQuickLaunch="TRUE {Specifies whether list instances should appear on the quick launch by default}"
	        DisplayName="Assistants List {Display name for the list definition, displayed when users create new lists}"
	        Description="List to store details of assistants {Description for the list definition}"
	</Elements>
```

The following code example shows the key elements of a list schema in a Schema.xml file. 

```XML
	<List xmlns="http://schemas.microsoft.com/sharepoint" … >
	    <MetaData>
	        <ContentTypes>
	            <ContentType ID="…" Name="…"> … </ContentType>
	            <ContentTypeRef ID="…" … />
	        </ContentTypes>
	        <Fields>
	            <Field ID="…" Name="…" … />
	            <FieldRef ID="…" Name="…" />
	        </Fields>
	        <Views>
	            <View … > … </View>            
	        </Views>
	        <Forms>
	            <Form Type="DisplayForm" Url="DispForm.aspx" … />
	        </Forms>
	    </Metadata>
	</List>
```

## **The Visual Studio List Designer**

Although you can develop a list definition manually, it is usually significantly easier and faster to use the list designer in Visual Studio. 

The list designer enables you to select items from lists instead of writing XML code. This is usually significantly easier because you do not need to look up the GUIDs of content types or columns that you want to include in your list. 

It is also usually faster because most developers can select options from a list faster than write XML code. Because the XML code is automatically generated, the risk of an error is also reduced. 

To use the Visual Studio list designer, you must add a list to a Visual Studio SharePoint solution. Visual Studio will ask you what type of list you want to create. You can choose to add a list instance based on an existing list definition. 

In this case, SharePoint will simply add the files necessary to deploy a list instance. You can choose to create a customizable list definition and list instance. In this case, Visual Studio will also add all of the necessary files to define a list, including the manifest file, and schema file. In addition, regardless of your selection, if your solution does not already contain a Feature, Visual Studio will add a Feature to deploy the list instance (and template if appropriate). 

After you add a list definition to your solution, you can use the list designer to:

- Add columns to the list definition.

- Manage the content types associated with the list definition (you can select either content types that you have added to the same Visual Studio solution or content types that are already deployed to the development SharePoint site, you can).

- Define views for the list definition, by simply selecting from the list of available columns to include in each view. 

- Specify the default list tile, URL, and description, as well as whether the list should be visible on the Quick Launch menu, and whether the list definition should be visible in the browser. 

Visual Studio will automatically update the Schema.xml, manifest file, and Feature with the changes you make. This significantly reduces development time and means that you only need to edit the XML files if you need to make more advanced customizations, for example to configure custom forms.

## **Provisioning Lists**

In addition to deploying lists by using the SharePoint interface, you can also deploy list instances by using either a **Feature** or the **SharePoint object model**. 

If you choose to add a new list instance by using a Feature, you must create an Elements.xml file that contains a ListInstance element. You should provide a title, description, URL, and template type. 

The value for the TemplateType attribute should be the value of the Type attribute of the ListTemplate element in the list definition deployment Feature. 

Standard list definitions provided out-of-the-box with SharePoint are installed by using Features. You can find the ID of built-in lists by viewing the XML relevant Manifest.xml file. If you use Visual Studio to create a list instance Feature, you can select from a list of installed list definitions (including custom list definitions) that will populate the TemplateType field automatically. 

The following code example shows how to create a list instance by using a Feature.

```XML
	<Elements xmlns="“"http://schemas.microsoft.com/sharepoint/"”">
	    <ListInstance Title="Products" TemplateType="10001"
	        Description="A list to store product information."
	        Url="Lists/Products">
	    </ListInstance>
	</Elements>
```

Alternatively, you can add a new list by writing code. To add a new list, you use the Add method on the Lists collection property of the **SPWeb** class. When you write code to add a list, for custom list definition, use an instance of the **SPListTemplate** class to identify the list definition, or for out-of-the-box template, you can use the **SPListTemplateType** enumeration to select a list definition. 

The following code example shows how to create a list instance by using code.

```C#
	using(SPWeb web = site.RootWeb)
	{
	    // Obtain a reference to the list definition by using the ListTemplates collection, and by specifying the 
	    // template name.
	    SPListTemplate template = web.ListsTemplates[“ProductListTemplate”];
	    // Create a new instance of the list definition.
	    Guid productsList = web.Lists.Add(“Products”, “A list to store product information.”, template);
	    // Create a new instance of the Announcements list definition by using the SPListTemplateType 
	    // enumeration to identify the list definition.
	    Guid announcementsList = web.Lists.Add(“Announcements”,  “A list to store announcements.”, 
	        SPListTemplateType.Announcements);
	    // Call the Update method on the SPWeb instance.
	    web.Update();
	}
```

## **Introduction to Site Definitions and Web Templates**

Site definitions provide the most powerful mechanism for developing custom sites. With a custom site definition, you can customize the pages, lists, and Features included in a site, and then deploy multiple copies within your organization. In addition, you can include content in a site definition. 

Site definitions do not have any dependencies on other site definitions and define the entire contents of a site at the point of creation. 

You can fully customize a site definition; however, you must either create a site definition by using declarative markup, or by using code. This can make developing a site definition too time consuming for some projects. 

Another issue with site definitions is that you must deploy them to the file system on the web server. This means that they cannot be used in sandboxed solutions, or with SharePoint Online; however, they are cached by the IIS worker process, which results in better performance compared to alternatives. 

Web templates provide an alternative to a site definition, and are simpler to develop. You create a web template by saving an existing site as a template. You can save a site as a web template programmatically, or by using the SharePoint UI. 

A significant difference when using a web template is that a web template does not define everything that is required to recreate a site; instead, it stores only differences between a base site definition and the site when it was saved. To create a web template, you can use the **SaveAsTemplate** method of the **SPWeb** class. 

If you create a site by using a web template, you must have the corresponding site definition installed in your SharePoint farm. If the original site definition is missing, you cannot use the web template. Another difference is how web templates are stored and cached. 

Site definitions are stored on the file system, and are cached by the IIS worker process; whereas, web templates are stored in the content database. When you use a web template, it must be retrieved from the database. This is less efficient than a site definition. 
