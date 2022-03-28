namespace UI.Webservice.Models
{
    public class TransactionModel
    {
        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public int? Amount { get; set; }
    }
}
