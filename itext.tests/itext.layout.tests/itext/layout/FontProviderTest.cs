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
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;
using iText.Test;

namespace iText.Layout {
    public class FontProviderTest : ExtendedITextTest {
        private class PdfFontProvider : FontProvider {
            private IList<FontInfo> pdfFontInfos = new List<FontInfo>();

            public virtual void AddPdfFont(PdfFont font, String alias) {
                FontInfo fontInfo = FontInfo.Create(font.GetFontProgram(), null, alias);
                // stored FontInfo will be used in FontSelector collection.
                pdfFontInfos.Add(fontInfo);
                // first of all FOntProvider search PdfFont in pdfFonts.
                pdfFonts.Put(fontInfo, font);
            }

            protected internal override FontSelector CreateFontSelector(ICollection<FontInfo> fonts, IList<String> fontFamilies
                , FontCharacteristics fc) {
                IList<FontInfo> newFonts = new List<FontInfo>(fonts);
                newFonts.AddAll(pdfFontInfos);
                return base.CreateFontSelector(newFonts, fontFamilies, fc);
            }
        }

        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/FontProviderTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/FontProviderTest/";

        public static readonly String fontsFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/fonts/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void StandardAndType3Fonts() {
            String fileName = "taggedDocumentWithType3Font";
            String srcFileName = sourceFolder + "src_" + fileName + ".pdf";
            String outFileName = destinationFolder + fileName + ".pdf";
            String cmpFileName = sourceFolder + "cmp_" + fileName + ".pdf";
            FontProviderTest.PdfFontProvider sel = new FontProviderTest.PdfFontProvider();
            sel.AddStandardPdfFonts();
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(new FileStream(srcFileName, FileMode.Open, FileAccess.Read
                )), new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            PdfType3Font pdfType3Font = (PdfType3Font)PdfFontFactory.CreateFont((PdfDictionary)pdfDoc.GetPdfObject(5));
            sel.AddPdfFont(pdfType3Font, "CustomFont");
            Document doc = new Document(pdfDoc);
            doc.SetFontProvider(sel);
            Paragraph paragraph = new Paragraph("Next paragraph contains a triangle, actually Type 3 Font");
            paragraph.SetProperty(Property.FONT, new String[] { StandardFontFamilies.TIMES });
            doc.Add(paragraph);
            paragraph = new Paragraph("A");
            paragraph.SetFontFamily("CustomFont");
            doc.Add(paragraph);
            paragraph = new Paragraph("Next paragraph");
            paragraph.SetProperty(Property.FONT, new String[] { StandardFonts.COURIER });
            doc.Add(paragraph);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff" + fileName));
        }

        [NUnit.Framework.Test]
        public virtual void CustomFontProvider() {
            String fileName = "customFontProvider.pdf";
            String outFileName = destinationFolder + fileName + ".pdf";
            String cmpFileName = sourceFolder + "cmp_" + fileName + ".pdf";
            FontProvider fontProvider = new FontProvider();
            // TODO DEVSIX-2119 Update if necessary
            fontProvider.GetFontSet().AddFont(StandardFonts.TIMES_ROMAN, null, "times");
            fontProvider.GetFontSet().AddFont(StandardFonts.HELVETICA);
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            Document doc = new Document(pdfDoc);
            doc.SetFontProvider(fontProvider);
            Paragraph paragraph1 = new Paragraph("Default Helvetica should be selected.");
            doc.Add(paragraph1);
            Paragraph paragraph2 = new Paragraph("Default Helvetica should be selected.").SetFontFamily(StandardFonts.
                COURIER);
            doc.Add(paragraph2);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff" + fileName));
        }

        [NUnit.Framework.Test]
        public virtual void CustomFontProvider2() {
            String fileName = "customFontProvider2.pdf";
            String outFileName = destinationFolder + fileName + ".pdf";
            String cmpFileName = sourceFolder + "cmp_" + fileName + ".pdf";
            FontProvider fontProvider = new FontProvider();
            // bold font. shouldn't be selected
            // TODO DEVSIX-2119 Update if necessary
            fontProvider.GetFontSet().AddFont(StandardFonts.TIMES_BOLD, null, "times");
            // monospace font. shouldn't be selected
            fontProvider.GetFontSet().AddFont(StandardFonts.COURIER);
            fontProvider.GetFontSet().AddFont(sourceFolder + "../fonts/FreeSans.ttf", PdfEncodings.IDENTITY_H);
            // TODO DEVSIX-2119 Update if necessary
            fontProvider.GetFontSet().AddFont(StandardFonts.TIMES_ROMAN, null, "times");
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            Document doc = new Document(pdfDoc);
            doc.SetFontProvider(fontProvider);
            Paragraph paragraph = new Paragraph("There is no default font (Helvetica) inside the used FontProvider's instance. So the first font, that has been added, should be selected. Here it's FreeSans."
                ).SetFontFamily("ABRACADABRA_THERE_IS_NO_SUCH_FONT");
            doc.Add(paragraph);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff" + fileName));
        }

        [NUnit.Framework.Test]
        public virtual void FontProviderNotSetExceptionTest() {
            NUnit.Framework.Assert.That(() =>  {
                String fileName = "fontProviderNotSetExceptionTest.pdf";
                String outFileName = destinationFolder + fileName + ".pdf";
                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
                Document doc = new Document(pdfDoc);
                Paragraph paragraph = new Paragraph("Hello world!").SetFontFamily("ABRACADABRA_NO_FONT_PROVIDER_ANYWAY");
                doc.Add(paragraph);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo(PdfException.FontProviderNotSetFontFamilyNotResolved))
;
        }
    }
}
