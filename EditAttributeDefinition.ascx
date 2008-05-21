<%@ Control Language="C#" CodeBehind="EditAttributeDefinition.ascx.cs" Inherits="Engage.Dnn.Locator.EditAttributeDefinition" AutoEventWireup="true" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<fieldset>
    <legend class="Head" id="lgDefinitions" runat="server">Attribute Definition</legend>
        <div id="div_Name">
            <asp:Label ID="lblName" runat="server" resourceKey="lblName" CssClass="Normal" ></asp:Label>
            <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox"></asp:TextBox><asp:RequiredFieldValidator ID="rfvName" CssClass ="Normal" runat="server" ControlToValidate ="txtName" Display="Dynamic" ErrorMessage="Name is required"></asp:RequiredFieldValidator>
        </div>
        <div id="div_DefaultValue">
            <asp:Label ID="lblDefaultValue" runat="server" resourceKey="lblDefaultValue" CssClass="Normal"></asp:Label>
            <asp:TextBox ID="txtDefaultValue" runat="server" CssClass="NormalTextBox"></asp:TextBox>
        </div>
</fieldset>
<br />
<div id="div_commands">
    <asp:ImageButton class="CommandButton" ID="cmdUpdate" OnClick="cmdUpdate_Click" ImageUrl="~/desktopmodules/EngageLocator/images/Update.gif" ResourceKey="cmdUpdate" runat="server" Text="Update" />
    <asp:ImageButton class="CommandButton" ID="cmdCancel" OnClick="cmdCancel_Click" ImageUrl="~/desktopmodules/EngageLocator/images/Cancel.gif" ResourceKey="cmdCancel" runat="server" Text="Cancel" CausesValidation="False" />
    <div id="divDelete" runat="server"> 
        <asp:ImageButton class="CommandButton" ID="cmdDelete" OnClick="cmdDelete_Click" ImageUrl="~/desktopmodules/EngageLocator/images/caDelete.gif" ResourceKey="cmdDelete" runat="server" Text="Delete" CausesValidation="False" />
    </div>
</div>