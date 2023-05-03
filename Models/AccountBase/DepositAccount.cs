using System;

namespace WPFBankDepartmentMVVM.Models.AccountBase
{
    internal class DepositAccount : IAccount
    {
        public readonly uint capitPercent = 6;
        public readonly uint penalty = 8;
        public string name { get; private set; }
        public int Id { get; private set; }

        private uint value;

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
            Id = id;
            name = $"Депозитный счет № {id + 100000}";
        }

        public DepositAccount(int id, uint value) : this(id)
        {
            Id = id;
            name = $"Депозитный счет № {id + 100000}";
            this.value = value;
        }

        public DepositAccount(int id, string name, uint value)
        {
            Id = id;
            this.name = name;
            this.value = value;
        }
    }
}
