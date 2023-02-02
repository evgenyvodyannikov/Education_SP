# Module 3 - Designing and Managing Features and Solutions

## **Topics**:

1. [Understanding Features](#understanding-features)
2. [Anatomy of a Feature](#anatomy-of-a-feature)
3. [Understanding Feature Scopes](#understanding-feature-scopes)
4. [Understanding Solutions](#understanding-solutions)
5. [Anatomy of a Solution](#anatomy-of-a-solution)
6. [Planning and Configuring Feature Dependencies](#planning-and-configuring-feature-dependencies)
7. [Creating Solution Activation Dependencies](#creating-solution-activation-dependencies)

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