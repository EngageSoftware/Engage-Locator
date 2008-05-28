<%@ Control Language="C#" AutoEventWireup="True" Inherits="Engage.Dnn.Locator.Settings"
    CodeBehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>

<dnn:sectionhead ID="dshMapProvider" runat="Server" Text="Locator Settings" CssClass="Head"
    Section="tblMapProvider" ResourceKey="dshMapProvider" IsExpanded="True" IncludeRule="true" />
<table id="tblMapProvider" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;"
    border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblMapProviders" ResourceKey="lblMapProviders" runat="server" />
        </td>
        <td width="350">
            <asp:RadioButtonList ID="rblProviderType" CssClass="Normal" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="rblProviderType_SelectedIndexChanged" RepeatDirection="horizontal"
                Width="200px" />
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
            <asp:CustomValidator ID="cvProviderType" runat="server" CssClass="Normal" ErrorMessage="Please select a map provider."
                OnServerValidate="cvProviderType_ServerValidate"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblAPIKey" ResourceKey="lblAPIKey" runat="server" />
        </td>
        <td width="350">
            <asp:TextBox CssClass="Normal" ID="txtApiKey" runat="server" Columns="55" />
            <div>
                <asp:Label ID="lblApiInstructions" CssClass="Normal" runat="server" resourceKey="lblApiInstructions" /></div>
            <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="Normal" ErrorMessage="Invalid API Key"
                OnServerValidate="apiKey_ServerValidate"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblLocatorCountry" runat="server" Visible="true" ResourceKey="lblLocatorCountry"
                CssClass="Normal" />
        </td>
        <td width="350" class="Normal">
            <div>
                <asp:DropDownList ID="ddlLocatorCountry" runat="server" CssClass="Normal" />
            </div>
            <asp:CustomValidator ID="cvLocatorCountry" runat="server" ControlToValidate="ddlLocatorCountry"
                ErrorMessage="Please select a default country." OnServerValidate="cvLocatorCountry_OnServerValidate" />
        </td>
    </tr>
</table>
<dnn:sectionhead ID="dshDisplaySetting" runat="Server" text="Display Settings" CSSClass="Head"
    section="tblDisplaySettings" resourcekey="dshDisplaySetting" IsExpanded="true"
    IncludeRule="true" />
<table id="tblDisplaySettings" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;"
    border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblSearchTitle" ResourceKey="lblSearchTitle" runat="server" Text="Search Instructions:" />
        </td>
        <td width="350">
            <asp:TextBox CssClass="Normal" ID="txtSearchTitle" runat="server" Columns="55" />
            <div>
                <asp:Label CssClass="Normal" ID="lblSearchInst" runat="server" resourceKey="lblSearchInst" /></div>
            <br />
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblDefaultDisplay" runat="server" ResourceKey="lblDefaultDisplay" />
        </td>
        <td width="350">
            <asp:RadioButton ID="rbSearch" runat="server" CssClass="Normal" GroupName="rbDefaultDisplay"
                Text="Search" resourcekey="rbSearch" />
            <asp:RadioButton ID="rbDisplayAll" runat="server" CssClass="Normal" GroupName="rbDefaultDisplay"
                Text="Show Locations" resourcekey="rbDisplayAll" />
            <asp:RadioButton ID="rbShowMap" runat="server" CssClass="Normal" GroupName="rbDefaultDisplay"
                Text="Show Locations & Map" resourcekey="rbShowMap" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblShowLocationDetails" runat="server" ResourceKey="lblShowLocationDetails" />
        </td>
        <td width="350">
            <asp:RadioButton ID="rbSamePage" runat="server" GroupName="rbLocationDetails" Text="Same Page"
                resourcekey="rbSamePage" CssClass="Normal" AutoPostBack="true" OnCheckedChanged="rbLoctionDetails_CheckChanged" />
            <asp:RadioButton ID="rbDetailsPage" runat="server" GroupName="rbLocationDetails"
                Text="Details Page" resourcekey="rbDetailsPage" CssClass="Normal" AutoPostBack="true"
                OnCheckedChanged="rbLoctionDetails_CheckChanged" />
            <asp:RadioButton ID="rbNoDetails" runat="server" GroupName="rbLocationDetails" Text="None"
                resourcekey="rbNoDetails" CssClass="Normal" AutoPostBack="true" OnCheckedChanged="rbLoctionDetails_CheckChanged" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
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
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblLocationComments" runat="server" Text="Allow Location Comments"
                ResourceKey="lblLocationComments" />
        </td>
        <td>
            <asp:CheckBox ID="chkAllowComments" runat="server" Enabled="false" 
                AutoPostBack="True" oncheckedchanged="chkAllowComments_CheckedChanged" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:label ID="lblModerateComments" runat="server" Text="Moderate Comments" ResourceKey="lblModerateComments" />
        </td>
        <td>
            <asp:CheckBox ID="chkModerateComments" runat="server" Enabled="false" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblMapType" runat="server" ResourceKey="lblMapType" />
        </td>
        <td width="350">
            <asp:RadioButtonList ID="rblMapDisplayType" runat="server" CssClass="Normal" RepeatDirection="horizontal">
                <asp:ListItem Text="Normal" Value="Normal" resourcekey="rbMapDisplayNormal" Selected="True" />
                <asp:ListItem Text="Satellite" Value="Satellite" resourcekey="rbMapDisplaySatellite" />
                <asp:ListItem Text="Hybrid" Value="Hybrid" resourcekey="rbMapDisplayHybrid" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblLocationType" ResourceKey="lblLocationType" runat="server" Text="Location Type" />
        </td>
        <td width="350">
            <div id="dvLocationType" runat="server" visible="true">
                <asp:ListBox ID="lbLocationType" runat="server" SelectionMode="Multiple" Width="150px"
                    CssClass="Normal"></asp:ListBox>
            </div>
        </td>
    </tr>
</table>
<dnn:sectionhead ID="dshSearchSettings" runat="Server" text="Search Settings" CSSClass="Head"
    section="tblSearchSettings" resourcekey="tblSearchSettings" IsExpanded="true"
    IncludeRule="true" />
<table id="tblSearchSettings" runat="server" cellspacing="0" cellpadding="0" style="padding-bottom: 20px;"
    border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:Label ID="lblLocatorModules" ResourceKey="lblLocatorModules" runat="server" />
        </td>
        <td width="350">
            <asp:GridView ID="gvTabModules" runat="server" GridLines="vertical" AllowPaging="false"
                AutoGenerateColumns="false" EnableViewState="true" Width="450px">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="10px" ControlStyle-Width="10px">
                        <ItemTemplate>
                            <asp:RadioButton ID="rbLocatorModule" runat="server" AutoPostBack="true" GroupName="rbLocatorModules"
                                OnCheckedChanged="rbLocatorModules_CheckChanged" CssClass="Normal" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Page Title" ControlStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblPageTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title", "{0:d}") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="Normal" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TabId" HeaderStyle-Width="50px" ControlStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblTabId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TabId", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Normal" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Module Title" ControlStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblModuleTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ModuleTitle", "{0:d}") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Normal" Width="200px" />
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#eeeeee" />
                <RowStyle BackColor="#f8f8f8" ForeColor="Black" />
            </asp:GridView>
            <asp:CustomValidator ID="cvLocatorModules" runat="server" CssClass="Normal" ErrorMessage="Please select a results display module."
                OnServerValidate="cvLocatorModules_ServerValidate"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <dnn:Label ID="lblSearchOptions" runat="server" Text="Search Options" ResourceKey="lblSearchOptions" />
        </td>
        <td>
            <table class="Normal">
                <tr>
                    <td>
                        <asp:CheckBox ID="chkAddress" runat="server" Text="Address" resourcekey="chkAddress" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCityRegion" runat="server" Text="City & Region" resourcekey="chkCityState" />
                        <asp:CheckBox ID="chkPostalCode" runat="server" Text="Postal Code" Checked="true" resourcekey="chkPostalCode" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCountry" runat="server" Text="Country" resourcekey="chkCountry" /></p>
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvSearchOptions" runat="server" CssClass="Normal" ErrorMessage="You must select at least one search option."
                OnServerValidate="cvSearchOptions_ServerValidate" resourcekey="cvSearchOptions" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label id="lblOptional" resourcekey="lblOptional" runat="server" Text="Optional Settings" />
        </td>
        <td width="350">
            <div id="dvAddress">
                <asp:RadioButtonList ID="rblRestrictions" runat="server" CssClass="Normal" RepeatDirection="horizontal">
                    <asp:ListItem Text="Country" Value="Country" resourcekey="liSearchRestrictionCountry" />
                    <asp:ListItem Text="Radius" Value="Radius" resourcekey="liSearchRestrictionRadius" />
                    <asp:ListItem Text="None" Value="None" resourcekey="liSearchRestrictionNone" />
                </asp:RadioButtonList>
            </div>
            <div>
                <asp:Label ID="lblOptionalInst" CssClass="Normal" runat="server" resourceKey="lblOptionalInst" />
            </div>
        </td>
    </tr>
</table>
<dnn:sectionhead ID="dshSubmissionSettings" runat="Server" text="Location Submission Settings"
    CSSClass="Head" section="tblSubmissionSettings" resourcekey="dshSubmissionSettings"
    IsExpanded="true" IncludeRule="true" />
<table id="tblSubmissionSettings" runat="server" cellspacing="0" cellpadding="0"
    style="padding-bottom: 20px;" border="0" summary="Module Mode">
    <tr>
        <td class="SubHead" width="150" valign="top">
            <dnn:label ID="lblAllowSubmissions" runat="server" ResourceKey="lblAllowSubmissions" />
        </td>
        <td width="350">
            <asp:CheckBox ID="chkAllowLocations" runat="server" AutoPostBack="True" 
                oncheckedchanged="chkAllowLocations_CheckedChanged" />
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
