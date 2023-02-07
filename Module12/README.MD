# Module 12 - Managing Taxonomy

## **Topics**:

1. [Understanding Taxonomy in SharePoint](#understanding-taxonomy-in-sharepoint)
2. [Creating Site Columns Declaratively](#creating-site-columns-declaratively)
3. [Creating Site Columns Programmatically](#creating-site-columns-programmatically)
4. [Retrieving and Editing Site Columns](#retrieving-and-editing-site-columns)

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