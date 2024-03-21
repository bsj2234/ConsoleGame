namespace ConsoleGameProject
{
    public static class UiFightLogManager
    {
        private static string content = "";

        public static void Append(string str)
        {
            content = String.Concat(content, str);
        }

        public static void Clear()
        {
            content = "";
        }

        public static string GetContent() { return content; }
    }
}
