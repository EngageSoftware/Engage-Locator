<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.Import" AutoEventWireup="True"
    CodeBehind="Import.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<div class="globalNav">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lbManageLocations_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lbImportFile_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Comments" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lbManageComments_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lbManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" CausesValidation="false" />
</div>
<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations."
    Visible="False" resourcekey="lblConfigured"></asp:Label>
    <fieldset>
    <legend class="Head" id="lgImport" runat="server">Import File</legend>
    <div class="importInstruction"><asp:Label ID="lblInstructions" runat="server" CssClass="Normal" Text="Click the browse button to locate a .csv file to import your locations." resourceKey="lblInstructions"></asp:Label></div>
    <div class="divPanelTab" id="divPanelTab" runat="server">
        <div class="importPanel" runat="server" id="fileDiv">
            <div class="importUploader"><asp:FileUpload ID="fileImport" runat="server" /></div>
            <div class="importError"><asp:Label ID="lblMessage" runat="server" CssClass="NormalRed"></asp:Label></div>
        </div>
    </fieldset>
<div id="div_navigation" class="btNav">
    <asp:ImageButton ID="btnSubmit" runat="server" CssClass="CommandButton" OnClick="btnSubmit_Click"
        ToolTip="Select a file and click Submit to queue your file for import." AlternateText="Submit"
        ImageUrl="~/desktopmodules/EngageLocator/images/submitBt.gif" />
    <asp:ImageButton ID="btnBack" runat="server" CssClass="CommandButton" OnClick="btnBack_Click"
        ToolTip="Click here to go back to the previous screen." AlternateText="Cancel"
        ImageUrl="~/desktopmodules/EngageLocator/images/back.gif" />
</div>
</div>