using System;
using WpfImagesViewer.Annotations;

namespace WpfImagesViewer.Models
{
    public class ImageModel
    {
        public ImageModel([NotNull]string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            var extIndex = filePath.LastIndexOfAny("\\/".ToCharArray());
            Name = extIndex > 0 ? filePath.Substring(extIndex + 1) : filePath;
        }

        public string Name { get; }
        public string FilePath { get; }
    }
}