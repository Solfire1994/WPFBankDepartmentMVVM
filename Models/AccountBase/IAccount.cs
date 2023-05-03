namespace WPFBankDepartmentMVVM.Models.AccountBase
{
    internal interface IAccount
    {
        int Id { get; }
        string name { get; }
        void TransferMoneyPayment(uint sum);

        void TransferMoneyReceiving(uint sum);

        void TopUpAccount(uint sum);

        uint GetAvaibleValue();

        uint GetValue();
    }
}
