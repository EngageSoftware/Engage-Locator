<%@ Control Language="C#" CodeBehind="EditAttributeDefinition.ascx.cs" Inherits="Engage.Dnn.Locator.EditAttributeDefinition" AutoEventWireup="true" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<div class="globalNav">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lbManageLocations_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lbImportFile_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Comments" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lbManageComments_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lbManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" CausesValidation="false" />
</div>
<div class="editeAttributeWrapper">
<fieldset>
    <legend class="Head" id="lgDefinitions" runat="server">Attribute Definition</legend>
        <div id="div_Name" class="eadLabel">
            <p><asp:Label ID="lblName" runat="server" resourceKey="lblName" CssClass="Normal" /></p>
        </div>
        <div id="div_Name_TextBox" class="eadTextBox">
                <p><asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox" /><asp:RequiredFieldValidator ID="rfvName" CssClass="NormalRed" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" /></p>
        </div>
        <div id="div_DefaultValue" class="eadLabel">
            <p><asp:Label ID="lblDefaultValue" runat="server" resourceKey="lblDefaultValue" CssClass="Normal" /></p>
        </div>
        <div id="div_DefaultValue_TextBox" class="eadTextBox">
            <p><asp:TextBox ID="txtDefaultValue" runat="server" CssClass="NormalTextBox" /></p>
        </div>
</fieldset>
<div id="div_commands" class="caNavBt">
    <asp:ImageButton class="CommandButton" ID="cmdUpdate" OnClick="cmdUpdate_Click" ImageUrl="~/desktopmodules/EngageLocator/images/updateBt.gif" ResourceKey="cmdUpdate" runat="server" Text="Update" />
    <asp:ImageButton class="CommandButton" ID="cmdCancel" OnClick="cmdCancel_Click" ImageUrl="~/desktopmodules/EngageLocator/images/cancelBt.gif" ResourceKey="cmdCancel" runat="server" Text="Cancel" CausesValidation="False" />
    <div id="divDelete" runat="server"> 
        <asp:ImageButton class="CommandButton" ID="cmdDelete" OnClick="cmdDelete_Click" ImageUrl="~/desktopmodules/EngageLocator/images/caDelete.gif" ResourceKey="cmdDelete" runat="server" Text="Delete" CausesValidation="False" />
    </div>
</div>
</div>