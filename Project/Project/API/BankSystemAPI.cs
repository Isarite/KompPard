namespace Project.API
{
    public class CreditCart
    {
        public string FullName { get; set; }
        public string CardNo { get; set; }
        public string ExprirationDate { get; set; }
        public string Cvc { get; set; }
    }

    public enum BankResponse
    {
        Ok, Invalid, NoMoney
    }

    public class BankSystemApi
    {
        public static BankResponse WithdrawFunds(CreditCart card, decimal amount)
        {
            // TODO: Actually implement bank api

            switch (card.Cvc)
            {
                case "100":
                    return BankResponse.Ok;
                case "200":
                    return BankResponse.NoMoney;
                default:
                    return BankResponse.Invalid;
            }
        }
    }
}
