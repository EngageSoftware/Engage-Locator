<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Details.ascx.cs" Inherits="Engage.Dnn.Locator.Details" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div class="div_ManagementButtons">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lbManageLocations_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lbImportFile_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Comments" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lbManageComments_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" OnClick="lbManageTypes_OnClick" CausesValidation="false" />
</div>

<table class="titleHeading">
    <tr class="locationEntryTR">
        <td class="locationTitleTD">
            <div class="ldName">
                <asp:Label ID="LocationNameLabel" runat="server" CssClass="SubHead" />
            </div>
            <div class="ldAddress1">
                <asp:Label ID="LocationAddress1Label" runat="server" CssClass="Normal" />
            </div>
            <div class="ldAddress2">
                <asp:Label ID="LocationAddress2Label" runat="server" CssClass="Normal" />
            </div>
            <div class="ldAddress3">
                <asp:Label ID="LocationAddress3Label" runat="server" CssClass="Normal" />
            </div>
            <div class="ldPhone">
                <asp:Label ID="PhoneNumberLabel" runat="server" CssClass="Normal" />
            </div>
            <div class="ldLink">
                <asp:HyperLink ID="LocationNameLink" runat="server" CssClass="Normal" Target="_blank" />
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <% if (ShowLocationDetails) { %>
            <div class="locationDetails">
                <div>
                    <asp:Label ID="LocationDetailsHeaderLabel" runat="server" CssClass="SubHead" resourcekey="lblLocationDetailsTitle" />
                </div>
                <div>
                    <asp:Label ID="LocationDetailsLabel" runat="server" CssClass="Normal" />
                </div>
            </div>
            <% } %>
            <div id="div_customAttributes" class="Normal">
                <asp:Repeater ID="CustomAttributeRepeater" runat="server">
                    <ItemTemplate>
                        <div class='div_CustomAttribute<%#Eval("AttributeId") %>'>
                            <asp:Label runat="server" Text='<%#Localization.GetString(Eval("AttributeName").ToString(), this.LocalResourceFile) ?? Eval("AttributeName") %>' />&nbsp;
                            <asp:Label runat="server" Text='<%#Eval("AttributeValue") %>' />
                            <br />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="ldRating">
                <asp:UpdatePanel ID="RatingUpdatePanel" runat="server" UpdateMode="conditional" Visible="false">
                    <ContentTemplate>
                        <div id="divRating" class="divRatingBefore">
                            <asp:Label ID="RatingMessageLabel" runat="server" CssClass="Normal" resourcekey="lblRatingMessage" />
                            <ajaxtoolkit:rating id="RatingControl" runat="server" maxrating="5" starcssclass="ratingStar" waitingstarcssclass="savedRatingStar" filledstarcssclass="filledRatingStar" emptystarcssclass="emptyRatingStar" visible="true" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="ldCommentHeading">
                <asp:Label ID="LocationCommentsLabel" runat="server" CssClass="SubHead" resourcekey="lblLocationComments" />
            </div>
            <div>
                <asp:Label ID="CommentSubmittedLabel" runat="server" CssClass="Normal" Visible="false" />
            </div>
            <div id="AddCommentSection" runat="server" style="display: none">
                <div>
                    <p>
                        <asp:Label ID="AddCommentInstructionsLabel" runat="server" CssClass="Normal" resourcekey="lblAddCommentInstructions" /></p>
                    <div>
                        <asp:TextBox ID="CommentTextBox" runat="server" CssClass="NormalTextBox" TextMode="multiLine" Rows="5" MaxLength="200" Columns="42" /></div>
                    <div>
                        <asp:RequiredFieldValidator ID="CommentRequiredValidator" runat="server" CssClass="NormalRed" ErrorMessage="Please enter a comment" resourcekey="rfvComment" ControlToValidate="CommentTextBox" /></div>
                </div>
                <div>
                    <asp:Label ID="SubmittedByLabel" runat="server" CssClass="Normal" resourcekey="lblSubmittedBy" />
                    <asp:TextBox ID="SubmittedByTextBox" runat="server" CssClass="Normal" />
                </div>
                <div>
                    <asp:Button ID="SubmitButton" runat="server" CssClass="CommandButton" resourcekey="btnSubmit" />
                    <asp:Button ID="CancelButton" runat="server" CssClass="CommandButton" resourcekey="btnCancel" UseSubmitBehavior="false" CausesValidation="false" />
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Repeater ID="CommentsRepeater" runat="server">
                <HeaderTemplate>
                    <table class="commentsTable">
                        <thead>
                            <tr>
                                <th class="locationComment">
                                    <asp:Label ID="CommentHeaderLabel" runat="server" resourcekey="lblLocationHeader" />
                                </th>
                                <th class="locationCommentAuthor">
                                    <asp:Label ID="CommentAuthorHeaderLabel" runat="server" resourcekey="lblCommentAuthor" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="userCommentRow">
                        <td class="userComment">
                            <asp:Label ID="CommentLabel" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Text").ToString())%></asp:Label>
                        </td>
                        <td class="usernameComment">
                            <asp:Label ID="CommentAuthorLabel" runat="server" CssClass="Normal"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Author").ToString())%></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </td>
    </tr>
</table>
<div>
    <asp:Button ID="ShowCommentEntryButton" runat="server" CssClass="CommandButton" UseSubmitBehavior="false" CausesValidation="false" resourcekey="btnAddComment" />
</div>
<div>
    <asp:Button ID="BackButton" runat="server" CssClass="CommandButton" Text="Back" resourceKey="btnBack" CausesValidation="false" />
</div>

<script type="text/javascript">
    <% if (this.RatingControl.Visible) { %>
    $addHandler($get('<%= this.RatingControl.ClientID %>'), 'click', function (eventElement) {
        Sys.UI.DomElement.removeCssClass($get('divRating'), 'divRatingBefore');
        Sys.UI.DomElement.addCssClass($get('divRating'), 'divRatingAfter');
    });
    <% } %>
    
    <% if (this.AddCommentSection.Visible) { %>
    function toggleVisible(hideElement, showElement) {
       hideElement.style.display = 'none';
       showElement.style.display = '';
    }
     
    var cancelButton = $get('<%=this.CancelButton.ClientID %>');
    var showCommentEntryButton = $get('<%=this.ShowCommentEntryButton.ClientID %>');
    var addCommentSection = $get('<%=this.AddCommentSection.ClientID %>');
    
    showCommentEntryButton.onclick = '';
    $addHandler(showCommentEntryButton, 'click', function() {
       toggleVisible(this, addCommentSection);
       return false;
    });
    
    cancelButton.onclick = '';
    $addHandler(cancelButton, 'click', function() {
       toggleVisible(addCommentSection, showCommentEntryButton);
       return false;
    });
    <% } %>
</script>

