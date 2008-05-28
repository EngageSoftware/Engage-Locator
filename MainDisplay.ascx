<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.MainDisplay" AutoEventWireup="True" Codebehind="MainDisplay.ascx.cs" %>
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
        <asp:Label ID="lblHeader" runat="server" EnableViewState="false" resourcekey="lblHeader" Text="lblHeader" /></h1>
</div>

<asp:MultiView ID="mvwLocator" runat="server" ActiveViewIndex="0">
    <asp:View ID="vwSetup" runat="server">
        <asp:Panel ID="pnlSetup" runat="server" CssClass="locatorSetupPage">
            <asp:Label ID="lblSetupText" runat="server" CssClass="Normal" />
        </asp:Panel>
    </asp:View>
    <asp:View ID="vwLocator" runat="server">
        <asp:Panel ID="pnlLocator" runat="server" CssClass="locatorLandingPage" DefaultButton="btnSubmit">
            <div class="locatorBody">
            <asp:Panel ID="pnlError" runat="server" CssClass="locatorErrorPanel" Visible="false">
                <h4 class="locatorErrorHeader">Please refine your search.</h4>
                <div>
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="NormalRed" />
                </div>
            </asp:Panel>
                <asp:Label ID="lblSearchTitle" runat="server" CssClass="SubHead" resourcekey="lblSearchTitle" />
                <div class="b001">
                    <div class="f02">
                        <asp:Panel ID="pnlAddress" runat="server" CssClass="addressPanel">
                                <div class="LocatorInput">
                                    <div id="addressFirstLine" runat="server" class="addressFirstLine">
                                        <div id="ltAddress" runat="server">
                                            <p class="Normal" ><asp:Label ID="lblLocationAddress" runat="server" Text="Address" resourcekey="lblLocationAddress" CssClass="Normal" /></p>
                                            <p class="LocatorInputText"><asp:TextBox ID="txtLocationAddress" runat="server" MaxLength="80" /></p>
                                        </div>
                                    </div>
                                    
                                    <div class="addressSecondLine">

                                        <div id="ltCity" runat="server" class="ltCity">
                                            <p class="Normal"><asp:Label ID="lblLocationCity" runat="server" Text="City" resourcekey="lblLocationCity" CssClass="Normal" /></p>
                                            <p class="LocatorInputText"><asp:TextBox ID="txtLocationCity" runat="server" MaxLength="80" /></p>
                                        </div>
                                        
                                        <div id="ltRegion" runat="server" class="ltRegion">
                                            <p class="Normal"><asp:Label ID="lblLocationState" runat="server" Text="Region" resourcekey="lblLocationRegion" CssClass="Normal" /></p>
                                            <p class="ltDropDownList"><asp:DropDownList ID="ddlLocationRegion" runat="server" CssClass="Normal" /></p>
                                        </div>
                                        
                                        <div id="ltPostalcode" runat="server" class="ltPostalCode">
                                            <p class="Normal"><asp:Label ID="lblLocationPostalCode" runat="server" Text="Postal Code" resourcekey="lblLocationPostalCode" CssClass="Normal" /></p>
                                            <p class="LocationPostalCode"><asp:TextBox ID="txtLocationPostalCode" runat="server" MaxLength="15" CssClass="LocationPostalCode" /></p>                                        
                                        </div>
                                        
                                    </div>
                                    
                                    <div class="addressThirdLine">
                                        <div id="ltCountry" runat="server">
                                            <p class="Normal"><asp:Label ID="lblLocatorCountry" runat="server" Text="Country" resourcekey="lblLocatorCountry" /> </p>
                                            <p class="ltCountry"><asp:DropDownList ID="ddlLocatorCountry" runat="server" CssClass="Normal" /></p>
                                        </div>
                                    </div>

                                    
                                </div>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlDistance" runat="server" CssClass="groupingPanel" GroupingText='<%# Localization.GetString("pnlDistance", LocalResourceFile) %>'>
                            <table class="inputTable">
                            <tr>
                            	<td>
                                <asp:Label ID="lblDistance" runat="server" CssClass="Normal" resourcekey="lblDistance">Find locations within:</asp:Label>
                                <asp:DropDownList ID="ddlDistance" runat="server" CssClass="ddlDistance" AutoPostBack="true" OnSelectedIndexChanged="ddlDistance_SelectedIndexChanged" >
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
                        <% if (ShowCountry && ShowRadius) { %><table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblOr" runat="server" Text="OR" CssClass="Normal" ></asp:Label>
                                </td>
                            </tr>
                        </table><% } %>
                        <asp:Panel ID="pnlCountry" runat="server" CssClass="groupingPanel" GroupingText='<%# Localization.GetString("pnlCountry", LocalResourceFile) %>'>
                            <table class="inputTable">
                            <tr><td class="locatorLabelCell">
                                <asp:Label ID="lblCountry" runat="server" CssClass="Normal" AssociatedControlID="ddlCountry" resourcekey="lblCountry">Country:</asp:Label>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="ddlCountry" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />
                            </td></tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:Panel>
        &nbsp;
        <div class="LocatorSubmitButton">
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Search" resourcekey="btnSubmit"/>
	    </div>
    </asp:View>
    
    <asp:View ID="vwResults" runat="server">
        <asp:Button ID="lnkViewMap" Text="View All Locations" runat="server" Visible="false" resourcekey="lnkViewMap" />
        <asp:Button ID="btnBack2" runat="server" Text="Start Over" OnClick="btnBack_Click" CssClass="LocatorMapItButton" resourcekey="btnBack2" />
        <asp:Button ID="btn_ShowAll" runat="server" OnClick="btnShowAll_Click" CssClass="LocatorMapItbutton" resourcekey="btnShowAll" />
        <asp:Button ID="lnkSubmitLocation" runat="server" Text="Submit A Location" Visible="false" resourcekey="lnkSubmitLocations" OnClick="lnkSubmitLocations_Click" />
        <div class="locationMapHeading">
            <div class="closestsLocations">
	            <asp:Label ID="lblNumClosest" runat="server" CssClass="Normal" resourcekey="lblNumClosest" />
	        </div>
	        <div class="closestLocationTitle"><a name="map"></a><asp:Label ID="lblLocatorMapLabel" runat="server" /><span id="locatorMapLabel" class="SubHead"></span> </div>
	        <div class="locatorMapLabelWrapper">
                <div id="Div1" style="display:none;">
	                <a id="A1" target="_blank" ><asp:Label ID="Label1" runat="server" resourcekey="lblMapLinkMapName" /></a>
	                <asp:Label ID="Label2" runat="server" resourcekey="lblMapLinkDrivingDirections"/>
                </div>
            </div>
        </div>
        <div id="divMap" runat="server" class="locatorMap" style="display:none;"></div>
        
         <div class="locatorMapLabelWrapper">
            <asp:Label ID="lblScrollToViewMore" runat="server" resourcekey="lblScrollToViewMore" style="display:none;" />
            <div id="lblMapLink" style="display:none;">
	            <a id="lnkDrivingDirections" target="_blank" ><asp:Label ID="lblMapLinkMapName" runat="server" resourcekey="lblMapLinkMapName" /></a>
	            <asp:Label ID="lblMapLinkDrivingDirections" runat="server" resourcekey="lblMapLinkDrivingDirections"/>
            </div>
        </div>
        <br />
        <div class="locationsGridWrapper">

            <table class="titleHeading">
                <asp:Repeater ID="rptLocations" runat="server" OnItemDataBound="rptLocations_ItemDataBound">
                    <HeaderTemplate>
					        <tr>
					        <th class="locationTitleHead"><asp:Label id="lblLocationHeader" runat="server" resourcekey="lblLocationHeader" /></th>
					        <th id="thDistance" class="distanceTitleHead"><asp:Label id="lblStoreDistance" runat="server" resourcekey="lblStoreDistance" /></th>
					        <% if(ShowLocationDetails == "True" || ShowLocationDetails == "SamePage") { %><th><br /><asp:Label id="lblLocationDetails" runat="server" resourcekey="lblLocationDetails" /></th><% } %>
					        </tr>
                        
                    </HeaderTemplate>
                    <ItemTemplate> 
                        <tr class="locationEntryTR">
                            <td class="locationTitleTD">
                                <table>
                                    <tr>
                                        <td class="locationTitleTD_Number">
                                            <asp:Label ID="lblLocationMapNumber" CssClass="NormalBold" runat="server" />
                                        </td>
                                        <td class="locationTitleTD">
                                            <asp:Hyperlink ID="lnkLocationName" runat="server" CssClass="SubHead" Target="_blank" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Website").ToString() %>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%></asp:Hyperlink> <br />
	                                        <asp:Label ID="lblLocationsGridAddress" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Address").ToString())%><br /></asp:Label>
	                                        <asp:Label ID="lblLocationsGridCity" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "City").ToString()) + ","%></asp:Label> 
	                                        <asp:Label ID="lblLocationsGridState" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Abbreviation").ToString())%></asp:Label> 
	                                        <asp:Label ID="lblLocationsGridPostalCode" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "PostalCode").ToString())%></asp:Label>
	                                        <br />
	                                        <asp:Label ID="lblPhoneNumber" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Phone").ToString())%></asp:Label>
	                                        <asp:HyperLink id="lnkShowLocationDetails" runat="server" class="Normal" resourcekey="lnkShowLocationDetails" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
	                        </td>
                            <td class="distanceTitleTD">
	                            <asp:Label ID="lblWalkIn" CssClass="Normal" runat="server"></asp:Label>
	                            <asp:Label ID="lblLocationsGridDistance" runat="server" CssClass="Normal" /><br />
	                            <asp:Label ID="lblCurrentlyMapped" runat="server" resourcekey="LocationDisplayHeaders" CssClass="hideCurrentlyMapped" />
	                            <asp:Panel ID="pnlDescription" runat="server" CssClass="locationsGridDescriptionPopup" style="display:none"></asp:Panel>
	                            <div class="viewMapBt"><asp:LinkButton ID="lnkMapIt" runat="server" Text="View Map" CssClass="CommandButton" resourcekey="lnkMapIt" /></div>
                            </td>
                            <% if(ShowLocationDetails == "True" || ShowLocationDetails == "SamePage") { %>
                            <td class="locationDetailsTitleTD">                                
	                            <asp:Label ID="lblLocationDetails" runat="server" CssClass="Normal"><%# DataBinder.Eval(Container.DataItem, "LocationDetails").ToString()%></asp:Label>
                            </td>
                            <% } %>
                           <td class="Normal">
                                <div id="div_SiteLink">
	                                <asp:HyperLink ID="lbSiteLink" Visible="false" runat="server" CssClass="Normal" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Website").ToString()%>' resourceKey="lbSiteLink" Text="Click Here"></asp:HyperLink>
	                            </div>                              
                            </td>

                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </asp:View>
</asp:MultiView>