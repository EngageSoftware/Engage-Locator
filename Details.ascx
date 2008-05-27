<%@ Control Language="C#" AutoEventWireup="True"  CodeBehind="Details.ascx.cs"  Inherits="Engage.Dnn.Locator.Details" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>

<div class="div_ManagementButtons">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lbManageLocations_OnClick" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lbImportFile_OnClick" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Comments" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lbManageComments_OnClick" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lbManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" />
</div>

<script type="text/javascript">
function hide(d) 
{
    document.getElementById(d).style.display = "none";
}
function show(d) 
{
    document.getElementById(d).style.display = "";
}
</script>

<table class="titleHeading">
    <tr class="locationEntryTR">
        <td class="locationTitleTD">
            <div class="ldID">
                <asp:Label ID="lblLocationId" runat="server" CssClass="Normal" Visible="false" />
            </div>
            <div class="ldName">
                <asp:Label ID="lblLocationName" runat="server" CssClass="SubHead" />
            </div>
            <div class="ldAddress1">
                <asp:Label ID="lblLocationsAddress1" runat="server" CssClass="Normal" />
            </div>
            <div class="ldAddress2">
                <asp:Label ID="lblLocationsAddress2" runat="server" CssClass="Normal" />
            </div>
            <div class="ldAddress3">
                <asp:Label ID="lblLocationsAddress3" runat="server" CssClass="Normal" />
            </div>
            <div class="ldPhone">
                <asp:Label ID="lblPhoneNumber" runat="server" CssClass="Normal" />
            </div>
            <div class="ldLink">
                <asp:HyperLink ID="lnkLocationName" runat="server" CssClass="Normal" Target="_blank"></asp:HyperLink>
            </div>
        </td>
    </tr>
    <% if (ShowLocationDetails)
       { %><tr>
        <td>
            <div>
                <asp:Label ID="lblLocationDetailsTitle" runat="server" CssClass="SubHead" resourcekey="lblLocationDetailsTitle" />
            </div>
            <div>
                <asp:Label ID="lblLocationDetails" runat="server" CssClass="Normal"></asp:Label>
            </div>
    <% } %>
            <div id="div_customAttributes" class="Normal">
                <asp:PlaceHolder ID="plhCustomAttributes" runat="server"></asp:PlaceHolder>
            </div>
			<div class="ldRating">
                <asp:UpdatePanel ID="upnlRating" runat="server" UpdateMode="conditional" Visible="false">
                    <ContentTemplate>
                        <div id="divRating" class="divRatingBefore">
                            <asp:Label ID="lblRatingMessage" runat="server" CssClass="Normal" resourcekey="lblRatingMessage"></asp:Label>
                            <ajaxToolkit:Rating ID="ajaxRating" runat="server" MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" OnChanged="ajaxRating_Changed" Visible="true" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
			</div>            
        </td>
    </tr>
    <tr>
        <td>
            <div class="ldCommentHeading">
                <asp:Label ID="lblLocationComments" runat="server" CssClass="SubHead" Text="Comments"
                    resourcekey="lblLocationComments" />
            </div>
            <div>
                <asp:Label ID="lblCommentSubmitted" runat="server" CssClass="Normal" Text="" Visible="false" />
            </div>	
            <div id="divAddComment" style="display: none">
                <div>
                    <p><asp:Label ID="lblAddCommentInstructions" runat="server" CssClass="Normal" resourcekey="lblAddCommentInstructions" />
                    <div><asp:TextBox ID="txtComment" runat="server" CssClass="NormalTextBox" TextMode="multiLine" Rows="5" MaxLength="200" Columns="42" /></div>
                    <div><asp:RequiredFieldValidator ID="rfvComment" runat="server" CssClass="NormalRed" ErrorMessage="Please enter a comment" 
                    	resourcekey="rfvComment" ControlToValidate="txtComment" /></div>
                </div>
                <div>
                    <asp:Label ID="lblSubmittedBy" runat="server" CssClass="Normal" resourcekey="lblSubmittedBy" />
                    <asp:TextBox ID="txtSubmittedBy" runat="server" CssClass="Normal" />
                </div>
                <div>
                    <asp:Button ID="btnSubmit" runat="server" CssClass="CommandButton" OnClick="btnSubmit_Click" resourcekey="btnSubmit" Text='<%# Localization.GetString("btnSubmit", LocalResourceFile) %>' />
                    <asp:Button ID="btnCancel" runat="server" CssClass="CommandButton" resourcekey="btnCancel" Text='<%# Localization.GetString("btnCancel", LocalResourceFile) %>' />
                </div>
            </div>
        </td>
        <td>
            <asp:Repeater ID="rptComments" runat="server">
                <HeaderTemplate>
                    <tr>
                        <th class="locationComment">
                            <asp:Label ID="lblCommentsHeader" runat="server" resourcekey="lblLocationHeader" />
                        </th>
                        <th class="locationCommentAuthor">
                            <asp:Label ID="lblCommentAuthor" runat="server" resourcekey="lblCommentAuthor" />
                        </th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="userCommentRow">
                        <td class="userComment">
                            <asp:Label ID="lblComment" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Text").ToString())%></asp:Label>
                        </td>
                        <td class="usernameComment">
                            <asp:Label ID="lblCommentAuthor" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Author").ToString())%></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
			</td>

			<tr>
            	<td>
                    <div>
                        <asp:Button ID="btnAddComment" runat="server" CssClass="CommandButton" resourcekey="btnAddComment" Text='<%# Localization.GetString("btnAddComment", LocalResourceFile) %>' />
                    </div>
				</td>                    
			</tr>
    </tr>
</table>
<div>
    <asp:Button ID="btnBack" runat="server" CssClass="CommandButton" Text="Back" resourceKey="btnBack" OnClick="btnBack_Click" CausesValidation="false" />
</div>

<script type="text/javascript">
    <% if (ajaxRating.Visible) { %>
    // Method called when the Rating is changed
    function changeCssClassMethod(eventElement) {
       Sys.UI.DomElement.removeCssClass($get('divRating'), 'divRatingBefore'); 
       Sys.UI.DomElement.addCssClass($get('divRating'), 'divRatingAfter'); 

       //Sys.UI.DomElement.toggleCssClass($get('divRating'), "divRatingAfter");
    }
    // Add handler using the getElementById method
    $addHandler(Sys.UI.DomElement.getElementById('<%= ajaxRating.ClientID %>'), 'click', changeCssClassMethod);
    <% } %>
</script>