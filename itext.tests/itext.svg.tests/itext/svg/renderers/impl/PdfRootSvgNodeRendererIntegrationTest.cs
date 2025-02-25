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
using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Svg.Converter;
using iText.Svg.Exceptions;
using iText.Svg.Renderers;

namespace iText.Svg.Renderers.Impl {
    public class PdfRootSvgNodeRendererIntegrationTest : SvgIntegrationTest {
        [NUnit.Framework.Test]
        public virtual void CalculateOutermostViewportTest() {
            Rectangle expected = new Rectangle(0, 0, 600, 600);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream(), new WriterProperties().SetCompressionLevel
                (0)));
            document.AddNewPage();
            PdfFormXObject pdfForm = new PdfFormXObject(expected);
            PdfCanvas canvas = new PdfCanvas(pdfForm, document);
            context.PushCanvas(canvas);
            SvgTagSvgNodeRenderer renderer = new SvgTagSvgNodeRenderer();
            PdfRootSvgNodeRenderer root = new PdfRootSvgNodeRenderer(renderer);
            Rectangle actual = root.CalculateViewPort(context);
            NUnit.Framework.Assert.IsTrue(expected.EqualsWithEpsilon(actual));
        }

        [NUnit.Framework.Test]
        public virtual void CalculateOutermostViewportWithDifferentXYTest() {
            Rectangle expected = new Rectangle(10, 20, 600, 600);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream(), new WriterProperties().SetCompressionLevel
                (0)));
            document.AddNewPage();
            PdfFormXObject pdfForm = new PdfFormXObject(expected);
            PdfCanvas canvas = new PdfCanvas(pdfForm, document);
            context.PushCanvas(canvas);
            SvgTagSvgNodeRenderer renderer = new SvgTagSvgNodeRenderer();
            PdfRootSvgNodeRenderer root = new PdfRootSvgNodeRenderer(renderer);
            Rectangle actual = root.CalculateViewPort(context);
            NUnit.Framework.Assert.IsTrue(expected.EqualsWithEpsilon(actual));
        }

        [NUnit.Framework.Test]
        public virtual void CalculateNestedViewportDifferentFromParentTest() {
            Rectangle expected = new Rectangle(0, 0, 500, 500);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream(), new WriterProperties().SetCompressionLevel
                (0)));
            document.AddNewPage();
            PdfFormXObject pdfForm = new PdfFormXObject(expected);
            PdfCanvas canvas = new PdfCanvas(pdfForm, document);
            context.PushCanvas(canvas);
            context.AddViewPort(expected);
            SvgTagSvgNodeRenderer parent = new SvgTagSvgNodeRenderer();
            SvgTagSvgNodeRenderer renderer = new SvgTagSvgNodeRenderer();
            PdfRootSvgNodeRenderer root = new PdfRootSvgNodeRenderer(parent);
            IDictionary<String, String> styles = new Dictionary<String, String>();
            styles.Put("width", "500");
            styles.Put("height", "500");
            renderer.SetAttributesAndStyles(styles);
            renderer.SetParent(parent);
            Rectangle actual = root.CalculateViewPort(context);
            NUnit.Framework.Assert.IsTrue(expected.EqualsWithEpsilon(actual));
        }

        [NUnit.Framework.Test]
        public virtual void NoBoundingBoxOnXObjectTest() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream(), new WriterProperties().SetCompressionLevel
                    (0)));
                document.AddNewPage();
                ISvgNodeRenderer processed = SvgConverter.Process(SvgConverter.Parse("<svg />"), null).GetRootRenderer();
                PdfRootSvgNodeRenderer root = new PdfRootSvgNodeRenderer(processed);
                PdfFormXObject pdfForm = new PdfFormXObject(new PdfStream());
                PdfCanvas canvas = new PdfCanvas(pdfForm, document);
                SvgDrawContext context = new SvgDrawContext(null, null);
                context.PushCanvas(canvas);
                root.Draw(context);
            }
            , NUnit.Framework.Throws.InstanceOf<SvgProcessingException>().With.Message.EqualTo(SvgLogMessageConstant.ROOT_SVG_NO_BBOX))
;
        }

        [NUnit.Framework.Test]
        public virtual void CalculateOutermostTransformation() {
            AffineTransform expected = new AffineTransform(1d, 0d, 0d, -1d, 0d, 600d);
            SvgDrawContext context = new SvgDrawContext(null, null);
            PdfDocument document = new PdfDocument(new PdfWriter(new MemoryStream(), new WriterProperties().SetCompressionLevel
                (0)));
            document.AddNewPage();
            PdfFormXObject pdfForm = new PdfFormXObject(new Rectangle(0, 0, 600, 600));
            PdfCanvas canvas = new PdfCanvas(pdfForm, document);
            context.PushCanvas(canvas);
            SvgTagSvgNodeRenderer renderer = new SvgTagSvgNodeRenderer();
            PdfRootSvgNodeRenderer root = new PdfRootSvgNodeRenderer(renderer);
            context.AddViewPort(root.CalculateViewPort(context));
            AffineTransform actual = root.CalculateTransformation(context);
            NUnit.Framework.Assert.AreEqual(expected, actual);
        }
    }
}
