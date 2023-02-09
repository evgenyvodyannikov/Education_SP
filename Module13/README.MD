# Module 12 - Managing Custom Components and Site Life Cycles

## **Topics**:

1. [Introduction to List Definitions](#introduction-to-list-definitions)
2. [Developing List Definitions](#developing-list-definitions)

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