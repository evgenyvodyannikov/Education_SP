# Module 3 - Designing and Managing Features and Solutions

## **Topics**:

1. [Understanding Features](#understanding-features)

### **Understanding Features**

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

### **Anatomy of a Feature**

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
