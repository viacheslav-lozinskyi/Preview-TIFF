
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace resource.preview
{
    public class TIFF : cartridge.AnyPreview
    {
        protected override void _Execute(atom.Trace context, string url)
        {
            if (File.Exists(url))
            {
                var a_Context = Image.FromFile(url);
                {
                    var a_Size = GetProperty(NAME.PROPERTY.LIMIT_PREVIEW_SIZE);
                    {
                        a_Size = Math.Min(a_Size, a_Context.Height / CONSTANT.OUTPUT_PREVIEW_ITEM_HEIGHT);
                        a_Size = Math.Max(a_Size, CONSTANT.OUTPUT_PREVIEW_MIN_SIZE);
                    }
                    for (var i = 0; i < a_Size; i++)
                    {
                        __Send(context, NAME.PATTERN.PREVIEW, 1, "", "");
                    }
                }
                {
                    context.
                        SetState(NAME.STATE.FOOTER).
                        Send(NAME.PATTERN.ELEMENT, 1, "[[Size]]: " + a_Context.Width.ToString() + " x " + a_Context.Height.ToString());
                    {
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[File Name]]", url);
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[File Size]]", (new FileInfo(url).Length).ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Width]]", a_Context.Width.ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Height]]", a_Context.Height.ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Physical Width]]", ((int)a_Context.PhysicalDimension.Width).ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Physical Height]]", ((int)a_Context.PhysicalDimension.Height).ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Horizontal Resolution]]", a_Context.HorizontalResolution.ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Vertical Resolution]]", a_Context.VerticalResolution.ToString());
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Pixel Format]]", __GetPixelFormat(a_Context));
                        __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Raw Format]]", "TIFF");
                    }
                }
                {
                    a_Context.Dispose();
                }
            }
            else
            {
                context.
                    SendError(1, "[[File not found]]");
            }
        }

        private static void __Send(atom.Trace context, string pattern, int level, string name, string value)
        {
            context.
                Clear().
                SetValue(value).
                Send(pattern, level, name);
        }

        private static string __GetPixelFormat(Image context)
        {
            switch (context.PixelFormat)
            {
                case PixelFormat.DontCare: return "Don't Care";
                case PixelFormat.Max: return "Max";
                case PixelFormat.Indexed: return "Indexed";
                case PixelFormat.Gdi: return "GDI";
                case PixelFormat.Format16bppRgb555: return "16bpp RGB 555";
                case PixelFormat.Format16bppRgb565: return "16bpp RGB 565";
                case PixelFormat.Format24bppRgb: return "24bpp RGB";
                case PixelFormat.Format32bppRgb: return "32bpp RGB";
                case PixelFormat.Format1bppIndexed: return "1bpp Indexed";
                case PixelFormat.Format4bppIndexed: return "4bpp Indexed";
                case PixelFormat.Format8bppIndexed: return "8bpp Indexed";
                case PixelFormat.Alpha: return "Alpha";
                case PixelFormat.Format16bppArgb1555: return "16bpp ARGB 1555";
                case PixelFormat.PAlpha: return "PAlpha";
                case PixelFormat.Format32bppPArgb: return "32bpp PARGB";
                case PixelFormat.Extended: return "Extended";
                case PixelFormat.Format16bppGrayScale: return "16bpp GrayScale";
                case PixelFormat.Format48bppRgb: return "48bpp RGB";
                case PixelFormat.Format64bppPArgb: return "64bpp PARGB";
                case PixelFormat.Canonical: return "Canonical";
                case PixelFormat.Format32bppArgb: return "32bpp ARGB";
                case PixelFormat.Format64bppArgb: return "64bpp ARGB";
            }
            return "[[Undefined]]";
        }
    };
}
