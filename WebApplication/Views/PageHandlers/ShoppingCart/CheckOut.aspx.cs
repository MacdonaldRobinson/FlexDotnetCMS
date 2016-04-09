using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace WebApplication.Views.PageHandlers.ShoppingCart
{
    public partial class CheckOut : FrontEndBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PayNow_OnClick(object sender, EventArgs e)
        {            
            
            /*NameValueCollection KeyValuePairsToPost = new NameValueCollection();

            KeyValuePairsToPost.Add("x_currency_code", "CAD");                        

            AuthorizationRequest authReq = default(AuthorizationRequest);
            authReq = new AuthorizeNet.AuthorizationRequest(KeyValuePairsToPost);

            //step 1 - create the request
            var request = new AuthorizationRequest("4111111111111111", "1216", 10.00M, "Test Transaction");
            request.AddCustomer("", "Visitor F", "Visitor L", "Address", "State", "ZIP");
            request.AddDuty(decimal.Parse("12.30"));
            request.AddTax(decimal.Parse("2.00"));
            request.AddFreight(decimal.Parse("32.32"));            

            //step 2 - create the gateway, sending in your credentials
            var gateway = new Gateway("xxxx", "xxx", true);

            //step 3 - make some money
            IGatewayResponse resp = gateway.Send(request);
            Response.Write(resp.Message);*/
        
        }
    }
}