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