<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.ManageComments" AutoEventWireup="True"  CodeBehind="ManageComments.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

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
                <asp:TemplateColumn HeaderText="Approved">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbCommentApproved" runat="server" />
                    </ItemTemplate>
                    <ItemStyle CssClass="typeDataGridFooter" />
                    <FooterStyle CssClass="typeDataGridFooter" />
                    <HeaderStyle CssClass="typeDataGridFooter" />
                </asp:TemplateColumn>
<%--                <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update"></asp:EditCommandColumn>--%>
                <asp:ButtonColumn CommandName="Delete" Text="Reject"></asp:ButtonColumn>
            </Columns>
            <SelectedItemStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <AlternatingItemStyle BackColor="#eeeeee" />
            <ItemStyle BackColor="#f8f8f8" ForeColor="Black" />
        </asp:DataGrid>
    </div>
    <div>
        <asp:Button ID="btnAcceptComment" runat="server" CssClass="CommandButton" Text="Accept"
            resourcekey="btnAccept" OnClick="btnAcceptComment_Click" />
        <asp:Button ID="btnCancelComment" runat="server" CssClass="CommandButton" Text="Cancel"
            resourcekey="btnCancelComment" OnClick="btnCancel_Click" />
        <asp:Button ID="btnDeleteComment" runat="server" CssClass="CommandButton" Text="Reject"
            resourcekey="btnDelete" OnClick="btnDeleteComment_Click" />
    </div>
</div>
