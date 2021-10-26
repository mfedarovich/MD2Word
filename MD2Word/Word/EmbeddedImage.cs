using System;
using System.Drawing;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace MD2Word.Word
{
    public class EmbeddedImage
    {
        private readonly WordprocessingDocument _document;
        private readonly long _windowWidth; // it is need for proper scaling of the images

        public EmbeddedImage(WordprocessingDocument document, long windowWidth)
        {
            _document = document;
            _windowWidth = windowWidth;
        }

        public void AddImage(Paragraph paragraph, byte[] buffer)
        {
            var mainPart = _document.MainDocumentPart;
            var imagePart = mainPart!.AddImagePart(ImagePartType.Png);
            using (var stream = new MemoryStream(buffer))
            {
                imagePart.FeedData(stream);
                using (var img = Image.FromStream(stream))
                {
                    AddImageToBody(paragraph, mainPart.GetIdOfPart(imagePart), img.Width, img.Height);
                }
            }
        }
        
        private void AddImageToBody(Paragraph paragraph, string relationshipId, long width, long height)
        {
            // Define the reference of the image.
            var cx = Math.Min(PixelsToEmu(width), PixelsToEmu(_windowWidth));
            var cy =Math.Min(PixelsToEmu(height), (long)( cx * ((double)height/width)));
            
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = cx, Cy = cy },
                         new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, 
                             RightEdge = 0L, BottomEdge = 0 },
                         new DW.DocProperties() { Id = (UInt32Value)1U, 
                             Name = $"{Guid.NewGuid()}" },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties() 
                                            { Id = (UInt32Value)0U, 
                                                Name = $"{Guid.NewGuid()}.png" },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip() 
                                         { Embed = relationshipId, CompressionState = A.BlipCompressionValues.Print },
                                         new A.Stretch(new A.FillRectangle())), 
                                            new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = cx, Cy = cy }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         ) { Preset = A.ShapeTypeValues.Rectangle }))
                             ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     ) { DistanceFromTop = (UInt32Value)0U, 
                         DistanceFromBottom = (UInt32Value)0U, 
                         DistanceFromLeft = (UInt32Value)0U, 
                         DistanceFromRight = (UInt32Value)0U, EditId = "50D07946" });

            paragraph.Append(new Run(element));
        }
        
        public static long PixelsToEmu(long pixels)
        {
            return (int)Math.Round((decimal)pixels * 9525L);
        }
    }
}