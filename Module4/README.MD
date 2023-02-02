# Module 3 - Designing and Managing Features and Solutions

## **Topics**:

1. [Understanding Features](#understanding-features)
2. [Anatomy of a Feature](#anatomy-of-a-feature)
3. [Understanding Feature Scopes](#understanding-feature-scopes)
4. [Understanding Solutions](#understanding-solutions)
5. [Anatomy of a Solution](#anatomy-of-a-solution)
6. [Planning and Configuring Feature Dependencies](#planning-and-configuring-feature-dependencies)
7. [Creating Solution Activation Dependencies](#creating-solution-activation-dependencies)
8. [Managing Feature Upgrades](#managing-feature-upgrades)
9. [Managing Solution Upgrades](#managing-solution-upgrades)
10. [Administering Features and Solutions](#administering-features-and-solutions)
11. [Creating Feature Receivers](#creating-feature-receivers)

## **Understanding Features**

A Feature is a set of files that provisions custom SharePoint functionality or components. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216036222-c2996389-8b87-41b0-9eff-3b5aed54b33e.png">
</p>

Features are targeted to a **specific scope** in the SharePoint logical architecture—the farm level, the web application level, the site collection level, or the site level—according to the type of functionality that the Feature provides. 

Administrators at the specified scope can make the Feature functionality available to their users by activating the Feature, and they can remove the functionality by deactivating the Feature. In this way, Features support a modular approach to customizations by enabling administrators to turn on and turn off customizations.

**What is a Feature?**

In SharePoint, a Feature is a set of one or more XML files. These XML files, which are known as **elements files or element manifests**, use Collaborative Application Markup Language (CAML) to define various SharePoint components, such as site columns or content types. 

Every Feature includes a file named **Feature.xml**. This file is often referred to as the feature manifest. This file performs two key functions:

- It identifies the **individual element manifests** that belong to the Feature.
- It identifies the **scope** of the Feature.

**What can you include in a Feature?**

You can use a Feature to deploy any SharePoint component that can be defined declaratively using CAML. The following table describes the different types of components that you can include in a Feature.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216036958-8a89f683-b384-40fc-b311-cb427207300d.png">
</p>

**How are Features deployed?**

You can deploy a Feature in three different ways:
- **Manually copy the Feature files** to the SharePoint file system. You can copy the Feature folder to the **15\TEMPLATE\FEATURES** folder in the SharePoint server file system and then use Windows PowerShell to install the Feature. 

- **Include** the Feature **in a farm solution or a sandboxed solution**. You can use SharePoint solutions to deploy and retract Features. The Features are installed when the solution is added and uninstalled when the feature is removed. Features deployed within solutions are automatically synchronized across every server in the farm.

- **Include** the Feature **in an app for SharePoint**. Features are used within app packages to provision declarative components on the app web and the host web.

## **Anatomy of a Feature**

When you create a Feature, every component of the Feature is contained within a top-level Feature folder. **The name of this folder is important**, because this is the name by which you will **refer to the Feature** if you install and uninstall it by using Windows PowerShell.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216037693-70595839-9821-4cb8-abdb-0beafacca2b8.png">
</p>

**The Feature manifest file**

The Feature manifest file is created at the root of the Feature folder. The Feature manifest file must always be placed in the root Feature folder, rather than in any subfolders, and the Feature manifest file is always named **Feature.xml**. 

The Feature manifest file performs several important roles:

- It defines a **unique identifier**, a **title**, and a **description** for the Feature.

- It defines the **scope** of the Feature.

- It identifies the **element manifest files** that belong to the Feature, and specifies their relative **location** within the Feature folder.

The following code example shows a feature manifest file. This is taken from the built-in DocumentLibrary feature, which defines the Document Library list template:

```XML
	<Feature Id="00BFEA71-E717-4E80-AA17-D0C71B360101"
	               Title="$Resources:core,documentlibraryFeatureTitle;"
	               Description="$Resources:core,documentlibraryFeatureDesc;"
	               Version="1.0.0.0"
	               Scope="Web"
	               Hidden="TRUE"
	               DefaultResourceFile="core"
	              xmlns="http://schemas.microsoft.com/sharepoint/">
	   <ElementManifests>
	      <ElementManifest Location="ListTemplates\DocumentLibrary.xml" />
	   </ElementManifests>
	</Feature>
```

The key points in this example are as follows:

- The Feature **is scoped at the web level**. This means that the Feature can be activated and deactivated on individual sites. 

- The Feature **is hidden**. This means that the Feature cannot be managed through the user interface.

- The Feature includes a **single element manifest**. The Feature manifest references a single element manifest file named DocumentLibrary.xml, which is in a subfolder named ListTemplates within the top-level Feature folder.

**Element manifest files**

Features can contain one or more element manifest files. Element manifest files are often **organized in subfolders** within the top-level Feature folder. However, this is not mandatory—beneath the top-level Feature folder and the Feature manifest file, you can structure your Feature any way you want. You can also choose any name you want for subfolders and element manifest files.

The following code example shows an element manifest file. This is taken from the built-in DocumentLibrary feature:

```XML
	<Elements xmlns="http://schemas.microsoft.com/sharepoint/">
	   <ListTemplate
	        Name="doclib"
	        Type="101"
	        BaseType="1"
	        OnQuickLaunch="TRUE"
	        SecurityBits="11"
	        Sequence="110"
	        DisplayName="$Resources:core,doclibList;"
	        Description="$Resources:core,doclibList_Desc_15;"
	        Image="/_layouts/15/images/itdl.png?rev=23"
	        DocumentTempalte="121"
	</Elements>
```

The top-level XML element in an element manifest file is always an **Elements** element. The Elements element supports various child elements that you can use to define different types of functionality.

## **Understanding Feature Scopes**

Every SharePoint Feature **must be scoped** to a level in the SharePoint logical architecture. 

The scope defines the level at which the Feature must be activated and where the functionality defined by the Feature is available.

The scope of a Feature is defined by the Scope attribute of the Feature element in the Feature manifest file. 

This attribute can take four possible values:

- **Farm**. The Farm scope specifies that the functionality contained within the Feature is available to all web applications, site collections, and sites within the farm. Farm-scoped Features must be activated by a farm administrator.

- **WebApplication**. The WebApplication scope applies to all of the sites collections and sites contained within a specific web application.

- **Site**. The Site scope scopes the Feature to an individual site collection. Site-scoped Features must be activated by the site collection administrator.

- **Web**. The Web scope is the most restrictive of scopes because it applies to a single site; however, it can be activated by a site owner.

Choosing the most appropriate scope is an important design consideration when you create a Feature. It is important to understand that you do not scope a Feature to a specific web application, site collection, or site. 

Instead, if you deploy a WebApplication-scoped Feature, the Feature is available for activation on every web application in the SharePoint farm. The administrator can choose to activate the Feature if the functionality is relevant to that specific web application.

## **Understanding Solutions**

In reality, you will rarely, if ever, manually deploy a Feature. There are two main reasons why manually deploying Features is not a good idea:

- You must **manually add the Feature folder** to the SharePoint file system, and remove it when it is no longer required.

- You must **manually copy the Feature folder to every server** in the SharePoint farm, and make sure the Feature contents remain synchronized on every server.

Fortunately, SharePoint provides a more satisfactory way of managing the life cycle of custom components in the form of the SharePoint solution package.

**What is a solution?**

In SharePoint terminology, a solution is essentially a cabinet file with a **.wsp** file name extension. The solution framework enables you to **package**, **deploy**, and **manage** the life cycle of various custom components for SharePoint.

There are three types of solutions in SharePoint 2013:

- **Farm solutions**. Farm solutions are deployed at the farm scope by a farm administrator. You can use a farm solution to package and deploy any custom functionality to SharePoint, including fully-trusted server-side code.

- **Sandboxed solutions**. Sandboxed solutions, also known as user solutions, are deployed at the site collection scope by a site collection administrator. Any custom code within a sandboxed solution runs under an isolated process with limited permissions, and sandboxed solutions are subject to various monitoring and throttling constraints.

- **Solutions within apps** for SharePoint. When you create an app for SharePoint, any components that you want to deploy to the app web are packaged in a solution. This solution package cannot contain any server-side code.

The big advantage of using solutions is that they provide **clean deployment**, **activation**, and **removal** of **custom components**. 

For example, if you deploy a Feature within a farm solution, SharePoint will ensure that the Feature files are added to every web server in the server farm. 

When you retract the solution, SharePoint will remove the Feature files from every web server in the server farm. More broadly, any changes that are made when you activate a solution will be reversed when you retract the solution.

**What can you include in a solution?**

You can use a solution package to deploy two types of component to a SharePoint environment:

- **Features**. You can use solution packages to deploy Features by including the entire contents of the Feature, including the Feature manifest file, within the solution package.

- **Assemblies**. If your custom components include server-side code, you can use the solution package to deploy the assembly to the global assembly cache or the bin folder for a specific IIS web application. In sandboxed solutions, assemblies are stored in the content database and loaded by an isolated worker process.

**Note:** Assemblies that are deployed to the global assembly cache are automatically granted full trust. 

Every solution package includes a solution manifest file named **Manifest.xml** that specifies what the package contains. You can also use the solution manifest file to perform various configuration tasks, such as adding **safe control** entries to the **Web.config** file when you deploy an assembly.

**How are solutions deployed?**

To deploy a farm solution, a farm administrator must add the solution to the farm using Windows PowerShell, and then install it using either Windows PowerShell or the Central Administration website. 

To deploy a sandboxed solution, a site collection administrator must add the solution to the Solutions gallery on a site collection. Farm administrators can also deploy sandboxed solutions to specific site collections using Windows PowerShell.

Solutions contained within app packages are deployed automatically as part of the app installation process.

## **Anatomy of a Solution**

A SharePoint solution package is a cabinet file with a **.wsp** file name extension. The contents and behavior of the solution are specified by the solution manifest file, which is an XML file named **Manifest.xml**.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216042056-f68a5c62-ef58-44b7-b124-27e312ff84a0.png">
</p>

The following code example shows a typical solution manifest file:

```XML
	<Solution xmlns="http://schemas.microsoft.com/sharepoint/"
	                 SolutionID="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
	                 SharePointProductVersion="15.0">
	   <FeatureManifests>
	      <FeatureManifest Location="ContosoProject\feature.xml" />
	   </FeatureManifests>
	   <RootFiles>
	      <RootFile Location="Resources\contoso.resx" />
	      <RootFile Location="Resources\contoso.en-US.resx" />
	      <RootFile Location="Resources\contoso.en-ES.resx" />
	   </RootFiles>
	   <TemplateFiles>
	      <TemplateFile Location="Controls\ContosoLogIn.ascx" />
	   </TemplateFiles>
	   <Assemblies>
	      <Assembly Location="ContosoSolution.dll" DeploymentTarget="GlobalAssemblyCache">
	      <SafeControls>
	         <SafeControl Assembly="[Assembly strong name]" 
	                               Namespace="[Namespace]"
	                               TypeName="*" />
	      <SafeControls>
	   </Assemblies>
	</Solution>
```

This example illustrates the deployment of various different types of custom component. For example:

- **Features**. To include a Feature in a solution, add a **FeatureManifest element** for each Feature. The Location attribute indicates the relative path to the Feature manifest file within the solution package.

- **Resources files**. To include localization resources in a solution, add a **RootFile element** for each resource file. The Location attribute indicates the relative path to the resource file within the solution package.

- **Template files**. To include template files in a solution, add a **TemplateFile element** for each template file. Template files are typically items that you want to add to the server file system, such as **ASCX control** files or **default webpages**. The Location attribute indicates the relative path to the template file within the solution package.

- **Assemblies**. To include assemblies in a solution, add an **Assembly element** for each assembly. The Location attribute indicates the relative path to the assembly within the solution package, and the **DeploymentTarget attribute** specifies whether you want to deploy the assembly to the global assembly cache or the web application bin folder. You can use a SafeControl child element to register classes within the assembly, such as Web Part classes, as safe controls in the Web.config file.

## **Planning and Configuring Feature Dependencies**

The built-in Features in SharePoint 2013 use Feature activation dependencies extensively. Feature activation dependencies enable you to require the activation of an existing Feature before an administrator can activate a new Feature. 

Dependencies are a useful tool when you design your own Feature sets.

**Dependency scenarios**

Feature activation dependencies are used in two main scenarios:

- **Grouping**. You can use dependencies to group multiple granular Features together in a single Feature. Suppose you create a set of hidden Features that each deploy a site column. You could then create one or more visible Features that depend on a subset of these hidden Features to deploy commonly-used sets of site columns, such as site columns for finance teams or project management tasks. When you activate the visible Feature, SharePoint will automatically activate the hidden Features that are included as dependencies. This approach enables you to define functionality at a very granular level but expose functionality at a business requirements level.

- **Resource guarantees**. You can use dependencies to make sure particular components are available before your Feature is activated. For example, suppose you create a Feature that binds a content type to a list instance. You could use Feature dependencies to make sure both the Feature that provisions the content type and the Feature that provisions the list instance are activated before your content type binding Feature is activated.

**Dependency rules**

Feature dependencies are governed by a series of rules that are designed to prevent circular dependencies and performance issues. The most notable rules are as follows:

- **Dependency chains**. Generally speaking, dependency chains are not supported. If Feature A depends on Feature B, Feature B cannot include dependencies to any other visible Features. Feature B can include dependencies on hidden Features.

- **Hidden Features**. Typically, hidden Features are intended to be used as dependencies by other Features. Hidden Features cannot themselves include dependencies. SharePoint will automatically deactivate a hidden Feature when the last visible Feature that depends on it is deactivated.

- **Same-scope dependencies**. If Feature A depends on Feature B, and both Features are at the same scope, SharePoint will automatically activate Feature B when you activate Feature A.

- **Cross-scope dependencies**. Features can only depend on other Features at a less restrictive scope. For example, a Web-scoped Feature can include a dependency on a Site-scoped Feature, but a Site-scoped Feature cannot include a dependency on a Web-scoped Feature. SharePoint will never automatically activate Features referenced in cross-scope dependencies—instead, the activation will fail and the administrator will be prompted to install the dependency Feature first. You cannot create cross-scope dependencies on hidden Features.

**Configuring dependencies**

When you define a Feature, you can specify dependencies on other Features by adding **ActivationDependency** elements to your Feature manifest file. Visual Studio 2012 includes a designer that can aid you in this process.

The following code example shows a Feature manifest file with two activation dependencies:

```XML
	<Feature xmlns="http://schemas.microsoft.com/sharepoint/"
	                Title="FinanceAssets"
	                Id="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
	                Scope="Web">
	    <ActivationDependencies>
	      <ActivationDependency FeatureId="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
	                                              FeatureTitle="FinanceSiteColumns" />
	      <ActivationDependency FeatureId="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
	                                              FeatureTitle="FinanceContentTypes" />
	   </ActivationDependencies>
	</Feature>
```

**Note:** When you add an ActivationDependency element, you only need to specify a **FeatureId** attribute. The FeatureTitle attribute is optional and simply serves to make your Feature manifest file more comprehensible for other humans.

## **Creating Solution Activation Dependencies**

In addition to creating Features that require the activation of one or more existing Features, you can create **solutions that require the activation of one or more existing solutions**. 

Solution activation dependencies are used **far less commonly** than Feature activation dependencies. This is because a solution, as the name suggests, typically represents a complete and self-contained set of components. By contrast, Features often represent more granular functionality.

To define a solution activation dependency, you must add an **ActivationDependency** element to the solution manifest file. You must provide a **SolutionId** attribute, and you can also specify an optional SolutionName attribute to make your manifest more readable. 

In contrast to Feature activation dependencies, the Visual Studio designer does not provide tooling for adding solution activation dependencies and you must manually edit the solution manifest.

The following code example shows a solution manifest file that includes an activation dependency:
```XML
	<Solution xmlns="http://schemas.microsoft.com/sharepoint/"
	              SolutionId="xxxxxxxx-xxxx-xxxx-xxxxxxxxxxxx"
	              SharePointProductVersion="15.0">
	   <ActivationDependencies>
	      <ActivationDependency SolutionId="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
	                                              SolutionName="ContosoFinanceTools" />
	   </ActivationDependencies>
	   <!-- Other solution manifest content -->
	</Solution>
```

## **Managing Feature Upgrades**

In any organization, custom software components are likely to evolve over time. It is very rare that an organization will release version 1.0 of a component and then make no additional changes. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216281110-c93b106f-48e9-41de-b84c-558f2b081f6c.png">
</p>

SharePoint 2013 provides a mechanism for managing component evolution and version in the form of Feature upgrades. An upgraded Feature must be able to do two things:

- Where a previous version of the Feature is not activated, it must provision every component within the Feature.

- Where a previous version of the Feature is activated, it must modify the existing components and add new components as required.

SharePoint meets these requirements by enabling you to target upgrade actions to specific Feature versions.

**Upgrading a Feature**

You can use the following high-level steps to upgrade a Feature:

1. Add any new or modified files to the Feature folder.
2. Add new element manifest files for any new items. Do not modify the existing element manifest files.
3. In the Feature manifest file, set the Version attribute to a new version number. 
4. In the Feature manifest file, add an **UpgradeActions** element.
5. Within the **UpgradeActions** element, add one or more **VersionRange** elements to target changes to specific Feature versions or version ranges.

The following code example shows a Feature manifest file that includes upgrade actions:

```XML
	<Feature Id="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
	                Title="Invoicing Resources"
	                Scope="Site"
	                Version="3.0.0.0">
	   <UpgradeActions>
	      <VersionRange BeginVersion="1.0.0.0" EndVersion="2.9.9.9">
	         <ApplyElementManifests>
	            <ElementManifest Location="Columns\InvoiceOwner.xml" />
	            <ElementManifest Location="Columns\ClientContact.xml" />
	         </ApplyElementManifests>
	         <AddContentTypeField ContentTypeId="0x01…"
	                FieldId="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
	                PushDown="True" />
	         <AddContentTypeField ContentTypeId="…"
	                FieldId="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
	                PushDown="True" />
	      </VersionRange>
	   </UpgradeActions>
	   <ElementManifests>
	      <!—Register all element manifests here as before. -->
	   </ElementManifests>
	</Feature>
```

**Note:** In many cases, Features are deployed without specifying a Version attribute. In these instances, SharePoint treats the version number as 0.0.0.0.

**Feature upgrade capabilities**

Within a VersionRange element, you can include the following child elements to perform various upgrade tasks:

- **ApplyElementManifests**. Use this to apply additional element manifests to a range of Feature versions. The element manifests specified here should be those that are missing from the specified range of Feature versions.

- **MapFile**. Use this element to replace an existing file with a new file in the specified range of Feature versions. Use the FromPath and ToPath to identify the locations of the existing file and the new file, respectively, within the Feature folder.

- **AddContentTypeField**. Use this element to add a new field to a site content type. The site content type you specify must be defined in the previous version of the Feature. Use the PushDown attribute to specify whether you want to cascade changes to list content types and inherited content types.

- **CustomUpgradeAction**. Use this element to specify Feature upgrade event receiver classes that you want to execute when the Feature is upgraded.
You do not have to include a VersionRange attribute in your Feature manifest file; all the preceding elements can be added directly to the UpgradeActions element if you want your actions to apply to all versions of the Feature. However, it is good practice to always specify a version range for readability and backward compatibility.

## **Managing Solution Upgrades**

Like any software component, custom SharePoint solutions are likely to evolve over time. Just like Features, the SharePoint solution framework provides mechanisms to help ensure that solution updates are applied smoothly and reliably. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216283635-dc78d84b-1a59-4967-bfb7-e1c8ee88bbbc.png">
</p>

To create a new version of a solution, you simply build a new solution package (**.wsp**) with any required changes. You can then update deployed instances of the solution in one of two ways:

- Remove the existing solution and then deploy the updated version.
- Perform an in-place update by using the **Update-SPSolution** Windows PowerShell cmdlet.
Determining the most appropriate approach depends on various aspects of the SharePoint environment and the solution contents.

**Uninstalling and then reinstalling the solution**

Uninstalling the previous version of the solution and then installing the new version of the solution provides the most flexible approach to updates. 

When you update a solution in this way, you can choose to uninstall the solution and remove it from the solution store, or just uninstall the solution. If you choose to only uninstall the solution, but not to remove the solution from the solution store, you must use a new name and solution ID when you deploy the updated solution. 

If you remove the solution from the solution store, you can reinstall by using the same name and solution ID. 

The major drawback of this approach is that it requires you to be able to uninstall the existing solution. In practice, this is often not possible. For example, if your solution includes a content type that has been deployed and used in a list, you will not be able to uninstall the solution while the content type remains in use. 

This is not possible without losing data. In these circumstances, you will usually need to perform an in-place update by using the Update-SPSolution cmdlet. 

**Performing an in-place update**

To perform an in-place update, you use the Update-SPSolution cmdlet to update the solution. 

When you call the Update-SPSolution cmdlet, you should use the Identity parameter to specify the GUID of the solution and you should use the LiteralPath parameter to specify the location of the revised solution package. 

Performing an in-place update enables you to update your solution without first uninstalling it. This prevents data loss, which would be necessary with an uninstall/reinstall solution update; however, in-place updates do not support any of the following changes:

- Adding or removing a Feature
- Changing the solution ID
- Changing the scope of a Feature
- Updating a Feature receiver
- Adding, removing, or editing an element manifest file
- Adding, removing or editing a Property element in the Feature.xml file

If you need to make any of these changes, you must either perform an uninstall/reinstall update, or consider developing a new solution with your new changes. You can make the new solution dependent on the original solution if appropriate.

**Assembly binding redirection**

In many cases, when you update a solution, you will need to replace assemblies within the solution with new versions. 

When you update an assembly, you should increment the version number. However, this may break other components if they are dependent on the old version of the assembly. 

In ASP.NET, you typically solve this problem by adding **bindingRedirect** elements to your **Web.config** files. 

In SharePoint, you can add **BindingRedirect** child elements to your solution manifest. For each BindingRedirect element that you specify in your solution manifest file, when you run the solution update, SharePoint adds a bindingRedirect element to the Web.config file of each web application on every server in the SharePoint farm. 

The following code example shows a solution manifest file that includes assembly binding redirection:
```XML
	<Solution xmlns="http://schemas.microsoft.com/sharepoint/" 
	                 SolutionId="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX" 
	                 SharePointProductVersion="15">
	    <Assemblies>
	        <Assembly Location="ContosoAssembly.dll" DeploymentTarget="GlobalAssemblyCache">
	            <BindingRedirects>
	                <BindingRedirect OldVersion="1.0.0.0" />
	            </BindingRedirects>
	        </Assembly>
	    </Assemblies>
	    ...
	</Solution>
```

## **Administering Features and Solutions**

As a developer, you will not often have to administer Features and solutions. In a production environment, Features and solutions are managed by farm administrators or site collection administrators. In your development environment, Visual Studio 2012 largely automates the process for you. However, as a well-rounded SharePoint developer, you should understand the basic techniques for deploying and managing Features and solutions.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216285532-042a11f6-806b-45d7-b7fd-3d57e22468ba.png">
</p>

**Administering farm solutions**

To administer farm solutions, you need to understand the solution life cycle. To activate the functionality within a solution, a farm administrator must:

1. Add the solution to the solution store. You can use the **Add-SPSolution** cmdlet in Windows PowerShell to add the solution.

2. Install the solution. You can use the **Install-SPSolution** cmdlet in Windows PowerShell to install the solution. Alternatively, you can install the solution from the Central Administration website.

When you install a solution, SharePoint will automatically install any Features contained within the solution.

To remove a solution from the SharePoint farm, the farm administrator must:

1. Retract the solution. You can use the **Uninstall-SPSolution** cmdlet to retract a solution. Alternatively, you can retract the solution from the Central Administration website. SharePoint will not allow you to retract a solution if items within the solution, such as content types, are currently in use.

2. Remove the solution from the solution store. You can use the **Remove-SPSolution** cmdlet to remove a solution from the solution store. You can also use the Central Administration website to remove solutions.

**Note:** The administration process for sandboxed solutions differs from the administration process for farm solutions.

**Administering Features**

Like solutions, making the functionality defined by Features available to users involves two steps:

1. Install the Feature. You can use the **Install-SPFeature** cmdlet in Windows PowerShell to do this.

2. Activate the Feature. You can use the **Enable-SPFeature** cmdlet in Windows PowerShell to do this. You can also activate Features from the Central Administration website (for farm or web application-scoped Features) or the SharePoint site user interface (for site collection or site-scoped Features).

To remove a Feature, the administrator must also perform two steps:

1. Deactivate the Feature. You can use the **Disable-SPFeature** cmdlet to do this. You can also deactivate Features through the Central Administration website or the SharePoint site user interface, depending on the scope of the Feature.

2. Uninstall the Feature. You can use the **Uninstall-SPFeature** cmdlet to do this.
In most cases, Features are deployed within solutions or app packages. In these cases, you do not need to install or uninstall the Feature. Features are installed when the parent solution or app package is installed, and Features are uninstalled when the parent solution or app package is uninstalled. 

In contrast to the default experience when you deploy a solution from Visual Studio, installing a solution does not automatically activate any Features within the solution. Installing the solution makes the Features available at the specified scope. 

Administrators at the specified scope can then choose whether they want to activate the Features provided by the solution.

## **Creating Feature Receivers**

When you deploy a Feature within a solution, you can add code that runs in response to Feature life cycle events. This is useful in a variety of scenarios, such as when you need to register custom components such as timer jobs or to programmatically modify configuration files. 

**Creating a Feature receiver class**

To create a Feature receiver, add a class that inherits from **SPFeatureReceiver** to your solution. The **abstract SPFeatureReceiver** class includes methods that respond to various events in the Feature life cycle. 

You add functionality to your Feature receiver by overriding the appropriate base class method:

- To run code after a Feature is installed, override the **FeatureInstalled** method.

- To run code after a Feature is activated, override the **FeatureActivated** method.

- To run code when a Feature is upgraded, override the **FeatureUpgrading** method.

- To run code when a Feature is deactivated, override the **FeatureDeactivating** method.

- To run code when a Feature is uninstalled, override the **FeatureUninstalling** method.

The following code example shows a Feature receiver class. This example adds an announcement to a list when the Feature is activated:

```C#
	public class InvoiceListEventReceiver : SPFeatureReceiver
	{
	   public override void FeatureActivated(SPFeatureReceiverProperties properties)
	   {
	      var web = properties.Feature.Parent as SPWeb;
	      var listAnnouncements = web.Lists.TryGetList("Announcements");
	      if (listAnnouncements != null)
	      {
	         var announcement = listAnnouncements.Items.Add();
	         announcement["Title"] = "Invoice List Feature Activated!";
	         announcement["Body"] = String.Format("{0} was activated at: {1}", 
	                                                      properties.Definition.DisplayName,
	                                                      DateTime.Now.ToString());
	         announcement.Update();
	      }
	   }
	}
```

**Best Practice:** If you use the **FeatureInstalled** method to make changes when a Feature is installed, you should also use the **FeatureUninstalling** method to **reverse your changes** if the Feature is uninstalled. 

Similarly, if you use the **FeatureActivated** method to make changes when a Feature is activated, you should use the **FeatureDeactivating** method to reverse the changes when the Feature is deactivated.

**Registering a Feature receiver**

After you create a Feature receiver class, you need to associate it with your Feature. You can do this by specifying **ReceiverAssembly** and **ReceiverClass** attributes in the Feature manifest file.

The following code example shows a Feature manifest file that registers an event receiver class:

```XML
	<Feature xmlns="http://schemas.microsoft.com/sharepoint/"
	               Id="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
	               Title="Invoice List"
	               Scope="Site"
	               ReceiverAssembly="[Assembly strong name]"
	               ReceiverClass="[Fully-qualified class name]">
	</Feature>
```