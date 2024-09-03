using System;
using System.IO;
using System.Reflection;

public static class ResourceHelper
{
    public static byte[] GetEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new FileNotFoundException("Resource not found", resourceName);

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
