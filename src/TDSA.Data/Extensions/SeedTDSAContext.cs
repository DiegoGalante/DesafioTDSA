using TDSA.Data.Context;

namespace TDSA.Data.Extensions
{
    public static class SeedTDSAContext
    {
        public static void CriarTabelas(this TDSAContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
