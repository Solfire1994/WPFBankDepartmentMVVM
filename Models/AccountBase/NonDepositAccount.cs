using System;

namespace WPFBankDepartmentMVVM.Models.AccountBase
{
    internal class NonDepositAccount : IAccount
    {
        private readonly uint cashback = 5;
        private readonly uint comission = 4;
        public string name { get; private set; }
        private uint value;
        public int Id { get; private set; }

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

        public uint GetValue()
        {
            return value;
        }

        public NonDepositAccount(int id)
        {
            Id = id;
            name = $"Дебетовый счет № {id + 200000}";
        }

        public NonDepositAccount(int id, uint value) : this(id)
        {
            Id = id;
            name = $"Дебетовый счет № {id + 200000}";
            this.value = value;
        }

        public NonDepositAccount(int id, string name, uint value)
        {
            Id = id;
            this.name = name;
            this.value = value;
        }
    }
}
