//using System.IO.Compression;
//using System.Text;

//namespace Blog.Application.Caching;
//public static class CompressionUtils
//{
//    public static string CompressGZip( this string inputStr)
//    {
//        var inputBytes = Encoding.UTF8.GetBytes(inputStr);

//        using (var outputStream = new MemoryStream())
//        {
//            using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
//            {
//                gZipStream.Write(inputBytes, 0, inputBytes.Length);
//            }

//            return Convert.ToBase64String(outputStream.ToArray());
//        }
//    }

//    public static string DecompressGZip(this string compressedText)
//    {
//        byte[] inputBytes = Convert.FromBase64String(compressedText);

//        using (var inputStream = new MemoryStream(inputBytes))
//        {
//            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
//            {
//                using (var streamReader = new StreamReader(gZipStream))
//                {
//                    var decompressed = streamReader.ReadToEnd();
//                    return decompressed;
//                }
//            }
//        }
//    }
//}