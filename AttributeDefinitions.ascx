<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="AttributeDefinitions.ascx.cs"
    Inherits="Engage.Dnn.Locator.AttributeDefinitions" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div style="text-align: left;">
    <asp:Label ID="lblLocationTypeHelp" runat="Server" class="Normal" resourcekey="LocationTypeAttributesHelp" />
</div>
<br />
<fieldset>
    <legend class="Head">Location Type</legend>
    <div class="locType">
        <h3 class="SubHead">
            Select a Location</h3>
        <div class="">
            <div class="locTypeListView">
                <asp:ListBox ID="lbLocType" CssClass="Normal" runat="server" Rows="5" Width="50%"
                    AutoPostBack="True" OnSelectedIndexChanged="lbLocType_SelectedIndexChanged">
                </asp:ListBox>
            </div>
            <br />
            <div class="locTypeNav">
                <div>
                    <asp:ImageButton ID="btnEdit" runat="server" ToolTip="Click here to edit this location type"
                        AlternateText="Edit this location type" CssClass="CommandButton" 
                        ImageUrl="~/desktopmodules/EngageLocator/images/caEdit.gif" 
                        onclick="btnEdit_Click" />
                </div>
                <div>
                    <asp:ImageButton ID="btnDelete" runat="server" ToolTip="Click here to delete this location type"
                        AlternateText="Delele this location type" CssClass="CommandButton" ImageUrl="~/desktopmodules/EngageLocator/images/caDelete.gif" />
                </div>
                <div>
                    <asp:ImageButton ID="btnCreateNew" runat="server" ToolTip="Click here to create a new location type"
                        AlternateText="Create new location type" CssClass="CommandButton" 
                        ImageUrl="~/desktopmodules/EngageLocator/images/caCreateNew.gif" 
                        onclick="btnCreateNew_Click" />
                </div>
                <div>
                <asp:Label ID="lblLocationTypeNotInUse" runat="server" Visible="false" CssClass="Normal"
                    resourcekey="lblLocationTypeNotInUse"></asp:Label>
            </div>
            <div id="dvLocationType" runat="server" visible="true">
            <div id="dvLocationTypeEdit" runat="server" visible="false">
                <asp:TextBox ID="txtEditLocationType" runat="server" Width="150px" CssClass="Normal"></asp:TextBox>
                <asp:LinkButton ID="lbUpdateLocationType" runat="server" OnClick="lbUpdateLocationType_Click"
                    CssClass="CommandButton">Update</asp:LinkButton>
                <asp:LinkButton ID="lbCancelLocationType" runat="server" OnClick="lbCancelLocationType_Click"
                    CssClass="CommandButton">Cancel</asp:LinkButton>
                                <div>
                <asp:Label ID="lblLocationTypeInst" CssClass="Normal" runat="server" resourceKey="lblLocationTypeInst" /></div>

            </div>
            </div>
            </div>
        </div>
    </div>
</fieldset>
<br />
<asp:UpdatePanel ID="upDataImport" runat="server">
    <ContentTemplate>
        <fieldset>
            <legend class="Head">Custom Attributes</legend>
            <div class="caData">
                <asp:DataGrid ID="grdLocationTypeAttributes" AutoGenerateColumns="false" runat="server"
                    Width="100%" CellPadding="4" GridLines="None" CssClass="DataGrid_Container" runat="server"
                    OnItemCommand="grdLocationTypeAttributes_ItemCommand" OnItemCreated="grdLocationTypeAttributes_ItemCreated"
                    OnItemDataBound="grdLocationTypeAttributes_ItemDataBound">
                    <HeaderStyle CssClass="NormalBold" VerticalAlign="Top" HorizontalAlign="Center" />
                    <ItemStyle CssClass="DataGrid_Item" HorizontalAlign="Left" />
                    <AlternatingItemStyle CssClass="DataGrid_AlternatingItem" />
                    <EditItemStyle CssClass="NormalTextBox" />
                    <SelectedItemStyle CssClass="NormalRed" />
                    <FooterStyle CssClass="DataGrid_Footer" />
                    <PagerStyle CssClass="DataGrid_Pager" />
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "AttributeDefinitionId").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <dnn:ImageCommandColumn CommandName="Edit" Text="Edit" ImageURL="~/images/edit.gif"
                            HeaderText="Edit" KeyField="AttributeDefinitionID" EditMode="URL" />
                        <dnn:ImageCommandColumn CommandName="Delete" Text="Delete" ImageURL="~/images/delete.gif"
                            HeaderText="Del" KeyField="AttributeDefinitionID" />
                        <dnn:ImageCommandColumn CommandName="MoveDown" ImageURL="~/images/dn.gif" HeaderText="Dn"
                            KeyField="AttributeDefinitionID" />
                        <dnn:ImageCommandColumn CommandName="MoveUp" ImageURL="~/images/up.gif" HeaderText="Up"
                            KeyField="AttributeDefinitionID" />
                        <dnn:TextColumn DataField="AttributeName" HeaderText="Name" Width="100px" />
                        <%--		<dnn:textcolumn DataField="AttributeCategory" HeaderText="Category" Width="100px" />--%>
                        <asp:TemplateColumn HeaderText="DataType">
                            <ItemStyle Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDataType" runat="server" Text='<%# DisplayDataType((Engage.Dnn.Locator.AttributeDefinition)Container.DataItem) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <dnn:TextColumn DataField="Length" HeaderText="Length" />
                        <dnn:TextColumn DataField="DefaultValue" HeaderText="DefaultValue" Width="100px" />
<%--                        <dnn:TextColumn DataField="ValidationExpression" HeaderText="ValidationExpression"
                            Width="100px" />
                        <dnn:CheckBoxColumn DataField="Required" HeaderText="Required" AutoPostBack="True" />
                        <dnn:CheckBoxColumn DataField="Visible" HeaderText="Visible" AutoPostBack="True" />
--%>                    </Columns>
                </asp:DataGrid>
            </div>
            <br />
            <div class="caAddNew">
                <asp:ImageButton ID="btnCAAdd" runat="server" ToolTip="Click here to add a new custom attribute"
                    AlternateText="Add a new custom attribute" CssClass="CommandButton" ImageUrl="~/desktopmodules/EngageLocator/images/caAddNew.gif"
                    OnClick="btnCAAdd_Click" />
            </div>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<br />
<div class="caNavBt">
    <asp:ImageButton ID="cmdUpdate" runat="server" ToolTip="Click here to update this window"
        AlternateText="Update this entire window" CssClass="CommandButton" 
        ImageUrl="~/desktopmodules/EngageLocator/images/update.gif" 
        onclick="cmdUpdate_Click" />
    <asp:ImageButton ID="cmdCancel" runat="server" ToolTip="Click here to go back to the previous screen"
        AlternateText="Cancel and return to previous screen" CssClass="CommandButton"
        ImageUrl="~/desktopmodules/EngageLocator/images/cancel.gif" 
        onclick="cmdCancel_Click" />
</div>
