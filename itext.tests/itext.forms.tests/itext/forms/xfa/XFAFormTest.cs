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
using System.Xml;
using System.Xml.Linq;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Test;
using NUnit.Framework;

namespace iText.Forms.Xfa {
    public class XFAFormTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/forms/xfa/XFAFormTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/forms/xfa/XFAFormTest/";

        public static readonly String XML = sourceFolder + "xfa.xml";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void CreateEmptyXFAFormTest01() {
            String outFileName = destinationFolder + "createEmptyXFAFormTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_createEmptyXFAFormTest01.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(outFileName));
            XfaForm xfa = new XfaForm(doc);
            XfaForm.SetXfaForm(xfa, doc);
            doc.AddNewPage();
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        [NUnit.Framework.Test]
        public virtual void CreateEmptyXFAFormTest02() {
            String outFileName = destinationFolder + "createEmptyXFAFormTest02.pdf";
            String cmpFileName = sourceFolder + "cmp_createEmptyXFAFormTest02.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(outFileName));
            XfaForm xfa = new XfaForm();
            XfaForm.SetXfaForm(xfa, doc);
            doc.AddNewPage();
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        [NUnit.Framework.Test]
        public virtual void CreateXFAFormTest() {
            String outFileName = destinationFolder + "createXFAFormTest.pdf";
            String cmpFileName = sourceFolder + "cmp_createXFAFormTest.pdf";
            PdfDocument doc = new PdfDocument(new PdfWriter(outFileName));
            XfaForm xfa = new XfaForm(new FileStream(XML, FileMode.Open, FileAccess.Read));
            xfa.Write(doc);
            doc.AddNewPage();
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        [NUnit.Framework.Test]
        public virtual void ReadXFAFormTest() {
            String inFileName = sourceFolder + "formTemplate.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFileName));
            Assert.DoesNotThrow(() => PdfAcroForm.GetAcroForm(pdfDocument, true));
        }

        [NUnit.Framework.Test]
        public virtual void FindFieldName() {
            String inFileName = sourceFolder + "TextField1.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFileName));
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(pdfDocument, true);
            XfaForm xfaForm = acroForm.GetXfaForm();
            xfaForm.FindFieldName("TextField1");
            String secondRun = xfaForm.FindFieldName("TextField1");
            NUnit.Framework.Assert.IsNotNull(secondRun);
        }

        [NUnit.Framework.Test]
        public virtual void FindFieldNameWithoutDataSet() {
            String inFileName = sourceFolder + "TextField1_empty.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFileName));
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(pdfDocument, true);
            XfaForm xfaForm = acroForm.GetXfaForm();
            String name = xfaForm.FindFieldName("TextField1");
            NUnit.Framework.Assert.IsNull(name);
        }

        [NUnit.Framework.Test]
        public virtual void ExtractXFADataTest() {
            String src = sourceFolder + "xfaFormWithDataSet.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(src));
            XfaForm xfa = new XfaForm(pdfDocument);
            XElement node = (XElement) xfa.FindDatasetsNode("Number1");
            NUnit.Framework.Assert.IsNotNull(node);
            NUnit.Framework.Assert.AreEqual("Number1", node.Name.LocalName);
        }

        [NUnit.Framework.Test]
        public virtual void GetXfaFieldValue() {
            String inFileName = sourceFolder + "TextField1.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFileName));
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(pdfDocument, true);
            XfaForm xfaForm = acroForm.GetXfaForm();
            string value = xfaForm.GetXfaFieldValue("TextField1");
            NUnit.Framework.Assert.AreEqual("Test", value);
        }
    }
}
