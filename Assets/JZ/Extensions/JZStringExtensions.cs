namespace JZExtensions.STRING
{
    public static class JZStringExtensions
    {
        public static bool IsAlphanumeric(this string value, bool allowOnlySpaces = false)
        {
            if(!allowOnlySpaces && value.IsOnlySpaces()) return false;

            foreach(var character in value)
            {
                if(char.IsLetterOrDigit(character)) continue;
                else if(character != ' ') return false;
            }

            return true;
        }

        public static bool IsOnlySpaces(this string value)
        {
            foreach(var character in value)
                if(character != ' ') return false;

            return true;
        }

        public static bool IsFullAlphanumeric(this string value)
        {
            if(string.IsNullOrEmpty(value)) return false;
            return value.IsAlphanumeric();
        }
    }
}