<%@ Control Language="C#" AutoEventWireup="True" Inherits="Engage.Dnn.Locator.Settings" CodeBehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%-- TODO: Localize table summaries --%>
<dnn:sectionhead ID="dshMapProvider" runat="Server" CssClass="Head" Section="tblMapProvider" ResourceKey="dshMapProvider" IsExpanded="True" IncludeRule="true" />
<table id="tblMapProvider" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;" border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblMapProviders" ResourceKey="lblMapProviders" runat="server" />
        </td>
        <td width="350">
            <asp:RadioButtonList ID="rblProviderType" CssClass="Normal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProviderType_SelectedIndexChanged" RepeatDirection="horizontal" Width="200px" />
            <div>
                <asp:Label ID="lblApiMapProvider" CssClass="Normal" runat="server" resourceKey="lblApiMapProvider" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            &nbsp;
        </td>
        <td width="350">
            <asp:CustomValidator ID="ProviderTypeValidator" runat="server" CssClass="Normal" ResourceKey="ProviderTypeValidator" OnServerValidate="ProviderTypeValidator_ServerValidate"/>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblAPIKey" ResourceKey="lblAPIKey" runat="server" />
        </td>
        <td width="350">
            <asp:TextBox CssClass="Normal" ID="txtApiKey" runat="server" Columns="55" />
            <div>
                <asp:Label ID="lblApiInstructions" CssClass="Normal" runat="server" resourceKey="lblApiInstructions" />
            </div>
            <asp:CustomValidator ID="ApiKeyValidator" runat="server" CssClass="Normal" resourcekey="ApiKeyValidator" OnServerValidate="apiKey_ServerValidate"/>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblLocatorCountry" runat="server" Visible="true" ResourceKey="lblLocatorCountry" />
        </td>
        <td width="350" class="Normal">
            <div>
                <asp:DropDownList ID="ddlLocatorCountry" runat="server" CssClass="Normal" />
            </div>
            <asp:CustomValidator ID="LocatorCountryValidator" runat="server" ControlToValidate="ddlLocatorCountry" ResourceKey="LocatorCountryValidator" OnServerValidate="LocatorCountryValidator_ServerValidate" />
        </td>
    </tr>
</table>
<dnn:sectionhead ID="dshDisplaySetting" runat="Server" CSSClass="Head" section="tblDisplaySettings" resourcekey="dshDisplaySetting" IsExpanded="true" IncludeRule="true" />
<table id="tblDisplaySettings" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;" border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblSearchTitle" ResourceKey="lblSearchTitle" runat="server" />
        </td>
        <td width="350">
            <asp:TextBox CssClass="Normal" ID="txtSearchTitle" runat="server" Columns="55" />
            <div>
                <asp:Label CssClass="Normal" ID="lblSearchInst" runat="server" resourceKey="lblSearchInst" />
            </div>
            <br />
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblDefaultDisplay" runat="server" ResourceKey="lblDefaultDisplay" />
        </td>
        <td width="350">
            <asp:RadioButton ID="rbSearch" runat="server" CssClass="Normal" GroupName="rbDefaultDisplay" resourcekey="rbSearch" />
            <asp:RadioButton ID="rbDisplayAll" runat="server" CssClass="Normal" GroupName="rbDefaultDisplay" resourcekey="rbDisplayAll" />
            <asp:RadioButton ID="rbShowMap" runat="server" CssClass="Normal" GroupName="rbDefaultDisplay" resourcekey="rbShowMap" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblShowLocationDetails" runat="server" ResourceKey="lblShowLocationDetails" />
        </td>
        <td width="350">
            <asp:RadioButton ID="rbSamePage" runat="server" GroupName="rbLocationDetails" resourcekey="rbSamePage" CssClass="Normal" AutoPostBack="true" OnCheckedChanged="rbLoctionDetails_CheckChanged" />
            <asp:RadioButton ID="rbDetailsPage" runat="server" GroupName="rbLocationDetails" resourcekey="rbDetailsPage" CssClass="Normal" AutoPostBack="true" OnCheckedChanged="rbLoctionDetails_CheckChanged" />
            <asp:RadioButton ID="rbNoDetails" runat="server" GroupName="rbLocationDetails" resourcekey="rbNoDetails" CssClass="Normal" AutoPostBack="true" OnCheckedChanged="rbLoctionDetails_CheckChanged" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="LocationsPerPageLabel" runat="server" ResourceKey="lblLocationsPerPage" />
        </td>
        <td width="350">
            <asp:TextBox ID="LocationsPerPageTextBox" runat="server" CssClass="NormalTextBox" />
            <asp:CompareValidator runat="server" ControlToValidate="LocationsPerPageTextBox" CssClass="Normal" ResourceKey="LocationsPerPageIntegerValidator" Type="Integer" Operator="DataTypeCheck" />
        </td>
    </tr>    
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblLocationRating" runat="server" ResourceKey="lblLocationRating" />
        </td>
        <td>
            <asp:CheckBox ID="cbLocationRating" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblLocationComments" runat="server" ResourceKey="lblLocationComments" />
        </td>
        <td>
            <asp:CheckBox ID="chkAllowComments" runat="server" Enabled="false" AutoPostBack="True" oncheckedchanged="chkAllowComments_CheckedChanged" />
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblModerateComments" runat="server" ResourceKey="lblModerateComments" />
        </td>
        <td>
            <asp:CheckBox ID="chkModerateComments" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblMapType" runat="server" ResourceKey="lblMapType" />
        </td>
        <td width="350">
            <asp:RadioButtonList ID="rblMapDisplayType" runat="server" CssClass="Normal" RepeatDirection="horizontal">
                <asp:ListItem Value="Normal" resourcekey="rbMapDisplayNormal" />
                <asp:ListItem Value="Satellite" resourcekey="rbMapDisplaySatellite" />
                <asp:ListItem Value="Hybrid" resourcekey="rbMapDisplayHybrid" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblLocationType" ResourceKey="lblLocationType" runat="server" />
        </td>
        <td width="350">
            <div id="dvLocationType" runat="server" visible="true">
                <asp:ListBox ID="lbLocationType" runat="server" SelectionMode="Multiple" Width="150px" CssClass="Normal" />
            </div>
        </td>
    </tr>
</table>
<dnn:sectionhead ID="dshSearchSettings" runat="Server" CSSClass="Head" section="tblSearchSettings" resourcekey="tblSearchSettings" IsExpanded="true" IncludeRule="true" />
<table id="tblSearchSettings" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;" border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:Label ID="lblLocatorModules" ResourceKey="lblLocatorModules" runat="server" />
        </td>
        <td width="350">
            <asp:GridView ID="gvTabModules" runat="server" GridLines="vertical" AllowPaging="false" AutoGenerateColumns="false" EnableViewState="true" Width="450px">
                <Columns>
                    <asp:TemplateField HeaderText="Page Title" ControlStyle-Width="60px">
                        <ItemTemplate>
                            <asp:RadioButton ID="LocatorModuleRadioButton" runat="server" AutoPostBack="true" GroupName="LocatorModuleRadioButtons" OnCheckedChanged="LocatorModuleRadioButtons_CheckChanged" CssClass="Normal" Text='<%# Eval("Title") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="Normal" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TabId" HeaderStyle-Width="50px" ControlStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblTabId" runat="server" Text='<%# Eval("TabId") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="Normal" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Module Title" ControlStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblModuleTitle" runat="server" Text='<%# Eval("ModuleTitle") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="Normal" Width="200px" />
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#eeeeee" />
                <RowStyle BackColor="#f8f8f8" ForeColor="Black" />
            </asp:GridView>
            <asp:CustomValidator ID="SearchResultsModuleValidator" runat="server" CssClass="Normal" ResourceKey="SearchResultsModuleValidator" OnServerValidate="SearchResultsModuleValidator_ServerValidate"/>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:Label ID="lblSearchOptions" runat="server" ResourceKey="lblSearchOptions" />
        </td>
        <td>
            <table class="Normal">
                <tr>
                    <td>
                        <asp:CheckBox ID="chkAddress" runat="server" resourcekey="chkAddress" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCityRegion" runat="server" resourcekey="chkCityState" />
                        <asp:CheckBox ID="chkPostalCode" runat="server" Checked="true" resourcekey="chkPostalCode" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCountry" runat="server" resourcekey="chkCountry" />
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="SearchOptionsValidator" runat="server" CssClass="Normal" OnServerValidate="SearchOptionsValidator_ServerValidate" resourcekey="cvSearchOptions" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label id="lblOptional" resourcekey="lblOptional" runat="server" />
        </td>
        <td width="350">
            <div id="dvAddress">
                <asp:RadioButtonList ID="rblRestrictions" runat="server" CssClass="Normal" RepeatDirection="horizontal">
                    <asp:ListItem Value="Country" resourcekey="liSearchRestrictionCountry" />
                    <asp:ListItem Value="Radius" resourcekey="liSearchRestrictionRadius" />
                    <asp:ListItem Value="None" resourcekey="liSearchRestrictionNone" />
                </asp:RadioButtonList>
            </div>
            <div>
                <asp:Label ID="lblOptionalInst" CssClass="Normal" runat="server" resourceKey="lblOptionalInst" />
            </div>
        </td>
    </tr>
</table>
<dnn:sectionhead ID="dshSubmissionSettings" runat="Server" CSSClass="Head" section="tblSubmissionSettings" resourcekey="dshSubmissionSettings" IsExpanded="true" IncludeRule="true" />
<table id="tblSubmissionSettings" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;" border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblAllowSubmissions" runat="server" ResourceKey="lblAllowSubmissions" />
        </td>
        <td width="350">
            <asp:CheckBox ID="chkAllowLocations" runat="server" AutoPostBack="True" oncheckedchanged="chkAllowLocations_CheckedChanged" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblSubmissionModeration" runat="server" ResourceKey="lblSubmissionModeration" />
        </td>
        <td width="350">
            <asp:CheckBox ID="chkModerateLocations" runat="server" Enabled="False" />
        </td>
    </tr>
</table>
