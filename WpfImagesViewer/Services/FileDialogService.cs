using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using WpfImagesViewer.Interfaces;

namespace WpfImagesViewer.Services
{
    class FileDialogService : IFileDialogService
    {
        private readonly string _imageExtensions;

        private readonly HashSet<string> _supportedExtensions;

        public FileDialogService()
        {
            var extensions = new[]
            {
                "3fr", "ai", "ani", "arw", "blk", "bmp", "cdr", "cpt", "cr2", "crw", "cs1", "ct", "cur", "dcm",
                "dcr", "dds", "dib", "dng", "emz", "erf", "exr", "fff", "fpx", "gif", "hdr", "icns", "ico",
                "iff", "j2k", "jfif", "jif", "jng", "jp2", "jpe", "jpeg", "jpg", "jps", "jxr", "kdc", "mef",
                "mpo", "mrw", "nef", "nrw", "orf", "pbm", "pcd", "pcx", "pdd", "pdn", "pef", "pgm", "pic", "png",
                "pnm", "ppm", "psd", "raf", "raw", "rgb", "rle", "rw2", "rwl", "sfw", "sgi", "spp", "sr2", "srf",
                "srw", "svg", "tdi", "tga", "thm", "tif", "tiff", "tpic", "wdp", "x3f", "xcf"
            };

            _supportedExtensions = new HashSet<string>(extensions);
            _imageExtensions = string.Format("All image files|{0}",
                string.Join(";", extensions.Select(ext => string.Format("*.{0}", ext))));
        }

        public IEnumerable<string> GetNewImageFileNames()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = _imageExtensions
            };

            return dialog.ShowDialog() == true ? dialog.FileNames : Array.Empty<string>();
        }

        public bool IsImageFileSupported(string path)
        {
            if (path == null) return false;

            var extIndex = path.LastIndexOf(".");
            if (extIndex < 0) return false;

            var extension = path.Substring(extIndex+1);
            return _supportedExtensions.Contains(extension.ToLower());
        }
    }
}