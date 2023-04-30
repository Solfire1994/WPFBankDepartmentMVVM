using System;

namespace WPFBankDepartmentMVVM.Models.AccountBase
{
    internal class DepositAccount : IAccount
    {
        public readonly uint capitPercent = 6;
        public readonly uint penalty = 8;
        public string name { get; private set; }
        private uint value;

        public void CloseAccount()
        {
            throw new NotImplementedException();
        }

        public uint GetAvaibleValue()
        {
            return value * 100 / (100 + penalty);
        }

        public void TopUpAccount(uint sum)
        {
            value += sum * (100 + capitPercent) / 100;
        }

        public void TransferMoneyPayment(uint sum)
        {
            value -= sum * (100 + penalty) / 100;
        }

        public void TransferMoneyReceiving(uint sum)
        {
            value += sum * (100 + capitPercent) / 100;
        }

        public uint GetValue()
        {
            return value;
        }

        public DepositAccount (int id)
        {
            name = $"Депозитный счет № {id + 100000}";
        }

        public DepositAccount(string name, uint value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
