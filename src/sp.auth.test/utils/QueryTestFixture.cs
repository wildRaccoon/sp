using System;
using sp.auth.app.ef;

namespace sp.auth.test.utils
{
    public class QueryTestFixture : IDisposable
    {
        public AuthDataContext Context { get; private set; }

        public QueryTestFixture()
        {
            Context = TestDbContextFactory.Create();
        }

        public void Dispose()
        {
            TestDbContextFactory.Destroy(Context);
        }
    }
}