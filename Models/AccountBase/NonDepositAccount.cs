using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Models.AccountBase
{
    internal class NonDepositAccount : IAccount
    {
        private readonly uint cashback = 5;
        private readonly uint comission = 4;
        public string name { get; private set; }
        private uint value;

        public void CloseAccount()
        {
            throw new NotImplementedException();
        }

        public uint GetAvaibleValue()
        {
            return value;
        }

        public void TopUpAccount(uint sum)
        {
            value += sum * (100 - comission) / 100;
        }

        public void TransferMoneyPayment(uint sum)
        {
            value -= sum * (100 - cashback) / 100;
        }

        public void TransferMoneyReceiving(uint sum)
        {
            value += sum;
        }

        public NonDepositAccount(int id)
        {
            name = $"Дебетовый счет № {id + 200000}";
        }

        public uint GetValue()
        {
            return value;
        }
    }
}
