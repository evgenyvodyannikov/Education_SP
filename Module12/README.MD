# Module 12 - Managing Taxonomy

## **Topics**:

1. [Understanding Taxonomy in SharePoint](#understanding-taxonomy-in-sharepoint)
2. [Creating Site Columns Declaratively](#creating-site-columns-declaratively)
3. [Creating Site Columns Programmatically](#creating-site-columns-programmatically)
4. [Retrieving and Editing Site Columns](#retrieving-and-editing-site-columns)
5. [Working with Lookup Fields](#working-with-lookup-fields)
6. [Creating Content Types Declaratively](#creating-content-types-declaratively)
7. [Understanding Content Type IDs](#understanding-content-type-ids)
8. [Working with Content Types in Code](#working-with-content-types-in-code)
9. [Adding Content Types to Lists](#adding-content-types-to-lists)
10. [Managing Document Templates](#managing-document-templates)
11. [Configuring Workflow Associations](#configuring-workflow-associations)

## **Understanding Taxonomy in SharePoint**

In an information management system such as SharePoint, a robust taxonomy is essential. It enables content creators to **organize and categorize** their data, and it enables content consumers to locate data quickly through search or through navigation.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/217195344-feeb8918-8af0-4484-8abe-1541e685c866.png">
</p>

In SharePoint, taxonomy is closely tied to metadata, which you can define as data about data. 

For example, in the case of a Microsoft Word document, the data is the contents of the document, whereas the metadata is information about the document such as the author, the date it was created, and so on. Metadata enables users to find data by filtering, searching, or navigating. 

SharePoint provides three core building blocks that solution architects can use to design and implement a taxonomy:

- **Site columns**. To collect additional metadata about list items or files in SharePoint, you add a column to the list or library. Site columns are reusable columns that you can add to lists or libraries within the site.

- **Content types**. A content type defines and classifies a document or a list item according to the requirements of the organization. Content types can specify the site columns, workflows, policies, and document templates that should be associated with a particular item. For example, you might define a content type named Invoice. If a user uploads a document and specifies a content type of Invoice, SharePoint will prompt the user for any required field values, such as the invoice amount, the supplier, and the due date. 

- **Managed metadata term sets**. A term set is a **hierarchical** set of terms that is typically maintained by one or more term set administrators. Term sets provide a way of ensuring that users use a consistent nomenclature when adding metadata. You can create site columns based on a term set, so that users can only add terms from the term set when they populate certain fields. You can also use term sets to drive navigation on publishing sites.

## **Creating Site Columns Declaratively**

A site column is effectively a template for a column in a list or a library. Site columns are useful for two main reasons:

- Reusability. In many cases, you will want to add the same column to multiple lists or libraries. For example, a date-based column named Expiry Date could be useful in many different scenarios. Instead of manually creating the same column in multiple lists and libraries, users can simply add the column from the site column collection.

- Consistency. If you are capturing the same information in multiple lists or libraries, you want that information to be captured in a consistent way. For example, if you use a choice field to represent distinct stages of a production process, you want those choices to be described consistently in different lists and libraries.

**Creating site columns**

To create a site column declaratively, you define a Field element within an element manifest.

The following code example shows an element manifest that defines several site columns of different types:

```XML
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	   <Field ID="{40dca724-d8f9-4983-856b-f8ca48bd6895}"
	               Name="Ingredients"
	               DisplayName="Ingredients"
	               Type="Note"
	               NumLines="5"
	               Required="TRUE"
	               Group="Contoso Columns">
	   </Field>
	   <Field ID="{f02f0407-ed85-463d-bca6-61ba79b5f74e}"
	               Name="LeadChemist"
	               DisplayName="Lead Chemist"
	               Type="User"
	               UserSelectionMode="0"
	               Required="TRUE"
	               Group="Contoso Columns">
	   </Field>
	   <Field ID="{60616688-e1ff-49b6-9ede-c4674e7a59fa}"
	               Name="ExpiryDate"
	               DisplayName="Expiry Date"
	               Type="DateTime"
	               Format="DateOnly"
	               Required="TRUE"
	               Group="Contoso Columns">
	   </Field>
	   <Field ID="{9c2fb13f-62ee-45e6-b73f-ade0489cb69c}"
	               Name="ProductionType"
	               DisplayName="Production Type"
	               Type="Choice"
	               Required="TRUE"
	               Group="Contoso Columns">
	      <CHOICES>
	         <CHOICE>Research</CHOICE>
	         <CHOICE>Prototype</CHOICE>
	         <CHOICE>Trial</CHOICE>
	         <CHOICE>Release</CHOICE>
	      </CHOICES>
	   </Field>
	</Elements>
```

As you can see from the example, the Field element includes attributes that are common to all field types, such as **ID**, **Name**, **DisplayName**, **Type**, **Required**, and **Group**. 

However, a Field element can also include attributes and elements that are specific to certain field types. For example, if you create a choice field, you define the available choices in a CHOICES child element.

## **Creating Site Columns Programmatically**

In some circumstances, you may want to create site columns programmatically, rather than declaratively. 

To some extent, this is a matter of personal preference: some developers prefer writing code to building CAML. The programmatic approach is also useful if you need to add columns dynamically, such as in feature receiver code.

If you are developing an app, the programmatic approach to creating site columns is useful if you want to add a site column to the host web. 

Declarative components such as site columns on content types within an app are deployed to the app web, so writing code is the only option if you want to provision artifacts to an alternative location.

Creating site columns by using server-side code
To create a site column programmatically, you must add a new field to the Fields collection of a SharePoint web.

Depending on your requirements, you can do this in various ways. If you are working with the server-side object model, you can:

- Call the **SPFieldCollection.Add** method, and specify the display name and the field type as arguments. 

- Call the **SPFieldCollection.Add** method, and provide an **SPField** instance as an argument. 

- Call the **SPFieldCollection.AddFieldAsXml** method, and provide a CAML-based Field element as a string argument.

The approach you use will likely depend on the complexity of your field requirements. If you want to create a field that requires little additional configuration, it is easier to call the Add method and specify a display name and a field type. 

If you want to change some of the field properties, it is easier to create an SPField instance before you call the Add method. 

Finally, if you want to customize the field extensively, you may find it easier to define the field in CAML and call the AddFieldAsXml method.

If you create a new **SPField** instance, you must specify the collection to which you want to add the new field as an argument to the constructor. 

Fields typically include two constructors: one to instantiate a field that already exists within the collection, and one to create a field that does not already exist within the collection. The difference is that the constructor that creates a new instance additionally requires you to specify the type name of the new field.

The following code example shows how to add fields in server-side code:

```C#
	var site = SPContext.Current.Site;
	var web = site.RootWeb;
	// Get the SPFieldCollection for the root web.
	var fields = web.Fields;
	// Add a new date field.
	var fieldExpiryDate = new SPFieldDateTime(fields, SPFieldType.DateTime.ToString(), "Expiry Date");
	fieldExpiryDate.StaticName = "ExpiryDate";
	fieldExpiryDate.DisplayFormat = SPDateTimeFieldFormatType.DateOnly;
	fieldExpiryDate.Group = "Contoso Columns";
	fieldExpiryDate.Required = true;
	fieldExpiryDate.Update();
	fields.Add(fieldExpiryDate);
	// Add a new choice field.
	var fieldProductionType = new SPFieldChoice(fields, SPFieldType.Choice.ToString(), "Production Type");
	fieldProductionType.StaticName = "ProductionType";
	fieldProductionType.Choices.Add("Research");
	fieldProductionType.Choices.Add("Prototype");
	fieldProductionType.Choices.Add("Trial");
	fieldProductionType.Choices.Add("Release");
	fieldProductionType.Group = "Contoso Columns";
	fieldProductionType.Required = true;
	fieldProductionType.Update();
	fields.Add(fieldProductionType);
	web.Dispose();
```

Creating site columns by using client-side code
The SharePoint client object models enable you to work with fields in much the same way as the server-side object model. One key difference is that you can only create a new field by using the **AddFieldAsXml** method of the field collection class.

**Note:** The JavaScript object model and the managed client object model both include **FieldCollection.Add** methods. However, these are intended for adding existing fields to a new collection—for example, to add site columns to the fields collection of a list—rather than for creating new fields.

The following code example shows how to add fields by using the JavaScript object model:

```JavaScript
	var context;
	var web;
	var fields;
	var addFields = function () {
	   context = new SP.ClientContext.get_current();
	   web = context.get_web();
	   fields = web.get_fields();

	   var fieldSchema = `
	   <Field Type="DateTime"
	   Name="ExpiryDate"
	   DisplayName="Expiry Date"
	   Format="DateOnly"
	   Required="TRUE"
	   Group="Contoso Columns"/>`;

	   fields.addFieldAsXml(fieldSchema, false, SP.AddFieldOptions.addFieldCheckDisplayName);
	   context.executeQueryAsync(onAddFieldsSuccess, onAddFieldsFail);
	}
	var onAddFieldsSuccess = function () {
	   // Field added successfully. Display a confirmation as required.
	}
	var onAddFieldsFail = function () {
	   // Alert the user that the new field was not created successfully.
	}
```

## **Retrieving and Editing Site Columns**

In some scenarios, you may want to edit site columns programmatically. 

For example, you might want to provide a more informative description, or amend a list of choices, or change the way your columns are grouped. 

You can use the following high-level process to retrieve and edit a site column:

1. Retrieve the column from a field collection and cast it to an appropriate type.

2. Update the properties of the field.

3. Call the Update method on the field to persist your changes to the content database.

The following code example shows how to retrieve and edit a site column in server-side code:

```C#
	var site = SPContext.Current.Site;
	var web = site.RootWeb;
	// Get the SPFieldCollection for the root web.
	var fields = web.Fields;
	// Retrieve the Production Type field and make changes.
	var fieldProductionType = fields["Production Type"] as SPFieldChoice;
	fieldProductionType.Choices.Clear();
	fieldProductionType.Choices.Add("Phase 1 Trial");
	fieldProductionType.Choices.Add("Phase 2 Trial");
	fieldProductionType.Choices.Add("Phase 3 Trial");
	fieldProductionType.Choices.Add("Production");
	// Call the Update method.
	// Specify false to prevent changes from being cascaded to list columns.
	fieldProductionType.Update(false);
```

The process for updating site columns in client-side code is broadly similar to the server-side code process. The major difference is that you also need to manage the client context.

The following code example shows how to retrieve and edit a site column by using the JavaScript object model:

```JavaScript
	var context;
	var web;
	var fields;
	var updateField = function () {
	   context = new SP.ClientContext.get_current();
	   web = context.get_web();
	   fields = web.get_fields();
	   var fieldExpiryDate = context.castTo(fields.getInternalNameOrTitle("ProductionType"), SP.FieldChoice);
	   var choices  = Array("Phase 1 Trial", "Phase 2 Trial", "Phase 3 Trial", "Production")
	   fieldExpiryDate.set_choices(choices);
	   context.ExecuteQueryAsync(onUpdateFieldSuccess, onUpdateFieldFail);
	}
	var onUpdateFieldSuccess = function () {
	   // Field updated successfully. Display a confirmation as required.
	}
	var onUpdateFieldFail = function () {
	   // Alert the user that the new field was not created successfully.
	}
```

## **Working with Lookup Fields**

SharePoint enables you to create relationships between lists through the use of lookup fields. 

A lookup field works in a similar way to a choice field, in that the user can select a value from a list of choices. 

However, in a **lookup field**, the **choices are actually list items from another list** within the same site collection. 

This enables architects and developers to create data models that behave like relationship databases, where the lookup field in a SharePoint list is analogous to the foreign key in a database table. Lookup fields also enable you to create sophisticated queries using join statements in LINQ to SharePoint or CAML.

Suppose you use one SharePoint list to track major programs of work, and another SharePoint list to track individual project statuses. 

Each program contains multiple projects, and each project is associated with a program. You can relate the two lists by adding a Program lookup field to the Projects list. When a user adds a new project to the Projects list, he or she can select the program name in the Program lookup field. 

If you need to query project or program data, you can then use join statements to retrieve related field values from both lists.

The following code example shows how to define a lookup field in CAML:

```XML
	<Field ID="{7fce20b8-9b48-4672-b4c2-011241766c0d}"
	               Name="ProgramsLookup"
	               DisplayName="Programs"
	               Type="Lookup"
	               List="Lists\Programs"
	               ShowField="ProgramName"
	               Overwrite="true"
	               Group="Contoso Columns">
	   </Field>
```

When you define a lookup field in CAML, the Lists attribute indicates which list the lookup field should retrieve data from. 

The **ShowField** attribute indicates which field from the list should be displayed when the lookup field is added to a list. 

In this example, if a user adds the **ProgramsLookup** field to a list, he or she will be able to select from a list of **programs represented by the ProgramName** value. 

You can also include projected fields, which enable you to display additional fields from the related list in the view that includes the lookup field.

SharePoint enables you to enforce list relationships by specifying deletion rules. There are two types of deletion rule:

- Cascade delete rule. When you delete a list item, this rule deletes all list items in related lists that reference the primary list item through a lookup field.

- Restrict delete rule. This rule prevents you from deleting a list item if it is referenced by items in a related list through a lookup field.

## **Creating Content Types Declaratively**

Content types are designed to define and encapsulate all the metadata, business processes, and UI requirements for a particular type of content. 

Content types should classify information by business purpose, rather than by file type. For example, you might create content types to represent invoices, or vacation requests, or expense claims. 

You can use content types to define:

- The metadata (fields) you require to manage the item.

- The document template for the item.

- The Display, Edit, and New forms that are used to manage the item.

**Creating content types**

You can define content types declaratively by using the ContentType element in CAML. The ContentType element enables you to define all the key aspects of the content type properties and behaviors.

The following example shows how to define a content type in CAML:

```XML
	<ContentType 
		ID="0x010100742830D3B25349C7A83DF4AEF639BFD5"
		Name="Contract"
		Group="Contoso Content Types"
		Description="A contract to supply goods or services"
		Inherits="TRUE"
		Version="0">
	   <FieldRefs>
	      <RemoveFieldRef ID="{fa564e0f-0c70-4ab9-b863-0177e6ddd247}" Name="Title" />
	      <FieldRef ID="{ff77d8e3-c172-43e6-b729-d3e6492e6334}" Name="CustomerName" Required="TRUE" />
	      <FieldRef ID="{fd125d1e-ad63-436e-9f1f-efce7ef00967}" Name="StartDate" Required="TRUE" />
	      <FieldRef ID="{b2db5ee2-e09e-4864-b00f-3f652187c843}" Name="FinalEffectiveDate" Required="TRUE" />
	      <FieldRef ID="{8da52b24-493d-4475-a812-875ba2c0faf4}" Name="Owner" Required="TRUE" />
	      <FieldRef ID="{cc4fd24e-f357-45e7-bb4c-a7567aae6347}" Name="Department" Required="TRUE" />
	   </FieldRefs>
	   <DocumentTemplate TargetName="ContosoContract.dotx" />
	</ContentType>
```

In the example, the Inherits attribute indicates whether the content type should inherit fields from its parent. 

Content type inheritance is specified by the content type ID value, which is described in more detail in the next topic. 

After specifying that the content type should inherit fields from its parent, you can use RemoveFieldRef child elements to remove any inherited fields that you do not require.

To add new fields to the content type, you use the **FieldRef** element. You can reference fields that already exist on the target site, or fields that are defined within the same feature as the content type.

**Deploying content types**

Like site columns, content types are deployed in **Web-scoped** or **Site-scoped** Features. 

To make a content type available to every web in a site collection, you should use a site-scoped feature. In this case, the content types are deployed to the root web in the site collection.

You can use both apps and solutions to deploy content types. If you include a declarative content type in an app, the content type is provisioned on the app web.

## **Understanding Content Type IDs**

All content types belong in an inheritance hierarchy, and ultimately inherit from one of the built-in base content types. 

However, the ContentType element does not explicitly specify the parent of the content type. Instead, inheritance is specified through the content type ID.

Every content type ID begins with the ID of a base content type. The following table shows the ID values of some common base content types:

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/217255389-17dfd230-9edc-4de0-b5c0-b5127621b8bf.png">
</p>

You can use the following rules to build a content type ID:

1. Start with the ID of the parent content type.

2. Append 00, which you can think of as a spacer.

3. Append an uppercase GUID, without any hyphens, spaces, or braces.

For example, suppose you want to create a content type named Invoice that inherits from Document. You start with the ID of the Document content type:

`0x0101`

Next, you append a double zero:

`0x010100`

Finally, you append a GUID:

`0x0101005AF7FDFCE5FD4C359A7AE34DFB008661`

When you deploy this content type, SharePoint will deduce the parentage from your content type ID. In this case, your content type will inherit from the Document content type. 

If you have set the Inherits attribute to TRUE, your content type will include all the fields that are present in the Document content type.

Suppose you now want to create a more specialized type of Invoice content type named Northwind Traders Invoice. 

The **Northwind Traders Invoice** content type should inherit from the **Invoice** content type. You can use the same approach to build the new content type ID. You start with the ID of the Invoice content type:

`0x0101005AF7FDFCE5FD4C359A7AE34DFB008661`

Next, you append a double zero:

`0x0101005AF7FDFCE5FD4C359A7AE34DFB00866100`
Finally, you append a GUID:

`0x0101005AF7FDFCE5FD4C359A7AE34DFB00866100BB4707E674E049D48B2BB38FE625C4B2`

You can use this approach to build as many levels of content type inheritance as you require.

## **Working with Content Types in Code**

In addition to creating content types declaratively in CAML, you can create and edit content types programmatically using server-side or client-side code. 

Choosing the declarative approach or the programmatic approach to creating content types is largely a matter of personal preference: some developers prefer the declarative approach for **readability**, whereas other developers consider the programmatic approach **more flexible** and **more robust**. 

In most cases, the programmatic approach is the only option if you need to edit existing content types.

**Creating content types by using server-side code**

To create a new content type using server-side code, you create an object of type **SPContentType**. 

To create a new SPContentType instance, you must provide three pieces of information:

- Either the ID of the **parent content type**, or an **SPContentType** instance that represents the parent content type.

- The **content type collection** to which you want to add the new content type. Both the SPWeb class and the SPList class expose a content type collection through the ContentTypes property.

- The name of the new content type.

In the server-side object model, field references are represented by the **SPFieldLink** class. You must create an SPFieldLink instance for every field that you want to add to the content type. 

You then add the SPFieldLink instances to the FieldLinks collection of the content type.

The following example shows how to create a content type in server-side code:

```C#
	var web = SPContext.Current.Site.RootWeb;
	var fields = web.fields;
	// Get the ID of the parent content type.
	var parentId = SPBuiltInContentTypeId.Document;
	// Create the Invoice content type.
	var ctInvoice = new SPContentType(parentId, web.ContentTypes, "Invoice");
	// Create field links.
	var fldClientName = fields.GetField("ClientName");
	var fldPaymentDueDate = fields.GetField("PaymentDueDate");
	var fldAmount = fields.GetField("Amount");
	var fldLinkClientName = new SPFieldLink(fldClientName);
	var fldLinkPaymentDueDate = new SPFieldLink(fldPaymentDueDate);
	var fldLinkAmount = new SPFieldLink(fldAmount);
	// Add the field links to the content type.
	ctInvoice.FieldLinks.Add(fldLinkClientName);
	ctInvoice.FieldLinks.Add(fldLinkPaymentDueDate);
	ctInvoice.FieldLinks.Add(fldLinkAmount);
	// Persist the changes to the content database.
	ctInvoice.Update();
	// Add the content type to the collection.
	web.ContentTypes.Add(ctInvoice);
	web.Dispose();
```

**Creating content types by using client-side code**

Creating content types in client-side code requires a slightly different approach. To create a content type, you must first create a **ContentTypeCreationInformation** object. Similarly, to create a field link, you must first create a **FieldLinkCreationInformation** object.

The following example shows how to create a content type by using the JavaScript object model:

```JavaScript
	var context;
	var web;
	var fields;
	var contentTypes;
	var addContentType = function () {
	   context = new SP.ClientContext.get_current();
	   web = context.get_web();
	   fields = web.get_availableFields();
	   contentTypes = web.get_contentTypes();
	   // Get a reference to the parent content type.
	   var parent = contentTypes.getById("0x0101");
	   // Create an SP.ContentTypeCreationInformation object.
	   var ctInfo = new SP.ContentTypeCreationInformation();
	   ctInfo.set_parentContentType(parent);
	   ctInfo.set_name("Invoice");
	   ctInfo.set_group("Contoso Content Types");
	   // Create the Invoice content type.
	   var ctInvoice = contentTypes.add(ctInfo);
	   // Add field links to the content type.
	   var fieldLinks = ctInvoice.get_fieldLinks();
	   var fldClientName = fields.getByInternalNameOrTitle("ClientName");
	   var fldLinkInfoClientName = new SP.FieldLinkCreationInformation();
	   fldLinkInfoClientName.set_field(fldClientName);
	   fieldLinks.add(fldLinkInfoClientName);
	   var fldPaymentDueDate = fields. getByInternalNameOrTitle ("PaymentDueDate");
	   var fldLinkInfoPaymentDueDate = new SP.FieldLinkCreationInformation();
	   fldLinkInfoPaymentDueDate.set_field(fldPaymentDueDate);
	   fieldLinks.add(fldLinkInfoPaymentDueDate);
	   var fldAmount = fields. getByInternalNameOrTitle ("Amount");
	   var fldLinkInfoAmount = new SP.FieldLinkCreationInformation();
	   fldLinkInfoAmount.set_field(fldAmount);
	   fieldLinks.add(fldLinkInfoAmount);
	   // Persist the changes to the content type.
	   ctInvoice.update();
	   context.executeQueryAsync(onAddContentTypeSuccess, onAddContentTypeFail);
	}
	var onAddContentTypeSuccess = function () {
	   // Content type added successfully. Display a confirmation as required.
	}
	var onAddContentTypeFail = function () {
	   // Alert the user that the new content type was not created successfully.
	}
```

## **Adding Content Types to Lists**

To use a content type, you must add it to a list or library. Users with sufficient permissions can add content types to lists through the user interface. 

If you need to automate the process, you can use both declarative and programmatic approaches to add content types to lists. The association between a specific content type and a specific list is known as a **content type binding**.

**Creating content type bindings declaratively**

You can add a content type to a list declaratively by adding a ContentTypeBinding element to an element manifest, and then deploying the element manifest within a Web-scoped or Site-scoped Feature. The ContentTypeBinding element simply requires you to specify the ID of the content type and the site-relative URL of the list.

The following code example shows how to define a content type binding declaratively:

```XML
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	   <ContentTypeBinding
	      ContentTypeId="0x0101005AF7FDFCE5FD4C359A7AE34DFB008661"
	      ListUrl="Invoices"
	      RootWebOnly="FALSE"
	   />
	</Elements>
```

**Creating content type bindings programmatically**

To create a content type binding programmatically, you essentially need to retrieve a content type from the site’s collection of content types and add it to the list's collection of content types. You also need to ensure that content types are enabled on the list before you attempt to add to the collection.

The following code example shows how to add a list content type in server-side code:

```C#
	var web = SPContext.Current.Web;
	// Get the list.
	var list = web.GetList("Invoices");
	// Get the site content type.
	var contentType = web.AvailableContentTypes["Invoice"];
	// Enable content types on the list.
	list.ContentTypesEnabled = true;
	// Add the content type to the list.
	list.ContentTypes.Add(contentType);
	// Persist the changes to the database.
	list.Update();
```

You can use a similar pattern to add list content types in client-side code. 

The following code example shows how to add a list content type by using the JavaScript object model:

```JavaScript
	var addListContentType = function () {
	   var context = new SP.ClientContext.get_current();
	   var web = context.get_web();
	   // Get a reference to the site content type.   
	   var contentTypes = web.get_availableContentTypes();
	   var invoiceCT = contentTypes.getById("0x0101005AF7FDFCE5FD4C359A7AE34DFB008661");
	   // Get a reference to the list.
	   var lists = web.get_lists();
	   var list = lists.getByTitle("Invoices");
	   // Enable content types on the list.
	   list.set_contentTypesEnabled(true);
	   // Add the content type to the list.
	   var listContentTypes = list.get_contentTypes();
	   listContentTypes.addExistingContentType(invoiceCT);
	   // Persist the changes to the database.
	   list.update();
	   // Submit the query.
	   context.executeQueryAsync(onAddListContentTypeSuccess, onAddListContentTypeFail);
	}
	var onAddListContentTypeSuccess = function () {
	   // List content type added successfully. Display a confirmation as required.
	}
	var onAddListContentTypeFail = function () {
	   // Alert the user that the list content type was not added successfully.
	}
```

## **Managing Document Templates**

In many scenarios, organizations may find it useful to associate document templates with content types. 

For example, if you are creating an invoice, you would be very unlikely to start with a blank document. Instead, you are likely to use an invoice template with formatting and boilerplate text, together with placeholders for amounts, dates, and descriptions. If you associate a template with the Invoice content type, SharePoint will launch the template every time a user creates a new Invoice item from a document library menu. 

In the previous lesson, you saw how you can add a DocumentTemplate child element to a declarative content type definition. Although the declarative approach is useful when you are creating a new content type, it does not allow you to change the document template associated with an existing content type. Fortunately, document templates are easy to manage using either server-side or client-side code.

The following code example shows how to set or update the document template for a site content type in server-side code:

```C#
	var web = SPContext.Current.Web;
	var invoiceCT = web.AvailableContentTypes["Invoice"];
	invoiceCT.DocumentTemplate = "SiteAssets/Invoice.dotx";
	invoiceCT.Update(true);
```

In this example, note that we pass an argument value of **true to the Update** method. This specifies that the updates should be cascaded to children of the content type. In practice, this means that **any instances of the content type** in lists are also **updated** to use the new document template.

You can use the same approach to manage document templates with client-side code. The following code example shows how to set or update the document template for a site content type by using the JavaScript object model:

```JavaScript
	var updateDocumentTemplate = function () {

	   var context = new SP.ClientContext.get_current();
	   var web = context.get_web();
	   var contentTypes = web.get_availableContentTypes();
	   var invoiceCT = contentTypes.getById("0x0101005AF7FDFCE5FD4C359A7AE34DFB008661");
	   invoiceCT.set_documentTemplate("SiteAssets/Invoice.dotx");
	   invoiceCT.update(true);
	   context.executeQueryAsync(onUpdateTemplateSuccess, onUpdateTemplateFail);

	}
	var onUpdateTemplateSuccess = function () {
	   // List content type added successfully. Display a confirmation as required.
	}
	var onUpdateTemplateFail = function () {
	   // Alert the user that the list content type was not added successfully.
	}
```

## **Configuring Workflow Associations**

Content types are a tool for modeling business content. Workflows are a tool for modeling business processes. 

As such, it makes sense to be able to associate workflows with content types. Rather than adding workflows to individual lists, **adding a workflow to the content type** ensures that the workflow is available and active wherever the content type is added.

You can associate workflows with content types by using the SharePoint object model (there is no declarative approach). 

Both the workflow template and the content type must be deployed to the SharePoint site before you create the workflow association. 

The high-level process for associating a workflow with a content type is as follows:

1. Retrieve the workflow template from the SharePoint web.

2. Create a new workflow association object, by specifying the workflow template, providing a name for the workflow instance, and specifying the task list and the history list that you want to use.

3. Retrieve the content type from the SharePoint web.

4. Add the workflow association to the content type.

The following code example shows how to associate a workflow with a content type by using the server-side object model:

```C#
	var web = SPContext.Current.Web;
	// Get the workflow template.
	SPWorkflowTemplate template = web.WorkflowTemplates.GetTemplateByName("InvoiceWorkflow", 
	   web.Locale);
	// Create a new workflow association.
	SPWorkflowAssociation association = SPWorkflowAssociation.CreateWebContentTypeAssociation(template, "Process Invoice", "Invoice Tasks", "Process Invoice History");
	// Get the site content type.
	var contentType = web.AvailableContentTypes["Invoice"];
	// Add the workflow association to the content type.
	if(contentType.WorkflowAssociations.GetAssocationByName("Process Invoice", web.Locale) == null)
	{
	   contentType.WorkflowAssociations.Add(association);
	   contentType.UpdateWorkflowAssociationsOnChildren(false, true, true, false);
	}
```

**Note:** The **SPWorkflowAssociation.CreateWebContentTypeAssociation** method requires you to specify the name of a tasks list and a workflow history list. If these lists do not already exist, SharePoint will create them when the workflow instance is used for the first time.

Associating a workflow with a content type in client-side code is slightly different. Rather than adding a workflow association to the content type, you construct an object of type **WorkflowAssociationCreationInformation** and pass this object to the add method of the workflow association collection.

The following code example shows how to associate a workflow with a content type by using the JavaScript object model:

```JavaScript
	var addWorkflowAssociation = function () {
	   var context = new SP.ClientContext.get_current();
	   var web = context.get_web();
	   // Get a reference to the workflow template.
	   var templates = web.get_workflowTemplates();
	   var template = templates.getByName("InvoiceWorkflow");
	   // Create a WorkflowAssociationCreationInformation object.
	   var info = new SP.Workflow.WorkflowAssociationCreationInformation();
	   info.set_template(template);
	   info.set_name("Process Invoice");
	   info.set_contentTypeAssociationTaskList("Invoice Tasks");
	   info.set_contentTypeAssociationHistoryList("Invoice History");   
	   // Get a reference to the site content type.   
	   var contentTypes = web.get_availableContentTypes();
	   var invoiceCT = contentTypes.getById("0x0101005AF7FDFCE5FD4C359A7AE34DFB008661");
	   // Add the association to the site content type.
	   var associations = invoiceCT.get_workflowAssociations();
	   associations.add(info);
	   invoiceCT.update(true);
	   // Submit the query.
	   context.executeQueryAsync(onAddAssociationSuccess, onAddAssociationFail);
	}
	var onAddAssociationSuccess = function () {
	   // List content type added successfully. Display a confirmation as required.
	}
	var onAddAssociationFail = function () {
	   // Alert the user that the list content type was not added successfully.
	}
```