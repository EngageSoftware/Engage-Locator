<%@ Control Language="C#" AutoEventWireup="True"  CodeBehind="Details.ascx.cs"  Inherits="Engage.Dnn.Locator.Details" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>

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
        <td class="locationTitleTD Normal">
            <div>
                <asp:Label ID="lblLocationId" runat="server" CssClass="Normal" Visible="false" />
            </div>
            <div>
                <asp:Label ID="lblLocationName" runat="server" CssClass="Head" />
            </div>
            <div>
                <asp:HyperLink ID="lnkLocationName" runat="server" CssClass="SubHead" Target="_blank"></asp:HyperLink>
            </div>
            <div>
                <asp:Label ID="lblLocationsAddress1" runat="server" CssClass="Normal" />
            </div>
            <div>
                <asp:Label ID="lblLocationsAddress2" runat="server" CssClass="Normal" />
            </div>
            <div>
                <asp:Label ID="lblLocationsAddress3" runat="server" CssClass="Normal" />
            </div>
            <div>
                <asp:Label ID="lblPhoneNumber" runat="server" CssClass="Normal" />
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
        </td>
    </tr>
    <% } %>
    <tr>
        <td>
            <div id="div_customAttributes">
                <asp:PlaceHolder ID="plhCustomAttributes" runat="server"></asp:PlaceHolder>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <asp:Label ID="lblLocationComments" runat="server" CssClass="SubHead" Text="Comments"
                    resourcekey="lblLocationComments" />
            </div>
            <div>
                <asp:Label ID="lblCommentSubmitted" runat="server" CssClass="Normal" Text="" Visible="false" />
            </div>	
            <asp:UpdatePanel ID="upnlRating" runat="server" UpdateMode="conditional" Visible="false">
	            <ContentTemplate>
                    <div id="divRating" class="divRatingBefore">
                        <asp:Label ID="lblRatingMessage" runat="server" resourcekey="lblRatingMessage"></asp:Label>
                        <ajaxToolkit:Rating ID="ajaxRating" runat="server" MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" OnChanged="ajaxRating_Changed" Visible="true" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="SubHead">
                <a id="lnkAddComment" href="javascript:show('divAddComment')" visible="true">
                    <asp:Label ID="lblAddComment" runat="server" Text="Add a Comment" resourcekey="lblAddComment" /></a>
            </div>
            <div id="divAddComment" style="display: none">
                <div>
                    <asp:Label ID="lblAddCommentInstructions" runat="server" CssClass="Normal" resourcekey="lblAddCommentInstructions" />
                    <asp:TextBox ID="txtComment" runat="server" CssClass="Normal" TextMode="multiLine"
                        Rows="5" MaxLength="200" />
                    <asp:RequiredFieldValidator ID="rfvComment" runat="server" CssClass="Normal" ErrorMessage="Please enter a comment"
                        resourcekey="rfvComment" ControlToValidate="txtComment" />
                </div>
                <div>
                    <asp:Label ID="lblSubmittedBy" runat="server" CssClass="Normal" resourcekey="lblSubmittedBy" />
                    <asp:TextBox ID="txtSubmittedBy" runat="server" CssClass="Normal" />
                </div>
                <div>
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="Normal" OnClick="btnSubmit_Click"
                        resourcekey="btnSubmit" Text='<%# Localization.GetString("btnSubmit", LocalResourceFile) %>' />
                    <a href="javascript:hide('divAddComment')" class="Normal">
                        <asp:Label ID="lblCancel" runat="server" Text="Cancel" resourcekey="lblCancel" /></a>
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
                    <tr>
                        <td class="locationComment">
                            <asp:Label ID="lblComment" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Text").ToString())%></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCommentAuthor" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Author").ToString())%></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </td>
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