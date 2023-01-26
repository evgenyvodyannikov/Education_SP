<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TitleCheckerWebPart.ascx.cs" Inherits="TitleChecker.TitleCheckerWebPart" %>
<asp:UpdatePanel ID="upTitleChecker" runat="server">
    <ContentTemplate>
        <div id="titleChecker">
            <p>This site includes the following subsites:</p>
            <asp:ListBox ID="lstWebs" runat="server" SelectionMode="Single" OnSelectedIndexChanged="lstWebs_SelectedIndexChanged" AutoPostBack="True" Rows="10" />
            <SharePoint:SPSecurityTrimmedControl runat="server" PermissionsString="ManageWeb">                
                <asp:Panel ID="pnlUpdateControls" runat="server">
                    <p>Type a new title in the box below, and then click Update.</p>
                    <asp:TextBox ID="txtTitle" runat="server" Width="200px" />
                    <asp:Button ID="btnUpdate" Text="Update" runat="server" OnClick="btnUpdate_Click" />
                </asp:Panel>
            </SharePoint:SPSecurityTrimmedControl>
            <asp:Panel ID="pnlResult" runat="server">
                <p>
                    <asp:Literal ID="litResult" runat="server" />
                </p>
            </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
