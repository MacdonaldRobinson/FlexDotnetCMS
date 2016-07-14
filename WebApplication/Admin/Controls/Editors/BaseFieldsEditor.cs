using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public class SelectorFieldOption
    {
        public string OptionText { get; set; }
        public string OptionValue { get; set; }
        public string AdminControl { get; set; }
        public string GetAdminControlValue { get; set; }
        public string SetAdminControlValue { get; set; }
        public string FrontEndLayout { get; set; }
    }

    public class BaseFieldsEditor : System.Web.UI.UserControl
    {
        public List<SelectorFieldOption> SelectorFieldOptions { get; } = new List<SelectorFieldOption>();

        public BaseFieldsEditor()
        {
            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Text Box Single Line",
                OptionValue = "TextBoxSingleLine",
                AdminControl = "<asp:TextBox runat='server' />",
                GetAdminControlValue = "Text",
                SetAdminControlValue = "Text"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Text Box Multi Line",
                OptionValue = "TextBoxMultiLine",
                AdminControl = "<asp:TextBox runat='server' TextMode='Multiline' Height='200px' />",
                GetAdminControlValue = "Text",
                SetAdminControlValue = "Text"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Html Editor",
                OptionValue = "HtmlEditor",
                AdminControl = "<asp:TextBox runat='server' TextMode='Multiline' CssClass='AceEditor' Height='200px' />",
                GetAdminControlValue = "Text",
                SetAdminControlValue = "Text"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "WYSIWYG Editor",
                OptionValue = "WYSIWYGEditor",
                AdminControl = "<Admin:Editor runat='server' Height='200px' />",
                GetAdminControlValue = "EditorInstance.Text",
                SetAdminControlValue = "EditorInstance.Text"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Date Time Picker",
                OptionValue = "DateTimePicker",
                AdminControl = "<asp:TextBox runat='server' class='datetimepicker' />",
                GetAdminControlValue = "Text",
                SetAdminControlValue = "Text"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "CheckBox",
                OptionValue = "CheckBox",
                AdminControl = "<asp:CheckBox runat='server' />",
                GetAdminControlValue = "Checked",
                SetAdminControlValue = "Checked"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "CheckBoxList",
                OptionValue = "CheckBoxList",
                AdminControl = @"<asp:CheckBoxList runat='server'>
    <asp:ListItem Text='Item1' Value='Item1'></asp:ListItem>
    <asp:ListItem Text='Item2' Value='Item2'></asp:ListItem>
    <asp:ListItem Text='Item3' Value='Item3'></asp:ListItem>
    <asp:ListItem Text='Item4' Value='Item4'></asp:ListItem>
    <asp:ListItem Text='Item5' Value='Item5'></asp:ListItem>
</asp:CheckBoxList>",
                GetAdminControlValue = @"@using System.Web.UI.WebControls
@{
    var selectedItems = ((ListItemCollection)Model.Control.Items).Cast<ListItem>().Where(i => i.Selected);
    @Raw(StringHelper.ObjectToJson(selectedItems.Select(i => i.Value).ToList()));
}",
                SetAdminControlValue = @"@{
    var newValues = StringHelper.JsonToObject<List<string>>(Model.NewValue);

    if(newValues != null)
    {
        foreach(var controlItem in Model.Control.Items)
        {
            if(Enumerable.Contains(newValues, controlItem.Value))
            {
                controlItem.Selected=true;
            }
        }
    }
}",
                FrontEndLayout = @"@{
    var fieldValues = StringHelper.JsonToObject<List<string>>(Model.Field.FieldValue);

    if(fieldValues != null)
    {
        foreach(var item in fieldValues)
        {
            <li>@item</li>
        }
    }
}",
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "RadioButtonList",
                OptionValue = "RadioButtonList",
                AdminControl = @"<asp:RadioButtonList runat='server'>
    <asp:ListItem Text='Item1' Value='Item1'></asp:ListItem>
    <asp:ListItem Text='Item2' Value='Item2'></asp:ListItem>
    <asp:ListItem Text='Item3' Value='Item3'></asp:ListItem>
    <asp:ListItem Text='Item4' Value='Item4'></asp:ListItem>
    <asp:ListItem Text='Item5' Value='Item5'></asp:ListItem>
</asp:RadioButtonList>",
                GetAdminControlValue = @"@using System.Web.UI.WebControls
@{
    var selectedItems = ((ListItemCollection)Model.Control.Items).Cast<ListItem>().Where(i => i.Selected);
    @Raw(StringHelper.ObjectToJson(selectedItems.Select(i => i.Value).ToList()));
}",
                SetAdminControlValue = @"@{
    var newValues = StringHelper.JsonToObject<List<string>>(Model.NewValue);

    if(newValues != null)
    {
        foreach(var controlItem in Model.Control.Items)
        {
            if(Enumerable.Contains(newValues, controlItem.Value))
            {
                controlItem.Selected=true;
            }
        }
    }
}",
                FrontEndLayout = @"@{
    var fieldValues = StringHelper.JsonToObject<List<string>>(Model.Field.FieldValue);

    if(fieldValues != null)
    {
        foreach(var item in fieldValues)
        {
            <li>@item</li>
        }
    }
}",
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "File Selector",
                OptionValue = "FileSelector",
                AdminControl = "<Admin:FileSelector runat='server' DirPath='/media/uploads/' />",
                GetAdminControlValue = "SelectedFilePath",
                SetAdminControlValue = "SelectedFilePath",
                FrontEndLayout = "<img src='@Model.Field.FieldValue' alt='@Model.Field.FieldValue' />"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Association Auto Suggest",
                OptionValue = "AssociationAutoSuggest",
                AdminControl = "<Admin:AssociationAutoSuggest runat='server' ParentMediaDetailID='0' MediaTypeID='1' />",
                GetAdminControlValue = "Value",
                SetAdminControlValue = "Value",
                FrontEndLayout = @"<ul>
@foreach(var item in Model.Field.FieldAssociations)
{
    <li>
        <a href=""@item.MediaDetail.AbsoluteUrl"">@item.MediaDetail.SectionTitle</a>
    </li>
}
</ul>"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Association Drag Drop",
                OptionValue = "AssociationDragDrop",
                AdminControl = "<Admin:AssociationDragDrop runat='server' />",
                GetAdminControlValue = "Value",
                SetAdminControlValue = "Value",
                FrontEndLayout = @"@{
    var field = (MediaDetailField)Model.Field;

    <ul>
    @foreach(var item in field.FieldAssociations.OrderBy(i=>i.OrderIndex))
    {
        <li><a href='@item.MediaDetail.AbsoluteUrl'>@item.MediaDetail.SectionTitle</a></li>
        <ul>
        @foreach(var child in item.MediaDetail.ChildMediaDetails.OrderBy(i => i.Media.OrderIndex))
        {
            <li><a href='@child.AbsoluteUrl'>@child.SectionTitle </a></li>
        }
        </ul>
    }
    </ul>
}"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Association Drop Down List",
                OptionValue = "Association Drop Down List",
                AdminControl = @"<Admin:AssociationDropDownList runat='server' ParentMediaDetailID='0' MediaTypeID='1' />",
                GetAdminControlValue = "SelectedValue",
                SetAdminControlValue = "SelectedValue",
                FrontEndLayout = @"<ul>
@foreach(var item in Model.Field.FieldAssociations)
{
    <li>
        <a href=""@item.MediaDetail.AbsoluteUrl"">@item.MediaDetail.SectionTitle</a>
    </li>
}
</ul>"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Association Check Box List",
                OptionValue = "Association Check Box List",
                AdminControl = @"<Admin:AssociationCheckBoxList runat='server' ParentMediaDetailID='0' MediaTypeID='1' />",
                GetAdminControlValue = "SelectedValue",
                SetAdminControlValue = "SelectedValue",
                FrontEndLayout = @"<ul>
@foreach(var item in Model.Field.FieldAssociations)
{
    <li>
        <a href=""@item.MediaDetail.AbsoluteUrl"">@item.MediaDetail.SectionTitle</a>
    </li>
}
</ul>"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Association Radio Box List",
                OptionValue = "Association Radio Box List",
                AdminControl = @"<Admin:AssociationCheckBoxList runat='server' IsRadioButtonList='True' ParentMediaDetailID='0' MediaTypeID='1' />",
                GetAdminControlValue = "SelectedValue",
                SetAdminControlValue = "SelectedValue",
                FrontEndLayout = @"<ul>
@foreach(var item in Model.Field.FieldAssociations)
{
    <li>
        <a href=""@item.MediaDetail.AbsoluteUrl"">@item.MediaDetail.SectionTitle</a>
    </li>
}
</ul>"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Multi Item - Uploader",
                OptionValue = "Multi Item - Uploader",
                AdminControl = @"<Admin:MultiFileUploader runat='server' SaveToFolder='' MediaTypeID='25' PageSize='12' Mode='Uploader' />",
                GetAdminControlValue = "SelectedValue",
                SetAdminControlValue = "SelectedValue",
                FrontEndLayout = @"@{
    var field = (MediaDetailField)Model.Field;

    <ul>
    @foreach(var item in field.FieldAssociations.OrderBy(i=>i.OrderIndex))
    {
        <li><a href='@URIHelper.ConvertToAbsUrl(item.MediaDetail.PathToFile)'><img src='@URIHelper.ConvertToAbsUrl(item.MediaDetail.PathToFile)?width=300&height=300&mode=min' alt='@item.MediaDetail.SectionTitle'></a></li>
    }
    </ul>
}"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Multi Item - GridView",
                OptionValue = "Multi Item - GridView",
                AdminControl = @"<Admin:MultiFileUploader runat='server' SaveToFolder='' MediaTypeID='25' PageSize='15' Mode='GridView' />",
                GetAdminControlValue = "SelectedValue",
                SetAdminControlValue = "SelectedValue",
                FrontEndLayout = @"@{
    var field = (MediaDetailField)Model.Field;

    <ul>
    @foreach(var item in field.FieldAssociations.OrderBy(i=>i.OrderIndex))
    {
        <li><a href='@URIHelper.ConvertToAbsUrl(item.MediaDetail.PathToFile)'><img src='@URIHelper.ConvertToAbsUrl(item.MediaDetail.PathToFile)?width=300&height=300&mode=min' alt='@item.MediaDetail.SectionTitle'></a></li>
    }
    </ul>
}"
            });

            SelectorFieldOptions.Add(new SelectorFieldOption()
            {
                OptionText = "Other",
                OptionValue = "Other",
                AdminControl = "",
                GetAdminControlValue = "",
                SetAdminControlValue = ""
            });
        }

        public void BindFieldTypeDropDown(DropDownList fieldTypeDropDown)
        {
            foreach (var SelectorFieldOption in SelectorFieldOptions)
            {
                var listItem = fieldTypeDropDown.Items.FindByValue(SelectorFieldOption.OptionValue);

                if (listItem != null)
                    fieldTypeDropDown.Items.Remove(listItem);
            }

            fieldTypeDropDown.DataSource = SelectorFieldOptions;
            fieldTypeDropDown.DataTextField = "OptionText";
            fieldTypeDropDown.DataValueField = "OptionValue";
            fieldTypeDropDown.DataBind();
        }

        public void FieldTypeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fieldTypeDropDown = ((DropDownList)sender);

            var selectorFieldOption = SelectorFieldOptions.SingleOrDefault(i => i.OptionValue == fieldTypeDropDown.SelectedValue);

            if (selectorFieldOption != null)
            {
                var adminControl = (TextBox)this.FindControl("AdminControl");
                var getAdminControlValue = (TextBox)this.FindControl("GetAdminControlValue");
                var setAdminControlValue = (TextBox)this.FindControl("SetAdminControlValue");
                var frontEndLayout = (TextBox)this.FindControl("FrontEndLayout");
                var fieldValue = (TextBox)this.FindControl("FieldValue");

                if (adminControl != null)
                {
                    adminControl.Text = selectorFieldOption.AdminControl;
                }

                if (getAdminControlValue != null)
                {
                    getAdminControlValue.Text = selectorFieldOption.GetAdminControlValue;
                }

                if (setAdminControlValue != null)
                {
                    setAdminControlValue.Text = selectorFieldOption.SetAdminControlValue;
                }

                if (frontEndLayout != null)
                {
                    frontEndLayout.Text = selectorFieldOption.FrontEndLayout;
                }

                if (fieldValue != null)
                {
                    fieldValue.Text = "";
                }
            }

            fieldTypeDropDown.SelectedValue = "";
        }
    }
}