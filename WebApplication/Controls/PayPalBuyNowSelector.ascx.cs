using FrameworkLibrary;
using System;
using System.Collections.Generic;

namespace WebApplication.Controls
{
    public partial class PayPalBuyNowSelector : System.Web.UI.UserControl
    {
        public void SetClassTypes(IEnumerable<IMediaDetail> classTypes)
        {
            ClassTypesDropDownList.DataSource = classTypes;
            ClassTypesDropDownList.DataTextField = "ParentSectionTitlePlusSectionTitlePlusPriceLabel";
            ClassTypesDropDownList.DataValueField = "ID";
            ClassTypesDropDownList.DataBind();
        }

        protected void ClassTypesDropDownList_TextChanged(object sender, EventArgs e)
        {
            if (ClassTypesDropDownList.SelectedItem == null)
                return;

            if (string.IsNullOrEmpty(ClassTypesDropDownList.SelectedItem.Value))
                return;

            var classType = MediaDetailsMapper.GetByID(long.Parse(ClassTypesDropDownList.SelectedItem.Value));

            var cmd = "_xclick";

            if (!string.IsNullOrEmpty(classType.RecurringTimePeriod))
            {
                cmd = "_xclick-subscriptions&src=1&t3=" + classType.RecurringTimePeriod + "&a3=" + classType.Price + "&p3=1";
            }

            var taxAmount = ((decimal.Parse(ClassTypesDropDownList.SelectedItem.Value) * FrameworkLibrary.SettingsMapper.GetSettings().ShoppingCartTax) / 100).ToString("0.00");
            BuyNowButton.NavigateUrl = "https://www.paypal.com/cgi-bin/webscr?button=buynow&business=" + AppSettings.PayPalBuisnessID + "&item_name=" + ClassTypesDropDownList.SelectedItem.Text + "&quantity=1&amount=" + classType.Price + "&currency_code=CAD&shipping=0&tax=" + taxAmount + "&cmd=" + cmd + "&bn=JavaScriptButton_buynow&env=www";
            BuyNowButton.Visible = true;
        }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}