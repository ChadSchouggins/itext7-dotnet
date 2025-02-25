/*
This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System.Collections.Generic;
using System.IO;
using iText.IO.Image;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Colors.Gradients;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Layout.Renderer {
    public class AbstractRendererUnitTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void CreateXObjectTest() {
            AbstractLinearGradientBuilder gradientBuilder = new StrategyBasedLinearGradientBuilder().SetGradientDirectionAsStrategy
                (StrategyBasedLinearGradientBuilder.GradientStrategy.TO_BOTTOM_LEFT).AddColorStop(new GradientColorStop
                (ColorConstants.RED.GetColorValue(), 0d, GradientColorStop.OffsetType.RELATIVE)).AddColorStop(new GradientColorStop
                (ColorConstants.GREEN.GetColorValue(), 0.5, GradientColorStop.OffsetType.RELATIVE));
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            PdfXObject pdfXObject = AbstractRenderer.CreateXObject(gradientBuilder, new Rectangle(0, 0, 20, 20), pdfDocument
                );
            NUnit.Framework.Assert.IsNotNull(pdfXObject.GetPdfObject().Get(PdfName.Resources));
        }

        [NUnit.Framework.Test]
        public virtual void CreateXObjectWithNullLinearGradientTest() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            PdfXObject pdfXObject = AbstractRenderer.CreateXObject(null, new Rectangle(0, 0, 20, 20), pdfDocument);
            NUnit.Framework.Assert.IsNull(pdfXObject.GetPdfObject().Get(PdfName.Resources));
        }

        [NUnit.Framework.Test]
        public virtual void CreateXObjectWithInvalidColorTest() {
            AbstractLinearGradientBuilder gradientBuilder = new StrategyBasedLinearGradientBuilder();
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            PdfXObject pdfXObject = AbstractRenderer.CreateXObject(gradientBuilder, new Rectangle(0, 0, 20, 20), pdfDocument
                );
            NUnit.Framework.Assert.IsNull(pdfXObject.GetPdfObject().Get(PdfName.Resources));
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImageTest() {
            AbstractRenderer renderer = new _DivRenderer_122(new Div());
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            int[] counter = new int[] { 0 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_132(counter, bytes, document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetImage(new _PdfImageXObject_149
                (ImageDataFactory.CreateRawImage(bytes))).Build());
            renderer.DrawBackground(context);
            NUnit.Framework.Assert.AreEqual(50, counter[0]);
        }

        private sealed class _DivRenderer_122 : DivRenderer {
            public _DivRenderer_122(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(99.0f, 49.0f);
            }
        }

        private sealed class _PdfCanvas_132 : PdfCanvas {
            public _PdfCanvas_132(int[] counter, byte[] bytes, PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.counter = counter;
                this.bytes = bytes;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                ++counter[0];
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(bytes));
                return null;
            }

            private readonly int[] counter;

            private readonly byte[] bytes;
        }

        private sealed class _PdfImageXObject_149 : PdfImageXObject {
            public _PdfImageXObject_149(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImageWithNoRepeatXTest() {
            AbstractRenderer renderer = new _DivRenderer_166(new Div());
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            int[] counter = new int[] { 0 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_176(counter, bytes, document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetImage(new _PdfImageXObject_193
                (ImageDataFactory.CreateRawImage(bytes))).SetBackgroundRepeat(new BackgroundRepeat(BackgroundRepeat.BackgroundRepeatValue
                .NO_REPEAT, BackgroundRepeat.BackgroundRepeatValue.REPEAT)).Build());
            renderer.DrawBackground(context);
            NUnit.Framework.Assert.AreEqual(5, counter[0]);
        }

        private sealed class _DivRenderer_166 : DivRenderer {
            public _DivRenderer_166(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 49.0f);
            }
        }

        private sealed class _PdfCanvas_176 : PdfCanvas {
            public _PdfCanvas_176(int[] counter, byte[] bytes, PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.counter = counter;
                this.bytes = bytes;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                ++counter[0];
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(bytes));
                return null;
            }

            private readonly int[] counter;

            private readonly byte[] bytes;
        }

        private sealed class _PdfImageXObject_193 : PdfImageXObject {
            public _PdfImageXObject_193(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImageWithNoRepeatYTest() {
            AbstractRenderer renderer = new _DivRenderer_210(new Div());
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            int[] counter = new int[] { 0 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_220(counter, bytes, document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetImage(new _PdfImageXObject_237
                (ImageDataFactory.CreateRawImage(bytes))).SetBackgroundRepeat(new BackgroundRepeat(BackgroundRepeat.BackgroundRepeatValue
                .REPEAT, BackgroundRepeat.BackgroundRepeatValue.NO_REPEAT)).Build());
            renderer.DrawBackground(context);
            NUnit.Framework.Assert.AreEqual(10, counter[0]);
        }

        private sealed class _DivRenderer_210 : DivRenderer {
            public _DivRenderer_210(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(99.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_220 : PdfCanvas {
            public _PdfCanvas_220(int[] counter, byte[] bytes, PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.counter = counter;
                this.bytes = bytes;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                ++counter[0];
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(bytes));
                return null;
            }

            private readonly int[] counter;

            private readonly byte[] bytes;
        }

        private sealed class _PdfImageXObject_237 : PdfImageXObject {
            public _PdfImageXObject_237(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImageWithNoRepeatTest() {
            AbstractRenderer renderer = new _DivRenderer_254(new Div());
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            int[] counter = new int[] { 0 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_264(counter, bytes, document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetImage(new _PdfImageXObject_281
                (ImageDataFactory.CreateRawImage(bytes))).SetBackgroundRepeat(new BackgroundRepeat(BackgroundRepeat.BackgroundRepeatValue
                .NO_REPEAT)).Build());
            renderer.DrawBackground(context);
            NUnit.Framework.Assert.AreEqual(1, counter[0]);
        }

        private sealed class _DivRenderer_254 : DivRenderer {
            public _DivRenderer_254(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_264 : PdfCanvas {
            public _PdfCanvas_264(int[] counter, byte[] bytes, PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.counter = counter;
                this.bytes = bytes;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                ++counter[0];
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(bytes));
                return null;
            }

            private readonly int[] counter;

            private readonly byte[] bytes;
        }

        private sealed class _PdfImageXObject_281 : PdfImageXObject {
            public _PdfImageXObject_281(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImageWithPositionTest() {
            AbstractRenderer renderer = new _DivRenderer_299(new Div());
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_308(bytes, document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetImage(new _PdfImageXObject_326
                (ImageDataFactory.CreateRawImage(bytes))).SetBackgroundPosition(new BackgroundPosition().SetXShift(new 
                UnitValue(UnitValue.PERCENT, 30))).Build());
            renderer.DrawBackground(context);
        }

        private sealed class _DivRenderer_299 : DivRenderer {
            public _DivRenderer_299(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_308 : PdfCanvas {
            public _PdfCanvas_308(byte[] bytes, PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.bytes = bytes;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(bytes));
                NUnit.Framework.Assert.AreEqual(27, (int)rect.GetX());
                NUnit.Framework.Assert.AreEqual(40, (int)rect.GetY());
                return null;
            }

            private readonly byte[] bytes;
        }

        private sealed class _PdfImageXObject_326 : PdfImageXObject {
            public _PdfImageXObject_326(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawGradientWithPositionTest() {
            AbstractRenderer renderer = new _DivRenderer_342(new Div());
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_350(document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetLinearGradientBuilder(new 
                StrategyBasedLinearGradientBuilder().AddColorStop(new GradientColorStop(ColorConstants.RED.GetColorValue
                ())).AddColorStop(new GradientColorStop(ColorConstants.GREEN.GetColorValue()))).SetBackgroundPosition(
                new BackgroundPosition().SetPositionX(BackgroundPosition.PositionX.RIGHT).SetPositionY(BackgroundPosition.PositionY
                .BOTTOM).SetYShift(UnitValue.CreatePointValue(100)).SetXShift(UnitValue.CreatePointValue(30))).Build()
                );
            renderer.DrawBackground(context);
        }

        private sealed class _DivRenderer_342 : DivRenderer {
            public _DivRenderer_342(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_350 : PdfCanvas {
            public _PdfCanvas_350(PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObject(PdfXObject xObject, Rectangle rect) {
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfFormXObject);
                NUnit.Framework.Assert.AreEqual(-30, (int)rect.GetX());
                NUnit.Framework.Assert.AreEqual(100, (int)rect.GetY());
                return null;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawGradientWithPercentagePositionTest() {
            AbstractRenderer renderer = new _DivRenderer_374(new Div());
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_382(document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, new BackgroundImage.Builder().SetLinearGradientBuilder(new 
                StrategyBasedLinearGradientBuilder().AddColorStop(new GradientColorStop(ColorConstants.RED.GetColorValue
                ())).AddColorStop(new GradientColorStop(ColorConstants.GREEN.GetColorValue()))).SetBackgroundPosition(
                new BackgroundPosition().SetPositionX(BackgroundPosition.PositionX.RIGHT).SetPositionY(BackgroundPosition.PositionY
                .BOTTOM).SetYShift(UnitValue.CreatePercentValue(70)).SetXShift(UnitValue.CreatePercentValue(33))).Build
                ());
            renderer.DrawBackground(context);
        }

        private sealed class _DivRenderer_374 : DivRenderer {
            public _DivRenderer_374(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_382 : PdfCanvas {
            public _PdfCanvas_382(PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObject(PdfXObject xObject, Rectangle rect) {
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfFormXObject);
                NUnit.Framework.Assert.AreEqual(0, (int)rect.GetX());
                NUnit.Framework.Assert.AreEqual(0, (int)rect.GetY());
                return null;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImagesTest() {
            AbstractRenderer renderer = new _DivRenderer_406(new Div());
            IList<byte[]> listBytes = JavaUtil.ArraysAsList(new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 }
                , new byte[] { 4, 15, 41, 23, 3, 2, 7, 14, 55, 27, 46, 12, 14, 14, 7, 7, 24, 25 });
            int[] counter = new int[] { 0 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_418(listBytes, counter, document, 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, JavaUtil.ArraysAsList((BackgroundImage)new BackgroundImage.Builder
                ().SetImage(new _PdfImageXObject_434(ImageDataFactory.CreateRawImage(listBytes[1]))).Build(), (BackgroundImage
                )new BackgroundImage.Builder().SetImage(new _PdfImageXObject_445(ImageDataFactory.CreateRawImage(listBytes
                [0]))).Build()));
            renderer.DrawBackground(context);
            NUnit.Framework.Assert.AreEqual(listBytes.Count, counter[0]);
        }

        private sealed class _DivRenderer_406 : DivRenderer {
            public _DivRenderer_406(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_418 : PdfCanvas {
            public _PdfCanvas_418(IList<byte[]> listBytes, int[] counter, PdfDocument baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.listBytes = listBytes;
                this.counter = counter;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(listBytes[counter[0]++]));
                return null;
            }

            private readonly IList<byte[]> listBytes;

            private readonly int[] counter;
        }

        private sealed class _PdfImageXObject_434 : PdfImageXObject {
            public _PdfImageXObject_434(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        private sealed class _PdfImageXObject_445 : PdfImageXObject {
            public _PdfImageXObject_445(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 10.0f;
            }

            public override float GetHeight() {
                return 10.0f;
            }
        }

        [NUnit.Framework.Test]
        public virtual void DrawBackgroundImagesWithPositionsTest() {
            AbstractRenderer renderer = new _DivRenderer_462(new Div());
            IList<byte[]> listBytes = JavaUtil.ArraysAsList(new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 }
                , new byte[] { 4, 15, 41, 23, 3, 2, 7, 14, 55, 27, 46, 12, 14, 14, 7, 7, 24, 25 });
            float widthHeight = 10.0f;
            IList<Rectangle> listRectangles = JavaUtil.ArraysAsList(new Rectangle(81, 20, widthHeight, widthHeight), new 
                Rectangle(0, 40, widthHeight, widthHeight));
            int[] counter = new int[] { 0 };
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream()));
            document.AddNewPage();
            DrawContext context = new DrawContext(document, new _PdfCanvas_479(listBytes, counter, listRectangles, document
                , 1));
            renderer.SetProperty(Property.BACKGROUND_IMAGE, JavaUtil.ArraysAsList((BackgroundImage)new BackgroundImage.Builder
                ().SetImage(new _PdfImageXObject_497(widthHeight, ImageDataFactory.CreateRawImage(listBytes[1]))).Build
                (), (BackgroundImage)new BackgroundImage.Builder().SetImage(new _PdfImageXObject_508(widthHeight, ImageDataFactory
                .CreateRawImage(listBytes[0]))).SetBackgroundPosition(new BackgroundPosition().SetPositionX(BackgroundPosition.PositionX
                .RIGHT).SetPositionY(BackgroundPosition.PositionY.CENTER).SetXShift(new UnitValue(UnitValue.PERCENT, 10
                ))).Build()));
            renderer.DrawBackground(context);
            NUnit.Framework.Assert.AreEqual(listBytes.Count, counter[0]);
        }

        private sealed class _DivRenderer_462 : DivRenderer {
            public _DivRenderer_462(Div baseArg1)
                : base(baseArg1) {
            }

            public override Rectangle GetOccupiedAreaBBox() {
                return new Rectangle(100.0f, 50.0f);
            }
        }

        private sealed class _PdfCanvas_479 : PdfCanvas {
            public _PdfCanvas_479(IList<byte[]> listBytes, int[] counter, IList<Rectangle> listRectangles, PdfDocument
                 baseArg1, int baseArg2)
                : base(baseArg1, baseArg2) {
                this.listBytes = listBytes;
                this.counter = counter;
                this.listRectangles = listRectangles;
                this.@object = null;
            }

            internal PdfXObject @object;

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                if (this.@object == xObject) {
                    return null;
                }
                this.@object = xObject;
                NUnit.Framework.Assert.IsTrue(xObject is PdfImageXObject);
                NUnit.Framework.Assert.AreEqual(JavaUtil.ArraysToString(((PdfImageXObject)xObject).GetImageBytes(false)), 
                    JavaUtil.ArraysToString(listBytes[counter[0]]));
                NUnit.Framework.Assert.AreEqual((int)listRectangles[counter[0]].GetX(), (int)rect.GetX());
                NUnit.Framework.Assert.AreEqual((int)listRectangles[counter[0]++].GetY(), (int)rect.GetY());
                return null;
            }

            private readonly IList<byte[]> listBytes;

            private readonly int[] counter;

            private readonly IList<Rectangle> listRectangles;
        }

        private sealed class _PdfImageXObject_497 : PdfImageXObject {
            public _PdfImageXObject_497(float widthHeight, ImageData baseArg1)
                : base(baseArg1) {
                this.widthHeight = widthHeight;
            }

            public override float GetWidth() {
                return widthHeight;
            }

            public override float GetHeight() {
                return widthHeight;
            }

            private readonly float widthHeight;
        }

        private sealed class _PdfImageXObject_508 : PdfImageXObject {
            public _PdfImageXObject_508(float widthHeight, ImageData baseArg1)
                : base(baseArg1) {
                this.widthHeight = widthHeight;
            }

            public override float GetWidth() {
                return widthHeight;
            }

            public override float GetHeight() {
                return widthHeight;
            }

            private readonly float widthHeight;
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundColorClipTest() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            PdfCanvas pdfCanvas = new _PdfCanvas_527(pdfDocument.AddNewPage());
            DrawContext drawContext = new DrawContext(pdfDocument, pdfCanvas);
            AbstractRenderer renderer = new DivRenderer(new Div().SetPadding(20).SetBorder(new DashedBorder(10)));
            renderer.occupiedArea = new LayoutArea(1, new Rectangle(100f, 200f, 300f, 400f));
            renderer.SetProperty(Property.BACKGROUND, new Background(new DeviceRgb(), 1, BackgroundBox.CONTENT_BOX));
            renderer.DrawBackground(drawContext);
        }

        private sealed class _PdfCanvas_527 : PdfCanvas {
            public _PdfCanvas_527(PdfPage baseArg1)
                : base(baseArg1) {
            }

            public override PdfCanvas Rectangle(double x, double y, double width, double height) {
                NUnit.Framework.Assert.AreEqual(130.0, x, 0);
                NUnit.Framework.Assert.AreEqual(230.0, y, 0);
                NUnit.Framework.Assert.AreEqual(240.0, width, 0);
                NUnit.Framework.Assert.AreEqual(340.0, height, 0);
                return this;
            }
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundImageClipOriginNoRepeatTest() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            PdfXObject rawImage = new _PdfImageXObject_548(ImageDataFactory.CreateRawImage(bytes));
            PdfCanvas pdfCanvas = new _PdfCanvas_559(rawImage, pdfDocument.AddNewPage());
            DrawContext drawContext = new DrawContext(pdfDocument, pdfCanvas);
            AbstractRenderer renderer = new DivRenderer(new Div().SetPadding(20).SetBorder(new DashedBorder(10)));
            renderer.occupiedArea = new LayoutArea(1, new Rectangle(100f, 200f, 300f, 400f));
            BackgroundImage backgroundImage = new BackgroundImage.Builder().SetImage(rawImage).SetBackgroundRepeat(new 
                BackgroundRepeat(BackgroundRepeat.BackgroundRepeatValue.NO_REPEAT)).SetBackgroundClip(BackgroundBox.CONTENT_BOX
                ).SetBackgroundOrigin(BackgroundBox.BORDER_BOX).Build();
            renderer.SetProperty(Property.BACKGROUND_IMAGE, backgroundImage);
            renderer.DrawBackground(drawContext);
        }

        private sealed class _PdfImageXObject_548 : PdfImageXObject {
            public _PdfImageXObject_548(ImageData baseArg1)
                : base(baseArg1) {
            }

            public override float GetWidth() {
                return 50f;
            }

            public override float GetHeight() {
                return 50f;
            }
        }

        private sealed class _PdfCanvas_559 : PdfCanvas {
            public _PdfCanvas_559(PdfXObject rawImage, PdfPage baseArg1)
                : base(baseArg1) {
                this.rawImage = rawImage;
            }

            public override PdfCanvas Rectangle(double x, double y, double width, double height) {
                NUnit.Framework.Assert.AreEqual(130.0, x, 0);
                NUnit.Framework.Assert.AreEqual(230.0, y, 0);
                NUnit.Framework.Assert.AreEqual(240.0, width, 0);
                NUnit.Framework.Assert.AreEqual(340.0, height, 0);
                return this;
            }

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                NUnit.Framework.Assert.AreEqual(rawImage, xObject);
                NUnit.Framework.Assert.AreEqual(100f, rect.GetX(), 0);
                NUnit.Framework.Assert.AreEqual(550f, rect.GetY(), 0);
                NUnit.Framework.Assert.AreEqual(50f, rect.GetWidth(), 0);
                NUnit.Framework.Assert.AreEqual(50f, rect.GetHeight(), 0);
                return this;
            }

            private readonly PdfXObject rawImage;
        }

        [NUnit.Framework.Test]
        public virtual void BackgroundLinearGradientClipOriginNoRepeatTest() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            byte[] bytes = new byte[] { 54, 25, 47, 15, 2, 2, 2, 44, 55, 77, 86, 24 };
            PdfCanvas pdfCanvas = new _PdfCanvas_593(pdfDocument.AddNewPage());
            DrawContext drawContext = new DrawContext(pdfDocument, pdfCanvas);
            AbstractRenderer renderer = new DivRenderer(new Div().SetPadding(20).SetBorder(new DashedBorder(10)));
            renderer.occupiedArea = new LayoutArea(1, new Rectangle(100f, 200f, 300f, 400f));
            Rectangle targetBoundingBox = new Rectangle(50f, 150f, 300f, 300f);
            AbstractLinearGradientBuilder gradientBuilder = new LinearGradientBuilder().SetGradientVector(targetBoundingBox
                .GetLeft() + 100f, targetBoundingBox.GetBottom() + 100f, targetBoundingBox.GetRight() - 100f, targetBoundingBox
                .GetTop() - 100f).SetSpreadMethod(GradientSpreadMethod.PAD).AddColorStop(new GradientColorStop(ColorConstants
                .RED.GetColorValue(), 0d, GradientColorStop.OffsetType.RELATIVE)).AddColorStop(new GradientColorStop(ColorConstants
                .BLUE.GetColorValue(), 1d, GradientColorStop.OffsetType.RELATIVE));
            BackgroundImage backgroundImage = new BackgroundImage.Builder().SetLinearGradientBuilder(gradientBuilder).
                SetBackgroundRepeat(new BackgroundRepeat(BackgroundRepeat.BackgroundRepeatValue.NO_REPEAT)).SetBackgroundClip
                (BackgroundBox.CONTENT_BOX).SetBackgroundOrigin(BackgroundBox.BORDER_BOX).Build();
            renderer.SetProperty(Property.BACKGROUND_IMAGE, backgroundImage);
            renderer.DrawBackground(drawContext);
        }

        private sealed class _PdfCanvas_593 : PdfCanvas {
            public _PdfCanvas_593(PdfPage baseArg1)
                : base(baseArg1) {
            }

            public override PdfCanvas Rectangle(double x, double y, double width, double height) {
                NUnit.Framework.Assert.AreEqual(130.0, x, 0);
                NUnit.Framework.Assert.AreEqual(230.0, y, 0);
                NUnit.Framework.Assert.AreEqual(240.0, width, 0);
                NUnit.Framework.Assert.AreEqual(340.0, height, 0);
                return this;
            }

            public override PdfCanvas AddXObjectFittedIntoRectangle(PdfXObject xObject, Rectangle rect) {
                NUnit.Framework.Assert.AreEqual(100f, rect.GetX(), 0);
                NUnit.Framework.Assert.AreEqual(200f, rect.GetY(), 0);
                NUnit.Framework.Assert.AreEqual(300f, rect.GetWidth(), 0);
                NUnit.Framework.Assert.AreEqual(400f, rect.GetHeight(), 0);
                return this;
            }
        }

        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.PAGE_WAS_FLUSHED_ACTION_WILL_NOT_BE_PERFORMED)]
        public virtual void ApplyLinkAnnotationFlushedPageTest() {
            AbstractRenderer abstractRenderer = new DivRenderer(new Div());
            abstractRenderer.occupiedArea = new LayoutArea(1, new Rectangle(100, 100));
            abstractRenderer.SetProperty(Property.LINK_ANNOTATION, new PdfLinkAnnotation(new Rectangle(0, 0, 0, 0)));
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
            pdfDocument.AddNewPage();
            pdfDocument.GetPage(1).Flush();
            abstractRenderer.ApplyLinkAnnotation(pdfDocument);
            // This test checks that there is log message and there is no NPE so assertions are not required
            NUnit.Framework.Assert.IsTrue(true);
        }
    }
}
