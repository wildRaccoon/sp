using System;

namespace sp.wallet.domain.Balance
{
    public class BalanceEvent
    {
        public string AccId { get; set; }

        public int OperationType { get; set; }

        public decimal BalanceChange { get; set; }

        public string OperationId { get; set; }

        /// <summary>
        /// utc - unix time stamp with millisecond's
        /// </summary>
        public long IssuedOn { get; set; }

        /// <summary>
        /// utc - unix time stamp with millisecond's
        /// </summary>
        public long ProcessedOn { get; set; }
    }
}