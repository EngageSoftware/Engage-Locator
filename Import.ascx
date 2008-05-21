<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.Import" AutoEventWireup="True" CodeBehind="Import.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<div class="div_ManagementButtons">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsbt.gif" OnClick="lbSettings_OnClick" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lblManageLocations_OnClick" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lblManageComments_OnClick" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lblManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" />
    
</div>
<br />
<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations." Visible="False" resourcekey="lblConfigured"></asp:Label>
<div class="divPanelTab" id="divPanelTab" runat="server">
    <div class="importPanel" runat="server" id="fileDiv">
        <asp:FileUpload ID="fileImport" runat="server" /><br />
        <asp:Label ID="lblMessage" runat="server" CssClass="Normal"></asp:Label><br />
        <asp:Button ID="btnSubmit" runat="server" CssClass="CommandButton" OnClick="btnSubmit_Click" Text="Submit" resourcekey="btnSubmit" />
        &nbsp; &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="CommandButton" OnClick="btnBack_Click" Text="Cancel" resourcekey="btnBack" />
    </div>
</div>
