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
using System.IO;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Utils;
using iText.Test;

namespace iText.Pdfa {
    public class PdfA2ActionCheckTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfa/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/pdfa/PdfA2ActionCheckTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck01() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Launch);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.Launch.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck02() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Hide);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.Hide.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck03() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Sound);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.Sound.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck04() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Movie);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.Movie.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck05() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.ResetForm);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.ResetForm.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck06() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.ImportData);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.ImportData.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck07() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.JavaScript);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.JavaScript.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck08() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Named);
                openActions.Put(PdfName.N, new PdfName("CustomName"));
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException.NAMED_ACTION_TYPE_0_IS_NOT_ALLOWED, "CustomName")))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck09() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.SetOCGState);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.SetOCGState.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck10() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Rendition);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.Rendition.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck11() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.Trans);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.Trans.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck12() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                PdfDictionary openActions = new PdfDictionary();
                openActions.Put(PdfName.S, PdfName.GoTo3DView);
                doc.GetCatalog().Put(PdfName.OpenAction, openActions);
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(MessageFormatUtil.Format(PdfAConformanceException._0_ACTIONS_ARE_NOT_ALLOWED, PdfName.GoTo3DView.GetValue())))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck13() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                PdfPage page = doc.AddNewPage();
                page.SetAdditionalAction(PdfName.C, PdfAction.CreateJavaScript("js"));
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.THE_PAGE_DICTIONARY_SHALL_NOT_CONTAIN_AA_ENTRY))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck14() {
            NUnit.Framework.Assert.That(() =>  {
                PdfWriter writer = new PdfWriter(new ByteArrayOutputStream());
                Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
                PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                    , "http://www.color.org", "sRGB IEC61966-2.1", @is));
                doc.AddNewPage();
                doc.GetCatalog().SetAdditionalAction(PdfName.C, PdfAction.CreateJavaScript("js"));
                doc.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.A_CATALOG_DICTIONARY_SHALL_NOT_CONTAIN_AA_ENTRY))
;
        }

        [NUnit.Framework.Test]
        public virtual void ActionCheck15() {
            String outPdf = destinationFolder + "pdfA2b_actionCheck15.pdf";
            String cmpPdf = sourceFolder + "cmp/PdfA2ActionCheckTest/cmp_pdfA2b_actionCheck15.pdf";
            PdfWriter writer = new PdfWriter(outPdf);
            Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
            PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, new PdfOutputIntent("Custom", ""
                , "http://www.color.org", "sRGB IEC61966-2.1", @is));
            doc.GetOutlines(true);
            PdfOutline @out = doc.GetOutlines(false);
            @out.AddOutline("New").AddAction(PdfAction.CreateGoTo("TestDest"));
            doc.AddNewPage();
            doc.Close();
            String result = new CompareTool().CompareByContent(outPdf, cmpPdf, destinationFolder, "diff_");
            if (result != null) {
                NUnit.Framework.Assert.Fail(result);
            }
        }
    }
}
