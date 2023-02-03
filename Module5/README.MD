# Module 5 - Working with Server-Side Code

## **Topics**:

1. [Introduction to Web Parts](#introduction-to-web-parts)

## **Introduction to Web Parts**

SharePoint enables you to create **Web Parts** that you can include on SharePoint pages. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/216547615-8d71f56c-5c87-495e-bfbd-42d2738c41d6.png">
</p>

Web Parts provide flexibility to users who can add a Web Part to any page. You can make Web Parts that display data relevant to the current page, site, or other contextual component, or you can make Web Parts that display the same content wherever they are deployed. 

In SharePoint 2013, much of what you can achieve by using a Web Part can also be achieved by using an app part. In most cases, you should choose to use an app part in preference to a Web Part; however, there are still some circumstances in which choosing a Web Part would be the appropriate choice. 

If you do not have an app infrastructure configured for your SharePoint farm, you may choose to develop a Web Part instead of an app part. Another reason you may choose to develop a Web Part is to take **advantage of Web Part connections**. Web Parts can expose data to other Web Parts on the page, and Web Parts can then consume exposed data. You cannot consume or expose data to other Web Parts by using an app part, so if you need to use this functionality, developing a Web Part would be an appropriate choice. 

To develop a Web Part, you normally create a class that inherits from the WebPart class (in the `System.Web.UI.WebControls.WebParts` namespace). You then override methods in that class to control how your Web Part is rendered. You then compile the assembly and deploy it to the server. You use a **.webpart** Web Part definition file to add the Web Part to a Web Part Gallery, from which users, with the appropriate permissions, can add the Web Part to a page. 


