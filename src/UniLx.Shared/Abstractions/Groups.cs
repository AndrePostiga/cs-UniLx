using System.Diagnostics.CodeAnalysis;

namespace UniLx.Shared.Abstractions
{
    [ExcludeFromCodeCoverage]
    public static class Groups
    {
        public const string GroupSection = "cognito:groups";

        public const string Admin = "AdminRole";
        public const string Moderator = "ModeratorRole";
        public const string User = "UserRole";
    }
}
