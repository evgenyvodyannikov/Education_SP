<%-- The following 4 lines are ASP.NET directives needed when using SharePoint components --%>

<%@ Page Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" MasterPageFile="~masterurl/default.master" Language="C#" %>

<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%-- The markup and script in the following Content element will be placed in the <head> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>

    <!-- Add your CSS styles to the following file -->
    <link rel="Stylesheet" type="text/css" href="../Content/App.css" />

    <!-- Add your JavaScript to the following file -->
    <script type="text/javascript" src="../Scripts/App.js"></script>
</asp:Content>

<%-- The markup in the following Content element will be placed in the TitleArea of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Site Suggestions
</asp:Content>

<%-- The markup and script in the following Content element will be placed in the <body> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div>
        <p id="message">
            <!-- The following content will be replaced with the user name when you run the app - see App.js -->
            initializing...
        </p>
    </div>

    <div id="new-item-form">
        Please enter your suggestion here:
        <div class="new-item-form-line">
            <span class="new-item-form-label">Subject:</span>
            <span class="new-item-form-field">
                <input id="subject-input" class="new-item-form-control" type="text" />
            </span>
        </div>
        <div class="new-item-form-line">
            <span class="new-item-form-label">Feedback:</span>
            <span class="new-item-form-field">
                <textarea rows="5" cols="50" class="new-item-form-control" id="feedback-input"></textarea>
            </span>
        </div>
        <div>
            <input type="submit" onclick="Contoso.SuggestionsApp.create_suggestion();" />
        </div>
    </div>


    <p>Here are the current suggestions:</p>
    <div id="suggestions-list">
    </div>

    <div id="item-display">    
       <div class="item-display-line">
           <span class="item-display-label">Subject:</span>
           <span id="item-display-subject" class="item-display-field"></span>
           <span class="item-voting-controls">
               <a>Like</a>
               <a>Dislike</a>
           </span>
       </div>
       <div class="item-display-line">
           <span class="item-display-label">Feedback:</span>
           <span id="item-display-feedback" class="item-display-field"></span>
           <span class="votes-display">
               <span>Votes:</span>
               <span id="votes-count"></span>
           </span>
       </div>

    </div>

    <div class="clear-floats"></div>

</asp:Content>
