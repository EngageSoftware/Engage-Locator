<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.Import" AutoEventWireup="True" CodeBehind="Import.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations." Visible="False" resourcekey="lblConfigured"></asp:Label>
<div class="divPanelTab" id="divPanelTab" runat="server">
    <div class="importPanel" runat="server" id="fileDiv">
        <asp:FileUpload ID="fileImport" runat="server" /><br />
        <asp:Label ID="lblMessage" runat="server" CssClass="Normal"></asp:Label><br />
        <asp:Button ID="btnSubmit" runat="server" CssClass="CommandButton" OnClick="btnSubmit_Click" Text="Submit" resourcekey="btnSubmit" />
        &nbsp; &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="CommandButton" OnClick="btnBack_Click" Text="Cancel" resourcekey="btnBack" />
    </div>
</div>
