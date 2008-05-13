<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="AttributeDefinitions.ascx.cs" Inherits="Engage.Dnn.Locator.AttributeDefinitions" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div style="text-align:left;">
    <asp:Label id="lblLocationTypeHelp" runat="Server" class="normal" resourcekey="LocationTypeAttributesHelp" />
</div>
<br />
<div style="text-align:center;">
    <p></p><asp:Label id="lblLocTypeName" runat="server" class="normal" resourcekey="LocTypeName" /><asp:TextBox id="txtLocationTypeName" runat="server" /></p>
</div>
<asp:datagrid id="grdLocationTypeAttributes" AutoGenerateColumns="false" width="100%" CellPadding="4"
	GridLines="None" cssclass="DataGrid_Container" Runat="server">
	<headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="Center" />
	<itemstyle cssclass="DataGrid_Item" horizontalalign="Left" />
	<alternatingitemstyle cssclass="DataGrid_AlternatingItem" />
	<edititemstyle cssclass="NormalTextBox" />
	<selecteditemstyle cssclass="NormalRed" />
	<footerstyle cssclass="DataGrid_Footer" />
	<pagerstyle cssclass="DataGrid_Pager" />
	<columns>
		<dnn:imagecommandcolumn CommandName="Edit" Text="Edit" ImageUrl="~/images/edit.gif" HeaderText="Edit" KeyField="AttributeDefinitionID" EditMode="URL" />
		<dnn:imagecommandcolumn CommandName="Delete" Text="Delete" ImageUrl="~/images/delete.gif" HeaderText="Del" KeyField="AttributeDefinitionID" />
		<dnn:imagecommandcolumn CommandName="MoveDown" ImageUrl="~/images/dn.gif" HeaderText="Dn" KeyField="AttributeDefinitionID" />
		<dnn:imagecommandcolumn CommandName="MoveUp" ImageUrl="~/images/up.gif" HeaderText="Up" KeyField="AttributeDefinitionID" />
		<dnn:textcolumn DataField="AttributeName" HeaderText="Name" Width="100px" />
<%--		<dnn:textcolumn DataField="AttributeCategory" HeaderText="Category" Width="100px" />--%>
		<asp:TemplateColumn HeaderText="DataType">
			<ItemStyle Width="100px"></ItemStyle>
			<ItemTemplate>
				<asp:label id="lblDataType" runat="server" Text='<%# DisplayDataType((Engage.Dnn.Locator.LocationTypeAttributeDefinition)Container.DataItem) %>'></asp:label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<dnn:textcolumn DataField="Length" HeaderText="Length" />
		<dnn:textcolumn DataField="DefaultValue" HeaderText="DefaultValue" Width="100px" />
		<dnn:textcolumn DataField="ValidationExpression" HeaderText="ValidationExpression" Width="100px" />
		<dnn:checkboxcolumn DataField="Required" HeaderText="Required" AutoPostBack="True" />
		<dnn:checkboxcolumn DataField="Visible" HeaderText="Visible" AutoPostBack="True" />
	</columns>
</asp:datagrid>
<br>
<br>
<p style="text-align:center">
	<dnn:commandbutton class="CommandButton" id="cmdUpdate" imageUrl="~/images/save.gif" resourcekey="cmdApply"
		runat="server" text="Apply Changes" />&nbsp;
	<dnn:commandbutton class="CommandButton" id="cmdRefresh" imageUrl="~/images/refresh.gif" resourcekey="cmdRefresh"
		runat="server" text="Refresh" />&nbsp;
</p>
