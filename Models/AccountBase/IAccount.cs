using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Models.AccountBase
{
    internal interface IAccount
    {
        string name { get; }
        void TransferMoneyPayment(uint sum);

        void TransferMoneyReceiving(uint sum);

        void TopUpAccount(uint sum);

        void CloseAccount();

        uint GetAvaibleValue();

        uint GetValue();
    }
}
