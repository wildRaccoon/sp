using System;
using System.Linq.Expressions;
using sp.auth.domain.account;

namespace sp.auth.app.account.commands.common
{
    public class RenewTokenModel
    {
        public string RenewToken { get; set; }
        
        public static Expression<Func<AccountSession, RenewTokenModel>> Projection
        {
            get
            {
                return session => new RenewTokenModel
                {
                    RenewToken = session.RenewToken
                };
            }
        }
        
        public static RenewTokenModel Create(AccountSession session)
        {
            return Projection.Compile().Invoke(session);
        }
    }
}