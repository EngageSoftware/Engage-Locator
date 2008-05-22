<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.ManageComments" AutoEventWireup="True"  CodeBehind="ManageComments.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<div class="div_ManagementButtons">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lblManageLocations_OnClick" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lblImportFile_OnClick" />

    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lblManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" />
</div>
<br />
<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations."  Visible="False" resourcekey="lblConfigured"></asp:Label>
<asp:Label ID="lblNoPending" runat="server" Text="No Pending Comments" CssClass="Normal" resourceKey="lblNoPending"></asp:Label>
<div class="divPanelTab" id="divPanelTab" runat="server">
    <asp:Label ID="lblSuccess" runat="server" CssClass="Normal"></asp:Label>
    <div>
        <asp:DataGrid ID="dgSubmittedComments" runat="server" CssClass="importData" GridLines="Vertical"
            AllowPaging="True" AutoGenerateColumns="False" 
            OnDeleteCommand="dgComments_DeleteCommand" OnItemCreated="dgComments_ItemCreated">
            <FooterStyle BackColor="#ccc" ForeColor="Black" />
            <PagerStyle CssClass="dataImportPage" HorizontalAlign="Center" Mode="NumericPages" />
            <HeaderStyle CssClass="dataImportHeader" />
            <Columns>
                <asp:TemplateColumn HeaderText="ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblCommentId" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.CommentId", "{0:d}") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Location">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationName" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Name", "{0:d}") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Comment">
                    <ItemTemplate>
                        <asp:Label ID="lblComment" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Text", "{0:d}") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Submitted By">
                    <ItemTemplate>
                        <asp:Label ID="lblCommentAuthor" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Author", "{0:d}") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn >
                    <ItemTemplate>
                        <asp:CheckBox ID="cbCommentApproved" runat="server" />
                    </ItemTemplate>
                    <ItemStyle CssClass="typeDataGridFooter" />
                    <FooterStyle CssClass="typeDataGridFooter" />
                    <HeaderStyle CssClass="typeDataGridFooter" />
                </asp:TemplateColumn>
            </Columns>
            <SelectedItemStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <AlternatingItemStyle BackColor="#eeeeee" />
            <ItemStyle BackColor="#f8f8f8" ForeColor="Black" />
        </asp:DataGrid>
    </div>
    <br />
    <div id="div_navigation">
        <asp:ImageButton ID="btnAcceptComment" runat="server" CssClass="CommandButton" AlternateText="Approve"  resourcekey="btnAccept" OnClick="btnApprove_Click" Visible="False" ToolTip="Click here to Approve the selected comments." ImageUrl="~/desktopmodules/EngageLocator/images/approve.gif"/>
        <asp:ImageButton ID="btnDeleteComment" runat="server" CssClass="CommandButton" AlternateText="Reject"  resourcekey="btnDelete" OnClick="btnReject_Click" Visible="False" ToolTip="Click here to Reject the selected comments." ImageUrl="~/desktopmodules/EngageLocator/images/reject.gif"/>
        <asp:ImageButton ID="btnCancelComment" runat="server" CssClass="CommandButton" AlternateText="Cancel"  resourcekey="btnCancel" OnClick="btnCancel_Click" ToolTip="Click here to go back to the previous screen." ImageUrl="~/desktopmodules/EngageLocator/images/back.gif" />        
    </div>
</div>
