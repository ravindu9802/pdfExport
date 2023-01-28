
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
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

        await generateHTML( headerContect, bodyContent);
    }

    public static async Task generateHTML(Headers headerContect, DetailBody detailBody)
    {
        try
        {
           //code here
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);

        }
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

