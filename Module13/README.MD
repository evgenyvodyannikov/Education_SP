# Module 13 - Managing Custom Components and Site Life Cycles

## **Topics**:

1. [Introduction to List Definitions](#introduction-to-list-definitions)
2. [Developing List Definitions](#developing-list-definitions)
3. [The Visual Studio List Designer](#the-visual-studio-list-designer)
4. [Provisioning Lists](#provisioning-lists)
5. [Introduction to Site Definitions and Web Templates](#introduction-to-site-definitions-and-web-templates)
6. [Developing a Site Definition Declaratively](#developing-a-site-definition-declaratively)
7. [Deploying Site Definitions](#deploying-site-definitions)
8. [Using Code to Provision a Custom Site](#using-code-to-provision-a-custom-site)
9. [Feature Stapling](#feature-stapling)
10. [Understanding the Site Hierarchy](#understanding-the-site-hierarchy)

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

## **Developing a Site Definition Declaratively**

A declarative site definition contains two main components, a **Webtemp*.xml** file and a **template folder** that contains an **Onet.xml** file. Although you may (and usually will) include other files with your site definition, all declarative site definitions must contain these files. 

**Webtemp*.xml file**

SharePoint uses **Webtemp*.xml** files to identify site definitions that are available on the farm. You can include multiple site definitions in a Webtemp*.xml file, and each site definition can include multiple configurations. Webtemp*.xml files are stored in the 15\TEMPLATE\LCID\XML folder (where LCID is the locale ID; for example, 1033 for U.S. English). 

Every Webtemp*.xml file must have a unique name, and you should neither overwrite nor edit any of the out-of-the-box Webtemp*.xml files. You can add any value in place of the * in the file name, but the file name must start with webtemp; for example, webtemp123.xml and webtemp_ab.xml are both valid file names. There is no link between the name of the Webtemp*.xml file and the corresponding site definition. 

The Webtemp*.xml file contains a root **Templates element**, which contains a set of Template elements. You should add one Template element per site definition. The Template element has a Name attribute and an ID attribute. Both of these attributes must be unique in your farm. 

The ID should be an integer; you should use a number above 10000 to avoid values that are used by SharePoint Foundation site definitions. 

The value of the Name attribute must be a string value, and you must use this as the name of the folder that contains the Onet.xml file. 

A Template element should contain one or more **Configuration elements**. You use the properties of the configuration element to determine how the site definition appears in the user interface. When you create an instance of a site definition, you must make reference to both the Name of the template (specified at the Template element level) and the ID of the configuration (specified at the Configuration element level). The ID of the configuration is an integer, and each Configuration element must have a unique configuration value; however, these do not need to be unique across the template; usually, your first configuration will have the ID 0. 

The following code example shows a Webtemp*.xml file. 

```XML
	<Templates>
	    <Template Name="ContosoSiteDefinition" ID="12000">
	        <Configuration ID="0"
	            Title="Contoso Support Team Site {Site definition title used in the SharePoint interface}"
	            Hidden="FALSE {Used to determine whether to display the definition in the interface}"
	            ImageUrl="{Server relative URL for a thumbnail image which represents the site definition}"
	            Description="Support team site definition description {Description of the definition for the interface}"
	            DisplayCategory="Contoso Sites {Category in which this site definition should be included}"
	    </Template>
	</Template>
```

**Template folder and Onet.xml file**

Although the Webtemp*.xml file is important for SharePoint to identify available site definitions, it does not contain any detail of what each site should include. 

You must create a folder in the 15\TEMPLATES\SiteTemplates folder to store the files used by your site definition. You must name the folder the same as the value of the Name attribute of the Template element. You should add any files you need for your site definition to this folder; for example, you should include any ".aspx" webpages and any images to this folder. 

You can create subfolders and organize your file as appropriate.
The folder must contain a subfolder named XML, which must contain an XML file named Onet.xml. This file defines the Features, lists, pages, and so on, that are included in each configuration of your site definition. Some settings defined in the site definition are common across all configurations of the site definition, whereas others are unique to individual configurations. 

By creating multiple configurations of a site definition, you can create site definitions that are easier to maintain than developing a new site definition for each configuration (for example, if you need to specify an email footer, you only need to specify this once rather than multiple times), but with multiple site definitions, you have more control over each site definition. 

You define configurations by including Configuration elements in the Onet.xml file. The ID of each of these configurations must match the ID of a configuration specified in the Webtemp*.xml file. 

The following code example shows the main components of an Onet.xml file.

```XML
	<Project xmlns="http://schemas.microsoft.com/sharepoint/"  Title="ContosoSiteDefinition">
	    <NavBars>
	        <!-- Add NavBar and NavBarLink elements here to add navigation bars and links to your site. -->
	    </NavBars>
	    <Configurations>
	        <!-- Add a Configuration element for each configuration defined in the webtemp*.xml file here. -->
	        <Configuration ID="0" Title="SalesSiteDefintion">
	            <Lists> 
	                <!-- Add lists to include in this configuration here. -->
	            </Lists>
	            <SiteFeatures> 
	                <!-- Add site collection Features to include in this configuration here. -->
	            </SiteFeatures>
	            <WebFeatures> 
	                <!-- Add site Features to include in this configuration here. -->
	            </WebFeatures>
	            <Modules> 
	                <!-- Add modules to include in this configuration here. -->
	            </Modules>
	        </Configuration>
	    </Configurations>
	    <Modules>
	        <!-- Add Module elements here, for example to include pages or files in your site definition. --> 
	    </Modules>
	    <Components>
	        <!-- Add custom components such as an external security provider here. -->
	    </Components>
	    <ServerEmailFooter>
	        <!-- Specify the server email footer here. -->
	    </ServerEmailFooter>
	</Project>
```

**Assemblies**

In addition to files in the SiteTemplates folder, and the Webtemp*.xml file, you may need to include assemblies that include compiled code required by your site definition. If you need to include any compiled code, you should deploy the assemblies to the global assembly cache. A common reason to include an assembly with a site definition is to include a class to customize the provisioning process for a site definition.

## **Deploying Site Definitions**

You must deploy site definitions to the file system on your SharePoint servers. You must deploy the files on every server; you should use a SharePoint farm solution to make sure your site definition is deployed to every server correctly. 

A site definition should be considered in three parts for deployment: the Webtemp*.xml file, any assemblies, and then everything else (which will include the Onet.xml file). The following list describes how you should deploy each of these three parts:

- Webtemp*.xml file. You must add the Webtemp*.xml file to the 15\TEMPLATE\LCID\XML folder. You must replace LCID with the locale ID for your locale. For U.S. English, this is 1033. You should add the files as an additional file, and never alter or overwrite any of the standard Webtemp*.xml files. You can add any text after webtemp to make your file name unique. 

- Assemblies. You should deploy your signed compiled assemblies to the global assembly cache.

- Everything else. You should add your other files to a folder in the 15\TEMPLATES\SiteTemplates folder. The name of the folder must match the value of the Name attribute of the Template element in the Webtemp*.xml file. You can include any files needed (such as ".aspx" page files) in this folder. You should add your onet.xml file in an XML subfolder. 

**Note:** When you add a site definition to a solution in Visual Studio, Visual Studio adds a Feature to the project—you do not need to include this Feature if you are only developing a site definition, so you can remove this from the project. Visual Studio also compiles an assembly whenever you build the project. If you have not added any code files to your solution (and so the assembly is empty), you can prevent Visual Studio from including the assembly in the SharePoint solution by changing the Include Assembly in Package project property. 

## **Using Code to Provision a Custom Site**

In addition to deploying a custom site definition, you can also deploy code to provision new sites. You can deploy a site by using code in addition to declarative markup, or you can write code to provision a site.

To use code to provision a custom site, you must create a class that inherits from the **SPWebProvisioningProvider** class. You must then override the Provision method with your custom logic to provision the new site. 

The Provision method accepts a **SPWebProvisioningProperties** parameter. You can use the properties of this parameter to obtain a reference to the new **SPWeb** object. When you use code to provision a site, the site does not have any initial state. 

You can use the **ApplyWebTemplate** method of the **SPWeb** object to apply another site template as an initial state, for example the blank site template (STS#0). In addition, you can use the Data property of the **SPWebProvisioningProperties** parameter to retrieve a string value that is defined in the site definition configuration (Webtemp*.xml) file. 

The following code example shows how to provision a site by using code.

```C#
	public class ContosoSite : SPWebProvisioningProvider
	{
	    public override Provision(SPWebProvisioningProperties props)
	    {        
	        // Obtain a reference to the new SPWeb object.
	        SPWeb newWeb = props.Web;
	        // Apply the blank site template.
	        newWeb.ApplyWbTemplate(“STS#1”);
	        // Retrieve configuration data.
	        string data = props.Data;
	        // Add lists, libraries, Features, pages, and other components to the site.
	        // web.Lists.Add(…);
	        // web.Features.Add(…);
	    }
	}
```

If you develop a custom site provisioning provider, you do not need to deploy a folder to the **SiteTemplates** folder, and you do not need to create an **Onet.xml** file; however, you must make some changes to the **Webtemp*.xml** file. In your Webtemp*.xml file, for each configuration you must add the **ProvisionAssembly**, **ProvisionClass**, and optionally **ProvisionData** attributes. 

The **ProvisionAssembly** attribute should contain the strong name key for your assembly.

The **ProvisionClass** attribute should contain the fully qualified class name for the provisioning provider. 

The **ProvisionData** attribute is optional. If you choose to provide this attribute, the value of this attribute is available in the Provision method by using the Data property of the parameter. Because this attribute only enables you to specify a single string value, and if you need to pass significant configuration data to your custom provisioning class, you may consider storing a file name in the ProvisionData attribute, and then create a corresponding configuration file for use by your custom provisioning class. 

You should deploy the assembly containing your custom site provisioning provider to the global assembly cache. 

The following code example shows a Webtemp*.xml file that specifies a custom provisioning provider.

```XML
	<Templates xmlns:ows="Microsoft.SharePoint">
	    <Template Name="ContosoSite" ID="12001">
	        <Configuration ID="0"
	            Title="Contoso Research Site"
	            Hidden="FALSE"
	            ImageUrl="/_layouts/15/ContosoSiteImage.png"
	            Description="This is a custom site template for Contoso research projects."
	            DisplayCategory="Contoso Sites"
	            ProvisionAssembly=
	                "ContosoAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=abcde1234567890a"
	            ProvisionClass="Contoso.ContosoSite"
	            ProvisionData="ResearchConfiguration.xml"
	            RootWebOnly="FALSE"
	    <Template>
	</Templates>
```

## **Feature Stapling**

A common requirement is to add a new Feature to a site definition. One option is to create a new site definition that includes the Feature. If an existing site definition already fulfills the remainder of your requirements, developing a new site definition is likely to introduce duplication and introduces risks of errors being introduced if a site definition is updated. 

In addition, you would need to communicate to your administrators that they should use the new site definition. Your administrators may continue to use the existing or out-of-the-box site definitions despite your best efforts. 

Another option is to update an existing site definition to include the Feature; however, there are problems with this solution:

- You must not change out-of-the-box site definitions.

- If you update an existing custom site definition, you may break any existing sites that use that site definition. 

- You may need to add a Feature to a site definition only for sites in a particular site collection or web application.

Feature stapling provides an alternative approach. You can **use Feature stapling** to **add a Feature to an existing site definition**. To add a Feature (the stapled Feature) to an existing site definition, you must create a new Feature (the stapler Feature). 

The stapler Feature should contain an **Elements.xml** file that contains a **FeatureSiteTemplateAssociation** element. 

The **FeatureSiteTemplateAssociation** element has two attributes that you must specify, the **Id** attribute, which should contain the ID of the stapled Feature, and the **TemplateName** attribute, which should contain the name and configuration of the site definition to which the stapled Feature should be added in the format SiteName#ConfigurationId. 

To staple more than one Feature, to a site definition, or to staple the Feature to more than one site definition, you can add additional **FeatureSiteTemplateAssociation** elements. 

The following code example shows how to staple a Feature to the Team Site site definition.

```XML
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	    <FeatureSiteTemplateAssociation 
	        Id="3E00ABF6-1301-4698-9BD1-5500725000F0"
	        TemplateName="STS#0"
	</Elements>
```

**Feature stapling scope**

A Feature stapler only impacts sites that are created within the scope at which the Feature stapler is activated. This enables you to add a Feature to a site definition selectively. You can choose the Farm, Web Application, or Site scope for the Feature stapler. The stapled Feature should have either the Site or Web scope. 

After you deploy the stapled and stapler Features, SharePoint will add the stapled Feature to any new sites that are created, within the activation scope of the stapler Feature, and that are based on the site definition. SharePoint will not change any existing sites. If you need to add a Feature to existing sites that are based on a particular site definition, you must either: write code, use Windows PowerShell, or use the SharePoint interface, to add the new Feature to those sites.

## **Understanding the Site Hierarchy**

All SharePoint sites form part of the SharePoint site hierarchy. If you start with the root of the hierarchy, you can obtain references to any site in the farm by using collection classes that expose each item in the hierarchy:

1. The **SPFarm** class represents the farm and is the root of the hierarchy. The SPFarm class exposes the Services property of type **SPServiceCollection**. 

2. The **SPService** class represents services within the farm. The most common services are the content service and the administration service, both represented by the **SPWebService** class (the SPWebService class derives from the SPService class). The SPWebService class exposes the WebApplications property of type **SPWebApplicationCollection**. 

3. The **SPWebApplication** class represents web applications. A typical SharePoint farm will have many web applications. This will include web applications for the Central Administration site, for SharePoint sites, and for service applications. The **SPWebApplication** class exposes the Sites property of type **SPSiteCollection**.

4. The **SPSite** class represents site collections. The number of site collections in your farm will depend on your deployment requirements. The SPSite class exposes the AllWebs property of type **SPWebCollection**.

5. The **SPWeb** class represents sites. Like site collections, the number of sites in your farm will depend on your deployment. 

Although the **SPWeb** class is the bottom component in the site hierarchy, it does not represent the bottom of the SharePoint hierarchy. For example, an SPWeb instance contains a collection of lists, which in turn contain a collection of list items, each with a collection of fields, and a collection of attachments. 