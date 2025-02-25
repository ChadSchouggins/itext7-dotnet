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
using iText.Kernel.Pdf;
using iText.Test;

namespace iText.Signatures {
    public class SignatureUtilTest : ExtendedITextTest {
        private static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/SignatureUtilTest/";

        [NUnit.Framework.Test]
        public virtual void GetSignaturesTest01() {
            String inPdf = sourceFolder + "simpleSignature.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            IList<String> signatureNames = signatureUtil.GetSignatureNames();
            NUnit.Framework.Assert.AreEqual(1, signatureNames.Count);
            NUnit.Framework.Assert.AreEqual("Signature1", signatureNames[0]);
        }

        [NUnit.Framework.Test]
        public virtual void GetSignaturesTest02() {
            String inPdf = sourceFolder + "simpleDocument.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            IList<String> signatureNames = signatureUtil.GetSignatureNames();
            NUnit.Framework.Assert.AreEqual(0, signatureNames.Count);
        }

        [NUnit.Framework.Test]
        public virtual void FirstBytesNotCoveredTest01() {
            String inPdf = sourceFolder + "firstBytesNotCoveredTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsFalse(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void LastBytesNotCoveredTest01() {
            String inPdf = sourceFolder + "lastBytesNotCoveredTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsFalse(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void LastBytesNotCoveredTest02() {
            String inPdf = sourceFolder + "lastBytesNotCoveredTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsFalse(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void BytesAreNotCoveredTest01() {
            String inPdf = sourceFolder + "bytesAreNotCoveredTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsFalse(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void BytesAreCoveredTest01() {
            String inPdf = sourceFolder + "bytesAreCoveredTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsTrue(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void BytesAreCoveredTest02() {
            String inPdf = sourceFolder + "bytesAreCoveredTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsTrue(signatureUtil.SignatureCoversWholeDocument("sig"));
        }

        [NUnit.Framework.Test]
        public virtual void TwoContentsTest01() {
            String inPdf = sourceFolder + "twoContentsTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsTrue(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void SpacesBeforeContentsTest01() {
            String inPdf = sourceFolder + "spacesBeforeContentsTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsFalse(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void SpacesBeforeContentsTest02() {
            String inPdf = sourceFolder + "spacesBeforeContentsTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsTrue(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }

        [NUnit.Framework.Test]
        public virtual void NotIndirectSigDictionaryTest() {
            String inPdf = sourceFolder + "notIndirectSigDictionaryTest.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inPdf));
            SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
            NUnit.Framework.Assert.IsTrue(signatureUtil.SignatureCoversWholeDocument("Signature1"));
        }
    }
}
