
using DinkToPdf;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;

internal class Program
{
    // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    private static async Task Main(string[] args)
    {

        dynamic token = await getAccessToken();
        Token result = JsonConvert.DeserializeObject<Token>(token);

        dynamic header = await getPageHeaders(result.token);
        Headers headerContect = JsonConvert.DeserializeObject<Headers>(header);

        dynamic body = await getPageContent(result.token);
        DetailBody bodyContent = JsonConvert.DeserializeObject<DetailBody>(body);

        string html =  generateHTML( headerContect, bodyContent);

        generatePDF(html);
    }

    public static string generateHTML(Headers headerContect, DetailBody detailBody)
    {
        string html = string.Empty;

        try
        {

            html = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <style>\r\n      * {\r\n        box-sizing: border-box;\r\n      }\r\n\r\n      /* Create two unequal columns that floats next to each other */\r\n      .column {\r\n        float: left;\r\n      }\r\n\r\n      .left {\r\n        width: 50%;\r\n      }\r\n\r\n      .right {\r\n        width: 50%;\r\n      }\r\n\r\n      /* Clear floats after the columns */\r\n      .row:after {\r\n        content: \"\";\r\n        display: table;\r\n        clear: both;\r\n      }\r\n\r\n      body{\r\n          padding:10px;\r\n          border:1px solid;\r\n          /*height: 3508px;*/\r\n          /*width: 2480px;*/\r\n      }\r\n    </style>\r\n  </head>\r\n  <body>\r\n    <center>"

            + " <center>\r\n<p style=\"font-size:10px;padding:0;margin:0;\">\r\n" + headerContect.godName + "\r\n      </p>\r\n      <p style=\"font-size:12px;padding:0;margin:0;\">\r\n" + headerContect.subjectTo + "\r\n      </p>\r\n      <p style=\"font-size:14px;padding:0;margin:0;\">" + headerContect.delOrderHeading + "</p>\r\n      <p style=\"font-size:16px;padding:0;margin:0;\">" + headerContect.brokerName + "</p>\r\n      <p style=\"font-size:10px;padding:0;margin:0;\">\r\n" + headerContect.brokerAdd1 + headerContect.brokerAdd2 + "\r\n      </p>\r\n      <p style=\"font-size:10px;padding:0;margin:0;\">" + headerContect.brokerAdd3 + "</p>\r\n    </center>"

            //order date
            + "<p style=\"font-size:14px;padding:0;margin:0;\">Order Date : " + detailBody.orderDate + "</p>"

            + "<div class=\"row\" style=\"border:1px solid black;\">\r\n      <div class=\"column left\" style=\"padding:0;margin:0;\">\r\n        <p\r\n          style=\"font-size:12px;padding:0 0 0 5px;margin:0;border-right:1px solid black;\"\r\n        >\r\n          Seller Name & Address Order.No :\r\n        </p>\r\n      </div>\r\n      <div class=\"column right\" style=\"padding:0;margin:0;\">\r\n        <p style=\"font-size:12px;padding:0 0 0 5px;margin:0;\">\r\n          Buyer Name & Address Order.No :\r\n        </p>\r\n      </div>\r\n    </div>"

            + "<div class=\"row\" style=\"border:1px solid black;\">\r\n      <div class=\"column left\" style=\"padding:0 0 0 5px;margin:0;\">\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.buyerAccount.aliaForPrint + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.buyerAccount.address1 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.buyerAccount.address2 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black; height: 12px;\"\r\n        >\r\n" + detailBody.buyerAccount.address3 + "</p>\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black; height: 12px;\"\r\n        >\r\n" + detailBody.buyerAccount.address4 + " </p>\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          GSTIN :" + detailBody.buyerAccount.gstno + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n Confirmed By" + detailBody.buyerAccount.name + "\r\n        </p>\r\n      </div>\r\n      <div class=\"column right\" style=\"padding:0 0 0 5px;margin:0;\">\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.name + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.address1 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.address2 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.address3 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          GSTIN : " + detailBody.shipToAccount.gstno + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          STATE : " + detailBody.shipToAccount.state + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          STATE CODE : " + detailBody.shipToAccount.state + "\r\n        </p>\r\n      </div>\r\n    </div>"

            //shipping details
            + " <div class=\"row\" style=\"border:1px solid black;\">\r\n      <div class=\"column left\" style=\"padding:0 0 0 5px;margin:0;\">\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          Shipping Address\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.name + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.address1 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.address2 + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n" + detailBody.shipToAccount.address3 + "\r\n        </p>\r\n      </div>\r\n      <div class=\"column right\" style=\"padding:0 0 0 5px;margin:0;\">\r\n        <p\r\n          style=\"font-size:12px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          Shipping GST Detail\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          GSTIN : " + detailBody.shipToAccount.gstno + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          STATE : " + detailBody.shipToAccount.state + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;\"\r\n        >\r\n          STATE CODE: " + detailBody.shipToAccount.state + "\r\n        </p>\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;height:12px;\"\r\n        >\r\n          \r\n        </p>\r\n      </div>\r\n    </div>"

            //authorize
            + "<div class=\"row\" style=\"border:1px solid black;\">\r\n      <div class=\"column left\" style=\"padding:0 0 0 5px;margin:0;\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Do Authorised By : " + headerContect.brokerName + "\r\n        </p>\r\n      </div>\r\n      <div class=\"column right\" style=\"padding:0 0 0 5px;margin:0;\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          E.Way.Bill : YES\r\n        </p>\r\n      </div>\r\n    </div>"

            //table header
            + "<div class=\"row\" style=\"border:1px solid black;\">\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;border-left:1px solid black;\"\r\n        >\r\n          Sr.\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:15%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Item Description\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Thick\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Width\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Length\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Grade\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Make\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Pcs\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Qty(P/ton)\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Rate\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Coill.No\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Location\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          HSN.NO\r\n        </p>\r\n      </div>\r\n    </div>";

            //table body
            //int i = 1;
            //foreach (var item in detailBody.itemDetail)
            //{
            //    html += "<div class=\"row\" style=\"min-height:300px; border:1px solid black;\">\r\n      <div class=\"column\" style=\"width:5%;\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;border-left:1px solid black;\"\r\n        >\r\n"+ i + "\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:15%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n"+item.+"\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Thick\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Width\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Length\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Grade\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Make\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Pcs\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Qty(P/ton)\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Rate\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:5%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Coill.No\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          Location\r\n        </p>\r\n      </div>\r\n      <div class=\"column\" style=\"width:10%\">\r\n        <p\r\n          style=\"font-size:10px;padding:0;margin:0;border-right:1px solid black;border-bottom:1px solid black;\"\r\n        >\r\n          HSN.NO\r\n        </p>\r\n      </div>\r\n    </div>"


            //        i++;
            //}







        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
        return html;
    }

    public static void generatePDF(string html)
    {
        //Setup the PDF converter
        var converter = new BasicConverter(new PdfTools());

        //Set PDF options
        var options = new HtmlToPdfDocument
        {
            GlobalSettings =
                    {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Out = @"C:\Users\ravin\OneDrive\Desktop\sample.pdf"
                    },
            Objects =
                    {
                        new ObjectSettings
                        {
                            PagesCount=true,
                            HtmlContent=html,
                            WebSettings = { DefaultEncoding = "UTF-8"},
                            HeaderSettings =
                            {
                                FontSize=10,
                                Line=true,
                                Spacing=3
                            }
                        }
                    }
        };

        converter.Convert(options);

    }


    public static async Task<String> getAccessToken()
    {
        try
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            var content = new StringContent("{\"userName\":\"vipul@scarceinfotech.com\",\"password\":\"vipul1234\",\"ip\":\"\",\"latitude\":\"0.0\",\"longitude\":\"0.0\"}",
                                    Encoding.UTF8,
                                    "application/json");//CONTENT-TYPE header

            using HttpResponseMessage response1 = await client.PostAsync("https://sauda.api.mytinyerp.com/api/Auth/login", content);

            response1.EnsureSuccessStatusCode();
            string res = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(res);

            return res;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);

            return e.Message;
        }
    }

    public class Token
    {
        public string token { get; set; }
    }
    public static async Task<String> getPageHeaders(string token)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);


            using HttpResponseMessage response1 = await client.GetAsync("https://sauda.api.mytinyerp.com/api/DOTemplate/3");

            response1.EnsureSuccessStatusCode();
            string res = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(res);

            return res;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);

            return e.Message;
        }
    }

    public static async Task<String> getPageContent(string token)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response1 = await client.GetAsync("https://sauda.api.mytinyerp.com/api/DeliveryOrder/detail?id=39");

            response1.EnsureSuccessStatusCode();
            string res = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(res);

            return res;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);

            return e.Message;
        }
    }
}


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Headers
{
    public int id { get; set; }
    public string detailRight1 { get; set; }
    public string detailRight2 { get; set; }
    public string detailRight3 { get; set; }
    public string detailLeft1 { get; set; }
    public string detailLeft2 { get; set; }
    public string detailLeft3 { get; set; }
    public string religiousName { get; set; }
    public string subjectTo { get; set; }
    public string delOrderHeading { get; set; }
    public string brokerName { get; set; }
    public string brokerAdd1 { get; set; }
    public string brokerAdd2 { get; set; }
    public string brokerAdd3 { get; set; }
    public int agentId { get; set; }
    public string godName { get; set; }
    public string factory { get; set; }
    public string manager { get; set; }
    public string deliveryNo { get; set; }
    public string office { get; set; }
    public string home { get; set; }
    public string fax { get; set; }
    public string signatoryName { get; set; }
    public string authSignatory { get; set; }
    public string specialRemark { get; set; }
    public object brokerUdyamNo { get; set; }
}






// body classes start

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class BuyerAccount
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public string aliaForPrint { get; set; }
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string address3 { get; set; }
    public string address4 { get; set; }
    public string place { get; set; }
    public string state { get; set; }
    public string pincode { get; set; }
    public string panNo { get; set; }
    public string gstno { get; set; }
    public string phoneNo1 { get; set; }
    public string phoneNo2 { get; set; }
    public string email1 { get; set; }
    public string email2 { get; set; }
    public string remarks { get; set; }
    public object contacts { get; set; }
    public object salesOrder { get; set; }
    public List<object> purchaseOrder { get; set; }
    public object shipToOrder { get; set; }
}

public class Grade
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public List<object> gradeName { get; set; }
}

public class GradeName
{
    public int id { get; set; }
    public int deliveryOrderId { get; set; }
    public object deliveryOrder { get; set; }
    public int itemId { get; set; }
    public Item item { get; set; }
    public double thickness { get; set; }
    public int gradeId { get; set; }
    public object grade { get; set; }
    public int makeId { get; set; }
    public Make make { get; set; }
    public int width { get; set; }
    public int length { get; set; }
    public bool tc { get; set; }
    public int pcs { get; set; }
    public double qty { get; set; }
    public double rate { get; set; }
    public double amount { get; set; }
}

public class Item
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public List<ItemName> itemName { get; set; }
}

public class ItemDetail
{
    public int id { get; set; }
    public int deliveryOrderId { get; set; }
    public object deliveryOrder { get; set; }
    public int itemId { get; set; }
    public Item item { get; set; }
    public double thickness { get; set; }
    public int gradeId { get; set; }
    public Grade grade { get; set; }
    public int makeId { get; set; }
    public Make make { get; set; }
    public int width { get; set; }
    public int length { get; set; }
    public bool tc { get; set; }
    public int pcs { get; set; }
    public double qty { get; set; }
    public double rate { get; set; }
    public double amount { get; set; }
}

public class ItemName
{
    public int id { get; set; }
    public int deliveryOrderId { get; set; }
    public object deliveryOrder { get; set; }
    public int itemId { get; set; }
    public object item { get; set; }
    public double thickness { get; set; }
    public int gradeId { get; set; }
    public Grade grade { get; set; }
    public int makeId { get; set; }
    public Make make { get; set; }
    public int width { get; set; }
    public int length { get; set; }
    public bool tc { get; set; }
    public int pcs { get; set; }
    public double qty { get; set; }
    public double rate { get; set; }
    public double amount { get; set; }
}

public class Make
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public List<MakeName> makeName { get; set; }
}

public class MakeName
{
    public int id { get; set; }
    public int deliveryOrderId { get; set; }
    public object deliveryOrder { get; set; }
    public int itemId { get; set; }
    public Item item { get; set; }
    public double thickness { get; set; }
    public int gradeId { get; set; }
    public Grade grade { get; set; }
    public int makeId { get; set; }
    public object make { get; set; }
    public int width { get; set; }
    public int length { get; set; }
    public bool tc { get; set; }
    public int pcs { get; set; }
    public double qty { get; set; }
    public double rate { get; set; }
    public double amount { get; set; }
}

public class DetailBody
{
    public int id { get; set; }
    public string orderNo { get; set; }
    public DateTime orderDate { get; set; }
    public int sellerAccountId { get; set; }
    public SellerAccount sellerAccount { get; set; }
    public int buyerAccountId { get; set; }
    public BuyerAccount buyerAccount { get; set; }
    public int shipToAccountId { get; set; }
    public ShipToAccount shipToAccount { get; set; }
    public int sellerPointId { get; set; }
    public SellerPoint sellerPoint { get; set; }
    public string supplierNo { get; set; }
    public string doBuyerNo { get; set; }
    public string brokerage { get; set; }
    public string loading { get; set; }
    public string paymentTerm { get; set; }
    public string delivery { get; set; }
    public string doValidity { get; set; }
    public string taxes { get; set; }
    public string doAuthorised { get; set; }
    public string deliveryBy { get; set; }
    public bool isEwayBill { get; set; }
    public string remark { get; set; }
    public string specialNote { get; set; }
    public double amount { get; set; }
    public string tcs { get; set; }
    public string tds { get; set; }
    public int agentId { get; set; }
    public string highlightedRemark { get; set; }
    public List<ItemDetail> itemDetail { get; set; }
    public List<object> glDetail { get; set; }
}

public class SellerAccount
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public string aliaForPrint { get; set; }
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string address3 { get; set; }
    public string address4 { get; set; }
    public string place { get; set; }
    public string state { get; set; }
    public string pincode { get; set; }
    public string panNo { get; set; }
    public string gstno { get; set; }
    public string phoneNo1 { get; set; }
    public string phoneNo2 { get; set; }
    public string email1 { get; set; }
    public string email2 { get; set; }
    public string remarks { get; set; }
    public object contacts { get; set; }
    public List<object> salesOrder { get; set; }
    public object purchaseOrder { get; set; }
    public object shipToOrder { get; set; }
}

public class SellerPoint
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public List<object> sellerPointName { get; set; }
}

public class ShipToAccount
{
    public int id { get; set; }
    public string name { get; set; }
    public int tallyCode { get; set; }
    public int agentId { get; set; }
    public string aliaForPrint { get; set; }
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string address3 { get; set; }
    public string address4 { get; set; }
    public string place { get; set; }
    public string state { get; set; }
    public string pincode { get; set; }
    public string panNo { get; set; }
    public string gstno { get; set; }
    public string phoneNo1 { get; set; }
    public string phoneNo2 { get; set; }
    public string email1 { get; set; }
    public string email2 { get; set; }
    public string remarks { get; set; }
    public object contacts { get; set; }
    public object salesOrder { get; set; }
    public object purchaseOrder { get; set; }
    public List<object> shipToOrder { get; set; }
}

