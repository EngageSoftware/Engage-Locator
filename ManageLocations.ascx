<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.ManageLocations" AutoEventWireup="True" CodeBehind="ManageLocations.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations." Visible="False" resourcekey="lblConfigured"></asp:Label>

<div class="divPanelTab" id="divPanelTab" runat="server">
    <asp:Button runat="server" ID="btnAddLocation" CssClass="CommandButton" Text="Add New Location" resourcekey="btnAddLocation" OnClick="btnAddLocation_Click" />
    <br />
    <asp:Label ID="lblSuccess" runat="server" CssClass="Normal"></asp:Label>
    <br />
    <asp:Label runat="server" CssClass="Normal" ID="lblEditLocations" resourcekey="lblEditLocations" Visible="false">Edit Locations</asp:Label>
    <asp:UpdatePanel ID="upDataImport" runat="server">
        <ContentTemplate>
            <div id="editDiv" runat="server">
                <div id="rbLocations" runat="server" class="rbLocations">
                    <asp:RadioButton ID="rbApproved" runat="server" GroupName="rbLocations" Checked="true"
                        resourcekey="rbApproved" CssClass="Normal" OnCheckedChanged="rbLocations_CheckChanged"
                        AutoPostBack="true" />
                    <asp:RadioButton ID="rbWaitingForApproval" runat="server" GroupName="rbLocations"
                        resourcekey="rbWaitingForApproval" CssClass="Normal" OnCheckedChanged="rbLocations_CheckChanged"
                        AutoPostBack="true" />
                </div>
                <asp:DataGrid ID="dgLocations" runat="server" CssClass="importData" GridLines="Vertical"
                    AllowPaging="True" OnDataBinding="dgLocations_DataBind" OnPageIndexChanged="dgLocations_PageChange"
                    AutoGenerateColumns="False" OnEditCommand="dgLocations_EditCommand" OnCancelCommand="dgLocations_CancelCommand"
                    OnDeleteCommand="dgLocations_DeleteCommand" OnItemDataBound="dgLocations_ItemDataBound"
                    OnItemCreated="dgLocations_ItemCreated">
                    <FooterStyle BackColor="#ccc" ForeColor="Black" />
                    <PagerStyle CssClass="dataImportPage" HorizontalAlign="Center" Mode="NumericPages" />
                    <HeaderStyle CssClass="dataImportHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblLocationId" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.LocationId", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                       <%-- <asp:TemplateColumn HeaderText="Key">
                            <ItemTemplate>
                                <asp:Label ID="lblLoc" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.ExternalIdentifier", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="idDataGridFooter" />
                            <FooterStyle CssClass="idDataGridFooter" />
                            <HeaderStyle CssClass="idDataGridFooter" />
                        </asp:TemplateColumn>--%>
                        <asp:TemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Name", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="nameDataGridFooter" />
                            <FooterStyle CssClass="nameDataGridFooter" />
                            <HeaderStyle CssClass="nameDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Address", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="addressDataGridFooter" />
                            <FooterStyle CssClass="addressDataGridFooter" />
                            <HeaderStyle CssClass="addressDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="City">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.City", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="cityDataGridFooter" />
                            <FooterStyle CssClass="cityDataGridFooter" />
                            <HeaderStyle CssClass="cityDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Region">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.StateName", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="stateDataGridFooter" />
                            <FooterStyle CssClass="stateDataGridFooter" />
                            <HeaderStyle CssClass="stateDataGridFooter" />
                        </asp:TemplateColumn>
                        <%--<asp:TemplateColumn HeaderText="PostalCode">
                            <ItemTemplate>
                                <asp:Label ID="lblZip" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.PostalCode", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="zipDataGridFooter" />
                            <FooterStyle CssClass="zipDataGridFooter" />
                            <HeaderStyle CssClass="zipDataGridFooter" />
                        </asp:TemplateColumn>--%>
<%--                        <asp:TemplateColumn HeaderText="Country">
                            <ItemTemplate>
                                <asp:Label ID="lblCountry" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Country", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="countryDataGridFooter" />
                            <FooterStyle CssClass="countryDataGridFooter" />
                            <HeaderStyle CssClass="countryDataGridFooter" />
                        </asp:TemplateColumn>
--%>                        <asp:TemplateColumn HeaderText="Latitude">
                            <ItemTemplate>
                                <asp:Label ID="lblLatitude" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Latitude") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="latitudeDataGridFooter" />
                            <FooterStyle CssClass="latitudeDataGridFooter" />
                            <HeaderStyle CssClass="latitudeDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Longitude">
                            <ItemTemplate>
                                <asp:Label ID="lblLongitude" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Longitude") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="longitudeDataGridFooter" />
                            <FooterStyle CssClass="longitudeDataGridFooter" />
                            <HeaderStyle CssClass="longitudeDataGridFooter" />
                        </asp:TemplateColumn>
                       <%-- <asp:TemplateColumn HeaderText="Phone">
                            <ItemTemplate>
                                <asp:Label ID="lblPhone" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Phone", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="phoneDataGridFooter" />
                            <FooterStyle CssClass="phoneDataGridFooter" />
                            <HeaderStyle CssClass="phoneDataGridFooter" />
                        </asp:TemplateColumn>--%>
                       <%-- <asp:TemplateColumn HeaderText="Location Details">
                            <ItemTemplate>
                                <asp:Label ID="lblLocationDetails" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.LocationDetails", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="locationDetailsGridFooter" />
                            <FooterStyle CssClass="locationDetailsDataGridFooter" />
                            <HeaderStyle CssClass="locationDetailsDataGridFooter" />
                        </asp:TemplateColumn>--%>
<%--                        <asp:TemplateColumn HeaderText="Website">
                            <ItemTemplate>
                                <asp:Label ID="lblWebsite" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Website", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
--%>                        <asp:TemplateColumn HeaderText="Location Type">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Type", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Comments">
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedComments" runat="server" CssClass="Normal" /><br />
                                <asp:Label ID="lblWaitingComments" runat="server" CssClass="Normal"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbApproved" runat="server" />
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
            <br />
            <div>
                <asp:Button ID="btnAccept" runat="server" Text="Accept" resourcekey="btnAccept" CssClass="CommandButton"
                    OnClick="btnAccept_Click" />
                <asp:Button ID="btnCancelLocation" runat="server" CssClass="CommandButton" Text="Cancel"
                    resourcekey="btnCancelLocation" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Reject" resourcekey="btnDelete" CssClass="CommandButton"
                    OnClick="btnReject_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
