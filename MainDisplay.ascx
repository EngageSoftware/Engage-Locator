<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.MainDisplay" AutoEventWireup="False" CodeBehind="MainDisplay.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/sectionheadcontrol.ascx" %>
<div class="div_ManagementButtons">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lbManageLocations_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lbImportFile_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Comments" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lbManageComments_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lbManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" CausesValidation="false" />
</div>
<div class="locatorHeader">
    <h1 class="locatorHeaderText">
        <asp:Label ID="HeaderLabel" runat="server" EnableViewState="false" />
    </h1>
</div>
<asp:MultiView ID="LocatorDisplayMultiView" runat="server" ActiveViewIndex="0">
    <asp:View ID="SetupView" runat="server">
        <asp:Panel ID="SetupPanel" runat="server" CssClass="locatorSetupPage">
            <asp:Label ID="SetupLabel" runat="server" CssClass="Normal" />
        </asp:Panel>
    </asp:View>
    <asp:View ID="SearchView" runat="server">
        <asp:Panel ID="SearchPanel" runat="server" CssClass="locatorLandingPage" DefaultButton="SearchSubmitButton">
            <div class="locatorBody">
                <asp:Panel ID="SearchErrorPanel" runat="server" CssClass="locatorErrorPanel" Visible="false">
                    <h4 class="locatorErrorHeader">
                        <asp:Label runat="server" ResourceKey="Please refine your search.Text" /></h4>
                    <div>
                        <asp:Label ID="ErrorMessageLabel" runat="server" CssClass="NormalRed" />
                    </div>
                </asp:Panel>
                <asp:Label ID="SearchTitleLabel" runat="server" CssClass="SubHead" />
                <div class="b001">
                    <div class="f02">
                        <asp:Panel ID="SearchAddressPanel" runat="server" CssClass="addressPanel">
                            <div class="LocatorInput">
                                <div id="AddressFirstLineSection" runat="server" class="addressFirstLine">
                                    <div>
                                        <p class="Normal">
                                            <asp:Label ID="SearchAddressLabel" runat="server" resourcekey="lblLocationAddress" CssClass="Normal" />
                                        </p>
                                        <p class="LocatorInputText">
                                            <asp:TextBox ID="SearchAddressTextBox" runat="server" MaxLength="80" />
                                        </p>
                                    </div>
                                </div>
                                <div class="addressSecondLine">
                                    <div id="SearchCitySection" runat="server" class="ltCity">
                                        <p class="Normal">
                                            <asp:Label ID="SearchCityLabel" runat="server" resourcekey="lblLocationCity" CssClass="Normal" />
                                        </p>
                                        <p class="LocatorInputText">
                                            <asp:TextBox ID="SearchCityTextBox" runat="server" MaxLength="80" />
                                        </p>
                                    </div>
                                    <div id="SearchRegionSection" runat="server" class="ltRegion">
                                        <p class="Normal">
                                            <asp:Label ID="SearchRegionLabel" runat="server" resourcekey="lblLocationRegion" CssClass="Normal" />
                                        </p>
                                        <p class="ltDropDownList">
                                            <asp:DropDownList ID="SearchRegionDropDownList" runat="server" CssClass="Normal" />
                                        </p>
                                    </div>
                                    <div id="SearchPostalCodeSection" runat="server" class="ltPostalCode">
                                        <p class="Normal">
                                            <asp:Label ID="SearchPostalCodeLabel" runat="server" resourcekey="lblLocationPostalCode" CssClass="Normal" />
                                        </p>
                                        <p class="LocationPostalCode">
                                            <asp:TextBox ID="SearchPostalCodeTextBox" runat="server" MaxLength="15" CssClass="LocationPostalCode" />
                                        </p>
                                    </div>
                                </div>
                                <div class="addressThirdLine">
                                    <div id="SearchCountrySection" runat="server">
                                        <p class="Normal">
                                            <asp:Label ID="SearchCountryLabel" runat="server" resourcekey="lblLocatorCountry" />
                                        </p>
                                        <p class="ltCountry">
                                            <asp:DropDownList ID="SearchCountryDropDownList" runat="server" CssClass="Normal" />
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="SearchRadiusPanel" runat="server" CssClass="groupingPanel" GroupingText='<%# Localization.GetString("pnlDistance", LocalResourceFile) %>'>
                            <table class="inputTable">
                                <tr>
                                    <td>
                                        <asp:Label ID="SearchRadiusLabel" runat="server" CssClass="Normal" resourcekey="SearchRadiusLabel"/>
                                        <asp:DropDownList ID="SearchRadiusDropDownList" runat="server" CssClass="ddlDistance">
                                            <asp:ListItem Text="5" />
                                            <asp:ListItem Text="10" />
                                            <asp:ListItem Text="25" />
                                            <asp:ListItem Text="50" />
                                            <asp:ListItem Text="100" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="FilterCountryPanel" runat="server" CssClass="groupingPanel" GroupingText='<%# Localization.GetString("pnlCountry", LocalResourceFile) %>'>
                            <table class="inputTable">
                                <tr>
                                    <td class="locatorLabelCell">
                                        <asp:Label ID="FilterCountryLabel" runat="server" CssClass="Normal" AssociatedControlID="FilterCountryDropDownList" resourcekey="lblCountry"/>
                                        <asp:DropDownList ID="FilterCountryDropDownList" runat="server" CssClass="ddlCountry"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:Panel>
        &nbsp;
        <div class="LocatorSubmitButton">
            <asp:Button ID="SearchSubmitButton" runat="server" Text="Search" resourcekey="btnSubmit" />
            <asp:Button ID="SubmitLocationFromSearchButton" runat="server" resourcekey="lnkSubmitLocations" />
        </div>
    </asp:View>
    <asp:View ID="LocationsListView" runat="server">
        <%--<asp:Button ID="lnkViewMap" Text="View All Locations" runat="server" Visible="false" resourcekey="lnkViewMap" OnClientClick="showAllLocations(); return false;" />--%>
        <asp:Button ID="NewSearchButton" runat="server" CssClass="LocatorMapItButton" resourcekey="btnBack2" />
        <asp:Button ID="ShowAllLocationsButton" runat="server" CssClass="LocatorMapItbutton" resourcekey="btnShowAll" />
        <asp:Button ID="SubmitLocationFromListButton" runat="server" Text="Submit A Location" Visible="false" resourcekey="lnkSubmitLocations" />
        <div class="locationMapHeading">
            <div class="closestsLocations">
                <asp:Label ID="CurrentLocationsLabel" runat="server" CssClass="Normal" resourcekey="lblNumClosest" />
            </div>
            <div class="closestLocationTitle">
                <a name="map"></a>
                <asp:Label ID="LocatorMapLabel" runat="server" />
                <asp:Label ID="CurrentLocationLabel" runat="server" CssClass="SubHead" />
            </div>
            <div class="locatorMapLabelWrapper">
                <div style="display: none;">
                    <a target="_blank">
                        <asp:Label runat="server" resourcekey="lblMapLinkMapName" />
                    </a>
                    <asp:Label runat="server" resourcekey="lblMapLinkDrivingDirections" />
                </div>
            </div>
        </div>
        <div id="MapSection" runat="server" class="locatorMap" style="display: none;">
        </div>
        <div class="locatorMapLabelWrapper Normal">
            <asp:Label ID="ScrollToViewMoreLabel" runat="server" resourcekey="lblScrollToViewMore" Style="display: none;" />
            <asp:Panel ID="MapLinkPanel" runat="server" Style="display: none;">
                <asp:HyperLink ID="DrivingDirectionsLink" runat="server" Target="_blank">
                    <asp:Label ID="MapLinkMapNameLabel" runat="server" resourcekey="lblMapLinkMapName" />
                </asp:HyperLink>
                <asp:Label ID="MapLinkDrivingDirectionsLabel" runat="server" resourcekey="lblMapLinkDrivingDirections" />
            </asp:Panel>
        </div>
        <br />
        <div class="locationsGridWrapper">
            <table class="titleHeading">
                <asp:Repeater ID="LocationsListRepeater" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th class="mdLocationHeading">
                                <asp:Label runat="server" resourcekey="lblLocationHeader" />
                            </th>
                            <th id="thDistance" class="mdDistanceHeading">
                                <asp:Label runat="server" resourcekey="lblStoreDistance" />
                            </th>
                            <% if(ShowLocationDetails == "True" || ShowLocationDetails == "SamePage") { %><th class="mdLDHeading">
                                <asp:Label ID="LocationDetailsLabel" runat="server" resourcekey="lblLocationDetails" />
                            </th>
                            <% } %>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="locationEntryTR">
                            <td class="mdLocation">
                                <table class="mdLocationDetailTable">
                                    <tr>
                                        <td class="mdLocationNumber">
                                            <p>
                                                <asp:Label CssClass="NormalBold" runat="server" Text='<%#(this.Eval("QueryIndex") ?? Container.ItemIndex) + ")" %>' />
                                            </p>
                                        </td>
                                        <td class="mdLocationInfo">
                                            <p>
                                                <asp:HyperLink ID="lnkLocationName" runat="server" CssClass="SubHead locatorName" Target="_blank" NavigateUrl='<%# Eval("Website") %>' Text='<%# Server.HtmlEncode(Eval("Name").ToString())%>' />
                                            </p>
                                            <asp:Label ID="LocationsGridAddressLabel" runat="server" CssClass="Normal locatorAddress"/>
                                            <asp:Label runat="server" CssClass="Normal locatorCity" Text='<%# Server.HtmlEncode(Eval("City").ToString()) + ","%>'/>
                                            <asp:Label runat="server" CssClass="Normal locatorState" Text='<%# Server.HtmlEncode(Eval("RegionAbbreviation").ToString())%>' />
                                            <asp:Label runat="server" CssClass="Normal locatorPostalCode" Text='<%# Server.HtmlEncode(Eval("PostalCode").ToString())%>' />
                                            <asp:Label runat="server" CssClass="Normal locatorPhone" Text='<%# Server.HtmlEncode(Eval("Phone").ToString())%>' />
                                            <div class="mdViewDetail">
                                                <asp:HyperLink ID="ShowLocationDetailsLink" CssClass="Normal" runat="server" resourcekey="lnkShowLocationDetails" Visible="false" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="mdDistance Normal">
                                <%--<asp:Label ID="lblWalkIn" runat="server"/>--%>
                                <asp:Label ID="LocationsGridDistanceLabel" runat="server" /><br />
                                <asp:Label runat="server" resourcekey="LocationDisplayHeaders" CssClass="hideCurrentlyMapped" />
                                <asp:Panel runat="server" CssClass="locationsGridDescriptionPopup" Style="display: none"></asp:Panel>
                                <div class="viewMapBt">
                                    <asp:LinkButton runat="server" resourcekey="lnkMapIt" />
                                </div>
                            </td>
                            <% if(ShowLocationDetails == "True" || ShowLocationDetails == "SamePage") { %>
                            <td class="locationDetailsTitleTD">
                                <asp:Label ID="LocationDetailsLabel" runat="server" CssClass="Normal" Text='<%# Eval("LocationDetails")%>' />
                            </td>
                            <% } %>
                            <%--<td class="mdLocationDetail Normal">
                                <div id="div_SiteLink">
                                    <br />
                                    <asp:HyperLink ID="SiteLink" Visible="false" runat="server" CssClass="Normal" NavigateUrl='<%# Eval("Website")%>'/>
                                </div>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div class="locationsGridPaging Normal">
            <asp:HyperLink ID="PreviousPageLink" runat="server" CssClass="PreviousPage" ResourceKey="PreviousPageLink.Text" />
            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="CurrentPage" />
            <asp:HyperLink ID="NextPageLink" runat="server" CssClass="NextPage" ResourceKey="NextPageLink.Text" />
        </div>
    </asp:View>
</asp:MultiView>