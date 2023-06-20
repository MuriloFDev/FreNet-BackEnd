namespace App.Model
{
    public partial class Quotation
    {
        public int id { get; set; }
        public string Carrier { get; set; }
        public string CarrierCode { get; set; }
        public string DeliveryTime { get; set; }
        public string Msg { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceDescription { get; set; }
        public string ShippingPrice { get; set; }
        public string OriginalDeliveryTime { get; set; }
        public string OriginalShippingPrice { get; set; }
        public string SellerCEP { get; set; }
        public string RecipientCEP { get; set; }

    }
}
