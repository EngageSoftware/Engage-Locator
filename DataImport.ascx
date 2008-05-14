<%@ Import namespace="DotNetNuke.Services.Localization"%>
<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.DataImport" AutoEventWireup="True"
    Codebehind="DataImport.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations."
    Visible="False" resourcekey="lblConfigured" ></asp:Label>
<div class="divPanelTab" id="divPanelTab" runat="server">
    <asp:Button runat="server" ID="btnAddLocation" CssClass="CommandButton" Text="Add New Location" resourcekey="btnAddLocation"
        OnClick="btnAddLocation_Click" />
    <asp:Label ID="lblSuccess" runat="server" CssClass="Normal"></asp:Label>    
    <ajaxToolkit:tabcontainer id="tabContainer" runat="server" activetabindex="0" autopostback="false">
        <ajaxToolkit:TabPanel ID="tabpnlLocations" runat="server">
            <HeaderTemplate>
                <asp:Label runat="server" CssClass="Normal" id="lblEditLocations" resourcekey="lblEditLocations">Edit Locations</asp:Label>
            </HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="upDataImport" runat="server" >
                    <ContentTemplate>
                        <div id="editDiv" runat="server">
                            <div id="rbLocations" runat="server" class="rbLocations">
                                <asp:RadioButton ID="rbApproved" runat="server" GroupName="rbLocations" Checked="true" resourcekey="rbApproved" CssClass="Normal" OnCheckedChanged="rbLocations_CheckChanged" AutoPostBack="true" />
                                <asp:RadioButton ID="rbWaitingForApproval" runat="server" GroupName="rbLocations" resourcekey="rbWaitingForApproval" CssClass="Normal" OnCheckedChanged="rbLocations_CheckChanged" AutoPostBack="true" /> 
                            </div>   
                            <asp:DataGrid ID="dgLocations" runat="server" CssClass="importData" GridLines="Vertical" AllowPaging="True" OnDataBinding="dgLocations_DataBind" OnPageIndexChanged="dgLocations_PageChange" AutoGenerateColumns="False" OnEditCommand="dgLocations_EditCommand" OnCancelCommand="dgLocations_CancelCommand" OnDeleteCommand="dgLocations_DeleteCommand" OnItemDataBound="dgLocations_ItemDataBound" OnItemCreated="dgLocations_ItemCreated" >
                                <FooterStyle BackColor="#ccc" ForeColor="Black" />
                                <PagerStyle CssClass="dataImportPage" HorizontalAlign="Center" Mode="NumericPages" />
                                <HeaderStyle CssClass="dataImportHeader" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="ID" Visible="False" >
                                        <ItemTemplate>
                                            <asp:Label id="lblLocationId" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.LocationId", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Key">
                                        <ItemTemplate>
                                            <asp:Label id="lblLoc" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.ExternalIdentifier", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="idDataGridFooter" />
                                        <FooterStyle CssClass="idDataGridFooter" />
                                        <HeaderStyle CssClass="idDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label id="lblName" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Name", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="nameDataGridFooter" />
                                        <FooterStyle CssClass="nameDataGridFooter" />
                                        <HeaderStyle CssClass="nameDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Address">
                                        <ItemTemplate>
                                            <asp:Label id="lblAddress" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Address", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="addressDataGridFooter" />
                                        <FooterStyle CssClass="addressDataGridFooter" />
                                        <HeaderStyle CssClass="addressDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="City">
                                        <ItemTemplate>
                                            <asp:Label id="lblCity" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.City", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="cityDataGridFooter" />
                                        <FooterStyle CssClass="cityDataGridFooter" />
                                        <HeaderStyle CssClass="cityDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Region">
                                        <ItemTemplate>
                                            <asp:Label id="lblState" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.StateName", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="stateDataGridFooter" />
                                        <FooterStyle CssClass="stateDataGridFooter" />
                                        <HeaderStyle CssClass="stateDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="PostalCode">
                                        <ItemTemplate>
                                            <asp:Label id="lblZip" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.PostalCode", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="zipDataGridFooter" />
                                        <FooterStyle CssClass="zipDataGridFooter" />
                                        <HeaderStyle CssClass="zipDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Country">
                                        <ItemTemplate>
                                            <asp:Label id="lblCountry" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Country", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="countryDataGridFooter" />
                                        <FooterStyle CssClass="countryDataGridFooter" />
                                        <HeaderStyle CssClass="countryDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Latitude">
                                        <ItemTemplate>
                                            <asp:Label id="lblLatitude" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Latitude") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="latitudeDataGridFooter" />
                                        <FooterStyle CssClass="latitudeDataGridFooter" />
                                        <HeaderStyle CssClass="latitudeDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Longitude">
                                        <ItemTemplate>
                                            <asp:Label id="lblLongitude" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Longitude") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="longitudeDataGridFooter" />
                                        <FooterStyle CssClass="longitudeDataGridFooter" />
                                        <HeaderStyle CssClass="longitudeDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Phone">
                                        <ItemTemplate>
                                            <asp:Label id="lblPhone" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Phone", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="phoneDataGridFooter" />
                                        <FooterStyle CssClass="phoneDataGridFooter" />
                                        <HeaderStyle CssClass="phoneDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Location Details">
                                        <ItemTemplate>
                                            <asp:Label id="lblLocationDetails" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.LocationDetails", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="locationDetailsGridFooter" />
                                        <FooterStyle CssClass="locationDetailsDataGridFooter" />
                                        <HeaderStyle CssClass="locationDetailsDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Website">
                                        <ItemTemplate>
                                            <asp:Label id="lblWebsite" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Website", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Location Type">
                                        <ItemTemplate>
                                            <asp:Label id="lblType" runat="server" CssClass="datagridLables"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.Type", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnApprovedComments" runat="server" CssClass="Normal" OnClick="btnApprovedComments_Click" />
                                            <asp:LinkButton ID="btnNewComments" runat="server" CssClass="Normal" OnClick="btnNewComents_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:CheckBox id="cbApproved" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="typeDataGridFooter" />
                                        <FooterStyle CssClass="typeDataGridFooter" />
                                        <HeaderStyle CssClass="typeDataGridFooter" />
                                    </asp:TemplateColumn>
                                    <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update"></asp:EditCommandColumn>
                                    <asp:ButtonColumn CommandName="Delete" Text="Delete" ></asp:ButtonColumn>
                                </Columns>
                                <SelectedItemStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <AlternatingItemStyle BackColor="#eeeeee" />
                                <ItemStyle BackColor="#f8f8f8" ForeColor="Black" />
                            </asp:DataGrid>        
                        </div>
                        <div>
                            <asp:Button ID="btnAccept" runat="server" Text="Accept" resourcekey="btnAccept" CssClass="CommandButton" OnClick="btnAccept_Click" />
                            <asp:Button ID="btnCancelLocation" runat="server" CssClass="CommandButton" Text="Cancel" resourcekey="btnCancelLocation" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Reject" resourcekey="btnDelete" CssClass="CommandButton" OnClick="btnReject_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>    
        <ajaxToolkit:TabPanel ID="tabPanelImport" runat="server">
            <HeaderTemplate>
                <asp:Label ID="lblImport" runat="server" resourcekey="lblImport" ></asp:Label>
            </HeaderTemplate>
                <ContentTemplate>
                    <div class="importPanel" runat="server" id="fileDiv">
                                <asp:FileUpload ID="fileImport" runat="server" /><br />
                                <asp:Label ID="lblMessage" runat="server" CssClass="Normal" ></asp:Label><br />
                                <asp:Button ID="btnSubmit" runat="server" CssClass="CommandButton" OnClick="btnSubmit_Click" Text="Submit" resourcekey="btnSubmit"/>
                                &nbsp; &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="CommandButton" OnClick="btnBack_Click" Text="Cancel" resourcekey="btnBack"/>
                            </div>
                </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tabpnlComments" runat="server">
            <HeaderTemplate>
                <asp:Label runat="server" CssClass="Normal" id="lblLocationComments" resourcekey="lblLocationComments" Text="New Comments" />
            </HeaderTemplate>
            <ContentTemplate>
                <div>
                    <asp:DataGrid ID="dgSubmittedComments" runat="server" CssClass="importData" GridLines="Vertical" AllowPaging="True" AutoGenerateColumns="False" OnEditCommand="dgComments_EditCommand" OnDeleteCommand="dgComments_DeleteCommand" OnItemCreated="dgComments_ItemCreated" >
                        <FooterStyle BackColor="#ccc" ForeColor="Black" />
                        <PagerStyle CssClass="dataImportPage" HorizontalAlign="Center" Mode="NumericPages" />
                        <HeaderStyle CssClass="dataImportHeader" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="ID" Visible="False" >
                                <ItemTemplate>
                                    <asp:Label id="lblCommentId" runat="server" CssClass="datagridLables"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.CommentId", "{0:d}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Location">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocationName" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Name", "{0:d}") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Comment">
                                <ItemTemplate>
                                    <asp:Label ID="lblComment" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Text", "{0:d}") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Submitted By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCommentAuthor" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Author", "{0:d}") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="cbApproved">
                                <ItemTemplate>
                                    <asp:CheckBox id="cbCommentApproved" runat="server" />
                                </ItemTemplate>
                                <ItemStyle CssClass="typeDataGridFooter" />
                                <FooterStyle CssClass="typeDataGridFooter" />
                                <HeaderStyle CssClass="typeDataGridFooter" />
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update"></asp:EditCommandColumn>
                            <asp:ButtonColumn CommandName="Delete" Text="Delete"></asp:ButtonColumn>
                        </Columns>                    
                        <SelectedItemStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <AlternatingItemStyle BackColor="#eeeeee" />
                        <ItemStyle BackColor="#f8f8f8" ForeColor="Black" />
                    </asp:DataGrid>
                </div>
                <div>
                    <asp:Button ID="btnAcceptComment" runat="server" CssClass="CommandButton" Text="Accept" resourcekey="btnAccept" OnClick="btnAcceptComment_Click" />
                    <asp:Button ID="btnCancelComment" runat="server" CssClass="CommandButton" Text="Cancel" resourcekey="btnCancelComment" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnDeleteComment" runat="server" CssClass="CommandButton" Text="Reject" resourcekey="btnDelete" OnClick="btnDeleteComment_Click" />
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:tabcontainer>
</div>
