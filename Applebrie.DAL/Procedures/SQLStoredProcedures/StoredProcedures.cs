namespace Applebrie.DAL.Procedures.SQLStoredProcedures
{
    public static class StoredProcedures
    {
        public static string GetUser_SP = "select * from [users].[Users] where [id]=@id";
        public static string CreateUser_SP = "[users].[CreateUser]";
        public static string RemoveUser_SP = "delete from [users].[Users] where [id]=@id";
        public static string UpsertUser_SP = "[users].[UpsertUsers]";
        public static string GetUsers_SP = "[users].[GetUsers]";
    }
}
