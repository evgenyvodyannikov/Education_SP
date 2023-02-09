# Module 12 - Managing Custom Components and Site Life Cycles

## **Topics**:

1. [Introduction to List Definitions](#introduction-to-list-definitions)
2. [Developing List Definitions](#developing-list-definitions)
3. [The Visual Studio List Designer](#the-visual-studio-list-designer)

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
