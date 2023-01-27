<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseCheckerWebPart.ascx.cs" Inherits="ExpenseChecker.ExpenseCheckerWebPart.ExpenseCheckerWebPart" %>
<div id="expenseChecker">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <p>Select one or more items, and then click <b>Approve</b> or <b>Reject</b>.</p>
            <asp:ListView runat="server" ID="lstExpenses">                
                <LayoutTemplate>
                    <table runat="server" id="tblExpenses" cellpadding="2">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Requestor</th>
                                <th>Category</th>
                                <th>Description</th>
                                <th>Amount</th>                                
                            </tr>
                        </thead>
                        <tr runat="server" id="itemPlaceholder"></tr>
                    </table>
                    <asp:DataPager runat="server" ID="dataPager" PageSize="10">
                        <Fields>
                            <asp:NumericPagerField ButtonCount="10" PreviousPageText="<--" NextPageText="-->" />
                        </Fields>
                    </asp:DataPager>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr runat="server">
                        <td runat="server">
                            <asp:CheckBox ID="chkUpdate" runat="server" />                            
                            <asp:HiddenField ID="hiddenID" runat="server" Value='TODO' />
                        </td>
                        <td runat="server">                            
                            <asp:Label ID="lblRequestor" runat="server" Text='TODO' />
                        </td>
                        <td runat="server">
                            <asp:Label ID="lblCategory" runat="server" Text='TODO' />
                        </td>
                        <td runat="server">
                            <asp:Label ID="lblDescription" runat="server" Text='TODO' />
                        </td>
                        <td runat="server">                            
                            <asp:Label ID="lblAmount" runat="server" Text='TODO' />
                        </td>                        
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <br />
            <asp:Button runat="server" ID="btnApprove" Text="Approve" Width="100" OnClick="btnApprove_Click" />
            &nbsp;
            <asp:Button runat="server" ID="btnReject" Text="Reject" Width="100" OnClick="btnReject_Click" />       
        </ContentTemplate>
    </asp:UpdatePanel>
</div>