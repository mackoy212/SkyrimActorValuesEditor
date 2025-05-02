using System.Text;

namespace SkyrimActorValueEditor.ViewModels.Utils
{
    public static class EncodingUtil
    {
        private static readonly Encoding Encoding1252;
        private static readonly Encoding EncodingUTF8;

        static EncodingUtil()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding1252 = Encoding.GetEncoding(1252);
            EncodingUTF8 = Encoding.UTF8;
        }

        public static string Enc1252ToUTF8(string? enc1252)
        {
            if (enc1252 == null) return string.Empty;
            return EncodingUTF8.GetString(Encoding1252.GetBytes(enc1252));
        }
    }
}